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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Views.Extensions;
using RevitLookup.Views.Utils;

namespace RevitLookup.Views.Pages.Abstraction;

public partial class SnoopViewBase
{
    private void CreateTreeContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources
        };

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(descriptor, parameter => Clipboard.SetText(parameter.Name))
            .SetShortcut(row, ModifierKeys.Control, Key.C);
        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName))
            .SetShortcut(row, Key.F1);

        if (descriptor is IDescriptorConnector connector) connector.RegisterMenu(contextMenu, row);
        row.ContextMenu = contextMenu;
    }

    private void CreateGridContextMenu(DataGrid dataGrid)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources
        };

        contextMenu.AddMenuItem("RefreshMenuItem")
            .SetCommand(ViewModel.RefreshMembersCommand)
            .SetGestureText(Key.F5);

        contextMenu.AddSeparator();
        contextMenu.AddLabel("Columns");

        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Time")
            .SetCommand(dataGrid.Columns[2].Visibility == Visibility.Visible, parameter =>
            {
                dataGrid.Columns[2].Visibility = parameter ? Visibility.Collapsed : Visibility.Visible;
                _settingsService.ShowTimeColumn = !parameter;
            });

        contextMenu.AddSeparator();
        contextMenu.AddLabel("Show");

        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Events")
            .SetCommand(_settingsService.IncludeEvents, parameter =>
            {
                _settingsService.IncludeEvents = !parameter;
                return RefreshGridAsync();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Extensions")
            .SetCommand(_settingsService.IncludeExtensions, parameter =>
            {
                _settingsService.IncludeExtensions = !parameter;
                return RefreshGridAsync();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Fields")
            .SetCommand(_settingsService.IncludeFields, parameter =>
            {
                _settingsService.IncludeFields = !parameter;
                return RefreshGridAsync();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Non-public")
            .SetCommand(_settingsService.IncludePrivate, parameter =>
            {
                _settingsService.IncludePrivate = !parameter;
                return RefreshGridAsync();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Root hierarchy")
            .SetCommand(_settingsService.IncludeRootHierarchy, parameter =>
            {
                _settingsService.IncludeRootHierarchy = !parameter;
                return RefreshGridAsync();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Static")
            .SetCommand(_settingsService.IncludeStatic, parameter =>
            {
                _settingsService.IncludeStatic = !parameter;
                return RefreshGridAsync();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Unsupported")
            .SetCommand(_settingsService.IncludeUnsupported, parameter =>
            {
                _settingsService.IncludeUnsupported = !parameter;
                return RefreshGridAsync();
            });

        dataGrid.ContextMenu = contextMenu;
    }

    private void CreateGridRowContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources
        };

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(descriptor, parameter => Clipboard.SetText($"{parameter.Name}: {parameter.Value.Descriptor.Name}"))
            .SetShortcut(row, ModifierKeys.Control, Key.C);

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy value")
            .SetCommand(descriptor, parameter => Clipboard.SetText(parameter.Value.Descriptor.Name))
            .SetShortcut(row, ModifierKeys.Control | ModifierKeys.Shift, Key.C)
            .SetAvailability(descriptor.Value.Descriptor.Name is not null);

        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName, parameter.Name))
            .SetShortcut(row, new KeyGesture(Key.F1));

        if (descriptor.Value.Descriptor is IDescriptorConnector connector) connector.RegisterMenu(contextMenu, row);
        row.ContextMenu = contextMenu;
    }
}