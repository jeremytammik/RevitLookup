//
// Copyright 2003-2010 by Autodesk, Inc. 
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
       private ControlledApplication m_app;

       public AppDocEvents(ControlledApplication app)
        {
            m_app = app;
        }

        public void EnableEvents()
        {
            m_app.DocumentClosed   += new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(m_app_DocumentClosed);
            m_app.DocumentOpened   += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(m_app_DocumentOpened);
            m_app.DocumentSaved    += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedEventArgs>(m_app_DocumentSaved);
            m_app.DocumentSavedAs  += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs>(m_app_DocumentSavedAs);            
        }        

        public void DisableEvents()
        {
            m_app.DocumentClosed   -= new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(m_app_DocumentClosed);
            m_app.DocumentOpened   -= new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(m_app_DocumentOpened);
            m_app.DocumentSaved    -= new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedEventArgs>(m_app_DocumentSaved);
            m_app.DocumentSavedAs  -= new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs>(m_app_DocumentSavedAs);            
        }

        void m_app_DocumentSavedAs(object sender, Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs e)
        {
        }

        void m_app_DocumentSaved(object sender, Autodesk.Revit.DB.Events.DocumentSavedEventArgs e)
        {
        }

        void m_app_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            if (EventTrack.Forms.EventsForm.m_docEvents.AreEventsEnabled)
            {
                EventTrack.Forms.EventsForm.m_docEvents.EnableEvents(e.Document);
            }
        }

        void m_app_DocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
        }
  
    }
}
