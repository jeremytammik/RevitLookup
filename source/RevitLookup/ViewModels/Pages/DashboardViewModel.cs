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

using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Dialogs;
using RevitLookup.Views.Pages;
using Wpf.Ui;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class DashboardViewModel(
    INavigationService navigationService,
    ISnoopVisualService snoopVisualService,
    NotificationService notificationService,
    IServiceProvider serviceProvider)
    : ObservableObject, IDashboardViewModel
{
    [RelayCommand]
    private async Task NavigateSnoopPage(string parameter)
    {
        if (!Validate()) return;

        switch (parameter)
        {
            case "view":
                await snoopVisualService.SnoopAsync(SnoopableType.View);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "document":
                await snoopVisualService.SnoopAsync(SnoopableType.Document);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "application":
                await snoopVisualService.SnoopAsync(SnoopableType.Application);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "uiApplication":
                await snoopVisualService.SnoopAsync(SnoopableType.UiApplication);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "database":
                await snoopVisualService.SnoopAsync(SnoopableType.Database);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "dependents":
                await snoopVisualService.SnoopAsync(SnoopableType.DependentElements);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "selection":
                await snoopVisualService.SnoopAsync(SnoopableType.Selection);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "linked":
                await snoopVisualService.SnoopAsync(SnoopableType.LinkedElement);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "face":
                await snoopVisualService.SnoopAsync(SnoopableType.Face);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "edge":
                await snoopVisualService.SnoopAsync(SnoopableType.Edge);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "point":
                await snoopVisualService.SnoopAsync(SnoopableType.Point);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "subElement":
                await snoopVisualService.SnoopAsync(SnoopableType.SubElement);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "components":
                await snoopVisualService.SnoopAsync(SnoopableType.ComponentManager);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "performance":
                await snoopVisualService.SnoopAsync(SnoopableType.PerformanceAdviser);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "updaters":
                await snoopVisualService.SnoopAsync(SnoopableType.UpdaterRegistry);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "services":
                await snoopVisualService.SnoopAsync(SnoopableType.Services);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "schemas":
                await snoopVisualService.SnoopAsync(SnoopableType.Schemas);
                navigationService.Navigate(typeof(SnoopView));
                break;
            case "events":
                navigationService.Navigate(typeof(EventsView));
                break;
            case "revitConfig":
                navigationService.NavigateWithHierarchy(typeof(RevitConfigView));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(parameter), parameter);
        }
    }

    [RelayCommand]
    private async Task OpenDialog(string parameter)
    {
        if (!Validate()) return;
        
        try
        {
            switch (parameter)
            {
                case "parameters":
                    var unitsDialog = new UnitsDialog(serviceProvider);
                    await unitsDialog.ShowParametersAsync();
                    return;
                case "categories":
                    unitsDialog = new UnitsDialog(serviceProvider);
                    await unitsDialog.ShowCategoriesAsync();
                    return;
                case "forge":
                    unitsDialog = new UnitsDialog(serviceProvider);
                    await unitsDialog.ShowForgeSchemaAsync();
                    return;
                case "search":
                    var searchDialog = new SearchElementsDialog(serviceProvider);
                    await searchDialog.ShowAsync();
                    return;
                case "modules":
                    var modulesDialog = new ModulesDialog(serviceProvider);
                    await modulesDialog.ShowAsync();
                    return;
            }
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Failed open dialog", exception);
        }
    }

    private bool Validate()
    {
        if (Context.UiApplication is null) return true;
        if (Context.UiDocument is not null) return true;

        notificationService.ShowWarning("Request denied", "There are no open documents");
        return false;
    }
}