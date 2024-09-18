using Nuke.Common.ProjectModel;

sealed partial class Build : NukeBuild
{
    string[] Configurations;
    Dictionary<Project, Project> InstallersMap;

    [Solution(GenerateProjects = true)] Solution Solution;

    public static int Main() => Execute<Build>(x => x.Compile);
}