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
using RevitLookup.Services.Enums;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.Views.Pages;

namespace RevitLookup.ViewModels.Pages;

public sealed class DashboardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ISnoopService _snoopService;

    public DashboardViewModel(INavigationService navigationService, ISnoopService snoopService)
    {
        _navigationService = navigationService;
        _snoopService = snoopService;
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
        SnoopApplicationCommand = new RelayCommand(SnoopApplication);
        SnoopDocumentCommand = new RelayCommand(SnoopDocument);
        SnoopViewCommand = new RelayCommand(SnoopView);
        SnoopDatabaseCommand = new RelayCommand(SnoopDatabase);
        SnoopEdgeCommand = new RelayCommand(SnoopEdge);
        SnoopFaceCommand = new RelayCommand(SnoopFace);
        SnoopDependentElementsCommand = new RelayCommand(SnoopDependentElements);
        SnoopLinkedElementCommand = new RelayCommand(SnoopLinkedElement);
    }

    public RelayCommand SnoopSelectionCommand { get; }
    public RelayCommand SnoopApplicationCommand { get; }
    public RelayCommand SnoopDocumentCommand { get; }
    public RelayCommand SnoopViewCommand { get; }
    public RelayCommand SnoopDatabaseCommand { get; }
    public RelayCommand SnoopEdgeCommand { get; }
    public RelayCommand SnoopFaceCommand { get; }
    public RelayCommand SnoopDependentElementsCommand { get; }
    public RelayCommand SnoopLinkedElementCommand { get; }

    private void SnoopSelection()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.Selection);
    }

    private void SnoopApplication()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.Application);
    }

    private void SnoopDocument()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.Document);
    }

    private void SnoopView()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.View);
    }

    private void SnoopDatabase()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.Database);
    }

    private void SnoopEdge()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.Edge);
    }

    private void SnoopFace()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.Face);
    }

    private void SnoopDependentElements()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.DependentElements);
    }

    private void SnoopLinkedElement()
    {
        _navigationService.Navigate(typeof(SnoopView));
        _snoopService.Snoop(SnoopableType.LinkedElement);
    }
}