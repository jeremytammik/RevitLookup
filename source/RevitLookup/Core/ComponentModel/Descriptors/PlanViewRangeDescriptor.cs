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

public sealed class PlanViewRangeDescriptor(PlanViewRange viewRange) : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(PlanViewRange.GetOffset) => ResolveGetOffset,
            nameof(PlanViewRange.GetLevelId) => ResolveGetLevelId,
            _ => null
        };
        
        IVariants ResolveGetOffset()
        {
            return new Variants<double>(4)
                .Add(viewRange.GetOffset(PlanViewPlane.TopClipPlane), "Top clip plane")
                .Add(viewRange.GetOffset(PlanViewPlane.CutPlane), "Cut plane")
                .Add(viewRange.GetOffset(PlanViewPlane.BottomClipPlane), "Bottom clip plane")
                .Add(viewRange.GetOffset(PlanViewPlane.UnderlayBottom), "Underlay bottom");
        }
        
        IVariants ResolveGetLevelId()
        {
            return new Variants<ElementId>(4)
                .Add(viewRange.GetLevelId(PlanViewPlane.TopClipPlane), "Top clip plane")
                .Add(viewRange.GetLevelId(PlanViewPlane.CutPlane), "Cut plane")
                .Add(viewRange.GetLevelId(PlanViewPlane.BottomClipPlane), "Bottom clip plane")
                .Add(viewRange.GetLevelId(PlanViewPlane.UnderlayBottom), "Underlay bottom");
        }
    }
}