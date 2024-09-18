using System.Diagnostics.CodeAnalysis;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

sealed partial class Build
{
    Target CreateInstaller => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var (installer, project) in InstallersMap)
            {
                Log.Information("Project: {Name}", project.Name);

                var exePattern = $"*{installer.Name}.exe";
                var exeFile = Directory.EnumerateFiles(installer.Directory, exePattern, SearchOption.AllDirectories)
                    .FirstOrDefault()
                    .NotNull($"No installer file was found for the project: {installer.Name}");

                var directories =
                    Directory.GetDirectories(project.Directory, "* Release *", SearchOption.AllDirectories);
                Assert.NotEmpty(directories, "No files were found to create an installer");

                var arguments = directories.Select(path => path.DoubleQuoteIfNeeded()).JoinSpace();
                var process =
                    ProcessTasks.StartProcess(exeFile, arguments, logInvocation: false, logger: InstallLogger);
                process.AssertZeroExitCode();
            }
        });

    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    void InstallLogger(OutputType outputType, string output)
    {
        if (outputType == OutputType.Err)
        {
            Log.Error(output);
            return;
        }

        var arguments = ArgumentsRegex.Matches(output);
        if (arguments.Count == 0)
        {
            Log.Debug(output);
            return;
        }

        var properties = arguments
            .Select(match => match.Value.Substring(1, match.Value.Length - 2))
            .Cast<object>()
            .ToArray();

        var messageTemplate = ArgumentsRegex.Replace(output, match => $"{{Property{match.Index}}}");
        Log.Information(messageTemplate, properties);
    }
}