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
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Views.Converters;

public sealed class CollectionEmptyVisibilityConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var collection = (IReadOnlyCollection<SnoopableObject>) value!;
        return collection.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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

public sealed class InverseCollectionSizeVisibilityConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var collection = (IReadOnlyCollection<SnoopableObject>) value!;
        return collection.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
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

public sealed class HandledDescriptorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var descriptor = (Descriptor) value!;
        if (descriptor is IDescriptorEnumerator enumerator) return !enumerator.IsEmpty;
        return descriptor is IDescriptorCollector;
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

public sealed class ExceptionDescriptorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var descriptor = (Descriptor) value!;
        return descriptor is ExceptionDescriptor;
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

public sealed class SingleDescriptorConverter : DescriptorConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ConvertInvalidNames(CreateSingleName((Descriptor) value!));
    }
}

public sealed class CombinedDescriptorConverter : DescriptorConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ConvertInvalidNames(CreateCombinedName((Descriptor) value!));
    }
}

public abstract class DescriptorConverter : MarkupExtension, IValueConverter
{
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public static string CreateCombinedName(Descriptor descriptor)
    {
        if (string.IsNullOrEmpty(descriptor.Name)) return descriptor.Name;
        return string.IsNullOrEmpty(descriptor.Description) ? descriptor.Name : $"{descriptor.Description}: {descriptor.Name}";
    }

    public static string CreateSingleName(Descriptor descriptor)
    {
        if (string.IsNullOrEmpty(descriptor.Name)) return descriptor.Name;
        return string.IsNullOrEmpty(descriptor.Description) ? descriptor.Name : descriptor.Description;
    }

    public string ConvertInvalidNames(string text)
    {
        return text switch
        {
            null => "<null>",
            "" => "<empty>",
            _ => text
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}