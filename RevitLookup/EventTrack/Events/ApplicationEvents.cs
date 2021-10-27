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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace RevitLookup.EventTrack.Events {

    /// <summary>
    /// Bring up a simple MessageBox to show that we trapped a given event.
    /// </summary>
    
    public class ApplicationEvents : EventsBase {
       
        static public Autodesk.Revit.ApplicationServices.Application MApp = null;

        public
        ApplicationEvents()
        {
        }

        protected override void
        EnableEventsImp()
        {           
            MessageBox.Show("Application Events Turned On ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
            MApp.DocumentClosed += m_app_DocumentClosed;
            MApp.DocumentClosing += m_app_DocumentClosing;
            MApp.DocumentOpened += m_app_DocumentOpened;
            MApp.DocumentOpening += m_app_DocumentOpening;
            MApp.DocumentCreated += m_app_DocumentCreated;
            MApp.DocumentCreating += m_app_DocumentCreating;
            MApp.DocumentSaved += m_app_DocumentSaved;
            MApp.DocumentSaving += m_app_DocumentSaving;           
            MApp.DocumentSavedAs += m_app_DocumentSavedAs;
            MApp.DocumentSavingAs += m_app_DocumentSavingAs;
            MApp.DocumentPrinted += m_app_DocumentPrinted;
            MApp.DocumentPrinting += m_app_DocumentPrinting;
            MApp.DocumentSynchronizedWithCentral += m_app_DocumentSavedToCentral;                        
            MApp.DocumentSynchronizingWithCentral += m_app_DocumentSavingToCentral;
            MApp.FileExported += m_app_FileExported;
            MApp.FileExporting += m_app_FileExporting;
            MApp.FileImported += m_app_FileImported;
            MApp.FileImporting += m_app_FileImporting;
            MApp.ViewPrinted += m_app_ViewPrinted;
            MApp.ViewPrinting += m_app_ViewPrinting;                                                 
        }        

        protected override void
        DisableEventsImp ()
        {            
            MessageBox.Show("Application Events Turned Off ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            MApp.DocumentClosed -= m_app_DocumentClosed;
            MApp.DocumentClosing -= m_app_DocumentClosing;
            MApp.DocumentOpened -= m_app_DocumentOpened;
            MApp.DocumentOpening -= m_app_DocumentOpening;
            MApp.DocumentCreated -= m_app_DocumentCreated;
            MApp.DocumentCreating -= m_app_DocumentCreating;
            MApp.DocumentSaved -= m_app_DocumentSaved;
            MApp.DocumentSaving -= m_app_DocumentSaving;
            MApp.DocumentSavedAs -= m_app_DocumentSavedAs;
            MApp.DocumentSavingAs -= m_app_DocumentSavingAs;
            MApp.DocumentPrinted -= m_app_DocumentPrinted;
            MApp.DocumentPrinting -= m_app_DocumentPrinting;
            MApp.DocumentSynchronizedWithCentral -= m_app_DocumentSavedToCentral;
            MApp.DocumentSynchronizingWithCentral -= m_app_DocumentSavingToCentral;
            MApp.FileExported -= m_app_FileExported;
            MApp.FileExporting -= m_app_FileExporting;
            MApp.FileImported -= m_app_FileImported;
            MApp.FileImporting -= m_app_FileImporting;
            MApp.ViewPrinted -= m_app_ViewPrinted;
            MApp.ViewPrinting -= m_app_ViewPrinting;   
        }

        private void m_app_ViewPrinting(object sender, Autodesk.Revit.DB.Events.ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        private void m_app_ViewPrinted(object sender, Autodesk.Revit.DB.Events.ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        private void m_app_DocumentOpening(object sender, Autodesk.Revit.DB.Events.DocumentOpeningEventArgs e)
        {
            DisplayEvent("Document opening");
        }

        private void m_app_DocumentCreating(object sender, Autodesk.Revit.DB.Events.DocumentCreatingEventArgs e)
        {
            DisplayEvent("Document creating");
        }

        private void m_app_DocumentClosing(object sender, Autodesk.Revit.DB.Events.DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }

        private void m_app_FileImporting(object sender, Autodesk.Revit.DB.Events.FileImportingEventArgs e)
        {
            DisplayEvent("File importing");
        }

        private void m_app_FileImported(object sender, Autodesk.Revit.DB.Events.FileImportedEventArgs e)
        {
            DisplayEvent("File imported");
        }

        private void m_app_FileExporting(object sender, Autodesk.Revit.DB.Events.FileExportingEventArgs e)
        {
            DisplayEvent("File exporting");
        }

        private void m_app_FileExported(object sender, Autodesk.Revit.DB.Events.FileExportedEventArgs e)
        {
            DisplayEvent("File exported");
        }

        private void m_app_DocumentSavingToCentral(object sender, Autodesk.Revit.DB.Events.DocumentSynchronizingWithCentralEventArgs e)
        {
            
            DisplayEvent("Document saving to central");
        }

        private void m_app_DocumentSavingAs(object sender, Autodesk.Revit.DB.Events.DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        private void m_app_DocumentSaving(object sender, Autodesk.Revit.DB.Events.DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        private void m_app_DocumentSavedToCentral(object sender, Autodesk.Revit.DB.Events.DocumentSynchronizedWithCentralEventArgs e)
        {
            DisplayEvent("Document saved to central");
        }

        private void m_app_DocumentPrinting(object sender, Autodesk.Revit.DB.Events.DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        private void m_app_DocumentPrinted(object sender, Autodesk.Revit.DB.Events.DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        private void m_app_DocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            DisplayEvent("Document created");
        }

        private void m_app_DocumentSavedAs(object sender, Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        private void m_app_DocumentSaved(object sender, Autodesk.Revit.DB.Events.DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        private void m_app_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            DisplayEvent("Document opened");
        }

        private void m_app_DialogBoxShowing(object sender, Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e)
        {
            DisplayEvent("Dialog Box");
        }

        private void m_app_DocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            DisplayEvent("Document closed");
        }                

        private void DisplayEvent(string eventStr)
        {
            MessageBox.Show($"Event: {eventStr}", "Application Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }       
    }
}
