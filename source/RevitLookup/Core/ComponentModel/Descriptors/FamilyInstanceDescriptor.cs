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
using Autodesk.Revit.DB.Architecture;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class FamilyInstanceDescriptor(FamilyInstance familyInstance) : ElementDescriptor(familyInstance)
{
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            "Room" when parameters.Length == 1 => ResolveGetRoom,
            "FromRoom" when parameters.Length == 1 => ResolveFromRoom,
            "ToRoom" when parameters.Length == 1 => ResolveToRoom,
            nameof(FamilyInstance.GetOriginalGeometry) => ResolveOriginalGeometry,
            nameof(FamilyInstance.GetReferences) => ResolveGetReferences,
            _ => null
        };
        
        IVariants ResolveGetRoom()
        {
            var variants = new Variants<Room>(familyInstance.Document.Phases.Size);
            foreach (Phase phase in familyInstance.Document.Phases)
            {
                if (familyInstance.GetPhaseStatus(phase.Id) == ElementOnPhaseStatus.Future) continue;
                variants.Add(familyInstance.get_Room(phase), phase.Name);
            }
            
            return variants;
        }
        
        IVariants ResolveFromRoom()
        {
            var variants = new Variants<Room>(familyInstance.Document.Phases.Size);
            foreach (Phase phase in familyInstance.Document.Phases)
            {
                if (familyInstance.GetPhaseStatus(phase.Id) == ElementOnPhaseStatus.Future) continue;
                variants.Add(familyInstance.get_FromRoom(phase), phase.Name);
            }
            
            return variants;
        }
        
        IVariants ResolveToRoom()
        {
            var variants = new Variants<Room>(familyInstance.Document.Phases.Size);
            foreach (Phase phase in familyInstance.Document.Phases)
            {
                if (familyInstance.GetPhaseStatus(phase.Id) == ElementOnPhaseStatus.Future) continue;
                variants.Add(familyInstance.get_ToRoom(phase), phase.Name);
            }
            
            return variants;
        }
        
        IVariants ResolveOriginalGeometry()
        {
            return new Variants<GeometryElement>(10)
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    View = Context.ActiveView,
                }), "Active view")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    View = Context.ActiveView,
                    IncludeNonVisibleObjects = true,
                }), "Active view, including non-visible objects")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                }), "Model, coarse detail level")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                }), "Model, fine detail level")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                }), "Model, medium detail level")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                }), "Model, undefined detail level")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                    IncludeNonVisibleObjects = true,
                }), "Model, coarse detail level, including non-visible objects")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                    IncludeNonVisibleObjects = true,
                }), "Model, fine detail level, including non-visible objects")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                    IncludeNonVisibleObjects = true,
                }), "Model, medium detail level, including non-visible objects")
                .Add(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                    IncludeNonVisibleObjects = true,
                }), "Model, undefined detail level, including non-visible objects");
        }
        
        IVariants ResolveGetReferences()
        {
            return new Variants<IList<Reference>>(11)
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.Back), "Back")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.Bottom), "Bottom")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.StrongReference), "Strong reference")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.WeakReference), "Weak reference")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.Front), "Front")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.Left), "Left")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.Right), "Right")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.Top), "Top")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.CenterElevation), "Center elevation")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.CenterFrontBack), "Center front back")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.CenterLeftRight), "Center left right")
                .Add(familyInstance.GetReferences(FamilyInstanceReferenceType.NotAReference), "Not a reference");
        }
    }
    
    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds),
            _ => AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(familyInstance));
        manager.Register(nameof(AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance),
            _ => AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance(familyInstance));
    }
}