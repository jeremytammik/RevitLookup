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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Dialogs;
using RevitLookup.Views.Pages;
using Wpf.Ui;

namespace RevitLookup.UI.Demo.Mock;

public partial class MockDashboardViewModel(
    INavigationService navigationService,
    ISnoopVisualService snoopVisualService,
    IServiceProvider serviceProvider)
    : ObservableObject, IDashboardViewModel
{
    [RelayCommand]
    private async Task NavigateSnoopPage(string parameter)
    {
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
            default:
                throw new ArgumentOutOfRangeException(nameof(parameter), parameter);
        }
    }

    [RelayCommand]
    private Task OpenDialog(string parameter)
    {
        switch (parameter)
        {
            case "parameters":
                var unitsDialog = new UnitsDialog(serviceProvider);
                return unitsDialog.ShowParametersAsync();
            case "categories":
                unitsDialog = new UnitsDialog(serviceProvider);
                return unitsDialog.ShowCategoriesAsync();
            case "forge":
                unitsDialog = new UnitsDialog(serviceProvider);
                return unitsDialog.ShowForgeSchemaAsync();
            case "search":
                var searchDialog = new SearchElementsDialog(serviceProvider);
                return searchDialog.ShowAsync();
            case "modules":
                var modulesDialog = new ModulesDialog(serviceProvider);
                return modulesDialog.ShowAsync();
        }

        return Task.CompletedTask;
    }
}