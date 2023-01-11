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
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Common.Interfaces;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.ViewModels.Contracts;
using DataGrid = RevitLookup.UI.Controls.DataGrid;

namespace RevitLookup.Views.Pages;

public sealed partial class SnoopView : INavigableView<ISnoopViewModel>
{
    public SnoopView(ISnoopService viewModel)
    {
        ViewModel = (ISnoopViewModel) viewModel;
        InitializeComponent();
        // TreeView.ItemContainerGenerator.StatusChanged += SelectItem;
    }
    //
    // private void SelectItem(object sender, EventArgs _)
    // {
    //     var generator = (ItemContainerGenerator) sender;
    //     if (generator.Status != GeneratorStatus.ContainersGenerated)
    //     {
    //         TreeView.UpdateLayout();
    //     }
    //
    //     if (generator.Items.Count > 0)
    //     {
    //         TreeViewItem firstItem = (TreeViewItem)TreeView.ItemContainerGenerator.ContainerFromIndex(0);
    //         firstItem.IsSelected = true;
    //     }
    // }

    public ISnoopViewModel ViewModel { get; }

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

    /// <summary>
    ///     Bypasses fixed header size when the collection is empty
    /// </summary>
    private void UpdateDataGridGroupStyle(object sender, EventArgs e)
    {
        var dataGrid = (DataGrid) sender;
        UpdateItemsControlGroupStyle(dataGrid, "DataGridGroupStyle");
    }

    private void UpdateItemsControlGroupStyle(ItemsControl control, string style)
    {
        if (control.Items.Count == 0)
        {
            if (control.GroupStyle.Count > 0) control.GroupStyle.Clear();
            return;
        }

        if (DataGrid.Items.GroupDescriptions!.Count == 0) DataGrid.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Descriptor.Type)));
        var groupStyle = (GroupStyle) control.TryFindResource(style);
        control.GroupStyle.Add(groupStyle);
    }
}