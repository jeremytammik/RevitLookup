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
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Enums;
using RevitLookup.Core.Objects;
using RevitLookup.Core.Utils;

namespace RevitLookup.Core.Metadata;

public sealed partial class DescriptorBuilder
{
    private void WriteDescriptor(object value)
    {
        var descriptor = new ObjectDescriptor
        {
            Depth = _depth,
            Value = EvaluateValue(value),
            TypeFullName = DescriptorUtils.MakeGenericFullTypeName(_type),
            MemberAttributes = MemberAttributes.Property,
            Type = DescriptorUtils.MakeGenericTypeName(_type)
        };

        descriptor.Name = descriptor.Value.Descriptor.Type;
        _descriptors.Add(descriptor);
    }

    private void WriteDescriptor(string name, object value)
    {
        var descriptor = new ObjectDescriptor
        {
            Depth = _depth,
            Name = name,
            Value = EvaluateValue(value),
            TypeFullName = DescriptorUtils.MakeGenericFullTypeName(_type),
            MemberAttributes = MemberAttributes.Extension,
            Type = DescriptorUtils.MakeGenericTypeName(_type),
            ComputationTime = _tracker.Elapsed.TotalMilliseconds
        };

        _descriptors.Add(descriptor);
        _tracker.Reset();
    }

    private void WriteDescriptor(MemberInfo member, object value, ParameterInfo[] parameters)
    {
        var descriptor = new ObjectDescriptor
        {
            Depth = _depth,
            TypeFullName = DescriptorUtils.MakeGenericFullTypeName(_type),
            Value = EvaluateValue(member, value),
            Name = EvaluateName(member, parameters),
            MemberAttributes = EvaluateAttributes(member),
            Type = DescriptorUtils.MakeGenericTypeName(_type),
            ComputationTime = _tracker.Elapsed.TotalMilliseconds
        };

        _descriptors.Add(descriptor);
        _tracker.Reset();
    }

    private SnoopableObject EvaluateValue(MemberInfo member, object value)
    {
        var snoopableObject = new SnoopableObject(value, Context);
        SnoopUtils.Redirect(member.Name, snoopableObject);
        RestoreSetDescription(member, value, snoopableObject);
        return snoopableObject;
    }

    private SnoopableObject EvaluateValue(object value)
    {
        var snoopableObject = new SnoopableObject(value, Context);
        SnoopUtils.Redirect(snoopableObject);
        RestoreSetDescription(value, snoopableObject);
        return snoopableObject;
    }

    private static string EvaluateName(MemberInfo member, [CanBeNull] ParameterInfo[] types)
    {
        if (types is null) return member.Name;
        if (types.Length == 0) return member.Name;

        var parameterNames = types.Select(info => DescriptorUtils.MakeGenericTypeName(info.ParameterType));
        var parameters = string.Join(", ", parameterNames);
        return $"{member.Name} ({parameters})";
    }

    private static MemberAttributes EvaluateAttributes(MemberInfo member)
    {
        return member switch
        {
            MethodInfo info => GetModifiers(MemberAttributes.Method, info.Attributes),
            PropertyInfo info => GetModifiers(MemberAttributes.Property, info.CanRead ? info.GetMethod.Attributes : info.SetMethod.Attributes),
            FieldInfo info => GetModifiers(MemberAttributes.Field, info.Attributes),
            EventInfo info => GetModifiers(MemberAttributes.Event, info.AddMethod.Attributes),
            _ => throw new ArgumentOutOfRangeException(nameof(member))
        };
    }

    private static MemberAttributes GetModifiers(MemberAttributes attributes, MethodAttributes methodAttributes)
    {
        if ((methodAttributes & MethodAttributes.Static) != 0) attributes |= MemberAttributes.Static;
        if ((methodAttributes & MethodAttributes.Private) != 0) attributes |= MemberAttributes.Private;
        return attributes;
    }

    private static MemberAttributes GetModifiers(MemberAttributes attributes, FieldAttributes fieldAttributes)
    {
        if ((fieldAttributes & FieldAttributes.Static) != 0) attributes |= MemberAttributes.Static;
        if ((fieldAttributes & FieldAttributes.Private) != 0) attributes |= MemberAttributes.Private;
        return attributes;
    }

    private static void RestoreSetDescription(object value, SnoopableObject snoopableObject)
    {
        if (value is not ResolveSet set) return;
        if (set.Variants.Count == 1) return;

        var type = set.Variants.Peek().Result.GetType();
        var name = DescriptorUtils.MakeGenericTypeName(type);
        snoopableObject.Descriptor.Name = name;
        snoopableObject.Descriptor.Type = name;
    }

    private static void RestoreSetDescription(MemberInfo member, object value, SnoopableObject snoopableObject)
    {
        if (value is not ResolveSet set) return;
        if (set.Variants.Count == 1) return;

        var name = member switch
        {
            PropertyInfo property => DescriptorUtils.MakeGenericTypeName(property.GetMethod.ReturnType),
            MethodInfo method => DescriptorUtils.MakeGenericTypeName(method.ReturnType),
            _ => snoopableObject.Descriptor.Name
        };

        snoopableObject.Descriptor.Name = name;
        snoopableObject.Descriptor.Type = name;
    }
}