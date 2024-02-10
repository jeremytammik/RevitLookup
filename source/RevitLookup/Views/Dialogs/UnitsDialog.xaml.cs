﻿// Copyright 2003-2023 by Autodesk, Inc.
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
using RevitLookup.Views.Pages;
using Wpf.Ui;

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
        _viewModel.Units = await Task.Run(RevitApi.GetUnitInfos<BuiltInParameter>);
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "BuiltIn Parameters",
            Content = this,
            CloseButtonText = "Close"
        };

        await _serviceProvider.GetService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions, _tokenSource.Token);
        _tokenSource.Dispose();
    }

    public async Task ShowCategoriesAsync()
    {
        _viewModel.Units = await Task.Run(RevitApi.GetUnitInfos<BuiltInCategory>);
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "BuiltIn Categories",
            Content = this,
            CloseButtonText = "Close"
        };

        await _serviceProvider.GetService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions, _tokenSource.Token);
        _tokenSource.Dispose();
    }

    public async Task ShowForgeSchemaAsync()
    {
        _viewModel.Units = await Task.Run(RevitApi.GetUnitInfos<ForgeTypeId>);
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "Forge Schema",
            Content = this,
            CloseButtonText = "Close"
        };

        await _serviceProvider.GetService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions, _tokenSource.Token);
        _tokenSource.Dispose();
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
            Resources = Wpf.Ui.Application.MainWindow.Resources
        };

        row.ContextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy unit")
            .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Unit))
            .SetShortcut(row, ModifierKeys.Control, Key.C);
        row.ContextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy label")
            .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Label))
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

                _tokenSource.Cancel();
                _serviceProvider.GetService<ISnoopVisualService>().Snoop(new SnoopableObject(obj));
                _serviceProvider.GetService<INavigationService>().Navigate(typeof(SnoopView));
            });
    }
}