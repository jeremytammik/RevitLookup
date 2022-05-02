// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace RevitLookup.UI.Appearance;

/// <summary>
///     Singleton container for appearance data.
/// </summary>
internal static class AppearanceData
{
    /// <summary>
    ///     Namespace for the XAML dictionaries.
    /// </summary>
    public const string LibraryNamespace = "revitlookup.ui;";

    /// <summary>
    ///     Default <see cref="System.Uri" /> for the application theme dictionaries.
    /// </summary>
    public const string LibraryThemeDictionariesUri = "pack://application:,,,/RevitLookup.UI;component/Styles/Theme/";

    /// <summary>
    ///     Current system theme.
    /// </summary>
    public static SystemThemeType SystemTheme = SystemThemeType.Unknown;

    /// <summary>
    ///     Current application theme.
    /// </summary>
    public static ThemeType ApplicationTheme = ThemeType.Unknown;

    /// <summary>
    ///     Collection of handlers that have a background effect applied.
    /// </summary>
    public static List<IntPtr> Handlers = new();
}