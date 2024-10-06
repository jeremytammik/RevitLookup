using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace RevitLookup.UI.Playground;

/// <summary>
///     Provides a host for the application's services and manages their lifetimes.
/// </summary>
public static class Host
{
    private static readonly IServiceProvider ServiceProvider = RegisterServices();

    /// <summary>
    ///     Gets a service of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns>A service object of type T or null if there is no such service.</returns>
    public static T GetService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    private static ServiceProvider RegisterServices()
    {
        var services = new ServiceCollection();

        //Frontend services
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IContentDialogService, ContentDialogService>();
        services.AddScoped<ISnackbarService, SnackbarService>();

        return services.BuildServiceProvider();
    }
}