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

using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.Services.Contracts;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Window;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;
    private readonly ISnackbarService _snackbarService;
    [ObservableProperty] private WindowBackdropType _background;
    [ObservableProperty] private bool _isExtensionsAllowed;
    [ObservableProperty] private bool _isSmoothEnabled;
    [ObservableProperty] private bool _isUnsupportedAllowed;
    [ObservableProperty] private bool _isModifyPanelVisible;
    [ObservableProperty] private ThemeType _theme;

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
        _isModifyPanelVisible = settingsService.IsModifyPanelVisible;
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

    partial void OnThemeChanged(ThemeType value)
    {
        _settingsService.Theme = value;
        _snackbarService.Show("Theme changed", "Changes will take effect for new windows", SymbolRegular.ChatWarning24, ControlAppearance.Success);
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        _settingsService.Background = value;
        var window = (FluentWindow) Wpf.Ui.Application.Current;
        window.WindowBackdropType = value;
    }

    partial void OnIsSmoothEnabledChanged(bool value)
    {
        var transitionDuration = _settingsService.ApplyTransition(value);
        _navigationService.GetNavigationControl().TransitionDuration = transitionDuration;
    }

    partial void OnIsUnsupportedAllowedChanged(bool value)
    {
        _settingsService.IsUnsupportedAllowed = value;
    }

    partial void OnIsExtensionsAllowedChanged(bool value)
    {
        _settingsService.IsExtensionsAllowed = value;
    }

    partial void OnIsModifyPanelVisibleChanged(bool value)
    {
        Application.UpdateModifyPanelVisibility(value);
        _settingsService.IsModifyPanelVisible = value;
    }
}