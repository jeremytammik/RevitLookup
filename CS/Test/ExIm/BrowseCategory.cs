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