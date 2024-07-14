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
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.Views.Dialogs.Visualization;
using RevitLookup.Views.Extensions;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class FaceDescriptor : Descriptor, IDescriptorCollector, IDescriptorConnector
{
    private readonly Face _face;
    
    public FaceDescriptor(Face face)
    {
        _face = face;
        Name = $"{face.Area.ToString(CultureInfo.InvariantCulture)} ft²";
    }
    
    public virtual void RegisterMenu(ContextMenu contextMenu)
    {
#if REVIT2023_OR_GREATER
        contextMenu.AddMenuItem("SelectMenuItem")
            .SetCommand(_face, face =>
            {
                if (Context.UiDocument is null) return;
                if (face.Reference is null) return;
                
                Application.ActionEventHandler.Raise(_ => Context.UiDocument.Selection.SetReferences([face.Reference]));
            })
            .SetShortcut(Key.F6);
        
        contextMenu.AddMenuItem("ShowMenuItem")
            .SetCommand(_face, face =>
            {
                if (Context.UiDocument is null) return;
                if (face.Reference is null) return;
                
                Application.ActionEventHandler.Raise(_ =>
                {
                    var element = face.Reference.ElementId.ToElement(Context.Document);
                    if (element is not null) Context.UiDocument.ShowElements(element);
                    Context.UiDocument.Selection.SetReferences([face.Reference]);
                });
            })
            .SetShortcut(Key.F7);
#endif
        
        contextMenu.AddMenuItem("VisualizeMenuItem")
            .SetAvailability(_face.Area > 1e-6)
            .SetCommand(_face, async face =>
            {
                if (Context.UiDocument is null) return;
                
                var context = (ISnoopViewModel) contextMenu.DataContext;
                
                try
                {
                    var dialog = context.ServiceProvider.GetRequiredService<FaceVisualizationDialog>();
                    await dialog.ShowAsync(face);
                }
                catch (Exception exception)
                {
                    var logger = context.ServiceProvider.GetRequiredService<ILogger<FaceDescriptor>>();
                    logger.LogError(exception, "VisualizationDialog error");
                }
            })
            .SetShortcut(Key.F8);
    }
}