// Copyright 2003-2022 by Autodesk, Inc. 
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

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nice3point.Revit.Toolkit.External;
using RevitLookup.Commands;
using RevitLookup.Core;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.UI.Mvvm.Services;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;

namespace RevitLookup;

[UsedImplicitly]
public class Application : ExternalApplication
{
    private static IHost _host;

    public override async void OnStartup()
    {
        RevitApi.UiApplication = UiApplication;
        CreateRibbonPanel();
        await StartHost();
    }

    public override async void OnShutdown()
    {
        UpdateSoftware();
        await StopHost();
    }

    private void CreateRibbonPanel()
    {
        var ribbonPanel = Application.CreatePanel("Revit Lookup");

        // var splitButton = ribbonPanel.AddSplitButton("RevitLookup", "RevitLookup");

        var dashboardButton = ribbonPanel.AddPushButton<DashboardCommand>("Snoop");
        dashboardButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        dashboardButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");
    }

    private static async Task StartHost()
    {
        _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyLocation = assembly.Location;
                var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
                builder.SetBasePath(assemblyDirectory);

                var targetFrameworkAttributes = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true);
                var targetFrameworkAttribute = (TargetFrameworkAttribute) targetFrameworkAttributes.First();
                var targetFramework = targetFrameworkAttribute.FrameworkDisplayName;

                builder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new("Assembly", assemblyLocation),
                    new("Framework", targetFramework),
                    new("ConfigFolder", Path.Combine(assemblyDirectory, "Configurations")),
                    new("DownloadFolder", Path.Combine(assemblyDirectory, "Downloads"))
                });
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddScoped<IPageService, PageService>();
                services.AddScoped<IDialogService, DialogService>();

                services.AddScoped<AboutView>();
                services.AddScoped<AboutViewModel>();

                services.AddScoped<DashboardView>();
                services.AddScoped<DashboardViewModel>();

                services.AddScoped<SettingsView>();
                services.AddScoped<SettingsViewModel>();

                services.AddScoped<SnoopSummaryView>();

                services.AddTransient<INavigationWindow, RevitLookupView>();
            }).Build();

        await _host.StartAsync();
    }

    private static async Task StopHost()
    {
        await _host.StopAsync();
        _host.Dispose();
    }

    private static void UpdateSoftware()
    {
        var updateService = GetService<ISoftwareUpdateService>();
        if (File.Exists(updateService.LocalFilePath)) Process.Start(updateService.LocalFilePath);
    }

    /// <summary>
    ///     Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null" />.</returns>
    public static T GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    ///     Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null" />.</returns>
    public static INavigationWindow Show()
    {
        return _host.Services.GetService(typeof(INavigationWindow)) as INavigationWindow;
    }
}