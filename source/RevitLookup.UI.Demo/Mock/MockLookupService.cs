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
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;
using Wpf.Ui;

namespace RevitLookup.UI.Demo.Mock;

public sealed class MockLookupService(IServiceScopeFactory scopeFactory) : ILookupService
{
    private Window _owner;
    private Task _activeTask;
    private readonly IServiceProvider _serviceProvider = scopeFactory.CreateScope().ServiceProvider;

    public ILookupServiceDependsStage Snoop(SnoopableType snoopableType)
    {
        _activeTask = _serviceProvider.GetService<ISnoopVisualService>()!.SnoopAsync(snoopableType);
        return this;
    }

    public ILookupServiceDependsStage Snoop(SnoopableObject snoopableObject)
    {
        _serviceProvider.GetService<ISnoopVisualService>()!.Snoop(snoopableObject);
        return this;
    }

    public ILookupServiceDependsStage Snoop(IReadOnlyCollection<SnoopableObject> snoopableObjects)
    {
        _serviceProvider.GetService<ISnoopVisualService>()!.Snoop(snoopableObjects);
        return this;
    }

    public ILookupServiceShowStage DependsOn(IServiceProvider provider)
    {
        _owner = (Window) provider.GetService<IWindow>();
        return this;
    }

    public ILookupServiceExecuteStage Show<T>() where T : Page
    {
        if (_activeTask is null)
        {
            ShowPage<T>();
        }
        else
        {
            _activeTask = _activeTask.ContinueWith(_ => ShowPage<T>(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        return this;
    }

    public void Execute<T>(Action<T> handler) where T : class
    {
        if (_activeTask is null)
        {
            InvokeHandler(handler);
        }
        else
        {
            _activeTask = _activeTask.ContinueWith(_ => InvokeHandler(handler), TaskScheduler.FromCurrentSynchronizationContext());
        }

    }

    private void ShowPage<T>() where T : Page
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

        window.Show();
        _serviceProvider.GetService<INavigationService>().Navigate(typeof(T));
    }

    private void InvokeHandler<T>(Action<T> handler) where T : class
    {
        var service = _serviceProvider.GetService<T>();
        handler.Invoke(service);
    }
}