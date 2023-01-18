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
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls;
using RevitLookup.UI.Controls.Window;

namespace RevitLookup.ViewModels.Pages;

public sealed class SettingsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;
    private readonly ISnackbarService _snackbarService;
    private WindowBackdropType _background;
    private ThemeType _theme;
    private bool _isSmoothEnabled;
    private bool _isUnsupportedAllowed;
    private bool _isExtensionsAllowed;

    public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService, ISnackbarService snackbarService)
    {
        _settingsService = settingsService;
        _navigationService = navigationService;
        _snackbarService = snackbarService;
        _theme = settingsService.Theme;
        _background = settingsService.Background;
        _isSmoothEnabled = settingsService.TransitionDuration > 0;
        _isUnsupportedAllowed = settingsService.IsUnsupportedAllowed;
        _isExtensionsAllowed = settingsService.IsExtensionsAllowed;
    }

    public ThemeType Theme
    {
        get => _theme;
        set
        {
            SetProperty(ref _theme, value);
            _settingsService.Theme = value;
            AppearanceData.ApplicationTheme = value;
            // Theme.Apply(value, CurrentBackground); not supported for pages
            _snackbarService.Show("Theme changed", "Changes will take effect for new windows", SymbolRegular.ChatWarning24, ControlAppearance.Success);
        }
    }

    public WindowBackdropType Background
    {
        get => _background;
        set
        {
            SetProperty(ref _background, value);
            _settingsService.Background = value;
            var window = (FluentWindow) UI.Application.Current;
            window.WindowBackdropType = value;
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

    public bool IsUnsupportedAllowed
    {
        get => _isUnsupportedAllowed;
        set
        {
            SetProperty(ref _isUnsupportedAllowed, value);
            _settingsService.IsUnsupportedAllowed = value;
        }
    }

    public bool IsExtensionsAllowed
    {
        get => _isExtensionsAllowed;
        set
        {
            SetProperty(ref _isExtensionsAllowed, value);
            _settingsService.IsExtensionsAllowed = value;
        }
    }

    public List<ThemeType> Themes { get; } = new()
    {
        ThemeType.Light,
        ThemeType.Dark
    };

    public List<WindowBackdropType> BackgroundEffects { get; } = new()
    {
        WindowBackdropType.None,
        WindowBackdropType.Mica
    };
}