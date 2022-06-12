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
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;
using RevitLookup.UI.Common;
using RevitLookup.UI.Tests.ViewModels.Enums;
using RevitLookup.UI.Tests.ViewModels.Objects;

namespace RevitLookup.UI.Tests.ViewModels.Pages;

public sealed class AboutViewModel : INotifyPropertyChanged
{
    private string _errorMessage;
    private bool _isCheckedUpdates;
    private string _latestCheckDate = "never";
    private string _newVersion;
    private string _releaseNotesUrl;
    private UpdatingState _state;
    private string _version;
    private bool _isDownloading;

    public AboutViewModel()
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var info = FileVersionInfo.GetVersionInfo(assembly.Location);
        Version = info.ProductVersion;
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
        get => $"Latest check: {_latestCheckDate}";
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

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (value == _errorMessage) return;
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public bool IsCheckedUpdates
    {
        get => _isCheckedUpdates;
        set
        {
            if (value == _isCheckedUpdates) return;
            _isCheckedUpdates = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public bool IsDownloading
    {
        get => _isDownloading;
        set
        {
            if (value == _isDownloading) return;
            _isDownloading = value;
            OnPropertyChanged();
        }
    }

    public string DownloadUrl { get; set; }
    public string DownloadedInstallerFilename { get; set; }

    public RelayCommand CheckUpdatesCommand => new(CheckUpdates);
    public RelayCommand DownloadCommand => new(DownloadUpdate);

    public event PropertyChangedEventHandler PropertyChanged;

    private async void CheckUpdates()
    {
        try
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
                ErrorMessage = "GitHub server unavailable to check for updates";
            }
            else
            {
                var latestRelease = releases.OrderByDescending(release => release.PublishedDate).First();
                var newVersion = new Version(latestRelease.TagName);
                if (newVersion > new Version(Version))
                {
                    State = UpdatingState.ReadyToDownload;
                    NewVersion = newVersion.ToString(3);
                    DownloadUrl = latestRelease.Assets[0].DownloadUrl;
                    ReleaseNotesUrl = latestRelease.Url;
                }
                else
                {
                    State = UpdatingState.UpToDate;
                }
            }
        }
        catch
        {
            State = UpdatingState.ErrorChecking;
            ErrorMessage = "An error occurred while checking for updates";
        }
        finally
        {
            IsCheckedUpdates = true;
            LatestCheckDate = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }
    }

    private async void DownloadUpdate()
    {
        IsDownloading = true;
        State = UpdatingState.ReadyToDownload;
        try
        {
            await Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(3)));
            throw new NetworkInformationException();
        }
        catch
        {
            State = UpdatingState.ErrorDownloading;
            ErrorMessage = "An error occurred while downloading the update";
        }
        finally
        {
            IsDownloading = false;
        }
    }

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}