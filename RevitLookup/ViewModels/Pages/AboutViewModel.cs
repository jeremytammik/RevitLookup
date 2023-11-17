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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.Views.Dialogs;
using Wpf.Ui;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class AboutViewModel : ObservableObject
{
    private readonly ISoftwareUpdateService _updateService;
    private readonly IContentDialogService _dialogService;

    [ObservableProperty] private string _dotNetVersion;
    [ObservableProperty] private bool _isUpdateChecked;
    [ObservableProperty] private string _runtimeVersion;

    public AboutViewModel(ISoftwareUpdateService updateService, IContentDialogService dialogService, IConfiguration configuration)
    {
        _updateService = updateService;
        _dialogService = dialogService;
        _dotNetVersion = configuration.GetValue<string>("Framework");
        _runtimeVersion = Environment.Version.ToString();
    }

    [RelayCommand]
    private async Task CheckUpdatesAsync()
    {
        await _updateService.CheckUpdates();
        IsUpdateChecked = true;
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(NewVersion));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(LatestCheckDate));
        OnPropertyChanged(nameof(ReleaseNotesUrl));
    }

    [RelayCommand]
    private async Task DownloadUpdateAsync()
    {
        await _updateService.DownloadUpdate();
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    [RelayCommand]
    private Task ShowSoftwareDialogAsync()
    {
        var openSourceDialog = new OpenSourceDialog(_dialogService);
        return openSourceDialog.ShowAsync();
    }

    #region Updater Wrapping

    public SoftwareUpdateState State => _updateService.State;
    public string CurrentVersion => _updateService.CurrentVersion;
    public string NewVersion => _updateService.NewVersion;
    public string ErrorMessage => _updateService.ErrorMessage;
    public string ReleaseNotesUrl => _updateService.ReleaseNotesUrl;
    public string LatestCheckDate => _updateService.LatestCheckDate;

    #endregion
}