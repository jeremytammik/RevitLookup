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

public sealed partial class PolylineVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<PolylineVisualizationViewModel> logger)
    : ObservableObject
{
    private readonly PolylineVisualizationServer _server = new();
    
    [ObservableProperty] private double _diameter = settingsService.RenderSettings.PolylineSettings.Diameter;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.PolylineSettings.Transparency;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = settingsService.RenderSettings.PolylineSettings.SurfaceColor;
    [ObservableProperty] private System.Windows.Media.Color _curveColor = settingsService.RenderSettings.PolylineSettings.CurveColor;
    [ObservableProperty] private System.Windows.Media.Color _directionColor = settingsService.RenderSettings.PolylineSettings.DirectionColor;
    
    [ObservableProperty] private bool _showSurface = settingsService.RenderSettings.PolylineSettings.ShowSurface;
    [ObservableProperty] private bool _showCurve = settingsService.RenderSettings.PolylineSettings.ShowCurve;
    [ObservableProperty] private bool _showDirection = settingsService.RenderSettings.PolylineSettings.ShowDirection;
    
    public double MinThickness => settingsService.RenderSettings.PolylineSettings.MinThickness;
    
    public void RegisterServer(Curve curve)
    {
        Initialize();
        _server.RenderFailed += HandleRenderFailure;
        _server.Register(curve.Tessellate());
    }
    
    public void RegisterServer(Edge edge)
    {
        Initialize();
        _server.RenderFailed += HandleRenderFailure;
        _server.Register(edge.Tessellate());
    }
    
    private void Initialize()
    {
        UpdateShowSurface(ShowSurface);
        UpdateShowCurve(ShowCurve);
        UpdateShowDirection(ShowDirection);
        
        UpdateSurfaceColor(SurfaceColor);
        UpdateCurveColor(CurveColor);
        UpdateDirectionColor(DirectionColor);
        
        UpdateTransparency(Transparency);
        UpdateDiameter(Diameter);
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
    
    partial void OnSurfaceColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.PolylineSettings.SurfaceColor = value;
        UpdateSurfaceColor(value);
    }
    
    partial void OnCurveColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.PolylineSettings.CurveColor = value;
        UpdateCurveColor(value);
    }
    
    partial void OnDirectionColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.PolylineSettings.DirectionColor = value;
        UpdateDirectionColor(value);
    }
    
    partial void OnDiameterChanged(double value)
    {
        settingsService.RenderSettings.PolylineSettings.Diameter = value;
        UpdateDiameter(value);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.PolylineSettings.Transparency = value;
        UpdateTransparency(value);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        settingsService.RenderSettings.PolylineSettings.ShowSurface = value;
        UpdateShowSurface(value);
    }
    
    partial void OnShowCurveChanged(bool value)
    {
        settingsService.RenderSettings.PolylineSettings.ShowCurve = value;
        UpdateShowCurve(value);
    }
    
    partial void OnShowDirectionChanged(bool value)
    {
        settingsService.RenderSettings.PolylineSettings.ShowDirection = value;
        UpdateShowDirection(value);
    }
    
    private void UpdateSurfaceColor(System.Windows.Media.Color value)
    {
        _server.UpdateSurfaceColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateCurveColor(System.Windows.Media.Color value)
    {
        _server.UpdateCurveColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateDirectionColor(System.Windows.Media.Color value)
    {
        _server.UpdateDirectionColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateDiameter(double value)
    {
        _server.UpdateDiameter(value / 12);
    }
    
    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    private void UpdateShowSurface(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
    
    private void UpdateShowCurve(bool value)
    {
        _server.UpdateCurveVisibility(value);
    }
    
    private void UpdateShowDirection(bool value)
    {
        _server.UpdateDirectionVisibility(value);
    }
}