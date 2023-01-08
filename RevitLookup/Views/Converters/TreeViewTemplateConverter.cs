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

using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitLookup.Views.Converters;

public sealed class TreeViewTemplateConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IDictionary)
        {
            return CreateGroupTemplate();
        }

        throw new NotSupportedException($"Unsupported collection {value}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    private static object CreateGroupTemplate()
    {
        var dataTemplate = new HierarchicalDataTemplate
        {
            ItemsSource = new Binding("Value"),
            VisualTree = new FrameworkElementFactory(typeof(TextBlock)),
            ItemTemplate = new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(typeof(TextBlock))
            }
        };

        dataTemplate.VisualTree.SetBinding(TextBlock.TextProperty, new Binding("Key"));
        dataTemplate.ItemTemplate.VisualTree.SetBinding(TextBlock.TextProperty, new Binding("Descriptor.Label")
        {
            Converter = new InvalidTextConverter()
        });

        return dataTemplate;
    }
}