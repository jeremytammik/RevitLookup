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
        manager.Register(_typeId, extension =>
        {
            extension.Name = "ToUnitLabel";
            extension.Result = extension.Value.ToUnitLabel();
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "ToSpecLabel";
            extension.Result = extension.Value.ToSpecLabel();
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "ToSymbolLabel";
            extension.Result = extension.Value.ToSymbolLabel();
        });
#if REVIT2022_OR_GREATER
        manager.Register(_typeId, extension =>
        {
            extension.Name = "ToGroupLabel";
            extension.Result = extension.Value.ToGroupLabel();
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "ToDisciplineLabel";
            extension.Result = extension.Value.ToDisciplineLabel();
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "ToParameterLabel";
            extension.Result = extension.Value.ToParameterLabel();
        });
#endif
        manager.Register(_typeId, extension =>
        {
            extension.Name = "IsUnit";
            extension.Result = UnitUtils.IsUnit(extension.Value);
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "IsSymbol";
            extension.Result = UnitUtils.IsSymbol(extension.Value);
        });
#if REVIT2022_OR_GREATER
        manager.Register(_typeId, extension =>
        {
            extension.Name = "IsSpec";
            extension.Result = SpecUtils.IsSpec(extension.Value);
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "IsMeasurableSpec";
            extension.Result = UnitUtils.IsMeasurableSpec(extension.Value);
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "IsBuiltInParameter";
            extension.Result = ParameterUtils.IsBuiltInParameter(extension.Value);
        });
        manager.Register(_typeId, extension =>
        {
            extension.Name = "IsBuiltInGroup";
            extension.Result = ParameterUtils.IsBuiltInGroup(extension.Value);
        });
#endif
    }
}