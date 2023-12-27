using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Utilities;
using Serilog;
using Serilog.Events;

sealed partial class Build
{
    Target CreateInstaller => _ => _
        .DependsOn(Compile)
        .OnlyWhenStatic(() => IsLocalBuild || GitRepository.IsOnMasterBranch())
        .Executes(() =>
        {
            foreach (var (installer, project) in InstallersMap)
            {
                Log.Information("Project: {Name}", project.Name);

                var exePattern = $"*{installer.Name}.exe";
                var exeFile = Directory.EnumerateFiles(installer.Directory, exePattern, SearchOption.AllDirectories)
                    .FirstOrDefault()
                    .NotNull($"No installer file was found for the project: {installer.Name}");

                var directories = Directory.GetDirectories(project.Directory, "* Release *", SearchOption.AllDirectories);
                Assert.NotEmpty(directories, "No files were found to create an installer");

                foreach (var directory in directories)
                {
                    var process = new Process();
                    process.StartInfo.FileName = exeFile;
                    process.StartInfo.Arguments = directory.DoubleQuoteIfNeeded();
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    RedirectStream(process.StandardOutput, LogEventLevel.Information);
                    RedirectStream(process.StandardError, LogEventLevel.Error);

                    process.WaitForExit();
                    if (process.ExitCode != 0) throw new InvalidOperationException($"The installer creation failed with ExitCode {process.ExitCode}");
                }
            }
        });

    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    void RedirectStream(StreamReader reader, LogEventLevel eventLevel)
    {
        while (!reader.EndOfStream)
        {
            var value = reader.ReadLine();
            if (value is null) continue;

            var matches = StreamRegex.Matches(value);
            if (matches.Count > 0)
            {
                var parameters = matches
                    .Select(match => match.Value.Substring(1, match.Value.Length - 2))
                    .Cast<object>()
                    .ToArray();

                var line = StreamRegex.Replace(value, match => $"{{Parameter{match.Index}}}");
                Log.Write(eventLevel, line, parameters);
            }
            else
            {
                Log.Debug(value);
            }
        }
    }
}