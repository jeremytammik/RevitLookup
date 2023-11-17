// Copyright 2003-2023 by Autodesk, Inc.
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
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Dialogs;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Dialogs;

public sealed partial class SearchElementsDialog
{
    private readonly ISnackbarService _snackbarService;
    private readonly ISnoopVisualService _snoopVisualService;
    private readonly SearchElementsViewModel _viewModel;

    public SearchElementsDialog(IServiceProvider serviceProvider, ContentPresenter contentPresenter) : base(contentPresenter)
    {
        _snoopVisualService = serviceProvider.GetService<ISnoopVisualService>();
        _snackbarService = serviceProvider.GetService<ISnackbarService>();
        InitializeComponent();
        _viewModel = new SearchElementsViewModel();
        DataContext = _viewModel;
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        //TODO
        // if (button != ContentDialogButton.Primary) return;
        // if (_viewModel.SearchIds(_snoopVisualService)) return;
        
        // _snackbarService.Show("Search elements", "There are no elements found for your request", SymbolRegular.Warning24, ControlAppearance.Caution);
        // return false;
    }
}