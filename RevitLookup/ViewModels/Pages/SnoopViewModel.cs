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

using RevitLookup.Core;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using RevitLookup.Views.Pages;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitLookup.ViewModels.Pages;

public sealed class SnoopViewModel : SnoopViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    private readonly IWindowController _windowController;

    public SnoopViewModel(IWindowController windowController, INavigationService navigationService, ISnackbarService snackbarService)
        : base(navigationService, snackbarService)
    {
        _windowController = windowController;
        _navigationService = navigationService;
        _snackbarService = snackbarService;
    }

    public override async Task Snoop(SnoopableType snoopableType)
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
                    case SnoopableType.Services:
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
                    case SnoopableType.Events:
                        throw new NotSupportedException();
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

    private bool Validate()
    {
        if (RevitApi.UiDocument is not null) return true;

        _snackbarService.Show("Request denied", "There are no open documents", SymbolRegular.Warning24, ControlAppearance.Caution);
        SnoopableObjects = Array.Empty<SnoopableObject>();
        return false;
    }
}