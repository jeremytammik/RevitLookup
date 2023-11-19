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
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SettingsViewModel(ISettingsService settingsService, INavigationService navigationService) : ObservableObject
{
    [ObservableProperty] private ApplicationTheme _theme = settingsService.Theme;
    [ObservableProperty] private WindowBackdropType _background = settingsService.Background;
    [ObservableProperty] private bool _isSmoothEnabled = settingsService.TransitionDuration > 0;
    [ObservableProperty] private bool _isHardwareRenderingAllowed = settingsService.IsHardwareRenderingAllowed;
    [ObservableProperty] private bool _isModifyTabAllowed = settingsService.IsModifyTabAllowed;
    [ObservableProperty] private bool _isUnsupportedAllowed = settingsService.IsUnsupportedAllowed;
    [ObservableProperty] private bool _isPrivateAllowed = settingsService.IsPrivateAllowed;
    [ObservableProperty] private bool _isStaticAllowed = settingsService.IsStaticAllowed;
    [ObservableProperty] private bool _isFieldsAllowed = settingsService.IsFieldsAllowed;
    [ObservableProperty] private bool _isEventsAllowed = settingsService.IsEventsAllowed;
    [ObservableProperty] private bool _isExtensionsAllowed = settingsService.IsExtensionsAllowed;

    public List<ApplicationTheme> Themes { get; } =
    [
        ApplicationTheme.Light,
        ApplicationTheme.Dark
    ];

    public List<WindowBackdropType> BackgroundEffects { get; } =
    [
        WindowBackdropType.None,
        WindowBackdropType.Mica
    ];

    partial void OnThemeChanged(ApplicationTheme value)
    {
        settingsService.Theme = value;
        ApplicationThemeManager.Apply(settingsService.Theme, settingsService.Background);
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        settingsService.Background = value;
        var window = (FluentWindow) Wpf.Ui.Application.MainWindow;
        window.WindowBackdropType = value;
    }

    partial void OnIsSmoothEnabledChanged(bool value)
    {
        var transitionDuration = settingsService.ApplyTransition(value);
        navigationService.GetNavigationControl().TransitionDuration = transitionDuration;
    }

    partial void OnIsHardwareRenderingAllowedChanged(bool value)
    {
        settingsService.IsHardwareRenderingAllowed = value;
        if (value) Application.EnableHardwareRendering(settingsService);
        else Application.DisableHardwareRendering(settingsService);
    }

    partial void OnIsModifyTabAllowedChanged(bool value)
    {
        settingsService.IsModifyTabAllowed = value;
        RibbonController.ReloadPanels(settingsService);
    }

    partial void OnIsUnsupportedAllowedChanged(bool value)
    {
        settingsService.IsUnsupportedAllowed = value;
    }

    partial void OnIsPrivateAllowedChanged(bool value)
    {
        settingsService.IsPrivateAllowed = value;
    }

    partial void OnIsStaticAllowedChanged(bool value)
    {
        settingsService.IsStaticAllowed = value;
    }

    partial void OnIsFieldsAllowedChanged(bool value)
    {
        settingsService.IsFieldsAllowed = value;
    }

    partial void OnIsEventsAllowedChanged(bool value)
    {
        settingsService.IsEventsAllowed = value;
    }

    partial void OnIsExtensionsAllowedChanged(bool value)
    {
        settingsService.IsExtensionsAllowed = value;
    }
}