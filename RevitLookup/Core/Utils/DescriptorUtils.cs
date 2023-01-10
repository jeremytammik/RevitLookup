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

using System.Text;
using RevitLookup.Core.ComponentModel;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.Utils;

public static class DescriptorUtils
{
    public static Descriptor FindSuitableDescriptor([CanBeNull] object obj)
    {
        var descriptor = DescriptorMap.FindDescriptor(obj);
        if (obj is null)
        {
            descriptor.Type = nameof(Object);
        }
        else
        {
            var type = obj.GetType();
            ValidateProperties(descriptor, type);
        }

        return descriptor;
    }

    public static Descriptor FindSuitableDescriptor([NotNull] object obj, Type type)
    {
        var descriptor = DescriptorMap.FindDescriptor(obj);
        ValidateProperties(descriptor, type);
        return descriptor;
    }

    private static void ValidateProperties(Descriptor descriptor, Type type)
    {
        descriptor.Type = MakeGenericTypeName(type);
        descriptor.Label ??= descriptor.Type;
    }

    //Benchmark tested version. Optimized for performance
    private static string MakeGenericTypeName(Type type)
    {
        if (!type.IsGenericType) return type.Name;

        var builder = new StringBuilder();
        var types = new Stack<Type>();
        var genericCount = 0;
        types.Push(type);
        while (types.Count != 0)
        {
            var currentType = types.Pop();
            if (!currentType.IsGenericType)
            {
                builder.Append(currentType.Name);
                if (genericCount > 0)
                {
                    builder.Append(new string('>', genericCount));
                    genericCount = 0;
                }

                continue;
            }

            genericCount++;
            if (currentType.IsGenericTypeDefinition)
            {
                builder.Append(currentType.Name.Substring(0, currentType.Name.IndexOf('`')));
                builder.Append("<");
                var args = currentType.GetGenericArguments();
                for (var i = 0; i < args.Length; i++)
                {
                    types.Push(args[i]);
                    if (i < args.Length - 1)
                    {
                        builder.Append(", ");
                    }
                }
            }
            else
            {
                builder.Append(currentType.Name);
                if (genericCount == 0) continue;
                builder.Append(new string('>', genericCount));
                genericCount = 0;
            }
        }

        return builder.ToString();
    }
}