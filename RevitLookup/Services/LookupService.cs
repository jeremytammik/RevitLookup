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
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Core;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using Wpf.Ui.Contracts;

namespace RevitLookup.Services;

public sealed class LookupService
{
    private Window _owner;
    private Task _activeTask;
    private readonly IServiceProvider _serviceProvider;

    public LookupService(IServiceScopeFactory scopeFactory)
    {
        _serviceProvider = scopeFactory.CreateScope().ServiceProvider;
    }

    public LookupService Snoop(SnoopableType snoopableType)
    {
        _activeTask = _serviceProvider.GetService<ISnoopService>()!.SnoopAsync(snoopableType);
        return this;
    }

    public LookupService Snoop(SnoopableObject snoopableObject)
    {
        _serviceProvider.GetService<ISnoopService>()!.Snoop(snoopableObject);
        return this;
    }
    
    
    public LookupService Attach(IWindow window)
    {
        _owner = (Window) window;
        return this;
    }

    public LookupService Show<T>() where T : Page
    {
        if (_activeTask is null)
        {
            ShowPage();
        }
        else
        {
            _activeTask = _activeTask.ContinueWith(_ => ShowPage(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        return this;

        void ShowPage()
        {
            var window = (Window) _serviceProvider.GetService<IWindow>();

            if (_owner is null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                window.Left = _owner.Left + 47;
                window.Top = _owner.Top + 49;
            }

            window.Show(RevitApi.UiApplication.MainWindowHandle);
            _serviceProvider.GetService<INavigationService>().Navigate(typeof(T));
        }
    }

    public void Execute<T>(Action<T> handler) where T : class
    {
        if (_activeTask is null)
        {
            InvokeHandler();
        }
        else
        {
            _activeTask = _activeTask.ContinueWith(_ => InvokeHandler(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        return;

        void InvokeHandler()
        {
            var service = _serviceProvider.GetService<T>();
            handler.Invoke(service);
        }
    }
}