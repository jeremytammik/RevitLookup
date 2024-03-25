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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Utils;
using Wpf.Ui.Controls;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using TreeView = Wpf.Ui.Controls.TreeView;

namespace RevitLookup.Views.Pages.Abstraction;

public partial class SnoopViewBase : Page, INavigableView<ISnoopViewModel>
{
    private readonly TreeView _treeViewControl;
    private readonly DataGrid _dataGridControl;
    private readonly ISettingsService _settingsService;

    protected SnoopViewBase(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        AddShortcuts();
    }

    protected TreeView TreeViewControl
    {
        get => _treeViewControl;
        init
        {
            _treeViewControl = value;
            OnTreeChanged(value);
        }
    }

    protected DataGrid DataGridControl
    {
        get => _dataGridControl;
        init
        {
            _dataGridControl = value;
            OnGridChanged(value);
        }
    }

    protected UIElement SearchBoxControl { get; init; }
    public ISnoopViewModel ViewModel { get; protected init; }

    /// <summary>
    ///     Handle data grid reference changed event
    /// </summary>
    /// <remarks>
    ///     Data grid initialization
    /// </remarks>
    private void OnTreeChanged(TreeView control)
    {
        control.ItemContainerGenerator.StatusChanged += OnTreeViewItemGenerated;
    }

    /// <summary>
    ///     Handle tree view source changed
    /// </summary>
    /// <remarks>
    ///     Tree view initialization, apply transition
    /// </remarks>
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

    /// <summary>
    ///     Handle tree view item loaded
    /// </summary>
    /// <remarks>
    ///     TreeView item customization after loading
    /// </remarks>
    private void OnTreeViewItemGenerated(object sender, EventArgs _)
    {
        var generator = (ItemContainerGenerator) sender;
        if (generator.Status == GeneratorStatus.ContainersGenerated)
        {
            foreach (var item in generator.Items)
            {
                var treeItem = (ItemsControl) generator.ContainerFromItem(item);
                if (treeItem is null) continue;

                treeItem.Loaded -= OnTreeItemLoaded;
                treeItem.PreviewMouseLeftButtonUp -= OnTreeItemClicked;

                treeItem.Loaded += OnTreeItemLoaded;
                treeItem.PreviewMouseLeftButtonUp += OnTreeItemClicked;

                if (treeItem.Items.Count > 0)
                {
                    treeItem.ItemContainerGenerator.StatusChanged -= OnTreeViewItemGenerated;
                    treeItem.ItemContainerGenerator.StatusChanged += OnTreeViewItemGenerated;
                }
            }
        }
    }

    /// <summary>
    ///     Handle tree view loaded event
    /// </summary>
    /// <remarks>
    ///     Create tree view tooltips, menus
    /// </remarks>
    private void OnTreeItemLoaded(object sender, RoutedEventArgs args)
    {
        var element = (FrameworkElement) sender;
        switch (element.DataContext)
        {
            case SnoopableObject context:
                CreateTreeTooltip(context.Descriptor, element);
                CreateTreeContextMenu(context.Descriptor, element);
                break;
        }
    }

    /// <summary>
    ///     Handle tree view reference changed event
    /// </summary>
    /// <remarks>
    ///     Tree view initialization, apply transition
    /// </remarks>
    private async void SetupTreeView()
    {
        // Await Frame transition. GetMembers freezes the thread and breaks the animation
        await Task.Delay(_settingsService.TransitionDuration);

        ExpandFirstTreeGroup();
    }

    /// <summary>
    ///     Handle data grid reference changed event
    /// </summary>
    /// <remarks>
    ///     Data grid initialization, validation
    /// </remarks>
    private void OnGridChanged(DataGrid control)
    {
        control.Loaded += (sender, _) =>
        {
            var dataGrid = (DataGrid) sender;
            ValidateTimeColumn(dataGrid);
            CreateGridContextMenu(dataGrid);
        };
        control.ItemsSourceChanged += OnGridItemsSourceChanged;
    }

    /// <summary>
    ///     Handle data grid source changed
    /// </summary>
    /// <remarks>
    ///     Group and sort items
    /// </remarks>
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

    /// <summary>
    ///     Handle data grid row loading event
    /// </summary>
    /// <remarks>
    ///     Select row style
    /// </remarks>
    protected void OnGridRowLoading(object sender, DataGridRowEventArgs args)
    {
        var row = args.Row;
        row.Loaded += OnGridRowLoaded;
        row.PreviewMouseLeftButtonUp += OnGridRowClicked;
        SelectDataGridRowStyle(row);
    }

    /// <summary>
    ///     Handle data grid row loaded event
    /// </summary>
    /// <remarks>
    ///     Create tooltips, context menu
    /// </remarks>
    protected void OnGridRowLoaded(object sender, RoutedEventArgs args)
    {
        var element = (FrameworkElement) sender;
        var context = (Descriptor) element.DataContext;
        CreateGridRowTooltip(context, element);
        CreateGridRowContextMenu(context, element);
    }

    /// <summary>
    ///     Expand first tree view group after navigation
    /// </summary>
    private void ExpandFirstTreeGroup()
    {
        if (TreeViewControl.Items.Count > 3) return;

        var rootItem = VisualUtils.GetTreeViewItem(TreeViewControl, 0);
        if (rootItem is null) return;

        var nestedItem = VisualUtils.GetTreeViewItem(rootItem, 0);
        if (nestedItem is null) return;

        nestedItem.IsSelected = true;
    }

    /// <summary>
    ///     Show/hide time column
    /// </summary>
    private void ValidateTimeColumn(System.Windows.Controls.DataGrid control)
    {
        control.Columns[2].Visibility = _settingsService.ShowTimeColumn ? Visibility.Visible : Visibility.Collapsed;
    }
}