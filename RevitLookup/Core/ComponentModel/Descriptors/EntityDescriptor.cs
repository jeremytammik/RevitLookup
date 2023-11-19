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

public sealed class EntityDescriptor(Entity entity) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Entity.Get) when parameters.Length == 1 &&
                                    parameters[0].ParameterType == typeof(string) => ResolveGetByField(),
            nameof(Entity.Get) when parameters.Length == 2 &&
                                    parameters[0].ParameterType == typeof(string) &&
                                    parameters[1].ParameterType == typeof(ForgeTypeId) => ResolveGetByFieldForge(),
            _ => null
        };

        ResolveSet ResolveGetByField()
        {
            var resolveSummary = new ResolveSet();
            foreach (var field in entity.Schema.ListFields())
            {
                var method = entity.GetType().GetMethod(nameof(Entity.Get), new[] {typeof(Field)})!;
                var genericMethod = MakeGenericInvoker(field, method);
                resolveSummary.AppendVariant(genericMethod.Invoke(entity, new object[] {field}), field.FieldName);
            }

            return resolveSummary;
        }

        ResolveSet ResolveGetByFieldForge()
        {
            var resolveSummary = new ResolveSet();
            foreach (var field in entity.Schema.ListFields())
            {
                var forgeTypeId = field.GetSpecTypeId();
                var unit = UnitUtils.IsMeasurableSpec(forgeTypeId) ? UnitUtils.GetValidUnits(forgeTypeId).First() : UnitTypeId.Custom;
                var method = entity.GetType().GetMethod(nameof(Entity.Get), new[] {typeof(Field), typeof(ForgeTypeId)})!;
                var genericMethod = MakeGenericInvoker(field, method);
                resolveSummary.AppendVariant(genericMethod.Invoke(entity, new object[] {field, unit}), field.FieldName);
            }

            return resolveSummary;
        }
    }

    private static MethodInfo MakeGenericInvoker(Field field, MethodInfo invoker)
    {
        var containerType = field.ContainerType switch
        {
            ContainerType.Simple => field.ValueType,
            ContainerType.Array => typeof(IList<>).MakeGenericType(field.ValueType),
            ContainerType.Map => typeof(IDictionary<,>).MakeGenericType(field.KeyType, field.ValueType),
            _ => throw new ArgumentOutOfRangeException()
        };

        return invoker.MakeGenericMethod(containerType);
    }
}