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

using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RevitLookup.Config;
using RevitLookup.Services.Contracts;

namespace RevitLookup.Models.Settings;

public class RenderSettings(string savingPath, IOptions<JsonSerializerOptions> jsonOptions) : ISettings
{
    private RenderConfiguration _settings;
    
    public BoundingBoxVisualizationSettings BoundingBoxSettings => _settings.BoundingBoxSettings;
    public FaceVisualizationSettings FaceSettings => _settings.FaceSettings;
    public MeshVisualizationSettings MeshSettings => _settings.MeshSettings;
    public PolylineVisualizationSettings PolylineSettings => _settings.PolylineSettings;
    public SolidVisualizationSettings SolidSettings => _settings.SolidSettings;
    public XyzVisualizationSettings XyzSettings => _settings.XyzSettings;
    
    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(savingPath)!);
        
        var json = JsonSerializer.Serialize(_settings, jsonOptions.Value);
        File.WriteAllText(savingPath, json);
    }
    
    public void Load()
    {
        if (!File.Exists(savingPath))
        {
            SetDefault();
            return;
        }
        
        try
        {
            using var config = File.OpenRead(savingPath);
            _settings = JsonSerializer.Deserialize<RenderConfiguration>(config, jsonOptions.Value);
        }
        catch
        {
            SetDefault();
            throw;
        }
    }
    
    public void SetDefault()
    {
        _settings = new RenderConfiguration();
    }
}