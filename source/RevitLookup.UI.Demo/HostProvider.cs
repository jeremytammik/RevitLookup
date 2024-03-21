using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RevitLookup.Config;
using RevitLookup.Services;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Demo.Mock.Services;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Pages;
using RevitLookup.Views;
using RevitLookup.Views.Pages;
using Wpf.Ui;
using MockDashboardViewModel = RevitLookup.UI.Demo.Mock.ViewModels.MockDashboardViewModel;
using MockEventsViewModel = RevitLookup.UI.Demo.Mock.ViewModels.MockEventsViewModel;
using MockSnoopViewModel = RevitLookup.UI.Demo.Mock.ViewModels.MockSnoopViewModel;

namespace RevitLookup.UI.Demo;

public static class HostProvider
{
    public static IHost CreateHost()
    {
        var builder = new HostApplicationBuilder(new HostApplicationBuilderSettings
        {
            ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location),
            DisableDefaults = true
        });

        //Logging
        builder.Logging.ClearProviders();
        builder.Logging.AddLoggerConfiguration();

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
        builder.Services.AddScoped<ISnoopVisualService, MockSnoopVisualService>();
        builder.Services.AddScoped<AboutView>();
        builder.Services.AddScoped<AboutViewModel>();
        builder.Services.AddScoped<DashboardView>();
        builder.Services.AddScoped<IDashboardViewModel, MockDashboardViewModel>();
        builder.Services.AddScoped<SettingsView>();
        builder.Services.AddScoped<SettingsViewModel>();
        builder.Services.AddScoped<EventsView>();
        builder.Services.AddScoped<IEventsViewModel, MockEventsViewModel>();
        builder.Services.AddScoped<SnoopView>();
        builder.Services.AddScoped<ISnoopViewModel, MockSnoopViewModel>();
        builder.Services.AddScoped<IWindow, RevitLookupView>();

        //Startup view
        builder.Services.AddTransient<ILookupService, MockLookupService>();

        return builder.Build();
    }
}