// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Demo.Moq;
using RevitLookup.UI.Services;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Demo;

public sealed partial class App
{
    private string _revitPath;
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        var host = CreateHost();
        await Host.StartHost(host);
        var window = Host.GetService<IWindow>();
        window.Show();
        window.Scope.GetService<INavigationService>().Navigate(typeof(DashboardView));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        var settingsService = Host.GetService<ISettingsService>();
        settingsService.Save();

        await Host.StopHost();
    }

    private static IHost CreateHost()
    {
        var host = Microsoft.Extensions.Hosting.Host
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
                    new("ConfigFolder", Path.Combine(assemblyDirectory, "Config")),
                    new("DownloadFolder", Path.Combine(assemblyDirectory, "Downloads"))
                });
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ISettingsService, SettingsService>();
                services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

                services.AddScoped<IWindowController, WindowController>();
                services.AddScoped<INavigationService, NavigationService>();
                services.AddScoped<IPageService, PageService>();
                services.AddScoped<ISnackbarService, SnackbarService>();
                services.AddScoped<IDialogService, DialogService>();

                services.AddScoped<AboutView>();
                services.AddScoped<AboutViewModel>();
                services.AddScoped<DashboardView>();
                services.AddScoped<DashboardViewModel>();
                services.AddScoped<SettingsView>();
                services.AddScoped<SettingsViewModel>();
                services.AddScoped<SnoopView>();
                services.AddScoped<ISnoopService, MoqSnoopViewModel>();

                services.AddTransient<IWindow, RevitLookupView>();
            }).Build();
        return host;
    }

    private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        var path = _revitPath ?? $@"C:\Program Files\Autodesk\Revit 20{assemblyName.Version.Major}";
        if (!Directory.Exists(path)) return null;
        _revitPath = path;

        return Directory.EnumerateFiles(_revitPath, $"{assemblyName.Name}.dll")
            .Select(enumerateFile => new FileInfo(enumerateFile))
            .Select(fileInfo => Assembly.LoadFile(fileInfo.FullName))
            .FirstOrDefault();
    }
}