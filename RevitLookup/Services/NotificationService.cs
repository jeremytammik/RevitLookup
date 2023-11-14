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
using RevitLookup.Services.Contracts;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace RevitLookup.Services;

public sealed class NotificationService
{
    private readonly IWindow _window;
    private readonly ISnackbarService _snackbarService;
    private Action _pendingNotifications;

    public NotificationService(ISnackbarService snackbarService, IWindow window)
    {
        _snackbarService = snackbarService;
        _window = window;
    }

    public void ShowError(string title, Exception exception)
    {
        ShowError(title, exception.Message);
    }

    public void ShowError(string title, string message)
    {
        if (!_window.IsLoaded)
        {
            if (_pendingNotifications is null) _window.Loaded += ShowPendingNotifications;
            _pendingNotifications += () => ShowErrorBar(title, message);
        }
        else
        {
            ShowErrorBar(title, message);
        }
    }

    public void ShowWarning(string title, string message)
    {
        if (!_window.IsLoaded)
        {
            if (_pendingNotifications is null) _window.Loaded += ShowPendingNotifications;
            _pendingNotifications += () => ShowWarningBar(title, message);
        }
        else
        {
            ShowWarningBar(title, message);
        }
    }

    private void ShowWarningBar(string title, string message)
    {
        _snackbarService.Show(title, message, SymbolRegular.Warning24, ControlAppearance.Caution);
    }

    private void ShowErrorBar(string title, string message)
    {
        _snackbarService.Show(title, message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
    }

    private void ShowPendingNotifications(object sender, RoutedEventArgs args)
    {
        _window.Loaded -= ShowPendingNotifications;
        _pendingNotifications.Invoke();
        _pendingNotifications = null;
    }
}