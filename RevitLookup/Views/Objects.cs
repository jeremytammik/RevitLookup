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
using System.ComponentModel;
using System.Drawing.Printing;
using Autodesk.Revit.DB;
using RevitLookup.Core;
using RevitLookup.Core.Collectors;
using RevitLookup.Core.RevitTypes;
using Exception = System.Exception;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Views;

/// <summary>
///     Summary description for Object form.
/// </summary>
public class Objects : Form, IHaveCollector
{
    private readonly CollectorObj _snoopCollector = new();
    private ToolStripMenuItem _copyToolStripMenuItem;
    private object _curObj;
    private int _currentPrintItem;
    private ContextMenuStrip _listViewContextMenuStrip;
    private ColumnHeader _lvColLabel;
    private ColumnHeader _lvColValue;
    private int[] _maxWidths;
    private MenuItem _mnuItemBrowseReflection;
    private MenuItem _mnuItemCopy;
    private PrintDialog _printDialog;
    private PrintDocument _printDocument;
    private PrintPreviewDialog _printPreviewDialog;
    private TableLayoutPanel _tableLayoutPanel1;
    private ToolStrip _toolStrip1;
    private ToolStripButton _toolStripButton1;
    private ToolStripButton _toolStripButton2;
    private ToolStripButton _toolStripButton3;
    private ToolStripButton _toolStripButtonRefreshListView;
    private ToolStripButton _toolStripButtonSnoopActiveView;
    private ToolStripButton _toolStripButtonSnoopApplication;
    private ToolStripButton _toolStripButtonSnoopCurrentSelection;
    private ToolStripButton _toolStripButtonSnoopDb;
    private ToolStripButton _toolStripButtonSnoopDependentElements;
    private ToolStripButton _toolStripButtonSnoopLinkedElement;
    private ToolStripButton _toolStripButtonSnoopPickEdge;
    private ToolStripButton _toolStripButtonSnoopPickFace;
    private ToolStrip _toolStripListView;
    private ToolStrip _toolStripSelectors;
    private ArrayList _treeTypeNodes = new();
    private ArrayList _types = new();
    protected Button BnOk;
    protected ContextMenu CntxMenuObjId;
    private IContainer components;
    protected ListView LvData;
    protected TreeView TvObjs;

    public Objects()
    {
        InitializeComponent();
    }

    public Objects(object obj)
    {
        InitializeComponent();
        CommonInit(new[] {SnoopableObjectWrapper.Create(obj)});
    }

    public Objects(ArrayList objs)
    {
        InitializeComponent();
        CommonInit(objs.Cast<object>().Select(SnoopableObjectWrapper.Create));
    }


    public Objects(IEnumerable<SnoopableObjectWrapper> objs)
    {
        InitializeComponent();
        CommonInit(objs);
    }

    public void SetDocument(Document document)
    {
        _snoopCollector.SourceDocument = document;
    }

    public async void SnoopAndShow(Selector selector)
    {
        await SelectElements(selector);
        ModelessWindowFactory.Show(this);
    }

    private void CommonInit(IEnumerable<SnoopableObjectWrapper> objs)
    {
        TvObjs.Nodes.Clear();
        LvData.Items.Clear();
        _curObj = null;
        _treeTypeNodes = new ArrayList();
        _types = new ArrayList();

        TvObjs.BeginUpdate();
        AddObjectsToTree(objs);

        // if the tree isn't well populated, expand it and select the first item
        // so its not a pain for the user when there is only one relevant item in the tree
        if (TvObjs.Nodes.Count == 1)
        {
            TvObjs.Nodes[0].Expand();
            TvObjs.SelectedNode = TvObjs.Nodes[0].Nodes.Count == 0
                ? TvObjs.Nodes[0]
                : TvObjs.Nodes[0].Nodes[0];
        }

        TvObjs.EndUpdate();
        TvObjs.Focus();

        // Add Load to update ListView Width
        Core.Utils.AddOnLoadForm(this);
    }

    private void CommonInit(object obj)
    {
        switch (obj)
        {
            case null:
                return;
            case IList<Element> lista:
                CommonInit(lista.Select(SnoopableObjectWrapper.Create));
                break;
            default:
                CommonInit(new[] {SnoopableObjectWrapper.Create(obj)});
                break;
        }
    }

    /// <summary>
    ///     Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///     Required method for Designer support - do not modify
    ///     the contents of this method with the code editor.
    /// </summary>
    protected void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Objects));
        this.TvObjs = new System.Windows.Forms.TreeView();
        this.CntxMenuObjId = new System.Windows.Forms.ContextMenu();
        this._mnuItemCopy = new System.Windows.Forms.MenuItem();
        this._mnuItemBrowseReflection = new System.Windows.Forms.MenuItem();
        this.BnOk = new System.Windows.Forms.Button();
        this.LvData = new System.Windows.Forms.ListView();
        this._lvColLabel = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
        this._lvColValue = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
        this._listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
        this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this._printDialog = new System.Windows.Forms.PrintDialog();
        this._printDocument = new System.Drawing.Printing.PrintDocument();
        this._printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
        this._tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this._toolStrip1 = new System.Windows.Forms.ToolStrip();
        this._toolStripButton1 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton2 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton3 = new System.Windows.Forms.ToolStripButton();
        this._toolStripListView = new System.Windows.Forms.ToolStrip();
        this._toolStripButtonRefreshListView = new System.Windows.Forms.ToolStripButton();
        this._toolStripSelectors = new System.Windows.Forms.ToolStrip();
        this._toolStripButtonSnoopDb = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopCurrentSelection = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopPickFace = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopPickEdge = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopLinkedElement = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopDependentElements = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopActiveView = new System.Windows.Forms.ToolStripButton();
        this._toolStripButtonSnoopApplication = new System.Windows.Forms.ToolStripButton();
        this._listViewContextMenuStrip.SuspendLayout();
        this._tableLayoutPanel1.SuspendLayout();
        this._toolStrip1.SuspendLayout();
        this._toolStripListView.SuspendLayout();
        this._toolStripSelectors.SuspendLayout();
        this.SuspendLayout();
        // 
        // m_tvObjs
        // 
        this.TvObjs.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                    | System.Windows.Forms.AnchorStyles.Left)));
        this.TvObjs.ContextMenu = this.CntxMenuObjId;
        this.TvObjs.HideSelection = false;
        this.TvObjs.Location = new System.Drawing.Point(12, 32);
        this.TvObjs.Name = "TvObjs";
        this.TvObjs.Size = new System.Drawing.Size(248, 430);
        this.TvObjs.TabIndex = 0;
        this.TvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
        // 
        // m_cntxMenuObjId
        // 
        this.CntxMenuObjId.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
        {
            this._mnuItemCopy,
            this._mnuItemBrowseReflection
        });
        // 
        // m_mnuItemCopy
        // 
        this._mnuItemCopy.Index = 0;
        this._mnuItemCopy.Text = "Copy";
        this._mnuItemCopy.Click += new System.EventHandler(this.ContextMenuClick_Copy);
        // 
        // m_mnuItemBrowseReflection
        // 
        this._mnuItemBrowseReflection.Index = 1;
        this._mnuItemBrowseReflection.Text = "Browse Using Reflection...";
        this._mnuItemBrowseReflection.Click += new System.EventHandler(this.ContextMenuClick_BrowseReflection);
        // 
        // m_bnOK
        // 
        this.BnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this.BnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.BnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.BnOk.Location = new System.Drawing.Point(284, 464);
        this.BnOk.Name = "BnOk";
        this.BnOk.Size = new System.Drawing.Size(504, 23);
        this.BnOk.TabIndex = 4;
        this.BnOk.Text = "OK";
        this.BnOk.Click += new System.EventHandler(this.ButtonOK_Click);
        // 
        // m_lvData
        // 
        this.LvData.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                     | System.Windows.Forms.AnchorStyles.Left)
                                                                    | System.Windows.Forms.AnchorStyles.Right)));
        this.LvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
        {
            this._lvColLabel,
            this._lvColValue
        });
        this.LvData.ContextMenuStrip = this._listViewContextMenuStrip;
        this.LvData.FullRowSelect = true;
        this.LvData.GridLines = true;
        this.LvData.HideSelection = false;
        this.LvData.Location = new System.Drawing.Point(284, 32);
        this.LvData.Name = "LvData";
        this.LvData.ShowItemToolTips = true;
        this.LvData.Size = new System.Drawing.Size(504, 430);
        this.LvData.TabIndex = 3;
        this.LvData.UseCompatibleStateImageBehavior = false;
        this.LvData.View = System.Windows.Forms.View.Details;
        this.LvData.Click += new System.EventHandler(this.DataItemSelected);
        this.LvData.DoubleClick += new System.EventHandler(this.DataItemSelected);
        // 
        // m_lvCol_label
        // 
        this._lvColLabel.Text = "Field";
        this._lvColLabel.Width = 200;
        // 
        // m_lvCol_value
        // 
        this._lvColValue.Text = "Value";
        this._lvColValue.Width = 800;
        // 
        // listViewContextMenuStrip
        // 
        this._listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            this._copyToolStripMenuItem
        });
        this._listViewContextMenuStrip.Name = "_listViewContextMenuStrip";
        this._listViewContextMenuStrip.Size = new System.Drawing.Size(103, 26);
        // 
        // copyToolStripMenuItem
        // 
        this._copyToolStripMenuItem.Image = global::RevitLookup.Properties.Resources.Copy;
        this._copyToolStripMenuItem.Name = "_copyToolStripMenuItem";
        this._copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
        this._copyToolStripMenuItem.Text = "Copy";
        this._copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
        // 
        // m_printDialog
        // 
        this._printDialog.Document = this._printDocument;
        this._printDialog.UseEXDialog = true;
        // 
        // m_printDocument
        // 
        this._printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
        // 
        // m_printPreviewDialog
        // 
        this._printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
        this._printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
        this._printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
        this._printPreviewDialog.Document = this._printDocument;
        this._printPreviewDialog.Enabled = true;
        this._printPreviewDialog.Icon = ((System.Drawing.Icon) (resources.GetObject("m_printPreviewDialog.Icon")));
        this._printPreviewDialog.Name = "_printPreviewDialog";
        this._printPreviewDialog.Visible = false;
        // 
        // tableLayoutPanel1
        // 
        this._tableLayoutPanel1.AutoSize = true;
        this._tableLayoutPanel1.ColumnCount = 3;
        this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
        this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
        this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
        this._tableLayoutPanel1.Controls.Add(this._toolStrip1, 0, 0);
        this._tableLayoutPanel1.Controls.Add(this._toolStripListView, 2, 0);
        this._tableLayoutPanel1.Controls.Add(this._toolStripSelectors, 1, 0);
        this._tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
        this._tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        this._tableLayoutPanel1.Name = "_tableLayoutPanel1";
        this._tableLayoutPanel1.RowCount = 1;
        this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this._tableLayoutPanel1.Size = new System.Drawing.Size(800, 26);
        this._tableLayoutPanel1.TabIndex = 5;
        // 
        // toolStrip1
        // 
        this._toolStrip1.AutoSize = false;
        this._toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            this._toolStripButton1,
            this._toolStripButton2,
            this._toolStripButton3
        });
        this._toolStrip1.Location = new System.Drawing.Point(0, 0);
        this._toolStrip1.Name = "_toolStrip1";
        this._toolStrip1.Size = new System.Drawing.Size(320, 26);
        this._toolStrip1.TabIndex = 5;
        // 
        // toolStripButton1
        // 
        this._toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButton1.Image = global::RevitLookup.Properties.Resources.Print;
        this._toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButton1.Name = "_toolStripButton1";
        this._toolStripButton1.Size = new System.Drawing.Size(24, 23);
        this._toolStripButton1.Text = "Print";
        this._toolStripButton1.Click += new System.EventHandler(this.PrintMenuItem_Click);
        // 
        // toolStripButton2
        // 
        this._toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButton2.Image = global::RevitLookup.Properties.Resources.Preview;
        this._toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButton2.Name = "_toolStripButton2";
        this._toolStripButton2.Size = new System.Drawing.Size(24, 23);
        this._toolStripButton2.Text = "Print Preview";
        this._toolStripButton2.Click += new System.EventHandler(this.PrintPreviewMenuItem_Click);
        // 
        // toolStripButton3
        // 
        this._toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButton3.Image = global::RevitLookup.Properties.Resources.Copy;
        this._toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButton3.Name = "_toolStripButton3";
        this._toolStripButton3.Size = new System.Drawing.Size(24, 23);
        this._toolStripButton3.Text = "Copy To Clipboard";
        this._toolStripButton3.Click += new System.EventHandler(this.ContextMenuClick_Copy);
        // 
        // toolStrip_ListView
        // 
        this._toolStripListView.AutoSize = false;
        this._toolStripListView.ImageScalingSize = new System.Drawing.Size(20, 20);
        this._toolStripListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            this._toolStripButtonRefreshListView
        });
        this._toolStripListView.Location = new System.Drawing.Point(640, 0);
        this._toolStripListView.Name = "_toolStripListView";
        this._toolStripListView.Size = new System.Drawing.Size(160, 26);
        this._toolStripListView.TabIndex = 7;
        this._toolStripListView.Text = "toolStrip3";
        // 
        // toolStripButton_RefreshListView
        // 
        this._toolStripButtonRefreshListView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonRefreshListView.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonRefreshListView.Image")));
        this._toolStripButtonRefreshListView.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonRefreshListView.Name = "_toolStripButtonRefreshListView";
        this._toolStripButtonRefreshListView.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonRefreshListView.Text = "toolStripButton4";
        this._toolStripButtonRefreshListView.ToolTipText = "Refresh selected element data in the list view";
        this._toolStripButtonRefreshListView.Click += new System.EventHandler(this.ToolStripButton_RefreshListView_Click);
        this._toolStripButtonRefreshListView.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStrip_Selectors
        // 
        this._toolStripSelectors.AutoSize = false;
        this._toolStripSelectors.ImageScalingSize = new System.Drawing.Size(20, 20);
        this._toolStripSelectors.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            this._toolStripButtonSnoopDb,
            this._toolStripButtonSnoopCurrentSelection,
            this._toolStripButtonSnoopPickFace,
            this._toolStripButtonSnoopPickEdge,
            this._toolStripButtonSnoopLinkedElement,
            this._toolStripButtonSnoopDependentElements,
            this._toolStripButtonSnoopActiveView,
            this._toolStripButtonSnoopApplication
        });
        this._toolStripSelectors.Location = new System.Drawing.Point(320, 0);
        this._toolStripSelectors.Name = "_toolStripSelectors";
        this._toolStripSelectors.Size = new System.Drawing.Size(320, 26);
        this._toolStripSelectors.TabIndex = 8;
        this._toolStripSelectors.Text = "toolStrip2";
        // 
        // toolStripButton_SnoopDB
        // 
        this._toolStripButtonSnoopDb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopDb.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopDb.Image")));
        this._toolStripButtonSnoopDb.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopDb.Name = "_toolStripButtonSnoopDb";
        this._toolStripButtonSnoopDb.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopDb.Tag = nameof(Selector.SnoopDb);
        this._toolStripButtonSnoopDb.Text = "Snoop DB";
        this._toolStripButtonSnoopDb.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopDb.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopCurrentSelection
        // 
        this._toolStripButtonSnoopCurrentSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopCurrentSelection.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopCurrentSelection.Image")));
        this._toolStripButtonSnoopCurrentSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopCurrentSelection.Name = "_toolStripButtonSnoopCurrentSelection";
        this._toolStripButtonSnoopCurrentSelection.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopCurrentSelection.Tag = nameof(Selector.SnoopCurrentSelection);
        this._toolStripButtonSnoopCurrentSelection.Text = "Snoop current selection";
        this._toolStripButtonSnoopCurrentSelection.ToolTipText = "Snoop current selection";
        this._toolStripButtonSnoopCurrentSelection.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopCurrentSelection.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopPickFace
        // 
        this._toolStripButtonSnoopPickFace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopPickFace.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopPickFace.Image")));
        this._toolStripButtonSnoopPickFace.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopPickFace.Name = "_toolStripButtonSnoopPickFace";
        this._toolStripButtonSnoopPickFace.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopPickFace.Tag = nameof(Selector.SnoopPickFace);
        this._toolStripButtonSnoopPickFace.Text = "Snoop pick face";
        this._toolStripButtonSnoopPickFace.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopPickFace.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopPickEdge
        // 
        this._toolStripButtonSnoopPickEdge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopPickEdge.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopPickEdge.Image")));
        this._toolStripButtonSnoopPickEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopPickEdge.Name = "_toolStripButtonSnoopPickEdge";
        this._toolStripButtonSnoopPickEdge.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopPickEdge.Tag = nameof(Selector.SnoopPickEdge);
        this._toolStripButtonSnoopPickEdge.Text = "Snoop pick edge";
        this._toolStripButtonSnoopPickEdge.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopPickEdge.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopLinkedElement
        // 
        this._toolStripButtonSnoopLinkedElement.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopLinkedElement.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopLinkedElement.Image")));
        this._toolStripButtonSnoopLinkedElement.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopLinkedElement.Name = "_toolStripButtonSnoopLinkedElement";
        this._toolStripButtonSnoopLinkedElement.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopLinkedElement.Tag = nameof(Selector.SnoopLinkedElement);
        this._toolStripButtonSnoopLinkedElement.Text = "Snoop linked element";
        this._toolStripButtonSnoopLinkedElement.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopLinkedElement.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopDependentElements
        // 
        this._toolStripButtonSnoopDependentElements.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopDependentElements.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopDependentElements.Image")));
        this._toolStripButtonSnoopDependentElements.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopDependentElements.Name = "_toolStripButtonSnoopDependentElements";
        this._toolStripButtonSnoopDependentElements.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopDependentElements.Tag = nameof(Selector.SnoopDependentElements);
        this._toolStripButtonSnoopDependentElements.Text = "Snoop dependent elements";
        this._toolStripButtonSnoopDependentElements.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopDependentElements.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopActiveView
        // 
        this._toolStripButtonSnoopActiveView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopActiveView.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopActiveView.Image")));
        this._toolStripButtonSnoopActiveView.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopActiveView.Name = "_toolStripButtonSnoopActiveView";
        this._toolStripButtonSnoopActiveView.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopActiveView.Tag = nameof(Selector.SnoopActiveView);
        this._toolStripButtonSnoopActiveView.Text = "Snoop active view";
        this._toolStripButtonSnoopActiveView.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopActiveView.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // toolStripButton_SnoopApplication
        // 
        this._toolStripButtonSnoopApplication.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButtonSnoopApplication.Image = ((System.Drawing.Image) (resources.GetObject("_toolStripButtonSnoopApplication.Image")));
        this._toolStripButtonSnoopApplication.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButtonSnoopApplication.Name = "_toolStripButtonSnoopApplication";
        this._toolStripButtonSnoopApplication.Size = new System.Drawing.Size(24, 23);
        this._toolStripButtonSnoopApplication.Tag = nameof(Selector.SnoopApplication);
        this._toolStripButtonSnoopApplication.Text = "Snoop application";
        this._toolStripButtonSnoopApplication.Click += new System.EventHandler(this.ToolStripButton_Snoop_Click);
        this._toolStripButtonSnoopApplication.MouseEnter += new System.EventHandler(this.ToolStrip_MouseEnter);
        // 
        // Objects
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.CancelButton = this.BnOk;
        this.ClientSize = new System.Drawing.Size(800, 492);
        this.Controls.Add(this._tableLayoutPanel1);
        this.Controls.Add(this.LvData);
        this.Controls.Add(this.TvObjs);
        this.Controls.Add(this.BnOk);
        this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(650, 200);
        this.Name = "Objects";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Snoop Objects";
        this._listViewContextMenuStrip.ResumeLayout(false);
        this._tableLayoutPanel1.ResumeLayout(false);
        this._toolStrip1.ResumeLayout(false);
        this._toolStrip1.PerformLayout();
        this._toolStripListView.ResumeLayout(false);
        this._toolStripListView.PerformLayout();
        this._toolStripSelectors.ResumeLayout(false);
        this._toolStripSelectors.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private void AddObjectsToTree(IEnumerable<SnoopableObjectWrapper> snoopableObjects)
    {
        TvObjs.Sorted = true;

        // initialize the tree control
        foreach (var snoopableObject in snoopableObjects)
        {
            // hook this up to the correct spot in the tree based on the object's type
            var parentNode = GetExistingNodeForType(snoopableObject.GetUnderlyingType());
            if (parentNode is null)
            {
                parentNode = new TreeNode(snoopableObject.GetUnderlyingType().Name);
                TvObjs.Nodes.Add(parentNode);

                // record that we've seen this one
                _treeTypeNodes.Add(parentNode);
                _types.Add(snoopableObject.GetUnderlyingType());
            }

            // add the new node for this element
            var tmpNode = new TreeNode(snoopableObject.Title) {Tag = snoopableObject.Object};
            parentNode.Nodes.Add(tmpNode);
        }
    }

    /// <summary>
    ///     If we've already seen this type before, return the existing TreeNode object
    /// </summary>
    /// <param name="objType">System.Type we're looking to find</param>
    /// <returns>The existing TreeNode or NULL</returns>
    private TreeNode GetExistingNodeForType(Type objType)
    {
        var len = _types.Count;
        for (var i = 0; i < len; i++)
            if ((Type) _types[i] == objType)
                return (TreeNode) _treeTypeNodes[i];

        return null;
    }

    private async Task CollectAndDisplayData()
    {
        try
        {
            // collect the data about this object
            await _snoopCollector.Collect(_curObj);

            // display it
            Core.Utils.Display(LvData, _snoopCollector);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task SelectElements(Selector selector)
    {
        await ExternalExecutor.ExecuteInRevitContextAsync(x =>
        {
            _tableLayoutPanel1.Enabled = false;
            TvObjs.Enabled = false;
            LvData.Enabled = false;
            BnOk.Enabled = false;

            var selected = Selectors.Snoop(x, selector);

            _tableLayoutPanel1.Enabled = true;
            TvObjs.Enabled = true;
            LvData.Enabled = true;
            BnOk.Enabled = true;

            SetDocument(selected.Item2);
            CommonInit(selected.Item1);
        });
    }

    #region Events

    private async void TreeNodeSelected(object sender, TreeViewEventArgs e)
    {
        _curObj = e.Node.Tag;
        await CollectAndDisplayData();
    }

    private void DataItemSelected(object sender, EventArgs e)
    {
        Core.Utils.DataItemSelected(LvData, new ModelessWindowFactory(this, _snoopCollector.SourceDocument));
    }

    private void ContextMenuClick_Copy(object sender, EventArgs e)
    {
        if (TvObjs.SelectedNode is not null) Core.Utils.CopyToClipboard(LvData);
    }

    private void ContextMenuClick_BrowseReflection(object sender, EventArgs e)
    {
        Core.Utils.BrowseReflection(_curObj);
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (LvData.SelectedItems.Count > 0)
            Core.Utils.CopyToClipboard(LvData.SelectedItems[0], false);
        else
            Clipboard.Clear();
    }

    private void PrintMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_printDocument, TvObjs, LvData, ref _maxWidths);
        Core.Utils.PrintMenuItemClick(_printDialog, TvObjs);
    }

    private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_printDocument, TvObjs, LvData, ref _maxWidths);
        Core.Utils.PrintPreviewMenuItemClick(_printPreviewDialog, TvObjs);
    }

    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        _currentPrintItem = Core.Utils.Print(TvObjs.SelectedNode.Text, LvData, e, _maxWidths[0], _maxWidths[1], _currentPrintItem);
    }

    private async void ToolStripButton_RefreshListView_Click(object sender, EventArgs e)
    {
        await CollectAndDisplayData();
    }

    private void ToolStrip_MouseEnter(object sender, EventArgs e)
    {
        if (ActiveForm != this) Activate();
    }

    private void ButtonOK_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
        Dispose();
    }

    private async void ToolStripButton_Snoop_Click(object sender, EventArgs e)
    {
        var btn = (ToolStripButton) sender;
        var selector = (Selector) Enum.Parse(typeof(Selector), btn.Tag as string ?? string.Empty);
        await SelectElements(selector);
    }

    #endregion
}