#region Header
//
// Copyright 2003-2019 by Autodesk, Inc. 
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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Forms
{
  /// <summary>
  /// Summary description for Object form.
  /// </summary>
  public class Objects : System.Windows.Forms.Form
  {
    protected System.Windows.Forms.Button m_bnOK;
    protected System.Windows.Forms.TreeView m_tvObjs;
    protected System.Windows.Forms.ContextMenu m_cntxMenuObjId;
    protected System.Windows.Forms.MenuItem m_mnuItemBrowseReflection;
    protected System.Windows.Forms.ListView m_lvData;
    protected System.Windows.Forms.ColumnHeader m_lvCol_label;
    protected System.Windows.Forms.ColumnHeader m_lvCol_value;

    protected Snoop.Collectors.CollectorObj m_snoopCollector = new Snoop.Collectors.CollectorObj();
    protected System.Object m_curObj;
    protected ArrayList m_treeTypeNodes = new ArrayList();
    protected ArrayList m_types = new ArrayList();
    private ContextMenuStrip listViewContextMenuStrip;
    private System.Windows.Forms.MenuItem m_mnuItemCopy;
    private ToolStripMenuItem copyToolStripMenuItem;
    private ToolStrip toolStrip1;
    private ToolStripButton toolStripButton1;
    private ToolStripButton toolStripButton2;
    private PrintDialog m_printDialog;
    private System.Drawing.Printing.PrintDocument m_printDocument;
    private PrintPreviewDialog m_printPreviewDialog;
    private IContainer components;
    private Int32[] m_maxWidths;
    private ToolStripButton toolStripButton3;
    private Int32 m_currentPrintItem = 0;

    protected
    Objects()
    {
      // this constructor is for derived classes to call
      InitializeComponent();
    }

    public
    Objects( System.Object obj )
    {
      InitializeComponent();

      // get in array form so we can call normal processing code.
      ArrayList objs = new ArrayList();
      objs.Add( obj );
      CommonInit( objs );
    }

    public
    Objects( ArrayList objs )
    {
      InitializeComponent();
      CommonInit( objs );
    }

    public
    Objects( Document doc, ICollection<ElementId> ids )
    {
      InitializeComponent();

      ICollection<Element> elemSet
        = new List<Element>( ids.Select<ElementId, Element>(
          id => doc.GetElement( id ) ) );

      CommonInit( elemSet );
    }

    protected void
    CommonInit( IEnumerable objs )
    {
      m_tvObjs.BeginUpdate();

      AddObjectsToTree( objs );

      // if the tree isn't well populated, expand it and select the first item
      // so its not a pain for the user when there is only one relevant item in the tree
      if( m_tvObjs.Nodes.Count == 1 )
      {
        m_tvObjs.Nodes[0].Expand();
        if( m_tvObjs.Nodes[0].Nodes.Count == 0 )
          m_tvObjs.SelectedNode = m_tvObjs.Nodes[0];
        else
          m_tvObjs.SelectedNode = m_tvObjs.Nodes[0].Nodes[0];
      }

      m_tvObjs.EndUpdate();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void
    Dispose( bool disposing )
    {
      if( disposing )
      {
        if( components != null )
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    protected void
    InitializeComponent()
    {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Objects));
			this.m_tvObjs = new System.Windows.Forms.TreeView();
			this.m_cntxMenuObjId = new System.Windows.Forms.ContextMenu();
			this.m_mnuItemCopy = new System.Windows.Forms.MenuItem();
			this.m_mnuItemBrowseReflection = new System.Windows.Forms.MenuItem();
			this.m_bnOK = new System.Windows.Forms.Button();
			this.m_lvData = new System.Windows.Forms.ListView();
			this.m_lvCol_label = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.m_lvCol_value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.m_printDialog = new System.Windows.Forms.PrintDialog();
			this.m_printDocument = new System.Drawing.Printing.PrintDocument();
			this.m_printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
			this.listViewContextMenuStrip.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_tvObjs
			// 
			this.m_tvObjs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.m_tvObjs.ContextMenu = this.m_cntxMenuObjId;
			this.m_tvObjs.HideSelection = false;
			this.m_tvObjs.Location = new System.Drawing.Point(12, 28);
			this.m_tvObjs.Name = "m_tvObjs";
			this.m_tvObjs.Size = new System.Drawing.Size(248, 430);
			this.m_tvObjs.TabIndex = 0;
			this.m_tvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
			this.m_tvObjs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeNodeSelected);
			// 
			// m_cntxMenuObjId
			// 
			this.m_cntxMenuObjId.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.m_mnuItemCopy,
            this.m_mnuItemBrowseReflection});
			// 
			// m_mnuItemCopy
			// 
			this.m_mnuItemCopy.Index = 0;
			this.m_mnuItemCopy.Text = "Copy";
			this.m_mnuItemCopy.Click += new System.EventHandler(this.ContextMenuClick_Copy);
			// 
			// m_mnuItemBrowseReflection
			// 
			this.m_mnuItemBrowseReflection.Index = 1;
			this.m_mnuItemBrowseReflection.Text = "Browse Using Reflection...";
			this.m_mnuItemBrowseReflection.Click += new System.EventHandler(this.ContextMenuClick_BrowseReflection);
			// 
			// m_bnOK
			// 
			this.m_bnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.m_bnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_bnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_bnOK.Location = new System.Drawing.Point(365, 464);
			this.m_bnOK.Name = "m_bnOK";
			this.m_bnOK.Size = new System.Drawing.Size(75, 23);
			this.m_bnOK.TabIndex = 2;
			this.m_bnOK.Text = "OK";
			// 
			// m_lvData
			// 
			this.m_lvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_lvCol_label,
            this.m_lvCol_value});
			this.m_lvData.ContextMenuStrip = this.listViewContextMenuStrip;
			this.m_lvData.FullRowSelect = true;
			this.m_lvData.GridLines = true;
			this.m_lvData.Location = new System.Drawing.Point(284, 28);
			this.m_lvData.Name = "m_lvData";
			this.m_lvData.ShowItemToolTips = true;
			this.m_lvData.Size = new System.Drawing.Size(504, 430);
			this.m_lvData.TabIndex = 3;
			this.m_lvData.UseCompatibleStateImageBehavior = false;
			this.m_lvData.View = System.Windows.Forms.View.Details;
			this.m_lvData.Click += new System.EventHandler(this.DataItemSelected);
			this.m_lvData.DoubleClick += new System.EventHandler(this.DataItemSelected);
			// 
			// m_lvCol_label
			// 
			this.m_lvCol_label.Text = "Field";
			this.m_lvCol_label.Width = 200;
			// 
			// m_lvCol_value
			// 
			this.m_lvCol_value.Text = "Value";
			this.m_lvCol_value.Width = 300;
			// 
			// listViewContextMenuStrip
			// 
			this.listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
			this.listViewContextMenuStrip.Name = "listViewContextMenuStrip";
			this.listViewContextMenuStrip.Size = new System.Drawing.Size(103, 26);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = global::RevitLookup.Properties.Resources.Copy;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(800, 25);
			this.toolStrip1.TabIndex = 4;
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::RevitLookup.Properties.Resources.Print;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "Print";
			this.toolStripButton1.Click += new System.EventHandler(this.PrintMenuItem_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::RevitLookup.Properties.Resources.Preview;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "Print Preview";
			this.toolStripButton2.Click += new System.EventHandler(this.PrintPreviewMenuItem_Click);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = global::RevitLookup.Properties.Resources.Copy;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "Copy To Clipboard";
			this.toolStripButton3.Click += new System.EventHandler(this.ContextMenuClick_Copy);
			// 
			// m_printDialog
			// 
			this.m_printDialog.Document = this.m_printDocument;
			this.m_printDialog.UseEXDialog = true;
			// 
			// m_printDocument
			// 
			this.m_printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
			// 
			// m_printPreviewDialog
			// 
			this.m_printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.m_printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.m_printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
			this.m_printPreviewDialog.Document = this.m_printDocument;
			this.m_printPreviewDialog.Enabled = true;
			this.m_printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("m_printPreviewDialog.Icon")));
			this.m_printPreviewDialog.Name = "m_printPreviewDialog";
			this.m_printPreviewDialog.Visible = false;
			// 
			// Objects
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_bnOK;
			this.ClientSize = new System.Drawing.Size(800, 492);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.m_lvData);
			this.Controls.Add(this.m_tvObjs);
			this.Controls.Add(this.m_bnOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(650, 200);
			this.Name = "Objects";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Snoop Objects";
			this.listViewContextMenuStrip.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

    }
    #endregion

    protected void
    AddObjectsToTree( IEnumerable objs )
    {
      m_tvObjs.Sorted = true;

      // initialize the tree control
      foreach( Object tmpObj in objs )
      {
        // hook this up to the correct spot in the tree based on the object's type
        TreeNode parentNode = GetExistingNodeForType( tmpObj.GetType() );
        if( parentNode == null )
        {
          parentNode = new TreeNode( tmpObj.GetType().Name );
          m_tvObjs.Nodes.Add( parentNode );

          // record that we've seen this one
          m_treeTypeNodes.Add( parentNode );
          m_types.Add( tmpObj.GetType() );
        }

        // add the new node for this element
        TreeNode tmpNode = new TreeNode( Snoop.Utils.ObjToLabelStr( tmpObj ) );
        tmpNode.Tag = tmpObj;
        parentNode.Nodes.Add( tmpNode );
      }
    }

    /// <summary>
    /// If we've already seen this type before, return the existing TreeNode object
    /// </summary>
    /// <param name="objType">System.Type we're looking to find</param>
    /// <returns>The existing TreeNode or NULL</returns>
    protected TreeNode
    GetExistingNodeForType( System.Type objType )
    {
      int len = m_types.Count;
      for( int i = 0; i < len; i++ )
      {
        if( (System.Type) m_types[i] == objType )
          return (TreeNode) m_treeTypeNodes[i];
      }

      return null;
    }


    #region Events
    protected void
    TreeNodeSelected( object sender, System.Windows.Forms.TreeViewEventArgs e )
    {
      m_curObj = e.Node.Tag;

      // collect the data about this object
      m_snoopCollector.Collect( m_curObj );

      // display it
      Snoop.Utils.Display( m_lvData, m_snoopCollector );
    }

    protected void
    TreeNodeSelected( object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e )
    {
      m_curObj = e.Node.Tag;

      // collect the data about this object
      m_snoopCollector.Collect( m_curObj );

      // display it
      Snoop.Utils.Display( m_lvData, m_snoopCollector );
    }

    protected void
    DataItemSelected( object sender, System.EventArgs e )
    {
      Snoop.Utils.DataItemSelected( m_lvData );
    }

    private void
    ContextMenuClick_Copy( object sender, System.EventArgs e )
    {
      if( m_tvObjs.SelectedNode != null )
      {
        Utils.CopyToClipboard( m_lvData );
      }
    }

    private void
    ContextMenuClick_BrowseReflection( object sender, System.EventArgs e )
    {
      Snoop.Utils.BrowseReflection( m_curObj );
    }

    private void
    CopyToolStripMenuItem_Click( object sender, System.EventArgs e )
    {
      if( m_lvData.SelectedItems.Count > 0 )
      {
        Utils.CopyToClipboard( m_lvData.SelectedItems[0], false );
      }
      else
      {
        Clipboard.Clear();
      }
    }

    private void
    PrintMenuItem_Click( object sender, EventArgs e )
    {
      Utils.UpdatePrintSettings( m_printDocument, m_tvObjs, m_lvData, ref m_maxWidths );
      Utils.PrintMenuItemClick( m_printDialog, m_tvObjs );
    }

    private void
    PrintPreviewMenuItem_Click( object sender, EventArgs e )
    {
      Utils.UpdatePrintSettings( m_printDocument, m_tvObjs, m_lvData, ref m_maxWidths );
      Utils.PrintPreviewMenuItemClick( m_printPreviewDialog, m_tvObjs );
    }

    private void
    PrintDocument_PrintPage( object sender, System.Drawing.Printing.PrintPageEventArgs e )
    {
      m_currentPrintItem = Utils.Print( m_tvObjs.SelectedNode.Text, m_lvData, e, m_maxWidths[0], m_maxWidths[1], m_currentPrintItem );
    }
    #endregion
  }
}
