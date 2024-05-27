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

public sealed partial class SolidVisualizationViewModel(Solid solid, ILogger<SolidVisualizationServer> logger) : ObservableObject
{
    private readonly SolidVisualizationServer _server = new(solid, logger);
    
    [ObservableProperty] private double _transparency = 20;
    [ObservableProperty] private double _cageTransparency = 90;
    [ObservableProperty] private double _cageSize = 1;
    
    [ObservableProperty] private System.Windows.Media.Color _faceColor = Colors.DodgerBlue;
    [ObservableProperty] private System.Windows.Media.Color _edgeColor = System.Windows.Media.Color.FromArgb(0, 30, 81, 255);
    [ObservableProperty] private System.Windows.Media.Color _cageSurfaceColor = System.Windows.Media.Color.FromArgb(0, 175, 175, 175);
    [ObservableProperty] private System.Windows.Media.Color _cageFrameColor = Colors.Black;
    
    [ObservableProperty] private bool _showFace = true;
    [ObservableProperty] private bool _showEdge = true;
    [ObservableProperty] private bool _showCageSurface = true;
    
    public void RegisterServer()
    {
        OnShowFaceChanged(ShowFace);
        OnShowEdgeChanged(ShowEdge);
        OnShowCageSurfaceChanged(ShowCageSurface);
        
        OnFaceColorChanged(FaceColor);
        OnEdgeColorChanged(EdgeColor);
        OnCageSurfaceColorChanged(CageSurfaceColor);
        OnCageFrameColorChanged(CageFrameColor);
        
        OnTransparencyChanged(Transparency);
        OnCageTransparencyChanged(CageTransparency);
        OnCageSizeChanged(CageSize);
        
        _server.Register();
    }
    
    public void UnregisterServer()
    {
        _server.Unregister();
    }
    
    partial void OnFaceColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateFaceColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnEdgeColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateEdgeColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnCageSurfaceColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateCageSurfaceColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnCageFrameColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateCageFrameColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnTransparencyChanged(double value)
    {
        _server.UpdateTransparency(value / 100);
    }
    
    partial void OnCageTransparencyChanged(double value)
    {
        _server.UpdateCageTransparency(value / 100);
    }
    
    partial void OnCageSizeChanged(double value)
    {
        _server.UpdateCageSize(value / 12);
    }
    
    partial void OnShowFaceChanged(bool value)
    {
        _server.UpdateFaceVisibility(value);
    }
    
    partial void OnShowEdgeChanged(bool value)
    {
        _server.UpdateEdgeVisibility(value);
    }
    
    partial void OnShowCageSurfaceChanged(bool value)
    {
        _server.UpdateCageSurfaceVisibility(value);
    }
}