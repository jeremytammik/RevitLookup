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
using RevitLookup.Services.Enums;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.ViewModels.Pages;

public sealed class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    private string _searchText;
    private readonly INavigationService _navigationService;
    private IReadOnlyList<ISnoopableObject> _snoopableObjects = Array.Empty<ISnoopableObject>();
    private IReadOnlyList<ISnoopableObject> _snoopableData = Array.Empty<ISnoopableObject>();
    private IReadOnlyList<ISnoopableObject> _filteredSnoopableObjects = Array.Empty<ISnoopableObject>();

    public SnoopViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
        RefreshCommand = new RelayCommand<object>(Refresh);
    }

    public IReadOnlyList<ISnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            if (Equals(value, _snoopableObjects)) return;
            _snoopableObjects = value;
            SearchText = string.Empty;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<ISnoopableObject> FilteredSnoopableObjects
    {
        get => _filteredSnoopableObjects;
        set
        {
            if (Equals(value, _filteredSnoopableObjects)) return;
            _filteredSnoopableObjects = value;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<ISnoopableObject> SnoopableData
    {
        get => _snoopableData;
        set
        {
            if (Equals(value, _snoopableData)) return;
            _snoopableData = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            UpdateSearchResults(value);
        }
    }

    public RelayCommand SnoopSelectionCommand { get; }
    public RelayCommand<object> RefreshCommand { get; }

    public void SnoopSelection()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Selection);
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
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = Snooper.Snoop(SnoopableType.Edge);
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopFace()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = Snooper.Snoop(SnoopableType.Face);
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopLinkedElement()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = Snooper.Snoop(SnoopableType.LinkedElement);
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.DependentElements);
    }

    private void Refresh(object param)
    {
        if (param is null)
        {
            SnoopableData = Array.Empty<ISnoopableObject>();
            return;
        }

        if (param is not ISnoopableObject snoopableObject) return;
        SnoopableData = snoopableObject.GetMembers();
    }

    private void UpdateSearchResults(string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
        {
            FilteredSnoopableObjects = SnoopableObjects;
            return;
        }

        var formattedText = searchText.ToLower().Trim();
        var searchResults = new List<ISnoopableObject>(SnoopableObjects.Count);
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var snoopableObject in SnoopableObjects)
            if (snoopableObject.Descriptor.Label.ToLower().Contains(formattedText))
                searchResults.Add(snoopableObject);

        FilteredSnoopableObjects = searchResults;
    }
}