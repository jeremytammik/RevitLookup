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
using Nice3point.Revit.Toolkit;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class FamilyInstanceDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly FamilyInstance _familyInstance;
    
    public FamilyInstanceDescriptor(FamilyInstance familyInstance)
    {
        _familyInstance = familyInstance;
        Name = ElementDescriptor.CreateName(familyInstance);
    }
    
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilyInstance.GetReferences) => new ResolveSet(11)
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Back), "Back")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Bottom), "Bottom")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.StrongReference), "Strong reference")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.WeakReference), "Weak reference")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Front), "Front")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Left), "Left")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Right), "Right")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Top), "Top")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.CenterElevation), "Center elevation")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.CenterFrontBack), "Center front back")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.CenterLeftRight), "Center left right")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.NotAReference), "Not a reference"),
            "Room" when parameters.Length == 1 => ResolveGetRoom(),
            "FromRoom" when parameters.Length == 1 => ResolveFromRoom(),
            "ToRoom" when parameters.Length == 1 => ResolveToRoom(),
            nameof(FamilyInstance.GetOriginalGeometry) => ResolveOriginalGeometry(),
            _ => null
        };
        
        ResolveSet ResolveGetRoom()
        {
            var resolveSummary = new ResolveSet(_familyInstance.Document.Phases.Size);
            foreach (Phase phase in _familyInstance.Document.Phases)
            {
                resolveSummary.AppendVariant(_familyInstance.get_Room(phase), phase.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveFromRoom()
        {
            var resolveSummary = new ResolveSet(_familyInstance.Document.Phases.Size);
            foreach (Phase phase in _familyInstance.Document.Phases)
            {
                resolveSummary.AppendVariant(_familyInstance.get_FromRoom(phase), phase.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveToRoom()
        {
            var resolveSummary = new ResolveSet(_familyInstance.Document.Phases.Size);
            foreach (Phase phase in _familyInstance.Document.Phases)
            {
                resolveSummary.AppendVariant(_familyInstance.get_ToRoom(phase), phase.Name);
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveOriginalGeometry()
        {
            return new ResolveSet(10)
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    View = Context.ActiveView,
                }), "Active view")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    View = Context.ActiveView,
                    IncludeNonVisibleObjects = true,
                }), "Active view, including non-visible objects")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                }), "Model, coarse detail level")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                }), "Model, fine detail level")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                }), "Model, medium detail level")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                }), "Model, undefined detail level")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                    IncludeNonVisibleObjects = true,
                }), "Model, coarse detail level, including non-visible objects")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                    IncludeNonVisibleObjects = true,
                }), "Model, fine detail level, including non-visible objects")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                    IncludeNonVisibleObjects = true,
                }), "Model, medium detail level, including non-visible objects")
                .AppendVariant(_familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                    IncludeNonVisibleObjects = true,
                }), "Model, undefined detail level, including non-visible objects");
        }
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds),
            _ => AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(_familyInstance));
        manager.Register(nameof(AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance),
            _ => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(_familyInstance));
    }
}