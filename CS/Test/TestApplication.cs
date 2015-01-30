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

using Autodesk.Revit.DB;

namespace RevitLookup.Test
{
    class TestApplication : RevitLookupTestFuncs
    {
        public
        TestApplication (Autodesk.Revit.UI.UIApplication app)
            : base(app)
        {
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Batch Printing", "Prints all selected files in a batch process", typeof(Autodesk.Revit.ApplicationServices.Application), new RevitLookupTestFuncInfo.TestFunc(BatchPrinting), RevitLookupTestFuncInfo.TestType.Other));
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Journal Playback", "Plays back a selected journal", typeof(Autodesk.Revit.ApplicationServices.Application), new RevitLookupTestFuncInfo.TestFunc(JournalPlayBack), RevitLookupTestFuncInfo.TestType.Other));
        }

        /// <summary>
        /// Peforms a batch process on selected files
        /// </summary>
        public void
        BatchPrinting ()
        {
            try {
                /// get files to process from user
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select all files to send to printer";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "RVT Files (*.rvt)|*.rvt";

                DocumentSet docSet = new DocumentSet();

                if (openFileDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                /// create a document set of all selected files
                foreach (string fileName in openFileDialog.FileNames) {
                    /// could directly deal with doc here but...
                    /// just testing out DocumentSet
                    Document doc = m_revitApp.Application.OpenDocumentFile(fileName);
                    docSet.Insert(doc);
                }


                /// process each file in the Document set
                DocumentSetIterator docSetIter = docSet.ForwardIterator();
                while (docSetIter.MoveNext()) {

                    Document doc = docSetIter.Current as Document;

                    ViewSet views = Utils.View.GetAvailableViewsToExport(doc);

                    /// TBD: remove comments after figuring out
                    /// why doc.EndTransaction() triggers asserts,
                    /// Note that it still works , but the asserts can be a nuisance
                    /// while dealing with multiple files 01/12/07

                    ///// align text middle and center
                    //TextAlignFlags align = TextAlignFlags.TEF_ALIGN_MIDDLE ^ TextAlignFlags.TEF_ALIGN_CENTER;

                    ///// the way to add an element for a document 
                    ///// in memory is within a transaction
                    //if (doc.BeginTransaction()) {

                    //    /// to identify what is printed
                    //    foreach (Autodesk.Revit.DB.View view in views) {

                    //        XYZ origin = new XYZ(view.CropBox.Max.X, view.CropBox.Min.Y, 0.0);
                    //        doc.Create.NewTextNote(view, origin, GeomUtils.kXAxis,
                    //                               view.ViewDirection, 15.0, .25,
                    //                               align, view.ViewName);

                    //    }
                    //}

                    ///// TBD: Asserts "unnamed transaction" ... dunno why
                    ////doc.EndTransaction();
                    //doc.Save();

                    ///// print now
                    doc.Print(views);
                }

                /// feedback to user
                MessageBox.Show("Done Printing!");
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void
        JournalPlayBack ()
        {
            try {
                /// get journal file from user
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select a journal file to playback";
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "TXT Files (*.txt)|*.txt";

                if (openFileDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                System.Diagnostics.Process.Start("revit", openFileDialog.FileName);
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
    }
}

