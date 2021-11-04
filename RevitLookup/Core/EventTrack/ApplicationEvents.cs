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

using System.Windows.Forms;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI.Events;

namespace RevitLookup.Core.EventTrack
{
    /// <summary>
    ///     Bring up a simple MessageBox to show that we trapped a given event.
    /// </summary>
    public class ApplicationEvents : EventsBase
    {
        public static Autodesk.Revit.ApplicationServices.Application MApp = null;

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
            DisableEventsImp()
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

        private void m_app_ViewPrinting(object sender, ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        private void m_app_ViewPrinted(object sender, ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        private void m_app_DocumentOpening(object sender, DocumentOpeningEventArgs e)
        {
            DisplayEvent("Document opening");
        }

        private void m_app_DocumentCreating(object sender, DocumentCreatingEventArgs e)
        {
            DisplayEvent("Document creating");
        }

        private void m_app_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }

        private void m_app_FileImporting(object sender, FileImportingEventArgs e)
        {
            DisplayEvent("File importing");
        }

        private void m_app_FileImported(object sender, FileImportedEventArgs e)
        {
            DisplayEvent("File imported");
        }

        private void m_app_FileExporting(object sender, FileExportingEventArgs e)
        {
            DisplayEvent("File exporting");
        }

        private void m_app_FileExported(object sender, FileExportedEventArgs e)
        {
            DisplayEvent("File exported");
        }

        private void m_app_DocumentSavingToCentral(object sender, DocumentSynchronizingWithCentralEventArgs e)
        {
            DisplayEvent("Document saving to central");
        }

        private void m_app_DocumentSavingAs(object sender, DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        private void m_app_DocumentSaving(object sender, DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        private void m_app_DocumentSavedToCentral(object sender, DocumentSynchronizedWithCentralEventArgs e)
        {
            DisplayEvent("Document saved to central");
        }

        private void m_app_DocumentPrinting(object sender, DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        private void m_app_DocumentPrinted(object sender, DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        private void m_app_DocumentCreated(object sender, DocumentCreatedEventArgs e)
        {
            DisplayEvent("Document created");
        }

        private void m_app_DocumentSavedAs(object sender, DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        private void m_app_DocumentSaved(object sender, DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        private void m_app_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            DisplayEvent("Document opened");
        }

        private void m_app_DialogBoxShowing(object sender, DialogBoxShowingEventArgs e)
        {
            DisplayEvent("Dialog Box");
        }

        private void m_app_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            DisplayEvent("Document closed");
        }

        private void DisplayEvent(string eventStr)
        {
            MessageBox.Show($"Event: {eventStr}", "Application Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}