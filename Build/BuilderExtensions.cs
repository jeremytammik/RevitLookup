using System.Text.RegularExpressions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

static class BuilderExtensions
{
    public static Project GetProject(this Solution solution, string projectName) =>
        solution.GetProject(projectName) ?? throw new NullReferenceException($"Cannon find project \"{projectName}\"");

    public static AbsolutePath GetBinDirectory(this Project project) => project.Directory / "bin";

    static AbsolutePath GetExePath(this Project project, string configuration) => project.GetBinDirectory() / configuration / $"{project.Name}.exe";

    public static AbsolutePath GetExecutableFile(this Project project, IEnumerable<string> configurations, List<DirectoryInfo> directories)
    {
        var directory = directories[0].Name;
        var subConfigRegex = new Regex(@"R\d+$");
        foreach (var subCategory in configurations.Select(configuration => configuration.Replace(Build.InstallerConfiguration, "")))
            if (string.IsNullOrEmpty(subCategory))
            {
                if (!string.IsNullOrEmpty(subConfigRegex.Match(directory).Value))
                    return project.GetExePath(Build.BuildConfiguration);
            }
            else
            {
                if (directory.EndsWith(subCategory))
                    return project.GetExePath($"{Build.BuildConfiguration}{subCategory}");
            }

        return null;
    }
}