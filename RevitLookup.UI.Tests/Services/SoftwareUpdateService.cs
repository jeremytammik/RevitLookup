// Copyright 2003-2022 by Autodesk, Inc.
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

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RevitLookup.UI.Tests.Models;
using RevitLookup.UI.Tests.Services.Contracts;
using RevitLookup.UI.Tests.Services.Enums;

namespace RevitLookup.UI.Tests.Services;

public class SoftwareUpdateService : ISoftwareUpdateService
{
    private readonly IConfiguration _configuration;

    public SoftwareUpdateService(IConfiguration configuration)
    {
        _configuration = configuration;
        var assembly = configuration.GetValue<string>("Assembly");
        CurrentVersion = FileVersionInfo.GetVersionInfo(assembly).ProductVersion;
    }

    public SoftwareUpdateState State { get; set; }
    public string CurrentVersion { get; }
    public string NewVersion { get; set; }
    public string LatestCheckDate { get; set; }
    public string ReleaseNotesUrl { get; set; }
    public string ErrorMessage { get; set; }
    public string DownloadUrl { get; set; }
    public string LocalFilePath { get; set; }
    public async Task CheckUpdates()
    {
        try
        {
            if (!string.IsNullOrEmpty(LocalFilePath))
            {
                if (LocalFilePath.Contains(NewVersion))
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

            var releases = JsonConvert.DeserializeObject<List<GutHubResponse>>(releasesJson);
            if (releases is null)
            {
                State = SoftwareUpdateState.ErrorChecking;
                ErrorMessage = "GitHub server unavailable to check for updates";
            }
            else
            {
                var latestRelease = releases.OrderByDescending(release => release.PublishedDate).First();
                var releaseTag = new Version(latestRelease.TagName);
                
                var downloadFolder = _configuration.GetValue<string>("DownloadFolder");
                var newVersion = releaseTag.ToString(3);
                foreach (var file in Directory.EnumerateFiles(downloadFolder))
                {
                    if (file.Contains(newVersion))
                    {
                        LocalFilePath = file;
                        NewVersion = newVersion;
                        State = SoftwareUpdateState.ReadyToInstall;
                        return;
                    }
                }
                
                if (releaseTag > new Version(CurrentVersion))
                {
                    State = SoftwareUpdateState.ReadyToDownload;
                    NewVersion = newVersion;
                    DownloadUrl = latestRelease.Assets[0].DownloadUrl;
                    ReleaseNotesUrl = latestRelease.Url;
                }
                else
                {
                    State = SoftwareUpdateState.UpToDate;
                }
            }
        }
        catch (HttpRequestException)
        {
            State = SoftwareUpdateState.ErrorChecking;
            ErrorMessage = "An error occurred while checking for updates. GitHub request limit exceeded";
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
            var downloadFolder = _configuration.GetValue<string>("DownloadFolder");
            Directory.CreateDirectory(downloadFolder);
            var fileName = Path.Combine(downloadFolder, $"RevitLookup-{NewVersion}.msi");
            
            using var webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(DownloadUrl, fileName);
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