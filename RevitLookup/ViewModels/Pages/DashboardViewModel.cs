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
using RevitLookup.Core;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.Views.Pages;
using Wpf.Ui;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class DashboardViewModel : ObservableObject
{
    private readonly IContentDialogService _dialogService;
    private readonly NotificationService _notificationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly ISnoopVisualService _snoopVisualService;

    public DashboardViewModel(
        INavigationService navigationService,
        ISnoopVisualService snoopVisualService,
        IContentDialogService dialogService,
        NotificationService notificationService,
        IServiceProvider serviceProvider)
    {
        _navigationService = navigationService;
        _snoopVisualService = snoopVisualService;
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
        _notificationService = notificationService;
    }

    [RelayCommand]
    private async Task NavigateSnoopPage(string parameter)
    {
        if (!Validate()) return;

        switch (parameter)
        {
            case "view":
                await _snoopVisualService.SnoopAsync(SnoopableType.View);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "document":
                await _snoopVisualService.SnoopAsync(SnoopableType.Document);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "application":
                await _snoopVisualService.SnoopAsync(SnoopableType.Application);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "uiApplication":
                await _snoopVisualService.SnoopAsync(SnoopableType.UiApplication);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "database":
                await _snoopVisualService.SnoopAsync(SnoopableType.Database);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "dependents":
                await _snoopVisualService.SnoopAsync(SnoopableType.DependentElements);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "selection":
                await _snoopVisualService.SnoopAsync(SnoopableType.Selection);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "linked":
                await _snoopVisualService.SnoopAsync(SnoopableType.LinkedElement);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "face":
                await _snoopVisualService.SnoopAsync(SnoopableType.Face);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "edge":
                await _snoopVisualService.SnoopAsync(SnoopableType.Edge);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "point":
                await _snoopVisualService.SnoopAsync(SnoopableType.Point);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "subElement":
                await _snoopVisualService.SnoopAsync(SnoopableType.SubElement);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "components":
                await _snoopVisualService.SnoopAsync(SnoopableType.ComponentManager);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "performance":
                await _snoopVisualService.SnoopAsync(SnoopableType.PerformanceAdviser);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "updaters":
                await _snoopVisualService.SnoopAsync(SnoopableType.UpdaterRegistry);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "services":
                await _snoopVisualService.SnoopAsync(SnoopableType.Services);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "schemas":
                await _snoopVisualService.SnoopAsync(SnoopableType.Schemas);
                _navigationService.Navigate(typeof(SnoopView));
                break;
            case "events":
                _navigationService.Navigate(typeof(EventsView));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(parameter), parameter);
        }
    }

    [RelayCommand]
    private async Task OpenDialog(string parameter)
    {
        if (!Validate()) return;

        // var dialog = _dialogService.CreateDialog();
        // switch (parameter)
        // {
        //     case "parameters":
        //         var units = await Task.Run(() => RevitApi.GetUnitInfos(typeof(BuiltInParameter)));
        //         dialog.Title = "BuiltIn Parameters";
        //         dialog.Content = new UnitsDialog(_serviceProvider, units);
        //         dialog.DialogWidth = 800;
        //         dialog.DialogHeight = 600;
        //         await dialog.ShowAsync();
        //         break;
        //     case "categories":
        //         units = await Task.Run(() => RevitApi.GetUnitInfos(typeof(BuiltInCategory)));
        //         dialog.Title = "BuiltIn Categories";
        //         dialog.Content = new UnitsDialog(_serviceProvider, units);
        //         dialog.DialogWidth = 800;
        //         dialog.DialogHeight = 600;
        //         await dialog.ShowAsync();
        //         break;
        //     case "forge":
        //         units = await Task.Run(() => RevitApi.GetUnitInfos(typeof(ForgeTypeId)));
        //         dialog.Title = "Forge Schema";
        //         dialog.Content = new UnitsDialog(_serviceProvider, units);
        //         dialog.DialogWidth = 800;
        //         dialog.DialogHeight = 600;
        //         await dialog.ShowAsync();
        //         break;
        //     case "search":
        //         dialog = new SearchElementsDialog(_serviceProvider, _dialogService.GetContentPresenter());
        //         dialog.DialogWidth = 570;
        //         dialog.DialogHeight = 330;
        //         await dialog.ShowAsync();
        //         break;
        // }
    }

    private bool Validate()
    {
        if (RevitApi.UiApplication is null) return true;
        if (RevitApi.UiDocument is not null) return true;

        _notificationService.ShowWarning("Request denied", "There are no open documents");
        return false;
    }
}