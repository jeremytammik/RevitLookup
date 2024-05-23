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

namespace RevitLookup.Core.Render;

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
            var normal = GetNormal(mesh, i, mesh.DistributionOfNormals);
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
            var normal = GetNormal(mesh, i, mesh.DistributionOfNormals);
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
    
    public static void MapNormalVectorBuffer(RenderingBufferStorage buffer, XYZ origin, XYZ normal, double thickness)
    {
        const double arrowLength = 1d;
        const double arrowHeadSize = 0.2;
        
        var arrowStart = origin + 0.2 * normal + normal * thickness;
        var arrowEnd = arrowStart + normal * arrowLength;
        var arrowHeadBase = arrowEnd - normal.Multiply(arrowHeadSize);
        var basisVector = Math.Abs(normal.Z).IsAlmostEqual(1) ? XYZ.BasisX : XYZ.BasisZ;
        var perpendicular1 = normal.CrossProduct(basisVector).Normalize().Multiply(arrowHeadSize * 0.5);
        
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
    
    public static XYZ GetNormal(Mesh mesh, int index, DistributionOfNormals normalDistribution)
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
}