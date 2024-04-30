using System.Text;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;
using Octokit;
using Serilog;

sealed partial class Build
{
    Target PublishGitHub => _ => _
        .DependsOn(CreateInstaller)
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

            ValidateRelease(version);

            var artifacts = Directory.GetFiles(ArtifactsDirectory, "*");
            var changelog = CreateGithubChangelog(version);
            Assert.NotEmpty(artifacts, "No artifacts were found to create the Release");

            var newRelease = new NewRelease(version)
            {
                Name = version,
                Body = changelog,
                TargetCommitish = GitRepository.Commit
            };

            var release = await GitHubTasks.GitHubClient.Repository.Release.Create(gitHubOwner, gitHubName, newRelease);
            await UploadArtifactsAsync(release, artifacts);
        });

    void ValidateRelease(string version)
    {
        var tags = GitTasks.Git("describe --tags --abbrev=0", logInvocation: false, logOutput: false);
        if (tags.Count == 0) return;

        Assert.False(tags.Last().Text == version, $"A Release with the specified tag already exists in the repository: {version}");
        Log.Information("Version: {Version}", version);
    }

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

    string CreateGithubChangelog(string version)
    {
        Assert.True(File.Exists(ChangeLogPath), $"Unable to locate the changelog file: {ChangeLogPath}");
        Log.Information("Changelog: {Path}", ChangeLogPath);

        var changelog = BuildChangelog(version);
        Assert.True(changelog.Length > 0, $"No version entry exists in the changelog: {version}");

        WriteCompareUrl(version, changelog);
        WriteAdditionalInfo(changelog);
        return changelog.ToString();
    }

    void WriteCompareUrl(string version, StringBuilder changelog)
    {
        var tags = GitTasks.Git("describe --tags --abbrev=0", logInvocation: false, logOutput: false);
        if (tags.Count == 0) return;

        if (changelog[^1] != '\r' || changelog[^1] != '\n') changelog.AppendLine(Environment.NewLine);
        changelog.Append("Full changelog: ");
        changelog.Append(GitRepository.GetGitHubCompareTagsUrl(version, tags.Last().Text));
    }

    void WriteAdditionalInfo(StringBuilder changelog)
    {
        changelog.AppendLine();
        changelog.Append("RevitLookup versioning: https://github.com/jeremytammik/RevitLookup/wiki/Versions");
    }

    StringBuilder BuildChangelog(string version)
    {
        const string separator = "# ";

        var hasEntry = false;
        var changelog = new StringBuilder();
        foreach (var line in File.ReadLines(ChangeLogPath))
        {
            if (hasEntry)
            {
                if (line.StartsWith(separator)) break;

                changelog.AppendLine(line);
                continue;
            }

            if (line.StartsWith(separator) && line.Contains(version))
            {
                hasEntry = true;
            }
        }

        TrimEmptyLines(changelog);
        return changelog;
    }

    static void TrimEmptyLines(StringBuilder builder)
    {
        if (builder.Length == 0) return;

        while (builder[^1] == '\r' || builder[^1] == '\n')
        {
            builder.Remove(builder.Length - 1, 1);
        }

        while (builder[0] == '\r' || builder[0] == '\n')
        {
            builder.Remove(0, 1);
        }
    }
}