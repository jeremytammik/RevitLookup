using Nuke.Common.Git;
using Nuke.Common.ProjectModel;

sealed partial class Build : NukeBuild
{
    string[] Configurations;
    Dictionary<string, string> VersionMap;
    Dictionary<Project, Project> InstallersMap;

    [Parameter] string GitHubToken;
    [GitRepository] readonly GitRepository GitRepository;
    [Solution(GenerateProjects = true)] Solution Solution;

    public static int Main() => Execute<Build>(x => x.Compile);
}