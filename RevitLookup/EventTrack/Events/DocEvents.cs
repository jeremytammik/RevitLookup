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

namespace RevitLookup.EventTrack.Events
{
    /// <summary>
    ///     Bring up a simple MessageBox to show that we trapped a given event.
    /// </summary>
    public class DocEvents : EventsBase
    {
        public static DocumentSet MDocSet = null;

        protected override void
            EnableEventsImp()
        {
            MessageBox.Show("Document Events Turned On ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (Document doc in MDocSet) EnableEvents(doc);
        }

        public void
            EnableEvents(Document doc)
        {
            doc.DocumentClosing += doc_DocumentClosing;
            doc.DocumentPrinted += doc_DocumentPrinted;
            doc.DocumentPrinting += doc_DocumentPrinting;
            doc.DocumentSaved += doc_DocumentSaved;
            doc.DocumentSavedAs += doc_DocumentSavedAs;
            doc.DocumentSaving += doc_DocumentSaving;
            doc.DocumentSavingAs += doc_DocumentSavingAs;
            doc.ViewPrinted += doc_ViewPrinted;
            doc.ViewPrinting += doc_ViewPrinting;
        }

        protected override void
            DisableEventsImp()
        {
            MessageBox.Show("Document Events Turned Off ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (Document doc in MDocSet) DisableEvents(doc);
        }

        public void
            DisableEvents(Document doc)
        {
            doc.DocumentClosing -= doc_DocumentClosing;
            doc.DocumentPrinted -= doc_DocumentPrinted;
            doc.DocumentPrinting -= doc_DocumentPrinting;
            doc.DocumentSaved -= doc_DocumentSaved;
            doc.DocumentSavedAs -= doc_DocumentSavedAs;
            doc.DocumentSaving -= doc_DocumentSaving;
            doc.DocumentSavingAs -= doc_DocumentSavingAs;
            doc.ViewPrinted -= doc_ViewPrinted;
            doc.ViewPrinting -= doc_ViewPrinting;
        }

        private void doc_ViewPrinting(object sender, ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        private void doc_ViewPrinted(object sender, ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        private void doc_DocumentSavingAs(object sender, DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        private void doc_DocumentSaving(object sender, DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        private void doc_DocumentSavedAs(object sender, DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        private void doc_DocumentSaved(object sender, DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        private void doc_DocumentPrinting(object sender, DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        private void doc_DocumentPrinted(object sender, DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        private void doc_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }

        private void
            DisplayEvent(string eventStr)
        {
            MessageBox.Show($"Event: {eventStr}", "Document Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}