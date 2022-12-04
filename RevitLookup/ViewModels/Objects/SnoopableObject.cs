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
using RevitLookup.Core.Descriptors.Interfaces;
using RevitLookup.Core.Descriptors.Utils;
using RevitLookup.ViewModels.Contracts;

namespace RevitLookup.ViewModels.Objects;

public sealed class SnoopableObject : ISnoopableObject
{
    private IReadOnlyList<SnoopableObject> _members;

    public SnoopableObject(Document context, object obj)
    {
        Descriptor = DescriptorUtils.FindSuitableDescriptor(obj);
        Descriptor.SnoopHandler = DescriptorUtils.FindSuitableHandler(obj);
        Descriptor.Type = obj.GetType().Name;
        ValidateLabel();
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

    private void ValidateLabel()
    {
        Descriptor.Label ??= Descriptor.Type;
    }
}