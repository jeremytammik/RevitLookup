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

using System.Windows;
using System.Windows.Controls;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Views.Pages.Abstraction;

public partial class SnoopViewBase
{
    private void SelectDataGridRowStyle(DataGridRow row)
    {
        var rowDescriptor = (Descriptor) row.DataContext;
        var styleName = rowDescriptor.Value.Descriptor switch
        {
            ExceptionDescriptor => "ExceptionDataGridRowStyle",
            IDescriptorEnumerator {IsEmpty: false} => "HandleDataGridRowStyle",
            IDescriptorEnumerator => "DefaultLookupDataGridRowStyle",
            IDescriptorCollector => "HandleDataGridRowStyle",
            _ => "DefaultLookupDataGridRowStyle"
        };

        row.Style = (Style) FindResource(styleName);
    }
}

public class DataGridCellStyleSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is null) return null;

        var descriptor = (Descriptor) item;
        var presenter = (ContentPresenter) container;
        var templateName = descriptor.Value.Descriptor switch
        {
            ColorDescriptor => "ColorCellTemplate",
            ColorMediaDescriptor => "ColorCellTemplate",
            _ => "DefaultLookupDataGridCellTemplate"
        };

        return (DataTemplate) presenter.FindResource(templateName);
    }
}