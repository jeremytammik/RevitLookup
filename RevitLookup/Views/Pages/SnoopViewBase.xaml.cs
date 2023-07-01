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

using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using DataGrid = Wpf.Ui.Controls.DataGrid;
using TreeView = Wpf.Ui.Controls.TreeView;

namespace RevitLookup.Views.Pages;

public class SnoopViewBase : Page, INavigableView<ISnoopViewModel>
{
    private readonly ISettingsService _settingsService;
    private readonly DataGrid _dataGridControl;
    private bool _isUpdatingResults;

    protected SnoopViewBase(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        AddShortcuts();
    }

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

    private async void OnDataGridChanged(DataGrid control)
    {
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

    /// <summary>
    ///     Expand treeView
    /// </summary>
    protected async void OnTreeSourceChanged(object sender, EventArgs args)
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
            if (generator.Status != GeneratorStatus.ContainersGenerated) return;

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

    private void ValidateTimeColumn(DataGrid control)
    {
        control.Columns[2].Visibility = _settingsService.IsTimeColumnAllowed ? Visibility.Visible : Visibility.Collapsed;
    }

    private void ToggleTimeColumn(DataGrid control)
    {
        var state = control.Columns[2].Visibility == Visibility.Visible;
        control.Columns[2].Visibility = state ? Visibility.Collapsed : Visibility.Visible;
        _settingsService.IsTimeColumnAllowed = !state;
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
                CreateGridRowTooltip(descriptor, element);
                CreateGridRowContextMenu(descriptor, element);
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
    }

    private void CreateGridRowTooltip(Descriptor descriptor, FrameworkElement row)
    {
        var builder = new StringBuilder();

        if ((descriptor.MemberAttributes & MemberAttributes.Private) != 0) builder.Append("Private ");
        if ((descriptor.MemberAttributes & MemberAttributes.Static) != 0) builder.Append("Static ");
        if ((descriptor.MemberAttributes & MemberAttributes.Property) != 0) builder.Append("Property: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Extension) != 0) builder.Append("Extension: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Method) != 0) builder.Append("Method: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Event) != 0) builder.Append("Event: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Field) != 0) builder.Append("Field: ");

        builder.AppendLine(descriptor.Name)
            .Append("Type: ")
            .AppendLine(descriptor.Value.Descriptor.Type)
            .Append("Value: ")
            .Append(descriptor.Value.Descriptor.Name);

        if (descriptor.Value.Descriptor.Description is not null)
            builder.AppendLine()
                .Append("Description: ")
                .Append(descriptor.Value.Descriptor.Description);

        builder.AppendLine()
            .Append("Time: ")
            .Append(descriptor.ComputationTime)
            .Append(" ms");

        row.ToolTip = new ToolTip
        {
            Content = builder.ToString()
        };
    }

    private void CreateTreeContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu();
        contextMenu.AddMenuItem(Resources["CopyMenuItem"])
            .SetCommand(descriptor, parameter => Clipboard.SetText(parameter.Name))
            .SetShortcut(row, ModifierKeys.Control, Key.C);
        contextMenu.AddMenuItem(Resources["HelpMenuItem"])
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName))
            .SetShortcut(row, Key.F1);

        if (descriptor is IDescriptorConnector connector) connector.RegisterMenu(contextMenu, row);
        row.ContextMenu = contextMenu;
    }

    private void CreateGridContextMenu(DataGrid dataGrid)
    {
        var contextMenu = new ContextMenu();
        contextMenu.AddMenuItem(Resources["ShowTimeMenuItem"])
            .SetCommand(() => ToggleTimeColumn(dataGrid));
        contextMenu.AddMenuItem(Resources["RefreshMenuItem"])
            .SetCommand(() => ViewModel.RefreshMembersCommand.Execute(null))
            .SetGestureText(Key.F5);

        dataGrid.ContextMenu = contextMenu;
    }

    private void CreateGridRowContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu();

        contextMenu.AddMenuItem(Resources["CopyMenuItem"])
            .SetCommand(descriptor, parameter => Clipboard.SetText($"{parameter.Name}: {parameter.Value.Descriptor.Name}"))
            .SetShortcut(row, ModifierKeys.Control, Key.C);

        contextMenu.AddMenuItem(Resources["CopyMenuItem"])
            .SetHeader("Copy value")
            .SetCommand(descriptor, parameter => Clipboard.SetText(parameter.Value.Descriptor.Name))
            .SetShortcut(row, ModifierKeys.Control | ModifierKeys.Shift, Key.C)
            .SetAvailability(descriptor.Value.Descriptor.Name is not null);

        contextMenu.AddMenuItem(Resources["HelpMenuItem"])
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp($"{parameter.TypeFullName} {parameter.Name}"))
            .SetShortcut(row, new KeyGesture(Key.F1));

        if (descriptor.Value.Descriptor is IDescriptorConnector connector) connector.RegisterMenu(contextMenu, row);
        row.ContextMenu = contextMenu;
    }

    private void AddShortcuts()
    {
        var command = new RelayCommand(() => ViewModel.RefreshMembersCommand.Execute(null));
        InputBindings.Add(new KeyBinding(command, new KeyGesture(Key.F5)));
    }
}