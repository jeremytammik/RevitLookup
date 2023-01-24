// Copyright 2003-2023 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ElementDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly Element _value;

    public ElementDescriptor(Element value)
    {
        _value = value;
        Label = value.Name == string.Empty ? $"ID{value.Id}" : $"{value.Name}, ID{value.Id}";
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        //TODO move to get Entity resolver
        // var schemas = Schema.ListSchemas();
        // foreach (var schema in schemas)
        // {
        //     if (!schema.ReadAccessGranted()) continue;
        //
        //     manager.Register(new DescriptorExtension<(Element _value, Schema schema)>((_value, schema))
        //     {
        //         Group = "Extensible Storage",
        //         Name = schema.SchemaName,
        //         Value = tuple => tuple._value.GetEntity(tuple.schema)
        //     });
        // }
    }

    public ResolveSummary Resolve(string name, ParameterInfo[] parameters)
    {
        return name switch
        {
            "BoundingBox" => ResolveSummary
                .Append(_value.get_BoundingBox(null), "Model")
                .AppendVariant(_value.get_BoundingBox(RevitApi.ActiveView), "Active view"),
            "Geometry" => ResolveSummary
                .Append(_value.get_Geometry(new Options
                {
                    View = RevitApi.ActiveView,
                    ComputeReferences = true
                }), "Active view")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    View = RevitApi.ActiveView,
                    IncludeNonVisibleObjects = true,
                    ComputeReferences = true
                }), "Active view, including non-visible objects")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                    ComputeReferences = true
                }), "Undefined view, coarse detail level")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                    ComputeReferences = true
                }), "Undefined view, fine detail level")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                    ComputeReferences = true
                }), "Undefined view, medium detail level")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                    ComputeReferences = true
                }), "Undefined view, undefined detail level")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Coarse,
                    IncludeNonVisibleObjects = true,
                    ComputeReferences = true
                }), "Undefined view, coarse detail level, including non-visible objects")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Fine,
                    IncludeNonVisibleObjects = true,
                    ComputeReferences = true
                }), "Undefined view, fine detail level, including non-visible objects")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Medium,
                    IncludeNonVisibleObjects = true,
                    ComputeReferences = true
                }), "Undefined view, medium detail level, including non-visible objects")
                .AppendVariant(_value.get_Geometry(new Options
                {
                    DetailLevel = ViewDetailLevel.Undefined,
                    IncludeNonVisibleObjects = true,
                    ComputeReferences = true
                }), "Undefined view, undefined detail level, including non-visible objects"),
            _ => null
        };
    }
}