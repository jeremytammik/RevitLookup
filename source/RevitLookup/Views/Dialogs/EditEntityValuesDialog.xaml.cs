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
using Autodesk.Revit.DB.ExtensibleStorage;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Services;
using RevitLookup.ViewModels.Dialogs.ExtensibleStorage;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Dialogs;

public sealed partial class EditEntityValuesDialog
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EditEntityViewModel _viewModel;
    
    public EditEntityValuesDialog(IServiceProvider serviceProvider, Element element, Entity entity)
    {
        _serviceProvider = serviceProvider;
        _viewModel = new EditEntityViewModel(entity, element);
        
        DataContext = _viewModel;
        InitializeComponent();
    }
    
    public async Task ShowAsync()
    {
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "Edit entity field values",
            Content = this,
            PrimaryButtonText = "Save",
            CloseButtonText = "Close",
            DialogVerticalAlignment = VerticalAlignment.Center,
            DialogHorizontalAlignment = HorizontalAlignment.Center,
            HorizontalScrollVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollVisibility = ScrollBarVisibility.Disabled
        };
        
        var dialogResult = await _serviceProvider.GetRequiredService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions);
        if (dialogResult != ContentDialogResult.Primary) return;
        
        try
        {
            await _viewModel.SaveValueAsync();
        }
        catch (Exception exception)
        {
            var notificationService = _serviceProvider.GetRequiredService<NotificationService>();
            notificationService.ShowWarning("Invalid data", exception.Message);
        }
    }
}