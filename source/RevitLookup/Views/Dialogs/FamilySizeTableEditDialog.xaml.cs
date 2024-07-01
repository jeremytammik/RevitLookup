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

using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RevitLookup.ViewModels.Dialogs;
using RevitLookup.Views.Extensions;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Dialogs;

public sealed partial class FamilySizeTableEditDialog
{
    private readonly IServiceProvider _serviceProvider;
    private readonly bool _isEditable;
    private readonly FamilySizeTableEditDialogViewModel _viewModel;
    
    public FamilySizeTableEditDialog(IServiceProvider serviceProvider, FamilySizeTableManager manager, string tableName)
    {
        _isEditable = true;
        _viewModel = new FamilySizeTableEditDialogViewModel(manager, tableName);
        DataContext = _viewModel;
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }
    
    public FamilySizeTableEditDialog(IServiceProvider serviceProvider, FamilySizeTable table)
    {
        DataContext = new FamilySizeTableEditDialogViewModel(table);
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }
    
    public async Task ShowAsync()
    {
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "Family size table",
            Content = this,
            CloseButtonText = "Close",
            HorizontalScrollVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollVisibility = ScrollBarVisibility.Disabled
        };
        if (_isEditable && Context.Document.IsFamilyDocument)
        {
            dialogOptions.PrimaryButtonText = "Save and close";
        }
        
        var dialogResult = await _serviceProvider.GetService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions);
        if (dialogResult == ContentDialogResult.Primary)
        {
            _viewModel.SaveData();
        }
    }
    
    private void CreateGridRowContextMenu(DataRow dataRow, FrameworkElement dataGridRow)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources,
            PlacementTarget = dataGridRow,
            DataContext = _viewModel
        };
        
        dataGridRow.ContextMenu = contextMenu;
        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Duplicate row")
            .SetCommand(dataRow, _ => _viewModel.DuplicateRow(dataRow))
            .SetShortcut(ModifierKeys.Control, Key.C);
        
        contextMenu.AddMenuItem("DeleteMenuItem")
            .SetHeader("Delete row")
            .SetCommand(dataRow, _ => _viewModel.DeleteRow(dataRow))
            .SetShortcut(ModifierKeys.Control, Key.Delete);
    }
    
    private void OnGridRowLoading(object sender, DataGridRowEventArgs args)
    {
        var row = args.Row;
        row.Loaded += OnGridRowLoaded;
    }
    
    private void OnGridRowLoaded(object sender, RoutedEventArgs args)
    {
        var element = (FrameworkElement) sender;
        var context = (DataRowView) element.DataContext;
        CreateGridRowContextMenu(context.Row, element);
    }
}