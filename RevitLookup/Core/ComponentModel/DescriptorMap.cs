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

using System.Collections;
using Autodesk.Revit.DB;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel;

public static class DescriptorMap
{
    /// <summary>
    ///     Finding the first match of a descriptor type in the inheritance hierarchy
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Descriptor FindDescriptor(object obj)
    {
        return obj switch
        {
            bool value => new BoolDescriptor(value),

            Element value => new ElementDescriptor(value),
            Parameter value => new ParameterDescriptor(value),
            Color value => new ColorDescriptor(value),
            Category value => new CategoryDescriptor(value),
            Document value => new DocumentDescriptor(value),
            Autodesk.Revit.ApplicationServices.Application value => new ApplicationDescriptor(value),
            PrintManager value => new PrintManagerDescriptor(value),

            CategoryNameMap value => new IEnumerableDescriptor(value),
            ICollection value => new IEnumerableDescriptor(value),
            IEnumerable value and APIObject => new IEnumerableDescriptor(value),

            APIObject => new APIObjectDescriptor(),
            Exception value => new ExceptionDescriptor(value),

            null => new ObjectDescriptor(),
            _ => new ObjectDescriptor(obj)
        };
    }
}