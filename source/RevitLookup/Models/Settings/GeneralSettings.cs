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
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Models.Settings;

public class GeneralSettings(string path, IOptions<JsonSerializerOptions> jsonOptions) : ISettings
{
    private GeneralConfiguration _settings;
    
    public ApplicationTheme Theme
    {
        get => _settings.Theme;
        set => _settings.Theme = value;
    }
    
    public WindowBackdropType Background
    {
        get => _settings.Background;
        set => _settings.Background = value;
    }
    
    public int TransitionDuration
    {
        get => _settings.TransitionDuration;
        private set => _settings.TransitionDuration = value;
    }
    
    public bool UseHardwareRendering
    {
        get => _settings.UseHardwareRendering;
        set => _settings.UseHardwareRendering = value;
    }
    
    public bool ShowTimeColumn
    {
        get => _settings.ShowTimeColumn;
        set => _settings.ShowTimeColumn = value;
    }
    
    public bool ShowMemoryColumn
    {
        get => _settings.ShowMemoryColumn;
        set => _settings.ShowMemoryColumn = value;
    }
    
    public bool UseModifyTab
    {
        get => _settings.UseModifyTab;
        set => _settings.UseModifyTab = value;
    }
    
    public bool UseSizeRestoring
    {
        get => _settings.UseSizeRestoring;
        set => _settings.UseSizeRestoring = value;
    }
    
    public double WindowWidth
    {
        get => _settings.WindowWidth;
        set => _settings.WindowWidth = value;
    }
    
    public double WindowHeight
    {
        get => _settings.WindowHeight;
        set => _settings.WindowHeight = value;
    }
    
    public bool IncludeUnsupported
    {
        get => _settings.IncludeUnsupported;
        set => _settings.IncludeUnsupported = value;
    }
    
    public bool IncludePrivate
    {
        get => _settings.IncludePrivate;
        set => _settings.IncludePrivate = value;
    }
    
    public bool IncludeStatic
    {
        get => _settings.IncludeStatic;
        set => _settings.IncludeStatic = value;
    }
    
    public bool IncludeFields
    {
        get => _settings.IncludeFields;
        set => _settings.IncludeFields = value;
    }
    
    public bool IncludeEvents
    {
        get => _settings.IncludeEvents;
        set => _settings.IncludeEvents = value;
    }
    
    public bool IncludeExtensions
    {
        get => _settings.IncludeExtensions;
        set => _settings.IncludeExtensions = value;
    }
    
    public bool IncludeRootHierarchy
    {
        get => _settings.IncludeRootHierarchy;
        set => _settings.IncludeRootHierarchy = value;
    }
    
    public int ApplyTransition(bool value)
    {
        return TransitionDuration = value ? _settings.DefaultTransitionDuration : 0;
    }
    
    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        
        var json = JsonSerializer.Serialize(_settings, jsonOptions.Value);
        File.WriteAllText(path, json);
    }
    
    public void Load()
    {
        if (!File.Exists(path))
        {
            SetDefault();
            return;
        }
        
        try
        {
            using var config = File.OpenRead(path);
            _settings = JsonSerializer.Deserialize<GeneralConfiguration>(config, jsonOptions.Value);
        }
        catch
        {
            SetDefault();
        }
    }
    
    public void SetDefault()
    {
        _settings = new GeneralConfiguration();
    }
}