using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;

sealed partial class Build : NukeBuild
{
    string[] Configurations;
    GitRepository GitRepositoryCache;
    Dictionary<string, string> VersionMap;
    Dictionary<Project, Project> InstallersMap;

    [Parameter] string GitHubToken;
    [Solution(GenerateProjects = true)] Solution Solution;

    GitRepository GitRepository => GitRepositoryCache ??= GitRepository.FromLocalDirectory(RootDirectory);

    public static int Main() => Execute<Build>(x => x.Clean);
}