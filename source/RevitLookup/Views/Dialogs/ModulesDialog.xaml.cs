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
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.ViewModels.Dialogs;
using Wpf.Ui;

namespace RevitLookup.Views.Dialogs;

public sealed partial class ModulesDialog
{
    private readonly IServiceProvider _serviceProvider;
    
    public ModulesDialog(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        DataContext = new ModulesViewModel();
        InitializeComponent();

#if NETCOREAPP
        ContainerColumn.Header = "Load context";
#else
        ContainerColumn.Header = "Domain";
#endif
    }
    
    public async Task ShowAsync()
    {
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "Modules",
            Content = this,
            CloseButtonText = "Close",
            DialogMaxWidth = 1500,
            HorizontalScrollVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollVisibility = ScrollBarVisibility.Disabled
        };
        
        await _serviceProvider.GetRequiredService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions);
    }
}