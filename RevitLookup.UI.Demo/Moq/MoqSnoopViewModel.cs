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
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Enums;
using RevitLookup.ViewModels.Utils;
using RevitLookup.Views.Pages;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Demo.Moq;

public sealed partial class MoqSnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    private CancellationTokenSource _searchCancellationToken = new();
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _filteredSnoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _snoopableData;
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _filteredSnoopableData;

    public MoqSnoopViewModel(ISnackbarService snackbarService, INavigationService navigationService)
    {
        _snackbarService = snackbarService;
        _navigationService = navigationService;
    }

    public SnoopableObject SelectedObject { get; set; }
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
            case SnoopableType.View:
                generationCount = 50_000;
                break;
            case SnoopableType.Document:
                generationCount = 10_000;
                break;
            case SnoopableType.Application:
                generationCount = 5_000;
                break;
            case SnoopableType.UiApplication:
                generationCount = 1_000;
                break;
            case SnoopableType.Database:
                generationCount = 500;
                break;
            case SnoopableType.DependentElements:
                generationCount = 100;
                break;
            case SnoopableType.Selection:
                generationCount = 50;
                break;
            case SnoopableType.LinkedElement:
                generationCount = 10;
                break;
            case SnoopableType.Face:
                generationCount = 5;
                break;
            case SnoopableType.Edge:
                generationCount = 3;
                break;
            case SnoopableType.Point:
                generationCount = 2;
                break;
            case SnoopableType.SubElement:
                generationCount = 1;
                break;
            case SnoopableType.ComponentManager:
            case SnoopableType.PerformanceAdviser:
            case SnoopableType.UpdaterRegistry:
            case SnoopableType.Services:
            case SnoopableType.Schemas:
            case SnoopableType.Events:
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

    async partial void OnSearchTextChanged(string value)
    {
        await UpdateSearchResults(SearchOption.Objects);
        SearchResultsChanged?.Invoke(this, EventArgs.Empty);
    }

    async partial void OnSnoopableObjectsChanged(IReadOnlyCollection<SnoopableObject> value)
    {
        SelectedObject = null;
        await UpdateSearchResults(SearchOption.Objects);
        TreeSourceChanged?.Invoke(this, EventArgs.Empty);
    }

    async partial void OnSnoopableDataChanged(IReadOnlyCollection<Descriptor> value)
    {
        await UpdateSearchResults(SearchOption.Selection);
    }

    private async Task UpdateSearchResults(SearchOption option)
    {
        _searchCancellationToken.Cancel();
        _searchCancellationToken = new CancellationTokenSource();

        if (string.IsNullOrEmpty(SearchText))
        {
            FilteredSnoopableObjects = SnoopableObjects;
            FilteredSnoopableData = SnoopableData;
            return;
        }

        try
        {
            var results = await SearchEngine.SearchAsync(this, option, _searchCancellationToken.Token);
            if (results.Data is not null) FilteredSnoopableData = results.Data;
            if (results.Objects is not null) FilteredSnoopableObjects = results.Objects;
        }
        catch (OperationCanceledException)
        {
            //Ignored
        }
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

    [RelayCommand]
    private async Task RefreshMembers()
    {
        await CollectMembersAsync();
    }
}