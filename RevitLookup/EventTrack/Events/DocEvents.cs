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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;

namespace RevitLookup.EventTrack.Events
{

    /// <summary>
    /// Bring up a simple MessageBox to show that we trapped a given event.
    /// </summary>

    public class DocEvents : EventsBase
    {

        static public DocumentSet m_docSet = null;

        public
        DocEvents()
        {
        }

        protected override void
        EnableEventsImp()
        {
            MessageBox.Show("Document Events Turned On ...", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (Document doc in m_docSet)
            {
                EnableEvents(doc);
            }
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

            foreach (Document doc in m_docSet)
            {
                DisableEvents(doc);
            }
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

        void doc_ViewPrinting(object sender, Autodesk.Revit.DB.Events.ViewPrintingEventArgs e)
        {
            DisplayEvent("View printing");
        }

        void doc_ViewPrinted(object sender, Autodesk.Revit.DB.Events.ViewPrintedEventArgs e)
        {
            DisplayEvent("View printed");
        }

        void doc_DocumentSavingAs(object sender, Autodesk.Revit.DB.Events.DocumentSavingAsEventArgs e)
        {
            DisplayEvent("Document saving as");
        }

        void doc_DocumentSaving(object sender, Autodesk.Revit.DB.Events.DocumentSavingEventArgs e)
        {
            DisplayEvent("Document saving");
        }

        void doc_DocumentSavedAs(object sender, Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs e)
        {
            DisplayEvent("Document saved as");
        }

        void doc_DocumentSaved(object sender, Autodesk.Revit.DB.Events.DocumentSavedEventArgs e)
        {
            DisplayEvent("Document saved");
        }

        void doc_DocumentPrinting(object sender, Autodesk.Revit.DB.Events.DocumentPrintingEventArgs e)
        {
            DisplayEvent("Document printing");
        }

        void doc_DocumentPrinted(object sender, Autodesk.Revit.DB.Events.DocumentPrintedEventArgs e)
        {
            DisplayEvent("Document printed");
        }

        void doc_DocumentClosing(object sender, Autodesk.Revit.DB.Events.DocumentClosingEventArgs e)
        {
            DisplayEvent("Document closing");
        }       
        
        private void
        DisplayEvent(string eventStr)
        {
            MessageBox.Show(string.Format("Event: {0}", eventStr), "Document Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
