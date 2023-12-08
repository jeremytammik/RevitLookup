﻿// Copyright 2003-2023 by Autodesk, Inc.
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
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.ViewModels.Converters;

[ValueConversion(typeof(WindowBackdropType), typeof(string))]
public sealed class BackgroundTypeConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var backgroundType = (WindowBackdropType) value!;
        return backgroundType switch
        {
            WindowBackdropType.None => "Disabled",
            WindowBackdropType.Acrylic => "Acrylic",
            WindowBackdropType.Tabbed => "Blur",
            WindowBackdropType.Mica => "Mica",
            WindowBackdropType.Auto => "Windows default",
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

[ValueConversion(typeof(ApplicationTheme), typeof(string))]
public sealed class ApplicationThemeConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var applicationTheme = (ApplicationTheme) value!;
        return applicationTheme switch
        {
            ApplicationTheme.Light => "Light",
            ApplicationTheme.Dark => "Dark",
            ApplicationTheme.HighContrast => "High contrast",
            ApplicationTheme.Unknown => throw new NotSupportedException(),
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