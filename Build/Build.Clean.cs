using Nuke.Common;
using Nuke.Common.IO;
using Serilog;

sealed partial class Build
{
    Target Clean => _ => _
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            CleanDirectory(ArtifactsDirectory);

            foreach (var project in Solution.AllProjects.Where(project => project != Solution.Build))
                CleanDirectory(project.Directory / "bin");
        });

    static void CleanDirectory(AbsolutePath path)
    {
        Log.Information("Cleaning directory: {Directory}", path);
        path.CreateOrCleanDirectory();
    }
}