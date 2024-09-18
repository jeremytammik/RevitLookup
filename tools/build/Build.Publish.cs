using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Publish => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetPublish(settings => settings
                .SetProject(Solution.DependenciesReport.Path)
                .SetOutput("output")
                .SetRuntime("win-x64")
                .SetProperty("PublishAot", true)
                .SetProperty("AssemblyName", "DependenciesReport-native")
                .SetVerbosity(DotNetVerbosity.minimal));
            
            DotNetPublish(settings => settings
                .SetProject(Solution.DependenciesReport.Path)
                .SetOutput("output")
                .SetRuntime("win-x64")
                .SetSelfContained(false)
                .SetPublishSingleFile(true)
                .SetProperty("AssemblyName", "DependenciesReport-dotnet8")
                .SetVerbosity(DotNetVerbosity.minimal));
        });
}