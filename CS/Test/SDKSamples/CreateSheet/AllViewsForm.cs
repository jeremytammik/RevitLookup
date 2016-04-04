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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;


namespace RevitLookup.Test.SDKSamples.CreateSheet {

    public partial class AllViewsForm : Form {
    
        private Views       m_data = null;
        private TreeNode    m_rootNode = null;
    
        public
        AllViewsForm(Views data)
        {
            m_data = data;
            InitializeComponent();
            
            InitFormControls();
        }

        /// <summary>
        /// Populate the Form's controls with thier initial values.
        /// </summary>
        private void
        InitFormControls()
        {
            m_tvViews.BeginUpdate();
            
            m_rootNode = new TreeNode("Views (all)");
            m_tvViews.Nodes.Add(m_rootNode);
            
            foreach (Autodesk.Revit.DB.View view in m_data.AllViews) {
                Autodesk.Revit.DB.ElementType objType = 
                    view.Document.GetElement(view.GetTypeId()) as Autodesk.Revit.DB.ElementType;
                if (null == objType || objType.Name.Equals("Drawing Sheet")) {
                    continue;
                }
                else {
                    AddNewViewNode(view.Name, objType.Name);
                }
            }

            m_tvViews.ExpandAll();
            m_tvViews.EndUpdate();

            foreach (string s in m_data.TitleBlockNames) {
                m_lbTitleBlocks.Items.Add(s);
            }
            
            m_lbTitleBlocks.SetSelected(0, true);   // set the first one by default
        }
        
        
        /// <summary>
        /// Find the correct spot in the tree to place this View node
        /// </summary>
        /// <param name="view">The view</param>
        /// <param name="type">The type of view</param>
        
        private void
        AddNewViewNode(string view, string type)
        {
            foreach (TreeNode t in m_rootNode.Nodes) {
                if (t.Text.Equals(type)) {
                    t.Nodes.Add(new TreeNode(view));
                    return;
                }
            }

            TreeNode categoryNode = new TreeNode(type);
            categoryNode.Nodes.Add(new TreeNode(view));
            m_rootNode.Nodes.Add(categoryNode);
        }

        private void
        OnBnClick_OK(object sender, EventArgs e)
        {
            m_data.SetSelectViewsFromNames(GetSelectedViewNames());
            m_data.SheetName = m_ebSheetName.Text;
            
            Debug.Assert(m_lbTitleBlocks.SelectedItems.Count == 1);

            m_data.SetTitleBlock(m_lbTitleBlocks.SelectedItems[0].ToString());
        }
        
        /// <summary>
        /// See which nodes in the Tree have been checked
        /// </summary>
        /// <returns>array of names of the view (as Strings)</returns>
        
        private ArrayList
        GetSelectedViewNames()
        {
            ArrayList names = new ArrayList();
            foreach (TreeNode t in m_rootNode.Nodes) {
                foreach (TreeNode n in t.Nodes) {
                    if (n.Checked && 0 == n.Nodes.Count) {  // only include child nodes
                        names.Add(n.Text);
                    }
                }
            }
            return names;
        }

        /// <summary>
        /// Check or uncheck all child nodes of a grouping (recursively)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        
        private void
        CheckNode(TreeNode node, bool check)
        {
            foreach (TreeNode t in node.Nodes) {
                t.Checked = check;
                CheckNode(t, check);
            }
        }

        /// <summary>
        /// When the user checks a TreeNode that is a Category of Views, we need to check
        /// or uncheck all the child nodes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        OnAfterCheck_TreeView(object sender, TreeViewEventArgs e)
        {
            CheckNode(e.Node, e.Node.Checked);
        }

    }
}