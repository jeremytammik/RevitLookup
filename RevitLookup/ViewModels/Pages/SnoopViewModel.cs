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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Core.Utils;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Common;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly ISnackbarService _snackbarService;
    private readonly IWindowController _windowController;
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private IReadOnlyList<Descriptor> _snoopableData = Array.Empty<Descriptor>();
    private IReadOnlyList<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();

    public SnoopViewModel(IWindowController windowController, ISnackbarService snackbarService)
    {
        _windowController = windowController;
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
            foreach (var obj in enumerator.Enumerate()) objects.Add(new SnoopableObject(snoopableObject.Context, obj));

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
        SnoopableObjects = Snooper.Snoop(SnoopableType.Selection);
    }

    public void SnoopApplication()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Application);
    }

    public void SnoopDocument()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Document);
    }

    public void SnoopView()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.View);
    }

    public void SnoopDatabase()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.Database);
    }

    public void SnoopEdge()
    {
        _windowController.Hide();
        SnoopableObjects = Snooper.Snoop(SnoopableType.Edge);
        _windowController.Show();
    }

    public void SnoopFace()
    {
        _windowController.Hide();
        SnoopableObjects = Snooper.Snoop(SnoopableType.Face);
        _windowController.Show();
    }

    public void SnoopLinkedElement()
    {
        _windowController.Hide();
        SnoopableObjects = Snooper.Snoop(SnoopableType.LinkedElement);
        _windowController.Show();
    }

    public void SnoopDependentElements()
    {
        SnoopableObjects = Snooper.Snoop(SnoopableType.DependentElements);
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