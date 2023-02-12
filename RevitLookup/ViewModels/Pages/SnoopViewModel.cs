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
using RevitLookup.ViewModels.Utils;
using RevitLookup.Views.Pages;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    private readonly IWindowController _windowController;
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private IReadOnlyList<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyList<SnoopableObject> _filteredSnoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyList<Descriptor> _snoopableData;
    [ObservableProperty] private IReadOnlyList<Descriptor> _filteredSnoopableData;

    public SnoopViewModel(IWindowController windowController, INavigationService navigationService, ISnackbarService snackbarService)
    {
        _windowController = windowController;
        _navigationService = navigationService;
        _snackbarService = snackbarService;
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
                    case SnoopableType.UpdaterRegistry:
                    case SnoopableType.Schemas:
                    case SnoopableType.UiApplication:
                        return Selector.Snoop(snoopableType);
                    case SnoopableType.Face:
                    case SnoopableType.Edge:
                    case SnoopableType.LinkedElement:
                    case SnoopableType.Point:
                    case SnoopableType.SubElement:
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
            _navigationService.Navigate(typeof(DashboardView));
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
        window.Scope.GetService<ISnoopService>()!.Snoop(selectedItem.Value);
    }

    partial void OnSearchTextChanged(string value)
    {
        UpdateSearchResults(SearchOption.Objects);
        SearchResultsChanged?.Invoke(this, EventArgs.Empty);
    }

    partial void OnSnoopableObjectsChanged(IReadOnlyList<SnoopableObject> value)
    {
        SelectedObject = null;
        UpdateSearchResults(SearchOption.Objects);
        TreeSourceChanged?.Invoke(this, EventArgs.Empty);
    }

    partial void OnSnoopableDataChanged(IReadOnlyList<Descriptor> value)
    {
        UpdateSearchResults(SearchOption.Selection);
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