using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(AzurePipelinesImage.WindowsLatest,
    AutoGenerate = false,
    TriggerBranchesInclude = new[] {"main"},
    PullRequestsBranchesInclude = new[] {"main"})]
partial class Build
{
}