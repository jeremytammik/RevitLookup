// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

/// <summary>
/// A DataGrid control that displays data in rows and columns and allows
/// for the entering and editing of data.
/// </summary>
public class DataGrid : System.Windows.Controls.DataGrid
{
    /// <summary>
    /// The DependencyProperty that represents the <see cref="CheckBoxColumnElementStyle"/> property.
    /// </summary>
    public static readonly DependencyProperty CheckBoxColumnElementStyleProperty = DependencyProperty.Register(nameof(CheckBoxColumnElementStyle),
        typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// The DependencyProperty that represents the <see cref="CheckBoxColumnEditingElementStyle"/> property.
    /// </summary>
    public static readonly DependencyProperty CheckBoxColumnEditingElementStyleProperty = DependencyProperty.Register(nameof(CheckBoxColumnEditingElementStyle),
        typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null));

    /// <summary>
    /// A style to apply to all checkbox column in the DataGrid
    /// </summary>
    public Style CheckBoxColumnElementStyle
    {
        get => (Style)GetValue(CheckBoxColumnElementStyleProperty);
        set => SetValue(CheckBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// A style to apply to all checkbox column in the DataGrid
    /// </summary>
    public Style CheckBoxColumnEditingElementStyle
    {
        get => (Style)GetValue(CheckBoxColumnEditingElementStyleProperty);
        set => SetValue(CheckBoxColumnEditingElementStyleProperty, value);
    }
    
    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        base.OnItemsSourceChanged(oldValue, newValue);
        ItemsSourceChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler ItemsSourceChanged;
}