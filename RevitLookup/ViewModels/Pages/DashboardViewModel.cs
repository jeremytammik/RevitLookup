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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Contracts;
using RevitLookup.Views.Pages;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class DashboardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ISnoopService _snoopService;

    public DashboardViewModel(ISnoopService snoopService, INavigationService navigationService)
    {
        _snoopService = snoopService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task NavigateSnoopPage(string parameter)
    {
        _navigationService.Navigate(typeof(SnoopView));
        await Task.Delay(300);
        switch (parameter)
        {
            case "selection":
                _snoopService.SnoopSelection();
                break;
            case "document":
                _snoopService.SnoopDocument();
                break;
            case "database":
                _snoopService.SnoopDatabase();
                break;
            case "view":
                _snoopService.SnoopView();
                break;
            case "application":
                _snoopService.SnoopApplication();
                break;
            case "linked":
                _snoopService.SnoopLinkedElement();
                break;
            case "dependents":
                _snoopService.SnoopDependentElements();
                break;
            case "face":
                _snoopService.SnoopFace();
                break;
            case "edge":
                _snoopService.SnoopEdge();
                break;
            case "eventMonitor":
                break;
        }
    }
}