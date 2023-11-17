// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Markup;
using RevitLookup.Services.Contracts;
using Wpf.Ui.Appearance;

namespace RevitLookup.Views.Markup;

/// <summary>
///     Provides a dictionary implementation that contains <c>WPF UI</c> theme resources used by components and other elements of a WPF application.
/// </summary>
[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public sealed class ThemesDictionary : ResourceDictionary
{
    public ThemesDictionary()
    {
#if DEBUG
        ApplicationTheme theme;
        try
        {
            theme = Host.GetService<ISettingsService>().Theme;
        }
        catch
        {
            //UnHosted build
            theme = ApplicationTheme.Light;
        }
#else
        var theme = Host.GetService<ISettingsService>().Theme;
#endif
        var themeName = theme switch
        {
            ApplicationTheme.Dark => "Dark",
            ApplicationTheme.HighContrast => "HighContrast",
            _ => "Light"
        };

        Source = new Uri($"{ApplicationThemeManager.ThemesDictionaryPath}{themeName}.xaml", UriKind.Absolute);
    }
}