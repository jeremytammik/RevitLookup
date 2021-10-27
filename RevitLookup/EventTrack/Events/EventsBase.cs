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
using System.Diagnostics;

namespace RevitLookup.EventTrack.Events
{

    public abstract class EventsBase {

        protected bool MShowDetails = false;
        protected bool MAreEventsEnabled = false;

        protected
        EventsBase()
        {
        }

        public bool
        ShowDetails
        {
            get { return MShowDetails; }
            set { MShowDetails = value; }
        }


        public bool
        AreEventsEnabled
        {
            get { return MAreEventsEnabled; }
        }

        public void
        EnableEvents()
        {
            Debug.Assert(MAreEventsEnabled == false);

            EnableEventsImp();
            MAreEventsEnabled = true;
        }

        public void
        DisableEvents()
        {
            Debug.Assert(MAreEventsEnabled == true);

            DisableEventsImp();
            MAreEventsEnabled = false;
        }

            // override these to enable and diable events in derived classes
        protected abstract void EnableEventsImp();
        protected abstract void DisableEventsImp();

    }
}
