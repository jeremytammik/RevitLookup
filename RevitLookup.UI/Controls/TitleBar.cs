// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RevitLookup.UI.Common;
using RevitLookup.UI.Win32;

namespace RevitLookup.UI.Controls;

/// <summary>
///     Custom navigation buttons for the window.
/// </summary>
public class TitleBar : UserControl
{
    /// <summary>
    ///     Property for <see cref="Title" />.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(TitleBar), new PropertyMetadata(null));

    /// <summary>
    ///     Property for <see cref="UseSnapLayout" />.
    /// </summary>
    public static readonly DependencyProperty UseSnapLayoutProperty = DependencyProperty.Register(
        nameof(UseSnapLayout),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

    /// <summary>
    ///     Property for <see cref="IsMaximized" />.
    /// </summary>
    public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(nameof(IsMaximized),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

    /// <summary>
    ///     Property for <see cref="ShowMaximize" />.
    /// </summary>
    public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(
        nameof(ShowMaximize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    /// <summary>
    ///     Property for <see cref="ShowMinimize" />.
    /// </summary>
    public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(
        nameof(ShowMinimize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    /// <summary>
    ///     Property for <see cref="CanMaximize" />
    /// </summary>
    public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(
        nameof(CanMaximize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    /// <summary>
    ///     Property for <see cref="Icon" />.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));

    /// <summary>
    ///     Routed event for <see cref="CloseClicked" />.
    /// </summary>
    public static readonly RoutedEvent CloseClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(CloseClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    /// <summary>
    ///     Routed event for <see cref="MaximizeClicked" />.
    /// </summary>
    public static readonly RoutedEvent MaximizeClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(MaximizeClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    /// <summary>
    ///     Routed event for <see cref="MinimizeClicked" />.
    /// </summary>
    public static readonly RoutedEvent MinimizeClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(MinimizeClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    /// <summary>
    ///     Property for <see cref="ButtonCommand" />.
    /// </summary>
    public static readonly DependencyProperty ButtonCommandProperty =
        DependencyProperty.Register(nameof(ButtonCommand),
            typeof(IRelayCommand), typeof(TitleBar), new PropertyMetadata(null));

    private User32.POINT _doubleClickPoint;
    private Window _parent;

    private SnapLayout _snapLayout;

    /// <summary>
    ///     Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded" /> event.
    /// </summary>
    public TitleBar()
    {
        SetValue(ButtonCommandProperty, new RelayCommand(o => TemplateButton_OnClick(this, o)));

        Loaded += TitleBar_Loaded;
    }

    /// <summary>
    ///     Gets or sets title displayed on the left.
    /// </summary>
    public string Title
    {
        get => (string) GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    ///     Gets or sets information whether the use Windows 11 Snap Layout.
    /// </summary>
    public bool UseSnapLayout
    {
        get => (bool) GetValue(UseSnapLayoutProperty);
        set => SetValue(UseSnapLayoutProperty, value);
    }

    /// <summary>
    ///     Gets or sets information whether the current window is maximized.
    /// </summary>
    public bool IsMaximized
    {
        get => (bool) GetValue(IsMaximizedProperty);
        internal set => SetValue(IsMaximizedProperty, value);
    }

    /// <summary>
    ///     Gets or sets information whether to show maximize button.
    /// </summary>
    public bool ShowMaximize
    {
        get => (bool) GetValue(ShowMaximizeProperty);
        set => SetValue(ShowMaximizeProperty, value);
    }

    /// <summary>
    ///     Gets or sets information whether to show minimize button.
    /// </summary>
    public bool ShowMinimize
    {
        get => (bool) GetValue(ShowMinimizeProperty);
        set => SetValue(ShowMinimizeProperty, value);
    }

    /// <summary>
    ///     Enables or disables the maximize functionality if disables the MaximizeActionOverride action won't be called
    /// </summary>
    public bool CanMaximize
    {
        get => (bool) GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
    }

    /// <summary>
    ///     Titlebar icon.
    /// </summary>
    public ImageSource Icon
    {
        get => (ImageSource) GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    ///     Command triggered after clicking the titlebar button.
    /// </summary>
    public IRelayCommand ButtonCommand => (IRelayCommand) GetValue(ButtonCommandProperty);

    /// <summary>
    ///     Lets you override the behavior of the Maximize/Restore button with an <see cref="Action" />.
    /// </summary>
    public Action<TitleBar, Window> MaximizeActionOverride { get; set; } = null;

    /// <summary>
    ///     Lets you override the behavior of the Minimize button with an <see cref="Action" />.
    /// </summary>
    public Action<TitleBar, Window> MinimizeActionOverride { get; set; } = null;

    private Window ParentWindow => _parent ??= Window.GetWindow(this);

    /// <summary>
    ///     Event triggered after clicking close button.
    /// </summary>
    public event RoutedEventHandler CloseClicked
    {
        add => AddHandler(CloseClickedEvent, value);
        remove => RemoveHandler(CloseClickedEvent, value);
    }

    /// <summary>
    ///     Event triggered after clicking maximize or restore button.
    /// </summary>
    public event RoutedEventHandler MaximizeClicked
    {
        add => AddHandler(MaximizeClickedEvent, value);
        remove => RemoveHandler(MaximizeClickedEvent, value);
    }

    /// <summary>
    ///     Event triggered after clicking minimize button.
    /// </summary>
    public event RoutedEventHandler MinimizeClicked
    {
        add => AddHandler(MinimizeClickedEvent, value);
        remove => RemoveHandler(MinimizeClickedEvent, value);
    }

    private void CloseWindow()
    {
        ParentWindow.Close();
    }

    private void MinimizeWindow()
    {
        if (MinimizeActionOverride != null)
        {
            MinimizeActionOverride(this, _parent);
            return;
        }

        ParentWindow.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow()
    {
        if (!CanMaximize) return;

        if (MaximizeActionOverride != null)
        {
            MaximizeActionOverride(this, _parent);
            return;
        }

        if (ParentWindow.WindowState == WindowState.Normal)
        {
            IsMaximized = true;
            ParentWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            IsMaximized = false;
            ParentWindow.WindowState = WindowState.Normal;
        }
    }

    private void InitializeSnapLayout(Button maximizeButton)
    {
        if (!SnapLayout.IsSupported())
            return;

        _snapLayout = new SnapLayout();
        _snapLayout.Register(maximizeButton);
    }

    private void TitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        // It may look ugly, but at the moment it works surprisingly well

        var maximizeButton = (Button) Template.FindName("ButtonMaximize", this);

        if (maximizeButton != null && UseSnapLayout)
            InitializeSnapLayout(maximizeButton);

        var rootGrid = (Grid) Template.FindName("RootGrid", this);

        if (rootGrid != null)
        {
            rootGrid.MouseLeftButtonDown += RootGrid_MouseLeftButtonDown;
            rootGrid.MouseMove += RootGrid_MouseMove;
        }

        if (ParentWindow != null)
            ParentWindow.StateChanged += ParentWindow_StateChanged;
    }

    private void RootGrid_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || ParentWindow == null)
            return;

        // prevent firing from double clicking when the mouse never actually moved
        User32.GetCursorPos(out var currentMousePos);

        if (currentMousePos.X == _doubleClickPoint.X && currentMousePos.Y == _doubleClickPoint.Y)
            return;

        if (IsMaximized)
        {
            var screenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
            screenPoint.X /= Dpi.SystemDpiXScale();
            screenPoint.Y /= Dpi.SystemDpiYScale();

            // TODO: refine the Left value to be more accurate
            // - This calculation is good enough using the center
            //   of the titlebar, however this isn't quite accurate for
            //   how the OS operates.
            // - It should be set as a % (e.g. screen X / maximized width),
            //   then offset from the left to line up more naturally.
            ParentWindow.Left = screenPoint.X - ParentWindow.RestoreBounds.Width * 0.5;
            ParentWindow.Top = screenPoint.Y;

            // style has to be quickly swapped to avoid restore animation delay
            var style = ParentWindow.WindowStyle;
            ParentWindow.WindowStyle = WindowStyle.None;
            ParentWindow.WindowState = WindowState.Normal;
            ParentWindow.WindowStyle = style;
        }

        // Call drag move only when mouse down, check again
        // if()
        if (e.LeftButton == MouseButtonState.Pressed)
            ParentWindow.DragMove();
    }

    private void ParentWindow_StateChanged(object sender, EventArgs e)
    {
        if (ParentWindow == null)
            return;

        if (IsMaximized != (ParentWindow.WindowState == WindowState.Maximized))
            IsMaximized = ParentWindow.WindowState == WindowState.Maximized;
    }

    private void RootGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        User32.GetCursorPos(out _doubleClickPoint);

        MaximizeWindow();
    }

    private void TemplateButton_OnClick(TitleBar sender, object parameter)
    {
        var command = parameter as string;

        switch (command)
        {
            case "close":
                RaiseEvent(new RoutedEventArgs(CloseClickedEvent, this));
                CloseWindow();
                break;

            case "minimize":
                RaiseEvent(new RoutedEventArgs(MinimizeClickedEvent, this));
                MinimizeWindow();
                break;

            case "maximize":
                RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                MaximizeWindow();
                break;
        }
    }
}