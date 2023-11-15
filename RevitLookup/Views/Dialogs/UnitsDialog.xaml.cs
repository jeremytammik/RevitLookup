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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.DB;
using RevitLookup.Core;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Dialogs;
using RevitLookup.ViewModels.Objects;
using RevitLookup.Views.Extensions;

namespace RevitLookup.Views.Dialogs;

public sealed partial class UnitsDialog
{
    private readonly IServiceProvider _serviceProvider;

    public UnitsDialog(IServiceProvider serviceProvider, List<UnitInfo> unitType)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        DataContext = new UnitsViewModel(unitType);
    }

    private void OnRowLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        var element = (FrameworkElement) sender;
        var unitInfo = (UnitInfo) element.DataContext;
        CreateTreeContextMenu(unitInfo, element);
    }

    private void CreateTreeContextMenu(UnitInfo info, FrameworkElement row)
    {
        row.ContextMenu = new ContextMenu
        {
            Resources = Resources
        };

        row.ContextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy unit")
            .SetCommand(info, unitInfo => Clipboard.SetText(unitInfo.Unit))
            .SetShortcut(row, ModifierKeys.Control, Key.C);
        row.ContextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy label")
            .SetCommand(info, unitInfo => Clipboard.SetText(unitInfo.Label))
            .SetShortcut(row, ModifierKeys.Control | ModifierKeys.Shift, Key.C);
        row.ContextMenu.AddMenuItem()
            .SetHeader("Snoop")
            .SetCommand(info, unitInfo =>
            {
                var obj = unitInfo.UnitObject switch
                {
                    BuiltInParameter parameter => RevitApi.GetBuiltinParameter(parameter),
                    BuiltInCategory category => RevitApi.GetBuiltinCategory(category),
                    _ => unitInfo.UnitObject
                };

                _serviceProvider.GetService<ISnoopVisualService>().Snoop(new SnoopableObject(obj));
            });
    }
}