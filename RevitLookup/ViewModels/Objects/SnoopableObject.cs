// Copyright 2003-2022 by Autodesk, Inc.
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
using RevitLookup.Core.Descriptors;
using RevitLookup.Core.Descriptors.Contracts;
using RevitLookup.Core.Descriptors.Extensions;
using RevitLookup.Core.Descriptors.Utils;

namespace RevitLookup.ViewModels.Objects;

public sealed class SnoopableObject
{
    private readonly Document _context;
    private readonly object _obj;
    private readonly List<Descriptor> _members = new(0);

    public SnoopableObject(Document context, object obj)
    {
        _obj = obj;
        _context = context;
        Descriptor = DescriptorUtils.FindSuitableDescriptor(obj);
    }

    public Descriptor Descriptor { get; }

    public IReadOnlyList<Descriptor> GetMembers()
    {
        if (Descriptor is IDescriptorCollector)
        {
            ReflectionUtils.CollectProperties(Descriptor, _context, _members, _obj);
            ReflectionUtils.CollectMethods(Descriptor, _context, _members, _obj);
        }

        if (Descriptor is IDescriptorExtension extension)
        {
            extension.RegisterExtensions(new ExtensionManager(Descriptor, _context, _members));
        }

        return _members;
    }

    [CanBeNull]
    public IReadOnlyList<Descriptor> GetCachedMembers()
    {
        return _members.Count == 0 ? GetMembers() : _members;
    }
}