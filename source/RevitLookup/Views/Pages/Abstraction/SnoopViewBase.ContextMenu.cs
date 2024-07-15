// Copyright 2003-2024 by Autodesk, Inc.
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Services;
using RevitLookup.Views.Extensions;
using RevitLookup.Views.Utils;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.Views.Pages.Abstraction;

public partial class SnoopViewBase
{
    /// <summary>
    ///     Tree view context menu
    /// </summary>
    private void CreateTreeContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources,
            PlacementTarget = row,
            DataContext = ViewModel
        };
        
        row.ContextMenu = contextMenu;
        
        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(descriptor, parameter => Clipboard.SetDataObject(parameter.Name))
            .SetShortcut(ModifierKeys.Control, Key.C);
        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName))
            .SetShortcut(Key.F1);
        
        if (descriptor is not IDescriptorConnector connector) return;
        
        try
        {
            connector.RegisterMenu(contextMenu);
        }
        catch (Exception exception)
        {
            var logger = ViewModel.ServiceProvider.GetRequiredService<ILogger<SnoopViewBase>>();
            var notificationService = ViewModel.ServiceProvider.GetRequiredService<NotificationService>();
            
            logger.LogError(exception, "RegisterMenu error");
            notificationService.ShowError("RegisterMenu error", exception);
        }
    }
    
    /// <summary>
    ///     Data grid context menu
    /// </summary>
    private void CreateGridContextMenu(DataGrid dataGrid)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources,
            PlacementTarget = dataGrid,
            DataContext = ViewModel
        };
        
        dataGrid.ContextMenu = contextMenu;
        
        contextMenu.AddMenuItem("RefreshMenuItem")
            .SetCommand(ViewModel.RefreshMembersCommand)
            .SetGestureText(Key.F5);
        
        contextMenu.AddSeparator();
        contextMenu.AddLabel("Columns");
        
        contextMenu.AddMenuItem()
            .SetHeader("Time")
            .SetChecked(dataGrid.Columns[2].Visibility == Visibility.Visible)
            .SetCommand(dataGrid.Columns[2], parameter =>
            {
                _settings.ShowTimeColumn = parameter.Visibility != Visibility.Visible;
                parameter.Visibility = _settings.ShowTimeColumn ? Visibility.Visible : Visibility.Collapsed;
            });
        
        contextMenu.AddMenuItem()
            .SetHeader("Memory")
            .SetChecked(dataGrid.Columns[3].Visibility == Visibility.Visible)
            .SetCommand(dataGrid.Columns[3], parameter =>
            {
                _settings.ShowMemoryColumn = parameter.Visibility != Visibility.Visible;
                parameter.Visibility = _settings.ShowMemoryColumn ? Visibility.Visible : Visibility.Collapsed;
            });
        
        contextMenu.AddSeparator();
        contextMenu.AddLabel("Show");
        
        contextMenu.AddMenuItem()
            .SetHeader("Events")
            .SetChecked(_settings.IncludeEvents)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludeEvents = !parameter.IncludeEvents;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Extensions")
            .SetChecked(_settings.IncludeExtensions)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludeExtensions = !parameter.IncludeExtensions;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Fields")
            .SetChecked(_settings.IncludeFields)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludeFields = !parameter.IncludeFields;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Non-public")
            .SetChecked(_settings.IncludePrivate)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludePrivate = !parameter.IncludePrivate;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Root hierarchy")
            .SetChecked(_settings.IncludeRootHierarchy)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludeRootHierarchy = !parameter.IncludeRootHierarchy;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Static")
            .SetChecked(_settings.IncludeStatic)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludeStatic = !parameter.IncludeStatic;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Unsupported")
            .SetChecked(_settings.IncludeUnsupported)
            .SetCommand(_settings, parameter =>
            {
                parameter.IncludeUnsupported = !parameter.IncludeUnsupported;
                return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
    }
    
    /// <summary>
    ///     Data grid row context menu
    /// </summary>
    private void CreateGridRowContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources,
            PlacementTarget = row,
            DataContext = ViewModel
        };
        
        row.ContextMenu = contextMenu;
        
        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(descriptor, parameter => Clipboard.SetDataObject($"{parameter.Name}: {parameter.Value.Descriptor.Name}"))
            .SetShortcut(ModifierKeys.Control, Key.C)
            .SetAvailability(descriptor.Value.Descriptor.Name is not null);
        
        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy value")
            .SetCommand(descriptor, parameter => Clipboard.SetDataObject(parameter.Value.Descriptor.Name))
            .SetShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.C)
            .SetAvailability(descriptor.Value.Descriptor.Name is not null);
        
        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName, parameter.Name))
            .SetShortcut(Key.F1);
        
        if (descriptor.Value.Descriptor is not IDescriptorConnector connector) return;
        
        try
        {
            connector.RegisterMenu(contextMenu);
        }
        catch (Exception exception)
        {
            var logger = ViewModel.ServiceProvider.GetRequiredService<ILogger<SnoopViewBase>>();
            var notificationService = ViewModel.ServiceProvider.GetRequiredService<NotificationService>();
            
            logger.LogError(exception, "RegisterMenu error");
            notificationService.ShowError("RegisterMenu error", exception);
        }
    }
}