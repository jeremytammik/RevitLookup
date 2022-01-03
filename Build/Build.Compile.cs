using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

partial class Build
{
    Target Compile => _ => _
        .TriggeredBy(Cleaning)
        .Executes(() =>
        {
            var configurations = GetConfigurations(BuildConfiguration, InstallerConfiguration);
            configurations.ForEach(configuration =>
            {
                MSBuild(s => s
                    .SetTargets("Rebuild")
                    .SetProcessToolPath(MsBuildPath.Value)
                    .SetConfiguration(configuration)
                    .SetVerbosity(MSBuildVerbosity.Minimal)
                    .DisableNodeReuse()
                    .EnableRestore());
            });
        });
}