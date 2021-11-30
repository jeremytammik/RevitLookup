#region Header

//
// Copyright 2003-2021 by Autodesk, Inc. 
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
//

#endregion // Header

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using RevitLookup.Commands;
using RevitLookup.Core.Snoop;
using RevitLookup.RevitUtils;

namespace RevitLookup
{
    public class Application : IExternalApplication
    {
        private ApplicationEvents _applicationEvents;

        public Result OnStartup(UIControlledApplication application)
        {
            ModelessWindowHandle.RevitMainWindowHandle = application.MainWindowHandle;
            ExternalExecutor.CreateExternalEvent();
            CreateRibbonPanel(application);
            AddAppDocEvents(application.ControlledApplication);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            RemoveAppDocEvents();
            return Result.Succeeded;
        }

        private void CreateRibbonPanel(UIControlledApplication application)
        {
            var ribbonPanel = RibbonUtils.CreateRibbonPanel(application, "Revit Lookup");
            var pullDownButton = ribbonPanel.AddPullDownButton("Options", "Revit Lookup");
            pullDownButton.SetImage("/RevitLookup;component/Resources/Images/RLookup-16.png");
            pullDownButton.SetLargeImage("/RevitLookup;component/Resources/Images/RLookup-32.png");
            pullDownButton.AddPushButton(typeof(HelloWorldCommand), "HelloWorld", "Hello World...");
            pullDownButton.AddPushButton(typeof(SnoopDbCommand), "Snoop Db..", "Snoop DB...");
            pullDownButton.AddPushButton(typeof(SnoopSelectionCommand), "Snoop Current Selection...", "Snoop Current Selection...");
            pullDownButton.AddPushButton(typeof(SnoopSurfaceCommand), "Snoop Pick Face...", "Snoop Pick Face...");
            pullDownButton.AddPushButton(typeof(SnoopPickEdgeCommand), "Snoop Pick Edge...", "Snoop Pick Edge...");
            pullDownButton.AddPushButton(typeof(SnoopLinkedElementCommand), "Snoop Pick Linked Element...", "Snoop Linked Element...");
            pullDownButton.AddPushButton(typeof(SnoopDependentsCommand), "Snoop Dependent Elements...", "Snoop Dependent Elements...");
            pullDownButton.AddPushButton(typeof(SnoopActiveViewCommand), "Snoop Active View...", "Snoop Active View...");
            pullDownButton.AddPushButton(typeof(SnoopApplicationCommand), "Snoop Application...", "Snoop Application...");
            pullDownButton.AddPushButton(typeof(SearchCommand), "Search and Snoop...", "Search and Snoop...");
        }

        private void AddAppDocEvents(ControlledApplication app)
        {
            _applicationEvents = new ApplicationEvents(app);
            _applicationEvents.EnableEvents();
        }

        private void RemoveAppDocEvents()
        {
            _applicationEvents.DisableEvents();
        }
    }
}