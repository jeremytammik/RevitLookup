using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.VSWhere;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

partial class Build
{
    Target Compile => _ => _
        .TriggeredBy(Cleaning)
        .Executes(() =>
        {
            var msBuildPath = GetMsBuildPath();
            var configurations = GetConfigurations(BuildConfiguration, InstallerConfiguration);
            foreach (var configuration in configurations) CompileProject(configuration, msBuildPath);
        });

    static string GetMsBuildPath()
    {
        if (IsServerBuild) return null;
        var (_, output) = VSWhereTasks.VSWhere(settings => settings
            .EnableLatest()
            .AddRequires("Microsoft.Component.MSBuild")
            .SetProperty("installationPath")
        );

        if (output.Count > 0) return null;
        if (!File.Exists(CustomMsBuildPath)) throw new Exception($"Missing file: {CustomMsBuildPath}. Change the path to the build platform or install Visual Studio.");
        return CustomMsBuildPath;
    }

    void CompileProject(string configuration, string toolPath) =>
        MSBuild(s => s
            .SetTargets("Rebuild")
            .SetTargetPath(Solution)
            .SetProcessToolPath(toolPath)
            .SetConfiguration(configuration)
            .SetVerbosity(MSBuildVerbosity.Minimal)
            .SetMSBuildPlatform(MSBuildPlatform.x64)
            .SetMaxCpuCount(Environment.ProcessorCount)
            .DisableNodeReuse()
            .EnableRestore());
}