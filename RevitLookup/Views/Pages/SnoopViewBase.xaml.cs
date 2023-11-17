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
using Wpf.Ui.Controls;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using TreeView = Wpf.Ui.Controls.TreeView;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace RevitLookup.Views.Pages;

public class SnoopViewBase : Page, INavigableView<ISnoopViewModel>, INavigationAware
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

        void OnLoaded(object o, RoutedEventArgs args)
        {
            Loaded -= OnLoaded;
            SetupTreeView();
        }
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
    ///     Navigate selection in new window
    /// </summary>
    protected void OnGridMouseLeftButtonUp(object sender, RoutedEventArgs routedEventArgs)
    {
        if (DataGridControl.SelectedItems.Count != 1) return;
        ViewModel.Navigate((Descriptor) DataGridControl.SelectedItem);
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

    public void OnNavigatedTo()
    {
        Wpf.Ui.Application.MainWindow.PreviewKeyDown += OnKeyPressed;
    }

    public void OnNavigatedFrom()
    {
        Wpf.Ui.Application.MainWindow.PreviewKeyDown -= OnKeyPressed;
    }

    private void OnKeyPressed(object sender, KeyEventArgs e)
    {
        if (SearchBoxControl.IsKeyboardFocused) return;
        if (e.KeyboardDevice.Modifiers != ModifierKeys.None) return;
        if (e.Key is >= Key.D0 and <= Key.Z or >= Key.NumPad0 and <= Key.NumPad9) SearchBoxControl.Focus();
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
        var contextMenu = new ContextMenu
        {
            Resources = Resources
        };

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(descriptor, parameter => Clipboard.SetText(parameter.Name))
            .SetShortcut(row, ModifierKeys.Control, Key.C);
        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp(parameter.TypeFullName))
            .SetShortcut(row, Key.F1);

        if (descriptor is IDescriptorConnector connector) connector.RegisterMenu(contextMenu, row);
        row.ContextMenu = contextMenu;
    }

    private void CreateGridContextMenu(DataGrid dataGrid)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources
        };

        contextMenu.AddMenuItem("RefreshMenuItem")
            .SetCommand(ViewModel.RefreshMembersCommand)
            .SetGestureText(Key.F5);

        contextMenu.AddSeparator();
        contextMenu.AddLabel("Columns");

        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Time")
            .SetCommand(dataGrid.Columns[2].Visibility == Visibility.Visible, parameter =>
            {
                dataGrid.Columns[2].Visibility = parameter ? Visibility.Collapsed : Visibility.Visible;
                _settingsService.IsTimeColumnAllowed = !parameter;
            });

        contextMenu.AddSeparator();
        contextMenu.AddLabel("Show");

        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Events")
            .SetCommand(_settingsService.IsEventsAllowed, parameter =>
            {
                _settingsService.IsEventsAllowed = !parameter;
                return GetRefreshGridTask();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Extensions")
            .SetCommand(_settingsService.IsExtensionsAllowed, parameter =>
            {
                _settingsService.IsExtensionsAllowed = !parameter;
                return GetRefreshGridTask();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Fields")
            .SetCommand(_settingsService.IsFieldsAllowed, parameter =>
            {
                _settingsService.IsFieldsAllowed = !parameter;
                return GetRefreshGridTask();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Non-public")
            .SetCommand(_settingsService.IsPrivateAllowed, parameter =>
            {
                _settingsService.IsPrivateAllowed = !parameter;
                return GetRefreshGridTask();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Static")
            .SetCommand(_settingsService.IsStaticAllowed, parameter =>
            {
                _settingsService.IsStaticAllowed = !parameter;
                return GetRefreshGridTask();
            });
        contextMenu.AddMenuItem("CheckableMenuItem")
            .SetHeader("Unsupported")
            .SetCommand(_settingsService.IsUnsupportedAllowed, parameter =>
            {
                _settingsService.IsUnsupportedAllowed = !parameter;
                return GetRefreshGridTask();
            });

        dataGrid.ContextMenu = contextMenu;
    }

    private void CreateGridRowContextMenu(Descriptor descriptor, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            Resources = Resources
        };

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(descriptor, parameter => Clipboard.SetText($"{parameter.Name}: {parameter.Value.Descriptor.Name}"))
            .SetShortcut(row, ModifierKeys.Control, Key.C);

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy value")
            .SetCommand(descriptor, parameter => Clipboard.SetText(parameter.Value.Descriptor.Name))
            .SetShortcut(row, ModifierKeys.Control | ModifierKeys.Shift, Key.C)
            .SetAvailability(descriptor.Value.Descriptor.Name is not null);

        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(descriptor, parameter => HelpUtils.ShowHelp($"{parameter.TypeFullName} {parameter.Name}"))
            .SetShortcut(row, new KeyGesture(Key.F1));

        if (descriptor.Value.Descriptor is IDescriptorConnector connector) connector.RegisterMenu(contextMenu, row);
        row.ContextMenu = contextMenu;
    }

    private Task GetRefreshGridTask()
    {
        return ViewModel.RefreshMembersCommand.ExecuteAsync(null);
    }

    private async void SetupTreeView()
    {
        // Await Frame transition. GetMembers freezes the thread and breaks the animation
        await Task.Delay(_settingsService.TransitionDuration);

        if (TryRestoreSelection()) return;

        ExpandFirstGroup();
    }

    private bool TryRestoreSelection()
    {
        if (ViewModel.SelectedObject is null) return false;

        var treeViewItem = VisualUtils.GetTreeViewItem(TreeViewControl, ViewModel.SelectedObject);
        if (treeViewItem is null || treeViewItem.IsSelected) return false;

        TreeViewControl.SelectedItemChanged -= OnTreeSelectionChanged;
        treeViewItem.IsSelected = true;
        TreeViewControl.SelectedItemChanged += OnTreeSelectionChanged;
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
        control.Columns[2].Visibility = _settingsService.IsTimeColumnAllowed ? Visibility.Visible : Visibility.Collapsed;
    }

    private void AddShortcuts()
    {
        var command = new AsyncRelayCommand(() => ViewModel.RefreshMembersCommand.ExecuteAsync(null));
        InputBindings.Add(new KeyBinding(command, new KeyGesture(Key.F5)));
    }
}