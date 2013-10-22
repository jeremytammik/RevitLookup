#region Header
//
// Copyright 2003-2013 by Autodesk, Inc. 
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using Autodesk.Revit.DB;

namespace RevitLookup.Test
{

   /// <summary>
   /// This is a generic Test "Shell" that allows you to easily add new test functions and organize
   /// them without much effort.  The Tests are divided into two basic categories, those that are
   /// organized by class hierarchy (e.g., this test applies to all objects at this level of the 
   /// class hierarchy), and Categorized tests (e.g., this test falls into the Category of "Data Extraction").
   /// The test funcs are collected and sorted into buckets that are then displayed in the correct
   /// location in the tree.
   /// </summary>

   public class TestForm : System.Windows.Forms.Form
   {

      enum ImageIconList
      {    // indicies of images to use in the tree control
         Test = 0,
         Category = 1,
         Class = 2,
      };

      private System.Windows.Forms.Label m_txtDesc;
      private System.Windows.Forms.Button m_bnOk;
      private System.Windows.Forms.Button m_bnCancel;
      private System.Windows.Forms.ImageList m_imageList;

      private System.ComponentModel.IContainer components;

      private ArrayList m_treeNodes = new ArrayList();
      private ArrayList m_classTypes = new ArrayList();
      private ArrayList m_funcBuckets = new ArrayList();  // The original list of TestFuncs sorted into their Tree location
      private ArrayList m_curFuncBucket = new ArrayList();
      private ArrayList m_placedClasses = new ArrayList();
      private ArrayList m_treeItems = new ArrayList();

      private SplitContainer m_splitContainer;
      private ListView m_lvData;
      private ColumnHeader m_testName;
      private ColumnHeader m_categoryName;
      private ColumnHeader m_type;
      private ColumnHeader m_description;

      private RevitLookupTestFuncInfo m_pickedTestFunc = null;
      //private Rvt.Utils.Controls.TreeViewWithSearch m_treeViewWithSearch;

      private RevitLookup.Utils.ListViewColumnSorter m_colSorter;
      private TreeView m_treeView;
      private CheckBox m_checkBxBaseTestsInclude;
      // tree node to remember last selected node
      private static TreeNode m_memoryNode = null;
      // list view item key to remember last selected test
      private static string m_itemKey = string.Empty;
      // to remember last option
      private static bool m_includeBaseTests = true;

      /// <summary>
      /// Generic dialog that will display and organize a set of test functions
      /// </summary>
      /// <param name="testFuncs">Array of TestFuncInfo objects</param>

      public
      TestForm(ArrayList testFuncs)
      {
         InitializeComponent();

         // Set the column sorter for the list view
         m_colSorter = new RevitLookup.Utils.ListViewColumnSorter();
         m_lvData.ListViewItemSorter = m_colSorter;

         m_treeView.BeginUpdate();

         // sort the TestFuncInfo objects into buckets
         foreach (RevitLookupTestFuncs testFuncGroup in testFuncs)
         {
            foreach (RevitLookupTestFuncInfo testFuncInfo in testFuncGroup.m_testFuncs)
            {
               AddFuncToBucket(testFuncInfo);
            }
         }

         BuildTree();

         m_treeView.ExpandAll();
         m_treeView.EndUpdate();

         m_treeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);

         RestoreDialogState();
      }

      /// <summary>
      /// Remember the last position selected in the tree
      /// </summary>

      void
      RestoreDialogState()
      {
         // if we can remember the last selected node then select it
         if (m_memoryNode != null)
         {
            TreeNode[] foundNodes = m_treeView.Nodes.Find(m_memoryNode.Name, true);
            if (foundNodes.Length != 0)
               m_treeView.SelectedNode = foundNodes[0];
         }

         // set to last selected option
         m_checkBxBaseTestsInclude.Checked = m_includeBaseTests;
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void
      Dispose(bool disposing)
      {
         if (disposing)
         {
            if (components != null)
            {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
         this.m_imageList = new System.Windows.Forms.ImageList(this.components);
         this.m_txtDesc = new System.Windows.Forms.Label();
         this.m_bnOk = new System.Windows.Forms.Button();
         this.m_bnCancel = new System.Windows.Forms.Button();
         this.m_splitContainer = new System.Windows.Forms.SplitContainer();
         this.m_treeView = new System.Windows.Forms.TreeView();
         this.m_lvData = new System.Windows.Forms.ListView();
         this.m_testName = new System.Windows.Forms.ColumnHeader();
         this.m_categoryName = new System.Windows.Forms.ColumnHeader();
         this.m_type = new System.Windows.Forms.ColumnHeader();
         this.m_description = new System.Windows.Forms.ColumnHeader();
         this.m_checkBxBaseTestsInclude = new System.Windows.Forms.CheckBox();
         this.m_splitContainer.Panel1.SuspendLayout();
         this.m_splitContainer.Panel2.SuspendLayout();
         this.m_splitContainer.SuspendLayout();
         this.SuspendLayout();
         // 
         // m_imageList
         // 
         this.m_imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_imageList.ImageStream")));
         this.m_imageList.TransparentColor = System.Drawing.Color.Transparent;
         this.m_imageList.Images.SetKeyName(0, "ImageTreeTest.bmp");
         this.m_imageList.Images.SetKeyName(1, "ImageTreeCategory.bmp");
         this.m_imageList.Images.SetKeyName(2, "ImageTreeClass.bmp");
         // 
         // m_txtDesc
         // 
         this.m_txtDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.m_txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.m_txtDesc.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.m_txtDesc.Location = new System.Drawing.Point(265, 422);
         this.m_txtDesc.Name = "m_txtDesc";
         this.m_txtDesc.Size = new System.Drawing.Size(698, 23);
         this.m_txtDesc.TabIndex = 1;
         // 
         // m_bnOk
         // 
         this.m_bnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.m_bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.m_bnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.m_bnOk.Location = new System.Drawing.Point(796, 461);
         this.m_bnOk.Name = "m_bnOk";
         this.m_bnOk.Size = new System.Drawing.Size(75, 23);
         this.m_bnOk.TabIndex = 2;
         this.m_bnOk.Text = "OK";
         // 
         // m_bnCancel
         // 
         this.m_bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.m_bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.m_bnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.m_bnCancel.Location = new System.Drawing.Point(889, 461);
         this.m_bnCancel.Name = "m_bnCancel";
         this.m_bnCancel.Size = new System.Drawing.Size(75, 23);
         this.m_bnCancel.TabIndex = 3;
         this.m_bnCancel.Text = "Cancel";
         // 
         // m_splitContainer
         // 
         this.m_splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.m_splitContainer.Location = new System.Drawing.Point(24, 12);
         this.m_splitContainer.Name = "m_splitContainer";
         // 
         // m_splitContainer.Panel1
         // 
         this.m_splitContainer.Panel1.Controls.Add(this.m_treeView);
         // 
         // m_splitContainer.Panel2
         // 
         this.m_splitContainer.Panel2.Controls.Add(this.m_lvData);
         this.m_splitContainer.Size = new System.Drawing.Size(940, 366);
         this.m_splitContainer.SplitterDistance = 239;
         this.m_splitContainer.TabIndex = 4;
         // 
         // m_treeView
         // 
         this.m_treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.m_treeView.HideSelection = false;
         this.m_treeView.ImageIndex = 0;
         this.m_treeView.ImageList = this.m_imageList;
         this.m_treeView.Location = new System.Drawing.Point(3, 3);
         this.m_treeView.Name = "m_treeView";
         this.m_treeView.SelectedImageIndex = 0;
         this.m_treeView.Size = new System.Drawing.Size(233, 360);
         this.m_treeView.TabIndex = 0;
         // 
         // m_lvData
         // 
         this.m_lvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.m_lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_testName,
            this.m_categoryName,
            this.m_type,
            this.m_description});
         this.m_lvData.FullRowSelect = true;
         this.m_lvData.GridLines = true;
         this.m_lvData.HideSelection = false;
         this.m_lvData.Location = new System.Drawing.Point(-1, 3);
         this.m_lvData.Name = "m_lvData";
         this.m_lvData.Size = new System.Drawing.Size(699, 360);
         this.m_lvData.TabIndex = 0;
         this.m_lvData.UseCompatibleStateImageBehavior = false;
         this.m_lvData.View = System.Windows.Forms.View.Details;
         this.m_lvData.SelectedIndexChanged += new System.EventHandler(this.ItemSelected);
         this.m_lvData.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnColumnClick);
         // 
         // m_testName
         // 
         this.m_testName.Text = "Test Name";
         this.m_testName.Width = 180;
         // 
         // m_categoryName
         // 
         this.m_categoryName.Text = "Category/Class";
         this.m_categoryName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.m_categoryName.Width = 101;
         // 
         // m_type
         // 
         this.m_type.Text = "Type";
         this.m_type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.m_type.Width = 96;
         // 
         // m_description
         // 
         this.m_description.Text = "Description";
         this.m_description.Width = 367;
         // 
         // m_checkBxBaseTestsInclude
         // 
         this.m_checkBxBaseTestsInclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.m_checkBxBaseTestsInclude.AutoSize = true;
         this.m_checkBxBaseTestsInclude.Checked = true;
         this.m_checkBxBaseTestsInclude.CheckState = System.Windows.Forms.CheckState.Checked;
         this.m_checkBxBaseTestsInclude.Location = new System.Drawing.Point(266, 392);
         this.m_checkBxBaseTestsInclude.Name = "m_checkBxBaseTestsInclude";
         this.m_checkBxBaseTestsInclude.Size = new System.Drawing.Size(137, 17);
         this.m_checkBxBaseTestsInclude.TabIndex = 6;
         this.m_checkBxBaseTestsInclude.Text = "Show Base Class Tests";
         this.m_checkBxBaseTestsInclude.UseVisualStyleBackColor = true;
         this.m_checkBxBaseTestsInclude.CheckedChanged += new System.EventHandler(this.OnCheckChanged_IncludeBaseClass);
         // 
         // TestForm
         // 
         this.AcceptButton = this.m_bnOk;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.CancelButton = this.m_bnCancel;
         this.ClientSize = new System.Drawing.Size(980, 492);
         this.Controls.Add(this.m_checkBxBaseTestsInclude);
         this.Controls.Add(this.m_splitContainer);
         this.Controls.Add(this.m_bnCancel);
         this.Controls.Add(this.m_bnOk);
         this.Controls.Add(this.m_txtDesc);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "TestForm";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Tests";
         this.m_splitContainer.Panel1.ResumeLayout(false);
         this.m_splitContainer.Panel2.ResumeLayout(false);
         this.m_splitContainer.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }


      #endregion

      #region Tree

      /// <summary>
      /// The functions have been sorted.  Walk the list of buckets and add them to the appropriate
      /// location in the TreeControl
      /// </summary>

      private void
      BuildTree()
      {
         TreeNode categoryItem, objHierItem;

         // extablish the two basic roots of the tree
         categoryItem = AddTreeItem("Categories", null, null, ImageIconList.Test);
         objHierItem = AddTreeItem("Object Hierarchy", null, null, ImageIconList.Test);

         // For the Object Hierarchy section of the tree, we need to add in the root
         // type to anchor us (for us, the root only makes sense as APIObject)
         System.Type rootType = typeof(APIObject);

         TreeNode rootNode = new TreeNode(rootType.Name);
         rootNode.ImageIndex = (int)ImageIconList.Class;
         rootNode.SelectedImageIndex = (int)ImageIconList.Class;
         rootNode.Tag = rootType.Name;
         rootNode.Tag = rootType;
         objHierItem.Nodes.Add(rootNode);

         // Add to our cached items of what we've visited
         m_classTypes.Add(rootType);
         m_treeNodes.Add(rootNode);
         m_placedClasses.Add(rootType);
         m_treeItems.Add(rootNode);

         // walk through all the TestFunc buckets and place each function
         // in the appropriate spot in the tree (sorted by class hierarchy).
         // NOTE: they've already been sorted into unique buckets so we
         // don't have to look for duplicates.
         RevitLookupTestFuncInfo tmpTestFuncInfo = null;
         ArrayList tmpBucket = null;

         int len = m_funcBuckets.Count;
         for (int i = 0; i < len; i++)
         {
            tmpBucket = (ArrayList)m_funcBuckets[i];
            tmpTestFuncInfo = (RevitLookupTestFuncInfo)tmpBucket[0];

            if (tmpTestFuncInfo.IsCategoryBased)
            {
               // just add the new Category to the tree
               AddTreeItem(tmpTestFuncInfo.Category, tmpTestFuncInfo.Category, categoryItem, ImageIconList.Category);
            }
            else
            {
               // add an entry for this classType and any intermediate type in the hierarchy
               AddNodeForClassType(tmpTestFuncInfo.ClassType);
            }
         }
      }

      /// <summary>
      /// This is the simple case of adding a root node to the tree or a simple Category-based node.
      /// Category and ParentNode can be null.
      /// </summary>
      /// <param name="nodeName">Text to display</param>
      /// <param name="category">Display string for the category</param>
      /// <param name="parentNode">Parent node to add to</param>
      /// <param name="imageIndex">Which image to display for the TreeNode</param>
      /// <returns>The newly added node</returns>

      private TreeNode
      AddTreeItem(string nodeName, string category, TreeNode parentNode, ImageIconList imageIndex)
      {
         TreeNode node = new TreeNode(nodeName);
         node.Name = nodeName;
         node.Tag = category;    // can be null

         node.ImageIndex = (int)imageIndex;
         node.SelectedImageIndex = (int)imageIndex;

         if (parentNode == null)
            m_treeView.Nodes.Add(node);
         else
            parentNode.Nodes.Add(node);

         return node;
      }

      /// <summary>
      /// Given a classType, add a node to the tree in the correct spot to house
      /// the tests from this level of the hierarchy.
      /// </summary>
      /// <param name="classType"></param>
      /// <returns></returns>

      private TreeNode AddNodeForClassType(System.Type classType)
      {
         // if we've already placed this guy, then we already have
         // a tree node available
         int indexFoundAt = m_classTypes.IndexOf(classType);
         if (indexFoundAt >= 0)
            return (TreeNode)m_treeItems[indexFoundAt];

         // we haven't placed it, so we need to generate all the nodes
         // (if they haven't been yet) between us and the root.  First
         // collect all the base classes (including ourselves)
         System.Type rootType = typeof(APIObject);
         ArrayList familyTree = new ArrayList();
         do
         {
            familyTree.Add(classType);
            classType = classType.BaseType;
         } while (classType != rootType && classType != typeof(Object));

         // now add them to the tree in reverse order
         int lastParentIndex = 0;     // Root type
         TreeNode tmpNode = null;
         System.Type tmpType = null;

         int len = familyTree.Count;
         for (int i = len - 1; i >= 0; i--)
         {   // no need to compare against Autodesk.Revit.APIObject
            indexFoundAt = m_placedClasses.IndexOf(familyTree[i]);
            if (indexFoundAt < 0)
            {     // we haven't seen this one yet, add it
               tmpType = (System.Type)familyTree[i];

               TreeNode newNode = new TreeNode(tmpType.Name);
               tmpNode = (TreeNode)m_treeItems[lastParentIndex];
               newNode.Name = tmpType.Name;
               newNode.Tag = tmpType;
               newNode.ImageIndex = (int)ImageIconList.Class;
               newNode.SelectedImageIndex = (int)ImageIconList.Class;
               tmpNode.Nodes.Add(newNode);

               m_placedClasses.Add(tmpType);
               m_treeItems.Add(newNode);

               lastParentIndex = m_placedClasses.Count - 1;
            }
            else
            {
               lastParentIndex = indexFoundAt;
            }
         }
         return (TreeNode)m_treeItems[lastParentIndex];
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      void
      TreeView_AfterSelect(object sender, TreeViewEventArgs e)
      {
         ProcessAfterSelect(e.Node);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="treeNode"></param>

      void
      ProcessAfterSelect(TreeNode treeNode)
      {
         if (treeNode != null)
         {
            // remember this node for later
            m_memoryNode = treeNode;

            // don't sort initially, just insert according to the hierarchy
            m_colSorter.Order = SortOrder.None;
            m_txtDesc.Text = string.Empty;

            if (treeNode.Tag == null)
            {
               m_pickedTestFunc = null;
               m_curFuncBucket.Clear();
            }
            else
            {  // see if it is Category based or Hierarchy based
               string categoryStr = treeNode.Tag as string;
               if (categoryStr == null)    // if tag is a class type
                  GetTests((System.Type)treeNode.Tag, m_checkBxBaseTestsInclude.Checked);
               else
                  GetTests(categoryStr);
            }
            DisplayCurrent();
         }
      }

      /// <summary>
      /// Recursive function to get tests from a type and its base types
      /// </summary>
      /// <param name="type">the type to get tests from</param>
      /// <param name="includeBaseType">switch to turn ON for including base classes</param>

      private void
      GetTests(Type type, bool includeBaseType)
      {
         if (type == null)
            return;

         foreach (ArrayList tmpBucket in m_funcBuckets)
         {
            foreach (RevitLookupTestFuncInfo tempInfo in tmpBucket)
            {
               if (tempInfo.IsCategoryBased == false)
               {
                  if (tempInfo.ClassType.FullName == type.FullName)
                  {
                     // base classes should appear before derived,
                     // so manipulate the order
                     m_curFuncBucket.Insert(0, tempInfo);
                  }
               }
            }
         }

         if (includeBaseType)
         {
            GetTests(type.BaseType, true);
         }
      }

      /// <summary>
      /// Walk the list of Function Buckets and collect all the functions in this bucket
      /// </summary>
      /// <param name="categoryStr">Category to get tests for</param>

      private void
      GetTests(string categoryStr)
      {
         foreach (ArrayList tmpBucket in m_funcBuckets)
         {
            foreach (RevitLookupTestFuncInfo tempInfo in tmpBucket)
            {
               if (tempInfo.IsCategoryBased && (tempInfo.Category == categoryStr))
               {
                  m_curFuncBucket.Insert(0, tempInfo);
               }
            }
         }
      }

      #endregion

      /// <summary>
      /// Sort the in-coming collection of tests in to like-buckets.
      /// </summary>
      /// <param name="newTestFuncInfo">the test to find a bucket for</param>

      private void
      AddFuncToBucket(RevitLookupTestFuncInfo newTestFuncInfo)
      {
         RevitLookupTestFuncInfo tempFuncInfo = null;
         ArrayList tmpBucket = null;

         // walk the list of buckets.  If we see one already there that we belong
         // to, just add ourselves.  If not, then add a new bucket with the passed
         // in testFuncInfo as the first member.
         int len = m_funcBuckets.Count;
         for (int i = 0; i < len; i++)
         {
            tmpBucket = (ArrayList)m_funcBuckets[i];
            tempFuncInfo = (RevitLookupTestFuncInfo)tmpBucket[0]; // get the first element to compare against

            if (newTestFuncInfo.IsCategoryBased)
            {
               if ((tempFuncInfo.IsCategoryBased) && (tempFuncInfo.Category.CompareTo(newTestFuncInfo.Category) == 0))
               {
                  tmpBucket.Add(newTestFuncInfo);
                  return;
               }
            }
            else
            {
               if (!(tempFuncInfo.IsCategoryBased) && (tempFuncInfo.ClassType == newTestFuncInfo.ClassType))
               {
                  tmpBucket.Add(newTestFuncInfo);
                  return;
               }
            }
         }

         // we didn't find an existing bucket, so make a new one and add this as the first item
         ArrayList newBucket = new ArrayList();
         newBucket.Add(newTestFuncInfo);
         m_funcBuckets.Add(newBucket);
      }

      #region List

      /// <summary>
      /// User has selected a node in the tree, update the ListCtrl with the appropriate tests
      /// </summary>

      private void
      DisplayCurrent()
      {
         m_lvData.BeginUpdate();
         m_lvData.Items.Clear();

         int len = m_curFuncBucket.Count;
         for (int i = 0; i < len; i++)
         {
            RevitLookupTestFuncInfo tmpTestFuncInfo = (RevitLookupTestFuncInfo)m_curFuncBucket[i];
            ListViewItem lvItem = new ListViewItem(tmpTestFuncInfo.Label);
            lvItem.Name = tmpTestFuncInfo.Label;

            if (tmpTestFuncInfo.IsCategoryBased)
            {
               lvItem.SubItems.Add(tmpTestFuncInfo.Category);
            }
            else
            {
               lvItem.SubItems.Add(tmpTestFuncInfo.ClassType.Name);
            }
            lvItem.SubItems.Add(tmpTestFuncInfo.GetTestType().ToString());
            lvItem.SubItems.Add(tmpTestFuncInfo.Description);
            lvItem.Tag = tmpTestFuncInfo;
            m_lvData.Items.Add(lvItem);
         }
         m_lvData.EndUpdate();

         // if we can remember the last selected test then select it
         if (m_itemKey.Length != 0)
         {
            if (m_lvData.Items[m_itemKey] != null)
            {
               m_lvData.FocusedItem = m_lvData.Items[m_itemKey];
               m_lvData.Items[m_itemKey].Selected = true;
            }
         }

         // remove all from current bucket for next selection
         m_curFuncBucket.Clear();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      private void
      ItemSelected(object sender, System.EventArgs e)
      {
         Debug.Assert((m_lvData.SelectedItems.Count > 1) == false);

         if (m_lvData.SelectedItems.Count != 0)
         {
            m_pickedTestFunc = (RevitLookupTestFuncInfo)m_lvData.SelectedItems[0].Tag;
            m_txtDesc.Text = m_pickedTestFunc.Description;

            // remember the key of the last selected test
            m_itemKey = m_lvData.SelectedItems[0].Name;
         }
         else
         {
            m_pickedTestFunc = null;
         }
      }

      /// <summary>
      /// Sort the columns according to the column sorter object mandate
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
            {
               m_colSorter.Order = SortOrder.Descending;
            }
            else
            {
               m_colSorter.Order = SortOrder.Ascending;
            }
         }
         else
         {
            // Set the column number that is to be sorted; default to ascending.
            m_colSorter.SortColumn = e.Column;
            m_colSorter.Order = SortOrder.Ascending;
         }

         // Perform the sort with these new sort options.
         m_lvData.Sort();
      }

      #endregion

      /// <summary>
      /// Run the Test function
      /// </summary>

      public void
      DoTest()
      {
         if (m_pickedTestFunc != null)
         {
            m_pickedTestFunc.RunTest();
         }
      }

      private void
      OnCheckChanged_IncludeBaseClass(object sender, EventArgs e)
      {
         ProcessAfterSelect(m_treeView.SelectedNode);
         m_includeBaseTests = m_checkBxBaseTestsInclude.Checked;
      }
   }
}


