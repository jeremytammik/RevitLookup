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

using System.Text.Json.Serialization;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace RevitLookup.Config;

/// <summary>
///     Settings options saved on disk
/// </summary>
[Serializable]
public sealed class RenderConfiguration
{
    [JsonPropertyName("BoundingBoxSettings")] public BoundingBoxVisualizationSettings BoundingBoxSettings { get; set; } = new();
    [JsonPropertyName("FaceSettings")] public FaceVisualizationSettings FaceSettings { get; set; } = new();
    [JsonPropertyName("MeshSettings")] public MeshVisualizationSettings MeshSettings { get; set; } = new();
    [JsonPropertyName("PolylineSettings")] public PolylineVisualizationSettings PolylineSettings { get; set; } = new();
    [JsonPropertyName("SolidSettings")] public SolidVisualizationSettings SolidSettings { get; set; } = new();
    [JsonPropertyName("XyzSettings")] public XyzVisualizationSettings XyzSettings { get; set; } = new();
}

[Serializable]
public class BoundingBoxVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; } = 60;

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; } = Colors.DodgerBlue;
    [JsonPropertyName("EdgeColor")] public Color EdgeColor { get; set; } = Color.FromArgb(255, 30, 81, 255);
    [JsonPropertyName("AxisColor")] public Color AxisColor { get; set; } = Color.FromArgb(255, 255, 89, 30);

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; } = true;
    [JsonPropertyName("ShowEdge")] public bool ShowEdge { get; set; } = true;
    [JsonPropertyName("ShowAxis")] public bool ShowAxis { get; set; } = true;
}

[Serializable]
public class FaceVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; } = 20;
    [JsonPropertyName("Extrusion")] public double Extrusion { get; set; } = Context.Application.VertexTolerance * 12;
    [JsonPropertyName("MinExtrusion")] public double MinExtrusion { get; set; } = Context.Application.VertexTolerance * 12;

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; } = Colors.DodgerBlue;
    [JsonPropertyName("MeshColor")] public Color MeshColor { get; set; } = Color.FromArgb(255, 30, 81, 255);
    [JsonPropertyName("NormalVectorColor")] public Color NormalVectorColor { get; set; } = Color.FromArgb(255, 255, 89, 30);

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; } = true;
    [JsonPropertyName("ShowMeshGrid")] public bool ShowMeshGrid { get; set; } = true;
    [JsonPropertyName("ShowNormalVector")] public bool ShowNormalVector { get; set; } = true;
}

[Serializable]
public class MeshVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; } = 20;
    [JsonPropertyName("Extrusion")] public double Extrusion { get; set; } = Context.Application.VertexTolerance * 12;
    [JsonPropertyName("MinExtrusion")] public double MinExtrusion { get; set; } = Context.Application.VertexTolerance * 12;

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; } = Colors.DodgerBlue;
    [JsonPropertyName("MeshColor")] public Color MeshColor { get; set; } = Color.FromArgb(255, 30, 81, 255);
    [JsonPropertyName("NormalVectorColor")] public Color NormalVectorColor { get; set; } = Color.FromArgb(255, 255, 89, 30);

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; } = true;
    [JsonPropertyName("ShowMeshGrid")] public bool ShowMeshGrid { get; set; } = true;
    [JsonPropertyName("ShowNormalVector")] public bool ShowNormalVector { get; set; } = true;
}

[Serializable]
public class PolylineVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; } = 20;
    [JsonPropertyName("Diameter")] public double Diameter { get; set; } = 2;
    [JsonPropertyName("MinThickness")] public double MinThickness { get; set; } = 0.1;

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; } = Colors.DodgerBlue;
    [JsonPropertyName("CurveColor")] public Color CurveColor { get; set; } = Color.FromArgb(255, 30, 81, 255);
    [JsonPropertyName("DirectionColor")] public Color DirectionColor { get; set; } = Color.FromArgb(255, 255, 89, 30);

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; } = true;
    [JsonPropertyName("ShowCurve")] public bool ShowCurve { get; set; } = true;
    [JsonPropertyName("ShowDirection")] public bool ShowDirection { get; set; } = true;
}

public class SolidVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; } = 20;
    [JsonPropertyName("Scale")] public double Scale { get; set; } = 1;

    [JsonPropertyName("FaceColor")] public Color FaceColor { get; set; } = Colors.DodgerBlue;
    [JsonPropertyName("EdgeColor")] public Color EdgeColor { get; set; } = Color.FromArgb(255, 30, 81, 255);

    [JsonPropertyName("ShowFace")] public bool ShowFace { get; set; } = true;
    [JsonPropertyName("ShowEdge")] public bool ShowEdge { get; set; } = true;
}

[Serializable]
public class XyzVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; } = 0;
    [JsonPropertyName("AxisLength")] public double AxisLength { get; set; } = 6;
    [JsonPropertyName("MinAxisLength")] public double MinAxisLength { get; set; } = 0.1;

    [JsonPropertyName("XColor")] public Color XColor { get; set; } = Color.FromArgb(255, 30, 227, 255);
    [JsonPropertyName("YColor")] public Color YColor { get; set; } = Color.FromArgb(255, 30, 144, 255);
    [JsonPropertyName("ZColor")] public Color ZColor { get; set; } = Color.FromArgb(255, 30, 81, 255);

    [JsonPropertyName("ShowPlane")] public bool ShowPlane { get; set; } = true;
    [JsonPropertyName("ShowXAxis")] public bool ShowXAxis { get; set; } = true;
    [JsonPropertyName("ShowYAxis")] public bool ShowYAxis { get; set; } = true;
    [JsonPropertyName("ShowZAxis")] public bool ShowZAxis { get; set; } = true;
}