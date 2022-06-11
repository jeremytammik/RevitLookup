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

using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;
using RevitLookup.UI.Common;
using RevitLookup.UI.Tests.ViewModels.Enums;
using RevitLookup.UI.Tests.ViewModels.Objects;

namespace RevitLookup.UI.Tests.ViewModels.Pages;

public sealed class AboutViewModel : INotifyPropertyChanged
{
    private string _latestCheckDate;
    private string _version;
    private UpdatingState _state;
    private string _newVersion;
    private string _releaseNotesUrl;

    public AboutViewModel()
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var info = FileVersionInfo.GetVersionInfo(assembly.Location);
        Version = info.ProductVersion;
        LatestCheckDate = $"Latest check: {DateTime.Now:yyyy.MM.dd HH:mm:ss}";
    }

    public RelayCommand CheckUpdatesCommand => new(CheckUpdates);

    private async void CheckUpdates()
    {
        string releasesJson;
        using (var gitHubClient = new HttpClient())
        {
            gitHubClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "RevitLookup");
            releasesJson = await gitHubClient.GetStringAsync("https://api.github.com/repos/jeremytammik/RevitLookup/releases");
        }

        var releases = JsonConvert.DeserializeObject<List<GutHubApiDto>>(releasesJson);
        if (releases is null)
        {
            State = UpdatingState.ErrorChecking;
            return;
        }

        var latestRelease = releases.OrderByDescending(release => release.PublishedDate).First();
        var newVersion = new Version(latestRelease.TagName);
        if (newVersion > new Version(Version))
        {
            State = UpdatingState.ReadyToDownload;
            NewVersion = newVersion.ToString(3);
            DownloadUrl = latestRelease.Assets[0].DownloadUrl;
        }
        else if (newVersion == new Version(Version))
        {
            State = UpdatingState.UpToDate;
        }

        LatestCheckDate = $"Latest check: {DateTime.Now:yyyy.MM.dd HH:mm:ss}";
        ReleaseNotesUrl = latestRelease.Url;
    }

    public UpdatingState State
    {
        get => _state;
        set
        {
            if (value == _state) return;
            _state = value;
            OnPropertyChanged();
        }
    }

    public string Version
    {
        get => _version;
        set
        {
            if (value == _version) return;
            _version = value;
            OnPropertyChanged();
        }
    }

    public string LatestCheckDate
    {
        get => _latestCheckDate;
        set
        {
            if (value == _latestCheckDate) return;
            _latestCheckDate = value;
            OnPropertyChanged();
        }
    }

    public string ReleaseNotesUrl
    {
        get => _releaseNotesUrl;
        set
        {
            if (value == _releaseNotesUrl) return;
            _releaseNotesUrl = value;
            OnPropertyChanged();
        }
    }

    public string DownloadUrl { get; set; }
    public string DownloadedInstallerFilename { get; set; }

    public string NewVersion
    {
        get => _newVersion;
        set
        {
            if (value == _newVersion) return;
            _newVersion = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}