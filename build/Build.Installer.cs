using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Nuke.Common;

partial class Build
{
    Target CreateInstaller => _ => _
        .TriggeredBy(Compile)
        .Produces(ArtifactsDirectory / "*.msi")
        .Executes(() =>
        {
            var installerProject = BuilderExtensions.GetProject(Solution, InstallerProject);
            var buildDirectories = GetBuildDirectories();
            var configurations = GetConfigurations(InstallerConfiguration);

            foreach (var directoryGroup in buildDirectories)
            {
                var directories = directoryGroup.ToList();
                var exeArguments = BuildExeArguments(directories.Select(info => info.FullName).ToList());
                var exeFile = installerProject.GetExecutableFile(configurations, directories);
                if (string.IsNullOrEmpty(exeFile))
                {
                    Logger.Warn($"No installer executable was found for these packages:\n {string.Join("\n", directories)}");
                    continue;
                }

                var proc = new Process();
                proc.StartInfo.FileName = exeFile;
                proc.StartInfo.Arguments = exeArguments;
                proc.Start();

                Logger.Normal($"Waiting {InstallerCreationTime} seconds to create installer on another thread");
                Thread.Sleep(TimeSpan.FromSeconds(InstallerCreationTime));
            }
        });

    string BuildExeArguments(IReadOnlyList<string> args)
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