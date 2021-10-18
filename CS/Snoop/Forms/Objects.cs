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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using RevitLookup.Snoop.Data;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Forms
{
    /// <summary>
    /// Summary description for Object form.
    /// </summary>
    public class Objects : System.Windows.Forms.Form, IHaveCollector
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
        private PrintDialog m_printDialog;
        private System.Drawing.Printing.PrintDocument m_printDocument;
        private PrintPreviewDialog m_printPreviewDialog;
        private IContainer components;
        private Int32[] m_maxWidths;
        private TableLayoutPanel tableLayoutPanel1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStrip toolStrip_ListView;
        private ToolStrip toolStrip_Selectors;
        private ToolStripButton toolStripButton_RefreshListView;
        private ToolStripButton toolStripButton_SnoopDB;
        private ToolStripButton toolStripButton_SnoopCurrentSelection;
        private ToolStripButton toolStripButton_SnoopPickFace;
        private ToolStripButton toolStripButton_SnoopPickEdge;
        private ToolStripButton toolStripButton_SnoopLinkedElement;
        private ToolStripButton toolStripButton_SnoopDependentElements;
        private ToolStripButton toolStripButton_SnoopActiveView;
        private ToolStripButton toolStripButton_SnoopApplication;
        private Int32 m_currentPrintItem = 0;

        public Objects()
        {
            // this constructor is for derived classes to call
            InitializeComponent();
        }

        public Objects(object obj)
        {
            InitializeComponent();

            CommonInit(new[] { SnoopableObjectWrapper.Create(obj) });
        }

        public Objects(ArrayList objs)
        {
            InitializeComponent();

            CommonInit(objs.Cast<object>().Select(SnoopableObjectWrapper.Create));
        }

        public async Task SnoopAndShow(Selector selector)
        {
            await SelectElements(selector);
            ModelessWindowFactory.Show(this);
        }


        public Objects(IEnumerable<SnoopableObjectWrapper> objs)
        {
            InitializeComponent();

            CommonInit(objs);
        }

        protected void CommonInit(IEnumerable<SnoopableObjectWrapper> objs)
        {
            m_tvObjs.Nodes.Clear();
            m_lvData.Items.Clear();
            m_curObj = null;
            m_treeTypeNodes = new ArrayList();
            m_types = new ArrayList();

            m_tvObjs.BeginUpdate();

            AddObjectsToTree(objs);

            // if the tree isn't well populated, expand it and select the first item
            // so its not a pain for the user when there is only one relevant item in the tree
            if (m_tvObjs.Nodes.Count == 1)
            {
                m_tvObjs.Nodes[0].Expand();
                if (m_tvObjs.Nodes[0].Nodes.Count == 0)
                    m_tvObjs.SelectedNode = m_tvObjs.Nodes[0];
                else
                    m_tvObjs.SelectedNode = m_tvObjs.Nodes[0].Nodes[0];
            }

            m_tvObjs.EndUpdate();
            m_tvObjs.Focus();

            // Add Load to update ListView Width
            Utils.AddOnLoadForm(this);
        }

        protected void CommonInit(object obj)
        {
            if (obj is IList<Element> lista)
            {
                CommonInit(lista.Select(SnoopableObjectWrapper.Create));
            }
            else
            {
                CommonInit(new[] { SnoopableObjectWrapper.Create(obj) });
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
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
            this.m_printDialog = new System.Windows.Forms.PrintDialog();
            this.m_printDocument = new System.Drawing.Printing.PrintDocument();
            this.m_printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_ListView = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_RefreshListView = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_Selectors = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_SnoopDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopCurrentSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopPickFace = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopPickEdge = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopLinkedElement = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopDependentElements = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopActiveView = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SnoopApplication = new System.Windows.Forms.ToolStripButton();
            this.listViewContextMenuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip_ListView.SuspendLayout();
            this.toolStrip_Selectors.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tvObjs
            // 
            this.m_tvObjs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_tvObjs.ContextMenu = this.m_cntxMenuObjId;
            this.m_tvObjs.HideSelection = false;
            this.m_tvObjs.Location = new System.Drawing.Point(12, 32);
            this.m_tvObjs.Name = "m_tvObjs";
            this.m_tvObjs.Size = new System.Drawing.Size(248, 430);
            this.m_tvObjs.TabIndex = 0;
            this.m_tvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
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
            this.m_bnOK.Location = new System.Drawing.Point(284, 464);
            this.m_bnOK.Name = "m_bnOK";
            this.m_bnOK.Size = new System.Drawing.Size(504, 23);
            this.m_bnOK.TabIndex = 4;
            this.m_bnOK.Text = "OK";
            this.m_bnOK.Click += new System.EventHandler(this.m_bnOK_Click);
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
            this.m_lvData.HideSelection = false;
            this.m_lvData.Location = new System.Drawing.Point(284, 32);
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
            this.m_lvCol_value.Width = 800;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip_ListView, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip_Selectors, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 26);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(320, 26);
            this.toolStrip1.TabIndex = 5;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::RevitLookup.Properties.Resources.Print;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton1.Text = "Print";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::RevitLookup.Properties.Resources.Preview;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton2.Text = "Print Preview";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::RevitLookup.Properties.Resources.Copy;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton3.Text = "Copy To Clipboard";
            // 
            // toolStrip_ListView
            // 
            this.toolStrip_ListView.AutoSize = false;
            this.toolStrip_ListView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip_ListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_RefreshListView});
            this.toolStrip_ListView.Location = new System.Drawing.Point(640, 0);
            this.toolStrip_ListView.Name = "toolStrip_ListView";
            this.toolStrip_ListView.Size = new System.Drawing.Size(160, 26);
            this.toolStrip_ListView.TabIndex = 7;
            this.toolStrip_ListView.Text = "toolStrip3";
            // 
            // toolStripButton_RefreshListView
            // 
            this.toolStripButton_RefreshListView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_RefreshListView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RefreshListView.Image")));
            this.toolStripButton_RefreshListView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RefreshListView.Name = "toolStripButton_RefreshListView";
            this.toolStripButton_RefreshListView.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_RefreshListView.Text = "toolStripButton4";
            this.toolStripButton_RefreshListView.ToolTipText = "Refresh selected element data in the list view";
            this.toolStripButton_RefreshListView.Click += new System.EventHandler(this.toolStripButton_RefreshListView_Click);
            this.toolStripButton_RefreshListView.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStrip_Selectors
            // 
            this.toolStrip_Selectors.AutoSize = false;
            this.toolStrip_Selectors.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip_Selectors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_SnoopDB,
            this.toolStripButton_SnoopCurrentSelection,
            this.toolStripButton_SnoopPickFace,
            this.toolStripButton_SnoopPickEdge,
            this.toolStripButton_SnoopLinkedElement,
            this.toolStripButton_SnoopDependentElements,
            this.toolStripButton_SnoopActiveView,
            this.toolStripButton_SnoopApplication});
            this.toolStrip_Selectors.Location = new System.Drawing.Point(320, 0);
            this.toolStrip_Selectors.Name = "toolStrip_Selectors";
            this.toolStrip_Selectors.Size = new System.Drawing.Size(320, 26);
            this.toolStrip_Selectors.TabIndex = 8;
            this.toolStrip_Selectors.Text = "toolStrip2";
            // 
            // toolStripButton_SnoopDB
            // 
            this.toolStripButton_SnoopDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopDB.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopDB.Image")));
            this.toolStripButton_SnoopDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopDB.Name = "toolStripButton_SnoopDB";
            this.toolStripButton_SnoopDB.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopDB.Tag = "SnoopDB";
            this.toolStripButton_SnoopDB.Text = "Snoop DB";
            this.toolStripButton_SnoopDB.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopDB.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopCurrentSelection
            // 
            this.toolStripButton_SnoopCurrentSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopCurrentSelection.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopCurrentSelection.Image")));
            this.toolStripButton_SnoopCurrentSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopCurrentSelection.Name = "toolStripButton_SnoopCurrentSelection";
            this.toolStripButton_SnoopCurrentSelection.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopCurrentSelection.Tag = "SnoopCurrentSelection";
            this.toolStripButton_SnoopCurrentSelection.Text = "Snoop current selection";
            this.toolStripButton_SnoopCurrentSelection.ToolTipText = "Snoop current selection";
            this.toolStripButton_SnoopCurrentSelection.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopCurrentSelection.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopPickFace
            // 
            this.toolStripButton_SnoopPickFace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopPickFace.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopPickFace.Image")));
            this.toolStripButton_SnoopPickFace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopPickFace.Name = "toolStripButton_SnoopPickFace";
            this.toolStripButton_SnoopPickFace.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopPickFace.Tag = "SnoopPickFace";
            this.toolStripButton_SnoopPickFace.Text = "Snoop pick face";
            this.toolStripButton_SnoopPickFace.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopPickFace.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopPickEdge
            // 
            this.toolStripButton_SnoopPickEdge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopPickEdge.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopPickEdge.Image")));
            this.toolStripButton_SnoopPickEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopPickEdge.Name = "toolStripButton_SnoopPickEdge";
            this.toolStripButton_SnoopPickEdge.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopPickEdge.Tag = "SnoopPickEdge";
            this.toolStripButton_SnoopPickEdge.Text = "Snoop pick edge";
            this.toolStripButton_SnoopPickEdge.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopPickEdge.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopLinkedElement
            // 
            this.toolStripButton_SnoopLinkedElement.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopLinkedElement.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopLinkedElement.Image")));
            this.toolStripButton_SnoopLinkedElement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopLinkedElement.Name = "toolStripButton_SnoopLinkedElement";
            this.toolStripButton_SnoopLinkedElement.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopLinkedElement.Tag = "SnoopLinkedElement";
            this.toolStripButton_SnoopLinkedElement.Text = "Snoop linked element";
            this.toolStripButton_SnoopLinkedElement.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopLinkedElement.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopDependentElements
            // 
            this.toolStripButton_SnoopDependentElements.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopDependentElements.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopDependentElements.Image")));
            this.toolStripButton_SnoopDependentElements.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopDependentElements.Name = "toolStripButton_SnoopDependentElements";
            this.toolStripButton_SnoopDependentElements.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopDependentElements.Tag = "SnoopDependentElements";
            this.toolStripButton_SnoopDependentElements.Text = "Snoop dependent elements";
            this.toolStripButton_SnoopDependentElements.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopDependentElements.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopActiveView
            // 
            this.toolStripButton_SnoopActiveView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopActiveView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopActiveView.Image")));
            this.toolStripButton_SnoopActiveView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopActiveView.Name = "toolStripButton_SnoopActiveView";
            this.toolStripButton_SnoopActiveView.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopActiveView.Tag = "SnoopActiveView";
            this.toolStripButton_SnoopActiveView.Text = "Snoop active view";
            this.toolStripButton_SnoopActiveView.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopActiveView.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // toolStripButton_SnoopApplication
            // 
            this.toolStripButton_SnoopApplication.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SnoopApplication.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SnoopApplication.Image")));
            this.toolStripButton_SnoopApplication.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SnoopApplication.Name = "toolStripButton_SnoopApplication";
            this.toolStripButton_SnoopApplication.Size = new System.Drawing.Size(24, 23);
            this.toolStripButton_SnoopApplication.Tag = "SnoopApplication";
            this.toolStripButton_SnoopApplication.Text = "Snoop application";
            this.toolStripButton_SnoopApplication.Click += new System.EventHandler(this.toolStripButton_Snoop_Click);
            this.toolStripButton_SnoopApplication.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // Objects
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_bnOK;
            this.ClientSize = new System.Drawing.Size(800, 492);
            this.Controls.Add(this.tableLayoutPanel1);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip_ListView.ResumeLayout(false);
            this.toolStrip_ListView.PerformLayout();
            this.toolStrip_Selectors.ResumeLayout(false);
            this.toolStrip_Selectors.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        protected void AddObjectsToTree(IEnumerable<SnoopableObjectWrapper> snoopableObjects)
        {
            m_tvObjs.Sorted = true;

            // initialize the tree control
            foreach (var snoopableObject in snoopableObjects)
            {
                // hook this up to the correct spot in the tree based on the object's type
                TreeNode parentNode = GetExistingNodeForType(snoopableObject.GetUnderlyingType());
                if (parentNode == null)
                {
                    parentNode = new TreeNode(snoopableObject.GetUnderlyingType().Name);
                    m_tvObjs.Nodes.Add(parentNode);

                    // record that we've seen this one
                    m_treeTypeNodes.Add(parentNode);
                    m_types.Add(snoopableObject.GetUnderlyingType());
                }

                // add the new node for this element
                var tmpNode = new TreeNode(snoopableObject.Title) { Tag = snoopableObject.Object };
                parentNode.Nodes.Add(tmpNode);
            }
        }

        /// <summary>
        /// If we've already seen this type before, return the existing TreeNode object
        /// </summary>
        /// <param name="objType">System.Type we're looking to find</param>
        /// <returns>The existing TreeNode or NULL</returns>
        protected TreeNode GetExistingNodeForType(System.Type objType)
        {
            int len = m_types.Count;
            for (int i = 0; i < len; i++)
            {
                if ((System.Type)m_types[i] == objType)
                    return (TreeNode)m_treeTypeNodes[i];
            }

            return null;
        }

        private async Task CollectAndDispalyData()
        {
            try
            {
                // collect the data about this object
                await m_snoopCollector.Collect(m_curObj);

                // display it
                Snoop.Utils.Display(m_lvData, m_snoopCollector);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SelectElements(Selector selector)
        {
            await ExternalExecutor.ExecuteInRevitContextAsync(x =>
            {
                tableLayoutPanel1.Enabled = false;
                m_tvObjs.Enabled = false;
                m_lvData.Enabled = false;
                m_bnOK.Enabled = false;

                var selected = Selectors.Snoop(x, selector);

                tableLayoutPanel1.Enabled = true;
                m_tvObjs.Enabled = true;
                m_lvData.Enabled = true;
                m_bnOK.Enabled = true;

                SetDocument(selected.Item2);
                CommonInit(selected.Item1);
            });
        }

        public void SetDocument(Document document)
        {
            m_snoopCollector.SourceDocument = document;
        }

        #region Events
        protected async void TreeNodeSelected(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            m_curObj = e.Node.Tag;
            await CollectAndDispalyData();
        }

        protected void DataItemSelected(object sender, System.EventArgs e)
        {
            Snoop.Utils.DataItemSelected(m_lvData, new ModelessWindowFactory(this, m_snoopCollector.SourceDocument));
        }

        private void ContextMenuClick_Copy(object sender, System.EventArgs e)
        {
            if (m_tvObjs.SelectedNode != null)
            {
                Utils.CopyToClipboard(m_lvData);
            }
        }

        private void ContextMenuClick_BrowseReflection(object sender, System.EventArgs e)
        {
            Snoop.Utils.BrowseReflection(m_curObj);
        }

        private void CopyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (m_lvData.SelectedItems.Count > 0)
            {
                Utils.CopyToClipboard(m_lvData.SelectedItems[0], false);
            }
            else
            {
                Clipboard.Clear();
            }
        }

        private void PrintMenuItem_Click(object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(m_printDocument, m_tvObjs, m_lvData, ref m_maxWidths);
            Utils.PrintMenuItemClick(m_printDialog, m_tvObjs);
        }

        private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(m_printDocument, m_tvObjs, m_lvData, ref m_maxWidths);
            Utils.PrintPreviewMenuItemClick(m_printPreviewDialog, m_tvObjs);
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            m_currentPrintItem = Utils.Print(m_tvObjs.SelectedNode.Text, m_lvData, e, m_maxWidths[0], m_maxWidths[1], m_currentPrintItem);
        }

        private async void toolStripButton_RefreshListView_Click(object sender, EventArgs e)
        {
            await CollectAndDispalyData();
        }

        private void toolStrip_MouseEnter(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Form.ActiveForm != this)
            {
                Activate();
            }
        }

        private void m_bnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            Dispose();
        }       

        private async void toolStripButton_Snoop_Click(object sender, EventArgs e)
        {
            var btn = sender as ToolStripButton;
            var selector = (Selector)Enum.Parse(typeof(Selector), btn.Tag as string);

            await SelectElements(selector);           
        }       
        #endregion
    }
}