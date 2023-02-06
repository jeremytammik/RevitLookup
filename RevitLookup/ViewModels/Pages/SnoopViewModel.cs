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
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core;
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
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    private readonly IWindowController _windowController;
    [ObservableProperty] private IReadOnlyList<Descriptor> _filteredSnoopableData;
    [ObservableProperty] private IReadOnlyList<SnoopableObject> _filteredSnoopableObjects;
    private string _searchText;
    private IReadOnlyList<Descriptor> _snoopableData;
    private IReadOnlyList<SnoopableObject> _snoopableObjects;

    public SnoopViewModel(IWindowController windowController, INavigationService navigationService, ISnackbarService snackbarService)
    {
        _windowController = windowController;
        _navigationService = navigationService;
        _snackbarService = snackbarService;
    }

    public SnoopableObject SelectedObject { get; set; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            var unused = UpdateSearchResults(SearchOption.Objects);
        }
    }

    public IReadOnlyList<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            SelectedObject = null;
            SetProperty(ref _snoopableObjects, value);
            UpdateObjectSource(value);

            async void UpdateObjectSource(IReadOnlyList<SnoopableObject> value)
            {
                await UpdateSearchResults(SearchOption.Objects);
                TreeSourceChanged?.Invoke(this, value);
            }
        }
    }

    public IReadOnlyList<Descriptor> SnoopableData
    {
        get => _snoopableData;
        private set
        {
            SetProperty(ref _snoopableData, value);
            var unused = UpdateSearchResults(SearchOption.Selection);
        }
    }

    public event EventHandler<IReadOnlyList<SnoopableObject>> TreeSourceChanged;
    public event EventHandler SearchResultsChanged;

    public void Snoop(SnoopableObject snoopableObject)
    {
        if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptor)
            SnoopableObjects = descriptor.ParseEnumerable(snoopableObject);
        else
            SnoopableObjects = new[] {snoopableObject};
    }

    public async Task Snoop(SnoopableType snoopableType)
    {
        if (!Validate()) return;

        try
        {
            SnoopableObjects = await Application.ExternalElementHandler.RaiseAsync(_ =>
            {
                switch (snoopableType)
                {
                    case SnoopableType.Application:
                    case SnoopableType.Document:
                    case SnoopableType.View:
                    case SnoopableType.Selection:
                    case SnoopableType.Database:
                    case SnoopableType.DependentElements:
                    case SnoopableType.ComponentManager:
                    case SnoopableType.PerformanceAdviser:
                        return Selector.Snoop(snoopableType);
                    case SnoopableType.Face:
                    case SnoopableType.Edge:
                    case SnoopableType.LinkedElement:
                        _windowController.Hide();
                        try
                        {
                            return Selector.Snoop(snoopableType);
                        }
                        finally
                        {
                            _windowController.Show();
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(snoopableType));
                }
            });

            SnoopableData = Array.Empty<Descriptor>();
            _navigationService.Navigate(typeof(SnoopView));
        }
        catch (OperationCanceledException exception)
        {
            // ReSharper disable once MethodHasAsyncOverload
            _snackbarService.Show("Operation cancelled", exception.Message, SymbolRegular.Warning24, ControlAppearance.Caution);
        }
        catch (Exception exception)
        {
            await _snackbarService.ShowAsync("Snoop engine error", exception.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
        }
    }

    public void Navigate(Descriptor selectedItem)
    {
        if (selectedItem.Value.Descriptor is not IDescriptorCollector or IDescriptorEnumerator {IsEmpty: true}) return;

        var window = Host.GetService<IWindow>();
        window.Show();
        window.Context.GetService<INavigationService>()!.Navigate(typeof(SnoopView));
        window.Context.GetService<ISnoopService>()!.Snoop(selectedItem.Value);
    }

    private async Task UpdateSearchResults(SearchOption option)
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            FilteredSnoopableObjects = SnoopableObjects;
            FilteredSnoopableData = SnoopableData;
        }
        else
        {
            await SearchEngine.SearchAsync(this, option);
        }

        SearchResultsChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool Validate()
    {
        if (RevitApi.UiDocument is not null) return true;

        _snackbarService.Show("Request denied", "There are no open documents", SymbolRegular.Warning24, ControlAppearance.Caution);
        SnoopableObjects = Array.Empty<SnoopableObject>();
        return false;
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
            SnoopableData = await SelectedObject.GetCachedMembersAsync();
        }
        catch (Exception exception)
        {
            await _snackbarService.ShowAsync("Snoop engine error", exception.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
        }
    }
}