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
using RevitLookup.Views.Utils;

namespace RevitLookup.Views
{
    public partial class ParamEnum : Form
    {
        private int _currentPrintItem;

        private int[] _maxWidths;

        public ParamEnum(ArrayList labelStrs, ArrayList valueStrs)
        {
            InitializeComponent();

            // Add Load to update ListView Width
            Core.Utils.AddOnLoadForm(this);

            // Set the column sorter for the list view
            colSorter = new ListViewColumnSorter();
            listView.ListViewItemSorter = colSorter;

            listView.BeginUpdate();

            //Debug.Assert(labelStrs.Count == valueStrs.Count);

            var len = valueStrs.Count;
            for (var i = 0; i < len; i++)
            {
                var lvItem = new ListViewItem((string) labelStrs[i]);
                lvItem.SubItems.Add((string) valueStrs[i]);
                listView.Items.Add(lvItem);
            }

            listView.EndUpdate();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
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
            if (e.Column == colSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (colSorter.Order == SortOrder.Ascending)
                    colSorter.Order = SortOrder.Descending;
                else
                    colSorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                colSorter.SortColumn = e.Column;
                colSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listView.Sort();
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                Core.Utils.CopyToClipboard(listView.SelectedItems[0], true);
            else
                Clipboard.Clear();
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            _currentPrintItem = Core.Utils.Print("", listView, e, _maxWidths[0], _maxWidths[1], _currentPrintItem);
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            PrintMenuItem_Click(object sender, EventArgs e)
        {
            Core.Utils.UpdatePrintSettings(listView, ref _maxWidths);
            Core.Utils.PrintMenuItemClick(m_printDialog);
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
        {
            Core.Utils.UpdatePrintSettings(listView, ref _maxWidths);
            Core.Utils.PrintPreviewMenuItemClick(m_printPreviewDialog, listView);
        }

        #endregion
    }
}