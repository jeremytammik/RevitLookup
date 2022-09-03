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

using System.Runtime.Versioning;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.UI.Tests.Services.Contracts;
using RevitLookup.UI.Tests.Services.Enums;

namespace RevitLookup.UI.Tests.ViewModels.Pages;

public sealed class AboutViewModel : ObservableObject
{
    private readonly ISoftwareUpdateService _updateService;
    private string _dotNetVersion;
    private string _runtimeVersion;
    private bool _isUpdateChecked;

    public AboutViewModel(ISoftwareUpdateService updateService)
    {
        _updateService = updateService;
        DotNetVersion = new FrameworkName(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName).Version.ToString();
        RuntimeVersion = Environment.Version.ToString();
        CheckUpdatesCommand = new AsyncRelayCommand(CheckUpdates);
        DownloadCommand = new AsyncRelayCommand(DownloadUpdate);
    }

    public bool IsUpdateChecked
    {
        get => _isUpdateChecked;
        set
        {
            if (value == _isUpdateChecked) return;
            _isUpdateChecked = value;
            OnPropertyChanged();
        }
    }

    public string DotNetVersion
    {
        get => _dotNetVersion;
        set
        {
            if (value == _dotNetVersion) return;
            _dotNetVersion = value;
            OnPropertyChanged();
        }
    }

    public string RuntimeVersion
    {
        get => _runtimeVersion;
        set
        {
            if (value == _runtimeVersion) return;
            _runtimeVersion = value;
            OnPropertyChanged();
        }
    }

    public IAsyncRelayCommand CheckUpdatesCommand { get; }
    public IAsyncRelayCommand DownloadCommand { get; }

    #region Updater Wrapping

    public SoftwareUpdateState State => _updateService.State;
    public string CurrentVersion => _updateService.CurrentVersion;
    public string NewVersion => _updateService.NewVersion;
    public string ErrorMessage => _updateService.ErrorMessage;
    public string ReleaseNotesUrl => _updateService.ReleaseNotesUrl;
    public string LatestCheckDate => _updateService.LatestCheckDate;

    #endregion


    private async Task CheckUpdates()
    {
        await _updateService.CheckUpdates();
        IsUpdateChecked = true;
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(NewVersion));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(LatestCheckDate));
        OnPropertyChanged(nameof(ReleaseNotesUrl));
    }

    private async Task DownloadUpdate()
    {
        await _updateService.DownloadUpdate();
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(ErrorMessage));
    }
}