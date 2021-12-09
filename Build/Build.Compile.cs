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
            var configurations = GetConfigurations(BuildConfiguration, InstallerConfiguration);
            foreach (var configuration in configurations) BuildProject(configuration);
        });

    static string GetMsBuildPath()
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

    void BuildProject(string configuration) =>
        MSBuild(s => s
            .SetTargets("Rebuild")
            .SetTargetPath(Solution)
            .SetConfiguration(configuration)
            .SetProcessToolPath(GetMsBuildPath())
            .SetVerbosity(MSBuildVerbosity.Minimal)
            .SetMSBuildPlatform(MSBuildPlatform.x64)
            .SetMaxCpuCount(Environment.ProcessorCount)
            .DisableNodeReuse()
            .EnableRestore());
}