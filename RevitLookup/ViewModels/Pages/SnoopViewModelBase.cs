﻿// Copyright 2003-2023 by Autodesk, Inc.
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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Enums;
using RevitLookup.ViewModels.Utils;
using RevitLookup.Views.Pages;

namespace RevitLookup.ViewModels.Pages;

public abstract partial class SnoopViewModelBase(NotificationService notificationService, IServiceProvider provider) : ObservableObject, ISnoopViewModel
{
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _filteredSnoopableData;
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _filteredSnoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _snoopableData;
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();

    public SnoopableObject SelectedObject { get; set; }

    public void Navigate(SnoopableObject selectedItem)
    {
        Host.GetService<ILookupService>()
            .Snoop(selectedItem)
            .DependsOn(provider)
            .Show<SnoopView>();
    }

    public void Navigate(IReadOnlyCollection<SnoopableObject> selectedItems)
    {
        Host.GetService<ILookupService>()
            .Snoop(selectedItems)
            .DependsOn(provider)
            .Show<SnoopView>();
    }

    partial void OnSearchTextChanged(string value)
    {
        UpdateSearchResults(SearchOption.Objects);
    }

    partial void OnSnoopableObjectsChanged(IReadOnlyCollection<SnoopableObject> value)
    {
        SelectedObject = null;
        UpdateSearchResults(SearchOption.Objects);
    }

    partial void OnSnoopableDataChanged(IReadOnlyCollection<Descriptor> value)
    {
        UpdateSearchResults(SearchOption.Selection);
    }

    private void UpdateSearchResults(SearchOption option)
    {
        Task.Run(() =>
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredSnoopableObjects = SnoopableObjects;
                FilteredSnoopableData = SnoopableData;
                return;
            }

            var results = SearchEngine.Search(this, option);
            if (results.Data is not null) FilteredSnoopableData = results.Data;
            if (results.Objects is not null) FilteredSnoopableObjects = results.Objects;
        });
    }

    [RelayCommand]
    private Task FetchMembersAsync()
    {
        return CollectMembersAsync(true);
    }

    [RelayCommand]
    private Task RefreshMembersAsync()
    {
        return CollectMembersAsync(false);
    }

    private async Task CollectMembersAsync(bool useCached)
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
            notificationService.ShowError("Invalid object", exception);
        }
        catch (InternalException)
        {
            notificationService.ShowError(
                "Invalid object",
                "A problem in the Revit code. Usually occurs when a managed API object is no longer valid and is unloaded from memory");
        }
        catch (SEHException)
        {
            notificationService.ShowError(
                "Revit API internal error",
                "A problem in the Revit code. Usually occurs when a managed API object is no longer valid and is unloaded from memory");
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Snoop engine error", exception);
        }
    }
}