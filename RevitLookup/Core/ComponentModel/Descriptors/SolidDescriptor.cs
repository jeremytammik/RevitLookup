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

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
#if R23_OR_GREATER
using System.Windows.Input;
#endif
using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Views.Extensions;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class SolidDescriptor : Descriptor, IDescriptorCollector, IDescriptorConnector
{
    private readonly Solid _solid;

    public SolidDescriptor(Solid solid)
    {
        _solid = solid;
        Name = $"{solid.Volume.ToString(CultureInfo.InvariantCulture)} ft³";
    }

    public void RegisterMenu(ContextMenu contextMenu, UIElement bindableElement)
    {
#if R23_OR_GREATER
        contextMenu.AddMenuItem("Show solid")
            .SetCommand(_solid, solid =>
            {
                Application.ActionEventHandler.Raise(_ =>
                {
                    if (RevitApi.UiDocument is null) return;
                    var references = solid.Faces
                        .Cast<Face>()
                        .Select(face => face.Reference)
                        .Where(reference => reference is not null)
                        .ToList();

                    if (references.Count == 0) return;

                    var element = references[0].ElementId.ToElement(RevitApi.Document);
                    if (element is not null) RevitApi.UiDocument.ShowElements(element);
                    RevitApi.UiDocument.Selection.SetReferences(references);
                });
            })
            .AddShortcut(bindableElement, ModifierKeys.Alt, Key.F7);
#endif
    }
}