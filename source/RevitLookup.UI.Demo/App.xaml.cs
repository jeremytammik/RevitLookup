// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.IO;
using System.Reflection;
using System.Windows;
using RevitLookup.Services.Contracts;
using RevitLookup.Views.Pages;

namespace RevitLookup.UI.Demo;

public sealed partial class App
{
    private string _revitPath;

    private void OnStartup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        var host = HostProvider.CreateHost();

        Host.StartProxy(host);
        Host.GetService<ILookupService>().Show<DashboardView>();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        var settingsService = Host.GetService<ISettingsService>();
        settingsService.SaveSettings();

        Host.Stop();
    }

    private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        var path = _revitPath ?? $@"C:\Program Files\Autodesk\Revit 20{assemblyName.Version!.Major}";
        if (!Directory.Exists(path)) return null;
        _revitPath = path;

        return Directory.EnumerateFiles(_revitPath, $"{assemblyName.Name}.dll")
            .Select(Assembly.LoadFrom)
            .First();
    }
}