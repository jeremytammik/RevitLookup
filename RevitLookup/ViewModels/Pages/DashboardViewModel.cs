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

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.Views.Pages;

namespace RevitLookup.ViewModels.Pages;

public sealed class DashboardViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        NavigateSnoopCommand = new RelayCommand<string>(NavigateSnoopPage);
    }

    public ICommand NavigateSnoopCommand { get; }

    private void NavigateSnoopPage(string parameter)
    {
        var snoopService = _serviceProvider.GetService<ISnoopService>()!;
        var navigationService = _serviceProvider.GetService<INavigationService>()!;
        navigationService.Navigate(typeof(SnoopView));
        switch (parameter)
        {
            case "selection":
                snoopService.SnoopSelection();
                break;
            case "document":
                snoopService.SnoopDocument();
                break;
            case "database":
                snoopService.SnoopDatabase();
                break;
            case "view":
                snoopService.SnoopView();
                break;
            case "application":
                snoopService.SnoopApplication();
                break;
            case "linked":
                snoopService.SnoopLinkedElement();
                break;
            case "dependents":
                snoopService.SnoopDependentElements();
                break;
            case "face":
                snoopService.SnoopFace();
                break;
            case "edge":
                snoopService.SnoopEdge();
                break;
        }
    }
}