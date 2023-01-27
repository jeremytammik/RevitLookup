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
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class DashboardViewModel : ObservableObject
{
    private readonly ISnoopService _snoopService;

    public DashboardViewModel(ISnoopService snoopService)
    {
        _snoopService = snoopService;
    }

    [RelayCommand]
    private async Task NavigateSnoopPage(string parameter)
    {
        switch (parameter)
        {
            case "selection":
                await _snoopService.Snoop(SnoopableType.Selection);
                break;
            case "document":
                await _snoopService.Snoop(SnoopableType.Document);
                break;
            case "database":
                await _snoopService.Snoop(SnoopableType.Database);
                break;
            case "view":
                await _snoopService.Snoop(SnoopableType.View);
                break;
            case "application":
                await _snoopService.Snoop(SnoopableType.Application);
                break;
            case "linked":
                await _snoopService.Snoop(SnoopableType.LinkedElement);
                break;
            case "dependents":
                await _snoopService.Snoop(SnoopableType.DependentElements);
                break;
            case "face":
                await _snoopService.Snoop(SnoopableType.Face);
                break;
            case "edge":
                await _snoopService.Snoop(SnoopableType.Edge);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(parameter), parameter);
        }
    }

    [RelayCommand]
    private async Task NavigateEventPage()
    {
        await Task.CompletedTask;
    }
}