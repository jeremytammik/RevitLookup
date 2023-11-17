// Copyright 2003-2023 by Autodesk, Inc.
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
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Services;

/// <summary>
///     Settings options saved to disk
/// </summary>
[Serializable]
internal sealed class Settings
{
    public ApplicationTheme Theme { get; set; } = ApplicationTheme.Light;
    public WindowBackdropType Background { get; set; } = WindowBackdropType.None;
    public int TransitionDuration { get; set; } //= SettingsService.DefaultTransitionDuration;
    public bool IsHardwareRenderingAllowed { get; set; } = true;
    public bool IsTimeColumnAllowed { get; set; }
    public bool IsModifyTabAllowed { get; set; }
    public bool IsUnsupportedAllowed { get; set; }
    public bool IsPrivateAllowed { get; set; }
    public bool IsStaticAllowed { get; set; }
    public bool IsFieldsAllowed { get; set; }
    public bool IsEventsAllowed { get; set; }
    public bool IsExtensionsAllowed { get; set; }
}

public sealed class SettingsService : ISettingsService
{
    private const int DefaultTransitionDuration = 200;
    private readonly Settings _settings;
    private readonly string _settingsFile;

    public SettingsService(IConfiguration configuration)
    {
        _settingsFile = configuration.GetValue<string>("ConfigFolder").AppendPath("Settings.cfg");
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

    public bool IsHardwareRenderingAllowed
    {
        get => _settings.IsHardwareRenderingAllowed;
        set => _settings.IsHardwareRenderingAllowed = value;
    }

    public bool IsTimeColumnAllowed
    {
        get => _settings.IsTimeColumnAllowed;
        set => _settings.IsTimeColumnAllowed = value;
    }

    public bool IsModifyTabAllowed
    {
        get => _settings.IsModifyTabAllowed;
        set => _settings.IsModifyTabAllowed = value;
    }

    public bool IsUnsupportedAllowed
    {
        get => _settings.IsUnsupportedAllowed;
        set => _settings.IsUnsupportedAllowed = value;
    }

    public bool IsPrivateAllowed
    {
        get => _settings.IsPrivateAllowed;
        set => _settings.IsPrivateAllowed = value;
    }

    public bool IsStaticAllowed
    {
        get => _settings.IsStaticAllowed;
        set => _settings.IsStaticAllowed = value;
    }

    public bool IsFieldsAllowed
    {
        get => _settings.IsFieldsAllowed;
        set => _settings.IsFieldsAllowed = value;
    }

    public bool IsEventsAllowed
    {
        get => _settings.IsEventsAllowed;
        set => _settings.IsEventsAllowed = value;
    }

    public bool IsExtensionsAllowed
    {
        get => _settings.IsExtensionsAllowed;
        set => _settings.IsExtensionsAllowed = value;
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
            using var config = File.OpenRead(_settingsFile);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            return JsonSerializer.Deserialize<Settings>(config, jsonSerializerOptions);
        }
        catch
        {
            return new Settings();
        }
    }
}