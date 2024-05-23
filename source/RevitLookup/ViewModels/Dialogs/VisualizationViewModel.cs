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
using RevitLookup.Core.Servers;
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.ViewModels.Dialogs;

public sealed partial class VisualizationViewModel(Face face, ILogger<FaceVisualizationServer> logger) : ObservableObject
{
    private readonly FaceVisualizationServer _server = new(face, logger);
    
    [ObservableProperty] private double _thickness = Context.Application.VertexTolerance * 12;
    [ObservableProperty] private double _transparency;
    [ObservableProperty] private bool _showSurface = true;
    [ObservableProperty] private bool _showMeshGrid = true;
    [ObservableProperty] private System.Windows.Media.Color _surfaceColor = Colors.DodgerBlue;
    [ObservableProperty] private System.Windows.Media.Color _meshColor = Colors.Black;
    
    public double MinThickness { get; } = Context.Application.VertexTolerance * 12;
    
    public void RegisterServer()
    {
        _server.UpdateSurfaceColor(new Color(SurfaceColor.R, SurfaceColor.G, SurfaceColor.B));
        _server.UpdateMeshColor(new Color(MeshColor.R, MeshColor.G, MeshColor.B));
        _server.UpdateThickness(Thickness / 12);
        _server.UpdateTransparency(Transparency / 100 * 255);
        _server.UpdateMeshGridVisibility(ShowMeshGrid);
        _server.UpdateSurfaceVisibility(ShowSurface);
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
    
    partial void OnMeshColorChanged(System.Windows.Media.Color value)
    {
        _server.UpdateMeshColor(new Color(value.R, value.G, value.B));
    }
    
    partial void OnThicknessChanged(double value)
    {
        _server.UpdateThickness(value / 12);
    }
    
    partial void OnTransparencyChanged(double value)
    {
        _server.UpdateTransparency(value / 100 * 255);
    }
    
    partial void OnShowSurfaceChanged(bool value)
    {
        _server.UpdateSurfaceVisibility(value);
    }
    
    partial void OnShowMeshGridChanged(bool value)
    {
        _server.UpdateMeshGridVisibility(value);
    }
}