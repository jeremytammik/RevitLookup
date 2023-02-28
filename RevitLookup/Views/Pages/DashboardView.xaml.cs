// Copyright 2003-2022 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views.Dialogs;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;

namespace RevitLookup.Views.Pages;

public sealed partial class DashboardView : INavigableView<DashboardViewModel>
{
    private readonly IContentDialogService _dialogService;

    public DashboardView(DashboardViewModel viewModel, IContentDialogService dialogService)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        _dialogService = dialogService;
    }

    public DashboardViewModel ViewModel { get; }

    private void OnClickParametersDialog(object sender, RoutedEventArgs e)
    {
        OpenUnitDialog(typeof(BuiltInParameter));
    }

    private void OnClickCategoriesDialog(object sender, RoutedEventArgs e)
    {
        OpenUnitDialog(typeof(BuiltInCategory));
    }

    private void OnClickForgeSchemaDialog(object sender, RoutedEventArgs e)
    {
        OpenUnitDialog(typeof(ForgeTypeId));
    }

    private async void OpenUnitDialog(Type unitType)
    {
        var dialog = _dialogService.CreateDialog();

        if (unitType == typeof(BuiltInParameter)) dialog.Title = "BuiltIn Parameters";
        else if (unitType == typeof(BuiltInCategory)) dialog.Title = "BuiltIn Categories";
        else if (unitType == typeof(ForgeTypeId)) dialog.Title = "Forge Schema";

        dialog.DialogWidth = 800;
        dialog.DialogHeight = 600;
        dialog.Content = new UnitsDialog(unitType);
        await dialog.ShowAsync();
    }
}