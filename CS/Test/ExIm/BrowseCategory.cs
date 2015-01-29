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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace RevitLookup.ExIm
{
    /// <summary>
    /// presents a category option to choose from
    /// </summary>
    public partial class BrowseCategory : System.Windows.Forms.Form
    {
        /// <summary>
        /// 
        /// </summary>
        private Document m_activeDoc;
        private Category m_category;

        /// <summary>
        /// 
        /// </summary>
        public Category Category
        {
            get { return m_category;}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activeDoc"></param>
        public BrowseCategory (Document activeDoc)
        {
            m_activeDoc = activeDoc;
            m_category = null;

            InitializeComponent();
            
            m_treeView.BeginUpdate();
            InitialiseTreeView();
            m_treeView.EndUpdate();

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitialiseTreeView ()
        {
            Categories categories = m_activeDoc.Settings.Categories;
            CategoryNameMapIterator catNameMapIter = categories.ForwardIterator();
            while (catNameMapIter.MoveNext()) {
                Category tempCategory = catNameMapIter.Current as Category;
                TreeNode node = new TreeNode(tempCategory.Name);
                node.Tag = tempCategory;
                m_treeView.Nodes.Add(node);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_treeView_AfterSelect (object sender, TreeViewEventArgs e)
        {
            m_category = e.Node.Tag as Category;
        }

    }
}