using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.VSWhere;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

/// <summary>
/// Documentation:
/// https://github.com/Nice3point/RevitTemplates/wiki
/// </summary>
partial class Build : NukeBuild
{
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / ArtifactsFolder;

    [Solution] public readonly Solution Solution;
    string ProcessToolPath;

    Target Restore => _ => _
        .Executes(() =>
        {
            ProcessToolPath = GetMsBuildPath();
            var releaseConfigurations = GetConfigurations(BuildConfiguration, InstallerConfiguration);
            foreach (var configuration in releaseConfigurations) RestoreProject(configuration);
        });

    Target Cleaning => _ => _
        .TriggeredBy(Restore)
        .Executes(() =>
        {
            if (Directory.Exists(ArtifactsDirectory))
            {
                var directoryInfo = new DirectoryInfo(ArtifactsDirectory);
                foreach (var file in directoryInfo.GetFiles())
                {
                    Logger.Normal($"Deleting file: {file.FullName}");
                    file.Delete();
                }

                foreach (var dir in directoryInfo.GetDirectories())
                {
                    Logger.Normal($"Deleting directory: {dir.FullName}");
                    dir.Delete(true);
                }
            }
            else
            {
                Directory.CreateDirectory(ArtifactsDirectory);
            }

            var wixTargetPath = Environment.ExpandEnvironmentVariables(WixTargetPath);

            Logger.Normal("Updating target: WixSharp");
            if (File.Exists(wixTargetPath)) ReplaceFileText("<Target Name=\"MSIAuthoring\">", wixTargetPath, 3);

            if (IsServerBuild) return;
            foreach (var projectName in Projects)
            {
                var project = BuilderExtensions.GetProject(Solution, projectName);
                var binDirectory = new DirectoryInfo(project.GetBinDirectory());
                if (!binDirectory.Exists) return;
                var addInDirectories = binDirectory.EnumerateDirectories().Where(info => info.Name.StartsWith(AddInBinPrefix)).ToList();
                foreach (var addInDirectory in addInDirectories)
                {
                    Logger.Normal($"Deleting directory: {addInDirectory.FullName}");
                    foreach (var file in addInDirectory.GetFiles()) file.Delete();
                    addInDirectory.Delete(true);
                }
            }
        });

    Target Compile => _ => _
        .TriggeredBy(Cleaning)
        .Executes(() =>
        {
            var configurations = GetConfigurations(BuildConfiguration, InstallerConfiguration);
            foreach (var configuration in configurations) BuildProject(configuration);
        });

    public static int Main() => Execute<Build>(x => x.Restore);

    List<string> GetConfigurations(params string[] startPatterns)
    {
        var configurations = Solution.Configurations
            .Select(pair => pair.Key)
            .Where(s => startPatterns.Any(s.StartsWith))
            .Select(s =>
            {
                var platformIndex = s.LastIndexOf('|');
                return s.Remove(platformIndex);
            })
            .ToList();
        if (configurations.Count == 0) throw new Exception($"Can't find configurations in the solution by patterns: {string.Join(" | ", startPatterns)}.");
        return configurations;
    }

    IEnumerable<IGrouping<int, DirectoryInfo>> GetBuildDirectories()
    {
        var directories = new List<DirectoryInfo>();
        foreach (var projectName in Projects)
        {
            var project = BuilderExtensions.GetProject(Solution, projectName);
            var directoryInfo = new DirectoryInfo(project.GetBinDirectory()).GetDirectories();
            directories.AddRange(directoryInfo);
        }

        if (directories.Count == 0) throw new Exception("There are no packaged assemblies in the project. Try to build the project again.");

        var addInsDirectory = directories
            .Where(dir => dir.Name.StartsWith(AddInBinPrefix))
            .Where(dir => dir.Name.Contains(BuildConfiguration))
            .GroupBy(dir => dir.Name.Length);

        return addInsDirectory;
    }

    static void ReplaceFileText(string newText, string fileName, int lineNumber)
    {
        var arrLine = File.ReadAllLines(fileName);
        var lineText = arrLine[lineNumber - 1];
        if (lineText.Equals(newText)) return;
        arrLine[lineNumber - 1] = newText;
        File.WriteAllLines(fileName, arrLine);
    }

    string GetMsBuildPath()
    {
        if (IsServerBuild) return null;
        var vsWhere = VSWhereTasks.VSWhere(settings => settings
            .EnableLatest()
            .AddRequires("Microsoft.Component.MSBuild")
            .DisableProcessLogOutput()
            .DisableProcessLogInvocation()
        );

        if (vsWhere.Output.Count > 3) return null;
        if (!File.Exists(CustomMsBuildPath)) throw new Exception($"Missing file: {CustomMsBuildPath}. Change the path to the build platform or install Visual Studio.");
        return CustomMsBuildPath;
    }

    void RestoreProject(string configuration)
    {
        MSBuild(s => s
            .SetTargets("Restore")
            .SetTargetPath(Solution)
            .SetConfiguration(configuration)
            .SetProcessToolPath(ProcessToolPath)
            .SetVerbosity(MSBuildVerbosity.Minimal)
        );
    }

    void BuildProject(string configuration)
    {
        MSBuild(s => s
            .SetTargets("Rebuild")
            .SetTargetPath(Solution)
            .SetConfiguration(configuration)
            .SetProcessToolPath(ProcessToolPath)
            .SetVerbosity(MSBuildVerbosity.Minimal)
            .SetMSBuildPlatform(MSBuildPlatform.x64)
            .SetMaxCpuCount(Environment.ProcessorCount)
            .DisableNodeReuse()
            .EnableRestore());
    }
}