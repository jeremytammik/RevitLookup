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

public sealed partial class FaceVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<FaceVisualizationViewModel> logger)
    : ObservableObject
{
    private readonly FaceVisualizationServer _server = new();
    
    [ObservableProperty] private double _extrusion = settingsService.RenderSettings.FaceSettings.Extrusion;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.FaceSettings.Transparency;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = settingsService.RenderSettings.FaceSettings.SurfaceColor;
    [ObservableProperty] private System.Windows.Media.Color _meshColor = settingsService.RenderSettings.FaceSettings.MeshColor;
    [ObservableProperty] private System.Windows.Media.Color _normalVectorColor = settingsService.RenderSettings.FaceSettings.NormalVectorColor;
    
    [ObservableProperty] private bool _showSurface = settingsService.RenderSettings.FaceSettings.ShowSurface;
    [ObservableProperty] private bool _showMeshGrid = settingsService.RenderSettings.FaceSettings.ShowMeshGrid;
    [ObservableProperty] private bool _showNormalVector = settingsService.RenderSettings.FaceSettings.ShowNormalVector;
    
    public double MinExtrusion => settingsService.RenderSettings.FaceSettings.MinExtrusion;
    
    public void RegisterServer(Face face)
    {
        UpdateShowSurface(ShowSurface);
        UpdateShowMeshGrid(ShowMeshGrid);
        UpdateShowNormalVector(ShowNormalVector);
        
        UpdateSurfaceColor(SurfaceColor);
        UpdateMeshColor(MeshColor);
        UpdateNormalVectorColor(NormalVectorColor);
        
        UpdateTransparency(Transparency);
        UpdateExtrusion(Extrusion);
        
        _server.RenderFailed += HandleRenderFailure;
        _server.Register(face);
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
        settingsService.RenderSettings.FaceSettings.SurfaceColor = value;
        UpdateSurfaceColor(value);
    }
    
    partial void OnMeshColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.FaceSettings.MeshColor = value;
        UpdateMeshColor(value);
    }
    
    partial void OnNormalVectorColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.FaceSettings.NormalVectorColor = value;
        UpdateNormalVectorColor(value);
    }
    
    partial void OnExtrusionChanged(double value)
    {
        settingsService.RenderSettings.FaceSettings.Extrusion = value;
        UpdateExtrusion(value);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.FaceSettings.Transparency = value;
        UpdateTransparency(value);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        settingsService.RenderSettings.FaceSettings.ShowSurface = value;
        UpdateShowSurface(value);
    }
    
    partial void OnShowMeshGridChanged(bool value)
    {
        settingsService.RenderSettings.FaceSettings.ShowMeshGrid = value;
        UpdateShowMeshGrid(value);
    }
    
    partial void OnShowNormalVectorChanged(bool value)
    {
        settingsService.RenderSettings.FaceSettings.ShowNormalVector = value;
        UpdateShowNormalVector(value);
    }
    
    private void UpdateSurfaceColor(System.Windows.Media.Color value)
    {
        _server.UpdateSurfaceColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateMeshColor(System.Windows.Media.Color value)
    {
        _server.UpdateMeshGridColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateNormalVectorColor(System.Windows.Media.Color value)
    {
        _server.UpdateNormalVectorColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateExtrusion(double value)
    {
        _server.UpdateExtrusion(value / 12);
    }
    
    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    private void UpdateShowSurface(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
    
    private void UpdateShowMeshGrid(bool value)
    {
        _server.UpdateMeshGridVisibility(value);
    }
    
    private void UpdateShowNormalVector(bool value)
    {
        _server.UpdateNormalVectorVisibility(value);
    }
}