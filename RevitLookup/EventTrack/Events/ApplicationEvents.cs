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
       
        static public Autodesk.Revit.ApplicationServices.Application m_app = null;

        public
        ApplicationEvents()
        {
        }

        protected override void
        EnableEventsImp()
        {           
            MessageBox.Show("Application Events Turned On ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
            m_app.DocumentClosed += m_app_DocumentClosed;
            m_app.DocumentClosing += m_app_DocumentClosing;
            m_app.DocumentOpened += m_app_DocumentOpened;
            m_app.DocumentOpening += m_app_DocumentOpening;
            m_app.DocumentCreated += m_app_DocumentCreated;
            m_app.DocumentCreating += m_app_DocumentCreating;
            m_app.DocumentSaved += m_app_DocumentSaved;
            m_app.DocumentSaving += m_app_DocumentSaving;           
            m_app.DocumentSavedAs += m_app_DocumentSavedAs;
            m_app.DocumentSavingAs += m_app_DocumentSavingAs;
            m_app.DocumentPrinted += m_app_DocumentPrinted;
            m_app.DocumentPrinting += m_app_DocumentPrinting;
            m_app.DocumentSynchronizedWithCentral += m_app_DocumentSavedToCentral;                        
            m_app.DocumentSynchronizingWithCentral += m_app_DocumentSavingToCentral;
            m_app.FileExported += m_app_FileExported;
            m_app.FileExporting += m_app_FileExporting;
            m_app.FileImported += m_app_FileImported;
            m_app.FileImporting += m_app_FileImporting;
            m_app.ViewPrinted += m_app_ViewPrinted;
            m_app.ViewPrinting += m_app_ViewPrinting;                                                 
        }        

        protected override void
        DisableEventsImp ()
        {            
            MessageBox.Show("Application Events Turned Off ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            m_app.DocumentClosed -= m_app_DocumentClosed;
            m_app.DocumentClosing -= m_app_DocumentClosing;
            m_app.DocumentOpened -= m_app_DocumentOpened;
            m_app.DocumentOpening -= m_app_DocumentOpening;
            m_app.DocumentCreated -= m_app_DocumentCreated;
            m_app.DocumentCreating -= m_app_DocumentCreating;
            m_app.DocumentSaved -= m_app_DocumentSaved;
            m_app.DocumentSaving -= m_app_DocumentSaving;
            m_app.DocumentSavedAs -= m_app_DocumentSavedAs;
            m_app.DocumentSavingAs -= m_app_DocumentSavingAs;
            m_app.DocumentPrinted -= m_app_DocumentPrinted;
            m_app.DocumentPrinting -= m_app_DocumentPrinting;
            m_app.DocumentSynchronizedWithCentral -= m_app_DocumentSavedToCentral;
            m_app.DocumentSynchronizingWithCentral -= m_app_DocumentSavingToCentral;
            m_app.FileExported -= m_app_FileExported;
            m_app.FileExporting -= m_app_FileExporting;
            m_app.FileImported -= m_app_FileImported;
            m_app.FileImporting -= m_app_FileImporting;
            m_app.ViewPrinted -= m_app_ViewPrinted;
            m_app.ViewPrinting -= m_app_ViewPrinting;   
        }

        void m_app_ViewPrinting(object sender, Autodesk.Revit.DB.Events.ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        void m_app_ViewPrinted(object sender, Autodesk.Revit.DB.Events.ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        void m_app_DocumentOpening(object sender, Autodesk.Revit.DB.Events.DocumentOpeningEventArgs e)
        {
            DisplayEvent("Document opening");
        }

        void m_app_DocumentCreating(object sender, Autodesk.Revit.DB.Events.DocumentCreatingEventArgs e)
        {
            DisplayEvent("Document creating");
        }

        void m_app_DocumentClosing(object sender, Autodesk.Revit.DB.Events.DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }

        void m_app_FileImporting(object sender, Autodesk.Revit.DB.Events.FileImportingEventArgs e)
        {
            DisplayEvent("File importing");
        }

        void m_app_FileImported(object sender, Autodesk.Revit.DB.Events.FileImportedEventArgs e)
        {
            DisplayEvent("File imported");
        }

        void m_app_FileExporting(object sender, Autodesk.Revit.DB.Events.FileExportingEventArgs e)
        {
            DisplayEvent("File exporting");
        }

        void m_app_FileExported(object sender, Autodesk.Revit.DB.Events.FileExportedEventArgs e)
        {
            DisplayEvent("File exported");
        }

        void m_app_DocumentSavingToCentral(object sender, Autodesk.Revit.DB.Events.DocumentSynchronizingWithCentralEventArgs e)
        {
            
            DisplayEvent("Document saving to central");
        }

        void m_app_DocumentSavingAs(object sender, Autodesk.Revit.DB.Events.DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        void m_app_DocumentSaving(object sender, Autodesk.Revit.DB.Events.DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        void m_app_DocumentSavedToCentral(object sender, Autodesk.Revit.DB.Events.DocumentSynchronizedWithCentralEventArgs e)
        {
            DisplayEvent("Document saved to central");
        }

        void m_app_DocumentPrinting(object sender, Autodesk.Revit.DB.Events.DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        void m_app_DocumentPrinted(object sender, Autodesk.Revit.DB.Events.DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        void m_app_DocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            DisplayEvent("Document created");
        }

        void m_app_DocumentSavedAs(object sender, Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        void m_app_DocumentSaved(object sender, Autodesk.Revit.DB.Events.DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        void m_app_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            DisplayEvent("Document opened");
        }

        void m_app_DialogBoxShowing(object sender, Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e)
        {
            DisplayEvent("Dialog Box");
        }

        void m_app_DocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            DisplayEvent("Document closed");
        }                

        private void
        DisplayEvent(string eventStr)
        {
            MessageBox.Show(string.Format("Event: {0}", eventStr), "Application Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }       
    }
}
