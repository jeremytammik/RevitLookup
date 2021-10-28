using Nuke.Common.CI.GitHubActions;

[GitHubActions("CreatePackage",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = false,
    OnPullRequestBranches = new[] {"main"},
    OnPushBranches = new[] {"main"})]
partial class Build
{
}