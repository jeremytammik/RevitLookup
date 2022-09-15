// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using RevitLookup.UI.Mvvm.Contracts;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Tests;

public sealed partial class App
{
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await Host.StartHost();
        var window = Host.GetService<INavigationWindow>();
        window.ShowWindow();
        window.Navigate(typeof(DashboardView));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await Host.StopHost();
    }
}