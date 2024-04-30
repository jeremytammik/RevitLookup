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
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevitLookup.Models.Options;
using RevitLookup.Services.Contracts;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Services;

/// <summary>
///     Settings options saved to disk
/// </summary>
[Serializable]
internal sealed class Settings
{
    [JsonPropertyName("Theme")] public ApplicationTheme Theme { get; set; } = ApplicationTheme.Light;
    [JsonPropertyName("Background")] public WindowBackdropType Background { get; set; } = WindowBackdropType.None;
    [JsonPropertyName("TransitionDuration")] public int TransitionDuration { get; set; } //= SettingsService.DefaultTransitionDuration;
    [JsonPropertyName("IsHardwareRenderingAllowed")] public bool UseHardwareRendering { get; set; } = true;
    [JsonPropertyName("IsTimeColumnAllowed")] public bool ShowTimeColumn { get; set; }
    [JsonPropertyName("UseSizeRestoring")] public bool UseSizeRestoring { get; set; }
    [JsonPropertyName("WindowWidth")] public double WindowWidth { get; set; }
    [JsonPropertyName("WindowHeight")] public double WindowHeight { get; set; }
    [JsonPropertyName("IsUnsupportedAllowed")] public bool IncludeUnsupported { get; set; }
    [JsonPropertyName("IsPrivateAllowed")] public bool IncludePrivate { get; set; }
    [JsonPropertyName("IsStaticAllowed")] public bool IncludeStatic { get; set; }
    [JsonPropertyName("IsFieldsAllowed")] public bool IncludeFields { get; set; }
    [JsonPropertyName("IsEventsAllowed")] public bool IncludeEvents { get; set; }
    [JsonPropertyName("IsExtensionsAllowed")] public bool IncludeExtensions { get; set; }
    [JsonPropertyName("IsRootHierarchyAllowed")] public bool IncludeRootHierarchy { get; set; }
    [JsonPropertyName("IsModifyTabAllowed")] public bool UseModifyTab { get; set; }
}

public sealed class SettingsService : ISettingsService
{
    private const int DefaultTransitionDuration = 200;

    private readonly FolderLocations _folderLocations;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ILogger<SettingsService> _logger;
    private readonly Settings _settings;
    
    public SettingsService(IOptions<FolderLocations> foldersOptions, IOptions<JsonSerializerOptions> jsonOptions, ILogger<SettingsService> logger)
    {
        _folderLocations = foldersOptions.Value;
        _jsonSerializerOptions = jsonOptions.Value;
        _logger = logger;
        _settings = LoadSettings();
    }
    
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
        return TransitionDuration = value ? DefaultTransitionDuration : 0;
    }
    
    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_folderLocations.SettingsPath)!);
        
        try
        {
            var json = JsonSerializer.Serialize(_settings, _jsonSerializerOptions);
            File.WriteAllText(_folderLocations.SettingsPath, json);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Settings serializing error");
        }
    }
    
    private Settings LoadSettings()
    {
        if (!File.Exists(_folderLocations.SettingsPath)) return new Settings();
        
        try
        {
            using var config = File.OpenRead(_folderLocations.SettingsPath);
            return JsonSerializer.Deserialize<Settings>(config, _jsonSerializerOptions);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Settings deserializing error");
        }
        
        return new Settings();
    }
}