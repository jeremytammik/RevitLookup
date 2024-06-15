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

using System.Windows.Controls;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class PartDescriptor(Part part) : ElementDescriptor(part)
{
    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(PartUtils.IsMergedPart), _ => Variants.Single(PartUtils.IsMergedPart(part)));
        manager.Register(nameof(PartUtils.IsPartDerivedFromLink), _ => Variants.Single(PartUtils.IsPartDerivedFromLink(part)));
        manager.Register(nameof(PartUtils.GetChainLengthToOriginal), _ => Variants.Single(PartUtils.GetChainLengthToOriginal(part)));
        manager.Register(nameof(PartUtils.GetMergedParts), context =>
        {
            if (PartUtils.IsMergedPart(part))
                return Variants.Single(PartUtils.GetMergedParts(part));
            return Variants.Empty<ElementId>();
        });
        
        manager.Register(nameof(PartUtils.ArePartsValidForDivide), context => Variants.Single(PartUtils.ArePartsValidForDivide(context, [part.Id])));
        manager.Register(nameof(PartUtils.FindMergeableClusters), context => Variants.Single(PartUtils.FindMergeableClusters(context, [part.Id])));
        manager.Register(nameof(PartUtils.ArePartsValidForMerge), context => Variants.Single(PartUtils.ArePartsValidForMerge(context, [part.Id])));
        manager.Register(nameof(PartUtils.GetAssociatedPartMaker), context => Variants.Single(PartUtils.GetAssociatedPartMaker(context, part.Id)));
        manager.Register(nameof(PartUtils.GetSplittingCurves), context => Variants.Single(PartUtils.GetSplittingCurves(context, part.Id)));
        manager.Register(nameof(PartUtils.GetSplittingElements), context => Variants.Single(PartUtils.GetSplittingElements(context, part.Id)));
        manager.Register(nameof(PartUtils.HasAssociatedParts), context => Variants.Single(PartUtils.HasAssociatedParts(context, part.Id)));
    }
}