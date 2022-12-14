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

using RevitLookup.Core.Descriptors;
using RevitLookup.Core.Descriptors.Interfaces;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.UI.Tests.Moq;

public sealed class MoqSnoopableObject : ISnoopableObject, IComparable<MoqSnoopableObject>, IComparable
{
    private IReadOnlyList<SnoopableObject> _members;

    public MoqSnoopableObject(string value)
    {
        Descriptor = new StringDescriptor(value);
        Descriptor.Type = value.GetType().Name;
    }

    public MoqSnoopableObject(int value)
    {
        Descriptor = new IntDescriptor(value);
        Descriptor.Type = value.GetType().Name;
    }
    
    public MoqSnoopableObject(bool value)
    {
        Descriptor = new BoolDescriptor(value);
        Descriptor.Type = value.GetType().Name;
    }

    public IDescriptor Descriptor { get; }

    public IReadOnlyList<ISnoopableObject> GetMembers()
    {
        return _members = Descriptor.SnoopHandler?.Invoke();
    }

    public IReadOnlyList<ISnoopableObject> GetCachedMembers()
    {
        return _members ?? GetMembers();
    }

    public int CompareTo(MoqSnoopableObject other)
    {
        return string.CompareOrdinal(Descriptor.Label, other.Descriptor.Label);
    }

    public int CompareTo(object obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is MoqSnoopableObject other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(MoqSnoopableObject)}");
    }
}