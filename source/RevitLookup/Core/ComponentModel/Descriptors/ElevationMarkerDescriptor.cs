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

using System.Reflection;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ElevationMarkerDescriptor(ElevationMarker elevationMarker) : ElementDescriptor(elevationMarker)
{
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ElevationMarker.IsAvailableIndex) => ResolveIndex,
            nameof(ElevationMarker.GetViewId) => ResolveViewId,
            _ => null
        };
        
        IVariants ResolveIndex()
        {
            var variants = new Variants<bool>(elevationMarker.MaximumViewCount);
            for (var i = 0; i < elevationMarker.MaximumViewCount; i++)
            {
                var result = elevationMarker.IsAvailableIndex(i);
                variants.Add(result, $"Index {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveViewId()
        {
            var variants = new Variants<ElementId>(elevationMarker.MaximumViewCount);
            for (var i = 0; i < elevationMarker.MaximumViewCount; i++)
            {
                if (elevationMarker.IsAvailableIndex(i)) continue;
                
                var result = elevationMarker.GetViewId(i);
                var element = result.ToElement(context);
                var name = element!.Name == string.Empty ? $"ID{element.Id}" : $"{element.Name}, ID{element.Id}";
                variants.Add(result, $"Index {i}: {name}");
            }
            
            return variants;
        }
    }
    
    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}