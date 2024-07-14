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
#if REVIT2025_OR_GREATER
using Autodesk.Revit.DB.Structure;
#endif

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class IndependentTagDescriptor(IndependentTag tag) : ElementDescriptor(tag)
{
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
#if REVIT2025_OR_GREATER //TODO Fatal https://github.com/jeremytammik/RevitLookup/issues/225
            nameof(IndependentTag.TagText) when RebarBendingDetail.IsBendingDetail(tag) => Variants.Disabled,
#endif
            nameof(IndependentTag.CanLeaderEndConditionBeAssigned) => ResolveLeaderEndCondition,
#if REVIT2022_OR_GREATER
            nameof(IndependentTag.GetLeaderElbow) => ResolveLeaderElbow,
            nameof(IndependentTag.GetLeaderEnd) => ResolveLeaderEnd,
            nameof(IndependentTag.HasLeaderElbow) => ResolveHasLeaderElbow,
#endif
#if REVIT2023_OR_GREATER
            nameof(IndependentTag.IsLeaderVisible) => ResolveIsLeaderVisible,
#endif
            _ => null
        };
        
        IVariants ResolveLeaderEndCondition()
        {
            var conditions = Enum.GetValues(typeof(LeaderEndCondition));
            var variants = new Variants<bool>(conditions.Length);
            
            foreach (LeaderEndCondition condition in conditions)
            {
                var result = tag.CanLeaderEndConditionBeAssigned(condition);
                variants.Add(result, $"{condition}: {result}");
            }
            
            return variants;
        }
#if REVIT2022_OR_GREATER
        IVariants ResolveLeaderElbow()
        {
            var references = tag.GetTaggedReferences();
            var variants = new Variants<XYZ>(references.Count);
            
            foreach (var reference in references)
            {
#if REVIT2023_OR_GREATER
                if (!tag.IsLeaderVisible(reference)) continue;
#endif
                if (!tag.HasLeaderElbow(reference)) continue;
                
                variants.Add(tag.GetLeaderElbow(reference));
            }
            
            return variants;
        }
        
        IVariants ResolveLeaderEnd()
        {
            var references = tag.GetTaggedReferences();
            var variants = new Variants<XYZ>(references.Count);
            
            foreach (var reference in references)
            {
#if REVIT2023_OR_GREATER
                if (!tag.IsLeaderVisible(reference)) continue;
#endif
                variants.Add(tag.GetLeaderEnd(reference));
            }
            
            return variants;
        }
        
        IVariants ResolveHasLeaderElbow()
        {
            var references = tag.GetTaggedReferences();
            var variants = new Variants<bool>(references.Count);
            foreach (var reference in references)
            {
#if REVIT2023_OR_GREATER
                if (!tag.IsLeaderVisible(reference)) continue;
#endif
                variants.Add(tag.HasLeaderElbow(reference));
            }
            
            return variants;
        }
#endif
#if REVIT2023_OR_GREATER
        
        IVariants ResolveIsLeaderVisible()
        {
            var references = tag.GetTaggedReferences();
            var variants = new Variants<bool>(references.Count);
            
            foreach (var reference in references)
            {
                variants.Add(tag.IsLeaderVisible(reference));
            }
            
            return variants;
        }
#endif
    }
    
    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}