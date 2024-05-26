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

using Autodesk.Revit.DB.DirectContext3D;
using RevitLookup.Models.Render;
using RevitLookup.Utils;

namespace RevitLookup.Core.Visualization;

public static class Render3dUtils
{
    public static void MapSurfaceBuffer(RenderingBufferStorage buffer, Mesh mesh, double thickness)
    {
        var vertexCount = mesh.Vertices.Count;
        var triangleCount = mesh.NumTriangles;
        
        buffer.VertexBufferCount = 2 * vertexCount;
        buffer.PrimitiveCount = 2 * triangleCount + 2 * vertexCount;
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * buffer.VertexBufferCount;
        buffer.FormatBits = VertexFormatBits.Position;
        buffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        buffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = buffer.VertexBuffer.GetVertexStreamPosition();
        var normals = new List<XYZ>(mesh.NumberOfNormals);
        
        for (var i = 0; i < mesh.Vertices.Count; i++)
        {
            var normal = GeometryUtils.GetMeshVertexNormal(mesh, i, mesh.DistributionOfNormals);
            normals.Add(normal);
        }
        
        foreach (var vertex in mesh.Vertices)
        {
            var vertexPosition = new VertexPosition(vertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        for (var i = 0; i < mesh.Vertices.Count; i++)
        {
            var vertex = mesh.Vertices[i];
            var normal = normals[i];
            var offsetVertex = vertex + normal * thickness;
            var vertexPosition = new VertexPosition(offsetVertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        buffer.VertexBuffer.Unmap();
        buffer.IndexBufferCount = 6 * triangleCount + 12 * vertexCount;
        buffer.IndexBuffer = new IndexBuffer(buffer.IndexBufferCount);
        buffer.IndexBuffer.Map(buffer.IndexBufferCount);
        
        var indexStream = buffer.IndexBuffer.GetIndexStreamTriangle();
        
        for (var i = 0; i < triangleCount; i++)
        {
            var meshTriangle = mesh.get_Triangle(i);
            var index0 = (int) meshTriangle.get_Index(0);
            var index1 = (int) meshTriangle.get_Index(1);
            var index2 = (int) meshTriangle.get_Index(2);
            indexStream.AddTriangle(new IndexTriangle(index0, index1, index2));
        }
        
        for (var i = 0; i < triangleCount; i++)
        {
            var meshTriangle = mesh.get_Triangle(i);
            var index0 = (int) meshTriangle.get_Index(0) + vertexCount;
            var index1 = (int) meshTriangle.get_Index(1) + vertexCount;
            var index2 = (int) meshTriangle.get_Index(2) + vertexCount;
            indexStream.AddTriangle(new IndexTriangle(index0, index1, index2));
        }
        
        for (var i = 0; i < vertexCount; i++)
        {
            var next = (i + 1) % vertexCount;
            indexStream.AddTriangle(new IndexTriangle(i, next, i + vertexCount));
            indexStream.AddTriangle(new IndexTriangle(next, next + vertexCount, i + vertexCount));
        }
        
        buffer.IndexBuffer.Unmap();
        buffer.VertexFormat = new VertexFormat(buffer.FormatBits);
    }
    
    public static void MapCurveBuffer(RenderingBufferStorage buffer, IList<XYZ> vertices, double diameter)
    {
        var tubeSegments = GetSegmentationTube(vertices, diameter);
        var segmentVerticesCount = tubeSegments[0].Count;
        var newVertexCount = vertices.Count * segmentVerticesCount;
        
        buffer.VertexBufferCount = newVertexCount;
        buffer.PrimitiveCount = (vertices.Count - 1) * segmentVerticesCount * 4;
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * buffer.VertexBufferCount;
        buffer.FormatBits = VertexFormatBits.Position;
        buffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        buffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = buffer.VertexBuffer.GetVertexStreamPosition();
        
        foreach (var segment in tubeSegments)
        {
            foreach (var point in segment)
            {
                var vertexPosition = new VertexPosition(point);
                vertexStream.AddVertex(vertexPosition);
            }
        }
        
        buffer.VertexBuffer.Unmap();
        
        buffer.IndexBufferCount = (vertices.Count - 1) * segmentVerticesCount * 4 * IndexLine.GetSizeInShortInts();
        buffer.IndexBuffer = new IndexBuffer(buffer.IndexBufferCount);
        buffer.IndexBuffer.Map(buffer.IndexBufferCount);
        
        var indexStream = buffer.IndexBuffer.GetIndexStreamLine();
        
        for (var i = 0; i < vertices.Count - 1; i++)
        {
            for (var j = 0; j < segmentVerticesCount; j++)
            {
                var currentStart = i * segmentVerticesCount + j;
                var nextStart = (i + 1) * segmentVerticesCount + j;
                var currentEnd = i * segmentVerticesCount + (j + 1) % segmentVerticesCount;
                var nextEnd = (i + 1) * segmentVerticesCount + (j + 1) % segmentVerticesCount;
                
                // First triangle
                indexStream.AddLine(new IndexLine(currentStart, nextStart));
                indexStream.AddLine(new IndexLine(nextStart, nextEnd));
                
                // Second triangle
                indexStream.AddLine(new IndexLine(nextEnd, currentEnd));
                indexStream.AddLine(new IndexLine(currentEnd, currentStart));
            }
        }
        
        buffer.IndexBuffer.Unmap();
        buffer.VertexFormat = new VertexFormat(buffer.FormatBits);
    }
    
    public static void MapCurveSurfaceBuffer(RenderingBufferStorage buffer, IList<XYZ> vertices, double diameter)
    {
        var tubeSegments = GetSegmentationTube(vertices, diameter);
        var segmentVerticesCount = tubeSegments[0].Count;
        var newVertexCount = vertices.Count * segmentVerticesCount;
        
        buffer.VertexBufferCount = newVertexCount;
        buffer.PrimitiveCount = (vertices.Count - 1) * segmentVerticesCount * 2;
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * buffer.VertexBufferCount;
        buffer.FormatBits = VertexFormatBits.Position;
        buffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        buffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = buffer.VertexBuffer.GetVertexStreamPosition();
        
        foreach (var segment in tubeSegments)
        {
            foreach (var point in segment)
            {
                var vertexPosition = new VertexPosition(point);
                vertexStream.AddVertex(vertexPosition);
            }
        }
        
        buffer.VertexBuffer.Unmap();
        
        buffer.IndexBufferCount = (vertices.Count - 1) * segmentVerticesCount * 6 * IndexTriangle.GetSizeInShortInts();
        buffer.IndexBuffer = new IndexBuffer(buffer.IndexBufferCount);
        buffer.IndexBuffer.Map(buffer.IndexBufferCount);
        
        var indexStream = buffer.IndexBuffer.GetIndexStreamTriangle();
        
        for (var i = 0; i < vertices.Count - 1; i++)
        {
            for (var j = 0; j < segmentVerticesCount; j++)
            {
                var currentStart = i * segmentVerticesCount + j;
                var nextStart = (i + 1) * segmentVerticesCount + j;
                var currentEnd = i * segmentVerticesCount + (j + 1) % segmentVerticesCount;
                var nextEnd = (i + 1) * segmentVerticesCount + (j + 1) % segmentVerticesCount;
                
                // First triangle
                indexStream.AddTriangle(new IndexTriangle(currentStart, nextStart, nextEnd));
                
                // Second triangle
                indexStream.AddTriangle(new IndexTriangle(nextEnd, currentEnd, currentStart));
            }
        }
        
        buffer.IndexBuffer.Unmap();
        buffer.VertexFormat = new VertexFormat(buffer.FormatBits);
    }
    
    public static void MapMeshGridBuffer(RenderingBufferStorage buffer, Mesh mesh, double thickness)
    {
        var vertexCount = mesh.Vertices.Count;
        var triangleCount = mesh.NumTriangles;
        
        buffer.VertexBufferCount = 2 * vertexCount;
        buffer.PrimitiveCount = 3 * (2 * triangleCount + vertexCount);
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * buffer.VertexBufferCount;
        buffer.FormatBits = VertexFormatBits.Position;
        buffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        buffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = buffer.VertexBuffer.GetVertexStreamPosition();
        var normals = new List<XYZ>(mesh.NumberOfNormals);
        
        for (var i = 0; i < mesh.Vertices.Count; i++)
        {
            var normal = GeometryUtils.GetMeshVertexNormal(mesh, i, mesh.DistributionOfNormals);
            normals.Add(normal);
        }
        
        foreach (var vertex in mesh.Vertices)
        {
            var vertexPosition = new VertexPosition(vertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        for (var i = 0; i < mesh.Vertices.Count; i++)
        {
            var vertex = mesh.Vertices[i];
            var normal = normals[i];
            var offsetVertex = vertex + normal * thickness;
            var vertexPosition = new VertexPosition(offsetVertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        buffer.VertexBuffer.Unmap();
        buffer.IndexBufferCount = (6 * triangleCount + 3 * vertexCount) * IndexLine.GetSizeInShortInts();
        buffer.IndexBuffer = new IndexBuffer(buffer.IndexBufferCount);
        buffer.IndexBuffer.Map(buffer.IndexBufferCount);
        
        var indexStream = buffer.IndexBuffer.GetIndexStreamLine();
        
        for (var i = 0; i < triangleCount; i++)
        {
            var meshTriangle = mesh.get_Triangle(i);
            var index0 = (int) meshTriangle.get_Index(0);
            var index1 = (int) meshTriangle.get_Index(1);
            var index2 = (int) meshTriangle.get_Index(2);
            
            indexStream.AddLine(new IndexLine(index0, index1));
            indexStream.AddLine(new IndexLine(index1, index2));
            indexStream.AddLine(new IndexLine(index2, index0));
        }
        
        for (var i = 0; i < triangleCount; i++)
        {
            var meshTriangle = mesh.get_Triangle(i);
            var index0 = (int) meshTriangle.get_Index(0) + vertexCount;
            var index1 = (int) meshTriangle.get_Index(1) + vertexCount;
            var index2 = (int) meshTriangle.get_Index(2) + vertexCount;
            
            indexStream.AddLine(new IndexLine(index0, index1));
            indexStream.AddLine(new IndexLine(index1, index2));
            indexStream.AddLine(new IndexLine(index2, index0));
        }
        
        for (var i = 0; i < vertexCount; i++)
        {
            var next = (i + 1) % vertexCount;
            indexStream.AddLine(new IndexLine(i, next));
            indexStream.AddLine(new IndexLine(i, i + vertexCount));
            indexStream.AddLine(new IndexLine(next, next + vertexCount));
        }
        
        buffer.IndexBuffer.Unmap();
        buffer.VertexFormat = new VertexFormat(buffer.FormatBits);
    }
    
    public static void MapSideBuffer(RenderingBufferStorage buffer, XYZ min, XYZ max)
    {
        var vertexCount = 4;
        var normal = (max - min).Normalize();
        var length = (max - min).GetLength() / 2;
        
        XYZ point1;
        XYZ point2;
        XYZ point3;
        XYZ point4;
        if (normal.IsAlmostEqualTo(XYZ.BasisX))
        {
            point1 = new XYZ(min.X, min.Y - length, min.Z);
            point2 = new XYZ(min.X, min.Y + length, min.Z);
            point3 = new XYZ(max.X, max.Y - length, max.Z);
            point4 = new XYZ(max.X, max.Y + length, max.Z);
        }
        else if (normal.IsAlmostEqualTo(XYZ.BasisY))
        {
            point1 = new XYZ(min.X, min.Y, min.Z - length);
            point2 = new XYZ(min.X, min.Y, min.Z + length);
            point3 = new XYZ(max.X, max.Y, max.Z - length);
            point4 = new XYZ(max.X, max.Y, max.Z + length);
        }
        else
        {
            point1 = new XYZ(min.X - length, min.Y, min.Z);
            point2 = new XYZ(min.X + length, min.Y, min.Z);
            point3 = new XYZ(max.X - length, max.Y, max.Z);
            point4 = new XYZ(max.X + length, max.Y, max.Z);
        }
        
        buffer.VertexBufferCount = vertexCount;
        buffer.PrimitiveCount = 2;
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * buffer.VertexBufferCount;
        buffer.FormatBits = VertexFormatBits.Position;
        buffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        buffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = buffer.VertexBuffer.GetVertexStreamPosition();
        vertexStream.AddVertex(new VertexPosition(point1));
        vertexStream.AddVertex(new VertexPosition(point2));
        vertexStream.AddVertex(new VertexPosition(point3));
        vertexStream.AddVertex(new VertexPosition(point4));
        
        buffer.VertexBuffer.Unmap();
        buffer.IndexBufferCount = 2 * IndexTriangle.GetSizeInShortInts();
        buffer.IndexBuffer = new IndexBuffer(buffer.IndexBufferCount);
        buffer.IndexBuffer.Map(buffer.IndexBufferCount);
        
        var indexStream = buffer.IndexBuffer.GetIndexStreamTriangle();
        indexStream.AddTriangle(new IndexTriangle(0, 1, 2));
        indexStream.AddTriangle(new IndexTriangle(1, 2, 3));
        
        buffer.IndexBuffer.Unmap();
        buffer.VertexFormat = new VertexFormat(buffer.FormatBits);
    }
    
    public static void MapNormalVectorBuffer(RenderingBufferStorage buffer, XYZ origin, XYZ vector, double offset, double length)
    {
        var headSize = length > 1 ? 0.2 : length * 0.2;
        
        var arrowStart = origin + vector * offset;
        var arrowEnd = arrowStart + vector * length;
        var arrowHeadBase = arrowEnd - vector * headSize;
        var basisVector = Math.Abs(vector.Z).IsAlmostEqual(1) ? XYZ.BasisX : XYZ.BasisZ;
        var perpendicular1 = vector.CrossProduct(basisVector).Normalize().Multiply(headSize * 0.5);
        
        buffer.VertexBufferCount = 4;
        buffer.PrimitiveCount = 3;
        
        var vertexBufferSizeInFloats = 4 * VertexPosition.GetSizeInFloats();
        buffer.FormatBits = VertexFormatBits.Position;
        buffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        buffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = buffer.VertexBuffer.GetVertexStreamPosition();
        vertexStream.AddVertex(new VertexPosition(arrowStart));
        vertexStream.AddVertex(new VertexPosition(arrowEnd));
        vertexStream.AddVertex(new VertexPosition(arrowHeadBase + perpendicular1));
        vertexStream.AddVertex(new VertexPosition(arrowHeadBase - perpendicular1));
        
        buffer.VertexBuffer.Unmap();
        buffer.IndexBufferCount = 3 * IndexLine.GetSizeInShortInts();
        buffer.IndexBuffer = new IndexBuffer(buffer.IndexBufferCount);
        buffer.IndexBuffer.Map(buffer.IndexBufferCount);
        
        var indexStream = buffer.IndexBuffer.GetIndexStreamLine();
        indexStream.AddLine(new IndexLine(0, 1));
        indexStream.AddLine(new IndexLine(1, 2));
        indexStream.AddLine(new IndexLine(1, 3));
        
        buffer.IndexBuffer.Unmap();
        buffer.VertexFormat = new VertexFormat(buffer.FormatBits);
    }
    
    private static List<List<XYZ>> GetSegmentationTube(IList<XYZ> vertices, double diameter)
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
            
            points.Add(GeometryUtils.TessellateCircle(center, normal, diameter / 2));
        }
        
        return points;
    }
}