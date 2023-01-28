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
using Autodesk.Revit.DB.ExtensibleStorage;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class EntityDescriptor : Descriptor, IDescriptorResolver
{
    private readonly Entity _entity;

    public EntityDescriptor(Entity entity)
    {
        _entity = entity;
    }

    public ResolveSet Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Entity.Get) when parameters.Length == 1 &&
                                    parameters[0].ParameterType.Name == nameof(String) => ResolveGetByField(),
            nameof(Entity.Get) when parameters.Length == 2 &&
                                    parameters[0].ParameterType.Name == nameof(String) &&
                                    parameters[1].ParameterType.Name == nameof(ForgeTypeId) => ResolveGetByFieldForge(),
            _ => null
        };

        ResolveSet ResolveGetByField()
        {
            var resolveSummary = new ResolveSet();
            foreach (var field in _entity.Schema.ListFields())
            {
                var forgeTypeId = field.GetSpecTypeId();
                if (!string.IsNullOrEmpty(forgeTypeId.TypeId)) continue;

                var method = _entity.GetType().GetMethod(nameof(Entity.Get), new[] {typeof(Field)})!;
                var genericMethod = method.MakeGenericMethod(field.ValueType);
                resolveSummary.AppendVariant(genericMethod.Invoke(_entity, new object[] {field}), field.FieldName);
            }

            return resolveSummary;
        }

        ResolveSet ResolveGetByFieldForge()
        {
            var resolveSummary = new ResolveSet();
            foreach (var field in _entity.Schema.ListFields())
            {
                var forgeTypeId = field.GetSpecTypeId();
                if (string.IsNullOrEmpty(forgeTypeId.TypeId)) continue;

                var method = _entity.GetType().GetMethod(nameof(Entity.Get), new[] {typeof(Field), typeof(ForgeTypeId)})!;
                var genericMethod = method.MakeGenericMethod(field.ValueType);
                resolveSummary.AppendVariant(genericMethod.Invoke(_entity,
                    new object[] {field, UnitUtils.GetValidUnits(forgeTypeId).First()}), field.FieldName);
            }

            return resolveSummary;
        }
    }
}