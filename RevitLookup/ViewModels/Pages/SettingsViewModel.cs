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
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Appearance;
using RevitLookup.UI.Common;
using RevitLookup.UI.Mvvm.Contracts;

namespace RevitLookup.ViewModels.Pages;

public sealed class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly INavigationService _navigationService;
    private readonly ISnackbarService _snackbarService;
    private ThemeType _currentTheme;
    private bool _isSmoothEnabled;
    private BackgroundType _currentBackground;

    public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService, ISnackbarService snackbarService)
    {
        _settingsService = settingsService;
        _navigationService = navigationService;
        _snackbarService = snackbarService;
        _currentTheme = settingsService.Theme;
        _currentBackground = settingsService.Background;
        _isSmoothEnabled = settingsService.TransitionDuration > 0;
    }

    public ThemeType CurrentTheme
    {
        get => _currentTheme;
        set
        {
            SetProperty(ref _currentTheme, value);
            _settingsService.Theme = value;
            Theme.Apply(value, CurrentBackground);
            _snackbarService.Show("Theme changed", "The theme will be applied after the window is reopened", SymbolRegular.ChatWarning24, ControlAppearance.Success);
        }
    }

    public BackgroundType CurrentBackground
    {
        get => _currentBackground;
        set
        {
            SetProperty(ref _currentBackground, value);
            _settingsService.Background = value;
            Theme.UpdateBackground(CurrentTheme, value);
            _snackbarService.Show("Background effect changed", "The theme will be applied after the window is reopened", SymbolRegular.ChatWarning24, ControlAppearance.Success);
        }
    }

    public bool IsSmoothEnabled
    {
        get => _isSmoothEnabled;
        set
        {
            SetProperty(ref _isSmoothEnabled, value);
            var transitionDuration = _settingsService.ApplyTransition(value);
            _navigationService.GetNavigationControl().TransitionDuration = transitionDuration;
        }
    }

    public List<ThemeType> Themes { get; } = new()
    {
        ThemeType.Light,
        ThemeType.Dark
    };

    public List<BackgroundType> BackgroundEffects { get; } = new()
    {
        BackgroundType.None,
        BackgroundType.Mica
    };
}