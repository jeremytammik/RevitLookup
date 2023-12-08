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

            var version = VersionMap.Values.Last();
            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();
            var artifacts = Directory.GetFiles(ArtifactsDirectory, "*");
            var tags = await GitHubTasks.GitHubClient.Repository.GetAllTags(gitHubOwner, gitHubName);

            Assert.False(tags.Select(tag => tag.Name).Contains(version), $"A Release with the specified tag already exists in the repository: {version}");
            Log.Information("Version: {Version}", version);

            var changelog = CreateChangelog(tags[0].Name, version);
            var newRelease = new NewRelease(version)
            {
                Name = version,
                Body = changelog,
                TargetCommitish = GitRepository.Commit
            };

            var release = await CreatedReleaseAsync(gitHubOwner, gitHubName, newRelease);
            await UploadArtifactsAsync(release, artifacts);
        });

    static Task<Release> CreatedReleaseAsync(string gitHubOwner, string gitHubName, NewRelease newRelease) =>
        GitHubTasks.GitHubClient.Repository.Release.Create(gitHubOwner, gitHubName, newRelease);

    static async Task UploadArtifactsAsync(Release createdRelease, IEnumerable<string> artifacts)
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

    string CreateChangelog(string previousVersion, string version)
    {
        if (!File.Exists(ChangeLogPath))
        {
            Log.Warning("Unable to locate the changelog file: {Log}", ChangeLogPath);
            return string.Empty;
        }

        const string nextRecordSymbol = "# ";

        var logBuilder = new StringBuilder();
        var changelogLineRegex = new Regex($@"^.*({version})\S*\s?");
        Log.Information("Changelog: {Path}", ChangeLogPath);

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

        AppendCompareUrl(logBuilder, previousVersion, version);
        return logBuilder.ToString();
    }

    void AppendCompareUrl(StringBuilder logBuilder, string previousVersion, string version)
    {
        logBuilder.Append("Full changelog: ");
        logBuilder.Append(GitRepository.GetGitHubCompareTagsUrl(version, previousVersion));
    }
}