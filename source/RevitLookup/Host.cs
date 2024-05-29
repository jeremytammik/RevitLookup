using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RevitLookup.Config;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Dialogs.Visualization;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Dialogs.Visualization;
using RevitLookup.Views.Pages;
using Wpf.Ui;

namespace RevitLookup;

public static class Host
{
    private static IHost _host;
    
    public static void Start()
    {
        var builder = new HostApplicationBuilder(new HostApplicationBuilderSettings
        {
            ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location),
            DisableDefaults = true
        });
        
        //Logging
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilogConfiguration();
        
        //Configuration
        builder.Services.AddOptions(builder.Configuration);
        
        //App services
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();
        
        //UI services
        builder.Services.AddScoped<INavigationService, NavigationService>();
        builder.Services.AddScoped<ISnackbarService, SnackbarService>();
        builder.Services.AddScoped<IContentDialogService, ContentDialogService>();
        builder.Services.AddScoped<NotificationService>();
        
        //Views
        builder.Services.AddScoped<ISnoopVisualService, SnoopVisualService>();
        builder.Services.AddScoped<AboutView>();
        builder.Services.AddScoped<AboutViewModel>();
        builder.Services.AddScoped<DashboardView>();
        builder.Services.AddScoped<IDashboardViewModel, DashboardViewModel>();
        builder.Services.AddScoped<SettingsView>();
        builder.Services.AddScoped<SettingsViewModel>();
        builder.Services.AddScoped<EventsView>();
        builder.Services.AddScoped<IEventsViewModel, EventsViewModel>();
        builder.Services.AddScoped<SnoopView>();
        builder.Services.AddScoped<ISnoopViewModel, SnoopViewModel>();
        builder.Services.AddScoped<IWindow, RevitLookupView>();
        
        //Dialogs
        builder.Services.AddScoped<BoundingBoxVisualizationDialog>();
        builder.Services.AddScoped<BoundingBoxVisualizationViewModel>();
        builder.Services.AddScoped<FaceVisualizationDialog>();
        builder.Services.AddScoped<FaceVisualizationViewModel>();
        builder.Services.AddScoped<MeshVisualizationDialog>();
        builder.Services.AddScoped<MeshVisualizationViewModel>();
        builder.Services.AddScoped<PolylineVisualizationDialog>();
        builder.Services.AddScoped<PolylineVisualizationViewModel>();
        builder.Services.AddScoped<SolidVisualizationDialog>();
        builder.Services.AddScoped<SolidVisualizationViewModel>();
        builder.Services.AddScoped<XyzVisualizationDialog>();
        builder.Services.AddScoped<XyzVisualizationViewModel>();
        
        //Startup view
        builder.Services.AddTransient<ILookupService, LookupService>();
        
        _host = builder.Build();
        _host.Start();
    }
    
    public static void Start(IHost host)
    {
        _host = host;
        host.Start();
    }
    
    public static void Stop()
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