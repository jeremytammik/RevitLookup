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
using RevitLookup.Core.Visualization.Helpers;
using RevitLookup.Models.Render;

namespace RevitLookup.Core.Visualization;

public sealed class SolidVisualizationServer : IDirectContext3DServer
{
    private Solid _solid;
    private bool _hasEffectsUpdates = true;
    private bool _hasGeometryUpdates = true;

    private readonly Guid _guid = Guid.NewGuid();
    private readonly object _renderLock = new();
    private readonly List<RenderingBufferStorage> _faceBuffers = new(4);
    private readonly List<RenderingBufferStorage> _edgeBuffers = new(8);

    private double _transparency;
    private double _scale;

    private Color _faceColor;
    private Color _edgeColor;

    private bool _drawFace;
    private bool _drawEdge;

    public Guid GetServerId() => _guid;
    public string GetVendorId() => "RevitLookup";
    public string GetName() => "Solid visualization server";
    public string GetDescription() => "Solid geometry visualization";
    public ExternalServiceId GetServiceId() => ExternalServices.BuiltInExternalServices.DirectContext3DService;
    public string GetApplicationId() => string.Empty;
    public string GetSourceId() => string.Empty;
    public bool UsesHandles() => false;
    public bool CanExecute(View view) => true;
    public bool UseInTransparentPass(View view) => _drawFace && _transparency > 0;

    public Outline GetBoundingBox(View view)
    {
        var boundingBox = _solid.GetBoundingBox();
        var minPoint = boundingBox.Transform.OfPoint(boundingBox.Min);
        var maxPoint = boundingBox.Transform.OfPoint(boundingBox.Max);

        return new Outline(minPoint, maxPoint);
    }

    public void RenderScene(View view, DisplayStyle displayStyle)
    {
        lock (_renderLock)
        {
            try
            {
                if (_hasGeometryUpdates)
                {
                    MapGeometryBuffer();
                    _hasGeometryUpdates = false;
                }

                if (_hasEffectsUpdates)
                {
                    UpdateEffects();
                    _hasEffectsUpdates = false;
                }

                if (_drawFace)
                {
                    var isTransparentPass = DrawContext.IsTransparentPass();
                    if (isTransparentPass && _transparency > 0 || !isTransparentPass && _transparency == 0)
                    {
                        foreach (var buffer in _faceBuffers)
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

                if (_drawEdge)
                {
                    foreach (var buffer in _edgeBuffers)
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
                RenderFailed?.Invoke(this, new RenderFailedEventArgs
                {
                    Exception = exception
                });
            }
        }
    }

    private void MapGeometryBuffer()
    {
        var scaledSolid = RenderGeometryHelper.ScaleSolid(_solid, _scale);

        var faceIndex = 0;
        foreach (Face face in scaledSolid.Faces)
        {
            var buffer = CreateOrUpdateBuffer(_faceBuffers, faceIndex++);
            MapFaceBuffers(buffer, face);
        }

        var edgeIndex = 0;
        foreach (Edge edge in scaledSolid.Edges)
        {
            var buffer = CreateOrUpdateBuffer(_edgeBuffers, edgeIndex++);
            MapEdgeBuffers(buffer, edge);
        }
    }

    private void MapFaceBuffers(RenderingBufferStorage buffer, Face face)
    {
        var mesh = face.Triangulate();
        RenderHelper.MapSurfaceBuffer(buffer, mesh, 0);
    }

    private void MapEdgeBuffers(RenderingBufferStorage buffer, Edge edge)
    {
        var mesh = edge.Tessellate();
        RenderHelper.MapCurveBuffer(buffer, mesh);
    }

    private RenderingBufferStorage CreateOrUpdateBuffer(List<RenderingBufferStorage> buffers, int index)
    {
        RenderingBufferStorage buffer;
        if (buffers.Count > index)
        {
            buffer = buffers[index];
        }
        else
        {
            buffer = new RenderingBufferStorage();
            buffers.Add(buffer);
        }

        return buffer;
    }

    private void UpdateEffects()
    {
        foreach (var buffer in _faceBuffers)
        {
            buffer.EffectInstance ??= new EffectInstance(buffer.FormatBits);
            buffer.EffectInstance.SetColor(_faceColor);
            buffer.EffectInstance.SetTransparency(_transparency);
        }

        foreach (var buffer in _edgeBuffers)
        {
            buffer.EffectInstance ??= new EffectInstance(buffer.FormatBits);
            buffer.EffectInstance.SetColor(_edgeColor);
        }
    }

    public void UpdateFaceColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;

        lock (_renderLock)
        {
            _faceColor = value;
            _hasEffectsUpdates = true;

            uiDocument.UpdateAllOpenViews();
        }
    }

    public void UpdateEdgeColor(Color value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;

        lock (_renderLock)
        {
            _edgeColor = value;
            _hasEffectsUpdates = true;

            uiDocument.UpdateAllOpenViews();
        }
    }

    public void UpdateTransparency(double value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;

        lock (_renderLock)
        {
            _transparency = value;
            _hasEffectsUpdates = true;

            uiDocument.UpdateAllOpenViews();
        }
    }

    public void UpdateScale(double value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;

        _scale = value;

        lock (_renderLock)
        {
            _hasGeometryUpdates = true;
            _hasEffectsUpdates = true;
            _faceBuffers.Clear();
            _edgeBuffers.Clear();

            uiDocument.UpdateAllOpenViews();
        }
    }

    public void UpdateFaceVisibility(bool value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;

        lock (_renderLock)
        {
            _drawFace = value;

            uiDocument.UpdateAllOpenViews();
        }
    }

    public void UpdateEdgeVisibility(bool value)
    {
        var uiDocument = Context.UiDocument;
        if (uiDocument is null) return;

        lock (_renderLock)
        {
            _drawEdge = value;

            uiDocument.UpdateAllOpenViews();
        }
    }

    public void Register(Solid solid)
    {
        _solid = solid;

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

    public event EventHandler<RenderFailedEventArgs> RenderFailed;
}