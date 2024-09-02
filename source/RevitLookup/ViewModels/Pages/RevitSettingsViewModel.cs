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

using System.IO;
using System.Text;
using Autodesk.Revit.DB.ExternalService;
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Modules.RevitSettings;
using RevitLookup.Services;
using RevitLookup.ViewModels.ObservableObjects;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class RevitSettingsViewModel : ObservableObject
{
    private readonly ILogger<RevitSettingsViewModel> _logger;
    private readonly NotificationService _notificationService;
    [ObservableProperty] private List<ObservableRevitSettingsEntry> _entries;

    public RevitSettingsViewModel(ILogger<RevitSettingsViewModel> logger, NotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            Entries = await Task.Run(() =>
            {
                var configurator = new RevitConfigurator();
                return configurator.GetRevitConfiguration();
            });
        }
        catch (Exception exception)
        {
            const string message = "Unavailable to parse Revit configuration";
            
            _logger.LogError(exception, message);
            _notificationService.ShowError(message, exception);
        }
    }

    [RelayCommand]
    private void DeleteEntry(ObservableRevitSettingsEntry entry)
    {
        Entries.Remove(entry);
    }
}