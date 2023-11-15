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
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Services;

namespace RevitLookup;

public static class Host
{
    private static IHost _host;

    public static void StartHost()
    {
        _host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(SetConfiguration)
            .ConfigureServices(AddServices)
            .Build();

        _host.Start();
    }

    private static void AddServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<ISnackbarService, SnackbarService>();
        services.AddScoped<IContentDialogService, ContentDialogService>();
        services.AddScoped<NotificationService>();

        services.AddScoped<ISnoopVisualService, SnoopVisualService>();
        services.AddScoped<AboutView>();
        services.AddScoped<AboutViewModel>();
        services.AddScoped<DashboardView>();
        services.AddScoped<DashboardViewModel>();
        services.AddScoped<SettingsView>();
        services.AddScoped<SettingsViewModel>();
        services.AddScoped<EventsView>();
        services.AddScoped<EventsViewModel>();
        services.AddScoped<SnoopView>();
        services.AddScoped<ISnoopViewModel, SnoopViewModel>();
        services.AddScoped<IWindow, RevitLookupView>();

        services.AddTransient<ILookupService, LookupService>();
    }

    private static void SetConfiguration(IConfigurationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var addinVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
#if RELEASE
        var version = addinVersion.Split('.')[0];
        if (version == "1") version = "Develop";
        var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var userDataLocation = Path.Combine(programDataPath, @"Autodesk\Revit\Addins\", version, "RevitLookup");
#else
        var userDataLocation = Path.GetDirectoryName(assemblyLocation)!;
#endif
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var writeAccess = AccessUtils.CheckWriteAccess(assemblyLocation) && !assemblyLocation.StartsWith(appDataPath);

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
    }

    public static void StartHost(IHost host)
    {
        _host = host;
        host.Start();
    }

    public static void StopHost()
    {
        _host.StopAsync();
    }

    public static T GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
    {
        return serviceProvider.GetService(typeof(T)) as T;
    }
}