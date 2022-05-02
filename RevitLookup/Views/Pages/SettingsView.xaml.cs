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

using System.Windows.Controls;
using System.Windows.Interop;
using RevitLookup.UI.Appearance;
using Brushes = System.Windows.Media.Brushes;
using ComboBox = System.Windows.Controls.ComboBox;

namespace RevitLookup.Views.Pages;

public partial class SettingsView
{
    public SettingsView()
    {
        InitializeComponent();
    }

    public ThemeType ThemeType { get; set; } = ThemeType.Unknown;
    public BackgroundType BackgroundType { get; set; } = BackgroundType.Unknown;

    private void ThemeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox) return;

        switch (comboBox.SelectedIndex)
        {
            case 0:
                if (new WindowInteropHelper(UI.Application.Current).Handle == IntPtr.Zero)
                    UI.Application.Current.Loaded += (_, _) =>
                    {
                        Watcher.Watch(UI.Application.Current, BackgroundType, true, true);
                    };
                else
                    Watcher.Watch(UI.Application.Current, BackgroundType, true, true);
                break;
            case 1:
                ThemeType = ThemeType.Dark;
                Theme.Set(ThemeType);
                break;
            case 2:
                ThemeType = ThemeType.Light;
                Theme.Set(ThemeType);
                break;
        }

        if (ComboBoxBackground != null) ApplyBackgroundEffect(ComboBoxBackground.SelectedIndex);
    }

    private void BackgroundSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox) return;
        ApplyBackgroundEffect(comboBox.SelectedIndex);
    }

    private void ApplyBackgroundEffect(int index)
    {
        var windowHandle = new WindowInteropHelper(UI.Application.Current).Handle;
        UI.Appearance.Background.Remove(windowHandle);

        if (ThemeType == ThemeType.Dark)
            UI.Appearance.Background.ApplyDarkMode(windowHandle);
        else
            UI.Appearance.Background.RemoveDarkMode(windowHandle);

        switch (index)
        {
            case 1:
                UI.Application.Current.Background = Brushes.Transparent;
                BackgroundType = BackgroundType.Auto;
                UI.Appearance.Background.Apply(windowHandle, BackgroundType, true);
                break;
            case 2:
                UI.Application.Current.Background = Brushes.Transparent;
                BackgroundType = BackgroundType.Mica;
                UI.Appearance.Background.Apply(windowHandle, BackgroundType, true);
                break;
            case 3:
                UI.Application.Current.Background = Brushes.Transparent;
                BackgroundType = BackgroundType.Acrylic;
                UI.Appearance.Background.Apply(windowHandle, BackgroundType, true);
                break;
        }
    }
}