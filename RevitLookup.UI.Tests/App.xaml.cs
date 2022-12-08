// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.UI.Mvvm.Services;
using RevitLookup.UI.Tests.Moq;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Tests;

public sealed partial class App
{
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var host = CreateHost();
        await Host.StartHost(host);
        var window = Host.GetService<ILookupInstance>();
        window.ShowWindow();
        window.Navigate(typeof(DashboardView));
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
                    new("SettingsFolder", Path.Combine(assemblyDirectory, "Settings")),
                    new("DownloadFolder", Path.Combine(assemblyDirectory, "Downloads"))
                });
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ISettingsService, SettingsService>();
                services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

                services.AddScoped<INavigationService, NavigationService>();
                services.AddScoped<IPageService, PageService>();
                services.AddScoped<IDialogService, DialogService>();

                services.AddScoped<AboutView>();
                services.AddScoped<AboutViewModel>();
                services.AddScoped<DashboardView>();
                services.AddScoped<DashboardViewModel>();
                services.AddScoped<SettingsView>();
                services.AddScoped<SettingsViewModel>();
                services.AddScoped<SnoopView>();
                services.AddScoped<ISnoopService, MoqSnoopViewModel>();

                services.AddTransient<ILookupInstance, RevitLookupView>();
            }).Build();
        return host;
    }
}