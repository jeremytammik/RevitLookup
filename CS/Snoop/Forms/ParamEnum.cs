#region Header
//
// Copyright 2003-2016 by Autodesk, Inc. 
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace RevitLookup.Snoop.Forms {

    public partial class ParamEnum : Form {

        private Int32[]  m_maxWidths;
        private Int32    m_currentPrintItem = 0;
    
        public
        ParamEnum(ArrayList labelStrs, ArrayList valueStrs)
        {
            InitializeComponent();
            
                // Set the column sorter for the list view
            m_colSorter = new RevitLookup.Utils.ListViewColumnSorter( );
            m_listView.ListViewItemSorter = m_colSorter;
            
            m_listView.BeginUpdate();
            
            //Debug.Assert(labelStrs.Count == valueStrs.Count);

            int len = valueStrs.Count;
            for (int i=0; i<len; i++) {
                ListViewItem lvItem = new ListViewItem((string)labelStrs[i]);
                lvItem.SubItems.Add((string)valueStrs[i]);
                m_listView.Items.Add(lvItem);
            }
            
            m_listView.EndUpdate();
        }


        #region Events
        /// <summary>
        /// Sort the columns according to the column sorter object mandate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void
        OnColumnClick(object sender, ColumnClickEventArgs e)
        {
                // Determine if clicked column is already the column that is being sorted.
            if (e.Column == m_colSorter.SortColumn) {
                    // Reverse the current sort direction for this column.
                if (m_colSorter.Order == SortOrder.Ascending) {
                    m_colSorter.Order = SortOrder.Descending;
                }
                else {
                    m_colSorter.Order = SortOrder.Ascending;
                }
            }
            else {
                    // Set the column number that is to be sorted; default to ascending.
                m_colSorter.SortColumn = e.Column;
                m_colSorter.Order = SortOrder.Ascending;
            }

                // Perform the sort with these new sort options.
            m_listView.Sort();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        CopyToolStripMenuItem_Click (object sender, System.EventArgs e)
        {
            if (m_listView.SelectedItems.Count > 0)
            {
                Utils.CopyToClipboard(m_listView.SelectedItems[0], true);
            }
            else
            {
                Clipboard.Clear();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintDocument_PrintPage (object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            m_currentPrintItem = Utils.Print("", m_listView, e, m_maxWidths[0], m_maxWidths[1], m_currentPrintItem);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintMenuItem_Click (object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(m_listView, ref m_maxWidths);
            Utils.PrintMenuItemClick(m_printDialog);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintPreviewMenuItem_Click (object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(m_listView, ref m_maxWidths);
            Utils.PrintPreviewMenuItemClick(m_printPreviewDialog, m_listView);
        }
        #endregion
    }
}