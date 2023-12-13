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

using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Services.Contracts;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Win32;

namespace RevitLookup.Views;

public sealed partial class RevitLookupView : IWindow
{
    private readonly ISettingsService _settingsService;

    private RevitLookupView()
    {
        Wpf.Ui.Application.MainWindow = this;
        Wpf.Ui.Application.Windows.Add(this);
        InitializeComponent();
        AddShortcuts();
    }

    public RevitLookupView(
        INavigationService navigationService,
        IContentDialogService dialogService,
        ISnackbarService snackbarService,
        ISettingsService settingsService)
        : this()
    {
        _settingsService = settingsService;
        RootNavigation.TransitionDuration = settingsService.TransitionDuration;
        WindowBackdropType = settingsService.Background;

        navigationService.SetNavigationControl(RootNavigation);
        dialogService.SetContentPresenter(RootContentDialog);

        snackbarService.SetSnackbarPresenter(RootSnackbar);
        snackbarService.DefaultTimeOut = TimeSpan.FromSeconds(3);

        RestoreSize(settingsService);
    }

    private void AddShortcuts()
    {
        var closeCurrentCommand = new RelayCommand(Close);
        var closeAllCommand = new RelayCommand(() =>
        {
            for (var i = Wpf.Ui.Application.Windows.Count - 1; i >= 0; i--)
            {
                var window = Wpf.Ui.Application.Windows[i];
                window.Close();
            }
        });

        InputBindings.Add(new KeyBinding(closeAllCommand, new KeyGesture(Key.Escape, ModifierKeys.Shift)));
        InputBindings.Add(new KeyBinding(closeCurrentCommand, new KeyGesture(Key.Escape)));
    }

    private void RestoreSize(ISettingsService settingsService)
    {
        if (!settingsService.UseSizeRestoring) return;

        if (settingsService.WindowWidth >= MinWidth) Width = settingsService.WindowWidth;
        if (settingsService.WindowHeight >= MinHeight) Height = settingsService.WindowHeight;

        EnableSizeTracking();
    }

    public void EnableSizeTracking()
    {
        SizeChanged += OnSizeChanged;
    }

    public void DisableSizeTracking()
    {
        SizeChanged -= OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args)
    {
        _settingsService.WindowWidth = args.NewSize.Width;
        _settingsService.WindowHeight = args.NewSize.Height;
    }

    protected override void OnActivated(EventArgs args)
    {
        base.OnActivated(args);
        Wpf.Ui.Application.MainWindow = this;
        ApplicationThemeManager.ApplySystemTheme();
        if (Utilities.IsOSWindows11OrNewer)
        {
            ApplicationThemeManager.Apply(_settingsService.Theme, _settingsService.Background);
        }
    }

    protected override void OnClosed(EventArgs args)
    {
        base.OnClosed(args);
        Wpf.Ui.Application.Windows.Remove(this);
    }
}