using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Services;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;

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