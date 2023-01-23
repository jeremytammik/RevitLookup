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

public class PlanViewRangeDescriptor : Descriptor, IDescriptorResolver
{
    private readonly PlanViewRange _value;

    public PlanViewRangeDescriptor(PlanViewRange value)
    {
        _value = value;
    }

    public ResolveSummary Resolve(string name, ParameterInfo[] parameters)
    {
        return name switch
        {
            nameof(PlanViewRange.GetOffset) => ResolveSummary
                .Append("Top clip plane", _value.GetOffset(PlanViewPlane.TopClipPlane))
                .AppendVariant("Cut plane", _value.GetOffset(PlanViewPlane.CutPlane))
                .AppendVariant("Bottom clip plane", _value.GetOffset(PlanViewPlane.BottomClipPlane))
                .AppendVariant("Underlay bottom", _value.GetOffset(PlanViewPlane.UnderlayBottom)),
            nameof(PlanViewRange.GetLevelId) => ResolveSummary
                .Append("Top clip plane", _value.GetLevelId(PlanViewPlane.TopClipPlane))
                .AppendVariant("Cut plane", _value.GetLevelId(PlanViewPlane.CutPlane))
                .AppendVariant("Bottom clip plane", _value.GetLevelId(PlanViewPlane.BottomClipPlane))
                .AppendVariant("Underlay bottom", _value.GetLevelId(PlanViewPlane.UnderlayBottom)),
            _ => null
        };
    }
}