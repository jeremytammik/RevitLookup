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

using System.Collections;
using System.ComponentModel;
using System.Drawing.Printing;
using Autodesk.Revit.DB;
using RevitLookup.Core;
using RevitLookup.Core.Collectors;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Views;

/// <summary>
///     Summary description for Object form.
/// </summary>
public class ParamEnumSnoopView : Form, IHaveCollector
{
    private readonly Hashtable _enumMap;
    private ToolStripMenuItem _copyToolStripMenuItem;
    private int _currentPrintItem;
    private ContextMenuStrip _listViewContextMenuStrip;
    private int[] _maxWidths;
    private MenuItem _mnuItemCopy;
    private PrintDialog _printDialog;
    private PrintDocument _printDocument;
    private PrintPreviewDialog _printPreviewDialog;
    private ToolStrip _toolStrip1;
    private ToolStripButton _toolStripButton1;
    private ToolStripButton _toolStripButton2;
    private ToolStripButton _toolStripButton3;
    private IContainer components;
    protected System.Windows.Forms.Button MBnOk;
    protected ContextMenu MCntxMenuObjId;
    protected object MCurObj;
    protected ColumnHeader MLvColLabel;
    protected ColumnHeader MLvColValue;
    protected System.Windows.Forms.ListView MLvData;
    protected MenuItem MMnuItemBrowseReflection;

    protected CollectorObj MSnoopCollector = new();
    protected System.Windows.Forms.TreeView TvObjs;

    public ParamEnumSnoopView(Hashtable enumMap)
    {
        _enumMap = enumMap;

        // this constructor is for derived classes to call
        InitializeComponent();

        // Add Load to update ListView Width
        Core.Utils.AddOnLoadForm(this);

        TvObjs.BeginUpdate();

        AddParametersToTree();

        TvObjs.ExpandAll();
        TvObjs.EndUpdate();
    }

    public Document Document
    {
        set => MSnoopCollector.Document = value;
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamEnumSnoopView));
        this.TvObjs = new System.Windows.Forms.TreeView();
        this.MCntxMenuObjId = new System.Windows.Forms.ContextMenu();
        this._mnuItemCopy = new System.Windows.Forms.MenuItem();
        this.MMnuItemBrowseReflection = new System.Windows.Forms.MenuItem();
        this.MBnOk = new System.Windows.Forms.Button();
        this.MLvData = new System.Windows.Forms.ListView();
        this.MLvColLabel = new System.Windows.Forms.ColumnHeader();
        this.MLvColValue = new System.Windows.Forms.ColumnHeader();
        this._listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
        this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this._toolStrip1 = new System.Windows.Forms.ToolStrip();
        this._toolStripButton1 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton2 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton3 = new System.Windows.Forms.ToolStripButton();
        this._printDialog = new System.Windows.Forms.PrintDialog();
        this._printDocument = new System.Drawing.Printing.PrintDocument();
        this._printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
        this.panel1 = new System.Windows.Forms.Panel();
        this._listViewContextMenuStrip.SuspendLayout();
        this._toolStrip1.SuspendLayout();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // TvObjs
        // 
        this.TvObjs.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
        this.TvObjs.ContextMenu = this.MCntxMenuObjId;
        this.TvObjs.HideSelection = false;
        this.TvObjs.Location = new System.Drawing.Point(3, 3);
        this.TvObjs.Name = "TvObjs";
        this.TvObjs.Size = new System.Drawing.Size(248, 446);
        this.TvObjs.TabIndex = 0;
        this.TvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
        // 
        // MCntxMenuObjId
        // 
        this.MCntxMenuObjId.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this._mnuItemCopy, this.MMnuItemBrowseReflection});
        // 
        // _mnuItemCopy
        // 
        this._mnuItemCopy.Index = 0;
        this._mnuItemCopy.Text = "Copy";
        this._mnuItemCopy.Click += new System.EventHandler(this.ContextMenuClick_Copy);
        // 
        // MMnuItemBrowseReflection
        // 
        this.MMnuItemBrowseReflection.Index = 1;
        this.MMnuItemBrowseReflection.Text = "Browse Using Reflection...";
        this.MMnuItemBrowseReflection.Click += new System.EventHandler(this.ContextMenuClick_BrowseReflection);
        // 
        // MBnOk
        // 
        this.MBnOk.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.MBnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.MBnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.MBnOk.Location = new System.Drawing.Point(257, 426);
        this.MBnOk.Name = "MBnOk";
        this.MBnOk.Size = new System.Drawing.Size(540, 23);
        this.MBnOk.TabIndex = 2;
        this.MBnOk.Text = "OK";
        this.MBnOk.Click += new System.EventHandler(this.m_bnOK_Click);
        // 
        // MLvData
        // 
        this.MLvData.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.MLvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.MLvColLabel, this.MLvColValue});
        this.MLvData.ContextMenuStrip = this._listViewContextMenuStrip;
        this.MLvData.FullRowSelect = true;
        this.MLvData.GridLines = true;
        this.MLvData.HideSelection = false;
        this.MLvData.Location = new System.Drawing.Point(257, 3);
        this.MLvData.Name = "MLvData";
        this.MLvData.Size = new System.Drawing.Size(540, 417);
        this.MLvData.TabIndex = 3;
        this.MLvData.UseCompatibleStateImageBehavior = false;
        this.MLvData.View = System.Windows.Forms.View.Details;
        this.MLvData.Click += new System.EventHandler(this.DataItemSelected);
        this.MLvData.DoubleClick += new System.EventHandler(this.DataItemSelected);
        // 
        // MLvColLabel
        // 
        this.MLvColLabel.Text = "Field";
        this.MLvColLabel.Width = 200;
        // 
        // MLvColValue
        // 
        this.MLvColValue.Text = "Value";
        this.MLvColValue.Width = 800;
        // 
        // _listViewContextMenuStrip
        // 
        this._listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this._copyToolStripMenuItem});
        this._listViewContextMenuStrip.Name = "_listViewContextMenuStrip";
        this._listViewContextMenuStrip.Size = new System.Drawing.Size(103, 26);
        // 
        // _copyToolStripMenuItem
        // 
        this._copyToolStripMenuItem.Image = global::RevitLookup.Properties.Resources.Copy;
        this._copyToolStripMenuItem.Name = "_copyToolStripMenuItem";
        this._copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
        this._copyToolStripMenuItem.Text = "Copy";
        this._copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
        // 
        // _toolStrip1
        // 
        this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this._toolStripButton1, this._toolStripButton2, this._toolStripButton3});
        this._toolStrip1.Location = new System.Drawing.Point(0, 0);
        this._toolStrip1.Name = "_toolStrip1";
        this._toolStrip1.Size = new System.Drawing.Size(800, 25);
        this._toolStrip1.TabIndex = 5;
        this._toolStrip1.Text = "toolStrip1";
        // 
        // _toolStripButton1
        // 
        this._toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButton1.Image = global::RevitLookup.Properties.Resources.Print;
        this._toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButton1.Name = "_toolStripButton1";
        this._toolStripButton1.Size = new System.Drawing.Size(23, 22);
        this._toolStripButton1.Text = "Print";
        this._toolStripButton1.Click += new System.EventHandler(this.PrintMenuItem_Click);
        // 
        // _toolStripButton2
        // 
        this._toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButton2.Image = global::RevitLookup.Properties.Resources.Preview;
        this._toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButton2.Name = "_toolStripButton2";
        this._toolStripButton2.Size = new System.Drawing.Size(23, 22);
        this._toolStripButton2.Text = "Print Preview";
        this._toolStripButton2.Click += new System.EventHandler(this.PrintPreviewMenuItem_Click);
        // 
        // _toolStripButton3
        // 
        this._toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this._toolStripButton3.Image = global::RevitLookup.Properties.Resources.Copy;
        this._toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
        this._toolStripButton3.Name = "_toolStripButton3";
        this._toolStripButton3.Size = new System.Drawing.Size(23, 22);
        this._toolStripButton3.Text = "Copy To Clipboard";
        this._toolStripButton3.Click += new System.EventHandler(this.ContextMenuClick_Copy);
        // 
        // _printDialog
        // 
        this._printDialog.Document = this._printDocument;
        this._printDialog.UseEXDialog = true;
        // 
        // _printDocument
        // 
        this._printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
        // 
        // _printPreviewDialog
        // 
        this._printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
        this._printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
        this._printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
        this._printPreviewDialog.Document = this._printDocument;
        this._printPreviewDialog.Enabled = true;
        this._printPreviewDialog.Icon = ((System.Drawing.Icon) (resources.GetObject("_printPreviewDialog.Icon")));
        this._printPreviewDialog.Name = "_printPreviewDialog";
        this._printPreviewDialog.Visible = false;
        // 
        // panel1
        // 
        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.panel1.Controls.Add(this.MBnOk);
        this.panel1.Controls.Add(this.TvObjs);
        this.panel1.Controls.Add(this.MLvData);
        this.panel1.Location = new System.Drawing.Point(0, 28);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(800, 454);
        this.panel1.TabIndex = 6;
        // 
        // ParamEnumSnoopView
        // 
        this.AcceptButton = this.MBnOk;
        this.CancelButton = this.MBnOk;
        this.ClientSize = new System.Drawing.Size(800, 489);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this._toolStrip1);
        this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
        this.MinimumSize = new System.Drawing.Size(650, 200);
        this.Name = "ParamEnumSnoopView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Snoop Built-In Parameters";
        this._listViewContextMenuStrip.ResumeLayout(false);
        this._toolStrip1.ResumeLayout(false);
        this._toolStrip1.PerformLayout();
        this.panel1.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Panel panel1;

    #endregion

    private void AddParametersToTree()
    {
        TvObjs.Sorted = true;

        foreach (DictionaryEntry de in _enumMap)
        {
            var tmpNode = new TreeNode(de.Key.ToString())
            {
                Tag = de.Value
            };

            TvObjs.Nodes.Add(tmpNode);
        }
    }

    private void m_bnOK_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
        Dispose();
    }

    #region Events

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void TreeNodeSelected(object sender, TreeViewEventArgs e)
    {
        MCurObj = e.Node.Tag;
        await MSnoopCollector.Collect(MCurObj);
        Core.Utils.Display(MLvData, MSnoopCollector);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataItemSelected(object sender, EventArgs e)
    {
        Core.Utils.DataItemSelected(MLvData, new ModelessWindowFactory(this, MSnoopCollector.Document));
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContextMenuClick_Copy(object sender, EventArgs e)
    {
        if (TvObjs.SelectedNode is not null) Core.Utils.CopyToClipboard(MLvData);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContextMenuClick_BrowseReflection(object sender, EventArgs e)
    {
        Core.Utils.BrowseReflection(MCurObj);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (MLvData.SelectedItems.Count > 0)
            Core.Utils.CopyToClipboard(MLvData.SelectedItems[0], false);
        else
            Clipboard.Clear();
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_printDocument, TvObjs, MLvData, ref _maxWidths);
        Core.Utils.PrintMenuItemClick(_printDialog, TvObjs);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_printDocument, TvObjs, MLvData, ref _maxWidths);
        Core.Utils.PrintPreviewMenuItemClick(_printPreviewDialog, TvObjs);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        _currentPrintItem = Core.Utils.Print(TvObjs.SelectedNode.Text, MLvData, e, _maxWidths[0], _maxWidths[1], _currentPrintItem);
    }

    #endregion
}