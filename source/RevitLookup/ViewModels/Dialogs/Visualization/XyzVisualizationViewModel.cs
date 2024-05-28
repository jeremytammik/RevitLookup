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
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.ViewModels.Dialogs.Visualization;

public sealed partial class XyzVisualizationViewModel(XYZ point, ILogger<XyzVisualizationServer> logger) : ObservableObject
{
    private readonly XyzVisualizationServer _server = new(point, logger);
    
    [ObservableProperty] private double _axisLength = 6;
    [ObservableProperty] private double _transparency;
    
    [ObservableProperty] private System.Windows.Media.Color _xColor = System.Windows.Media.Color.FromArgb(255, 30, 227, 255);
    [ObservableProperty] private System.Windows.Media.Color _yColor = System.Windows.Media.Color.FromArgb(255, 30, 144, 255);
    [ObservableProperty] private System.Windows.Media.Color _zColor = System.Windows.Media.Color.FromArgb(255, 30, 81, 255);
    
    [ObservableProperty] private bool _showPlane = true;
    [ObservableProperty] private bool _showXAxis = true;
    [ObservableProperty] private bool _showYAxis = true;
    [ObservableProperty] private bool _showZAxis = true;
    
    public double MinAxisLength => 0.1;
    
    public void RegisterServer()
    {
        OnShowPlaneChanged(ShowPlane);
        OnShowXAxisChanged(ShowPlane);
        OnShowYAxisChanged(ShowPlane);
        OnShowZAxisChanged(ShowPlane);
        
        OnXColorChanged(XColor);
        OnYColorChanged(YColor);
        OnZColorChanged(ZColor);
        
        OnAxisLengthChanged(AxisLength);
        OnTransparencyChanged(Transparency);
        
        _server.Register();
    }
    
    public void UnregisterServer()
    {
        _server.Unregister();
    }
    
    partial void OnXColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateXColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnYColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateYColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnZColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateZColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnAxisLengthChanged(double value)
    {
        _server.UpdateAxisLength(value / 12);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    partial void OnShowPlaneChanged(bool value)
    {
        _server.UpdatePlaneVisibility(value);
    }
    
    partial void OnShowXAxisChanged(bool value)
    {
        _server.UpdateXAxisVisibility(value);
    }
    
    partial void OnShowYAxisChanged(bool value)
    {
        _server.UpdateYAxisVisibility(value);
    }
    
    partial void OnShowZAxisChanged(bool value)
    {
        _server.UpdateZAxisVisibility(value);
    }
}