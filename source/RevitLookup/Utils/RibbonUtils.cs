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

using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using RevitLookup.Core;
using UIFramework;
using UIFrameworkServices;
using RibbonButton = Autodesk.Windows.RibbonButton;
using RibbonItem = Autodesk.Revit.UI.RibbonItem;
using RibbonPanel = Autodesk.Windows.RibbonPanel;

namespace RevitLookup.Utils;

/// <summary>
///     The tools use reflection. Need to analyse changes when moving to a new API version
/// </summary>
public static class RibbonUtils
{
    public static PushButton AddPushButton<TCommand>(this RibbonPanel internalPanel, string buttonText) where TCommand : IExternalCommand, new()
    {
        var commandType = typeof(TCommand);
        var buttonType = typeof(RibbonItem);
        var buttonDataType = typeof(PushButtonData);

        var pushButtonData = new PushButtonData(commandType.FullName, buttonText, Assembly.GetAssembly(commandType).Location, commandType.FullName);
        var createMethod = buttonDataType.GetMethod("createPushButton", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)!;
        var buttonField = buttonType.GetField("m_RibbonItem", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)!;

        var button = (PushButton) createMethod.Invoke(null, [pushButtonData, false, internalPanel.Source.Id]);
        var internalButton = (RibbonButton) buttonField.GetValue(button);

        internalPanel.Source.Items.Add(internalButton);
        return button;
    }

    public static RibbonPanel CreatePanel(this RibbonTab tab, string panelName)
    {
        var panel = new RvtRibbonPanel
        {
            Source = new RibbonPanelSource
            {
                Id = panelName,
                Title = panelName
            }
        };

        tab.Panels.Add(panel);
        return panel;
    }

    public static void RemovePanel(string id, string panelName)
    {
        ComponentManager.Ribbon.FindItem(id, false, out var panel, out var tab, true);
        if (panel is null) return;

        //Remove panel
        tab.Panels.Remove(panel);

        //Remove internal history.
        //RibbonItemDictionary used to block RibbonItem re-creation
        var uiApplicationType = typeof(UIApplication);
        var ribbonItemsProperty = uiApplicationType.GetProperty("RibbonItemDictionary", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;
        var ribbonItems = (Dictionary<string, Dictionary<string, Autodesk.Revit.UI.RibbonPanel>>) ribbonItemsProperty.GetValue(RevitApi.UiApplication);
        if (ribbonItems.TryGetValue(tab.Id, out var tabItem)) tabItem.Remove(panelName);
    }

    public static void ReloadShortcuts()
    {
        //Slow shortcut reloading
        //ShortcutsHelper.ReloadCommands();

        //Fast shortcut reloading
        RevitRibbonControl.RibbonControl.ShouldJournalTabChange = false;
        var type = typeof(ShortcutsHelper);
        var methodInfo = type.GetMethod("LoadRibbonCommands", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;

        methodInfo.Invoke(null, [DocUIType.Model]);
        methodInfo.Invoke(null, [DocUIType.Project]);

        RevitRibbonControl.RibbonControl.ShouldJournalTabChange = true;
    }

    [PublicAPI]
    private static void TestPrivateMembers()
    {
        // Uncomment to check the existence of members 
        // nameof(RibbonItem.m_RibbonItem);
        // nameof(PushButtonData.createPushButton);
        // nameof(UIApplication.RibbonItemDictionary);
        // nameof(ShortcutsHelper.LoadRibbonCommands);
    }
}