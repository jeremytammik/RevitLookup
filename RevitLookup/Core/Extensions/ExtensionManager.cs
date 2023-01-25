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

using Autodesk.Revit.DB;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.Extensions;

public sealed class ExtensionManager : IExtensionManager
{
    private readonly Document _context;
    private readonly Descriptor _descriptor;

    public ExtensionManager(Document context, Descriptor descriptor)
    {
        _context = context;
        _descriptor = descriptor;
        Descriptors = new List<Descriptor>();
    }

    public List<Descriptor> Descriptors { get; set; }

    public void Register<T>(string name, T value, Action<DescriptorExtension<T>> extension)
    {
        var descriptorExtension = new DescriptorExtension<T>
        {
            Value = value,
            Context = _context
        };

        var descriptor = new ObjectDescriptor
        {
            Label = name,
            Type = _descriptor.Type
        };

        try
        {
            extension.Invoke(descriptorExtension);
            descriptor.Value = new SnoopableObject(_context, descriptorExtension.Result);
        }
        catch (Exception exception)
        {
            descriptor.Value = new SnoopableObject(_context, exception);
        }

        Descriptors.Add(descriptor);
    }

    public void Register(string name, Func<object> result)
    {
        var descriptor = new ObjectDescriptor
        {
            Label = name,
            Type = _descriptor.Type
        };

        try
        {
            descriptor.Value = new SnoopableObject(_context, result());
        }
        catch (Exception exception)
        {
            descriptor.Value = new SnoopableObject(_context, exception);
        }

        Descriptors.Add(descriptor);
    }
}