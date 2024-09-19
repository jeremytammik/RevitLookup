using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Clean => _ => _
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            CleanDirectory(ArtifactsDirectory);
            foreach (var project in Solution.AllProjects.Where(project => project != Solution.Build.Build))
            {
                CleanDirectory(project.Directory / "bin");
                CleanDirectory(project.Directory / "obj");
            }

            foreach (var configuration in GlobBuildConfigurations())
                DotNetClean(settings => settings
                    .SetConfiguration(configuration)
                    .SetVerbosity(DotNetVerbosity.minimal)
                    .EnableNoLogo());
        });

    static void CleanDirectory(AbsolutePath path)
    {
        Log.Information("Cleaning directory: {Directory}", path);
        path.CreateOrCleanDirectory();
    }
}