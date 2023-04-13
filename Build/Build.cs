using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;

partial class Build : NukeBuild
{
    [GitRepository] readonly GitRepository GitRepository;
    [Solution(GenerateProjects = true)] readonly Solution Solution;

    public static int Main() => Execute<Build>(x => x.Cleaning);
}