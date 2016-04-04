#region Header
//
// Copyright 2003-2016 by Autodesk, Inc. 
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
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

using Autodesk.Revit.DB;

using Revit = Autodesk.Revit;



namespace RevitLookup.Test
{
   class TestImportExport : RevitLookupTestFuncs
   {

      public TestImportExport(Autodesk.Revit.UI.UIApplication app)
         : base(app)
      {
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Export to DWG", "Write out a set of Views to DWG format", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(ExportToDwg), RevitLookupTestFuncInfo.TestType.Other));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Export to 2D DWF", "Write out a set of Views to 2D DWF format", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(ExportTo2dDwf), RevitLookupTestFuncInfo.TestType.Other));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Export to 3D DWF", "Write out a set of Views to 3D DWF format", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(ExportTo3dDwf), RevitLookupTestFuncInfo.TestType.Other));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Import DWG", "Import a file of type DWG", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(ImportDwg), RevitLookupTestFuncInfo.TestType.Other));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Import Image", "Import an Image file", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(ImportImage), RevitLookupTestFuncInfo.TestType.Other));

         m_testFuncs.Add(new RevitLookupTestFuncInfo("Publish XML", "Write out an XML file with project data", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(PublishToXML), RevitLookupTestFuncInfo.TestType.Other));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Snoop XML DOM Document", "Exercise XML DOM API", "Import/Export", new RevitLookupTestFuncInfo.TestFunc(SnoopXML), RevitLookupTestFuncInfo.TestType.Query));

      }

      public void ExportToDwg()
      {
         // get output dir to save DWGs to
         System.Windows.Forms.FolderBrowserDialog dbox = new System.Windows.Forms.FolderBrowserDialog();
         dbox.Description = "Folder to save exported DWG files to";
         dbox.ShowNewFolderButton = true;

         if (dbox.ShowDialog() == DialogResult.OK)
         {
            ViewSet viewsToExport = Utils.View.GetAvailableViewsToExport(m_revitApp.ActiveUIDocument.Document);
            List<ElementId> views = new List<ElementId>();
             foreach (Autodesk.Revit.DB.View view in viewsToExport)
             {
                 views.Add(view.Id);
             }


            DWGExportOptions opts = new DWGExportOptions();

            // The ACIS option has supposedly been fixed, will have to verify this with the latest code 
            // at a later time.
            opts.ExportOfSolids = SolidGeometry.ACIS;
            opts.TargetUnit = ExportUnit.Millimeter;

            m_revitApp.ActiveUIDocument.Document.Export(dbox.SelectedPath, "", views, opts);
         }
      }

      public void
      ExportTo2dDwf()
      {
         /// get output dir to save DWFs to
         FolderBrowserDialog dbox = new FolderBrowserDialog();
         dbox.Description = "Folder to save exported DWF files to";
         dbox.ShowNewFolderButton = true;

         try
         {
            if (dbox.ShowDialog() == DialogResult.OK)
            {

               ViewSet viewsToExport = Utils.View.GetAvailableViewsToExport(m_revitApp.ActiveUIDocument.Document);

               DWFExportOptions opts = new DWFExportOptions();
               opts.MergedViews = true;

               /// export now
               m_revitApp.ActiveUIDocument.Document.Export(dbox.SelectedPath, "", viewsToExport, opts);

               /// feedback to user
               MessageBox.Show("Done exporting to 2d Dwf!!");
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }

      public void
      ExportTo3dDwf()
      {
         // get output dir to save DWFs to
         System.Windows.Forms.FolderBrowserDialog dbox = new System.Windows.Forms.FolderBrowserDialog();
         dbox.Description = "Folder to save exported DWF files to";
         dbox.ShowNewFolderButton = true;

         try
         {
            if (dbox.ShowDialog() == DialogResult.OK)
            {

               ViewSet viewsToExport = Utils.View.GetAvailableViewsToExport(m_revitApp.ActiveUIDocument.Document);

               /// filter out only 3d views
               ViewSet views3dToExport = new ViewSet();
               foreach (Autodesk.Revit.DB.View view in viewsToExport)
               {
                  if (view.ViewType == ViewType.ThreeD)
                  {
                     views3dToExport.Insert(view);
                  }
               }

               if (views3dToExport.Size == 0)
                  throw new Exception("No files exported. Make sure you have atleast one 3d view.");

               DWFExportOptions opts = new DWFExportOptions();
               /// export now
               m_revitApp.ActiveUIDocument.Document.Export(dbox.SelectedPath, "", views3dToExport, opts);

               /// feedback to user
               MessageBox.Show("Done exporting to 3d Dwf!!");
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }

      public void
      ImportDwg()
      {
         // get input file of type DWG
         System.Windows.Forms.OpenFileDialog dbox = new System.Windows.Forms.OpenFileDialog();
         dbox.CheckFileExists = true;
         dbox.Multiselect = false;
         dbox.AddExtension = true;
         dbox.DefaultExt = "dwg";
         dbox.Filter = "DWG Files (*.dwg)|*.dwg";
         dbox.Title = "DWG file to import";

         if (dbox.ShowDialog() == DialogResult.OK)
         {
            DWGImportOptions opts = new DWGImportOptions();
            opts.Placement = ImportPlacement.Origin;

            ElementId newElementId;
            m_revitApp.ActiveUIDocument.Document.Import(dbox.FileName, opts, m_revitApp.ActiveUIDocument.ActiveView, out newElementId);
         }
      }

      public void
      ImportImage()
      {
         // get input file of type DWG
         System.Windows.Forms.OpenFileDialog dbox = new System.Windows.Forms.OpenFileDialog();
         dbox.CheckFileExists = true;
         dbox.Multiselect = false;
         dbox.AddExtension = true;
         dbox.DefaultExt = "jpg";
         dbox.Filter = "Image Files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";
         dbox.Title = "Image file to import";

         if (dbox.ShowDialog() == DialogResult.OK)
         {
            ImageImportOptions opts = new ImageImportOptions();
            opts.Placement = BoxPlacement.BottomLeft;

            Element newElement;
            m_revitApp.ActiveUIDocument.Document.Import(dbox.FileName, opts, m_revitApp.ActiveUIDocument.ActiveView, out newElement);
         }
      }



      public void
      PublishToXML()
      {
         // get output file to save XML report to
         System.Windows.Forms.SaveFileDialog dbox = new System.Windows.Forms.SaveFileDialog();
         dbox.CreatePrompt = false;
         dbox.OverwritePrompt = true;
         dbox.AddExtension = true;
         dbox.DefaultExt = "xml";
         dbox.Filter = "XML Files (*.xml)|*.xml";
         dbox.Title = "XML file to save report as";

         if (dbox.ShowDialog() == DialogResult.OK)
         {
            ModelStats.Report statReport = new ModelStats.Report();
            statReport.XmlReport(dbox.FileName, m_revitApp.ActiveUIDocument.Document);
         }
      }

      public void
      SnoopXML()
      {
         System.Windows.Forms.OpenFileDialog dbox = new System.Windows.Forms.OpenFileDialog();
         dbox.CheckFileExists = true;
         dbox.AddExtension = true;
         dbox.DefaultExt = "xml";
         dbox.Filter = "XML Files (*.xml)|*.xml";
         dbox.Multiselect = false;
         dbox.Title = "Select an XML file";

         if (dbox.ShowDialog() == DialogResult.OK)
         {
            try
            {
               System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
               xmlDoc.Load(dbox.FileName);

               Xml.Forms.Dom form = new Xml.Forms.Dom(xmlDoc);
               form.ShowDialog();
            }
            catch (System.Xml.XmlException e)
            {
               MessageBox.Show(e.Message, "XML Exception");
            }
         }
      }


   }
}
