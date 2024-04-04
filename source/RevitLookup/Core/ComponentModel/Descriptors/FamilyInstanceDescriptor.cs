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

public sealed class FamilyInstanceDescriptor(FamilyInstance familyInstance) : ElementDescriptor(familyInstance), IDescriptorResolver
{
    public new ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilyInstance.GetReferences) => new ResolveSet(11)
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.Back), "Back")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.Bottom), "Bottom")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.StrongReference), "Strong reference")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.WeakReference), "Weak reference")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.Front), "Front")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.Left), "Left")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.Right), "Right")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.Top), "Top")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.CenterElevation), "Center elevation")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.CenterFrontBack), "Center front back")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.CenterLeftRight), "Center left right")
                .AppendVariant(familyInstance.GetReferences(FamilyInstanceReferenceType.NotAReference), "Not a reference"),
            "Room" when parameters.Length == 1 => ResolveGetRoom(),
            "FromRoom" when parameters.Length == 1 => ResolveFromRoom(),
            "ToRoom" when parameters.Length == 1 => ResolveToRoom(),
            nameof(FamilyInstance.GetOriginalGeometry) => ResolveOriginalGeometry(),
            _ => null
        };

        ResolveSet ResolveGetRoom()
        {
            var resolveSummary = new ResolveSet(familyInstance.Document.Phases.Size);
            foreach (Phase phase in familyInstance.Document.Phases)
            {
                resolveSummary.AppendVariant(familyInstance.get_Room(phase), phase.Name);
            }

            return resolveSummary;
        }

        ResolveSet ResolveFromRoom()
        {
            var resolveSummary = new ResolveSet(familyInstance.Document.Phases.Size);
            foreach (Phase phase in familyInstance.Document.Phases)
            {
                resolveSummary.AppendVariant(familyInstance.get_FromRoom(phase), phase.Name);
            }

            return resolveSummary;
        }

        ResolveSet ResolveToRoom()
        {
            var resolveSummary = new ResolveSet(familyInstance.Document.Phases.Size);
            foreach (Phase phase in familyInstance.Document.Phases)
            {
                resolveSummary.AppendVariant(familyInstance.get_ToRoom(phase), phase.Name);
            }

            return resolveSummary;
        }

        ResolveSet ResolveOriginalGeometry()
        {
            return new ResolveSet(10)
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    View = Context.ActiveView,
                }), "Active view")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    View = Context.ActiveView,
                    IncludeNonVisibleObjects = true,
                }), "Active view, including non-visible objects")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                }), "Model, coarse detail level")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                }), "Model, fine detail level")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                }), "Model, medium detail level")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                }), "Model, undefined detail level")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                    IncludeNonVisibleObjects = true,
                }), "Model, coarse detail level, including non-visible objects")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                    IncludeNonVisibleObjects = true,
                }), "Model, fine detail level, including non-visible objects")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                    IncludeNonVisibleObjects = true,
                }), "Model, medium detail level, including non-visible objects")
                .AppendVariant(familyInstance.GetOriginalGeometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                    IncludeNonVisibleObjects = true,
                }), "Model, undefined detail level, including non-visible objects");
        }
    }
}