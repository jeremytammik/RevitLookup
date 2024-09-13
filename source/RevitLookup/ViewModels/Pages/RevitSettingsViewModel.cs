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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Modules.Configuration;
using RevitLookup.Services;
using RevitLookup.ViewModels.ObservableObjects;
using RevitLookup.Views.Dialogs;
using Wpf.Ui.Controls;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class RevitSettingsViewModel(
    ILogger<RevitSettingsViewModel> logger,
    NotificationService notificationService,
    IServiceProvider serviceProvider)
    : ObservableObject
{
    [ObservableProperty] private ObservableRevitSettingsEntry _selectedEntry;
    [ObservableProperty] private List<ObservableRevitSettingsEntry> _entries;

    public async Task InitializeAsync()
    {
        try
        {
            Entries = await Task.Run(() =>
            {
                var configurator = new RevitConfigurator();
                return configurator.ParseSources();
            });
        }
        catch (Exception exception)
        {
            const string message = "Unavailable to parse Revit configuration";

            logger.LogError(exception, message);
            notificationService.ShowError(message, exception);
        }
    }

    [RelayCommand]
    private void DeleteEntry(ObservableRevitSettingsEntry entry)
    {
        Entries.Remove(entry);
    }

    async partial void OnSelectedEntryChanged(ObservableRevitSettingsEntry value)
    {
        var editingValue = value.Clone();
        var dialog = serviceProvider.GetRequiredService<EditSettingsEntryDialog>();

        try
        {
            var result = await dialog.ShowDialogAsync(editingValue);
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
        SelectedEntry.Category = entry.Category;
        SelectedEntry.Property = entry.Property;
        SelectedEntry.Value = entry.Value;
    }
}