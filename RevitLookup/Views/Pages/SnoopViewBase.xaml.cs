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

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Enums;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Extensions;
using RevitLookup.Views.Utils;
using Wpf.Ui.Controls.Navigation;
using static System.Windows.Controls.Primitives.GeneratorStatus;
using MenuItem = RevitLookup.Core.Objects.MenuItem;

namespace RevitLookup.Views.Pages;

public class SnoopViewBase : Page, INavigableView<ISnoopViewModel>
{
    private readonly ISettingsService _settingsService;
    private bool _isUpdatingResults;
    private int _scrollTick;

    protected SnoopViewBase(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        AddShortcuts();
    }

    protected TreeView TreeViewControl { get; init; }
    protected DataGrid DataGridControl { get; init; }
    public ISnoopViewModel ViewModel { get; init; }

    /// <summary>
    ///     Expand treeView
    /// </summary>
    protected async void OnTreeSourceChanged(object sender, EventArgs readOnlyList)
    {
        if (TreeViewControl.Items.Count > 3) return;

        // Await Frame transition. GetMembers freezes the thread and breaks the animation
        await Task.Delay(_settingsService.TransitionDuration);

        var rootItem = VisualUtils.GetTreeViewItem(TreeViewControl, 0);
        if (rootItem is null) return;

        var nestedItem = VisualUtils.GetTreeViewItem(rootItem, 0);
        if (nestedItem is null) return;

        nestedItem.IsSelected = true;
    }

    /// <summary>
    ///     Execute collector for selection
    /// </summary>
    protected async void OnTreeSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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

        await ViewModel.FetchMembersCommand.ExecuteAsync(null);
    }

    /// <summary>
    ///     Select, expand treeView search results
    /// </summary>
    protected void OnSearchResultsChanged(object sender, EventArgs _)
    {
        if (_isUpdatingResults) return;
        _isUpdatingResults = true;

        TreeViewControl.ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorOnStatusChanged;

        void OnItemContainerGeneratorOnStatusChanged(object statusSender, EventArgs __)
        {
            var generator = (ItemContainerGenerator) statusSender;
            if (generator.Status != ContainersGenerated) return;

            generator.StatusChanged -= OnItemContainerGeneratorOnStatusChanged;

            if (ViewModel.SelectedObject is not null)
            {
                var treeViewItem = VisualUtils.GetTreeViewItem(TreeViewControl, ViewModel.SelectedObject);
                if (treeViewItem is not null && !treeViewItem.IsSelected)
                {
                    TreeViewControl.SelectedItemChanged -= OnTreeSelectionChanged;
                    treeViewItem.IsSelected = true;
                    TreeViewControl.SelectedItemChanged += OnTreeSelectionChanged;
                }
            }

            if (TreeViewControl.Items.Count == 1)
            {
                var containerFromIndex = (TreeViewItem) TreeViewControl.ItemContainerGenerator.ContainerFromIndex(0);
                if (containerFromIndex is not null) containerFromIndex.IsExpanded = true;
            }

            _isUpdatingResults = false;
        }
    }

    /// <summary>
    ///     Navigate selection in new window
    /// </summary>
    protected void OnGridMouseLeftButtonUp(object sender, RoutedEventArgs routedEventArgs)
    {
        if (DataGridControl.SelectedItems.Count != 1) return;
        ViewModel.Navigate((Descriptor) DataGridControl.SelectedItem);
    }

    /// <summary>
    ///     Disable tooltips while scrolling
    /// </summary>
    protected void OnDataGridScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (e.VerticalChange != 0) _scrollTick = Environment.TickCount;
    }

    /// <summary>
    ///     Disable tooltips while scrolling
    /// </summary>
    private void OnGridToolTipOpening(object o, ToolTipEventArgs args)
    {
        //Fixed by the tooltip work in 6.0-preview7 https://github.com/dotnet/wpf/pull/6058 but we use net48

        if (_scrollTick != 0 && Environment.TickCount - _scrollTick < 73) args.Handled = true;
    }

    /// <summary>
    ///     Create tooltip, menu
    /// </summary>
    protected async void OnRowLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        //Lazy init. 1 ms is enough to display data and start initialising components
        await Task.Delay(1);

        var element = (FrameworkElement) sender;
        Descriptor descriptor;
        switch (element.DataContext)
        {
            case SnoopableObject context:
                descriptor = context.Descriptor;
                var treeItem = VisualUtils.FindVisualParent<TreeViewItem>((DependencyObject) sender);
                CreateTreeTooltip(descriptor, treeItem);
                CreateTreeContextMenu(descriptor, treeItem);
                break;
            case Descriptor context:
                descriptor = context;
                CreateGridTooltip(descriptor, element);
                CreateGridContextMenu(descriptor, element);
                break;
            default:
                return;
        }
    }

    private void CreateTreeTooltip(Descriptor descriptor, FrameworkElement row)
    {
        row.ToolTip = new ToolTip
        {
            Content = new StringBuilder()
                .Append("Type: ")
                .AppendLine(descriptor.Type)
                .Append("Value: ")
                .Append(descriptor.Name)
                .ToString()
        };

        row.ToolTipOpening += OnGridToolTipOpening;
    }

    private void CreateGridTooltip(Descriptor descriptor, FrameworkElement row)
    {
        var builder = new StringBuilder();

        if ((descriptor.MemberAttributes & MemberAttributes.Private) != 0)
        {
            builder.Append("Private ");
        }

        if ((descriptor.MemberAttributes & MemberAttributes.Static) != 0)
        {
            builder.Append("Static ");
        }

        builder.Append(descriptor.MemberType switch
            {
                MemberType.Property => "Property: ",
                MemberType.Extension => "Extension: ",
                MemberType.Method => "Method: ",
                _ => throw new ArgumentOutOfRangeException()
            })
            .AppendLine(descriptor.Name)
            .Append("Type: ")
            .AppendLine(descriptor.Value.Descriptor.Type)
            .Append("Value: ")
            .Append(descriptor.Value.Descriptor.Name);

        if (descriptor.Value.Descriptor.Description is not null)
            builder.AppendLine()
                .Append("Description: ")
                .Append(descriptor.Value.Descriptor.Description);

        row.ToolTip = new ToolTip
        {
            Content = builder.ToString()
        };

        row.ToolTipOpening += OnGridToolTipOpening;
    }

    private void CreateTreeContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu();
        contextMenu.AddMenuItem("Copy", descriptor, parameter => Clipboard.SetText(parameter.Name))
            .AddShortcut(row, ModifierKeys.Control, Key.C);
        contextMenu.AddMenuItem("Help", descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName))
            .AddShortcut(row, new KeyGesture(Key.F1));

        row.ContextMenu = contextMenu;
        if (descriptor is IDescriptorConnector connector) AttachMenu(row, connector);
    }

    private static void CreateGridContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu();
        contextMenu.AddMenuItem("Copy", ApplicationCommands.Copy);
        var copyValue = contextMenu.AddMenuItem("Copy value", descriptor, parameter => Clipboard.SetText(parameter.Value.Descriptor.Name));
        copyValue.AddShortcut(row, ModifierKeys.Control | ModifierKeys.Shift, Key.C);
        contextMenu.AddMenuItem("Help", descriptor, parameter => HelpUtils.ShowHelp($"{parameter.TypeFullName} {parameter.Name}"))
            .AddShortcut(row, new KeyGesture(Key.F1));

        row.ContextMenu = contextMenu;
        if (descriptor.Value.Descriptor is IDescriptorConnector connector) AttachMenu(row, connector);
        if (descriptor.Value.Descriptor.Name is null) copyValue.IsEnabled = false;
    }

    private static void AttachMenu(FrameworkElement row, IDescriptorConnector connector)
    {
        MenuItem[] menuItems;
        try
        {
            menuItems = connector.RegisterMenu();
        }
        catch
        {
            return;
        }

        foreach (var menuItem in menuItems)
        {
            var item = row.ContextMenu.AddMenuItem(menuItem.Name, menuItem.Command);
            if (menuItem.Parameter is not null) item.CommandParameter = menuItem.Parameter;
            if (menuItem.Gesture is not null) item.AddShortcut(row, menuItem.Gesture);
        }
    }

    private void AddShortcuts()
    {
        var command = new RelayCommand(() => { ViewModel.RefreshMembersCommand.Execute(null); });
        InputBindings.Add(new KeyBinding(command, new KeyGesture(Key.F5)));
    }
}