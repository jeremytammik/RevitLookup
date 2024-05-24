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

using System.Windows.Media;
using Microsoft.Extensions.Logging;
using RevitLookup.Core.Visualization;
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.ViewModels.Dialogs.Visualization;

public sealed partial class XyzVisualizationViewModel(XYZ point, ILogger<XyzVisualizationServer> logger) : ObservableObject
{
    private readonly XyzVisualizationServer _server = new(point, logger);
    
    [ObservableProperty] private double _axisLength = 12;
    [ObservableProperty] private double _transparency = 40;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = Colors.DodgerBlue;
    [ObservableProperty] private System.Windows.Media.Color _axisColor = System.Windows.Media.Color.FromArgb(0, 255, 89, 30);
    
    [ObservableProperty] private bool _showSurface = true;
    
    public double MinAxisLength { get; } = 4;
    
    public void RegisterServer()
    {
        OnShowSurfaceChanged(ShowSurface);
        
        OnSurfaceColorChanged(SurfaceColor);
        OnAxisColorChanged(AxisColor);
        
        OnAxisLengthChanged(AxisLength);
        OnTransparencyChanged(Transparency);
        
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
    
    partial void OnAxisColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateAxisColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnAxisLengthChanged(double value)
    {
        _server.UpdateAxisLength(value / 12);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
}