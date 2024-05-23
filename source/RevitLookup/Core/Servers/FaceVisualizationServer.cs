﻿// Copyright 2003-2024 by Autodesk, Inc.
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
using Microsoft.Extensions.Logging;
using RevitLookup.Models.Geometry;

namespace RevitLookup.Core.Servers;

public sealed class FaceVisualizationServer(Face face, ILogger<FaceVisualizationServer> logger) : IDirectContext3DServer
{
    private bool _hasGeometryUpdates = true;
    private bool _hasEffectsUpdates = true;
    
    private readonly Guid _guid = Guid.NewGuid();
    private readonly RenderingBufferStorage _faceBuffer = new();
    private readonly RenderingBufferStorage _meshGridBuffer = new();
    private readonly RenderingBufferStorage _normalBuffer = new();
    
    private double _thickness;
    private double _transparency;
    private bool _drawMeshGrid;
    private bool _drawSurface;
    private bool _drawNormalVector;
    private Color _surfaceColor;
    private Color _meshColor;
    private Color _normalColor;
    
    public Guid GetServerId() => _guid;
    public string GetVendorId() => "RevitLookup";
    public string GetName() => "Face visualization server";
    public string GetDescription() => "Face geometry visualization";
    public ExternalServiceId GetServiceId() => ExternalServices.BuiltInExternalServices.DirectContext3DService;
    public string GetApplicationId() => string.Empty;
    public string GetSourceId() => string.Empty;
    public bool UsesHandles() => false;
    public bool CanExecute(View view) => true;
    public bool UseInTransparentPass(View view) => true;
    
    public Outline GetBoundingBox(View view)
    {
        var element = face.Reference.ElementId.ToElement(view.Document)!;
        var boundingBox = element.get_BoundingBox(view) ?? element.get_BoundingBox(null);
        
        return new Outline(boundingBox.Min, boundingBox.Max);
    }
    
    public void RenderScene(View view, DisplayStyle displayStyle)
    {
        try
        {
            if (_hasGeometryUpdates || !_faceBuffer.IsValid() || !_meshGridBuffer.IsValid() || !_normalBuffer.IsValid())
            {
                UpdateGeometryBuffer();
                _hasGeometryUpdates = false;
            }
            
            if (_hasEffectsUpdates)
            {
                UpdateEffects();
                _hasEffectsUpdates = false;
            }
            
            if (_faceBuffer!.PrimitiveCount == 0) return;
            
            if (_drawSurface)
            {
                var isTransparentPass = DrawContext.IsTransparentPass();
                if (isTransparentPass && _transparency > 0 || !isTransparentPass && _transparency == 0)
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
            
            if (_drawMeshGrid)
            {
                DrawContext.FlushBuffer(_meshGridBuffer.VertexBuffer,
                    _meshGridBuffer.VertexBufferCount,
                    _meshGridBuffer.IndexBuffer,
                    _meshGridBuffer.IndexBufferCount,
                    _meshGridBuffer.VertexFormat,
                    _meshGridBuffer.EffectInstance, PrimitiveType.LineList, 0,
                    _meshGridBuffer.PrimitiveCount);
            }
            
            if (_drawNormalVector)
            {
                DrawContext.FlushBuffer(_normalBuffer.VertexBuffer,
                    _normalBuffer.VertexBufferCount,
                    _normalBuffer.IndexBuffer,
                    _normalBuffer.IndexBufferCount,
                    _normalBuffer.VertexFormat,
                    _normalBuffer.EffectInstance, PrimitiveType.LineList, 0,
                    _normalBuffer.PrimitiveCount);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Rendering error");
        }
    }
    
    private void UpdateGeometryBuffer()
    {
        var mesh = face.Triangulate();
        var faceBox = face.GetBoundingBox();
        var center = (faceBox.Min + faceBox.Max) / 2;
        var normal = face.ComputeNormal(center);
        
        UpdateFaceBuffer(mesh, normal);
        UpdateMeshGridBuffer(mesh, normal);
        UpdateNormalVectorBuffer(face.Evaluate(center), normal);
    }
    
    private void UpdateFaceBuffer(Mesh mesh, XYZ normal)
    {
        var vertexCount = mesh.Vertices.Count;
        var triangleCount = mesh.NumTriangles;
        
        _faceBuffer.VertexBufferCount = 2 * vertexCount;
        _faceBuffer.PrimitiveCount = 2 * triangleCount + 2 * vertexCount;
        
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
        
        foreach (var vertex in mesh.Vertices)
        {
            var offsetVertex = vertex + normal.Multiply(_thickness);
            var vertexPosition = new VertexPosition(offsetVertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        _faceBuffer.VertexBuffer.Unmap();
        _faceBuffer.IndexBufferCount = 6 * triangleCount + 12 * vertexCount;
        _faceBuffer.IndexBuffer = new IndexBuffer(_faceBuffer.IndexBufferCount);
        _faceBuffer.IndexBuffer.Map(_faceBuffer.IndexBufferCount);
        
        var indexStream = _faceBuffer.IndexBuffer.GetIndexStreamTriangle();
        
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
        
        _faceBuffer.IndexBuffer.Unmap();
        _faceBuffer.VertexFormat = new VertexFormat(_faceBuffer.FormatBits);
    }
    
    private void UpdateMeshGridBuffer(Mesh mesh, XYZ normal)
    {
        var vertexCount = mesh.Vertices.Count;
        var triangleCount = mesh.NumTriangles;
        
        _meshGridBuffer.VertexBufferCount = 2 * vertexCount;
        _meshGridBuffer.PrimitiveCount = 3 * (2 * triangleCount + vertexCount);
        
        var vertexBufferSizeInFloats = VertexPosition.GetSizeInFloats() * _meshGridBuffer.VertexBufferCount;
        _meshGridBuffer.FormatBits = VertexFormatBits.Position;
        _meshGridBuffer.VertexBuffer = new VertexBuffer(vertexBufferSizeInFloats);
        _meshGridBuffer.VertexBuffer.Map(vertexBufferSizeInFloats);
        
        var vertexStream = _meshGridBuffer.VertexBuffer.GetVertexStreamPosition();
        foreach (var vertex in mesh.Vertices)
        {
            var vertexPosition = new VertexPosition(vertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        foreach (var vertex in mesh.Vertices)
        {
            var offsetVertex = vertex + normal * _thickness;
            var vertexPosition = new VertexPosition(offsetVertex);
            vertexStream.AddVertex(vertexPosition);
        }
        
        _meshGridBuffer.VertexBuffer.Unmap();
        _meshGridBuffer.IndexBufferCount = (6 * triangleCount + 3 * vertexCount) * IndexLine.GetSizeInShortInts();
        _meshGridBuffer.IndexBuffer = new IndexBuffer(_meshGridBuffer.IndexBufferCount);
        _meshGridBuffer.IndexBuffer.Map(_meshGridBuffer.IndexBufferCount);
        
        var indexStream = _meshGridBuffer.IndexBuffer.GetIndexStreamLine();
        
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
        
        _meshGridBuffer.IndexBuffer.Unmap();
        _meshGridBuffer.VertexFormat = new VertexFormat(_meshGridBuffer.FormatBits);
    }
    
    public void UpdateNormalVectorBuffer(XYZ origin, XYZ normal)
    {
        const double arrowLength = 1d;
        const double arrowHeadSize = 0.2;
        
        var arrowStart = origin + 0.2 * normal + normal * _thickness;
        var arrowEnd = arrowStart + normal * arrowLength;
        var arrowHeadBase = arrowEnd - normal.Multiply(arrowHeadSize);
        var basisVector = Math.Abs(normal.Z).IsAlmostEqual(1) ? XYZ.BasisX : XYZ.BasisZ;
        var perpendicular1 = normal.CrossProduct(basisVector).Normalize().Multiply(arrowHeadSize * 0.5);
        
        _normalBuffer.VertexBufferCount = 4;
        _normalBuffer.PrimitiveCount = 3;
        _normalBuffer.FormatBits = VertexFormatBits.Position;
        _normalBuffer.VertexBuffer = new VertexBuffer(4 * VertexPosition.GetSizeInFloats());
        _normalBuffer.VertexBuffer.Map(4 * VertexPosition.GetSizeInFloats());
        
        var vertexStream = _normalBuffer.VertexBuffer.GetVertexStreamPosition();
        vertexStream.AddVertex(new VertexPosition(arrowStart));
        vertexStream.AddVertex(new VertexPosition(arrowEnd));
        vertexStream.AddVertex(new VertexPosition(arrowHeadBase + perpendicular1));
        vertexStream.AddVertex(new VertexPosition(arrowHeadBase - perpendicular1));
        
        _normalBuffer.VertexBuffer.Unmap();
        
        _normalBuffer.IndexBufferCount = 3 * IndexLine.GetSizeInShortInts();
        _normalBuffer.IndexBuffer = new IndexBuffer(_normalBuffer.IndexBufferCount);
        _normalBuffer.IndexBuffer.Map(_normalBuffer.IndexBufferCount);
        
        var indexStream = _normalBuffer.IndexBuffer.GetIndexStreamLine();
        indexStream.AddLine(new IndexLine(0, 1));
        indexStream.AddLine(new IndexLine(1, 2));
        indexStream.AddLine(new IndexLine(1, 3));
        
        _normalBuffer.IndexBuffer.Unmap();
        
        _normalBuffer.VertexFormat = new VertexFormat(_normalBuffer.FormatBits);
    }
    
    private void UpdateEffects()
    {
        _faceBuffer.EffectInstance ??= new EffectInstance(_faceBuffer.FormatBits);
        _meshGridBuffer.EffectInstance ??= new EffectInstance(_faceBuffer.FormatBits);
        _normalBuffer.EffectInstance ??= new EffectInstance(_faceBuffer.FormatBits);
        
        _faceBuffer.EffectInstance.SetColor(_surfaceColor);
        _meshGridBuffer.EffectInstance.SetColor(_meshColor);
        _normalBuffer.EffectInstance.SetColor(_normalColor);
        _faceBuffer.EffectInstance.SetTransparency(_transparency);
    }
    
    public void UpdateSurfaceColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _surfaceColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateMeshGridColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _meshColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateNormalVectorColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _normalColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateThickness(double value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _thickness = value;
        _hasGeometryUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateTransparency(double value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _transparency = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    
    public void UpdateSurfaceVisibility(bool visible)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _drawSurface = visible;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateMeshGridVisibility(bool visible)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _drawMeshGrid = visible;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateNormalVectorVisibility(bool visible)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _drawNormalVector = visible;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void Register()
    {
        Application.ActionEventHandler.Raise(application =>
        {
            if (application.ActiveUIDocument is null) return;
            
            var directContextService = (MultiServerService) ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.DirectContext3DService);
            var serverIds = directContextService.GetActiveServerIds();
            
            directContextService.AddServer(this);
            serverIds.Add(GetServerId());
            directContextService.SetActiveServers(serverIds);
            
            application.ActiveUIDocument.UpdateAllOpenViews();
        });
    }
    
    public void Unregister()
    {
        Application.ActionEventHandler.Raise(application =>
        {
            var directContextService = (MultiServerService) ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.DirectContext3DService);
            directContextService.RemoveServer(GetServerId());
            
            application.ActiveUIDocument?.UpdateAllOpenViews();
        });
    }
}