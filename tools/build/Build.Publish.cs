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
                .SetOutput("output/native")
                .SetCommonPublishProperties()
                .SetProperty("PublishAot", true)
                .SetProperty("AssemblyName", "DependenciesReport-native"));

            DotNetPublish(settings => settings
                .SetProject(Solution.DependenciesReport.Path)
                .SetOutput("output/dotnet")
                .SetCommonPublishProperties()
                .SetSelfContained(false)
                .SetPublishSingleFile(true)
                .SetProperty("AssemblyName", "DependenciesReport-dotnet"));
        });
}

static class PublishExtensions
{
    public static DotNetPublishSettings SetCommonPublishProperties(this DotNetPublishSettings settings)
    {
        return settings
            .SetRuntime("win-x64")
            .SetConfiguration("Release")
            .SetProperty("DebugType", "None")
            .SetProperty("DebugSymbols", "False")
            .SetVerbosity(DotNetVerbosity.minimal);
    }
}