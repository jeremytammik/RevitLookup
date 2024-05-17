// Copyright 2003-2024 by Autodesk, Inc.
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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class BoundingBoxXyzDescriptor(BoundingBoxXYZ box) : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            "Bounds" => ResolveBounds,
            "MinEnabled" => ResolveMinEnabled,
            "MaxEnabled" => ResolveMaxEnabled,
            "BoundEnabled" => ResolveBoundEnabled,
            _ => null
        };
        
        IVariants ResolveBounds()
        {
            return new Variants<XYZ>(2)
                .Add(box.get_Bounds(0), "Bound 0")
                .Add(box.get_Bounds(1), "Bound 1");
        }
        
        IVariants ResolveMinEnabled()
        {
            var minEnabled0 = box.get_MinEnabled(0);
            var minEnabled1 = box.get_MinEnabled(1);
            var minEnabled2 = box.get_MinEnabled(2);
            
            return new Variants<bool>(3)
                .Add(minEnabled0, $"Dimension 0: {minEnabled0}")
                .Add(minEnabled1, $"Dimension 1: {minEnabled1}")
                .Add(minEnabled2, $"Dimension 2: {minEnabled2}");
        }
        
        IVariants ResolveMaxEnabled()
        {
            var maxEnabled0 = box.get_MaxEnabled(0);
            var maxEnabled1 = box.get_MaxEnabled(1);
            var maxEnabled2 = box.get_MaxEnabled(2);
            
            return new Variants<bool>(3)
                .Add(maxEnabled0, $"Dimension 0: {maxEnabled0}")
                .Add(maxEnabled1, $"Dimension 1: {maxEnabled1}")
                .Add(maxEnabled2, $"Dimension 2: {maxEnabled2}");
        }
        
        IVariants ResolveBoundEnabled()
        {
            var boundEnabled00 = box.get_BoundEnabled(0, 0);
            var boundEnabled01 = box.get_BoundEnabled(0, 1);
            var boundEnabled02 = box.get_BoundEnabled(0, 2);
            var boundEnabled10 = box.get_BoundEnabled(1, 0);
            var boundEnabled11 = box.get_BoundEnabled(1, 1);
            var boundEnabled12 = box.get_BoundEnabled(1, 2);
            
            return new Variants<bool>(6)
                .Add(boundEnabled00, $"Bound 0, dimension 0: {boundEnabled00}")
                .Add(boundEnabled01, $"Bound 0, dimension 1: {boundEnabled01}")
                .Add(boundEnabled02, $"Bound 0, dimension 2: {boundEnabled02}")
                .Add(boundEnabled10, $"Bound 1, dimension 0: {boundEnabled10}")
                .Add(boundEnabled11, $"Bound 1, dimension 1: {boundEnabled11}")
                .Add(boundEnabled12, $"Bound 1, dimension 2: {boundEnabled12}");
        }
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("Centroid", _ => (box.Min + box.Max) / 2);
        manager.Register("Vertices", _ => new Variants<XYZ>(8)
            .Add(new XYZ(box.Min.X, box.Min.Y, box.Min.Z))
            .Add(new XYZ(box.Min.X, box.Min.Y, box.Max.Z))
            .Add(new XYZ(box.Min.X, box.Max.Y, box.Min.Z))
            .Add(new XYZ(box.Min.X, box.Max.Y, box.Max.Z))
            .Add(new XYZ(box.Max.X, box.Min.Y, box.Min.Z))
            .Add(new XYZ(box.Max.X, box.Min.Y, box.Max.Z))
            .Add(new XYZ(box.Max.X, box.Max.Y, box.Min.Z))
            .Add(new XYZ(box.Max.X, box.Max.Y, box.Max.Z)));
        manager.Register("Volume", _ =>
        {
            var length = box.Max.X - box.Min.X;
            var width = box.Max.Y - box.Min.Y;
            var height = box.Max.Z - box.Min.Z;
            
            return length * width * height;
        });
        manager.Register("SurfaceArea", _ =>
        {
            var length = box.Max.X - box.Min.X;
            var width = box.Max.Y - box.Min.Y;
            var height = box.Max.Z - box.Min.Z;
            
            var area1 = length * width;
            var area2 = length * height;
            var area3 = width * height;
            
            return 2 * (area1 + area2 + area3);
        });
    }
}