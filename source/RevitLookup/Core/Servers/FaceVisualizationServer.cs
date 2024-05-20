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
using Autodesk.Revit.DB.ExternalService;
using RevitLookup.Models.Geometry;

namespace RevitLookup.Core.Servers;

public sealed class FaceVisualizationServer(Face face) : IDirectContext3DServer
{
    private readonly Guid _guid = Guid.NewGuid();
    private readonly ColorWithTransparency _highlightColor = new(0, 0, 255, 0);
    private readonly RenderingBufferStorage _faceBuffer = new();
    
    public Guid GetServerId() => _guid;
    public string GetVendorId() => "RevitLookup";
    public string GetName() => "Face visualization server";
    public string GetDescription() => "Face geometry visualization";
    public ExternalServiceId GetServiceId() => ExternalServices.BuiltInExternalServices.DirectContext3DService;
    public string GetApplicationId() => string.Empty;
    public string GetSourceId() => string.Empty;
    public bool UsesHandles() => false;
    public bool CanExecute(View view) => true;
    public Outline GetBoundingBox(View view) => null;
    public bool UseInTransparentPass(View view) => true;
    
    public void RenderScene(View view, DisplayStyle displayStyle)
    {
        if (_faceBuffer.IsUpdateRequire())
        {
            UpdateGeometryBuffer();
        }
        
        if (_faceBuffer!.PrimitiveCount > 0)
        {
            DrawContext.FlushBuffer(_faceBuffer.VertexBuffer,
                _faceBuffer.VertexBufferCount,
                _faceBuffer.IndexBuffer,
                _faceBuffer.IndexBufferCount,
                _faceBuffer.VertexFormat,
                _faceBuffer.EffectInstance, PrimitiveType.TriangleList, 0,
                _faceBuffer.PrimitiveCount);
        }
    }
    
    private void UpdateGeometryBuffer()
    {
        var mesh = face.Triangulate();
        var faceBox = face.GetBoundingBox();
        var center = (faceBox.Min + faceBox.Max) / 2;
        var normal = face.ComputeNormal(center);
        
        _faceBuffer.VertexBufferCount += mesh.Vertices.Count;
        _faceBuffer.PrimitiveCount += mesh.NumTriangles;
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * _faceBuffer.VertexBufferCount;
        
        _faceBuffer.FormatBits = VertexFormatBits.Position;
        _faceBuffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        _faceBuffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = _faceBuffer.VertexBuffer.GetVertexStreamPosition();
        foreach (var vertex in mesh.Vertices)
        {
            var vertexPosition = new VertexPosition(vertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        _faceBuffer.VertexBuffer.Unmap();
        _faceBuffer.IndexBufferCount = _faceBuffer.PrimitiveCount * IndexTriangle.GetSizeInShortInts();
        _faceBuffer.IndexBuffer = new IndexBuffer(_faceBuffer.IndexBufferCount);
        _faceBuffer.IndexBuffer.Map(_faceBuffer.IndexBufferCount);
        var indexStream = _faceBuffer.IndexBuffer.GetIndexStreamTriangle();
        for (var i = 0; i < mesh.NumTriangles; i++)
        {
            var meshTriangle = mesh.get_Triangle(i);
            var index0 = (int) meshTriangle.get_Index(0);
            var index1 = (int) meshTriangle.get_Index(1);
            var index2 = (int) meshTriangle.get_Index(2);
            indexStream.AddTriangle(new IndexTriangle(index0, index1, index2));
        }
        
        _faceBuffer.IndexBuffer.Unmap();
        _faceBuffer.VertexFormat = new VertexFormat(_faceBuffer.FormatBits);
        
        var effectInstance = new EffectInstance(_faceBuffer.FormatBits);
        effectInstance.SetColor(new Color(255, 35, 100));
        effectInstance.SetTransparency(30);
        effectInstance.SetGlossiness(100);
        effectInstance.SetDiffuseColor(new Color(0, 0, 255));
        _faceBuffer.EffectInstance = effectInstance;
    }
}