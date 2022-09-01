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
using RevitLookup.UI.Common.Interfaces;
using RevitLookup.UI.Controls.Interfaces;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.UI.Tests.ViewModels.Pages;

namespace RevitLookup.UI.Tests.Views.Pages;

public partial class AboutView : INavigableView<AboutViewModel>
{
    private readonly IDialogControl _dialogControl;

    public AboutView(AboutViewModel viewModel, IDialogService dialogService)
    {
        _dialogControl = dialogService.GetDialogControl();
        ViewModel = viewModel;
        InitializeComponent();
    }

    public AboutViewModel ViewModel { get; }

    private void ShowOpenSourceSoftware(object sender, RoutedEventArgs e)
    {
        _dialogControl.Show("Work", "In progress");
    }
}