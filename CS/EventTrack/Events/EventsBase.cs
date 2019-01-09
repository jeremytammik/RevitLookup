#region Header
//
// Copyright 2003-2019 by Autodesk, Inc. 
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
using System.Diagnostics;

namespace RevitLookup.EventTrack.Events
{

    public abstract class EventsBase {

        protected bool m_showDetails = false;
        protected bool m_areEventsEnabled = false;

        protected
        EventsBase()
        {
        }

        public bool
        ShowDetails
        {
            get { return m_showDetails; }
            set { m_showDetails = value; }
        }


        public bool
        AreEventsEnabled
        {
            get { return m_areEventsEnabled; }
        }

        public void
        EnableEvents()
        {
            Debug.Assert(m_areEventsEnabled == false);

            EnableEventsImp();
            m_areEventsEnabled = true;
        }

        public void
        DisableEvents()
        {
            Debug.Assert(m_areEventsEnabled == true);

            DisableEventsImp();
            m_areEventsEnabled = false;
        }

            // override these to enable and diable events in derived classes
        protected abstract void EnableEventsImp();
        protected abstract void DisableEventsImp();

    }
}
