using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Utilities;
using Serilog;
using Serilog.Events;

partial class Build
{
    Target CreateInstaller => _ => _
        .TriggeredBy(Compile)
        .OnlyWhenStatic(() => IsLocalBuild || GitRepository.IsOnMasterBranch())
        .Executes(() =>
        {
            foreach (var (installer, project) in InstallersMap)
            {
                Log.Information("Project: {Name}", project.Name);

                var exePattern = $"*{installer.Name}.exe";
                var exeFile = Directory.EnumerateFiles(installer.Directory, exePattern, SearchOption.AllDirectories).First();

                var directories = Directory.GetDirectories(project.Directory, "* Release *", SearchOption.AllDirectories);
                if (directories.Length == 0) throw new Exception("No files were found to create an installer");

                foreach (var directory in directories)
                {
                    var proc = new Process();
                    proc.StartInfo.FileName = exeFile;
                    proc.StartInfo.Arguments = directory.DoubleQuoteIfNeeded();
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();

                    RedirectStream(proc.StandardOutput, LogEventLevel.Information);
                    RedirectStream(proc.StandardError, LogEventLevel.Error);

                    proc.WaitForExit();
                    if (proc.ExitCode != 0) throw new Exception($"The installer creation failed with ExitCode {proc.ExitCode}");
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