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

    public UnitsDialog(
        IContentDialogService dialogService,
        IServiceProvider serviceProvider)
        : base(dialogService.GetDialogHost())
    {
        _serviceProvider = serviceProvider;
        _viewModel = new UnitsViewModel();
        DataContext = _viewModel;
        InitializeComponent();
    }

    public async Task ShowParametersDialogAsync()
    {
        _viewModel.Units = await Task.Run(RevitShell.GetUnitInfos<BuiltInParameter>);

        Title = "BuiltIn Parameters";
        DialogMaxWidth = 1000;

        await ShowAsync();
    }

    public async Task ShowCategoriesDialogAsync()
    {
        _viewModel.Units = await Task.Run(RevitShell.GetUnitInfos<BuiltInCategory>);

        Title = "BuiltIn Categories";
        DialogMaxWidth = 600;
        
        await ShowAsync();
    }

    public async Task ShowForgeSchemaDialogAsync()
    {
        _viewModel.Units = await Task.Run(RevitShell.GetUnitInfos<ForgeTypeId>);
        
        ClassColumn.Visibility = Visibility.Visible;
        Title = "Forge Schema";
        DialogMaxWidth = 1100;

        await ShowAsync();
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

                Hide();
                _serviceProvider.GetRequiredService<ISnoopVisualService>().Snoop(new SnoopableObject(obj));
                _serviceProvider.GetRequiredService<INavigationService>().Navigate(typeof(SnoopPage));
            });


        row.ContextMenu = contextMenu;
    }
}