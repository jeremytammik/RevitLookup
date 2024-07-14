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

public sealed class FamilyDescriptor(Family family) : ElementDescriptor(family)
{
    public override Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return null;
    }
    
    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(FamilySizeTableManager.GetFamilySizeTableManager), context => FamilySizeTableManager.GetFamilySizeTableManager(context, family.Id));
        manager.Register(nameof(FamilyUtils.FamilyCanConvertToFaceHostBased), context => FamilyUtils.FamilyCanConvertToFaceHostBased(context, family.Id));
        manager.Register(nameof(FamilyUtils.GetProfileSymbols), ResolveProfileSymbols);
    }
    
    private static object ResolveProfileSymbols(Document context)
    {
        var values = Enum.GetValues(typeof(ProfileFamilyUsage));
        var capacity = values.Length*2;
        var variants = new Variants<ICollection<ElementId>>(capacity);
        
        foreach (ProfileFamilyUsage value in values)
        {
            variants.Add(FamilyUtils.GetProfileSymbols(context, value, false), $"{value}, with multiple curve loops");
            variants.Add(FamilyUtils.GetProfileSymbols(context, value, true), $"{value}, with single curve loop");
        }
        
        return variants;
    }
}