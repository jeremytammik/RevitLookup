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

using System.Text;
using System.Windows;
using System.Windows.Controls;
using RevitLookup.Core.Enums;

namespace RevitLookup.Views.Pages.Abstraction;

public partial class SnoopViewBase
{
    /// <summary>
    ///     Create tree view tooltips
    /// </summary>
    private void CreateTreeTooltip(Descriptor descriptor, FrameworkElement row)
    {
        row.ToolTip = new ToolTip
        {
            Content = new StringBuilder()
                .Append("Type: ")
                .AppendLine(descriptor.Type)
                .Append("Value: ")
                .Append(descriptor.Name)
                .ToString()
        };
    }

    /// <summary>
    ///     Create data grid tooltips
    /// </summary>
    private void CreateGridRowTooltip(Descriptor descriptor, FrameworkElement row)
    {
        var builder = new StringBuilder();

        if ((descriptor.MemberAttributes & MemberAttributes.Private) != 0) builder.Append("Private ");
        if ((descriptor.MemberAttributes & MemberAttributes.Static) != 0) builder.Append("Static ");
        if ((descriptor.MemberAttributes & MemberAttributes.Property) != 0) builder.Append("Property: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Extension) != 0) builder.Append("Extension: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Method) != 0) builder.Append("Method: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Event) != 0) builder.Append("Event: ");
        if ((descriptor.MemberAttributes & MemberAttributes.Field) != 0) builder.Append("Field: ");

        builder.AppendLine(descriptor.Name)
            .Append("Type: ")
            .AppendLine(descriptor.Value.Descriptor.Type)
            .Append("Value: ")
            .Append(descriptor.Value.Descriptor.Name);

        if (descriptor.Value.Descriptor.Description is not null)
            builder.AppendLine()
                .Append("Description: ")
                .Append(descriptor.Value.Descriptor.Description);

        if (descriptor.ComputationTime > 0)
            builder.AppendLine()
                .Append("Time: ")
                .Append(descriptor.ComputationTime)
                .Append(" ms");
        
        if (descriptor.AllocatedBytes > 0)
            builder.AppendLine()
                .Append("Allocated: ")
                .Append(descriptor.AllocatedBytes)
                .Append(" bytes");

        row.ToolTip = new ToolTip
        {
            Content = builder.ToString()
        };
    }
}