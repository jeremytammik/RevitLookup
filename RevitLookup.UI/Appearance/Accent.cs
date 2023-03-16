// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Extensions;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Lets you update the color accents of the application.
/// </summary>
public static class Accent
{
    /// <summary>
    /// The maximum value of the background HSV brightness after which the text on the accent will be turned dark.
    /// </summary>
    private const double BackgroundBrightnessThresholdValue = 80d;

    /// <summary>
    /// SystemAccentColor.
    /// </summary>
    public static Color SystemAccent
    {
        get
        {
            var resource = Application.Current.Resources["SystemAccentColor"];

            if (resource is Color color)
                return color;

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Brush of the SystemAccentColor.
    /// </summary>
    public static Brush SystemAccentBrush => new SolidColorBrush(SystemAccent);

    /// <summary>
    /// SystemAccentColorPrimary.
    /// </summary>
    public static Color PrimaryAccent
    {
        get
        {
            var resource = Application.Current.Resources["SystemAccentColorPrimary"];

            if (resource is Color color)
                return color;

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Brush of the SystemAccentColorPrimary.
    /// </summary>
    public static Brush PrimaryAccentBrush => new SolidColorBrush(PrimaryAccent);

    /// <summary>
    /// SystemAccentColorSecondary.
    /// </summary>
    public static Color SecondaryAccent
    {
        get
        {
            var resource = Application.Current.Resources["SystemAccentColorSecondary"];

            if (resource is Color color)
                return color;

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Brush of the SystemAccentColorSecondary.
    /// </summary>
    public static Brush SecondaryAccentBrush => new SolidColorBrush(SecondaryAccent);

    /// <summary>
    /// SystemAccentColorTertiary.
    /// </summary>
    public static Color TertiaryAccent
    {
        get
        {
            var resource = Application.Current.Resources["SystemAccentColorTertiary"];

            if (resource is Color color)
                return color;

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Brush of the SystemAccentColorTertiary.
    /// </summary>
    public static Brush TertiaryAccentBrush => new SolidColorBrush(TertiaryAccent);

    /// <summary>
    /// Changes the color accents of the application based on the color entered.
    /// </summary>
    /// <param name="element">Framework element</param>
    /// <param name="systemAccent">Primary accent color.</param>
    /// <param name="themeType">If <see cref="ThemeType.Dark"/>, the colors will be different.</param>
    /// <param name="systemGlassColor">If the color is taken from the Glass Color System, its brightness will be increased with the help of the operations on HSV space.</param>
    public static void Apply(FrameworkElement element, Color systemAccent, ThemeType themeType = ThemeType.Light,
        bool systemGlassColor = false)
    {
        if (systemGlassColor)
        {
            // WindowGlassColor is little darker than accent color
            systemAccent = systemAccent.UpdateBrightness(6f);
        }

        Color primaryAccent, secondaryAccent, tertiaryAccent;

        if (themeType == ThemeType.Dark)
        {
            primaryAccent = systemAccent.Update(15f, -12f);
            secondaryAccent = systemAccent.Update(30f, -24f);
            tertiaryAccent = systemAccent.Update(45f, -36f);
        }
        else
        {
            primaryAccent = systemAccent.UpdateBrightness(-5f);
            secondaryAccent = systemAccent.UpdateBrightness(-10f);
            tertiaryAccent = systemAccent.UpdateBrightness(-15f);
        }

        UpdateColorResources(
            element,
            systemAccent,
            primaryAccent,
            secondaryAccent,
            tertiaryAccent
        );
    }

    /// <summary>
    /// Changes the color accents of the application based on the entered colors.
    /// </summary>
    /// <param name="element">Framework element</param>
    /// <param name="systemAccent">Primary color.</param>
    /// <param name="primaryAccent">Alternative light or dark color.</param>
    /// <param name="secondaryAccent">Second alternative light or dark color (most used).</param>
    /// <param name="tertiaryAccent">Third alternative light or dark color.</param>
    public static void Apply(FrameworkElement element, Color systemAccent, Color primaryAccent,
        Color secondaryAccent, Color tertiaryAccent)
    {
        UpdateColorResources(element, systemAccent, primaryAccent, secondaryAccent, tertiaryAccent);
    }

    /// <summary>
    /// Applies system accent color to the application.
    /// </summary>
    public static void ApplySystemAccent(FrameworkElement element)
    {
        Apply(element, GetColorizationColor(), Theme.GetAppTheme());
    }

    /// <summary>
    /// Gets current Desktop Window Manager colorization color.
    /// <para>It should be the color defined in the system Personalization.</para>
    /// </summary>
    public static Color GetColorizationColor()
    {
        return UnsafeNativeMethods.GetDwmColor();
    }

    /// <summary>
    /// Updates application resources.
    /// </summary>
    private static void UpdateColorResources(FrameworkElement element, Color systemAccent, Color primaryAccent,
        Color secondaryAccent, Color tertiaryAccent)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColor: " + systemAccent, "Wpf.Ui.Accent");
        System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColorPrimary: " + primaryAccent, "Wpf.Ui.Accent");
        System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColorSecondary: " + secondaryAccent, "Wpf.Ui.Accent");
        System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColorTertiary: " + tertiaryAccent, "Wpf.Ui.Accent");
#endif

        if (secondaryAccent.GetBrightness() > BackgroundBrightnessThresholdValue)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("INFO | Text on accent is DARK", "Wpf.Ui.Accent");
#endif
            element.Resources["TextOnAccentFillColorPrimary"] = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            element.Resources["TextOnAccentFillColorSecondary"] = Color.FromArgb(0x80, 0x00, 0x00, 0x00);
            element.Resources["TextOnAccentFillColorDisabled"] = Color.FromArgb(0x77, 0x00, 0x00, 0x00);
            element.Resources["TextOnAccentFillColorSelectedText"] = Color.FromArgb(0x00, 0x00, 0x00, 0x00);
            element.Resources["AccentTextFillColorDisabled"] = Color.FromArgb(0x5D, 0x00, 0x00, 0x00);
        }
        else
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("INFO | Text on accent is LIGHT", "Wpf.Ui.Accent");
#endif
            element.Resources["TextOnAccentFillColorPrimary"] = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            element.Resources["TextOnAccentFillColorSecondary"] = Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF);
            element.Resources["TextOnAccentFillColorDisabled"] = Color.FromArgb(0x87, 0xFF, 0xFF, 0xFF);
            element.Resources["TextOnAccentFillColorSelectedText"] = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            element.Resources["AccentTextFillColorDisabled"] = Color.FromArgb(0x5D, 0xFF, 0xFF, 0xFF);
        }

        element.Resources["SystemAccentColor"] = systemAccent;
        element.Resources["SystemAccentColorPrimary"] = primaryAccent;
        element.Resources["SystemAccentColorSecondary"] = secondaryAccent;
        element.Resources["SystemAccentColorTertiary"] = tertiaryAccent;

        element.Resources["SystemAccentBrush"] = secondaryAccent.ToBrush();
        element.Resources["SystemFillColorAttentionBrush"] = secondaryAccent.ToBrush();
        element.Resources["AccentTextFillColorPrimaryBrush"] = tertiaryAccent.ToBrush();
        element.Resources["AccentTextFillColorSecondaryBrush"] = tertiaryAccent.ToBrush();
        element.Resources["AccentTextFillColorTertiaryBrush"] = secondaryAccent.ToBrush();
        element.Resources["AccentFillColorSelectedTextBackgroundBrush"] = systemAccent.ToBrush();
        element.Resources["AccentFillColorDefaultBrush"] = secondaryAccent.ToBrush();

        element.Resources["AccentFillColorSecondaryBrush"] = secondaryAccent.ToBrush(0.9);
        element.Resources["AccentFillColorTertiaryBrush"] = secondaryAccent.ToBrush(0.8);
    }
}
