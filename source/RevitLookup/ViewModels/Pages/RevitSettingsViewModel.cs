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

using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Modules.Configuration;
using RevitLookup.Services;
using RevitLookup.Utils;
using RevitLookup.ViewModels.ObservableObjects;
using RevitLookup.Views.Dialogs;
using Wpf.Ui.Controls;

namespace RevitLookup.ViewModels.Pages;

#nullable enable
public sealed partial class RevitSettingsViewModel(
    ILogger<RevitSettingsViewModel> logger,
    NotificationService notificationService,
    IServiceProvider serviceProvider)
    : ObservableObject
{
    private TaskNotifier<List<ObservableRevitSettingsEntry>>? _initializationTask;

    [ObservableProperty] private ObservableRevitSettingsEntry? _selectedEntry;
    
    [ObservableProperty] private List<ObservableRevitSettingsEntry> _entries = [];
    [ObservableProperty] private ObservableCollection<ObservableRevitSettingsEntry> _filteredEntries = [];

    public Task<List<ObservableRevitSettingsEntry>>? InitializationTask
    {
        get => _initializationTask!;
        private set => SetPropertyAndNotifyOnCompletion(ref _initializationTask, value);
    }

    public async Task InitializeAsync()
    {
        try
        {
            InitializationTask = Task.Run(async () =>
            {
                // Smooth loading.
                // But we can decide to add Async binding to avoid grouping lags.
                await Task.Delay(500).ConfigureAwait(false);
                
                var configurator = new RevitConfigurator();
                return configurator.ParseSources();
            });

            Entries = await InitializationTask;
        }
        catch (Exception exception)
        {
            const string message = "Unavailable to parse Revit configuration";

            logger.LogError(exception, message);
            notificationService.ShowError(message, exception);
        }
    }

    [RelayCommand]
    private async Task CreateEntry()
    {
        var dialog = serviceProvider.GetRequiredService<EditSettingsEntryDialog>();
        var result = await dialog.ShowCreateDialogAsync();
        if (result == ContentDialogResult.Primary)
        {
            //TODO add to ini
            Entries.Add(dialog.Entry);
        }
    }

    [RelayCommand]
    private void DeleteEntry(ObservableRevitSettingsEntry entry)
    {
        //TODO remove from ini
        Entries.Remove(entry);
    }

    [RelayCommand]
    private void ShowHelp()
    {
        var version = Context.Application.VersionNumber;
        ProcessTasks.StartShell($"https://help.autodesk.com/view/RVT/{version}/ENU/?guid=GUID-9ECD669E-81D3-43E5-9970-9FA1C38E8507");
    }

    [RelayCommand]
    private void OpenSettings()
    {
        var iniFile = Context.Application.CurrentUsersDataFolderPath.AppendPath("Revit.ini");
        if (!File.Exists(iniFile))
        {
            notificationService.ShowWarning("Missing settings", "Revit.ini file does not exists");
        }

        ProcessTasks.StartShell(iniFile);
    }


    partial void OnEntriesChanged(List<ObservableRevitSettingsEntry>? value)
    {
        if (value is null) return;
        FilteredEntries = new ObservableCollection<ObservableRevitSettingsEntry>(value);
    }

    public async Task UpdateEntryAsync()
    {
        if (SelectedEntry is null) return;

        try
        {
            var editingValue = SelectedEntry.Clone();
            var dialog = serviceProvider.GetRequiredService<EditSettingsEntryDialog>();
            var result = await dialog.ShowUpdateDialogAsync(editingValue);
            if (result == ContentDialogResult.Primary) UpdateEntry(editingValue);
        }
        catch (Exception exception)
        {
            const string message = "Unavailable to update Revit configuration";

            logger.LogError(exception, message);
            notificationService.ShowError(message, exception);
        }
    }

    private void UpdateEntry(ObservableRevitSettingsEntry entry)
    {
        if (SelectedEntry is null) return;

        SelectedEntry.Category = entry.Category;
        SelectedEntry.Property = entry.Property;
        SelectedEntry.Value = entry.Value;
    }
}