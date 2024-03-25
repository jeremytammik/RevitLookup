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

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Core.Objects;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Enums;
using RevitLookup.ViewModels.Utils;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Demo.Mock.ViewModels;

public sealed partial class MockSnoopViewModel(NotificationService notificationService, IServiceProvider provider) : ObservableObject, ISnoopViewModel
{
    private Task _updatingTask = Task.CompletedTask;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private IList<SnoopableObject> _snoopableObjects = [];
    [ObservableProperty] private IList<SnoopableObject> _filteredSnoopableObjects = [];
    [ObservableProperty] private IList<Descriptor> _filteredSnoopableData;
    [ObservableProperty] private IList<Descriptor> _snoopableData;

    public SnoopableObject SelectedObject { get; set; }
    public IServiceProvider ServiceProvider { get; } = provider;

    public void Navigate(SnoopableObject selectedItem)
    {
        Host.GetService<ILookupService>()
            .Snoop(selectedItem)
            .DependsOn(ServiceProvider)
            .Show<SnoopView>();
    }

    public void Navigate(IList<SnoopableObject> selectedItems)
    {
        Host.GetService<ILookupService>()
            .Snoop(selectedItems)
            .DependsOn(ServiceProvider)
            .Show<SnoopView>();
    }

    async partial void OnSearchTextChanged(string value)
    {
        await _updatingTask;
        UpdateSearchResults(SearchOption.Objects);
    }

    async partial void OnSnoopableObjectsChanged(IList<SnoopableObject> value)
    {
        SelectedObject = null;
        await _updatingTask;
        UpdateSearchResults(SearchOption.Objects);
    }

    async partial void OnSnoopableDataChanged(IList<Descriptor> value)
    {
        await _updatingTask;
        UpdateSearchResults(SearchOption.Selection);
    }

    private void UpdateSearchResults(SearchOption option)
    {
        _updatingTask = Task.Run(() =>
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                if (option == SearchOption.Objects)
                {
                    FilteredSnoopableObjects = SnoopableObjects;
                }

                FilteredSnoopableData = SnoopableData;
                return;
            }

            var results = SearchEngine.Search(this, option);
            if (results.Data is not null) FilteredSnoopableData = results.Data;
            if (results.Objects is not null) FilteredSnoopableObjects = new ObservableCollection<SnoopableObject>(results.Objects);
        });
    }

    public void RemoveObject(object obj)
    {
        var snoopableObject = obj switch
        {
            SnoopableObject snoopable => snoopable,
            Descriptor descriptor => descriptor.Value.Descriptor.Value,
            _ => throw new NotSupportedException($"Type {obj.GetType().Name} removing not supported")
        };
        
        SnoopableObjects.Remove(snoopableObject);
        FilteredSnoopableObjects.Remove(snoopableObject);
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