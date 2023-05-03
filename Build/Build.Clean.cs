using Nuke.Common;
using static Nuke.Common.IO.FileSystemTasks;

partial class Build
{
    Target Clean => _ => _
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);

            foreach (var project in Solution.AllProjects.Where(project => project != Solution.Build))
                EnsureCleanDirectory(project.Directory / "bin");
        });
}