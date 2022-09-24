// Copyright 2003-2022 by Autodesk, Inc. 
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

using System.Diagnostics;
using System.IO;
using Nice3point.Revit.Toolkit.External;
using RevitLookup.Commands;
using RevitLookup.Core;
using RevitLookup.Services.Contracts;

namespace RevitLookup;

[UsedImplicitly]
public class Application : ExternalApplication
{
    public override async void OnStartup()
    {
        RevitApi.UiApplication = UiApplication;
        CreateRibbonPanel();
        await Host.StartHost();
    }

    public override async void OnShutdown()
    {
        UpdateSoftware();
        await Host.StopHost();
    }

    private void CreateRibbonPanel()
    {
        var ribbonPanel = Application.CreatePanel("Revit Lookup");
        var splitButton = ribbonPanel.AddSplitButton("RevitLookup", "RevitLookup");

        var dashboardButton = splitButton.AddPushButton<DashboardCommand>("Dashboard");
        dashboardButton.SetImage("/RevitLookup;component/Resources/Images/Icon16.png");
        dashboardButton.SetLargeImage("/RevitLookup;component/Resources/Images/Icon32.png");

        var snoopSelection = splitButton.AddPushButton<SnoopSelectionCommand>("Snoop selection");
        snoopSelection.SetImage("/RevitLookup;component/Resources/Images/Icon16.png");
        snoopSelection.SetLargeImage("/RevitLookup;component/Resources/Images/Icon32.png");
    }

    private static void UpdateSoftware()
    {
        var updateService = Host.GetService<ISoftwareUpdateService>();
        if (File.Exists(updateService.LocalFilePath)) Process.Start(updateService.LocalFilePath);
    }
}