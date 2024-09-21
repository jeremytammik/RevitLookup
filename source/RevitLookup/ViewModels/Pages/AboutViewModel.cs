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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RevitLookup.Models.Options;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.Views.Dialogs;
using Wpf.Ui;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class AboutViewModel(
    ISoftwareUpdateService updateService,
    IServiceProvider serviceProvider,
    IOptions<AssemblyInfo> assemblyOptions)
    : ObservableObject
{
    [ObservableProperty] private bool _isUpdateChecked;
    [ObservableProperty] private SoftwareUpdateState _state;
    [ObservableProperty] private Version _currentVersion = assemblyOptions.Value.AddinVersion;
    [ObservableProperty] private string _newVersion;
    [ObservableProperty] private string _errorMessage;
    [ObservableProperty] private string _releaseNotesUrl;
    [ObservableProperty] private string _latestCheckDate;

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
        await updateService.CheckUpdatesAsync();
        IsUpdateChecked = true;

        State = updateService.State;
        NewVersion = updateService.NewVersion;
        ErrorMessage = updateService.ErrorMessage;
        ReleaseNotesUrl = updateService.ReleaseNotesUrl;
        LatestCheckDate = updateService.LatestCheckDate;
    }

    [RelayCommand]
    private async Task DownloadUpdateAsync()
    {
        await updateService.DownloadUpdate();

        State = updateService.State;
        ErrorMessage = updateService.ErrorMessage;
    }

    [RelayCommand]
    private Task ShowSoftwareDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<OpenSourceDialog>();
        return dialog.ShowAsync();
    }
}