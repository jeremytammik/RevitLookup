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

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using RevitLookup.UI.Appearance;

namespace RevitLookup.ViewModels.Converters;

[ValueConversion(typeof(BackgroundType), typeof(string))]
public class BackgroundTypeConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return null;
        var backgroundType = (BackgroundType) value;
        return backgroundType switch
        {
            BackgroundType.Unknown => "Invalid",
            BackgroundType.Disabled => "Disabled",
            BackgroundType.Auto => "Windows default",
            BackgroundType.Mica => "Mica",
            BackgroundType.Acrylic => "Acrylic",
            BackgroundType.Tabbed => "Tabbed",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

[ValueConversion(typeof(ThemeType), typeof(string))]
public class ThemeTypeConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return null;
        var themeType = (ThemeType) value;
        return themeType switch
        {
            ThemeType.Unknown => "Invalid",
            ThemeType.Auto => "Windows default",
            ThemeType.Dark => "Dark",
            ThemeType.Light => "Light",
            ThemeType.HighContrast => "High contrast",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}