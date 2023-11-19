// Copyright 2003-2023 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.DTO;
using RevitLookup.Services.Enums;

namespace RevitLookup.Services;

public sealed class SoftwareUpdateService(IConfiguration configuration) : ISoftwareUpdateService
{
    private readonly Regex _versionRegex = new(@"(\d+\.)+\d+", RegexOptions.Compiled);
    private readonly bool _writeAccess = configuration.GetValue<string>("FolderAccess") == "Write";
    private string _downloadUrl;

    public SoftwareUpdateState State { get; private set; }
    public string CurrentVersion { get; } = configuration.GetValue<string>("AddinVersion");
    public string NewVersion { get; private set; }
    public string LatestCheckDate { get; private set; }
    public string ReleaseNotesUrl { get; private set; }
    public string ErrorMessage { get; private set; }
    public string LocalFilePath { get; private set; }

    public async Task CheckUpdates()
    {
        try
        {
            if (!string.IsNullOrEmpty(LocalFilePath))
                if (File.Exists(LocalFilePath))
                {
                    var fileName = Path.GetFileName(LocalFilePath);
                    if (fileName.Contains(NewVersion))
                    {
                        State = SoftwareUpdateState.ReadyToInstall;
                        return;
                    }
                }

            string releasesJson;
            using (var gitHubClient = new HttpClient())
            {
                gitHubClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "RevitLookup");
                releasesJson = await gitHubClient.GetStringAsync("https://api.github.com/repos/jeremytammik/RevitLookup/releases");
            }

            var releases = JsonSerializer.Deserialize<List<GutHubResponse>>(releasesJson);
            if (releases is not null)
            {
                var latestRelease = releases
                    .Where(response => !response.Draft)
                    .Where(response => !response.PreRelease)
                    .OrderByDescending(release => release.PublishedDate)
                    .FirstOrDefault();

                if (latestRelease is null)
                {
                    State = SoftwareUpdateState.UpToDate;
                    return;
                }

                // Finding a new version
                Version newVersionTag = null;
                var currentTag = new Version(CurrentVersion);
                foreach (var asset in latestRelease.Assets)
                {
                    var match = _versionRegex.Match(asset.Name);
                    if (!match.Success) continue;
                    if (!match.Value.StartsWith(currentTag.Major.ToString())) continue;
                    if (_writeAccess && asset.Name.Contains("MultiUser")) continue;

                    newVersionTag = new Version(match.Value);
                    _downloadUrl = asset.DownloadUrl;
                    break;
                }

                // Checking available releases
                if (newVersionTag is null)
                {
                    State = SoftwareUpdateState.UpToDate;
                    return;
                }

                // Checking for a new release version
                if (newVersionTag <= currentTag)
                {
                    State = SoftwareUpdateState.UpToDate;
                    return;
                }

                // Checking downloaded releases
                var downloadFolder = configuration.GetValue<string>("DownloadFolder");
                NewVersion = newVersionTag.ToString(3);
                if (Directory.Exists(downloadFolder))
                    foreach (var file in Directory.EnumerateFiles(downloadFolder))
                        if (file.EndsWith(Path.GetFileName(_downloadUrl)!))
                        {
                            LocalFilePath = file;
                            State = SoftwareUpdateState.ReadyToInstall;
                            return;
                        }

                State = SoftwareUpdateState.ReadyToDownload;
                ReleaseNotesUrl = latestRelease.Url;
            }
            else
            {
                State = SoftwareUpdateState.ErrorChecking;
                ErrorMessage = "GitHub server unavailable to check for updates";
            }
        }
        catch (HttpRequestException)
        {
            // GitHub request limit exceeded
            State = SoftwareUpdateState.UpToDate;
        }
        catch
        {
            State = SoftwareUpdateState.ErrorChecking;
            ErrorMessage = "An error occurred while checking for updates";
        }
        finally
        {
            LatestCheckDate = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }
    }

    public async Task DownloadUpdate()
    {
        try
        {
            var downloadFolder = configuration.GetValue<string>("DownloadFolder");
            Directory.CreateDirectory(downloadFolder);
            var fileName = Path.Combine(downloadFolder, Path.GetFileName(_downloadUrl));

            using var webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(_downloadUrl!, fileName);
            LocalFilePath = fileName;
            State = SoftwareUpdateState.ReadyToInstall;
        }
        catch
        {
            State = SoftwareUpdateState.ErrorDownloading;
            ErrorMessage = "An error occurred while downloading the update";
        }
    }
}