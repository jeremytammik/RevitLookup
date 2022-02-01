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

using Autodesk.Revit.UI;
using RevitLookup.Commands;
using RevitLookup.Core;

namespace RevitLookup;

[UsedImplicitly]
public class Application : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        CreateRibbonPanel(application);
        ModelessWindowHandle.SetHandler(application.MainWindowHandle);
        ExternalExecutor.CreateExternalEvent();
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }

    private static void CreateRibbonPanel(UIControlledApplication application)
    {
        var ribbonPanel = application.CreatePanel("Revit Lookup");
        var pullDownButton = ribbonPanel.AddPullDownButton("Options", "Revit Lookup");
        pullDownButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        pullDownButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");
        pullDownButton.AddPushButton(typeof(HelloWorldCommand), "Hello World...");
        pullDownButton.AddPushButton(typeof(SnoopDbCommand), "Snoop DB...");
        pullDownButton.AddPushButton(typeof(SnoopSelectionCommand), "Snoop Current Selection...");
        pullDownButton.AddPushButton(typeof(SnoopSurfaceCommand), "Snoop Pick Face...");
        pullDownButton.AddPushButton(typeof(SnoopPickEdgeCommand), "Snoop Pick Edge...");
        pullDownButton.AddPushButton(typeof(SnoopLinkedElementCommand), "Snoop Linked Element...");
        pullDownButton.AddPushButton(typeof(SnoopDependentsCommand), "Snoop Dependent Elements...");
        pullDownButton.AddPushButton(typeof(SnoopActiveViewCommand), "Snoop Active View...");
        pullDownButton.AddPushButton(typeof(SnoopApplicationCommand), "Snoop Application...");
        pullDownButton.AddPushButton(typeof(SearchCommand), "Search and Snoop...");
    }
}