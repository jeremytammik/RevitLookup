using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Octokit;
using Serilog;

partial class Build
{
    [GitVersion(NoFetch = true)] readonly GitVersion GitVersion;
    [Parameter] string GitHubToken { get; set; }
    readonly Regex VersionRegex = new(@"(\d+\.)+\d+", RegexOptions.Compiled);

    Target PublishGitHubRelease => _ => _
        .TriggeredBy(CreateInstaller)
        .Requires(() => GitHubToken)
        .Requires(() => GitRepository)
        .Requires(() => GitVersion)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
            {
                Credentials = new Credentials(GitHubToken)
            };

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();
            var artifacts = Directory.GetFiles(ArtifactsDirectory, "*");
            var version = GetProductVersion(artifacts);

            CheckTags(gitHubOwner, gitHubName, version);
            Log.Information("Detected Tag: {Version}", version);

            var newRelease = new NewRelease(version)
            {
                Name = version,
                Body = CreateChangelog(version),
                Draft = true,
                TargetCommitish = GitVersion.Sha
            };

            var draft = CreatedDraft(gitHubOwner, gitHubName, newRelease);
            UploadArtifacts(draft, artifacts);
            ReleaseDraft(gitHubOwner, gitHubName, draft);
        });

    string CreateChangelog(string version)
    {
        if (!File.Exists(ChangeLogPath))
        {
            Log.Warning("Can't find changelog file: {Log}", ChangeLogPath);
            return string.Empty;
        }

        Log.Information("Detected Changelog: {Path}", ChangeLogPath);

        var logBuilder = new StringBuilder();
        var changelogLineRegex = new Regex($@"^.*({version})\S*\s");

        foreach (var line in File.ReadLines(ChangeLogPath))
        {
            if (logBuilder.Length > 0)
            {
                logBuilder.AppendLine(line);
                continue;
            }

            if (!changelogLineRegex.Match(line).Success) continue;
            var truncatedLine = changelogLineRegex.Replace(line, string.Empty);
            logBuilder.AppendLine(truncatedLine);
        }

        if (logBuilder.Length == 0) Log.Warning("There is no version entry in the changelog: {Version}", version);
        return logBuilder.ToString();
    }

    static void CheckTags(string gitHubOwner, string gitHubName, string version)
    {
        var gitHubTags = GitHubTasks.GitHubClient.Repository
            .GetAllTags(gitHubOwner, gitHubName)
            .Result;

        if (gitHubTags.Select(tag => tag.Name).Contains(version)) throw new ArgumentException($"The repository already contains a Release with the tag: {version}");
    }

    string GetProductVersion(IEnumerable<string> artifacts)
    {
        var stringVersion = string.Empty;
        var doubleVersion = 0d;
        foreach (var file in artifacts)
        {
            var fileInfo = new FileInfo(file);
            var match = VersionRegex.Match(fileInfo.Name);
            if (!match.Success) continue;
            var version = match.Value;
            var parsedValue = double.Parse(version.Replace(".", ""));
            if (parsedValue > doubleVersion)
            {
                doubleVersion = parsedValue;
                stringVersion = version;
            }
        }

        if (stringVersion.Equals(string.Empty)) throw new ArgumentException("Could not determine product version from artifacts.");

        return stringVersion;
    }

    static void UploadArtifacts(Release createdRelease, IEnumerable<string> artifacts)
    {
        foreach (var file in artifacts)
        {
            var releaseAssetUpload = new ReleaseAssetUpload
            {
                ContentType = "application/x-binary",
                FileName = Path.GetFileName(file),
                RawData = File.OpenRead(file)
            };
            var _ = GitHubTasks.GitHubClient.Repository.Release.UploadAsset(createdRelease, releaseAssetUpload).Result;
            Log.Information("Added artifact: {Path}", file);
        }
    }

    static Release CreatedDraft(string gitHubOwner, string gitHubName, NewRelease newRelease) =>
        GitHubTasks.GitHubClient.Repository.Release
            .Create(gitHubOwner, gitHubName, newRelease)
            .Result;

    static void ReleaseDraft(string gitHubOwner, string gitHubName, Release draft)
    {
        var _ = GitHubTasks.GitHubClient.Repository.Release
            .Edit(gitHubOwner, gitHubName, draft.Id, new ReleaseUpdate {Draft = false})
            .Result;
    }
}