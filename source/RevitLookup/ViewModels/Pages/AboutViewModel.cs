// Copyright 2003-2024 by Autodesk, Inc.
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

using System.Runtime;
using System.Text;
using Microsoft.Extensions.Options;
using RevitLookup.Models.Options;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.Views.Dialogs;
using Wpf.Ui;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class AboutViewModel(
    ISoftwareUpdateService updateService,
    IContentDialogService dialogService,
    IOptions<AssemblyInfo> assemblyOptions)
    : ObservableObject
{
    [ObservableProperty] private bool _isUpdateChecked;

    [ObservableProperty] private string _runtime = new StringBuilder()
        .Append(assemblyOptions.Value.Framework)
        .Append(' ')
        .Append(Environment.Is64BitProcess ? "x64" : "x86")
        .Append(" (")
        .Append(GCSettings.IsServerGC ? "Server" : "Workstation")
        .Append(" GC)")
        .ToString();

    [RelayCommand]
    private async Task CheckUpdatesAsync()
    {
        await updateService.CheckUpdates();
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
        await updateService.DownloadUpdate();
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(ErrorMessage));
    }
    
    [RelayCommand]
    private Task ShowSoftwareDialogAsync()
    {
        var openSourceDialog = new OpenSourceDialog(dialogService);
        return openSourceDialog.ShowAsync();
    }
    
    #region Updater Wrapping
    
    public SoftwareUpdateState State => updateService.State;
    public Version CurrentVersion => updateService.CurrentVersion;
    public string NewVersion => updateService.NewVersion;
    public string ErrorMessage => updateService.ErrorMessage;
    public string ReleaseNotesUrl => updateService.ReleaseNotesUrl;
    public string LatestCheckDate => updateService.LatestCheckDate;
    
    #endregion
}