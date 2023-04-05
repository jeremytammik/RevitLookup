// Copyright 2003-2023 by Autodesk, Inc. 
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
using Autodesk.Windows;
using Nice3point.Revit.Toolkit.External;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitLookup.Commands;
using RevitLookup.Core;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;
using RevitLookup.Utils;

namespace RevitLookup;

[UsedImplicitly]
public class Application : ExternalApplication
{
    public static AsyncEventHandler AsyncEventHandler { get; private set; }
    public static AsyncEventHandler<IReadOnlyCollection<SnoopableObject>> ExternalElementHandler { get; private set; }
    public static AsyncEventHandler<IReadOnlyCollection<Descriptor>> ExternalDescriptorHandler { get; private set; }

    public override async void OnStartup()
    {
        RevitApi.UiApplication = UiApplication;

        AsyncEventHandler = new AsyncEventHandler();
        ExternalElementHandler = new AsyncEventHandler<IReadOnlyCollection<SnoopableObject>>();
        ExternalDescriptorHandler = new AsyncEventHandler<IReadOnlyCollection<Descriptor>>();

        await Host.StartHost();

        CreateRibbonPanel();
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
        var splitButton = ribbonPanel.AddSplitButton("RevitLookup", "Interaction");

        var splitDashboardButton = splitButton.AddPushButton<DashboardCommand>("Dashboard");
        splitDashboardButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitDashboardButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitViewButton = splitButton.AddPushButton<SnoopViewCommand>("Snoop Active view");
        splitViewButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitViewButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitDocumentButton = splitButton.AddPushButton<SnoopDocumentCommand>("Snoop Document");
        splitDocumentButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitDocumentButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitDatabaseButton = splitButton.AddPushButton<SnoopDatabaseCommand>("Snoop Database");
        splitDatabaseButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitDatabaseButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitFaceButton = splitButton.AddPushButton<SnoopFaceCommand>("Snoop Face");
        splitFaceButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitFaceButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitEdgeButton = splitButton.AddPushButton<SnoopEdgeCommand>("Snoop Edge");
        splitEdgeButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitEdgeButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitPointButton = splitButton.AddPushButton<SnoopPointCommand>("Snoop Point");
        splitPointButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitPointButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitLinkedButton = splitButton.AddPushButton<SnoopLinkedElementCommand>("Snoop Linked element");
        splitLinkedButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitLinkedButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitSearchButton = splitButton.AddPushButton<SearchElementsCommand>("Search Elements");
        splitSearchButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitSearchButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var eventMonitorButton = splitButton.AddPushButton<EventMonitorCommand>("Event monitor");
        eventMonitorButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        eventMonitorButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        //TODO Sync with settings service
        if (false)
        {
            var snoopSelectionButton = splitButton.AddPushButton<SnoopSelectionCommand>("Snoop\nselection");
            snoopSelectionButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
            snoopSelectionButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");
        }
        else
        {
            var ribbonTab = ComponentManager.Ribbon.FindTab("Modify");
            var modifyPanel = ribbonTab.CreatePanel("Revit Lookup");

            var snoopSelectionButton = modifyPanel.AddPushButton<SnoopSelectionCommand>("Snoop\nselection");
            snoopSelectionButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
            snoopSelectionButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");
        }
    }
}