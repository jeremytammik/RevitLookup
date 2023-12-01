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

namespace RevitLookup.UI.Demo.ViewModels;

public sealed partial class MoqSnoopViewModel(NotificationService notificationService, IServiceProvider provider) : ObservableObject, ISnoopViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyCollection<SnoopableObject> _filteredSnoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _snoopableData;
    [ObservableProperty] private IReadOnlyCollection<Descriptor> _filteredSnoopableData;

    public SnoopableObject SelectedObject { get; set; }

    public void Navigate(Descriptor selectedItem)
    {
        if (selectedItem.Value.Descriptor is not IDescriptorCollector) return;
        if (selectedItem.Value.Descriptor is IDescriptorEnumerator {IsEmpty: true}) return;

        Host.GetService<ILookupService>()
            .Snoop(selectedItem.Value)
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
        CollectMembers(true);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RefreshMembersAsync()
    {
        CollectMembers(false);
        return Task.CompletedTask;
    }

    private void CollectMembers(bool useCached)
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
            notificationService.ShowError("Snoop engine error", exception);
        }
    }
}