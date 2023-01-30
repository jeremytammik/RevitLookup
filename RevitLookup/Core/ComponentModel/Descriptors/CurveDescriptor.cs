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

using System.Globalization;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class CurveDescriptor : Descriptor, IDescriptorResolver
{
    private readonly Curve _curve;

    public CurveDescriptor(Curve curve)
    {
        _curve = curve;
        if (curve.IsBound || curve.IsCyclic) Name = $"{curve.Length.ToString(CultureInfo.InvariantCulture)} ft";
    }

    public ResolveSet Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Curve.GetEndPoint) => ResolveSet
                .Append(_curve.GetEndPoint(0), "Point 0")
                .AppendVariant(_curve.GetEndPoint(1), "Point 1"),
            nameof(Curve.GetEndParameter) => ResolveSet
                .Append(_curve.GetEndParameter(0), "Parameter 0")
                .AppendVariant(_curve.GetEndParameter(1), "Parameter 1"),
            nameof(Curve.GetEndPointReference) => ResolveSet
                .Append(_curve.GetEndPointReference(0), "Reference 0")
                .AppendVariant(_curve.GetEndPointReference(1), "Reference 1"),
            _ => null
        };
    }
}