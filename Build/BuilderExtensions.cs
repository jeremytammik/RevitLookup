using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

static class BuilderExtensions
{
    public static Project GetProject(this Solution solution, string projectName) =>
        solution.GetProject(projectName) ?? throw new NullReferenceException($"Cannon find project \"{projectName}\"");

    public static AbsolutePath GetBinDirectory(this Project project) => project.Directory / "bin";

    static AbsolutePath GetInstallerPath(this Project project, string configuration) => project.GetBinDirectory() / configuration / $"{project.Name}.exe";

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