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
        var splitButton = ribbonPanel.AddSplitButton("RevitLookup", "Interaction");

        var splitButton1 = splitButton.AddPushButton<DashboardCommand>("Dashboard");
        splitButton1.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton1.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton2 = splitButton.AddPushButton<SearchElementsCommand>("Search elements");
        splitButton2.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton2.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton3 = splitButton.AddPushButton<SnoopFaceCommand>("Snoop face");
        splitButton3.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton3.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton4 = splitButton.AddPushButton<SnoopEdgeCommand>("Snoop edge");
        splitButton4.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton4.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton5 = splitButton.AddPushButton<SnoopPointCommand>("Snoop point");
        splitButton5.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton5.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton6 = splitButton.AddPushButton<SnoopSubElementCommand>("Snoop sub-element");
        splitButton6.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton6.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        var splitButton7 = splitButton.AddPushButton<SnoopLinkedElementCommand>("Snoop linked element");
        splitButton7.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        splitButton7.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

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
                            LargeImage = new BitmapImage(new Uri(@"/RevitLookup;component/Resources/Images/RibbonIcon32.png", UriKind.RelativeOrAbsolute))
                        }
                    }
                }
            };

            ribbonTab.Panels.Add(modifyPanel);
            break;
        }
    }
}