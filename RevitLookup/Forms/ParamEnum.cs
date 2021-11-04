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
using System.Collections;
using System.Drawing.Printing;
using System.Windows.Forms;
using RevitLookup.Forms.Utils;

namespace RevitLookup.Forms
{
    public partial class ParamEnum : Form
    {
        private int _mCurrentPrintItem;

        private int[] _mMaxWidths;

        public
            ParamEnum(ArrayList labelStrs, ArrayList valueStrs)
        {
            InitializeComponent();

            // Add Load to update ListView Width
            Core.Snoop.Utils.AddOnLoadForm(this);

            // Set the column sorter for the list view
            m_colSorter = new ListViewColumnSorter();
            m_listView.ListViewItemSorter = m_colSorter;

            m_listView.BeginUpdate();

            //Debug.Assert(labelStrs.Count == valueStrs.Count);

            var len = valueStrs.Count;
            for (var i = 0; i < len; i++)
            {
                var lvItem = new ListViewItem((string) labelStrs[i]);
                lvItem.SubItems.Add((string) valueStrs[i]);
                m_listView.Items.Add(lvItem);
            }

            m_listView.EndUpdate();
        }

        private void m_bnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            Dispose();
        }


        #region Events

        /// <summary>
        ///     Sort the columns according to the column sorter object mandate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == m_colSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (m_colSorter.Order == SortOrder.Ascending)
                    m_colSorter.Order = SortOrder.Descending;
                else
                    m_colSorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                m_colSorter.SortColumn = e.Column;
                m_colSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            m_listView.Sort();
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_listView.SelectedItems.Count > 0)
                Core.Snoop.Utils.CopyToClipboard(m_listView.SelectedItems[0], true);
            else
                Clipboard.Clear();
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            _mCurrentPrintItem = Core.Snoop.Utils.Print("", m_listView, e, _mMaxWidths[0], _mMaxWidths[1], _mCurrentPrintItem);
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            PrintMenuItem_Click(object sender, EventArgs e)
        {
            Core.Snoop.Utils.UpdatePrintSettings(m_listView, ref _mMaxWidths);
            Core.Snoop.Utils.PrintMenuItemClick(m_printDialog);
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            PrintPreviewMenuItem_Click(object sender, EventArgs e)
        {
            Core.Snoop.Utils.UpdatePrintSettings(m_listView, ref _mMaxWidths);
            Core.Snoop.Utils.PrintPreviewMenuItemClick(m_printPreviewDialog, m_listView);
        }

        #endregion
    }
}