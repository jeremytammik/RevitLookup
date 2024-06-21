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

using System.Windows.Controls;
using RevitLookup.ViewModels.Dialogs;
using Wpf.Ui;

namespace RevitLookup.Views.Dialogs;

public sealed partial class FamilySizeTableShowDialog
{
    private readonly IServiceProvider _serviceProvider;
    private bool _isEditable;
    
    public FamilySizeTableShowDialog(IServiceProvider serviceProvider, FamilySizeTableManager manager, string tableName)
    {
        _isEditable = true;
        DataContext = new FamilySizeTableShowDialogViewModel(manager, tableName);
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }
    
    public FamilySizeTableShowDialog(IServiceProvider serviceProvider, FamilySizeTable table)
    {
        DataContext = new FamilySizeTableShowDialogViewModel(table);
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
        if (_isEditable)
        {
            dialogOptions.PrimaryButtonText = "Save and close";
        }
        
        await _serviceProvider.GetService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions);
    }
}