﻿// Copyright 2003-2024 by Autodesk, Inc.
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
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Dialogs;
using RevitLookup.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Dialogs;

public sealed partial class SearchElementsDialog
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SearchElementsViewModel _viewModel;

    public SearchElementsDialog(
        IContentDialogService dialogService,
        IServiceProvider serviceProvider)
        : base(dialogService.GetDialogHost())
    {
        _serviceProvider = serviceProvider;
        _viewModel = new SearchElementsViewModel();
        DataContext = _viewModel;
        InitializeComponent();
    }

    public async Task ShowDialogAsync()
    {
        var result = await ShowAsync();
        if (result != ContentDialogResult.Primary) return;

        var elements = _viewModel.SearchElements();
        if (elements.Count == 0)
        {
            var notificationService = _serviceProvider.GetRequiredService<NotificationService>();
            notificationService.ShowWarning("Search elements", "There are no elements found for your request");
            return;
        }

        _serviceProvider.GetRequiredService<ISnoopVisualService>().Snoop(new SnoopableObject(elements));
        _serviceProvider.GetRequiredService<INavigationService>().Navigate(typeof(SnoopPage));
    }
}