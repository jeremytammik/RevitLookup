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
using RevitLookup.UI.Demo.Moq;
using RevitLookup.Utils;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Services;

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
                var addinVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
#if RELEASE
                var userDataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"Autodesk\Revit\Addins\", addinVersion.Split('.')[0], "RevitLookup");
#else
                var userDataLocation = Path.GetDirectoryName(assemblyLocation)!;
#endif
                var writeAccess = AccessUtils.CheckWriteAccess(assemblyLocation) &&
                                  !assemblyLocation.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

                var targetFrameworkAttributes = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true);
                var targetFrameworkAttribute = (TargetFrameworkAttribute) targetFrameworkAttributes.First();
                var targetFramework = targetFrameworkAttribute.FrameworkDisplayName;

                builder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new("Assembly", assemblyLocation),
                    new("Framework", targetFramework),
                    new("AddinVersion", addinVersion),
                    new("ConfigFolder", Path.Combine(userDataLocation, "Config")),
                    new("DownloadFolder", Path.Combine(userDataLocation, "Downloads")),
                    new("FolderAccess", writeAccess ? "Write" : "Read")
                });
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ISettingsService, SettingsService>();
                services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

                services.AddScoped<IWindowController, WindowController>();
                services.AddScoped<INavigationService, NavigationService>();
                services.AddScoped<ISnackbarService, SnackbarService>();
                services.AddScoped<IContentDialogService, ContentDialogService>();

                services.AddScoped<AboutView>();
                services.AddScoped<AboutViewModel>();
                services.AddScoped<DashboardView>();
                services.AddScoped<DashboardViewModel>();
                services.AddScoped<SettingsView>();
                services.AddScoped<SettingsViewModel>();
                services.AddScoped<SnoopView>();
                services.AddScoped<EventsView>();
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