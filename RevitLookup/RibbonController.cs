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

using Autodesk.Revit.UI;
using Autodesk.Windows;
using RevitLookup.Commands;
using RevitLookup.Core;
using RevitLookup.Services.Contracts;
using RevitLookup.Utils;
using UIFramework;
using RibbonButton = Autodesk.Revit.UI.RibbonButton;

namespace RevitLookup;

public static class RibbonController
{
    private const string PanelName = "Revit Lookup";

    public static void CreatePanel(UIControlledApplication application, ISettingsService settingsService)
    {
        var addinPanel = application.CreatePanel("Revit Lookup");
        var splitButton = addinPanel.AddSplitButton("RevitLookupSplitButton", "RevitLookup");

        var dashboardButton = splitButton.AddPushButton<DashboardCommand>("Dashboard");
        var selectionButton = ResolveSelectionButton(settingsService, splitButton);
        var viewButton = splitButton.AddPushButton<SnoopViewCommand>("Snoop Active view");
        var documentButton = splitButton.AddPushButton<SnoopDocumentCommand>("Snoop Document");
        var databaseButton = splitButton.AddPushButton<SnoopDatabaseCommand>("Snoop Database");
        var faceButton = splitButton.AddPushButton<SnoopFaceCommand>("Snoop Face");
        var edgeButton = splitButton.AddPushButton<SnoopEdgeCommand>("Snoop Edge");
        var pointButton = splitButton.AddPushButton<SnoopPointCommand>("Snoop Point");
        var linkedButton = splitButton.AddPushButton<SnoopLinkedElementCommand>("Snoop Linked element");
        var searchButton = splitButton.AddPushButton<SearchElementsCommand>("Search Elements");
        var monitorButton = splitButton.AddPushButton<EventMonitorCommand>("Event monitor");

        dashboardButton.SetDefaultImage();
        selectionButton.SetDefaultImage();
        viewButton.SetDefaultImage();
        documentButton.SetDefaultImage();
        databaseButton.SetDefaultImage();
        faceButton.SetDefaultImage();
        edgeButton.SetDefaultImage();
        pointButton.SetDefaultImage();
        linkedButton.SetDefaultImage();
        searchButton.SetDefaultImage();
        monitorButton.SetDefaultImage();

        // Disabled by Autodesk request https://github.com/jeremytammik/RevitLookup/issues/161
        // ReloadDebugTab(settingsService);
    }

    private static void SetDefaultImage(this RibbonButton button)
    {
        button.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        button.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");
    }

    private static PushButton ResolveSelectionButton(ISettingsService settingsService, PulldownButton splitButton)
    {
        if (!settingsService.IsModifyTabAllowed)
            return splitButton.AddPushButton<SnoopSelectionCommand>("Snoop Selection");

        var modifyTab = ComponentManager.Ribbon.FindTab("Modify");
        var modifyPanel = modifyTab.CreatePanel(PanelName);
        return modifyPanel.AddPushButton<SnoopSelectionCommand>("Snoop \nSelection");
    }

    public static void ReloadDebugTab(ISettingsService settingsService)
    {
        //Synchronising the execution context
        Application.ActionEventHandler.Raise(_ =>
        {
            var ribbonControl = (RevitRibbonControl) ComponentManager.Ribbon;

            var debugTab = ribbonControl.FindTab("Debug");
            if (settingsService.IsDebugTabAllowed)
            {
                if (debugTab is null) ribbonControl.Tabs.Add(ribbonControl.DebugTab);
            }
            else
            {
                if (debugTab is not null) ribbonControl.Tabs.Remove(ribbonControl.DebugTab);
            }
        });
    }

    public static void ReloadPanels(ISettingsService settingsService)
    {
        //Synchronising the execution context
        Application.ActionEventHandler.Raise(_ =>
        {
            RibbonUtils.RemovePanel("CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%RevitLookupSplitButton", PanelName);
            RibbonUtils.RemovePanel("CustomCtrl_%Revit Lookup%RevitLookup.Commands.SnoopSelectionCommand", PanelName);

            var controlledApplication = RevitApi.CreateUiControlledApplication();
            CreatePanel(controlledApplication, settingsService);

            RibbonUtils.ReloadShortcuts();
        });
    }
}