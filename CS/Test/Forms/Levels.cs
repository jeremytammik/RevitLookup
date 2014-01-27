#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Test.Forms
{
    public partial class Levels : System.Windows.Forms.Form
    {
        private Autodesk.Revit.UI.UIApplication m_app;
        private Autodesk.Revit.DB.Level m_selectedLevel;

        public Levels(Autodesk.Revit.UI.UIApplication app)
        {
            InitializeComponent();

            m_app = app;

            InitializeListView();
        }

        /// <summary>
        /// populate with levels
        /// </summary>
        private void
        InitializeListView ()
        {
           FilteredElementCollector fec = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
           ElementClassFilter levelsAreWanted = new ElementClassFilter(typeof(Level));
           fec.WherePasses(levelsAreWanted);
           List<Element> elements = fec.ToElements() as List<Element>;

           foreach (Element element in elements)
           {
              Autodesk.Revit.DB.Level sysLevel = element as Autodesk.Revit.DB.Level;
              if (sysLevel != null)
              {
                 ListViewItem lev = new ListViewItem(sysLevel.Name);
                 lev.SubItems.Add(sysLevel.Elevation.ToString());
                 lev.Tag = sysLevel;

                 m_levlv.Items.Add(lev);
              }      
           }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        m_btnOk_Click (object sender, EventArgs e)
        {
            Debug.Assert((m_levlv.SelectedItems.Count > 1) == false);
                
            if (m_levlv.SelectedItems.Count != 0)
                m_selectedLevel = (Autodesk.Revit.DB.Level)m_levlv.SelectedItems[0].Tag;
            else
                m_selectedLevel = null;
        }

        /// <summary>
        /// Selected level
        /// </summary>
        public Autodesk.Revit.DB.Level
        LevelSelected
        {
            get
            {
                return m_selectedLevel;
            }
        }
    }
}