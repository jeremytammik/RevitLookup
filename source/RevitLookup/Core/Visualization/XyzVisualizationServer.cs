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
using RevitLookup.Models.Render;

namespace RevitLookup.Core.Visualization;

public sealed class XyzVisualizationServer(XYZ point, ILogger<XyzVisualizationServer> logger) : IDirectContext3DServer
{
    private readonly RenderingBufferStorage[] _axisBuffers = Enumerable.Range(0, 3)
        .Select(_ => new RenderingBufferStorage())
        .ToArray();
    
    private readonly Guid _guid = Guid.NewGuid();
    
    private readonly XYZ[] _normals =
    [
        XYZ.BasisX,
        XYZ.BasisY,
        XYZ.BasisZ
    ];
    
    private readonly RenderingBufferStorage[] _planeBuffers = Enumerable.Range(0, 3)
        .Select(_ => new RenderingBufferStorage())
        .ToArray();
    
    private Color _axisColor;
    
    private double _axisLength;
    private bool _drawPlane;
    private bool _hasEffectsUpdates = true;
    private bool _hasGeometryUpdates = true;
    private Color _planeColor;
    private double _transparency;
    
    public Guid GetServerId() => _guid;
    public string GetVendorId() => "RevitLookup";
    public string GetName() => "XYZ visualization server";
    public string GetDescription() => "XYZ geometry visualization";
    public ExternalServiceId GetServiceId() => ExternalServices.BuiltInExternalServices.DirectContext3DService;
    public string GetApplicationId() => string.Empty;
    public string GetSourceId() => string.Empty;
    public bool UsesHandles() => false;
    public bool CanExecute(View view) => true;
    public bool UseInTransparentPass(View view) => true;
    
    public Outline GetBoundingBox(View view)
    {
        return null;
    }
    
    public void RenderScene(View view, DisplayStyle displayStyle)
    {
        try
        {
            if (_hasGeometryUpdates || _planeBuffers.Any(storage => !storage.IsValid()) || _axisBuffers.Any(storage => !storage.IsValid()))
            {
                UpdateGeometryBuffer();
                _hasGeometryUpdates = false;
            }
            
            if (_hasEffectsUpdates)
            {
                UpdateEffects();
                _hasEffectsUpdates = false;
            }
            
            foreach (var buffer in _axisBuffers)
            {
                DrawContext.FlushBuffer(buffer.VertexBuffer,
                    buffer.VertexBufferCount,
                    buffer.IndexBuffer,
                    buffer.IndexBufferCount,
                    buffer.VertexFormat,
                    buffer.EffectInstance, PrimitiveType.LineList, 0,
                    buffer.PrimitiveCount);
            }
            
            if (_drawPlane)
            {
                var isTransparentPass = DrawContext.IsTransparentPass();
                if (isTransparentPass && _transparency > 0 || !isTransparentPass && _transparency == 0)
                {
                    foreach (var buffer in _planeBuffers)
                    {
                        DrawContext.FlushBuffer(buffer.VertexBuffer,
                            buffer.VertexBufferCount,
                            buffer.IndexBuffer,
                            buffer.IndexBufferCount,
                            buffer.VertexFormat,
                            buffer.EffectInstance, PrimitiveType.TriangleList, 0,
                            buffer.PrimitiveCount);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Rendering error");
        }
    }
    
    private void UpdateGeometryBuffer()
    {
        MapNormalBuffer();
        MapPlaneBuffer();
    }
    
    private void MapNormalBuffer()
    {
        for (var i = 0; i < _normals.Length; i++)
        {
            var normal = _normals[i];
            var buffer = _axisBuffers[i];
            Render3dUtils.MapNormalVectorBuffer(buffer, point - normal * _axisLength, normal, 0, _axisLength * 2);
        }
    }
    
    private void MapPlaneBuffer()
    {
        for (var i = 0; i < _normals.Length; i++)
        {
            var normal = _normals[i];
            var buffer = _planeBuffers[i];
            Render3dUtils.MapSideBuffer(buffer, point - normal * _axisLength, point + normal * _axisLength);
        }
    }
    
    private void UpdateEffects()
    {
        foreach (var buffer in _planeBuffers)
        {
            buffer.EffectInstance ??= new EffectInstance(buffer.FormatBits);
            buffer.EffectInstance.SetColor(_planeColor);
            buffer.EffectInstance.SetTransparency(_transparency);
        }
        
        foreach (var buffer in _axisBuffers)
        {
            buffer.EffectInstance ??= new EffectInstance(buffer.FormatBits);
            buffer.EffectInstance.SetColor(_axisColor);
        }
    }
    
    public void UpdatePlaneColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _planeColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateAxisColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _axisColor = value;
        _hasEffectsUpdates = true;
        
        uiDocument.UpdateAllOpenViews();
    }
    
    public void UpdateAxisLength(double value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _axisLength = value;
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
    
    public void UpdatePlaneVisibility(bool visible)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;
        
        _drawPlane = visible;
        
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