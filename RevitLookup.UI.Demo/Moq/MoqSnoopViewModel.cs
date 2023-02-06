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

using Autodesk.Revit.DB;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Core.Utils;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Common;
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Enums;
using RevitLookup.ViewModels.Search;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Demo.Moq;

public sealed partial class MoqSnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    [ObservableProperty] private IReadOnlyList<Descriptor> _filteredSnoopableData;
    [ObservableProperty] private IReadOnlyList<SnoopableObject> _filteredSnoopableObjects;
    private string _searchText;
    private IReadOnlyList<Descriptor> _snoopableData;
    private IReadOnlyList<SnoopableObject> _snoopableObjects;

    public MoqSnoopViewModel(ISnackbarService snackbarService, INavigationService navigationService)
    {
        _snackbarService = snackbarService;
        _navigationService = navigationService;
    }

    public SnoopableObject SelectedObject { get; set; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            UpdateSearchResults(SearchOption.Objects);
        }
    }

    public IReadOnlyList<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            SelectedObject = null;
            _snoopableObjects = value;
            UpdateSearchResults(SearchOption.Objects);
            OnPropertyChanged();
            TreeSourceChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public IReadOnlyList<Descriptor> SnoopableData
    {
        get => _snoopableData;
        private set
        {
            SetProperty(ref _snoopableData, value);
            UpdateSearchResults(SearchOption.Selection);
        }
    }

    public event EventHandler TreeSourceChanged;
    public event EventHandler SearchResultsChanged;

    public void Snoop(SnoopableObject snoopableObject)
    {
        if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptor)
            SnoopableObjects = descriptor.ParseEnumerable(snoopableObject);
        else
            SnoopableObjects = new[] {snoopableObject};
        
        _navigationService.Navigate(typeof(SnoopView));
    }

    public async Task Snoop(SnoopableType snoopableType)
    {
        int generationCount;
        switch (snoopableType)
        {
            case SnoopableType.Selection:
                generationCount = 10_000;
                break;
            case SnoopableType.View:
                generationCount = 1_000;
                break;
            case SnoopableType.Document:
                generationCount = 100;
                break;
            case SnoopableType.Application:
                generationCount = 50;
                break;
            case SnoopableType.Database:
                generationCount = 10;
                break;
            case SnoopableType.LinkedElement:
                generationCount = 5;
                break;
            case SnoopableType.Face:
                generationCount = 2;
                break;
            case SnoopableType.Edge:
                generationCount = 1;
                break;
            case SnoopableType.DependentElements:
                generationCount = 1;
                break;
            case SnoopableType.ComponentManager:
                generationCount = 1;
                break;
            case SnoopableType.PerformanceAdviser:
                generationCount = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(snoopableType), snoopableType, null);
        }

        SnoopableObjects = generationCount == 0
            ? Array.Empty<SnoopableObject>()
            : await Task.Run(() =>
            {
                return new Faker<SnoopableObject>()
                    .CustomInstantiator(faker =>
                    {
                        if (faker.IndexFaker == 0)
                            return new SnoopableObject(null, faker.Lorem.Word());
                        if (faker.IndexFaker % 1000 == 0)
                            return new SnoopableObject(null, faker.Make(150, () => faker.Internet.UserName()));
                        if (faker.IndexFaker % 700 == 0)
                            return new SnoopableObject(typeof(Console));
                        if (faker.IndexFaker % 500 == 0)
                            return new SnoopableObject(null, null);
                        if (faker.IndexFaker % 200 == 0)
                            return new SnoopableObject(null, string.Empty);
                        if (faker.IndexFaker % 100 == 0)
                            return new SnoopableObject(null, new Color(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte()));
                        if (faker.IndexFaker % 5 == 0)
                            return new SnoopableObject(null, faker.Random.Int(0));
                        if (faker.IndexFaker % 3 == 0)
                            return new SnoopableObject(null, faker.Random.Bool());

                        return new SnoopableObject(null, faker.Lorem.Word());
                    })
                    .Generate(generationCount);
            });

        SnoopableData = Array.Empty<Descriptor>();
        _navigationService.Navigate(typeof(SnoopView));
    }

    public void Navigate(Descriptor selectedItem)
    {
        if (selectedItem.Value.Descriptor is not IDescriptorCollector or IDescriptorEnumerator {IsEmpty: true}) return;

        var window = Host.GetService<IWindow>();
        window.Show();
        window.Scope.GetService<ISnoopService>()!.Snoop(selectedItem.Value);
    }

    private void UpdateSearchResults(SearchOption option)
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            FilteredSnoopableObjects = SnoopableObjects;
            FilteredSnoopableData = SnoopableData;
        }
        else
        {
            SearchEngine.SearchAsync(this, option);
        }

        SearchResultsChanged?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task CollectMembersAsync()
    {
        if (SelectedObject is null)
        {
            SnoopableData = Array.Empty<Descriptor>();
            return;
        }

        try
        {
            // ReSharper disable once MethodHasAsyncOverload
            SnoopableData = SelectedObject.GetMembers();
        }
        catch (Exception exception)
        {
            await _snackbarService.ShowAsync("Snoop engine error", exception.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
        }
    }
}