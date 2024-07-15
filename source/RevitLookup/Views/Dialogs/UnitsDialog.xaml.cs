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

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core;
using RevitLookup.Models;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Dialogs;
using RevitLookup.Views.Extensions;
using RevitLookup.Views.Pages;
using Wpf.Ui;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.Views.Dialogs;

public sealed partial class UnitsDialog
{
    private readonly UnitsViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationTokenSource _tokenSource = new();
    
    public UnitsDialog(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _viewModel = new UnitsViewModel();
        DataContext = _viewModel;
        InitializeComponent();
    }
    
    public async Task ShowParametersAsync()
    {
        _viewModel.Units = await Task.Run(RevitShell.GetUnitInfos<BuiltInParameter>);
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "BuiltIn Parameters",
            Content = this,
            CloseButtonText = "Close",
            DialogMaxWidth = 1000,
            HorizontalScrollVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollVisibility = ScrollBarVisibility.Disabled
        };
        
        await _serviceProvider.GetRequiredService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions, _tokenSource.Token);
        _tokenSource.Dispose();
    }
    
    public async Task ShowCategoriesAsync()
    {
        _viewModel.Units = await Task.Run(RevitShell.GetUnitInfos<BuiltInCategory>);
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "BuiltIn Categories",
            Content = this,
            CloseButtonText = "Close",
            DialogMaxWidth = 600,
            HorizontalScrollVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollVisibility = ScrollBarVisibility.Disabled
        };
        
        await _serviceProvider.GetRequiredService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions, _tokenSource.Token);
        _tokenSource.Dispose();
    }
    
    public async Task ShowForgeSchemaAsync()
    {
        _viewModel.Units = await Task.Run(RevitShell.GetUnitInfos<ForgeTypeId>);
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "Forge Schema",
            Content = this,
            CloseButtonText = "Close",
            DialogMaxWidth = 1100,
            HorizontalScrollVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollVisibility = ScrollBarVisibility.Disabled
        };
        
        ClassColumn.Visibility = Visibility.Visible;
        await _serviceProvider.GetRequiredService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions, _tokenSource.Token);
        _tokenSource.Dispose();
    }
    
    private void OnMouseEnter(object sender, RoutedEventArgs routedEventArgs)
    {
        var element = (FrameworkElement) sender;
        var unitInfo = (UnitInfo) element.DataContext;
        CreateTreeContextMenu(unitInfo, element);
    }
    
    private void CreateTreeContextMenu(UnitInfo info, FrameworkElement row)
    {
        Debug.WriteLine(info.Label);
        var contextMenu = new ContextMenu
        {
            Resources = Wpf.Ui.Application.MainWindow.Resources,
            PlacementTarget = row
        };
        
        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy unit")
            .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Unit))
            .SetShortcut(ModifierKeys.Control, Key.C);
        
        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy label")
            .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Label))
            .SetShortcut(ModifierKeys.Control | ModifierKeys.Alt, Key.C);
        
        if (info.Class is not null)
        {
            contextMenu.AddMenuItem("CopyMenuItem")
                .SetHeader("Copy class")
                .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Class))
                .SetShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.C);
        }
        
        contextMenu.AddMenuItem("SnoopMenuItem")
            .SetHeader("Snoop")
            .SetCommand(info, unitInfo =>
            {
                var obj = unitInfo.UnitObject switch
                {
                    BuiltInParameter parameter => RevitShell.GetBuiltinParameter(parameter),
                    BuiltInCategory category => RevitShell.GetBuiltinCategory(category),
                    _ => unitInfo.UnitObject
                };
                
                _tokenSource.Cancel();
                _serviceProvider.GetRequiredService<ISnoopVisualService>().Snoop(new SnoopableObject(obj));
                _serviceProvider.GetRequiredService<INavigationService>().Navigate(typeof(SnoopView));
            });
        
        
        row.ContextMenu = contextMenu;
    }
}