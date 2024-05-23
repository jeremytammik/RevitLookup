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

public sealed class SolidDescriptor : Descriptor, IDescriptorExtension, IDescriptorConnector
{
    private readonly Solid _solid;
    
    public SolidDescriptor(Solid solid)
    {
        _solid = solid;
        Name = $"{solid.Volume.ToString(CultureInfo.InvariantCulture)} ft³";
    }
    
    public void RegisterMenu(ContextMenu contextMenu)
    {
#if REVIT2023_OR_GREATER
        contextMenu.AddMenuItem("SelectMenuItem")
            .SetCommand(_solid, solid =>
            {
                if (Context.UiDocument is null) return;
                
                Application.ActionEventHandler.Raise(_ =>
                {
                    var references = solid.Faces
                        .Cast<Face>()
                        .Select(face => face.Reference)
                        .Where(reference => reference is not null)
                        .ToList();
                    
                    if (references.Count == 0) return;
                    
                    Context.UiDocument.Selection.SetReferences(references);
                });
            })
            .SetShortcut(Key.F6);
        
        contextMenu.AddMenuItem("ShowMenuItem")
            .SetCommand(_solid, solid =>
            {
                if (Context.UiDocument is null) return;
                
                Application.ActionEventHandler.Raise(_ =>
                {
                    var references = solid.Faces
                        .Cast<Face>()
                        .Select(face => face.Reference)
                        .Where(reference => reference is not null)
                        .ToList();
                    
                    if (references.Count == 0) return;
                    
                    var element = references[0].ElementId.ToElement(Context.Document);
                    if (element is not null) Context.UiDocument.ShowElements(element);
                    Context.UiDocument.Selection.SetReferences(references);
                });
            })
            .SetShortcut(Key.F7);
#endif
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(SolidUtils.SplitVolumes), _ => SolidUtils.SplitVolumes(_solid));
        manager.Register(nameof(SolidUtils.IsValidForTessellation), _ => SolidUtils.IsValidForTessellation(_solid));
    }
}