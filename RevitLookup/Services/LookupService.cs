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
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using Wpf.Ui;

namespace RevitLookup.Services;

public sealed class LookupService : ILookupService
{
    private static Dispatcher _dispatcher;

    private Window _owner;
    private Task _activeTask;
    private IServiceScope _serviceScope;
    private ISnoopVisualService _visualService;
    private INavigationService _navigationService;
    private Window _window;

    static LookupService()
    {
        var uiThread = new Thread(Dispatcher.Run);
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();

        EnsureThreadStart(uiThread);
    }

    public LookupService(IServiceScopeFactory scopeFactory)
    {
        _dispatcher.Invoke(() =>
        {
            _serviceScope = scopeFactory.CreateScope();

            _window = (Window) _serviceScope.ServiceProvider.GetService<IWindow>();
            _visualService = _serviceScope.ServiceProvider.GetService<ISnoopVisualService>();
            _navigationService = _serviceScope.ServiceProvider.GetService<INavigationService>();

            _window.Closed += (_, _) => _serviceScope.Dispose();
        });
    }

    public ILookupServiceDependsStage Snoop(SnoopableType snoopableType)
    {
        _dispatcher.Invoke(() => _activeTask = _visualService!.SnoopAsync(snoopableType));
        return this;
    }

    public ILookupServiceDependsStage Snoop(SnoopableObject snoopableObject)
    {
        _dispatcher.Invoke(() => _visualService.Snoop(snoopableObject));
        return this;
    }

    public ILookupServiceDependsStage Snoop(IReadOnlyCollection<SnoopableObject> snoopableObjects)
    {
        _dispatcher.Invoke(() => _visualService.Snoop(snoopableObjects));
        return this;
    }

    public ILookupServiceShowStage DependsOn(IServiceProvider provider)
    {
        _dispatcher.Invoke(() => _owner = (Window) provider.GetService<IWindow>());
        return this;
    }

    public ILookupServiceExecuteStage Show<T>() where T : Page
    {
        _dispatcher.Invoke(() =>
        {
            if (_activeTask is null)
            {
                ShowPage<T>();
            }
            else
            {
                _activeTask = _activeTask.ContinueWith(_ => ShowPage<T>(), TaskScheduler.FromCurrentSynchronizationContext());
            }
        });
        return this;
    }

    public void Execute<T>(Action<T> handler) where T : class
    {
        _dispatcher.Invoke(() =>
        {
            if (_activeTask is null)
            {
                InvokeHandler(handler);
            }
            else
            {
                _activeTask = _activeTask.ContinueWith(_ => InvokeHandler(handler), TaskScheduler.FromCurrentSynchronizationContext());
            }
        });
    }

    private void InvokeHandler<T>(Action<T> handler) where T : class
    {
        var service = _serviceScope.ServiceProvider.GetService<T>();
        handler.Invoke(service);
    }

    private void ShowPage<T>() where T : Page
    {
        if (_owner is null)
        {
            _window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            _window.Left = _owner.Left + 47;
            _window.Top = _owner.Top + 49;
        }

        _window.Show(RevitApi.UiApplication.MainWindowHandle);
        _navigationService.Navigate(typeof(T));
    }

    private static void EnsureThreadStart(Thread thread)
    {
        var dispatcher = Dispatcher.FromThread(thread);
        if (dispatcher is null)
        {
            var spinWait = new SpinWait();
            while (dispatcher is null)
            {
                spinWait.SpinOnce();
                dispatcher = Dispatcher.FromThread(thread);
            }
        }

        _dispatcher = dispatcher;
    }
}