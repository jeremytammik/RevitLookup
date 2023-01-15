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
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.UI.Common;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SnoopViewModel : ObservableObject, ISnoopViewModel
{
    private readonly ISnackbarService _snackbarService;
    private readonly ISettingsService _settingsService;
    private readonly IWindowController _windowController;
    [ObservableProperty] private string _searchText;
    [ObservableProperty] private IReadOnlyList<Descriptor> _snoopableData = Array.Empty<Descriptor>();
    private IReadOnlyList<SnoopableObject> _snoopableObjects = Array.Empty<SnoopableObject>();

    public SnoopViewModel(ISettingsService settingsService, IWindowController windowController, ISnackbarService snackbarService)
    {
        _settingsService = settingsService;
        _windowController = windowController;
        _snackbarService = snackbarService;
    }

    public IReadOnlyList<SnoopableObject> SnoopableObjects
    {
        get => _snoopableObjects;
        private set
        {
            SnoopableData = Array.Empty<Descriptor>();
            SetProperty(ref _snoopableObjects, value);
        }
    }

    public void Snoop(SnoopableObject snoopableObject)
    {
        if (snoopableObject.Descriptor is IDescriptorEnumerator {IsEmpty: false} descriptorEnumerator)
        {
            var objects = new List<SnoopableObject>();
            var enumerator = descriptorEnumerator.GetEnumerator();
            while (enumerator.MoveNext()) objects.Add(new SnoopableObject(snoopableObject.Context, enumerator.Current));

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
        if (!ValidateDocuments()) return;
        SnoopableObjects = Selector.Snoop(SnoopableType.Selection);
    }

    public void SnoopApplication()
    {
        if (!ValidateDocuments()) return;
        SnoopableObjects = Selector.Snoop(SnoopableType.Application);
    }

    public void SnoopDocument()
    {
        if (!ValidateDocuments()) return;
        SnoopableObjects = Selector.Snoop(SnoopableType.Document);
    }

    public void SnoopView()
    {
        if (!ValidateDocuments()) return;
        SnoopableObjects = Selector.Snoop(SnoopableType.View);
    }

    public void SnoopDatabase()
    {
        if (!ValidateDocuments()) return;
        SnoopableObjects = Selector.Snoop(SnoopableType.Database);
    }

    public void SnoopEdge()
    {
        if (!ValidateDocuments()) return;
        _windowController.Hide();
        SnoopableObjects = Selector.Snoop(SnoopableType.Edge);
        _windowController.Show();
    }

    public void SnoopFace()
    {
        if (!ValidateDocuments()) return;
        _windowController.Hide();
        SnoopableObjects = Selector.Snoop(SnoopableType.Face);
        _windowController.Show();
    }

    public void SnoopLinkedElement()
    {
        if (!ValidateDocuments()) return;
        _windowController.Hide();
        SnoopableObjects = Selector.Snoop(SnoopableType.LinkedElement);
        _windowController.Show();
    }

    public void SnoopDependentElements()
    {
        if (!ValidateDocuments()) return;
        SnoopableObjects = Selector.Snoop(SnoopableType.DependentElements);
    }

    private bool ValidateDocuments()
    {
        if (RevitApi.UiDocument is not null) return true;

        _snackbarService.Show("Request denied", "There are no open documents", SymbolRegular.Warning24, ControlAppearance.Caution);
        SnoopableObjects = Array.Empty<SnoopableObject>();
        return false;
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
            //Await Frame transition. GetMembers freezes the thread and breaks the animation
            await Task.Delay(_settingsService.TransitionDuration);
            SnoopableData = await snoopableObject.GetCachedMembersAsync();
        }
        catch (Exception exception)
        {
            await _snackbarService.ShowAsync("Snoop engine error", exception.Message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
        }
    }
}