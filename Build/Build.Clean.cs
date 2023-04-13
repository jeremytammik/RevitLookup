using Nuke.Common;
using static Nuke.Common.IO.FileSystemTasks;

partial class Build
{
    Target Cleaning => _ => _
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
            if (IsServerBuild) return;

            foreach (var project in Solution.AllProjects.Where(project => project != Solution.Build)) 
                EnsureCleanDirectory(project.Directory / "bin");
        });
}