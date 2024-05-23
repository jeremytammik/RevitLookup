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

using Microsoft.Extensions.Logging;
using RevitLookup.Core.Servers;
using RevitLookup.ViewModels.Dialogs;
using Wpf.Ui;

namespace RevitLookup.Views.Dialogs;

public sealed partial class VisualizationDialog
{
    private readonly IServiceProvider _serviceProvider;
    private readonly VisualizationViewModel _viewModel;
    
    public VisualizationDialog(IServiceProvider serviceProvider, Face face)
    {
        var logger = serviceProvider.GetService<ILogger<FaceVisualizationServer>>();
        
        _serviceProvider = serviceProvider;
        _viewModel = new VisualizationViewModel(face, logger);
        
        DataContext = _viewModel;
        InitializeComponent();
        MonitorServerConnection();
    }
    
    public async Task ShowAsync()
    {
        var dialogOptions = new SimpleContentDialogCreateOptions
        {
            Title = "Visualization settings",
            Content = this,
            CloseButtonText = "Close",
            DialogMaxWidth = 500,
            DialogMaxHeight = 450
        };
        
        await _serviceProvider.GetService<IContentDialogService>().ShowSimpleDialogAsync(dialogOptions);
    }
    
    private void MonitorServerConnection()
    {
        _viewModel.RegisterServer();
        Unloaded += (_, _) => _viewModel.UnregisterServer();
    }
}