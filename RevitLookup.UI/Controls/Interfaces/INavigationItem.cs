// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;

namespace RevitLookup.UI.Controls.Interfaces;

/// <summary>
///     Navigation element.
/// </summary>
public interface INavigationItem
{
    /// <summary>
    ///     Content is the data used to generate the child elements of this control.
    /// </summary>
    public object Content { get; }

    /// <summary>
    ///     Gets information whether the page has a tag and type.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    ///     Gets information whether the current element is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    ///     <see cref="System.Windows.Controls.Page" /> type.
    /// </summary>
    public object Page { get; set; }

    /// <summary>
    ///     Add / Remove ClickEvent handler
    /// </summary>
    [Category("Behavior")]
    public event RoutedEventHandler Click;
}