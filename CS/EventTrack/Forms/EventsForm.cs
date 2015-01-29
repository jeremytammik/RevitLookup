#region Header
//
// Copyright 2003-2015 by Autodesk, Inc. 
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RevitLookup.EventTrack.Forms {    

    public partial class EventsForm : Form {

        public static Events.ApplicationEvents    m_appEvents       = new Events.ApplicationEvents();
        public static Events.DocEvents            m_docEvents       = new Events.DocEvents();

        public
        EventsForm()
        {
            InitializeComponent();

            m_cbAppEventsOn.Checked = m_appEvents.AreEventsEnabled;
            m_cbDocEventsOn.Checked = m_docEvents.AreEventsEnabled;
        }

        private void
        event_OnBnOkClick(object sender, EventArgs e)
        {
            SetEventsOnOff(m_appEvents, m_cbAppEventsOn.Checked, false);
            SetEventsOnOff(m_docEvents, m_cbDocEventsOn.Checked, false);          

            this.Close();
        }

        private void
        SetEventsOnOff(Events.EventsBase eventGroup, bool onOff, bool showDetails)
        {
            if (onOff) {    // on
                if (eventGroup.AreEventsEnabled == false) {
                    eventGroup.EnableEvents();
                }
            }
            else {          // off
                if (eventGroup.AreEventsEnabled == true) {
                    eventGroup.DisableEvents();
                }
            }
            eventGroup.ShowDetails = showDetails;
        }
    }
}