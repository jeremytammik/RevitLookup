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

namespace RevitLookup.Core.Visualization.Helpers;

public static class RenderGeometryHelper
{
    public static List<List<XYZ>> GetSegmentationTube(IList<XYZ> vertices, double diameter)
    {
        var points = new List<List<XYZ>>();
        
        for (var i = 0; i < vertices.Count; i++)
        {
            var center = vertices[i];
            XYZ normal;
            if (i == 0)
            {
                normal = (vertices[i + 1] - center).Normalize();
            }
            else if (i == vertices.Count - 1)
            {
                normal = (center - vertices[i - 1]).Normalize();
            }
            else
            {
                normal = ((vertices[i + 1] - vertices[i - 1]) / 2.0).Normalize();
            }
            
            points.Add(TessellateCircle(center, normal, diameter / 2));
        }
        
        return points;
    }
    
    public static XYZ GetMeshVertexNormal(Mesh mesh, int index, DistributionOfNormals normalDistribution)
    {
        switch (normalDistribution)
        {
            case DistributionOfNormals.AtEachPoint:
                return mesh.GetNormal(index);
            case DistributionOfNormals.OnEachFacet:
                var vertex = mesh.Vertices[index];
                for (var i = 0; i < mesh.NumTriangles; i++)
                {
                    var triangle = mesh.get_Triangle(i);
                    var triangleVertex = triangle.get_Vertex(0);
                    if (triangleVertex.IsAlmostEqualTo(vertex)) return mesh.GetNormal(i);
                    triangleVertex = triangle.get_Vertex(1);
                    if (triangleVertex.IsAlmostEqualTo(vertex)) return mesh.GetNormal(i);
                    triangleVertex = triangle.get_Vertex(2);
                    if (triangleVertex.IsAlmostEqualTo(vertex)) return mesh.GetNormal(i);
                }
                
                return XYZ.Zero;
            case DistributionOfNormals.OnePerFace:
                return mesh.GetNormal(0);
            default:
                throw new ArgumentOutOfRangeException(nameof(normalDistribution), normalDistribution, null);
        }
    }
    
    public static List<XYZ> TessellateCircle(XYZ center, XYZ normal, double radius)
    {
        var vertices = new List<XYZ>();
        var segmentCount = InterpolateSegmentsCount(radius);
        var xDirection = normal.CrossProduct(XYZ.BasisZ).Normalize() * radius;
        if (xDirection.IsZeroLength())
        {
            xDirection = normal.CrossProduct(XYZ.BasisX).Normalize() * radius;
        }
        
        var yDirection = normal.CrossProduct(xDirection).Normalize() * radius;
        
        for (var i = 0; i < segmentCount; i++)
        {
            var angle = 2 * Math.PI * i / segmentCount;
            var vertex = center + xDirection * Math.Cos(angle) + yDirection * Math.Sin(angle);
            vertices.Add(vertex);
        }
        
        return vertices;
    }
    
    public static int InterpolateSegmentsCount(double diameter)
    {
        const int minSegments = 6;
        const int maxSegments = 33;
        const double minDiameter = 0.1 / 12d;
        const double maxDiameter = 3 / 12d;
        
        if (diameter <= minDiameter) return minSegments;
        if (diameter >= maxDiameter) return maxSegments;
        
        var normalDiameter = (diameter - minDiameter) / (maxDiameter - minDiameter);
        return (int) (minSegments + normalDiameter * (maxSegments - minSegments));
    }
    
    public static double InterpolateOffsetByDiameter(double diameter)
    {
        const double minOffset = 0.01d;
        const double maxOffset = 0.1d;
        const double minDiameter = 0.1 / 12d;
        const double maxDiameter = 3 / 12d;
        
        if (diameter <= minDiameter) return minOffset;
        if (diameter >= maxDiameter) return maxOffset;
        
        var normalOffset = (diameter - minDiameter) / (maxDiameter - minDiameter);
        return minOffset + normalOffset * (maxOffset - minOffset);
    }
    
    public static double InterpolateOffsetByArea(double area)
    {
        const double minOffset = 0.01d;
        const double maxOffset = 0.1d;
        const double minArea = 0.01d;
        const double maxArea = 1d;
        
        if (area <= minArea) return minOffset;
        if (area >= maxArea) return maxOffset;
        
        var normalOffset = (area - minArea) / (maxArea - minArea);
        return minOffset + normalOffset * (maxOffset - minOffset);
    }
    
    public static double InterpolateAxisLengthByArea(double area)
    {
        const double minLength = 0.1d;
        const double maxLength = 1d;
        const double minArea = 0.01d;
        const double maxArea = 1d;
        
        if (area <= minArea) return minLength;
        if (area >= maxArea) return maxLength;
        
        var normalOffset = (area - minArea) / (maxArea - minArea);
        return minLength + normalOffset * (maxLength - minLength);
    }
    
    public static double InterpolateAxisLengthByPoints(XYZ min, XYZ max)
    {
        const double maxLength = 1d;
        
        var width = max.X - min.X;
        var height = max.Y - min.Y;
        var depth = max.Z - min.Z;
        
        var maxSize = Math.Max(width, Math.Max(height, depth));
        
        if (maxLength * 2 < maxSize)
        {
            return maxLength;
        }
        
        return maxSize * 0.35;
    }
}