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
        public static readonly Autodesk.Revit.ApplicationServices.Application Application = null;

        protected override void EnableEventsImp()
        {
            MessageBox.Show("Application Events Turned On ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.DocumentClosed += DocumentClosed;
            Application.DocumentClosing += DocumentClosing;
            Application.DocumentOpened += DocumentOpened;
            Application.DocumentOpening += DocumentOpening;
            Application.DocumentCreated += DocumentCreated;
            Application.DocumentCreating += DocumentCreating;
            Application.DocumentSaved += DocumentSaved;
            Application.DocumentSaving += DocumentSaving;
            Application.DocumentSavedAs += DocumentSavedAs;
            Application.DocumentSavingAs += DocumentSavingAs;
            Application.DocumentPrinted += DocumentPrinted;
            Application.DocumentPrinting += DocumentPrinting;
            Application.DocumentSynchronizedWithCentral += DocumentSavedToCentral;
            Application.DocumentSynchronizingWithCentral += DocumentSavingToCentral;
            Application.FileExported += FileExported;
            Application.FileExporting += FileExporting;
            Application.FileImported += FileImported;
            Application.FileImporting += FileImporting;
            Application.ViewPrinted += ViewPrinted;
            Application.ViewPrinting += ViewPrinting;
        }

        protected override void DisableEventsImp()
        {
            MessageBox.Show("Application Events Turned Off ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.DocumentClosed -= DocumentClosed;
            Application.DocumentClosing -= DocumentClosing;
            Application.DocumentOpened -= DocumentOpened;
            Application.DocumentOpening -= DocumentOpening;
            Application.DocumentCreated -= DocumentCreated;
            Application.DocumentCreating -= DocumentCreating;
            Application.DocumentSaved -= DocumentSaved;
            Application.DocumentSaving -= DocumentSaving;
            Application.DocumentSavedAs -= DocumentSavedAs;
            Application.DocumentSavingAs -= DocumentSavingAs;
            Application.DocumentPrinted -= DocumentPrinted;
            Application.DocumentPrinting -= DocumentPrinting;
            Application.DocumentSynchronizedWithCentral -= DocumentSavedToCentral;
            Application.DocumentSynchronizingWithCentral -= DocumentSavingToCentral;
            Application.FileExported -= FileExported;
            Application.FileExporting -= FileExporting;
            Application.FileImported -= FileImported;
            Application.FileImporting -= FileImporting;
            Application.ViewPrinted -= ViewPrinted;
            Application.ViewPrinting -= ViewPrinting;
        }

        private void ViewPrinting(object sender, ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        private void ViewPrinted(object sender, ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        private void DocumentOpening(object sender, DocumentOpeningEventArgs e)
        {
            DisplayEvent("Document opening");
        }

        private void DocumentCreating(object sender, DocumentCreatingEventArgs e)
        {
            DisplayEvent("Document creating");
        }

        private void DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }

        private void FileImporting(object sender, FileImportingEventArgs e)
        {
            DisplayEvent("File importing");
        }

        private void FileImported(object sender, FileImportedEventArgs e)
        {
            DisplayEvent("File imported");
        }

        private void FileExporting(object sender, FileExportingEventArgs e)
        {
            DisplayEvent("File exporting");
        }

        private void FileExported(object sender, FileExportedEventArgs e)
        {
            DisplayEvent("File exported");
        }

        private void DocumentSavingToCentral(object sender, DocumentSynchronizingWithCentralEventArgs e)
        {
            DisplayEvent("Document saving to central");
        }

        private void DocumentSavingAs(object sender, DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        private void DocumentSaving(object sender, DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        private void DocumentSavedToCentral(object sender, DocumentSynchronizedWithCentralEventArgs e)
        {
            DisplayEvent("Document saved to central");
        }

        private void DocumentPrinting(object sender, DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        private void DocumentPrinted(object sender, DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        private void DocumentCreated(object sender, DocumentCreatedEventArgs e)
        {
            DisplayEvent("Document created");
        }

        private void DocumentSavedAs(object sender, DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        private void DocumentSaved(object sender, DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        private void DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            DisplayEvent("Document opened");
        }

        private void DialogBoxShowing(object sender, DialogBoxShowingEventArgs e)
        {
            DisplayEvent("Dialog Box");
        }

        private void DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            DisplayEvent("Document closed");
        }

        private void DisplayEvent(string eventStr)
        {
            MessageBox.Show($"Event: {eventStr}", "Application Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}