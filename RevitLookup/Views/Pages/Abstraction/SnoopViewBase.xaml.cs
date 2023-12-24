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

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Utils;
using Wpf.Ui.Controls;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using TreeView = Wpf.Ui.Controls.TreeView;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace RevitLookup.Views.Pages.Abstraction;

public partial class SnoopViewBase : Page, INavigableView<ISnoopViewModel>, INavigationAware
{
    private readonly ISettingsService _settingsService;
    private readonly DataGrid _dataGridControl;

    protected SnoopViewBase(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        AddShortcuts();
    }

    protected UIElement SearchBoxControl { get; init; }
    protected TreeView TreeViewControl { get; init; }

    protected DataGrid DataGridControl
    {
        get => _dataGridControl;
        init
        {
            _dataGridControl = value;
            OnDataGridChanged(value);
        }
    }

    public ISnoopViewModel ViewModel { get; protected init; }

    protected void OnTreeSourceChanged(object sender, IEnumerable enumerable)
    {
        if (IsLoaded)
        {
            SetupTreeView();
            return;
        }

        Loaded += OnLoaded;
        return;

        void OnLoaded(object o, RoutedEventArgs args)
        {
            Loaded -= OnLoaded;
            SetupTreeView();
        }
    }

    private async void OnDataGridChanged(DataGrid control)
    {
        //Lazy init. 1 ms is enough to display data and start initialising components
        await Task.Delay(1);

        ValidateTimeColumn(control);
        CreateGridContextMenu(control);
        control.ItemsSourceChanged += OnGridItemsSourceChanged;
    }

    private void OnGridItemsSourceChanged(object sender, EventArgs _)
    {
        var dataGrid = (DataGrid) sender;

        //Clear shapingStorage for remove duplications. WpfBug?
        dataGrid.Items.GroupDescriptions!.Clear();
        dataGrid.Items.SortDescriptions.Add(new SortDescription(nameof(Descriptor.Depth), ListSortDirection.Descending));
        dataGrid.Items.SortDescriptions.Add(new SortDescription(nameof(Descriptor.MemberAttributes), ListSortDirection.Ascending));
        dataGrid.Items.SortDescriptions.Add(new SortDescription(nameof(Descriptor.Name), ListSortDirection.Ascending));
        dataGrid.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Descriptor.Type)));
    }

    public void OnNavigatedTo()
    {
        Wpf.Ui.Application.MainWindow.PreviewKeyDown += OnPageKeyPressed;
    }

    public void OnNavigatedFrom()
    {
        Wpf.Ui.Application.MainWindow.PreviewKeyDown -= OnPageKeyPressed;
    }

    /// <summary>
    ///     Enable navigation
    /// </summary>
    protected async void OnHeaderLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        //Lazy init. 1 ms is enough to display data and start initialising components
        await Task.Delay(1);

        var treeItem = VisualUtils.FindVisualParent<TreeViewItem>((DependencyObject) sender);
        if (treeItem is null) return;

        treeItem.PreviewMouseLeftButtonUp += OnTreeItemClicked;
    }

    /// <summary>
    ///     Create tooltip, menu
    /// </summary>
    protected async void OnRowLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        //Lazy init. 1 ms is enough to display data and start initialising components
        await Task.Delay(1);

        var element = (FrameworkElement) sender;
        switch (element.DataContext)
        {
            case SnoopableObject context:
                var treeItem = VisualUtils.FindVisualParent<TreeViewItem>((DependencyObject) sender);
                CreateTreeTooltip(context.Descriptor, treeItem);
                CreateTreeContextMenu(context.Descriptor, treeItem);
                break;
            case Descriptor context:
                CreateGridRowTooltip(context, element);
                CreateGridRowContextMenu(context, element);
                break;
            default:
                return;
        }
    }

    private async void SetupTreeView()
    {
        // Await Frame transition. GetMembers freezes the thread and breaks the animation
        await Task.Delay(_settingsService.TransitionDuration);

        // if (TryRestoreSelection()) return;

        ExpandFirstGroup();
    }

    [Obsolete("Low performance")]
    private bool TryRestoreSelection()
    {
        if (ViewModel.SelectedObject is null) return false;

        var treeViewItem = VisualUtils.GetTreeViewItem(TreeViewControl, ViewModel.SelectedObject);
        if (treeViewItem is null || treeViewItem.IsSelected) return false;

        TreeViewControl.SelectedItemChanged -= OnTreeItemSelected;
        treeViewItem.IsSelected = true;
        TreeViewControl.SelectedItemChanged += OnTreeItemSelected;
        return true;
    }

    private void ExpandFirstGroup()
    {
        if (TreeViewControl.Items.Count > 3) return;

        var rootItem = VisualUtils.GetTreeViewItem(TreeViewControl, 0);
        if (rootItem is null) return;

        var nestedItem = VisualUtils.GetTreeViewItem(rootItem, 0);
        if (nestedItem is null) return;

        nestedItem.IsSelected = true;
    }

    private void ValidateTimeColumn(System.Windows.Controls.DataGrid control)
    {
        control.Columns[2].Visibility = _settingsService.ShowTimeColumn ? Visibility.Visible : Visibility.Collapsed;
    }

    private Task RefreshGridAsync()
    {
        return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
    }
}