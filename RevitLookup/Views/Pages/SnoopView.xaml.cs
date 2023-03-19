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

using System.Windows.Data;
using System.Windows.Input;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Navigation;

namespace RevitLookup.Views.Pages;

public sealed partial class SnoopView : INavigationAware
{
    public SnoopView(IServiceProvider serviceProvider, ISettingsService settingsService) : base(settingsService)
    {
        ViewModel = (ISnoopViewModel) serviceProvider.GetService(typeof(ISnoopService));
        DataContext = this;
        InitializeComponent();
        TreeViewControl = TreeView;
        DataGridControl = DataGrid;

        //Clear shapingStorage for remove duplications. WpfBug?
        DataGrid.Items.GroupDescriptions!.Clear();
        DataGrid.Items.GroupDescriptions!.Add(new PropertyGroupDescription(nameof(Descriptor.Type)));

        ViewModel.SearchResultsChanged += OnSearchResultsChanged;
        TreeView.SelectedItemChanged += OnTreeSelectionChanged;
        ViewModel.TreeSourceChanged += OnTreeSourceChanged;
        SelectFirstTreeViewContainer();
        Theme.Apply(this, settingsService.Theme, settingsService.Background);
    }

    public void OnNavigatedTo()
    {
        Wpf.Ui.Application.Current.PreviewKeyDown += OnKeyPressed;
    }

    public void OnNavigatedFrom()
    {
        Wpf.Ui.Application.Current.PreviewKeyDown -= OnKeyPressed;
    }

    private void OnKeyPressed(object sender, KeyEventArgs e)
    {
        if (SearchBox.IsKeyboardFocused) return;
        if (e.KeyboardDevice.Modifiers != ModifierKeys.None) return;
        if (e.Key is >= Key.D0 and <= Key.Z or >= Key.NumPad0 and <= Key.NumPad9)
        {
            SearchBox.Focus();
        }
    }
}