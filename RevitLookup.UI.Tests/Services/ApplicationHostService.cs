using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Hosting;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.UI.Tests.Services.Contracts;
using RevitLookup.UI.Tests.Views;
using RevitLookup.UI.Tests.Views.Pages;

namespace RevitLookup.UI.Tests.Services;

/// <summary>
///     Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly INavigationService _navigationService;
    private readonly IPageService _pageService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISoftwareUpdateService _updateService;

    private INavigationWindow _navigationWindow;

    public ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService, IPageService pageService, ISoftwareUpdateService updateService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _pageService = pageService;
        _updateService = updateService;
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (File.Exists(_updateService.LocalFilePath))
        {
            Process.Start(_updateService.LocalFilePath);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    ///     Creates main window during activation.
    /// </summary>
    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        _navigationService.SetPageService(_pageService);
        if (!System.Windows.Application.Current.Windows.OfType<RevitLookupView>().Any())
        {
            _navigationWindow = _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow;
            _navigationWindow!.ShowWindow();

            _navigationWindow.Navigate(typeof(DashboardView));
        }

        await Task.CompletedTask;
    }
}