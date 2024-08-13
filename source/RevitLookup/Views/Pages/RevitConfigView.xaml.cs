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
using RevitLookup.ViewModels.Pages.RevitSettings;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Pages;

public sealed partial class RevitConfigView : INavigableView<RevitConfigViewModel>
{
    public RevitConfigView(RevitConfigViewModel viewModel, IContentDialogService dialogService, INavigationService navigationService)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;

        ShowWarningDialog(dialogService, navigationService);
    }

    public RevitConfigViewModel ViewModel { get; }

    private static async void ShowWarningDialog(IContentDialogService dialogService, INavigationService navigationService)
    {
        var options = new SimpleContentDialogCreateOptions
        {
            Title = "Proceed with caution",
            Content = "Changing advanced configuration preferences can impact Revit performance or security",
            PrimaryButtonText = "Accept the Risk and Continue",
            CloseButtonText = "Quit",
            DialogHorizontalAlignment = HorizontalAlignment.Center,
            DialogVerticalAlignment = VerticalAlignment.Center
        };

        var result = await dialogService.ShowSimpleDialogAsync(options);
        if (result != ContentDialogResult.Primary)
        {
            navigationService.GoBack();
        }
    }
}