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

using Autodesk.Revit.DB;

namespace RevitLookup.Core.Descriptors.Utils;

public static class DescriptorUtils
{
    public static Descriptor FindSuitableDescriptor(object obj)
    {
        Descriptor descriptor = obj switch
        {
            bool value => new BoolDescriptor(value),
            string value => new StringDescriptor(value),
            int value => new IntDescriptor(value),
            Element value => new ElementDescriptor(value),
            Exception value => new ExceptionDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };
        
        descriptor.Type ??= obj.GetType().Name;
        descriptor.Label ??= descriptor.Type;
        return descriptor;
    }
}