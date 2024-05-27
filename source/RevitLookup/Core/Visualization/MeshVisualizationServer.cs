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
using RevitLookup.Core.Visualization.Helpers;
using RevitLookup.Models.Render;

namespace RevitLookup.Core.Visualization;

public sealed class MeshVisualizationServer(Mesh mesh, ILogger<MeshVisualizationServer> logger) : IDirectContext3DServer
{
    private bool _hasEffectsUpdates = true;
    private bool _hasGeometryUpdates = true;
    
    private readonly Guid _guid = Guid.NewGuid();
    
    private readonly RenderingBufferStorage _surfaceBuffer = new();
    private readonly RenderingBufferStorage _meshGridBuffer = new();
    
    private readonly RenderingBufferStorage[] _normalBuffers = Enumerable.Range(0, mesh.Vertices.Count)
        .Select(_ => new RenderingBufferStorage())
        .ToArray();
    
    private double _thickness;
    private double _transparency;
    
    private bool _drawMeshGrid;
    private bool _drawNormalVector;
    private bool _drawSurface;
    
    private Color _meshColor;
    private Color _normalColor;
    private Color _surfaceColor;
    
    public Guid GetServerId() => _guid;
    public string GetVendorId() => "RevitLookup";
    public string GetName() => "Mesh visualization server";
    public string GetDescription() => "Mesh geometry visualization";
    public ExternalServiceId GetServiceId() => ExternalServices.BuiltInExternalServices.DirectContext3DService;
    public string GetApplicationId() => string.Empty;
    public string GetSourceId() => string.Empty;
    public bool UsesHandles() => false;
    public bool CanExecute(View view) => true;
    public bool UseInTransparentPass(View view) => _drawSurface && _transparency > 0;
    
    public Outline GetBoundingBox(View view)
    {
        //TODO evaluate BB
        return null;
    }
    
    public void RenderScene(View view, DisplayStyle displayStyle)
    {
        try
        {
            if (_hasGeometryUpdates || !_surfaceBuffer.IsValid() || !_meshGridBuffer.IsValid() || _normalBuffers.Any(storage => !storage.IsValid()))
            {
                MapGeometryBuffer();
                _hasGeometryUpdates = false;
            }
            
            if (_hasEffectsUpdates)
            {
                UpdateEffects();
                _hasEffectsUpdates = false;
            }
            
            if (_drawSurface)
            {
                var isTransparentPass = DrawContext.IsTransparentPass();
                if (isTransparentPass && _transparency > 0 || !isTransparentPass && _transparency == 0)
                {
                    DrawContext.FlushBuffer(_surfaceBuffer.VertexBuffer,
                        _surfaceBuffer.VertexBufferCount,
                        _surfaceBuffer.IndexBuffer,
                        _surfaceBuffer.IndexBufferCount,
                        _surfaceBuffer.VertexFormat,
                        _surfaceBuffer.EffectInstance, PrimitiveType.TriangleList, 0,
                        _surfaceBuffer.PrimitiveCount);
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
                foreach (var buffer in _normalBuffers)
                {
                    DrawContext.FlushBuffer(buffer.VertexBuffer,
                        buffer.VertexBufferCount,
                        buffer.IndexBuffer,
                        buffer.IndexBufferCount,
                        buffer.VertexFormat,
                        buffer.EffectInstance, PrimitiveType.LineList, 0,
                        buffer.PrimitiveCount);
                }
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Rendering error");
        }
    }
    
    private void MapGeometryBuffer()
    {
        RenderHelper.MapSurfaceBuffer(_surfaceBuffer, mesh, _thickness);
        RenderHelper.MapMeshGridBuffer(_meshGridBuffer, mesh, _thickness);
        MapNormalsBuffer();
    }
    
    private void MapNormalsBuffer()
    {
        var area = RenderGeometryHelper.ComputeMeshSurfaceArea(mesh);
        var offset = RenderGeometryHelper.InterpolateOffsetByArea(area);
        var normalLength = RenderGeometryHelper.InterpolateAxisLengthByArea(area);
        
        for (var i = 0; i < mesh.Vertices.Count; i++)
        {
            var vertex = mesh.Vertices[i];
            var buffer = _normalBuffers[i];
            var normal = RenderGeometryHelper.GetMeshVertexNormal(mesh, i, mesh.DistributionOfNormals);
            
            RenderHelper.MapNormalVectorBuffer(buffer, vertex + normal * (offset + _thickness), normal, normalLength);
        }
    }
    
    private void UpdateEffects()
    {
        _surfaceBuffer.EffectInstance ??= new EffectInstance(_surfaceBuffer.FormatBits);
        _meshGridBuffer.EffectInstance ??= new EffectInstance(_surfaceBuffer.FormatBits);
        
        _surfaceBuffer.EffectInstance.SetColor(_surfaceColor);
        _meshGridBuffer.EffectInstance.SetColor(_meshColor);
        _surfaceBuffer.EffectInstance.SetTransparency(_transparency);
        
        foreach (var normalBuffer in _normalBuffers)
        {
            normalBuffer.EffectInstance ??= new EffectInstance(_surfaceBuffer.FormatBits);
            normalBuffer.EffectInstance.SetColor(_normalColor);
        }
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