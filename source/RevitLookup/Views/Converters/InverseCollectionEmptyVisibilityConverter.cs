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

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.Views.Converters;

public sealed class InverseCollectionEmptyVisibilityConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 1)
        {
            var count = (int) values[0]!;
            if (count == 0) return Visibility.Collapsed;
        }
        else if (values.Length == 2)
        {
            var collection = (IReadOnlyCollection<SnoopableObject>) values[0]!;
            var count = (int) values[1]!;
            
            if (collection.Count == 0) return Visibility.Collapsed;
            if (count == 0) return Visibility.Collapsed;
        }
        else
        {
            throw new ArgumentException("Invalid parameter");
        }
        
        return Visibility.Visible;
    }
    
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}