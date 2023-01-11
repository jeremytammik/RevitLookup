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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Extensions;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class PlanViewRangeDescriptor : Descriptor, IDescriptorExtension
{
    private readonly PlanViewRange _value;

    public PlanViewRangeDescriptor(PlanViewRange value)
    {
        _value = value;
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetOffset(TopClipPlane)",
            Value = range => range.GetOffset(PlanViewPlane.TopClipPlane)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetOffset(CutPlane)",
            Value = range => range.GetOffset(PlanViewPlane.CutPlane)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetOffset(BottomClipPlane)",
            Value = range => range.GetOffset(PlanViewPlane.BottomClipPlane)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetOffset(UnderlayBottom)",
            Value = range => range.GetOffset(PlanViewPlane.UnderlayBottom)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetLevelId(TopClipPlane)",
            Value = range => range.GetLevelId(PlanViewPlane.TopClipPlane)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetLevelId(CutPlane)",
            Value = range => range.GetLevelId(PlanViewPlane.CutPlane)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetLevelId(BottomClipPlane)",
            Value = range => range.GetLevelId(PlanViewPlane.BottomClipPlane)
        });
        manager.Register(new DescriptorExtension<PlanViewRange>(_value)
        {
            Name = "GetLevelId(UnderlayBottom)",
            Value = range => range.GetLevelId(PlanViewPlane.UnderlayBottom)
        });
    }
}