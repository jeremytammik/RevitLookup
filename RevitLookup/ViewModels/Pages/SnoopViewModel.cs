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
using RevitLookup.Core;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Objects;
using RevitLookup.Views.Pages;

namespace RevitLookup.ViewModels.Pages;

public sealed class SnoopViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public SnoopViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
    }

    private IReadOnlyList<SnoopableObject> _snoopableObjects;

    public IReadOnlyList<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            if (Equals(value, _snoopableObjects)) return;
            _snoopableObjects = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand SnoopSelectionCommand { get; }

    public void SnoopSelection()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Selection);
        _navigationService.GetNavigationWindow().Focus();
    }

    public void SnoopApplication()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Application);
    }

    public void SnoopDocument()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Document);
    }

    public void SnoopView()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.View);
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Database);
    }

    public void SnoopEdge()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Edge);
        _navigationService.GetNavigationWindow().Focus();
    }

    public void SnoopFace()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Face);
        _navigationService.GetNavigationWindow().Focus();
    }

    public void SnoopLinkedElement()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.LinkedElement);
        _navigationService.GetNavigationWindow().Focus();
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.DependentElements);
    }
}