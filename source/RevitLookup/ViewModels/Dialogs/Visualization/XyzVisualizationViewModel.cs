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

public sealed partial class XyzVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<XyzVisualizationViewModel> logger)
    : ObservableObject
{
    private readonly XyzVisualizationServer _server = new();
    
    [ObservableProperty] private double _axisLength = settingsService.RenderSettings.XyzSettings.AxisLength;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.XyzSettings.Transparency;
    
    [ObservableProperty] private System.Windows.Media.Color _xColor = settingsService.RenderSettings.XyzSettings.XColor;
    [ObservableProperty] private System.Windows.Media.Color _yColor = settingsService.RenderSettings.XyzSettings.YColor;
    [ObservableProperty] private System.Windows.Media.Color _zColor = settingsService.RenderSettings.XyzSettings.ZColor;
    
    [ObservableProperty] private bool _showPlane = settingsService.RenderSettings.XyzSettings.ShowPlane;
    [ObservableProperty] private bool _showXAxis = settingsService.RenderSettings.XyzSettings.ShowXAxis;
    [ObservableProperty] private bool _showYAxis = settingsService.RenderSettings.XyzSettings.ShowYAxis;
    [ObservableProperty] private bool _showZAxis = settingsService.RenderSettings.XyzSettings.ShowZAxis;
    
    public double MinAxisLength => settingsService.RenderSettings.XyzSettings.MinAxisLength;
    
    public void RegisterServer(XYZ point)
    {
        UpdateShowPlane(ShowPlane);
        UpdateShowXAxis(ShowXAxis);
        UpdateShowYAxis(ShowYAxis);
        UpdateShowZAxis(ShowZAxis);
        
        UpdateXColor(XColor);
        UpdateYColor(YColor);
        UpdateZColor(ZColor);
        
        UpdateAxisLength(AxisLength);
        UpdateTransparency(Transparency);
        
        _server.RenderFailed += HandleRenderFailure;
        _server.Register(point);
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
    
    partial void OnXColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.XyzSettings.XColor = value;
        UpdateXColor(value);
    }
    
    partial void OnYColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.XyzSettings.YColor = value;
        UpdateYColor(value);
    }
    
    partial void OnZColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.XyzSettings.ZColor = value;
        UpdateZColor(value);
    }
    
    partial void OnAxisLengthChanged(double value)
    {
        settingsService.RenderSettings.XyzSettings.AxisLength = value;
        UpdateAxisLength(value);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.XyzSettings.Transparency = value;
        UpdateTransparency(value);
    }
    
    partial void OnShowPlaneChanged(bool value)
    {
        settingsService.RenderSettings.XyzSettings.ShowPlane = value;
        UpdateShowPlane(value);
    }
    
    partial void OnShowXAxisChanged(bool value)
    {
        settingsService.RenderSettings.XyzSettings.ShowXAxis = value;
        UpdateShowXAxis(value);
    }
    
    partial void OnShowYAxisChanged(bool value)
    {
        settingsService.RenderSettings.XyzSettings.ShowYAxis = value;
        UpdateShowYAxis(value);
    }
    
    partial void OnShowZAxisChanged(bool value)
    {
        settingsService.RenderSettings.XyzSettings.ShowZAxis = value;
        UpdateShowZAxis(value);
    }
    
    private void UpdateXColor(System.Windows.Media.Color value)
    {
        _server.UpdateXColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateYColor(System.Windows.Media.Color value)
    {
        _server.UpdateYColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateZColor(System.Windows.Media.Color value)
    {
        _server.UpdateZColor(new Color(value.R, value.G, value.B));
    }
    
    private void UpdateAxisLength(double value)
    {
        _server.UpdateAxisLength(value / 12);
    }
    
    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    private void UpdateShowPlane(bool value)
    {
        _server.UpdatePlaneVisibility(value);
    }
    
    private void UpdateShowXAxis(bool value)
    {
        _server.UpdateXAxisVisibility(value);
    }
    
    private void UpdateShowYAxis(bool value)
    {
        _server.UpdateYAxisVisibility(value);
    }
    
    private void UpdateShowZAxis(bool value)
    {
        _server.UpdateZAxisVisibility(value);
    }
}