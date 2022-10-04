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

using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.UI.Tests.Moq;

public sealed class MoqSnoopViewModel : ObservableObject, ISnoopViewModel
{
    private IReadOnlyList<SnoopableObject> _filteredSnoopableObjects;
    private string _searchText;
    private IReadOnlyList<SnoopableObject> _snoopableData;
    private IReadOnlyList<SnoopableObject> _snoopableObjects;

    public MoqSnoopViewModel()
    {
        SnoopSelectionCommand = new RelayCommand(SnoopSelection);
        RefreshCommand = new RelayCommand<SnoopableObject>(Refresh);
    }

    public IReadOnlyList<SnoopableObject> SnoopableObjects
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

    public IReadOnlyList<SnoopableObject> FilteredSnoopableObjects
    {
        get => _filteredSnoopableObjects;
        private set
        {
            if (Equals(value, _filteredSnoopableObjects)) return;
            _filteredSnoopableObjects = value;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<SnoopableObject> SnoopableData
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
            if (value == _searchText) return;
            _searchText = value;
            OnPropertyChanged();
            UpdateSearchResults(value);
        }
    }

    public RelayCommand SnoopSelectionCommand { get; }
    public RelayCommand<SnoopableObject> RefreshCommand { get; }

    public void SnoopSelection()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopApplication()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopDocument()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopView()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopEdge()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopFace()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopLinkedElement()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    private void Refresh(SnoopableObject obj)
    {
        if (obj is null)
        {
            SnoopableData = Array.Empty<SnoopableObject>();
            return;
        }

        SnoopableData = new Faker<SnoopableObject>()
            .CustomInstantiator(faker => new SnoopableObject(faker.Lorem.Word()))
            .Generate(100);
    }

    private void UpdateSearchResults(string searchText)
    {
        Task.Run(() =>
        {
            if (string.IsNullOrEmpty(searchText))
            {
                FilteredSnoopableObjects = SnoopableObjects;
                return;
            }

            var formattedText = searchText.ToLower().Trim();
            var searchResults = new List<SnoopableObject>(SnoopableObjects.Count);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var snoopableObject in SnoopableObjects)
                if (snoopableObject.Descriptor.Label.ToLower().Contains(formattedText))
                    searchResults.Add(snoopableObject);

            FilteredSnoopableObjects = searchResults;
        });
    }
}