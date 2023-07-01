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

using System.Runtime.InteropServices;
using Autodesk.Revit.Exceptions;
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
using OperationCanceledException = System.OperationCanceledException;

namespace RevitLookup.ViewModels.Pages;

public abstract partial class SnoopViewModelBase : ObservableObject, ISnoopViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _filteredSnoopableData;
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _filteredSnoopableObjects = Array.Empty<SnoopableObject>();
    private CancellationTokenSource _searchCancellationToken = new();
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _snoopableData;
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();

    protected SnoopViewModelBase(INavigationService navigationService, ISnackbarService snackbarService)
    {
        _navigationService = navigationService;
        _snackbarService = snackbarService;
    }

    public SnoopableObject SelectedObject { get; set; }
    public event EventHandler TreeSourceChanged;
    public event EventHandler SearchResultsChanged;

    public void Snoop(SnoopableObject snoopableObject)
    {
        if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptor)
            try
            {
                SnoopableObjects = descriptor.ParseEnumerable(snoopableObject);
            }
            catch (Exception exception)
            {
                SnowException("Invalid object", exception.Message);
            }
        else
            SnoopableObjects = new[] {snoopableObject};

        _navigationService.Navigate(typeof(SnoopView));
    }

    public abstract Task Snoop(SnoopableType snoopableType);

    public void Navigate(Descriptor selectedItem)
    {
        if (selectedItem.Value.Descriptor is not IDescriptorCollector or IDescriptorEnumerator {IsEmpty: true}) return;

        var owner = Wpf.Ui.Application.MainWindow;
        var window = Host.GetService<IWindow>();
        window.Show(owner);
        window.ServiceProvider.GetService<ISnoopService>()!.Snoop(selectedItem.Value);
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
    private async Task FetchMembersAsync()
    {
        await CollectMembers(true);
    }

    [RelayCommand]
    private async Task RefreshMembersAsync()
    {
        await CollectMembers(false);
    }

    private async Task CollectMembers(bool useCached)
    {
        if (SelectedObject is null)
        {
            SnoopableData = Array.Empty<Descriptor>();
            return;
        }

        try
        {
            SnoopableData = useCached ? await SelectedObject.GetCachedMembersAsync() : await SelectedObject.GetMembersAsync();
        }
        catch (InvalidObjectException exception)
        {
            SnowException("Invalid object", exception.Message);
        }
        catch (InternalException)
        {
            const string message = "A problem in the Revit code. Usually occurs when a managed API object is no longer valid and is unloaded from memory";
            SnowException("Revit API internal error", message);
        }
        catch (SEHException)
        {
            const string message = "A problem in the Revit code. Usually occurs when a managed API object is no longer valid and is unloaded from memory";
            SnowException("Revit API internal error", message);
        }
        catch (Exception exception)
        {
            SnowException("Snoop engine error", exception.Message);
        }
    }

    private void SnowException(string title, string message)
    {
        _snackbarService.Show(title, message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
    }
}