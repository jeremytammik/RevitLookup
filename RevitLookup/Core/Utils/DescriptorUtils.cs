// Copyright 2003-2022 by Autodesk, Inc.
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

using RevitLookup.Core.ComponentModel;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.Utils;

public static class DescriptorUtils
{
    [NotNull]
    public static Descriptor FindSuitableDescriptor(SnoopableObject snoopableObject)
    {
        var descriptor = DescriptorMap.FindDescriptor(snoopableObject.Object, null);
        while (descriptor is IDescriptorRedirection redirection)
        {
            if (!redirection.TryRedirect(snoopableObject.Context, out var value)) break;
            
            descriptor = DescriptorMap.FindDescriptor(snoopableObject.Object, null);
            snoopableObject.Object = value;
        }

        if (snoopableObject.Object is null)
        {
            descriptor.Type = nameof(Object);
        }
        else
        {
            var type = snoopableObject.Object.GetType();
            ValidateProperties(descriptor, type);
        }

        return descriptor;
    }

    [CanBeNull]
    public static Descriptor FindSuitableDescriptor(SnoopableObject snoopableObject, Type type)
    {
        var descriptor = DescriptorMap.FindDescriptor(snoopableObject.Object, type);
        if (descriptor is null) return null;

        ValidateProperties(descriptor, type);
        return descriptor;
    }

    private static void ValidateProperties(Descriptor descriptor, Type type)
    {
        descriptor.Type = MakeGenericTypeName(type);
        descriptor.Name ??= descriptor.Type;
    }

    public static string MakeGenericTypeName(Type type)
    {
        if (!type.IsGenericType) return type.Name;

        var typeName = type.Name;
        var apostropheIndex = typeName.IndexOf('`');
        if (apostropheIndex > 0) typeName = typeName.Substring(0, apostropheIndex);
        typeName += "<";
        var genericArguments = type.GetGenericArguments();
        for (var i = 0; i < genericArguments.Length; i++)
        {
            typeName += MakeGenericTypeName(genericArguments[i]);
            if (i < genericArguments.Length - 1) typeName += ", ";
        }

        typeName += ">";
        return typeName;
    }
}