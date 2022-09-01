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

using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.UI.Appearance;
using RevitLookup.UI.Mvvm.Contracts;

namespace RevitLookup.UI.Tests.ViewModels.Pages;

public sealed class SettingsViewModel : ObservableObject
{
    private readonly IThemeService _themeService;
    private ThemeType _currentTheme;

    public SettingsViewModel(IThemeService themeService)
    {
        _themeService = themeService;
        CurrentTheme = themeService.GetTheme();
    }

    public List<ThemeType> Themes { get; } = new()
    {
        ThemeType.Auto,
        ThemeType.Dark,
        ThemeType.Light
    };

    public ThemeType CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (value == _currentTheme) return;
            _currentTheme = value;
            _themeService.SetTheme(value);
            OnPropertyChanged();
        }
    }
}