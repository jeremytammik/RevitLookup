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

public sealed class ForgeTypeIdDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly ForgeTypeId _typeId;
    
    public ForgeTypeIdDescriptor(ForgeTypeId typeId)
    {
        _typeId = typeId;
        Name = typeId.TypeId;
    }
    
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ForgeTypeId.Clear) when parameters.Length == 0 => ResolveSet.Append(false, "Method execution disabled"),
            _ => null
        };
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("ToUnitLabel", _ => _typeId.ToUnitLabel());
        manager.Register("ToSpecLabel", _ => _typeId.ToSpecLabel());
        manager.Register("ToSymbolLabel", _ => _typeId.ToSymbolLabel());
#if REVIT2022_OR_GREATER
        manager.Register("ToGroupLabel", _ => _typeId.ToGroupLabel());
        manager.Register("ToDisciplineLabel", _ => _typeId.ToDisciplineLabel());
        manager.Register("ToParameterLabel", _ => _typeId.ToParameterLabel());
#endif
        manager.Register("IsUnit", _ => UnitUtils.IsUnit(_typeId));
        manager.Register("IsSymbol", _ => UnitUtils.IsSymbol(_typeId));
#if REVIT2022_OR_GREATER
        manager.Register("IsSpec", _ => SpecUtils.IsSpec(_typeId));
        manager.Register("IsMeasurableSpec", _ => UnitUtils.IsMeasurableSpec(_typeId));
        manager.Register("IsBuiltInParameter", _ => ParameterUtils.IsBuiltInParameter(_typeId));
        manager.Register("IsBuiltInGroup", _ => ParameterUtils.IsBuiltInGroup(_typeId));
#endif
    }
}