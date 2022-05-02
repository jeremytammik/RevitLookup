// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Controls;
using RevitLookup.UI.Common;

namespace RevitLookup.UI.Controls.Interfaces;

/// <summary>
/// Represents navigation class.
/// </summary>
public interface INavigation
{
    /// <summary>
    /// Currently used item like <see cref="INavigationItem"/>.
    /// </summary>
    public INavigationItem Current { get; }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
    /// </summary>
    public ContentPresenter Frame { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="INavigationItem"/> that will be displayed on the menu.
    /// </summary>
    public ObservableCollection<INavigationItem> Items { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="INavigationItem"/> which will be displayed at the bottom of the navigation and will not be scrolled.
    /// </summary>
    public ObservableCollection<INavigationItem> Footer { get; set; }
    
    /// <summary>
    /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during navigation.
    /// </summary>
    public event RoutedNavigationEvent Navigated;
}
