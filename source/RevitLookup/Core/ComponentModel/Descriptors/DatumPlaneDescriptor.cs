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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class DatumPlaneDescriptor(DatumPlane datumPlane) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(DatumPlane.CanBeVisibleInView) => ResolveCanBeVisibleInView(),
            nameof(DatumPlane.GetCurvesInView) => ResolveCurvesInView(),
            nameof(DatumPlane.GetDatumExtentTypeInView) => ResolveDatumExtentTypeInView(),
            nameof(DatumPlane.GetLeader) => ResolveLeader(),
            nameof(DatumPlane.GetPropagationViews) => ResolvePropagationViews(),
            nameof(DatumPlane.HasBubbleInView) => ResolveHasBubbleInView(),
            nameof(DatumPlane.IsBubbleVisibleInView) => ResolveBubbleVisibleInView(),
            _ => null
        };
        
        ResolveSet ResolveCanBeVisibleInView()
        {
            var views = context.EnumerateInstances<View>().ToArray();
            var resolveSummary = new ResolveSet(views.Length);
            
            foreach (var view in views)
            {
                var result = datumPlane.CanBeVisibleInView(view);
                resolveSummary.AppendVariant(result, $"{view.Name}: {result}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveDatumExtentTypeInView()
        {
            var resolveSummary = new ResolveSet(2);
            
            var resultEnd0 = datumPlane.GetDatumExtentTypeInView(DatumEnds.End0, context.ActiveView);
            var resultEnd1 = datumPlane.GetDatumExtentTypeInView(DatumEnds.End1, context.ActiveView);
            resolveSummary.AppendVariant(resultEnd0, $"End 0, Active view: {resultEnd0}");
            resolveSummary.AppendVariant(resultEnd1, $"End 1, Active view: {resultEnd1}");
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCurvesInView()
        {
            var resolveSummary = new ResolveSet(2);
            
            resolveSummary.AppendVariant(datumPlane.GetCurvesInView(DatumExtentType.Model, context.ActiveView), $"Model, Active view");
            resolveSummary.AppendVariant(datumPlane.GetCurvesInView(DatumExtentType.ViewSpecific, context.ActiveView), $"ViewSpecific, Active view");
            
            return resolveSummary;
        }
        
        ResolveSet ResolveLeader()
        {
            var resolveSummary = new ResolveSet(2);
            
            resolveSummary.AppendVariant(datumPlane.GetLeader(DatumEnds.End0, context.ActiveView), $"End 0, Active view");
            resolveSummary.AppendVariant(datumPlane.GetLeader(DatumEnds.End1, context.ActiveView), $"End 1, Active view");
            
            return resolveSummary;
        }
        
        ResolveSet ResolvePropagationViews()
        {
            var views = context.EnumerateInstances<View>().ToArray();
            var resolveSummary = new ResolveSet(views.Length);
            
            foreach (var view in views)
            {
                if (!datumPlane.CanBeVisibleInView(view)) continue;
                
                var result = datumPlane.GetPropagationViews(view);
                resolveSummary.AppendVariant(result, view.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveHasBubbleInView()
        {
            var resolveSummary = new ResolveSet(2);
            
            var resultEnd0 = datumPlane.HasBubbleInView(DatumEnds.End0, context.ActiveView);
            var resultEnd1 = datumPlane.HasBubbleInView(DatumEnds.End1, context.ActiveView);
            resolveSummary.AppendVariant(resultEnd0, $"End 0, Active view: {resultEnd0}");
            resolveSummary.AppendVariant(resultEnd1, $"End 1, Active view: {resultEnd1}");
            
            return resolveSummary;
        }
        
        ResolveSet ResolveBubbleVisibleInView()
        {
            var resolveSummary = new ResolveSet(2);
            
            var resultEnd0 = datumPlane.IsBubbleVisibleInView(DatumEnds.End0, context.ActiveView);
            var resultEnd1 = datumPlane.IsBubbleVisibleInView(DatumEnds.End1, context.ActiveView);
            resolveSummary.AppendVariant(resultEnd0, $"End 0, Active view: {resultEnd0}");
            resolveSummary.AppendVariant(resultEnd1, $"End 1, Active view: {resultEnd1}");
            
            return resolveSummary;
        }
    }
}