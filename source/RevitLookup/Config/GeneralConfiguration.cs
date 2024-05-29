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
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Config;

/// <summary>
///     Settings options saved on disk
/// </summary>
[Serializable]
public sealed class GeneralConfiguration
{
    public int DefaultTransitionDuration => 200;
    
    [JsonPropertyName("Theme")] public ApplicationTheme Theme { get; set; } = ApplicationTheme.Light;
    [JsonPropertyName("Background")] public WindowBackdropType Background { get; set; } = WindowBackdropType.None;
    
    [JsonPropertyName("TransitionDuration")]
    public int TransitionDuration { get; set; } //= SettingsService.DefaultTransitionDuration;
    
    [JsonPropertyName("IsHardwareRenderingAllowed")]
    public bool UseHardwareRendering { get; set; } = true;
    
    [JsonPropertyName("IsTimeColumnAllowed")]
    public bool ShowTimeColumn { get; set; }
    
    [JsonPropertyName("ShowMemoryColumn")] public bool ShowMemoryColumn { get; set; }
    [JsonPropertyName("UseSizeRestoring")] public bool UseSizeRestoring { get; set; }
    [JsonPropertyName("WindowWidth")] public double WindowWidth { get; set; }
    [JsonPropertyName("WindowHeight")] public double WindowHeight { get; set; }
    
    [JsonPropertyName("IsUnsupportedAllowed")]
    public bool IncludeUnsupported { get; set; }
    
    [JsonPropertyName("IsPrivateAllowed")] public bool IncludePrivate { get; set; }
    [JsonPropertyName("IsStaticAllowed")] public bool IncludeStatic { get; set; }
    [JsonPropertyName("IsFieldsAllowed")] public bool IncludeFields { get; set; }
    [JsonPropertyName("IsEventsAllowed")] public bool IncludeEvents { get; set; }
    
    [JsonPropertyName("IsExtensionsAllowed")]
    public bool IncludeExtensions { get; set; }
    
    [JsonPropertyName("IsRootHierarchyAllowed")]
    public bool IncludeRootHierarchy { get; set; }
    
    [JsonPropertyName("IsModifyTabAllowed")]
    public bool UseModifyTab { get; set; }
}