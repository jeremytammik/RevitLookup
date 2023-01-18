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
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autodesk.Windows;
using CommunityToolkit.Mvvm.Input;
using Nice3point.Revit.Toolkit.External;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitLookup.Commands;
using RevitLookup.Core;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RibbonButton = Autodesk.Windows.RibbonButton;
using RibbonPanel = Autodesk.Windows.RibbonPanel;

namespace RevitLookup;

[UsedImplicitly]
public class Application : ExternalApplication
{
    public static AsyncEventHandler<IReadOnlyList<SnoopableObject>> ExternalElementHandler { get; private set; }
    public static AsyncEventHandler<IReadOnlyList<Descriptor>> ExternalDescriptorHandler { get; private set; }

    public override async void OnStartup()
    {
        RevitApi.UiApplication = UiApplication;

        ExternalElementHandler = new AsyncEventHandler<IReadOnlyList<SnoopableObject>>();
        ExternalDescriptorHandler = new AsyncEventHandler<IReadOnlyList<Descriptor>>();

        CreateRibbonPanel();
        await Host.StartHost();
    }

    public override async void OnShutdown()
    {
        SaveSettings();
        UpdateSoftware();
        await Host.StopHost();
    }

    private static void UpdateSoftware()
    {
        var updateService = Host.GetService<ISoftwareUpdateService>();
        if (File.Exists(updateService.LocalFilePath)) Process.Start(updateService.LocalFilePath);
    }

    private static void SaveSettings()
    {
        var settingsService = Host.GetService<ISettingsService>();
        settingsService.Save();
    }

    private void CreateRibbonPanel()
    {
        var ribbonPanel = Application.CreatePanel("Revit Lookup");

        var dashboardButton = ribbonPanel.AddPushButton<DashboardCommand>("Dashboard");
        dashboardButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        dashboardButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton = ribbonPanel.AddSplitButton("RevitLookup", "RevitLookup");

        var snoopSelection = splitButton.AddPushButton<SnoopSelectionCommand>("Snoop selection");
        snoopSelection.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        snoopSelection.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var snoopDocument = splitButton.AddPushButton<SnoopDocumentCommand>("Snoop document");
        snoopDocument.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        snoopDocument.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var snoopDatabase = splitButton.AddPushButton<SnoopDatabaseCommand>("Snoop database");
        snoopDatabase.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        snoopDatabase.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        //Add button to modify tab
        foreach (var ribbonTab in ComponentManager.Ribbon.Tabs)
        {
            if (ribbonTab.Id != "Modify") continue;
            var modifyPanel = new RibbonPanel
            {
                Source = new RibbonPanelSource
                {
                    Title = "RevitLookup",
                    Items =
                    {
                        new RibbonButton
                        {
                            ShowText = true,
                            Text = "Snoop\nselection",
                            Size = RibbonItemSize.Large,
                            Orientation = Orientation.Vertical,
                            CommandHandler = new RelayCommand(() => SnoopSelectionCommand.Execute(RevitApi.UiApplication)),
                            Image = new BitmapImage(new Uri(@"/RevitLookup;component/Resources/Images/RibbonIcon16.png", UriKind.RelativeOrAbsolute)),
                            LargeImage = new BitmapImage(new Uri(@"/RevitLookup;component/Resources/Images/RibbonIcon32.png", UriKind.RelativeOrAbsolute)),
                        }
                    }
                }
            };

            ribbonTab.Panels.Add(modifyPanel);
            break;
        }
    }
}