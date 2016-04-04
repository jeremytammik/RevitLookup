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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace RevitLookup.Test.Forms
{
    /// <summary>
    /// Form with a tree view to enumerate 
    /// all the element types in the active document
    /// </summary>
    public partial class Elements : System.Windows.Forms.Form
    {
       private Autodesk.Revit.UI.UIApplication m_app;
        private Type m_selectedType;

        public Elements (Autodesk.Revit.UI.UIApplication app)
        {
            InitializeComponent();

            m_app = app;
            InitializeTreeView();
        }

        /// <summary>
        /// populate with element types from active doc
        /// </summary>
        private void
        InitializeTreeView ()
        {

            FilteredElementCollector coll = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
            coll.WherePasses(new LogicalOrFilter(new ElementIsElementTypeFilter(false), new ElementIsElementTypeFilter(true)));

            FilteredElementIterator elemIter = coll.GetElementIterator();
            //ElementIterator elemIter = m_app.ActiveUIDocument.Document.Elements;
            ArrayList elemTypes = new ArrayList();

            /// collect all unique elem types
            while (elemIter.MoveNext()) {

                /// if this elem type is already accounted for, ignore it
                if (elemTypes.Contains(elemIter.Current.GetType()))
                    continue;
                else {
                    elemTypes.Add(elemIter.Current.GetType());
                }
            }

            /// populate tree with elem types
            foreach (Type type in elemTypes) {
                TreeNode treeNode = new TreeNode(type.Name);
                treeNode.Tag = type;
                m_treeView.Nodes.Add(treeNode);
            }

            /// sort the tree
            m_treeView.TreeViewNodeSorter = new TreeSorter();
            m_treeView.Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 
        m_btnOk_Click (object sender, EventArgs e)
        {
            m_selectedType = m_treeView.SelectedNode.Tag as Type;
        }

        /// <summary>
        /// selected elem type
        /// </summary>
        public Type
        ElemTypeSelected
        {
            get
            {
                return m_selectedType;
            }
        }

    }

    /// <summary>
    /// IComparer implementation
    /// </summary>
    public class TreeSorter : IComparer
    {
        #region IComparer Members

        public int Compare (object x, object y)
        {
            TreeNode treeNode1 = x as TreeNode;
            TreeNode treeNode2 = y as TreeNode;
            return treeNode1.Text.CompareTo(treeNode2.Text);
        }

        #endregion
    }
}