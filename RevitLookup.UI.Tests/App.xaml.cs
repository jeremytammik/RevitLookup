// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.UI.Mvvm.Services;
using RevitLookup.UI.Tests.Services;
using RevitLookup.UI.Tests.Services.Contracts;
using RevitLookup.UI.Tests.ViewModels.Pages;
using RevitLookup.UI.Tests.Views;
using RevitLookup.UI.Tests.Views.Pages;

namespace RevitLookup.UI.Tests;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private static readonly IHost Host = Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(builder =>
        {
            var assemblyLocation = Assembly.GetEntryAssembly()!.Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
            builder.SetBasePath(assemblyDirectory);
            builder.AddInMemoryCollection(new KeyValuePair<string, string>[]
            {
                new("Assembly", assemblyLocation),
                new("ConfigFolder", Path.Combine(assemblyDirectory, "Configurations")),
                new("DownloadFolder",  Path.Combine(assemblyDirectory, "Downloads"))
            });
        })
        .ConfigureServices((context, services) =>
        {
            services.AddHostedService<ApplicationHostService>();

            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

            services.AddScoped<INavigationWindow, RevitLookupView>();

            services.AddScoped<AboutView>();
            services.AddScoped<AboutViewModel>();

            services.AddScoped<DashboardView>();
            services.AddScoped<DashboardViewModel>();

            services.AddScoped<SettingsView>();
            services.AddScoped<SettingsViewModel>();

            services.AddScoped<SnoopSummaryView>();
        }).Build();

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T GetService<T>() where T : class
    {
        return Host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await Host.StartAsync();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await Host.StopAsync();
        Host.Dispose();
    }
}