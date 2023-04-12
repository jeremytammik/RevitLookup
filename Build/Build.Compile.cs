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
                MSBuild(settings =>
                {
                    if (VersionMap.TryGetValue(configuration, out var version))
                        settings = settings.SetProperty("Version", version);

                    return settings
                        .SetTargets("Rebuild")
                        .SetConfiguration(configuration)
                        .SetProcessToolPath(MsBuildPath.Value)
                        .SetVerbosity(MSBuildVerbosity.Minimal)
                        .DisableNodeReuse()
                        .EnableRestore();
                });
            });
        });
}