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

using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Visualization;
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.ViewModels.Dialogs.Visualization;

[SuppressMessage("ReSharper", "ContextualLoggerProblem")]
public sealed partial class PolylineVisualizationViewModel : ObservableObject
{
    private readonly PolylineVisualizationServer _server;
    
    [ObservableProperty] private double _diameter = 2;
    [ObservableProperty] private double _transparency = 20;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = Colors.DodgerBlue;
    [ObservableProperty] private System.Windows.Media.Color _curveColor = System.Windows.Media.Color.FromArgb(255, 30, 81, 255);
    [ObservableProperty] private System.Windows.Media.Color _directionColor = System.Windows.Media.Color.FromArgb(255, 255, 89, 30);
    
    [ObservableProperty] private bool _showSurface = true;
    [ObservableProperty] private bool _showCurve = true;
    [ObservableProperty] private bool _showDirection = true;
    
    
    public PolylineVisualizationViewModel(Edge edge, ILogger<PolylineVisualizationServer> logger)
    {
        _server = new PolylineVisualizationServer(edge, logger);
    }
    
    public PolylineVisualizationViewModel(Curve curve, ILogger<PolylineVisualizationServer> logger)
    {
        _server = new PolylineVisualizationServer(curve, logger);
    }
    
    public double MinThickness => 0.1;
    
    public void RegisterServer()
    {
        OnShowSurfaceChanged(ShowSurface);
        OnShowCurveChanged(ShowCurve);
        OnShowDirectionChanged(ShowDirection);
        
        OnSurfaceColorChanged(SurfaceColor);
        OnCurveColorChanged(CurveColor);
        OnDirectionColorChanged(DirectionColor);
        
        OnTransparencyChanged(Transparency);
        OnDiameterChanged(Diameter);
        
        _server.Register();
    }
    
    public void UnregisterServer()
    {
        _server.Unregister();
    }
    
    partial void OnSurfaceColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateSurfaceColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnCurveColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateCurveColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnDirectionColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateDirectionColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnDiameterChanged(double value)
    {
        _server.UpdateDiameter(value / 12);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
    
    partial void OnShowCurveChanged(bool value)
    {
        _server.UpdateCurveVisibility(value);
    }
    
    partial void OnShowDirectionChanged(bool value)
    {
        _server.UpdateDirectionVisibility(value);
    }
}