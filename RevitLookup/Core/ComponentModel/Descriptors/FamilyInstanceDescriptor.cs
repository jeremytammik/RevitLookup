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

using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class FamilyInstanceDescriptor : ElementDescriptor, IDescriptorResolver
{
    private readonly FamilyInstance _familyInstance;

    public FamilyInstanceDescriptor(FamilyInstance familyInstance) : base(familyInstance)
    {
        _familyInstance = familyInstance;
    }

    public new ResolveSet Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilyInstance.GetReferences) => ResolveSet
                .Append(_familyInstance.GetReferences(FamilyInstanceReferenceType.Back), "Back")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Bottom), "Bottom")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.StrongReference), "Strong reference")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.WeakReference), "Weak reference")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Front), "Front")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Left), "Left")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Right), "Right")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.Top), "Top")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.CenterElevation), "Center elevation")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.CenterFrontBack), "Center front back")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.CenterLeftRight), "Center left right")
                .AppendVariant(_familyInstance.GetReferences(FamilyInstanceReferenceType.NotAReference), "Not a reference"),
            _ => null
        };
    }
}