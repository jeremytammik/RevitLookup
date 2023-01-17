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
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls;
using RevitLookup.UI.Controls.Navigation;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views.Dialogs;

namespace RevitLookup.Views.Pages;

public sealed partial class AboutView : INavigableView<AboutViewModel>
{
    private readonly IDialogControl _dialogControl;

    public AboutView(AboutViewModel viewModel, IDialogService dialogService)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        _dialogControl = dialogService.GetDialogControl();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public AboutViewModel ViewModel { get; }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick += DialogControlOnButtonRightClick;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick -= DialogControlOnButtonRightClick;
    }

    private void ShowSoftwareDialog(object sender, RoutedEventArgs e)
    {
        _dialogControl.Title = "Third-Party Software";
        _dialogControl.DialogWidth = 500;
        _dialogControl.DialogHeight = 450;
        _dialogControl.Content = new OpenSourceDialog();
        _dialogControl.Show();
    }

    private void DialogControlOnButtonRightClick(object sender, RoutedEventArgs e)
    {
        _dialogControl.Hide();
    }
}