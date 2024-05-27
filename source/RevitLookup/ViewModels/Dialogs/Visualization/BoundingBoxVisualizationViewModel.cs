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

public sealed partial class BoundingBoxVisualizationViewModel(BoundingBoxXYZ box, ILogger<BoundingBoxVisualizationServer> logger) : ObservableObject
{
    private readonly BoundingBoxVisualizationServer _server = new(box, logger);
    
    [ObservableProperty] private double _transparency = 60;
    
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = Colors.DodgerBlue;
    [ObservableProperty] private System.Windows.Media.Color _edgeColor = System.Windows.Media.Color.FromArgb(0, 30, 81, 255);
    [ObservableProperty] private System.Windows.Media.Color _axisColor = System.Windows.Media.Color.FromArgb(0, 255, 89, 30);
    
    [ObservableProperty] private bool _showSurface = true;
    [ObservableProperty] private bool _showEdge = true;
    [ObservableProperty] private bool _showAxis = true;
    
    public void RegisterServer()
    {
        OnShowSurfaceChanged(ShowSurface);
        OnShowEdgeChanged(ShowEdge);
        OnShowAxisChanged(ShowAxis);
        
        OnSurfaceColorChanged(SurfaceColor);
        OnEdgeColorChanged(EdgeColor);
        OnAxisColorChanged(AxisColor);
        
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
    
    partial void OnEdgeColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateEdgeColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnAxisColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateAxisColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnTransparencyChanged(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
    
    partial void OnShowEdgeChanged(bool value)
    {
        _server.UpdateEdgeVisibility(value);
    }
    
    partial void OnShowAxisChanged(bool value)
    {
        _server.UpdateAxisVisibility(value);
    }
}