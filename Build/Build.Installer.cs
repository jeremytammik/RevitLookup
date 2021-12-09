using System.Diagnostics;
using System.Text;
using Nuke.Common;
using Nuke.Common.Git;
using Serilog;

partial class Build
{
    Target CreateInstaller => _ => _
        .TriggeredBy(Compile)
        .Produces(ArtifactsDirectory / "*.msi")
        .OnlyWhenStatic(() => IsLocalBuild || GitRepository.IsOnMasterBranch())
        .Executes(() =>
        {
            var installerProject = BuilderExtensions.GetProject(Solution, InstallerProject);
            var buildDirectories = GetBuildDirectories();
            var configurations = GetConfigurations(InstallerConfiguration);

            var releasesDirectory = Solution.Directory / "Releases";
            var releasesInfos = new DirectoryInfo(releasesDirectory).EnumerateDirectories().Select(info => info.FullName).ToList();

            foreach (var directoryGroup in buildDirectories)
            {
                var directories = directoryGroup.ToList();
                var exeArguments = BuildExeArguments(directories.Select(info => info.FullName).Concat(releasesInfos).ToList());
                var exeFile = installerProject.GetExecutableFile(configurations, directories);
                if (string.IsNullOrEmpty(exeFile))
                {
                    Log.Warning("No installer executable was found for these packages:\n {Directories}", string.Join("\n", directories));
                    continue;
                }

                var proc = new Process();
                proc.StartInfo.FileName = exeFile;
                proc.StartInfo.Arguments = exeArguments;
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode != 0) throw new Exception("The installer creation failed.");
            }
        });

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