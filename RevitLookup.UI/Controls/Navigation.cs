// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using RevitLookup.UI.Common;
using RevitLookup.UI.Controls.Interfaces;

namespace RevitLookup.UI.Controls;

// TODO: It's still a disgusting mix. Requirements are: preview in the designer, and the ability to add items with XAML.

/// <summary>
///     Base class for creating new navigation controls.
/// </summary>
public abstract class Navigation : Control, INavigation
{
    /// <summary>
    ///     Property for <see cref="SelectedPageIndex" />.
    /// </summary>
    public static readonly DependencyProperty SelectedPageIndexProperty = DependencyProperty.Register(
        nameof(SelectedPageIndex),
        typeof(int), typeof(Navigation),
        new PropertyMetadata(0));

    /// <summary>
    ///     Property for <see cref="Frame" />.
    /// </summary>
    public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
        typeof(ContentPresenter), typeof(Navigation),
        new PropertyMetadata(null));

    /// <summary>
    ///     Property for <see cref="Items" />.
    /// </summary>
    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items),
        typeof(ObservableCollection<INavigationItem>), typeof(Navigation),
        new PropertyMetadata(default(ObservableCollection<INavigationItem>), Items_OnChanged));

    /// <summary>
    ///     Property for <see cref="Footer" />.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(ObservableCollection<INavigationItem>), typeof(Navigation),
        new PropertyMetadata(default(ObservableCollection<INavigationItem>), Footer_OnChanged));

    /// <summary>
    ///     Event triggered when <see cref="Navigation" /> navigate to page.
    /// </summary>
    public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(nameof(Navigated),
        RoutingStrategy.Bubble, typeof(RoutedNavigationEvent), typeof(Navigation));

    /// <summary>
    ///     Creates new instance of <see cref="INavigation" /> and sets it's default <see cref="FrameworkElement.Loaded" /> event.
    /// </summary>
    protected Navigation()
    {
        Items = new ObservableCollection<INavigationItem>();
        Footer = new ObservableCollection<INavigationItem>();
        Loaded += LoadFirstPage;
    }

    /// <inheritdoc />
    public int SelectedPageIndex
    {
        get => (int) GetValue(SelectedPageIndexProperty);
        set => SetValue(SelectedPageIndexProperty, value);
    }

    /// <inheritdoc />
    public ContentPresenter Frame
    {
        get => (ContentPresenter) GetValue(FrameProperty);
        set => SetValue(FrameProperty, value);
    }

    /// <inheritdoc />
    public ObservableCollection<INavigationItem> Items
    {
        get => GetValue(ItemsProperty) as ObservableCollection<INavigationItem>;
        set => SetValue(ItemsProperty, value);
    }

    /// <inheritdoc />
    public ObservableCollection<INavigationItem> Footer
    {
        get => GetValue(FooterProperty) as ObservableCollection<INavigationItem>;
        set => SetValue(FooterProperty, value);
    }

    /// <inheritdoc />
    public event RoutedNavigationEvent Navigated
    {
        add => AddHandler(NavigatedEvent, value);
        remove => RemoveHandler(NavigatedEvent, value);
    }

    /// <inheritdoc />
    public INavigationItem Current { get; internal set; }

    private void LoadFirstPage(object sender, RoutedEventArgs e)
    {
        if (SelectedPageIndex < 0 || Items.Count == 0 && Footer.Count == 0) return;

        var indexShift = SelectedPageIndex;

        for (var i = 0; i < Items?.Count; i++)
        {
            if (i != indexShift) continue;

            Navigate(Items[i]);
            return;
        }

        indexShift -= Items?.Count ?? 0;

        if (indexShift < 0) return;

        for (var i = 0; i < Footer?.Count; i++)
        {
            if (i != indexShift)
                continue;

            Navigate(Footer[i]);
            return;
        }
    }

    private void Items_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (var addedItem in e.NewItems)

                if (addedItem is INavigationItem)
                    ((INavigationItem) addedItem).Click += Item_OnClicked;

        if (e.OldItems == null)
            return;

        foreach (var deletedItem in e.OldItems)
            ((INavigationItem) deletedItem).Click -= Item_OnClicked;
    }

    private void Footer_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (var addedItem in e.NewItems)
                if (addedItem is INavigationItem)
                    ((INavigationItem) addedItem).Click += Item_OnClicked;

        if (e.OldItems == null)
            return;

        foreach (var deletedItem in e.OldItems)
            ((INavigationItem) deletedItem).Click -= Item_OnClicked;
    }

    private static void Items_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // TODO: Fix Navigation direct class reference
        if (d is not Navigation navigation)
            return;

        if (navigation.Items == null)
            return;

        foreach (var navigationItem in navigation.Items)
            if (navigationItem.Page != null)
                navigationItem.Click += navigation.Item_OnClicked;

        navigation.Items.CollectionChanged += navigation.Items_OnCollectionChanged;
    }

    private static void Footer_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Navigation navigation)
            return;

        if (navigation.Footer == null)
            return;

        foreach (var navigationItem in navigation.Footer)
            if (navigationItem.Page != null)
                navigationItem.Click += navigation.Item_OnClicked;

        navigation.Footer.CollectionChanged += navigation.Footer_OnCollectionChanged;
    }

    private void Item_OnClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not INavigationItem item) return;
        Navigate(item);
    }

    private void Navigate(INavigationItem item)
    {
        if (item.Page == null) return;
        Current = item;
        Frame.Content = item.Page;

        var newEvent = new RoutedNavigationEventArgs(NavigatedEvent, this, item);
        RaiseEvent(newEvent);
    }
}