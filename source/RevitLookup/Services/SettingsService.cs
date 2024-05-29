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

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevitLookup.Models.Options;
using RevitLookup.Models.Settings;
using RevitLookup.Services.Contracts;

namespace RevitLookup.Services;

public sealed class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    
    public SettingsService(IOptions<FolderLocations> foldersOptions, IOptions<JsonSerializerOptions> jsonOptions, ILogger<SettingsService> logger)
    {
        _logger = logger;
        GeneralSettings = new GeneralSettings(foldersOptions.Value.GeneralSettingsPath, jsonOptions);
        RenderSettings = new RenderSettings(foldersOptions.Value.RenderSettingsPath, jsonOptions);
        
        LoadSettings();
    }
    
    public GeneralSettings GeneralSettings { get; }
    public RenderSettings RenderSettings { get; }
    
    public void SaveSettings()
    {
        try
        {
            GeneralSettings.Save();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "General settings deserializing error");
        }
        
        try
        {
            RenderSettings.Save();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Render settings deserializing error");
        }
    }
    
    public void LoadSettings()
    {
        try
        {
            GeneralSettings.Load();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "General settings serializing error");
        }
        
        try
        {
            RenderSettings.Load();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Render settings serializing error");
        }
    }
}