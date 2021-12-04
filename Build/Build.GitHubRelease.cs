using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Octokit;

partial class Build
{
    [GitVersion(NoFetch = true)] readonly GitVersion GitVersion;
    readonly Regex VersionRegex = new(@"(\d+\.)+\d+", RegexOptions.Compiled);
    [Parameter] string GitHubToken { get; set; }
    AbsolutePath ChangeLogPath => RootDirectory / "Doc" / "Changelog.md";

    Target PublishGitHubRelease => _ => _
        .TriggeredBy(CreateInstaller)
        .Requires(() => GitHubToken)
        .Requires(() => GitRepository)
        .Requires(() => GitVersion)
        .OnlyWhenStatic(() => GitRepository.IsOnMasterBranch())
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
            {
                Credentials = new Credentials(GitHubToken)
            };

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();
            var msiFiles = Directory.GetFiles(ArtifactsDirectory, "*.msi");
            var version = GetMsiVersion(msiFiles);

            CheckTags(gitHubOwner, gitHubName, version);
            Logger.Normal($"Detected Tag: {version}");

            var newRelease = new NewRelease(version)
            {
                Name = version,
                Body = CreateChangelog(version),
                Draft = true,
                TargetCommitish = GitVersion.Sha
            };

            var draft = CreatedDraft(gitHubOwner, gitHubName, newRelease);
            UploadMsiFiles(draft, msiFiles);
            ReleaseDraft(gitHubOwner, gitHubName, draft);
        });

    string CreateChangelog(string version)
    {
        if (!File.Exists(ChangeLogPath))
        {
            Logger.Warn($"Can't find changelog file: {ChangeLogPath}");
            return string.Empty;
        }

        Logger.Normal($"Detected Changelog: {ChangeLogPath}");
        var logBuilder = new StringBuilder();
        foreach (var line in File.ReadLines(ChangeLogPath))
        {
            if (logBuilder.Length > 0)
            {
                if (line.StartsWith("-")) break;
                logBuilder.AppendLine(line);
                continue;
            }

            var match = VersionRegex.Match(line);
            if (!match.Success) continue;
            if (match.Value.Equals(version))
            {
                var truncatedLine = Regex.Replace(line, $"^.*{version}.? ", string.Empty);
                logBuilder.AppendLine(truncatedLine);
            }
        }

        var log = logBuilder.ToString();
        if (string.IsNullOrEmpty(log)) Logger.Warn($"There is no version entry in the changelog: {version}");
        return log;
    }

    static void CheckTags(string gitHubOwner, string gitHubName, string version)
    {
        var gitHubTags = GitHubTasks.GitHubClient.Repository
            .GetAllTags(gitHubOwner, gitHubName)
            .Result;

        if (gitHubTags.Select(tag => tag.Name).Contains(version)) throw new ArgumentException($"The repository already contains a Release with the tag: {version}");
    }

    string GetMsiVersion(IEnumerable<string> msiFiles)
    {
        var stringVersion = string.Empty;
        var doubleVersion = 0d;
        foreach (var msiFile in msiFiles)
        {
            var fileInfo = new FileInfo(msiFile);
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

        if (stringVersion.Equals(string.Empty)) throw new ArgumentException("The version number of the MSI files was not found.");

        return stringVersion;
    }

    static void UploadMsiFiles(Release createdRelease, IEnumerable<string> msiFiles)
    {
        foreach (var file in msiFiles)
        {
            var releaseAssetUpload = new ReleaseAssetUpload
            {
                ContentType = "application/x-binary",
                FileName = Path.GetFileName(file),
                RawData = File.OpenRead(file)
            };
            var _ = GitHubTasks.GitHubClient.Repository.Release.UploadAsset(createdRelease, releaseAssetUpload).Result;
            Logger.Normal($"Added MSI file: {file}");
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