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

using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.Views.Appearance;
using RevitLookup.Views.Dialogs;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.ViewModels.Pages;

public sealed partial class SettingsViewModel(
    ISettingsService settingsService,
    INavigationService navigationService,
    IContentDialogService dialogService,
    NotificationService notificationService,
    IWindow window)
    : ObservableObject
{
    [ObservableProperty] private ApplicationTheme _theme = settingsService.GeneralSettings.Theme;
    [ObservableProperty] private WindowBackdropType _background = settingsService.GeneralSettings.Background;

    [ObservableProperty] private bool _useTransition = settingsService.GeneralSettings.TransitionDuration > 0;
    [ObservableProperty] private bool _useHardwareRendering = settingsService.GeneralSettings.UseHardwareRendering;
    [ObservableProperty] private bool _useSizeRestoring = settingsService.GeneralSettings.UseSizeRestoring;
    [ObservableProperty] private bool _useModifyTab = settingsService.GeneralSettings.UseModifyTab;

    public List<ApplicationTheme> Themes { get; } =
    [
        ApplicationTheme.Auto,
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

    [RelayCommand]
    private async Task ResetSettings()
    {
        var dialog = new ResetSettingsDialog(dialogService, settingsService);
        var result = await dialog.ShowAsync();
        if (!result) return;

        foreach (var settings in dialog.SelectedSettings)
        {
            try
            {
                settings.SetDefault();
            }
            catch (Exception exception)
            {
                notificationService.ShowWarning("Reset settings error", exception.Message);
            }
        }

        notificationService.ShowSuccess("Reset was successful", "Some changes will be applied after closing the window");
    }

    partial void OnThemeChanged(ApplicationTheme value)
    {
        settingsService.GeneralSettings.Theme = value;

        foreach (var target in Wpf.Ui.Application.Windows)
        {
            Wpf.Ui.Application.MainWindow = target;

            if (value == ApplicationTheme.Auto)
            {
                RevitThemeWatcher.Watch(target);
            }
            else
            {
                ApplicationThemeManager.Apply(settingsService.GeneralSettings.Theme, settingsService.GeneralSettings.Background);
            }
        }
    }

    partial void OnThemeChanged(ApplicationTheme oldValue, ApplicationTheme newValue)
    {
        if (oldValue == ApplicationTheme.Auto)
        {
            RevitThemeWatcher.Unwatch();
        }
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        settingsService.GeneralSettings.Background = value;
        ApplicationThemeManager.Apply(settingsService.GeneralSettings.Theme, settingsService.GeneralSettings.Background);
    }

    partial void OnUseTransitionChanged(bool value)
    {
        var transitionDuration = settingsService.GeneralSettings.ApplyTransition(value);
        navigationService.GetNavigationControl().TransitionDuration = transitionDuration;
    }

    partial void OnUseHardwareRenderingChanged(bool value)
    {
        settingsService.GeneralSettings.UseHardwareRendering = value;
        if (value) Application.EnableHardwareRendering(settingsService.GeneralSettings);
        else Application.DisableHardwareRendering(settingsService.GeneralSettings);
    }

    partial void OnUseSizeRestoringChanged(bool value)
    {
        settingsService.GeneralSettings.UseSizeRestoring = value;
        if (value) window.EnableSizeTracking();
        else window.DisableSizeTracking();
    }

    partial void OnUseModifyTabChanged(bool value)
    {
        settingsService.GeneralSettings.UseModifyTab = value;
        RibbonController.ReloadPanels(settingsService.GeneralSettings);
    }
}