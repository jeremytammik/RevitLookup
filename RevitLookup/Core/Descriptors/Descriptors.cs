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
using RevitLookup.Core.Descriptors.Contracts;

namespace RevitLookup.Core.Descriptors;

public sealed class ColorDescriptor : Descriptor
{
    public ColorDescriptor(Color color)
    {
        Label = color.IsValid ? $"RGB: {color.Red} {color.Green} {color.Blue}" : "The color represents uninitialized/invalid value";
    }
}

public sealed class ElementDescriptor : Descriptor, IDescriptorCollector, IDescriptorResolver
{
    private readonly Element _value;

    public ElementDescriptor(Element value)
    {
        _value = value;
        Label = value.Name == string.Empty ? $"ID{value.Id}" : $"{value.Name}, ID{value.Id}";
        
    }

    public void RegisterResolvers(IResolverManager manager)
    {
        manager.Register("BoundingBox", _value.get_BoundingBox(null));
    }
}

public sealed class APIObjectDescriptor : Descriptor, IDescriptorCollector
{
}

public sealed class IEnumerableDescriptor : Descriptor, IDescriptorEnumerator
{
    private readonly IEnumerable _value;

    public IEnumerableDescriptor(IEnumerable value)
    {
        _value = value;
    }

    public IEnumerable Enumerate()
    {
        return _value;
    }
}

public sealed class BoolDescriptor : Descriptor
{
    public BoolDescriptor(bool value)
    {
        Label = value ? "True" : "False";
    }
}

public sealed class ExceptionDescriptor : Descriptor
{
    public ExceptionDescriptor(Exception value)
    {
        if (value.InnerException is null)
            Label = value.Message;
        else
            Label = string.IsNullOrEmpty(value.InnerException.Message) ? value.Message : value.InnerException.Message;
    }
}

public sealed class ObjectDescriptor : Descriptor
{
    public ObjectDescriptor()
    {
    }

    public ObjectDescriptor(object value)
    {
        if (value is not null) Label = value.ToString();
    }
}