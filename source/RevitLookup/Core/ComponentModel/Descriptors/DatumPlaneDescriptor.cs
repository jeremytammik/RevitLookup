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

public sealed class DatumPlaneDescriptor : Descriptor, IDescriptorResolver
{
    private readonly DatumPlane _datumPlane;
    
    public DatumPlaneDescriptor(DatumPlane datumPlane)
    {
        _datumPlane = datumPlane;
        Name = ElementDescriptor.CreateName(datumPlane);
    }
    
    public IVariants Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(DatumPlane.CanBeVisibleInView) => ResolveCanBeVisibleInView(),
            nameof(DatumPlane.GetDatumExtentTypeInView) => ResolveDatumExtentTypeInView(),
            nameof(DatumPlane.GetPropagationViews) => ResolvePropagationViews(),
            nameof(DatumPlane.HasBubbleInView) => ResolveHasBubbleInView(),
            nameof(DatumPlane.IsBubbleVisibleInView) => ResolveBubbleVisibleInView(),
            nameof(DatumPlane.GetCurvesInView) => new Variants<IList<Curve>>(2)
                .Add(_datumPlane.GetCurvesInView(DatumExtentType.Model, context.ActiveView), "Model, Active view")
                .Add(_datumPlane.GetCurvesInView(DatumExtentType.ViewSpecific, context.ActiveView), "ViewSpecific, Active view"),
            nameof(DatumPlane.GetLeader) => new Variants<Leader>(2)
                .Add(_datumPlane.GetLeader(DatumEnds.End0, context.ActiveView), "End 0, Active view")
                .Add(_datumPlane.GetLeader(DatumEnds.End1, context.ActiveView), "End 1, Active view"),
            _ => null
        };
        
        IVariants ResolveCanBeVisibleInView()
        {
#if REVIT2025_OR_GREATER //TODO Fatal https://github.com/jeremytammik/RevitLookup/issues/225
            return Variants.Single(new NotSupportedException("Temporary disabled. Revit API critical Exception"));
#else
            var views = context.EnumerateInstances<View>().ToArray();
            var variants = new Variants<bool>(views.Length);
            
            foreach (var view in views)
            {
                var result = _datumPlane.CanBeVisibleInView(view);
                variants.Add(result, $"{view.Name}: {result}");
            }
            
            return variants;
#endif
        }
        
        IVariants ResolveDatumExtentTypeInView()
        {
            var variants = new Variants<DatumExtentType>(2);
            
            var resultEnd0 = _datumPlane.GetDatumExtentTypeInView(DatumEnds.End0, context.ActiveView);
            var resultEnd1 = _datumPlane.GetDatumExtentTypeInView(DatumEnds.End1, context.ActiveView);
            variants.Add(resultEnd0, $"End 0, Active view: {resultEnd0}");
            variants.Add(resultEnd1, $"End 1, Active view: {resultEnd1}");
            
            return variants;
        }
        
        IVariants ResolvePropagationViews()
        {
#if REVIT2025_OR_GREATER //TODO Fatal https://github.com/jeremytammik/RevitLookup/issues/225
            return Variants.Single(new NotSupportedException("Temporary disabled. Revit API critical Exception"));
#else
            var views = context.EnumerateInstances<View>().ToArray();
            var variants = new Variants<ISet<ElementId>>(views.Length);
            
            foreach (var view in views)
            {
                if (!_datumPlane.CanBeVisibleInView(view)) continue;
                
                var result = _datumPlane.GetPropagationViews(view);
                variants.Add(result, view.Name);
            }
            
            return variants;
#endif
        }
        
        IVariants ResolveHasBubbleInView()
        {
            var variants = new Variants<bool>(2);
            
            var resultEnd0 = _datumPlane.HasBubbleInView(DatumEnds.End0, context.ActiveView);
            var resultEnd1 = _datumPlane.HasBubbleInView(DatumEnds.End1, context.ActiveView);
            variants.Add(resultEnd0, $"End 0, Active view: {resultEnd0}");
            variants.Add(resultEnd1, $"End 1, Active view: {resultEnd1}");
            
            return variants;
        }
        
        IVariants ResolveBubbleVisibleInView()
        {
            var variants = new Variants<bool>(2);
            
            var resultEnd0 = _datumPlane.IsBubbleVisibleInView(DatumEnds.End0, context.ActiveView);
            var resultEnd1 = _datumPlane.IsBubbleVisibleInView(DatumEnds.End1, context.ActiveView);
            variants.Add(resultEnd0, $"End 0, Active view: {resultEnd0}");
            variants.Add(resultEnd1, $"End 1, Active view: {resultEnd1}");
            
            return variants;
        }
    }
}