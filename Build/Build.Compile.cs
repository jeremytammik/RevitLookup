using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    Target Compile => _ => _
        .TriggeredBy(Cleaning)
        .Executes(() =>
        {
            var buildConfig = GetConfigurations(BuildConfiguration, InstallerConfiguration);
            buildConfig.ForEach(configuration =>
            {
                DotNetBuild(settings =>
                {
                    if (VersionMap.TryGetValue(configuration, out var value))
                        settings = settings.SetVersion(value);

                    return settings.SetConfiguration(configuration)
                        .SetVerbosity(DotNetVerbosity.Minimal);
                });
            });
        });
}