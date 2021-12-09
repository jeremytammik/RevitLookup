using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;

partial class Build
{
    Target Cleaning => _ => _
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);

            if (IsServerBuild) return;
            foreach (var projectName in Projects)
            {
                var project = BuilderExtensions.GetProject(Solution, projectName);
                var binDirectory = (AbsolutePath) new DirectoryInfo(project.GetBinDirectory()).FullName;
                binDirectory.GlobDirectories($"{AddInBinPrefix}*", "Release*").ForEach(DeleteDirectory);
            }
        });
}