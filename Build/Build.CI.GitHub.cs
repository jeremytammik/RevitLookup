using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Octokit;
using Serilog;

sealed partial class Build
{
    Target Publish => _ => _
        .TriggeredBy(CreateInstaller)
        .Requires(() => GitHubToken)
        .OnlyWhenStatic(() => IsServerBuild && GitRepository.IsOnMainOrMasterBranch())
        .Executes(async () =>
        {
            GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
            {
                Credentials = new Credentials(GitHubToken)
            };

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();
            var artifacts = Directory.GetFiles(ArtifactsDirectory, "*");
            var version = VersionRegex.Match(artifacts[^1]).Value;

            Assert.False(GitRepository.Tags.Contains(version), $"A Release with the specified tag already exists in the repository: {version}");
            Log.Information("Tag: {Version}", version);

            var newRelease = new NewRelease(version)
            {
                Name = version,
                Body = CreateChangelog(version),
                Draft = true,
                TargetCommitish = GitRepository.Commit
            };

            var draftRelease = await GitHubTasks.GitHubClient.Repository.Release.Create(gitHubOwner, gitHubName, newRelease);
            await UploadArtifactsAsync(draftRelease, artifacts);
            await GitHubTasks.GitHubClient.Repository.Release.Edit(gitHubOwner, gitHubName, draftRelease.Id, new ReleaseUpdate {Draft = false});
        });

    static async Task UploadArtifactsAsync(Release createdRelease, string[] artifacts)
    {
        foreach (var file in artifacts)
        {
            var releaseAssetUpload = new ReleaseAssetUpload
            {
                ContentType = "application/x-binary",
                FileName = Path.GetFileName(file),
                RawData = File.OpenRead(file)
            };

            await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(createdRelease, releaseAssetUpload);
            Log.Information("Artifact: {Path}", file);
        }
    }

    string CreateChangelog(string version)
    {
        if (!File.Exists(ChangeLogPath))
        {
            Log.Warning("Unable to locate the changelog file: {Log}", ChangeLogPath);
            return string.Empty;
        }

        Log.Information("Changelog: {Path}", ChangeLogPath);

        var logBuilder = new StringBuilder();
        var changelogLineRegex = new Regex($@"^.*({version})\S*\s?");
        const string nextRecordSymbol = "# ";

        foreach (var line in File.ReadLines(ChangeLogPath))
        {
            if (logBuilder.Length > 0)
            {
                if (line.StartsWith(nextRecordSymbol)) break;
                logBuilder.AppendLine(line);
                continue;
            }

            if (!changelogLineRegex.Match(line).Success) continue;
            var truncatedLine = changelogLineRegex.Replace(line, string.Empty);
            logBuilder.AppendLine(truncatedLine);
        }

        if (logBuilder.Length == 0) Log.Warning("No version entry exists in the changelog: {Version}", version);
        return logBuilder.ToString();
    }
}