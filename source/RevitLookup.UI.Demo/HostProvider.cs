using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Demo.Mock;
using RevitLookup.Utils;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;
using Wpf.Ui;
using MoqSnoopViewModel = RevitLookup.UI.Demo.Mock.MoqSnoopViewModel;
using MoqEventsViewModel = RevitLookup.UI.Demo.Mock.MoqEventsViewModel;

namespace RevitLookup.UI.Demo;

public static class HostProvider
{
    public static IHost CreateHost()
    {
        var host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(SetConfiguration)
            .ConfigureServices(AddServices)
            .Build();

        return host;
    }

    private static void AddServices(HostBuilderContext _, IServiceCollection services)
    {
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<ISnackbarService, SnackbarService>();
        services.AddScoped<IContentDialogService, ContentDialogService>();
        services.AddScoped<NotificationService>();

        services.AddScoped<ISnoopVisualService, MoqSnoopVisualService>();
        services.AddScoped<AboutView>();
        services.AddScoped<AboutViewModel>();
        services.AddScoped<DashboardView>();
        services.AddScoped<DashboardViewModel>();
        services.AddScoped<SettingsView>();
        services.AddScoped<SettingsViewModel>();
        services.AddScoped<EventsView>();
        services.AddScoped<IEventsViewModel, MoqEventsViewModel>();
        services.AddScoped<SnoopView>();
        services.AddScoped<ISnoopViewModel, MoqSnoopViewModel>();
        services.AddScoped<IWindow, RevitLookupView>();

        services.AddTransient<ILookupService, MoqLookupService>();
    }

    private static void SetConfiguration(IConfigurationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
        var addinVersion = new Version(versionInfo.FileVersion).ToString(3);
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
}