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

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Controls.Navigation;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Utils;
using static System.Windows.Controls.Primitives.GeneratorStatus;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace RevitLookup.Views.Pages;

public sealed partial class SnoopView : INavigableView<ISnoopViewModel>
{
    private readonly ISettingsService _settingsService;
    private int _scrollTick;

    public SnoopView(ISnoopService viewModel, ISettingsService settingsService)
    {
        _settingsService = settingsService;
        ViewModel = (ISnoopViewModel) viewModel;
        InitializeComponent();
        DataContext = this;

        //Clear shapingStorage for remove duplications
        DataGrid.Items.GroupDescriptions!.Clear();
        DataGrid.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Descriptor.Type)));

        ViewModel.TreeSourceChanged += OnTreeSourceChanged;
        ViewModel.SearchResultsChanged += OnSearchResultsChanged;
        TreeView.SelectedItemChanged += OnTreeSelectionChanged;
    }

    public ISnoopViewModel ViewModel { get; }

    /// <summary>
    ///     Expand treeView for first opening
    /// </summary>
    private void OnTreeSourceChanged(object sender, IReadOnlyList<SnoopableObject> enumerable)
    {
        if (enumerable.Count is < 0 or > 3) return;
        TreeView.ItemContainerGenerator.StatusChanged += OnGeneratorStatusChanged;
    }

    /// <summary>
    ///     Expand treeView for first opening
    /// </summary>
    private async void OnGeneratorStatusChanged(object sender, EventArgs _)
    {
        var generator = (ItemContainerGenerator) sender;
        if (generator.Status != ContainersGenerated) return;

        generator.StatusChanged -= OnGeneratorStatusChanged;

        //Await Frame transition. GetMembers freezes the thread and breaks the animation
        await Task.Delay(_settingsService.TransitionDuration);

        var treeViewItem = (TreeViewItem) TreeView.ItemContainerGenerator.ContainerFromIndex(0);
        treeViewItem.ExpandSubtree();
        treeViewItem = (TreeViewItem) treeViewItem.ItemContainerGenerator.ContainerFromIndex(0);
        treeViewItem.IsSelected = true;
    }

    /// <summary>
    ///     Execute collector
    /// </summary>
    private async void OnTreeSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        switch (e.NewValue)
        {
            case SnoopableObject snoopableObject:
                ViewModel.SelectedObject = snoopableObject;
                break;
            case CollectionViewGroup:
                ViewModel.SelectedObject = null;
                break;
            default:
                //Internal __Canon object
                return;
        }

        await ViewModel.CollectMembersCommand.ExecuteAsync(null);
    }

    /// <summary>
    ///     Select, expand treeView search results
    /// </summary>
    private void OnSearchResultsChanged(object sender, EventArgs args)
    {
        if (ViewModel.SelectedObject is not null)
        {
            var treeViewItem = VisualUtils.GetTreeViewItem(TreeView, ViewModel.SelectedObject);
            if (treeViewItem is not null && !treeViewItem.IsSelected)
            {
                TreeView.SelectedItemChanged -= OnTreeSelectionChanged;
                treeViewItem.IsSelected = true;
                TreeView.SelectedItemChanged += OnTreeSelectionChanged;
            }
        }
        if (TreeView.Items.Count == 1)
        {
            TreeView.UpdateLayout();
            var containerFromIndex = (TreeViewItem) TreeView.ItemContainerGenerator.ContainerFromIndex(0);
            if (containerFromIndex is not null) containerFromIndex.IsExpanded = true;
        }
    }

    /// <summary>
    ///     Navigate selection in new window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    private void OnGridMouseButtonUp(object sender, RoutedEventArgs routedEventArgs)
    {
        if (DataGrid.SelectedItems.Count != 1) return;
        ViewModel.Navigate((Descriptor) DataGrid.SelectedItem);
    }

    /// <summary>
    ///     Disable tooltips while scrolling
    /// </summary>
    private void OnDataGridScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (e.VerticalChange != 0) _scrollTick = Environment.TickCount;
    }

    /// <summary>
    ///    Disable tooltips while scrolling
    /// </summary>
    private void OnGridToolTipOpening(object o, ToolTipEventArgs args)
    {
        //Fixed by the tooltip work in 6.0-preview7 https://github.com/dotnet/wpf/pull/6058 but we use net48
        if (Environment.TickCount - _scrollTick < 73) args.Handled = true;
    }

    /// <summary>
    ///     Lazy tooltip creation
    /// </summary>
    private void OnGridMouseEnter(object sender, MouseEventArgs e)
    {
        var row = (DataGridRow) sender;
        if (row.ToolTip is not null) return;

        var descriptor = (Descriptor) row.DataContext;
        var builder = new StringBuilder();
        builder.Append("Field: ");
        builder.AppendLine(descriptor.Name);
        builder.Append("Type: ");
        builder.AppendLine(descriptor.Value.Descriptor.Type);
        builder.Append("Value: ");
        builder.Append(descriptor.Value.Descriptor.Name);
        if (descriptor.Value.Descriptor.Description is not null)
        {
            builder.AppendLine();
            builder.Append("Description: ");
            builder.Append(descriptor.Value.Descriptor.Description);
        }

        row.ToolTip = new ToolTip
        {
            Content = builder.ToString()
        };

        row.ToolTipOpening += OnGridToolTipOpening;
    }
}