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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;

namespace RevitLookup.Core.EventTrack
{
    /// <summary>
    ///     Bring up a simple MessageBox to show that we trapped a given event.
    /// </summary>
    public class DocEvents : EventsBase
    {
        public static readonly DocumentSet DocSet = null;

        protected override void EnableEventsImp()
        {
            MessageBox.Show("Document Events Turned On ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (Document doc in DocSet) EnableEvents(doc);
        }

        public void EnableEvents(Document doc)
        {
            doc.DocumentClosing += DocumentClosing;
            doc.DocumentPrinted += DocumentPrinted;
            doc.DocumentPrinting += DocumentPrinting;
            doc.DocumentSaved += DocumentSaved;
            doc.DocumentSavedAs += DocumentSavedAs;
            doc.DocumentSaving += DocumentSaving;
            doc.DocumentSavingAs += DocumentSavingAs;
            doc.ViewPrinted += ViewPrinted;
            doc.ViewPrinting += ViewPrinting;
        }

        protected override void DisableEventsImp()
        {
            MessageBox.Show("Document Events Turned Off ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (Document doc in DocSet) DisableEvents(doc);
        }

        public void DisableEvents(Document doc)
        {
            doc.DocumentClosing -= DocumentClosing;
            doc.DocumentPrinted -= DocumentPrinted;
            doc.DocumentPrinting -= DocumentPrinting;
            doc.DocumentSaved -= DocumentSaved;
            doc.DocumentSavedAs -= DocumentSavedAs;
            doc.DocumentSaving -= DocumentSaving;
            doc.DocumentSavingAs -= DocumentSavingAs;
            doc.ViewPrinted -= ViewPrinted;
            doc.ViewPrinting -= ViewPrinting;
        }

        private void ViewPrinting(object sender, ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        private void ViewPrinted(object sender, ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        private void DocumentSavingAs(object sender, DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        private void DocumentSaving(object sender, DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        private void DocumentSavedAs(object sender, DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        private void DocumentSaved(object sender, DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        private void DocumentPrinting(object sender, DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        private void DocumentPrinted(object sender, DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        private void DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }

        private void DisplayEvent(string eventStr)
        {
            MessageBox.Show($"Event: {eventStr}", "Document Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}