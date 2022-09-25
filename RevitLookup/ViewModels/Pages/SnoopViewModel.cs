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

using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Core;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.ViewModels.Pages;

public sealed class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly INavigationService _navigationService;
    private ObservableCollection<SnoopableObject> _snoopableObjects;

    public event EventHandler SelectionChanged;

    public SnoopViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
    }

    public ObservableCollection<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            if (Equals(value, _snoopableObjects)) return;
            _snoopableObjects = value;
            OnPropertyChanged();
        }
    }

    public ICommand SnoopSelectionCommand { get; }

    public void SnoopSelection()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
    }

    public void SnoopApplication()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
    }

    public void SnoopDocument()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
    }

    public void SnoopView()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
    }

    public void SnoopEdge()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopFace()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopLinkedElement()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = new ObservableCollection<SnoopableObject>(Snooper.Snoop(SnoopableType.Selection));
    }

    private ICommand _snoopObjectCommand;

    [UsedImplicitly]
    public ICommand SnoopObjectCommand => _snoopObjectCommand ??= new RelayCommand(() =>
    {
        
    });
}