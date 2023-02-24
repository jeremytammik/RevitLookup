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
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls;
using RevitLookup.UI.Controls.Navigation;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views.Dialogs;

namespace RevitLookup.Views.Pages;

public sealed partial class DashboardView : INavigableView<DashboardViewModel>
{
    private readonly IDialogControl _dialogControl;

    public DashboardView(DashboardViewModel viewModel, IDialogService dialogService)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        _dialogControl = dialogService.GetDialogControl();
    }

    public DashboardViewModel ViewModel { get; }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick += DialogControlOnButtonRightClick;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick -= DialogControlOnButtonRightClick;
    }
    
    private void DialogControlOnButtonRightClick(object sender, RoutedEventArgs e)
    {
        _dialogControl.Hide();
    }

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

    private void OpenUnitDialog(Type unitType)
    {
        if (unitType == typeof(BuiltInParameter))
        {
            _dialogControl.Title = "BuiltIn Parameters";
        }
        else if(unitType == typeof(BuiltInCategory))
        {
            _dialogControl.Title = "BuiltIn Categories";
        }
        else if(unitType == typeof(ForgeTypeId))
        {
            _dialogControl.Title = "Forge Schema";
        }
        
        _dialogControl.DialogWidth = 800;
        _dialogControl.DialogHeight = 600;
        _dialogControl.Content = new UnitsDialog(unitType);
        _dialogControl.Show();
    }
}