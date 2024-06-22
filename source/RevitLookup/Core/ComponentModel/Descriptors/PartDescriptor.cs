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

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class PartDescriptor(Part part) : ElementDescriptor(part)
{
    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(PartUtils.IsMergedPart), _ => PartUtils.IsMergedPart(part));
        manager.Register(nameof(PartUtils.IsPartDerivedFromLink), _ => PartUtils.IsPartDerivedFromLink(part));
        manager.Register(nameof(PartUtils.GetChainLengthToOriginal), _ =>PartUtils.GetChainLengthToOriginal(part));
        manager.Register(nameof(PartUtils.GetMergedParts), _ => PartUtils.GetMergedParts(part));
        manager.Register(nameof(PartUtils.ArePartsValidForDivide), context => PartUtils.ArePartsValidForDivide(context, [part.Id]));
        manager.Register(nameof(PartUtils.FindMergeableClusters), context => PartUtils.FindMergeableClusters(context, [part.Id]));
        manager.Register(nameof(PartUtils.ArePartsValidForMerge), context => PartUtils.ArePartsValidForMerge(context, [part.Id]));
        manager.Register(nameof(PartUtils.GetAssociatedPartMaker), context => PartUtils.GetAssociatedPartMaker(context, part.Id));
        manager.Register(nameof(PartUtils.GetSplittingCurves), context => PartUtils.GetSplittingCurves(context, part.Id));
        manager.Register(nameof(PartUtils.GetSplittingElements), context => PartUtils.GetSplittingElements(context, part.Id));
        manager.Register(nameof(PartUtils.HasAssociatedParts), context => PartUtils.HasAssociatedParts(context, part.Id));
    }
    
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return null;
    }
}