using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Git;
using Serilog;

partial class Build
{
    readonly Regex StreamRegex = new("'(.+?)'", RegexOptions.Compiled);

    Target CreateInstaller => _ => _
        .TriggeredBy(Compile)
        .OnlyWhenStatic(() => IsLocalBuild || GitRepository.IsOnMasterBranch())
        .Executes(() =>
        {
            var releasesDirectory = Solution.Directory / "Releases";
            var installerProject = BuilderExtensions.GetProject(Solution, InstallerProject);

            var buildDirectories = EnumerateBuildDirectories()
                .Concat(new DirectoryInfo(releasesDirectory).EnumerateDirectories())
                .Select(info => info.FullName)
                .OrderByDescending(info => info)
                .ToList();

            var exeFile = installerProject.GetExePath(BuildConfiguration);

            foreach (var buildDirectory in buildDirectories)
            {
                var proc = new Process();
                proc.StartInfo.FileName = exeFile;
                proc.StartInfo.Arguments = BuildExeArguments(new[] {buildDirectory});
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                while (!proc.StandardOutput.EndOfStream) ParseProcessOutput(proc.StandardOutput.ReadLine());
                proc.WaitForExit();
                if (proc.ExitCode != 0) throw new Exception("The installer creation failed.");
            }
        });

    void ParseProcessOutput([CanBeNull] string value)
    {
        if (value is null) return;
        var matches = StreamRegex.Matches(value);
        if (matches.Count > 0)
        {
            var parameters = matches.Select(match => match.Value
                    .Substring(1, match.Value.Length - 2))
                .Cast<object>()
                .ToArray();
            var line = StreamRegex.Replace(value, match => $"{{Parameter{match.Index}}}");
            Log.Information(line, parameters);
        }
        else
        {
            Log.Debug(value);
        }
    }

    static string BuildExeArguments(IReadOnlyList<string> args)
    {
        var argumentBuilder = new StringBuilder();
        for (var i = 0; i < args.Count; i++)
        {
            if (i > 0) argumentBuilder.Append(' ');
            var value = args[i];
            if (value.Contains(' ')) value = $"\"{value}\"";
            argumentBuilder.Append(value);
        }

        return argumentBuilder.ToString();
    }
}