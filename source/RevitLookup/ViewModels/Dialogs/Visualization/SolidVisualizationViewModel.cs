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

public sealed partial class SolidVisualizationViewModel(
    NotificationService notificationService,
    ISettingsService settingsService,
    ILogger<SolidVisualizationViewModel> logger)
    : ObservableObject
{
    private readonly SolidVisualizationServer _server = new();

    [ObservableProperty] private double _scale = settingsService.RenderSettings.SolidSettings.Scale;
    [ObservableProperty] private double _transparency = settingsService.RenderSettings.SolidSettings.Transparency;

    [ObservableProperty] private System.Windows.Media.Color _faceColor = settingsService.RenderSettings.SolidSettings.FaceColor;
    [ObservableProperty] private System.Windows.Media.Color _edgeColor = settingsService.RenderSettings.SolidSettings.EdgeColor;

    [ObservableProperty] private bool _showFace = settingsService.RenderSettings.SolidSettings.ShowFace;
    [ObservableProperty] private bool _showEdge = settingsService.RenderSettings.SolidSettings.ShowEdge;

    public void RegisterServer(Solid solid)
    {
        UpdateShowFace(ShowFace);
        UpdateShowEdge(ShowEdge);

        UpdateFaceColor(FaceColor);
        UpdateEdgeColor(EdgeColor);

        UpdateTransparency(Transparency);
        UpdateScale(Scale);

        _server.RenderFailed += HandleRenderFailure;
        _server.Register(solid);
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

    partial void OnFaceColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.SolidSettings.FaceColor = value;
        UpdateFaceColor(value);
    }

    partial void OnEdgeColorChanged(System.Windows.Media.Color value)
    {
        settingsService.RenderSettings.SolidSettings.EdgeColor = value;
        UpdateEdgeColor(value);
    }

    partial void OnTransparencyChanged(double value)
    {
        settingsService.RenderSettings.SolidSettings.Transparency = value;
        UpdateTransparency(value);
    }

    partial void OnScaleChanged(double value)
    {
        settingsService.RenderSettings.SolidSettings.Scale = value;
        UpdateScale(value);
    }

    partial void OnShowFaceChanged(bool value)
    {
        settingsService.RenderSettings.SolidSettings.ShowFace = value;
        UpdateShowFace(value);
    }

    partial void OnShowEdgeChanged(bool value)
    {
        settingsService.RenderSettings.SolidSettings.ShowEdge = value;
        UpdateShowEdge(value);
    }

    private void UpdateFaceColor(System.Windows.Media.Color value)
    {
        _server.UpdateFaceColor(new Color(value.R, value.G, value.B));
    }

    private void UpdateEdgeColor(System.Windows.Media.Color value)
    {
        _server.UpdateEdgeColor(new Color(value.R, value.G, value.B));
    }

    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }

    private void UpdateScale(double value)
    {
        _server.UpdateScale(value / 100);
    }

    private void UpdateShowFace(bool value)
    {
        _server.UpdateFaceVisibility(value);
    }

    private void UpdateShowEdge(bool value)
    {
        _server.UpdateEdgeVisibility(value);
    }
}