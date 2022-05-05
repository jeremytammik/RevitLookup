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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Interop;
using RevitLookup.UI.Appearance;
using Brushes = System.Windows.Media.Brushes;

namespace RevitLookup.ViewModels.Pages;

public sealed class SettingsViewModel : INotifyPropertyChanged
{
    private BackgroundType _currentEffect;
    private ThemeType _currentTheme;

    public SettingsViewModel()
    {
        UI.Application.Current.Loaded += (_, _) =>
        {
            CurrentTheme = ThemeType.Dark;
            CurrentEffect = BackgroundType.Mica;
        };
    }

    public List<ThemeType> Themes { get; } = new()
    {
        ThemeType.Auto,
        ThemeType.Dark,
        ThemeType.Light
    };

    public List<BackgroundType> Effects { get; } = new()
    {
        BackgroundType.Disabled,
        // BackgroundType.Auto,
        // BackgroundType.Acrylic,
        BackgroundType.Mica
        // BackgroundType.Tabbed
    };

    public ThemeType CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (value == _currentTheme) return;
            _currentTheme = value;
            ApplyTheme(value);
            OnPropertyChanged();
        }
    }

    public BackgroundType CurrentEffect
    {
        get => _currentEffect;
        set
        {
            if (value == _currentEffect) return;
            _currentEffect = value;
            ApplyBackgroundEffect(value);
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void ApplyTheme(ThemeType theme)
    {
        switch (theme)
        {
            case ThemeType.Auto:
                if (new WindowInteropHelper(UI.Application.Current).Handle == IntPtr.Zero)
                    UI.Application.Current.Loaded += (_, _) =>
                    {
                        Watcher.Watch(UI.Application.Current, CurrentEffect, true, true);
                    };
                else
                    Watcher.Watch(UI.Application.Current, CurrentEffect, true, true);
                return;
            case ThemeType.Dark:
                Theme.Apply(ThemeType.Dark);
                break;
            case ThemeType.Light:
                Theme.Apply(ThemeType.Light);
                break;
            case ThemeType.Unknown:
            case ThemeType.HighContrast:
            default:
                throw new NotSupportedException();
        }

        ApplyBackgroundEffect(CurrentEffect);
    }

    private void ApplyBackgroundEffect(BackgroundType effect)
    {
        var windowHandle = new WindowInteropHelper(UI.Application.Current).Handle;
        Background.Remove(windowHandle);

        if (CurrentTheme == ThemeType.Dark)
            Background.ApplyDarkMode(windowHandle);
        else
            Background.RemoveDarkMode(windowHandle);

        switch (effect)
        {
            case BackgroundType.Unknown:
            case BackgroundType.Disabled:
                break;
            case BackgroundType.Auto:
            case BackgroundType.Acrylic:
            case BackgroundType.Mica:
            case BackgroundType.Tabbed:
                UI.Application.Current.Background = Brushes.Transparent;
                Background.Apply(windowHandle, effect, true);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}