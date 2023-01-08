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
using RevitLookup.Core.ComponentModel;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Utils;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Common;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    [ObservableProperty] private string _searchText;
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    private IReadOnlyList<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();
    [ObservableProperty] private IReadOnlyList<Descriptor> _snoopableData = Array.Empty<Descriptor>();

    public SnoopViewModel(INavigationService navigationService, ISnackbarService snackbarService)
    {
        _navigationService = navigationService;
        _snackbarService = snackbarService;
    }

    public IReadOnlyList<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            SetProperty(ref _snoopableObjects, value);
            SearchText = string.Empty;
        }
    }

    public void Snoop(SnoopableObject snoopableObject)
    {
        if (snoopableObject.Descriptor is IDescriptorEnumerator enumerator)
        {
            var objects = new List<SnoopableObject>();
            foreach (var obj in enumerator.Enumerate())
            {
                objects.Add(new SnoopableObject(snoopableObject.Context, obj));
            }

            SnoopableObjects = objects;
        }
        else
        {
            SnoopableObjects = new[] {snoopableObject};
        }
    }

    [RelayCommand]
    public void SnoopSelection()
    {
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.Selection);
    }

    public void SnoopApplication()
    {
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.Application);
    }

    public void SnoopDocument()
    {
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.Document);
    }

    public void SnoopView()
    {
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.View);
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.Database);
    }

    public void SnoopEdge()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.Edge);
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopFace()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.Face);
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopLinkedElement()
    {
        _navigationService.GetNavigationWindow().Hide();
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.LinkedElement);
        _navigationService.GetNavigationWindow().Show();
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = CollectorUtils.Snoop(SnoopableType.DependentElements);
    }

    [RelayCommand]
    private async Task Refresh(object param)
    {
        if (param is null)
        {
            SnoopableData = Array.Empty<Descriptor>();
            return;
        }

        if (param is not SnoopableObject snoopableObject) return;
        try
        {
            var members = await snoopableObject.GetCachedMembersAsync();
            if (members is null) return;

            SnoopableData = members;
        }
        catch (Exception exception)
        {
            await _snackbarService.ShowAsync("Snoop engine error", exception.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
        }
    }
}