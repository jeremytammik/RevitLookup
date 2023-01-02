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

using System.Reflection;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.Core.Descriptors;

public abstract class Descriptor : IComparable<Descriptor>, IComparable
{
    public string Type { get; set; }
    public string Label { get; set; }
    public SnoopableObject Child { get; set; }

    public virtual bool TryInvoke(out IReadOnlyList<Descriptor> members)
    {
        members = null;
        return false;
    }
    
    public virtual bool TryInvoke(string methodName, ParameterInfo[] args, out object result)
    {
        result = null;
        return false;
    }

    public int CompareTo(Descriptor other)
    {
        return string.CompareOrdinal(Label, other.Label);
    }

    public int CompareTo(object obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is Descriptor other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Descriptor)}");
    }
}