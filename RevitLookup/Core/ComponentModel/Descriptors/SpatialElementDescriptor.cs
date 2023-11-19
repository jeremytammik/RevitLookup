// Copyright 2003-$File.CreatedYear by Autodesk, Inc.
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

public class SpatialElementDescriptor : ElementDescriptor, IDescriptorResolver
{
    private readonly SpatialElement _spatialElement;
    public SpatialElementDescriptor(SpatialElement spatialElement) : base(spatialElement)
    {
        _spatialElement = spatialElement;
    }

    public new ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(SpatialElement.GetBoundarySegments) => new ResolveSet(8)
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center,
                    StoreFreeBoundaryFaces = true
                }), $"Center, store free boundary faces")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.CoreBoundary,
                    StoreFreeBoundaryFaces = true
                }), "Core boundary, store free boundary faces")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish,
                    StoreFreeBoundaryFaces = true
                }), "Finish, store free boundary faces")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.CoreCenter,
                    StoreFreeBoundaryFaces = true
                }), "Core center, store free boundary faces")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center,
                    StoreFreeBoundaryFaces = true
                }), "Center")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.CoreBoundary,
                    StoreFreeBoundaryFaces = true
                }), "Core boundary")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish,
                    StoreFreeBoundaryFaces = true
                }), "Finish")
                .AppendVariant(_spatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions()
                {
                    SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.CoreCenter,
                    StoreFreeBoundaryFaces = true
                }), "Core center"),
            _ => null
        };
    }
}