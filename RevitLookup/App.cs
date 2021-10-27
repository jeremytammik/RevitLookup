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

using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.RevitUtils;
using RevitLookup.Snoop;

namespace RevitLookup
{
    public class App : IExternalApplication
    {
        private static AddInId _mAppId = new(new Guid("356CDA5A-E6C5-4c2f-A9EF-B3222116B8C8"));

        private AppDocEvents _mAppDocEvents;

        public Result OnStartup(UIControlledApplication application)
        {
            ModelessWindowHandle.RevitMainWindowHandle = application.MainWindowHandle;
            ExternalExecutor.CreateExternalEvent();
            AddMenu(application);
            AddAppDocEvents(application.ControlledApplication);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            RemoveAppDocEvents();
            return Result.Succeeded;
        }

        private void AddMenu(UIControlledApplication app)
        {
            var rvtRibbonPanel = app.CreatePanel("Revit Lookup");
            var data = new PulldownButtonData("Options", "Revit Lookup");

            var item = rvtRibbonPanel.AddItem(data);
            var optionsBtn = (PulldownButton) item;

            // Add Icons to main RevitLookup Menu
            optionsBtn.Image = new BitmapImage(new Uri("pack://application:,,,/RevitLookup;component/Resources/RLookup-16.png"));
            optionsBtn.LargeImage = new BitmapImage(new Uri("pack://application:,,,/RevitLookup;component/Resources/RLookup-32.png"));
            optionsBtn.AddPushButton(typeof(HelloWorld), "HelloWorld", "Hello World...");
            optionsBtn.AddPushButton(typeof(CmdSnoopDb), "Snoop Db..", "Snoop DB...");
            optionsBtn.AddPushButton(typeof(CmdSnoopModScope), "Snoop Current Selection...", "Snoop Current Selection...");
            optionsBtn.AddPushButton(typeof(CmdSnoopModScopePickSurface), "Snoop Pick Face...", "Snoop Pick Face...");
            optionsBtn.AddPushButton(typeof(CmdSnoopModScopePickEdge), "Snoop Pick Edge...", "Snoop Pick Edge...");
            optionsBtn.AddPushButton(typeof(CmdSnoopModScopeLinkedElement), "Snoop Pick Linked Element...", "Snoop Linked Element...");
            optionsBtn.AddPushButton(typeof(CmdSnoopModScopeDependents), "Snoop Dependent Elements...", "Snoop Dependent Elements...");
            optionsBtn.AddPushButton(typeof(CmdSnoopActiveView), "Snoop Active View...", "Snoop Active View...");
            optionsBtn.AddPushButton(typeof(CmdSnoopApp), "Snoop Application...", "Snoop Application...");
            optionsBtn.AddPushButton(typeof(CmdSearchBy), "Search and Snoop...", "Search and Snoop...");
        }

        private void AddAppDocEvents(ControlledApplication app)
        {
            _mAppDocEvents = new AppDocEvents(app);
            _mAppDocEvents.EnableEvents();
        }

        private void RemoveAppDocEvents()
        {
            _mAppDocEvents.DisableEvents();
        }
    }
}