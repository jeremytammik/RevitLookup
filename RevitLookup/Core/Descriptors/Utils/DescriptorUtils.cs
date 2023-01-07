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

using System.Collections;
using Autodesk.Revit.DB;

namespace RevitLookup.Core.Descriptors.Utils;

public static class DescriptorUtils
{
    public static Descriptor FindSuitableDescriptor(object obj)
    {
        return FindSuitableDescriptor(obj, null);
    }

    public static Descriptor FindSuitableDescriptor(object obj, [CanBeNull] string type)
    {
        Descriptor descriptor = obj switch
        {
            bool value when type is null or nameof(Boolean) => new BoolDescriptor(value),
            Element value when type is null or nameof(Element) => new ElementDescriptor(value),
            Color value when type is null or nameof(Color) => new ColorDescriptor(value),
            CategoryNameMap {Size: > 0} value => new IEnumerableDescriptor(value),
            ICollection {Count: > 0} value => new IEnumerableDescriptor(value),
            CategoryNameMap => new ObjectDescriptor(),
            ICollection => new ObjectDescriptor(),
            IEnumerable value and APIObject => new IEnumerableDescriptor(value),
            APIObject => new APIObjectDescriptor(),
            Exception value => new ExceptionDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };

        descriptor.Type = obj is null ? nameof(Object) : obj.GetType().Name;
        descriptor.Label ??= descriptor.Type;
        return descriptor;
    }
}