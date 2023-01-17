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

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls.Navigation;
using RevitLookup.ViewModels.Contracts;
using static System.Windows.Controls.Primitives.GeneratorStatus;

namespace RevitLookup.Views.Pages;

public sealed partial class SnoopView : INavigableView<ISnoopViewModel>
{
    public SnoopView(ISnoopService viewModel)
    {
        ViewModel = (ISnoopViewModel) viewModel;
        DataContext = this;
        InitializeComponent();

        //Clear shapingStorage for remove duplications
        DataGrid.Items.GroupDescriptions!.Clear();
        DataGrid.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Descriptor.Type)));

        SelectTreeViewItem(TreeView, TreeView.Items);
        TreeView.ItemsSourceChanged += SelectTreeViewItem;
    }

    public ISnoopViewModel ViewModel { get; }

    private void SelectTreeViewItem(object sender, IEnumerable enumerable)
    {
        TreeView.SelectedItemChanged -= UpdateDataGrid;

        var collection = (IList) enumerable;
        if (collection.Count == 0) return;

        //Expand rule
        if (collection.Count < 3)
        {
            var viewGroup = (CollectionViewGroup) collection[0];
            ViewModel.CollectMembersCommand.Execute(viewGroup.Items[0]);

            var treeView = (TreeView) sender;
            treeView.ItemContainerGenerator.StatusChanged += ExpandTreeView;
            return;
        }

        TreeView.SelectedItemChanged += UpdateDataGrid;
    }

    private void ExpandTreeView(object sender, EventArgs _)
    {
        var generator = (ItemContainerGenerator) sender;
        if (generator.Status != ContainersGenerated) return;

        generator.StatusChanged -= ExpandTreeView;

        //Select first item
        var treeViewItem = (TreeViewItem) TreeView.ItemContainerGenerator.ContainerFromIndex(0);
        treeViewItem.ExpandSubtree();
        treeViewItem = (TreeViewItem) treeViewItem.ItemContainerGenerator.ContainerFromIndex(0);
        treeViewItem.IsSelected = true;

        TreeView.SelectedItemChanged += UpdateDataGrid;
    }

    private void UpdateDataGrid(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is null) return;
        ViewModel.CollectMembersCommand.ExecuteAsync(e.NewValue);
    }

    private void SnoopSelectedRow(object sender, RoutedEventArgs routedEventArgs)
    {
        if (DataGrid.SelectedItems.Count != 1) return;

        var selectedItem = (Descriptor) DataGrid.SelectedItem;
        if (selectedItem.Value.Descriptor is not IDescriptorCollector or IDescriptorEnumerator {IsEmpty: true}) return;

        var window = Host.GetService<IWindow>();
        window.Show();
        window.Context.GetService<INavigationService>()!.Navigate(typeof(SnoopView));
        window.Context.GetService<ISnoopService>()!.Snoop(selectedItem.Value);
    }
}