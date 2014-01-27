#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;

using RevitLookup.Snoop.Collectors;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitLookup.Snoop.CollectorExts
{
   /// <summary>
   /// Provide Snoop.Data for any classes related to an Application.
   /// </summary>

   public class CollectorExtApp : CollectorExt
   {
      public
      CollectorExtApp()
      {
      }

      protected override void
      CollectEvent(object sender, CollectorEventArgs e)
      {
         // cast the sender object to the SnoopCollector we are expecting
         Collector snoopCollector = sender as Collector;
         if (snoopCollector == null)
         {
            Debug.Assert(false);    // why did someone else send us the message?
            return;
         }

         // see if it is a type we are responsible for
         Autodesk.Revit.ApplicationServices.Application app = e.ObjToSnoop as Autodesk.Revit.ApplicationServices.Application;
         if (app != null)
         {
            Stream(snoopCollector.Data(), app);
            return;
         }

         // no more app options?
         //Autodesk.Revit.Options.Application appOptions = e.ObjToSnoop as Autodesk.Revit.Options.Application;
         //if (appOptions != null) {
         //    Stream(snoopCollector.Data(), appOptions);
         //    return;
         //}

         ControlledApplication contrApp = e.ObjToSnoop as Autodesk.Revit.ApplicationServices.ControlledApplication;
         if (contrApp != null)
         {
            Stream(snoopCollector.Data(), contrApp);
            return;
         }

         Autodesk.Revit.UI.ExternalCommandData extCmd = e.ObjToSnoop as Autodesk.Revit.UI.ExternalCommandData;
         if (extCmd != null)
         {
            Stream(snoopCollector.Data(), extCmd);
            return;
         }

         RibbonItem ribbonItem = e.ObjToSnoop as RibbonItem;
         if (ribbonItem != null)
         {
            Stream(snoopCollector.Data(), ribbonItem);
            return;
         }

         RibbonPanel ribbonPanel = e.ObjToSnoop as RibbonPanel;
         if (ribbonPanel != null)
         {
            Stream(snoopCollector.Data(), ribbonPanel);
            return;
         }
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.ApplicationServices.Application app)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.ApplicationServices.Application)));

         data.Add(new Snoop.Data.Object("Cities", app.Cities));
         data.Add(new Snoop.Data.Enumerable("Documents", app.Documents));
         data.Add(new Snoop.Data.Bool("Is quiescent", app.IsQuiescent));
         data.Add(new Snoop.Data.String("Language", app.Language.ToString()));
         data.Add(new Snoop.Data.Object("Object factory", app.ObjectFactory));
         data.Add(new Snoop.Data.Enumerable("Library Paths", app.GetLibraryPaths()));
         data.Add(new Snoop.Data.String("Shared Parameter File", app.SharedParametersFilename));
         data.Add(new Snoop.Data.String("Family Template Path", app.FamilyTemplatePath));
         data.Add(new Snoop.Data.String("Product", app.Product.ToString()));
         data.Add(new Snoop.Data.String("Recording journal filename", app.RecordingJournalFilename));
         data.Add(new Snoop.Data.String("Version build", app.VersionBuild));
         data.Add(new Snoop.Data.String("Version name", app.VersionName));
         data.Add(new Snoop.Data.String("Version number", app.VersionNumber));

         data.Add(new Snoop.Data.String("Current Accelerator", app.CurrentRevitServerAccelerator));
         data.Add(new Snoop.Data.String("User Name", app.Username));

         data.Add(new Snoop.Data.Double("Vertex Tolerance", app.VertexTolerance));
         data.Add(new Snoop.Data.Object("Schema", Schema.ListSchemas()));

         data.Add(new Snoop.Data.Bool("IsArchitectureEnabled", app.IsArchitectureEnabled));
         data.Add(new Snoop.Data.Bool("IsElectricalAnalysisEnabled", app.IsElectricalAnalysisEnabled));
         data.Add(new Snoop.Data.Bool("IsElectricalEnabled", app.IsElectricalEnabled));
         data.Add(new Snoop.Data.Bool("IsEnergyAnalysisEnabled", app.IsEnergyAnalysisEnabled));
         data.Add(new Snoop.Data.Bool("IsMassingEnabled", app.IsMassingEnabled));
         data.Add(new Snoop.Data.Bool("IsMechanicalAnalysisEnabled", app.IsMechanicalAnalysisEnabled));
         data.Add(new Snoop.Data.Bool("IsMechanicalEnabled", app.IsMechanicalEnabled));
         data.Add(new Snoop.Data.Bool("IsPipingAnalysisEnabled", app.IsPipingAnalysisEnabled));
         data.Add(new Snoop.Data.Bool("IsPipingEnabled", app.IsPipingEnabled));
         data.Add(new Snoop.Data.Bool("IsStructuralAnalysisEnabled", app.IsStructuralAnalysisEnabled));
         data.Add(new Snoop.Data.Bool("IsStructureEnabled", app.IsStructureEnabled));
         data.Add(new Snoop.Data.Bool("IsSystemsEnabled", app.IsSystemsEnabled));
         

      }

      // no more app options? MM
      //private void
      //Stream(ArrayList data, Autodesk.Revit.ApplicationServices.Application appOptions)
      //{
      //    data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.Options.Application)));

      //    data.Add(new Snoop.Data.String("Shared parameters filename", appOptions.SharedParametersFilename));

      //    data.Add(new Snoop.Data.CategorySeparator("Library Paths"));

      //    StringStringMapIterator iter = appOptions.LibraryPaths.ForwardIterator();
      //    while (iter.MoveNext())
      //        data.Add(new Snoop.Data.String(iter.Key, iter.Current.ToString()));
      //}

      private void
      Stream(ArrayList data, ControlledApplication contrApp)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ControlledApplication)));

         data.Add(new Snoop.Data.Enumerable("Cities", contrApp.Cities));
         data.Add(new Snoop.Data.Object("Create", contrApp.Create));
         data.Add(new Snoop.Data.Bool("Is quiescent", contrApp.IsQuiescent));
         // no more in 2011: data.Add(new Snoop.Data.Object("Options", contrApp));
         data.Add(new Snoop.Data.Enumerable("Library Paths", contrApp.GetLibraryPaths()));
         data.Add(new Snoop.Data.String("Shared Parameter File", contrApp.SharedParametersFilename));
         data.Add(new Snoop.Data.String("Product", contrApp.Product.ToString()));
         data.Add(new Snoop.Data.String("Version build", contrApp.VersionBuild));
         data.Add(new Snoop.Data.String("Version name", contrApp.VersionName));
         data.Add(new Snoop.Data.String("Version number", contrApp.VersionNumber));

      }

      private void
      Stream(ArrayList data, Autodesk.Revit.UI.ExternalCommandData extCmd)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.UI.ExternalCommandData)));

         data.Add(new Snoop.Data.Object("Application", extCmd.Application));
         data.Add(new Snoop.Data.Enumerable("Data", extCmd.JournalData));
         data.Add(new Snoop.Data.Object("View", extCmd.View));
      }

      private void
      Stream(ArrayList data, RibbonItem ribbonItem)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RibbonItem)));

         data.Add(new Snoop.Data.Bool("Enabled", ribbonItem.Enabled));
         data.Add(new Snoop.Data.String("Item text", ribbonItem.ItemText));
         data.Add(new Snoop.Data.String("Item type", ribbonItem.ItemType.ToString()));
         data.Add(new Snoop.Data.String("Tool tip", ribbonItem.ToolTip));

         PulldownButton pullDownBtn = ribbonItem as PulldownButton;
         if (pullDownBtn != null)
         {
            Stream(data, pullDownBtn);
            return;
         }

         PushButton pushBtn = ribbonItem as PushButton;
         if (pushBtn != null)
         {
            Stream(data, pushBtn);
            return;
         }

         //RibbonMenuItem ribMenuItem = ribbonItem as RibbonMenuItem;
         //if (ribMenuItem != null) {
         //    Stream(data, ribMenuItem);
         //    return;
         //}
      }

      private void
      Stream(ArrayList data, PulldownButton pullDownBt)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PulldownButton)));

         data.Add(new Snoop.Data.Object("Image", pullDownBt.Image));
         data.Add(new Snoop.Data.Enumerable("Items", pullDownBt.GetItems()));
         data.Add(new Snoop.Data.String("Name", pullDownBt.Name));
      }

      private void
      Stream(ArrayList data, PushButton pushBt)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PushButton)));

         data.Add(new Snoop.Data.String("Assembly name", pushBt.AssemblyName));
         data.Add(new Snoop.Data.String("Class name", pushBt.ClassName));
         data.Add(new Snoop.Data.Object("Image", pushBt.Image));
         data.Add(new Snoop.Data.String("Name", pushBt.Name));
      }

      //private void
      //Stream(ArrayList data, RibbonItem ribMenuItem)
      //{
      //    data.Add(new Snoop.Data.ClassSeparator(typeof(RibbonItem)));

      //    data.Add(new Snoop.Data.Enumerable("Items", ribMenuItem.Items));
      //    data.Add(new Snoop.Data.String("Item text", ribMenuItem.ItemText));
      //}

      private void
      Stream(ArrayList data, RibbonPanel ribbonPanel)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RibbonPanel)));

         data.Add(new Snoop.Data.Bool("Enabled", ribbonPanel.Enabled));
         data.Add(new Snoop.Data.Enumerable("Items", ribbonPanel.GetItems()));
         data.Add(new Snoop.Data.String("Name", ribbonPanel.Name));
         data.Add(new Snoop.Data.String("Title", ribbonPanel.Title));
         data.Add(new Snoop.Data.Bool("Visible", ribbonPanel.Visible));
      }
   }
}
