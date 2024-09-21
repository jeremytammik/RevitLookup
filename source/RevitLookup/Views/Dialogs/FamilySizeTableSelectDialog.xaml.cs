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

using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Services;
using RevitLookup.ViewModels.Dialogs;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Dialogs;

public sealed partial class FamilySizeTableSelectDialog
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FamilySizeTableManager _manager;
    private readonly FamilySizeTableSelectDialogViewModel _viewModel;
    private readonly Document _document;

    public FamilySizeTableSelectDialog(IServiceProvider serviceProvider, Document document, FamilySizeTableManager manager)
    {
        _serviceProvider = serviceProvider;
        _document = document;
        _manager = manager;
        _viewModel = new FamilySizeTableSelectDialogViewModel(manager);

        DataContext = _viewModel;
        InitializeComponent();
    }

    public async Task ShowExportDialogAsync()
    {
        PrimaryButtonText = "Export";
        
        var dialogResult = await ShowAsync();
        if (dialogResult != ContentDialogResult.Primary) return;
        
        try
        {
            _viewModel.Export();
        }
        catch (Exception exception)
        {
            var notificationService = _serviceProvider.GetRequiredService<NotificationService>();
            notificationService.ShowWarning("Export error", exception.Message);
        }
    }

    public async Task ShowEditDialogAsync()
    {
        PrimaryButtonText = "Edit";

        var dialogResult = await ShowAsync();
        if (dialogResult != ContentDialogResult.Primary) return;

        try
        {
            var dialog = new FamilySizeTableEditDialog(_document, _manager, _viewModel.SelectedTable);
            await dialog.ShowDialogAsync();
        }
        catch (Exception exception)
        {
            var notificationService = _serviceProvider.GetRequiredService<NotificationService>();
            notificationService.ShowWarning("Edit error", exception.Message);
        }
    }
}