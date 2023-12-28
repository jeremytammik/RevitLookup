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

public sealed partial class SettingsViewModel(
    ISettingsService settingsService,
    INavigationService navigationService,
    IWindow window)
    : ObservableObject
{
    [ObservableProperty] private ApplicationTheme _theme = settingsService.Theme;
    [ObservableProperty] private WindowBackdropType _background = settingsService.Background;

    [ObservableProperty] private bool _useTransition = settingsService.TransitionDuration > 0;
    [ObservableProperty] private bool _useHardwareRendering = settingsService.UseHardwareRendering;
    [ObservableProperty] private bool _useSizeRestoring = settingsService.UseSizeRestoring;
    [ObservableProperty] private bool _useModifyTab = settingsService.UseModifyTab;

    public List<ApplicationTheme> Themes { get; } =
    [
        ApplicationTheme.Light,
        ApplicationTheme.Dark
        // ApplicationTheme.HighContrast
    ];

    public List<WindowBackdropType> BackgroundEffects { get; } =
    [
        WindowBackdropType.None,
        WindowBackdropType.Acrylic,
        WindowBackdropType.Tabbed,
        WindowBackdropType.Mica
    ];

    partial void OnThemeChanged(ApplicationTheme value)
    {
        settingsService.Theme = value;

        foreach (var target in Wpf.Ui.Application.Windows)
        {
            Wpf.Ui.Application.MainWindow = target;
            ApplicationThemeManager.Apply(settingsService.Theme, settingsService.Background);
        }
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        settingsService.Background = value;
        ApplicationThemeManager.Apply(settingsService.Theme, settingsService.Background);
    }

    partial void OnUseTransitionChanged(bool value)
    {
        var transitionDuration = settingsService.ApplyTransition(value);
        navigationService.GetNavigationControl().TransitionDuration = transitionDuration;
    }

    partial void OnUseHardwareRenderingChanged(bool value)
    {
        settingsService.UseHardwareRendering = value;
        if (value) Application.EnableHardwareRendering(settingsService);
        else Application.DisableHardwareRendering(settingsService);
    }

    partial void OnUseSizeRestoringChanged(bool value)
    {
        settingsService.UseSizeRestoring = value;
        if (value) window.EnableSizeTracking();
        else window.DisableSizeTracking();
    }

    partial void OnUseModifyTabChanged(bool value)
    {
        settingsService.UseModifyTab = value;
        RibbonController.ReloadPanels(settingsService);
    }
}