#if REVIT2024_OR_GREATER
// Copyright 2003-2024 by Autodesk, Inc.
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

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RevitLookup.Core;
using RevitLookup.Services.Contracts;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Views.Appearance;

public static class RevitThemeWatcher
{
    private static bool _isWatching;

    public static void Watch(WindowBackdropType background)
    {
        if (!_isWatching)
        {
            RevitShell.ActionEventHandler.Raise(_ => Context.UiApplication.ThemeChanged += ObserveThemeChanged);
            _isWatching = true;
        }

        var theme = GetCurrentTheme();
        ApplicationThemeManager.Apply(theme, background);
    }

    public static void Unwatch()
    {
        if (!_isWatching) return;

        RevitShell.ActionEventHandler.Raise(_ => Context.UiApplication.ThemeChanged -= ObserveThemeChanged);
        _isWatching = false;
    }

    private static void ObserveThemeChanged(object sender, ThemeChangedEventArgs args)
    {
        if (args.ThemeChangedType != ThemeType.UITheme) return;

        var theme = GetCurrentTheme();
        var settings = Host.GetService<ISettingsService>().GeneralSettings;
        foreach (var observedWindow in Wpf.Ui.Application.Windows)
        {
            observedWindow.Dispatcher.Invoke(() =>
            {
                Wpf.Ui.Application.MainWindow = observedWindow;
                ApplicationThemeManager.Apply(theme, settings.Background);
            });
        }
    }

    private static ApplicationTheme GetCurrentTheme()
    {
        return UIThemeManager.CurrentTheme switch
        {
            UITheme.Light => ApplicationTheme.Light,
            UITheme.Dark => ApplicationTheme.Dark,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
#endif