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
using System.Windows.Controls;
#if REVIT2023_OR_GREATER
using System.Windows.Input;
using RevitLookup.Views.Extensions;
#endif
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class EdgeDescriptor : Descriptor, IDescriptorCollector, IDescriptorConnector
{
#if REVIT2023_OR_GREATER
    private readonly Edge _edge;

#endif
    public EdgeDescriptor(Edge edge)
    {
#if REVIT2023_OR_GREATER
        _edge = edge;
#endif
        Name = $"{edge.ApproximateLength.ToString(CultureInfo.InvariantCulture)} ft";
    }

    public void RegisterMenu(ContextMenu contextMenu)
    {
#if REVIT2023_OR_GREATER
        contextMenu.AddMenuItem("ShowMenuItem")
            .SetHeader("Show edge")
            .SetCommand(_edge, edge =>
            {
                if (Context.UiDocument is null) return;
                if (edge.Reference is null) return;
                
                Application.ActionEventHandler.Raise(_ =>
                {
                    var element = edge.Reference.ElementId.ToElement(Context.Document);
                    if (element is not null) Context.UiDocument.ShowElements(element);
                    Context.UiDocument.Selection.SetReferences([edge.Reference]);
                });
            })
            .SetShortcut(ModifierKeys.Alt, Key.F7);
        
        contextMenu.AddMenuItem("SelectMenuItem")
            .SetHeader("Select edge")
            .SetCommand(_edge, edge =>
            {
                if (Context.UiDocument is null) return;
                if (edge.Reference is null) return;
                
                Application.ActionEventHandler.Raise(_ =>
                {
                    Context.UiDocument.Selection.SetReferences([edge.Reference]);
                });
            })
            .SetShortcut(ModifierKeys.Alt, Key.F8);
#endif
    }
}