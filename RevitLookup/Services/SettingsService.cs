// Copyright 2003-2022 by Autodesk, Inc.
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
using Microsoft.Extensions.Configuration;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Appearance;
using RevitLookup.UI.Controls.Window;

namespace RevitLookup.Services;

public sealed class SettingsService : ISettingsService
{
    public const int DefaultTransitionDuration = 300;
    private readonly Settings _settings;
    private readonly string _settingsFile;

    public SettingsService(IConfiguration configuration)
    {
        _settingsFile = configuration.GetValue<string>("ConfigFolder").AppendPath("Settings.cfg");
        _settings = LoadSettings();
    }

    public ThemeType Theme
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

    public int ApplyTransition(bool value)
    {
        return TransitionDuration = value ? DefaultTransitionDuration : 0;
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsFile)!);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        var json = JsonSerializer.Serialize(_settings, jsonSerializerOptions);

        File.WriteAllText(_settingsFile, json);
    }

    private Settings LoadSettings()
    {
        if (!File.Exists(_settingsFile)) return new Settings();

        try
        {
            var config = File.ReadAllText(_settingsFile);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            var json = JsonSerializer.Deserialize<Settings>(config, jsonSerializerOptions);
            return json;
        }
        catch
        {
            return new Settings();
        }
    }
}

internal sealed class Settings
{
    public ThemeType Theme { get; set; } = ThemeType.Light;
    public WindowBackdropType Background { get; set; } = WindowBackdropType.Mica;
    public int TransitionDuration { get; set; } = SettingsService.DefaultTransitionDuration;
}