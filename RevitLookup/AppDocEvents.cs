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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.ApplicationServices;

namespace RevitLookup
{
    public class AppDocEvents
    {
       private readonly ControlledApplication _mApp;

       public AppDocEvents(ControlledApplication app)
        {
            _mApp = app;
        }

        public void EnableEvents()
        {
            _mApp.DocumentClosed   += m_app_DocumentClosed;
            _mApp.DocumentOpened   += m_app_DocumentOpened;
            _mApp.DocumentSaved    += m_app_DocumentSaved;
            _mApp.DocumentSavedAs  += m_app_DocumentSavedAs;            
        }        

        public void DisableEvents()
        {
            _mApp.DocumentClosed   -= m_app_DocumentClosed;
            _mApp.DocumentOpened   -= m_app_DocumentOpened;
            _mApp.DocumentSaved    -= m_app_DocumentSaved;
            _mApp.DocumentSavedAs  -= m_app_DocumentSavedAs;            
        }

        private void m_app_DocumentSavedAs(object sender, DocumentSavedAsEventArgs e)
        {
        }

        private void m_app_DocumentSaved(object sender, DocumentSavedEventArgs e)
        {
        }

        private void m_app_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            if (EventTrack.Forms.EventsForm.MDocEvents.AreEventsEnabled)
            {
                EventTrack.Forms.EventsForm.MDocEvents.EnableEvents(e.Document);
            }
        }

        private void m_app_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
        }
  
    }
}
