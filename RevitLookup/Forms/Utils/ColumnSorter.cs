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

using System.Collections;
using System.Windows.Forms;

namespace RevitLookup.Forms.Utils
{
    /// <summary>
    ///     This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        ///     Case insensitive comparer object
        /// </summary>
        private readonly CaseInsensitiveComparer _objectCompare;

        /// <summary>
        ///     Specifies the column to be sorted
        /// </summary>
        private int _columnToSort;

        /// <summary>
        ///     Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder _orderOfSort;

        /// <summary>
        /// </summary>
        public ListViewColumnSorter()
        {
            _columnToSort = 0;
            _orderOfSort = SortOrder.None;
            _objectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        ///     Gets or sets the number of the column
        ///     to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set => _columnToSort = value;
            get => _columnToSort;
        }

        /// <summary>
        ///     Gets or sets the order of sorting
        ///     to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set => _orderOfSort = value;
            get => _orderOfSort;
        }

        /// <summary>
        ///     This method is inherited from the IComparer interface.
        ///     Compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>
        ///     The result of the comparison.
        ///     "0" if equal, negative if 'x' is less than 'y'
        ///     and positive if 'x' is greater than 'y'
        /// </returns>
        public int Compare(object x, object y)
        {
            var listviewX = (ListViewItem) x;
            var listviewY = (ListViewItem) y;

            var compareResult = _objectCompare.Compare(listviewX.SubItems[_columnToSort].Text, listviewY.SubItems[_columnToSort].Text);

            switch (_orderOfSort)
            {
                case SortOrder.Ascending:
                    return compareResult;
                case SortOrder.Descending:
                    return -compareResult;
                default:
                    return 0;
            }
        }
    }
}