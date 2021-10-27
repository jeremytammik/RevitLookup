using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

public static class BuilderExtensions
{
    public static Project GetProject(this Solution solution, string projectName) =>
        solution.GetProject(projectName) ?? throw new NullReferenceException($"Cannon find project \"{projectName}\"");

    public static AbsolutePath GetBinDirectory(this Project project) => project.Directory / "bin";

    public static AbsolutePath GetBundleDirectory(this Solution solution, AbsolutePath basePath) => basePath / $"{solution.Name}.bundle";

    static AbsolutePath GetInstallerPath(this Project project, string configuration)
    {
        var configurationDirectory = project.GetBinDirectory() / configuration;
        var netDirectory = Directory.GetDirectories(configurationDirectory).FirstOrDefault();
        if (netDirectory is null) throw new DirectoryNotFoundException($"Missing .Net subdirectories in: {configurationDirectory}");

        var directoryInfo = new DirectoryInfo(netDirectory);
        return configurationDirectory / directoryInfo.Name / $"{project.Name}.exe";
    }

    public static AbsolutePath GetExecutableFile(this Project project, IEnumerable<string> configurations, List<DirectoryInfo> directories)
    {
        var directory = directories[0].Name;
        var subConfigRegex = new Regex(@"\d+");
        foreach (var subCategory in configurations.Select(configuration => configuration.Replace(Build.InstallerConfiguration, "")))
            if (string.IsNullOrEmpty(subCategory))
            {
                if (!string.IsNullOrEmpty(subConfigRegex.Match(directory).Value))
                    return project.GetInstallerPath(Build.BuildConfiguration);
            }
            else
            {
                if (directory.EndsWith(subCategory))
                    return project.GetInstallerPath($"{Build.BuildConfiguration}{subCategory}");
            }

        return null;
    }
}