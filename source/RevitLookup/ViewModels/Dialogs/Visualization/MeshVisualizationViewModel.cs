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

public sealed partial class MeshVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<MeshVisualizationViewModel> logger)
    : ObservableObject
{
    private readonly MeshVisualizationServer _server = new();
    
    [ObservableProperty] private double _extrusion = settingsService.RenderSettings.MeshSettings.Extrusion;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.MeshSettings.Transparency;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = settingsService.RenderSettings.MeshSettings.SurfaceColor;
    [ObservableProperty] private System.Windows.Media.Color _meshColor = settingsService.RenderSettings.MeshSettings.MeshColor;
    [ObservableProperty] private System.Windows.Media.Color _normalVectorColor = settingsService.RenderSettings.MeshSettings.NormalVectorColor;
    
    [ObservableProperty] private bool _showSurface = settingsService.RenderSettings.MeshSettings.ShowSurface;
    [ObservableProperty] private bool _showMeshGrid = settingsService.RenderSettings.MeshSettings.ShowMeshGrid;
    [ObservableProperty] private bool _showNormalVector = settingsService.RenderSettings.MeshSettings.ShowNormalVector;
    
    public double MinExtrusion => settingsService.RenderSettings.MeshSettings.MinExtrusion;
    
    public void RegisterServer(Mesh mesh)
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
        _server.Register(mesh);
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
        settingsService.RenderSettings.MeshSettings.SurfaceColor = value;
        UpdateSurfaceColor(value);
    }
    
    partial void OnMeshColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.MeshSettings.MeshColor = value;
        UpdateMeshColor(value);
    }
    
    partial void OnNormalVectorColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.MeshSettings.NormalVectorColor = value;
        UpdateNormalVectorColor(value);
    }
    
    partial void OnExtrusionChanged(double value)
    {
        settingsService.RenderSettings.MeshSettings.Extrusion = value;
        UpdateExtrusion(value);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.MeshSettings.Transparency = value;
        UpdateTransparency(value);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        settingsService.RenderSettings.MeshSettings.ShowSurface = value;
        UpdateShowSurface(value);
    }
    
    partial void OnShowMeshGridChanged(bool value)
    {
        settingsService.RenderSettings.MeshSettings.ShowMeshGrid = value;
        UpdateShowMeshGrid(value);
    }
    
    partial void OnShowNormalVectorChanged(bool value)
    {
        settingsService.RenderSettings.MeshSettings.ShowNormalVector = value;
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