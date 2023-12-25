// Copyright 2003-2023 by Autodesk, Inc.
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

using System.Drawing;

namespace RevitLookup.Utils;

/// <summary>
///     Helper class to easier work with color formats
/// </summary>
/// <remarks>
///     Implementation: https://github.com/microsoft/PowerToys/blob/main/src/common/ManagedCommon/ColorFormatHelper.cs
/// </remarks>
public static class ColorFormatUtils
{
    /// <summary>
    ///     Return a drawing color of a given <see cref="System.Windows.Media.Color"/>
    /// </summary>
    public static Color GetDrawingColor(this System.Windows.Media.Color color)
    {
        return Color.FromArgb(1, color.R, color.G, color.B);
    }

    /// <summary>
    ///     Return a drawing color of a given <see cref="Autodesk.Revit.DB.Color"/>
    /// </summary>
    public static Color GetDrawingColor(this Autodesk.Revit.DB.Color color)
    {
        return Color.FromArgb(1, color.Red, color.Green, color.Blue);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a CMYK color (cyan, magenta, yellow, black key)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The cyan[0..1], magenta[0..1], yellow[0..1] and black key[0..1] of the converted color</returns>
    public static (double Cyan, double Magenta, double Yellow, double BlackKey) ConvertToCmykColor(Color color)
    {
        // special case for black (avoid division by zero)
        if (color is { R: 0, G: 0, B: 0})
        {
            return (0d, 0d, 0d, 1d);
        }

        var red = color.R / 255d;
        var green = color.G / 255d;
        var blue = color.B / 255d;

        var blackKey = 1d - Math.Max(Math.Max(red, green), blue);

        // special case for black (avoid division by zero)
        if (1d - blackKey == 0d)
        {
            return (0d, 0d, 0d, 1d);
        }

        var cyan = (1d - red - blackKey) / (1d - blackKey);
        var magenta = (1d - green - blackKey) / (1d - blackKey);
        var yellow = (1d - blue - blackKey) / (1d - blackKey);

        return (cyan, magenta, yellow, blackKey);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a HSB color (hue, saturation, brightness)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The hue [0°..360°], saturation [0..1] and brightness [0..1] of the converted color</returns>
    public static (double Hue, double Saturation, double Brightness) ConvertToHsbColor(Color color)
    {
        // HSB and HSV represents the same color space
        return ConvertToHsvColor(color);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a HSV color (hue, saturation, value)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The hue [0°..360°], saturation [0..1] and value [0..1] of the converted color</returns>
    public static (double Hue, double Saturation, double Value) ConvertToHsvColor(Color color)
    {
        var min = Math.Min(Math.Min(color.R, color.G), color.B) / 255d;
        var max = Math.Max(Math.Max(color.R, color.G), color.B) / 255d;

        return (color.GetHue(), max == 0d ? 0d : (max - min) / max, max);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a HSI color (hue, saturation, intensity)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The hue [0°..360°], saturation [0..1] and intensity [0..1] of the converted color</returns>
    public static (double Hue, double Saturation, double Intensity) ConvertToHsiColor(Color color)
    {
        // special case for black
        if (color.R == 0 && color.G == 0 && color.B == 0)
        {
            return (0d, 0d, 0d);
        }

        var red = color.R / 255d;
        var green = color.G / 255d;
        var blue = color.B / 255d;

        var intensity = (red + green + blue) / 3d;

        var min = Math.Min(Math.Min(color.R, color.G), color.B) / 255d;

        return (color.GetHue(), 1d - (min / intensity), intensity);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a HSL color (hue, saturation, lightness)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The hue [0°..360°], saturation [0..1] and lightness [0..1] values of the converted color</returns>
    public static (double Hue, double Saturation, double Lightness) ConvertToHslColor(Color color)
    {
        var min = Math.Min(Math.Min(color.R, color.G), color.B) / 255d;
        var max = Math.Max(Math.Max(color.R, color.G), color.B) / 255d;

        var lightness = (max + min) / 2d;

        if (lightness == 0d || Math.Abs(min - max) < 1e-9)
        {
            return (color.GetHue(), 0d, lightness);
        }

        if (lightness is > 0d and <= 0.5d)
        {
            return (color.GetHue(), (max - min) / (max + min), lightness);
        }

        return (color.GetHue(), (max - min) / (2d - (max + min)), lightness);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a HWB color (hue, whiteness, blackness)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The hue [0°..360°], whiteness [0..1] and blackness [0..1] of the converted color</returns>
    public static (double Hue, double Whiteness, double Blackness) ConvertToHwbColor(Color color)
    {
        var min = Math.Min(Math.Min(color.R, color.G), color.B) / 255d;
        var max = Math.Max(Math.Max(color.R, color.G), color.B) / 255d;

        return (color.GetHue(), min, 1 - max);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a CIE LAB color (LAB)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The lightness [0..100] and two chromaticities [-128..127]</returns>
    public static (double Lightness, double ChromaticityA, double ChromaticityB) ConvertToCielabColor(Color color)
    {
        var xyz = ConvertToCiexyzColor(color);
        var lab = GetCielabColorFromCieXyz(xyz.X, xyz.Y, xyz.Z);

        return lab;
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a CIE XYZ color (XYZ)
    /// The constants of the formula matches this Wikipedia page, but at a higher precision:
    /// https://en.wikipedia.org/wiki/SRGB#The_reverse_transformation_(sRGB_to_CIE_XYZ)
    /// This page provides a method to calculate the constants:
    /// http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The X [0..1], Y [0..1] and Z [0..1]</returns>
    public static (double X, double Y, double Z) ConvertToCiexyzColor(Color color)
    {
        var r = color.R / 255d;
        var g = color.G / 255d;
        var b = color.B / 255d;

        // inverse companding, gamma correction must be undone
        var rLinear = (r > 0.04045) ? Math.Pow((r + 0.055) / 1.055, 2.4) : (r / 12.92);
        var gLinear = (g > 0.04045) ? Math.Pow((g + 0.055) / 1.055, 2.4) : (g / 12.92);
        var bLinear = (b > 0.04045) ? Math.Pow((b + 0.055) / 1.055, 2.4) : (b / 12.92);

        return (
            (rLinear * 0.41239079926595948) + (gLinear * 0.35758433938387796) + (bLinear * 0.18048078840183429),
            (rLinear * 0.21263900587151036) + (gLinear * 0.71516867876775593) + (bLinear * 0.07219231536073372),
            (rLinear * 0.01933081871559185) + (gLinear * 0.11919477979462599) + (bLinear * 0.95053215224966058)
        );
    }

    /// <summary>
    /// Convert a CIE XYZ color <see cref="double"/> to a CIE LAB color (LAB) adapted to sRGB D65 white point
    /// The constants of the formula used come from this wikipedia page:
    /// https://en.wikipedia.org/wiki/CIELAB_color_space#Converting_between_CIELAB_and_CIEXYZ_coordinates
    /// </summary>
    /// <param name="x">The <see cref="x"/> represents a mix of the three CIE RGB curves</param>
    /// <param name="y">The <see cref="y"/> represents the luminance</param>
    /// <param name="z">The <see cref="z"/> is quasi-equal to blue (of CIE RGB)</param>
    /// <returns>The lightness [0..100] and two chromaticities [-128..127]</returns>
    private static (double Lightness, double ChromaticityA, double ChromaticityB) GetCielabColorFromCieXyz(double x, double y, double z)
    {
        // sRGB reference white (x=0.3127, y=0.3290, Y=1.0), actually CIE Standard Illuminant D65 truncated to 4 decimal places,
        // then converted to XYZ using the formula:
        //   X = x * (Y / y)
        //   Y = Y
        //   Z = (1 - x - y) * (Y / y)
        const double xN = 0.9504559270516717;
        const double yN = 1.0;
        const double zN = 1.0890577507598784;

        // Scale XYZ values relative to reference white
        x /= xN;
        y /= yN;
        z /= zN;

        // XYZ to CIELab transformation
        const double delta = 6d / 29;
        var m = (1d / 3) * Math.Pow(delta, -2);
        var t = Math.Pow(delta, 3);

        var fx = (x > t) ? Math.Pow(x, 1.0 / 3.0) : (x * m) + (16.0 / 116.0);
        var fy = (y > t) ? Math.Pow(y, 1.0 / 3.0) : (y * m) + (16.0 / 116.0);
        var fz = (z > t) ? Math.Pow(z, 1.0 / 3.0) : (z * m) + (16.0 / 116.0);

        var l = (116 * fy) - 16;
        var a = 500 * (fx - fy);
        var b = 200 * (fy - fz);

        return (l, a, b);
    }

    /// <summary>
    /// Convert a given <see cref="Color"/> to a natural color (hue, whiteness, blackness)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>The hue, whiteness [0..1] and blackness [0..1] of the converted color</returns>
    public static (string Hue, double Whiteness, double Blackness) ConvertToNaturalColor(Color color)
    {
        var min = Math.Min(Math.Min(color.R, color.G), color.B) / 255d;
        var max = Math.Max(Math.Max(color.R, color.G), color.B) / 255d;

        return (GetNaturalColorFromHue(color.GetHue()), min, 1 - max);
    }

    /// <summary>
    /// Return the natural color for the given hue value
    /// </summary>
    /// <param name="hue">The hue value to convert</param>
    /// <returns>A natural color</returns>
    private static string GetNaturalColorFromHue(double hue)
    {
        return hue switch
        {
            < 60d => $"R{Math.Round(hue / 0.6d, 0)}",
            < 120d => $"Y{Math.Round((hue - 60d) / 0.6d, 0)}",
            < 180d => $"G{Math.Round((hue - 120d) / 0.6d, 0)}",
            < 240d => $"C{Math.Round((hue - 180d) / 0.6d, 0)}",
            < 300d => $"B{Math.Round((hue - 240d) / 0.6d, 0)}",
            _ => $"M{Math.Round((hue - 300d) / 0.6d, 0)}"
        };
    }
}