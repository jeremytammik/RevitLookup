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

using Microsoft.Extensions.Logging;
using RevitLookup.Core.Visualization;
using RevitLookup.Models.Render;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.ViewModels.Dialogs.Visualization;

public sealed partial class BoundingBoxVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<BoundingBoxVisualizationViewModel> logger)
    : ObservableObject
{
    private readonly BoundingBoxVisualizationServer _server = new();
    
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.BoundingBoxSettings.Transparency;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = settingsService.RenderSettings.BoundingBoxSettings.SurfaceColor;
    [ObservableProperty] private System.Windows.Media.Color _edgeColor = settingsService.RenderSettings.BoundingBoxSettings.EdgeColor;
    [ObservableProperty] private System.Windows.Media.Color _axisColor = settingsService.RenderSettings.BoundingBoxSettings.AxisColor;
    
    [ObservableProperty] private bool _showSurface = settingsService.RenderSettings.BoundingBoxSettings.ShowSurface;
    [ObservableProperty] private bool _showEdge = settingsService.RenderSettings.BoundingBoxSettings.ShowEdge;
    [ObservableProperty] private bool _showAxis = settingsService.RenderSettings.BoundingBoxSettings.ShowAxis;
    
    public void RegisterServer(BoundingBoxXYZ box)
    {
        UpdateShowSurface(ShowSurface);
        UpdateShowEdge(ShowEdge);
        UpdateShowAxis(ShowAxis);
        
        UpdateSurfaceColor(SurfaceColor);
        UpdateEdgeColor(EdgeColor);
        UpdateAxisColor(AxisColor);
        
        UpdateTransparency(Transparency);
        
        _server.RenderFailed += HandleRenderFailure;
        _server.Register(box);
    }
    
    public void UnregisterServer()
    {
        _server.RenderFailed -= HandleRenderFailure;
        _server.Unregister();
    }
    
    private void HandleRenderFailure(object sender, RenderFailedEventArgs args)
    {
        logger.LogError(args.Exception, "Render error");
        notificationService.ShowError("Render error", args.Exception);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.Transparency = value;
        UpdateTransparency(value);
    }
    
    partial void OnSurfaceColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.SurfaceColor = value;
        UpdateSurfaceColor(value);
    }
    
    partial void OnEdgeColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.EdgeColor = value;
        UpdateEdgeColor(value);
    }
    
    partial void OnAxisColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.AxisColor = value;
        UpdateAxisColor(value);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.ShowSurface = value;
        UpdateShowSurface(value);
    }
    
    partial void OnShowEdgeChanged(bool value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.ShowEdge = value;
        UpdateShowEdge(value);
    }
    
    partial void OnShowAxisChanged(bool value)
    {
        settingsService.RenderSettings.BoundingBoxSettings.ShowEdge = value;
        UpdateShowAxis(value);
    }
    
    private void UpdateSurfaceColor(System.Windows.Media.Color value)
    {
        _server.UpdateSurfaceColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateEdgeColor(System.Windows.Media.Color value)
    {
        _server.UpdateEdgeColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateAxisColor(System.Windows.Media.Color value)
    {
        _server.UpdateAxisColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    private void UpdateShowSurface(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
    
    private void UpdateShowEdge(bool value)
    {
        _server.UpdateEdgeVisibility(value);
    }
    
    private void UpdateShowAxis(bool value)
    {
        _server.UpdateAxisVisibility(value);
    }
}