#region Header
//
// Copyright 2003-2015 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace RevitLookup
{

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class App : IExternalApplication
   {
      static Autodesk.Revit.DB.AddInId m_appId = new Autodesk.Revit.DB.AddInId(new Guid("356CDA5A-E6C5-4c2f-A9EF-B3222116B8C8"));
      // get the absolute path of this assembly
      static string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
      private AppDocEvents m_appDocEvents;

      public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
      {

         // Call this method explicitly in App.cs when Revit starts up because 
         // in .Net 4, the static variables will not be initialized until use them,
         Snoop.Collectors.CollectorObj.InitializeCollectors();
         AddMenu(application);
         AddAppDocEvents(application.ControlledApplication);

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
      {
         RemoveAppDocEvents();

         return Autodesk.Revit.UI.Result.Succeeded;
      }

      private void AddMenu(UIControlledApplication app)
      {
         Autodesk.Revit.UI.RibbonPanel rvtRibbonPanel = app.CreateRibbonPanel("Revit Lookup");
         PulldownButtonData data = new PulldownButtonData("Options", "Revit Lookup");

         RibbonItem item = rvtRibbonPanel.AddItem(data);
         PulldownButton optionsBtn = item as PulldownButton;

         optionsBtn.AddPushButton(new PushButtonData("HelloWorld", "Hello World...", ExecutingAssemblyPath, "RevitLookup.HelloWorld"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Db..", "Snoop DB...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopDb"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Current Selection...", "Snoop Current Selection...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopModScope"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Active View...", "Snoop Active View...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopActiveView"));
         optionsBtn.AddPushButton(new PushButtonData("Snoop Application...", "Snoop Application...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopApp"));
         optionsBtn.AddPushButton(new PushButtonData("Test Framework...", "Test Framework...", ExecutingAssemblyPath, "RevitLookup.CmdTestShell"));
         optionsBtn.AddPushButton(new PushButtonData("Events...", "Events...", ExecutingAssemblyPath, "RevitLookup.CmdEvents"));
      }

      private void AddAppDocEvents(Autodesk.Revit.ApplicationServices.ControlledApplication app)
      {
         m_appDocEvents = new AppDocEvents(app);
         m_appDocEvents.EnableEvents();
      }

      private void RemoveAppDocEvents()
      {
         m_appDocEvents.DisableEvents();
      }
   }
}
