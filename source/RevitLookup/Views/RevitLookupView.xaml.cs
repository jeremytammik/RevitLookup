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

using System.Windows;
using System.Windows.Input;
using RevitLookup.Models.Settings;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.Views;

// ReSharper disable once RedundantExtendsListEntry
public sealed partial class RevitLookupView : FluentWindow, IWindow
{
    private readonly GeneralSettings _settings;
    
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
        ISettingsService settingsService,
        ISoftwareUpdateService updateService)
        : this()
    {
        _settings = settingsService.GeneralSettings;
        RootNavigation.TransitionDuration = _settings.TransitionDuration;
        WindowBackdropType = _settings.Background;
        
        navigationService.SetNavigationControl(RootNavigation);
        dialogService.SetContentPresenter(RootContentDialog);
        
        snackbarService.SetSnackbarPresenter(RootSnackbar);
        snackbarService.DefaultTimeOut = TimeSpan.FromSeconds(3);
        
        RestoreSize();
        SetupBadges(updateService);
        ApplicationThemeManager.Apply(_settings.Theme, _settings.Background);
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
    
    private void RestoreSize()
    {
        if (!_settings.UseSizeRestoring) return;
        
        if (_settings.WindowWidth >= MinWidth) Width = _settings.WindowWidth;
        if (_settings.WindowHeight >= MinHeight) Height = _settings.WindowHeight;
        
        EnableSizeTracking();
    }
    
    private void SetupBadges(ISoftwareUpdateService updateService)
    {
        if (updateService.State != SoftwareUpdateState.ReadyToDownload) return;
        AboutItemBadge.Visibility = Visibility.Visible;
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
        _settings.WindowWidth = args.NewSize.Width;
        _settings.WindowHeight = args.NewSize.Height;
    }
    
    protected override void OnActivated(EventArgs args)
    {
        base.OnActivated(args);
        Wpf.Ui.Application.MainWindow = this;
    }
    
    protected override void OnClosed(EventArgs args)
    {
        base.OnClosed(args);
        Wpf.Ui.Application.Windows.Remove(this);
    }
}