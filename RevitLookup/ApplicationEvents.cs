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

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using RevitLookup.Forms;

namespace RevitLookup
{
    public class ApplicationEvents
    {
        private readonly ControlledApplication _application;

        public ApplicationEvents(ControlledApplication app)
        {
            _application = app;
        }

        public void EnableEvents()
        {
            _application.DocumentClosed += DocumentClosed;
            _application.DocumentOpened += DocumentOpened;
            _application.DocumentSaved += DocumentSaved;
            _application.DocumentSavedAs += DocumentSavedAs;
        }

        public void DisableEvents()
        {
            _application.DocumentClosed -= DocumentClosed;
            _application.DocumentOpened -= DocumentOpened;
            _application.DocumentSaved -= DocumentSaved;
            _application.DocumentSavedAs -= DocumentSavedAs;
        }

        private void DocumentSavedAs(object sender, DocumentSavedAsEventArgs e)
        {
        }

        private void DocumentSaved(object sender, DocumentSavedEventArgs e)
        {
        }

        private void DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            if (EventsForm.MDocEvents.AreEventsEnabled) EventsForm.MDocEvents.EnableEvents(e.Document);
        }

        private void DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
        }
    }
}