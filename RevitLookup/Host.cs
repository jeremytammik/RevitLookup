using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.Utils;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Services;

namespace RevitLookup;

public static class Host
{
    private static IHost _host;

    public static async Task StartHost()
    {
        _host = Microsoft.Extensions.Hosting.Host
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
                services.AddScoped<EventsView>();
                services.AddScoped<EventsViewModel>();
                services.AddScoped<SnoopView>();
                services.AddScoped<ISnoopService, SnoopViewModel>();

                services.AddTransient<IWindow, RevitLookupView>();
            }).Build();

        await _host.StartAsync();
    }

    public static async Task StartHost(IHost host)
    {
        _host = host;
        await host.StartAsync();
    }

    public static async Task StopHost()
    {
        await _host.StopAsync();
        _host.Dispose();
    }

    public static T GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }
}