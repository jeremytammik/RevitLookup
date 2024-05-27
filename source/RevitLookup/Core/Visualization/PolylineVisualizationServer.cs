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
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Visualization.Helpers;
using RevitLookup.Models.Render;

namespace RevitLookup.Core.Visualization;

public sealed class PolylineVisualizationServer : IDirectContext3DServer
{
    private bool _hasEffectsUpdates = true;
    private bool _hasGeometryUpdates = true;
    
    private readonly Guid _guid = Guid.NewGuid();
    private readonly ILogger<PolylineVisualizationServer> _logger;
    
    private readonly RenderingBufferStorage _surfaceBuffer = new();
    private readonly RenderingBufferStorage _curveBuffer = new();
    private readonly List<RenderingBufferStorage> _normalsBuffers = new(1);
    private readonly IList<XYZ> _vertices;
    
    private double _transparency;
    private double _diameter;
    
    private Color _surfaceColor;
    private Color _curveColor;
    private Color _directionColor;
    
    private bool _drawCurve;
    private bool _drawDirection;
    private bool _drawSurface;
    
    public PolylineVisualizationServer(Edge edge, ILogger<PolylineVisualizationServer> logger)
    {
        _vertices = edge.Tessellate();
        _logger = logger;
    }
    
    public PolylineVisualizationServer(Curve edge, ILogger<PolylineVisualizationServer> logger)
    {
        _vertices = edge.Tessellate();
        _logger = logger;
    }
    
    public Guid GetServerId() => _guid;
    public string GetVendorId() => "RevitLookup";
    public string GetName() => "Polyline visualization server";
    public string GetDescription() => "Polyline geometry visualization";
    public ExternalServiceId GetServiceId() => ExternalServices.BuiltInExternalServices.DirectContext3DService;
    public string GetApplicationId() => string.Empty;
    public string GetSourceId() => string.Empty;
    public bool UsesHandles() => false;
    public bool CanExecute(View view) => true;
    public bool UseInTransparentPass(View view) => _transparency > 0;
    
    public Outline GetBoundingBox(View view)
    {
        //TODO evaluate BB
        return null;
    }
    
    public void RenderScene(View view, DisplayStyle displayStyle)
    {
        try
        {
            if (_hasGeometryUpdates || !_surfaceBuffer.IsValid() || !_curveBuffer.IsValid() || _normalsBuffers.Any(storage => !storage.IsValid()))
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
            
            if (_drawCurve)
            {
                DrawContext.FlushBuffer(_curveBuffer.VertexBuffer,
                    _curveBuffer.VertexBufferCount,
                    _curveBuffer.IndexBuffer,
                    _curveBuffer.IndexBufferCount,
                    _curveBuffer.VertexFormat,
                    _curveBuffer.EffectInstance, PrimitiveType.LineList, 0,
                    _curveBuffer.PrimitiveCount);
            }
            
            if (_drawDirection)
            {
                foreach (var buffer in _normalsBuffers)
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
            _logger.LogError(exception, "Rendering error");
        }
    }
    
    private void MapGeometryBuffer()
    {
        RenderHelper.MapCurveSurfaceBuffer(_surfaceBuffer, _vertices, _diameter);
        RenderHelper.MapCurveBuffer(_curveBuffer, _vertices, _diameter);
        MapDirectionsBuffer();
    }
    
    private void MapDirectionsBuffer()
    {
        var verticalOffset = 0d;
        
        for (var i = 0; i < _vertices.Count - 1; i++)
        {
            var startPoint = _vertices[i];
            var endPoint = _vertices[i + 1];
            var centerPoint = (startPoint + endPoint) / 2;
            var buffer = CreateNormalBuffer(i);
            
            var segmentVector = endPoint - startPoint;
            var segmentLength = segmentVector.GetLength();
            var segmentDirection = segmentVector.Normalize();
            if (verticalOffset == 0)
            {
                verticalOffset = RenderGeometryHelper.InterpolateOffsetByDiameter(_diameter) + _diameter / 2d;
            }
            
            var arrowLength = segmentLength > 1 ? 1d : segmentLength * 0.6;
            
            var offsetVector = XYZ.BasisX.CrossProduct(segmentDirection).Normalize() * verticalOffset;
            if (offsetVector.IsZeroLength())
            {
                offsetVector = XYZ.BasisY.CrossProduct(segmentDirection).Normalize() * verticalOffset;
            }
            
            var arrowOrigin = centerPoint + offsetVector - segmentDirection * (arrowLength / 2);
            
            RenderHelper.MapNormalVectorBuffer(buffer, arrowOrigin, segmentDirection, arrowLength);
        }
    }
    
    
    private RenderingBufferStorage CreateNormalBuffer(int vertexIndex)
    {
        RenderingBufferStorage buffer;
        if (_normalsBuffers.Count > vertexIndex)
        {
            buffer = _normalsBuffers[vertexIndex];
        }
        else
        {
            buffer = new RenderingBufferStorage();
            _normalsBuffers.Add(buffer);
        }
        
        return buffer;
    }
    
    private void UpdateEffects()
    {
        _surfaceBuffer.EffectInstance ??= new EffectInstance(_surfaceBuffer.FormatBits);
        _surfaceBuffer.EffectInstance.SetColor(_surfaceColor);
        _surfaceBuffer.EffectInstance.SetTransparency(_transparency);
        
        _curveBuffer.EffectInstance ??= new EffectInstance(_curveBuffer.FormatBits);
        _curveBuffer.EffectInstance.SetColor(_curveColor);
        
        foreach (var buffer in _normalsBuffers)
        {
            buffer.EffectInstance ??= new EffectInstance(buffer.FormatBits);
            buffer.EffectInstance.SetColor(_directionColor);
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
    
    public void UpdateCurveColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _curveColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateDirectionColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _directionColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateDiameter(double value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _diameter = value;
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
    
    public void UpdateCurveVisibility(bool visible)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _drawCurve = visible;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateDirectionVisibility(bool visible)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _drawDirection = visible;
        
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