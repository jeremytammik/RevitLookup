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

using System.ComponentModel;
using System.Drawing.Printing;
using Autodesk.Revit.DB;
using RevitLookup.Core;
using RevitLookup.Core.Collectors;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Views;

/// <summary>
///     Summary description for ObjTreeBase form.
/// </summary>
public class ObjTreeBaseView : Form, IHaveCollector
{
    private readonly CollectorObj _mSnoopCollector = new();
    private ToolStripMenuItem _copyToolStripMenuItem;
    private ContextMenuStrip _listViewContextMenuStrip;
    private ColumnHeader _lvColLabel;
    private ColumnHeader _lvColValue;
    private object _mCurObj;
    private int _mCurrentPrintItem;
    private int[] _mMaxWidths;
    private MenuItem _mMnuItemCopy;
    private MenuItem _mnuItemBrowseReflection;
    private PrintDialog _mPrintDialog;
    private PrintDocument _mPrintDocument;
    private PrintPreviewDialog _mPrintPreviewDialog;
    private ToolStrip _toolStrip1;
    private ToolStripButton _toolStripButton1;
    private ToolStripButton _toolStripButton2;
    private ToolStripButton _toolStripButton3;
    protected Button BnOk;
    protected ContextMenu CntxMenuObjId;
    private IContainer components;
    protected ListView LvData;
    protected TreeView TvObjs;


    public ObjTreeBaseView()
    {
        InitializeComponent();

        // Add Load to update ListView Width
        Core.Utils.AddOnLoadForm(this);

        // derived classes are responsible for populating the tree
    }

    public Document Document
    {
        set => _mSnoopCollector.Document = value;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjTreeBaseView));
        this.TvObjs = new System.Windows.Forms.TreeView();
        this.CntxMenuObjId = new System.Windows.Forms.ContextMenu();
        this._mMnuItemCopy = new System.Windows.Forms.MenuItem();
        this._mnuItemBrowseReflection = new System.Windows.Forms.MenuItem();
        this.BnOk = new System.Windows.Forms.Button();
        this.LvData = new System.Windows.Forms.ListView();
        this._lvColLabel = new System.Windows.Forms.ColumnHeader();
        this._lvColValue = new System.Windows.Forms.ColumnHeader();
        this._listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
        this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this._toolStrip1 = new System.Windows.Forms.ToolStrip();
        this._toolStripButton1 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton2 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton3 = new System.Windows.Forms.ToolStripButton();
        this._mPrintDialog = new System.Windows.Forms.PrintDialog();
        this._mPrintDocument = new System.Drawing.Printing.PrintDocument();
        this._mPrintPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
        this._listViewContextMenuStrip.SuspendLayout();
        this._toolStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // TvObjs
        // 
        this.TvObjs.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
        this.TvObjs.ContextMenu = this.CntxMenuObjId;
        this.TvObjs.HideSelection = false;
        this.TvObjs.Location = new System.Drawing.Point(12, 28);
        this.TvObjs.Name = "TvObjs";
        this.TvObjs.Size = new System.Drawing.Size(248, 416);
        this.TvObjs.Sorted = true;
        this.TvObjs.TabIndex = 0;
        this.TvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
        // 
        // CntxMenuObjId
        // 
        this.CntxMenuObjId.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this._mMnuItemCopy, this._mnuItemBrowseReflection});
        // 
        // _mMnuItemCopy
        // 
        this._mMnuItemCopy.Index = 0;
        this._mMnuItemCopy.Text = "Copy";
        this._mMnuItemCopy.Click += new System.EventHandler(this.ContextMenuClick_Copy);
        // 
        // _mnuItemBrowseReflection
        // 
        this._mnuItemBrowseReflection.Index = 1;
        this._mnuItemBrowseReflection.Text = "Browse Using Reflection...";
        this._mnuItemBrowseReflection.Click += new System.EventHandler(this.ContextMenuClick_BrowseReflection);
        // 
        // BnOk
        // 
        this.BnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this.BnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.BnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.BnOk.Location = new System.Drawing.Point(364, 448);
        this.BnOk.Name = "BnOk";
        this.BnOk.Size = new System.Drawing.Size(75, 23);
        this.BnOk.TabIndex = 2;
        this.BnOk.Text = "OK";
        this.BnOk.Click += new System.EventHandler(this.m_bnOK_Click);
        // 
        // LvData
        // 
        this.LvData.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.LvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this._lvColLabel, this._lvColValue});
        this.LvData.ContextMenuStrip = this._listViewContextMenuStrip;
        this.LvData.FullRowSelect = true;
        this.LvData.GridLines = true;
        this.LvData.HideSelection = false;
        this.LvData.Location = new System.Drawing.Point(284, 28);
        this.LvData.Name = "LvData";
        this.LvData.ShowItemToolTips = true;
        this.LvData.Size = new System.Drawing.Size(504, 416);
        this.LvData.TabIndex = 3;
        this.LvData.UseCompatibleStateImageBehavior = false;
        this.LvData.View = System.Windows.Forms.View.Details;
        this.LvData.Click += new System.EventHandler(this.DataItemSelected);
        this.LvData.DoubleClick += new System.EventHandler(this.DataItemSelected);
        // 
        // _lvColLabel
        // 
        this._lvColLabel.Text = "Field";
        this._lvColLabel.Width = 200;
        // 
        // _lvColValue
        // 
        this._lvColValue.Text = "Value";
        this._lvColValue.Width = 800;
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
        this._toolStrip1.TabIndex = 4;
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
        // _mPrintDialog
        // 
        this._mPrintDialog.Document = this._mPrintDocument;
        this._mPrintDialog.UseEXDialog = true;
        // 
        // _mPrintDocument
        // 
        this._mPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
        // 
        // _mPrintPreviewDialog
        // 
        this._mPrintPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
        this._mPrintPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
        this._mPrintPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
        this._mPrintPreviewDialog.Document = this._mPrintDocument;
        this._mPrintPreviewDialog.Enabled = true;
        this._mPrintPreviewDialog.Icon = ((System.Drawing.Icon) (resources.GetObject("_mPrintPreviewDialog.Icon")));
        this._mPrintPreviewDialog.Name = "_mPrintPreviewDialog";
        this._mPrintPreviewDialog.Visible = false;
        // 
        // ObjTreeBaseView
        // 
        this.AcceptButton = this.BnOk;
        this.CancelButton = this.BnOk;
        this.ClientSize = new System.Drawing.Size(800, 478);
        this.Controls.Add(this._toolStrip1);
        this.Controls.Add(this.LvData);
        this.Controls.Add(this.BnOk);
        this.Controls.Add(this.TvObjs);
        this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
        this.MinimumSize = new System.Drawing.Size(650, 200);
        this.Name = "ObjTreeBaseView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Snoop Tree";
        this._listViewContextMenuStrip.ResumeLayout(false);
        this._toolStrip1.ResumeLayout(false);
        this._toolStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

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
        _mCurObj = e.Node.Tag;
        await _mSnoopCollector.Collect(_mCurObj);
        Core.Utils.Display(LvData, _mSnoopCollector);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataItemSelected(object sender, EventArgs e)
    {
        Core.Utils.DataItemSelected(LvData, new ModelessWindowFactory(this, _mSnoopCollector.Document));
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContextMenuClick_Copy(object sender, EventArgs e)
    {
        if (TvObjs.SelectedNode is not null) Core.Utils.CopyToClipboard(LvData);
    }


    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContextMenuClick_BrowseReflection(object sender, EventArgs e)
    {
        Core.Utils.BrowseReflection(_mCurObj);
    }


    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (LvData.SelectedItems.Count > 0)
            Core.Utils.CopyToClipboard(LvData.SelectedItems[0], false);
        else
            Clipboard.Clear();
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        _mCurrentPrintItem = Core.Utils.Print(TvObjs.SelectedNode.Text, LvData, e, _mMaxWidths[0], _mMaxWidths[1], _mCurrentPrintItem);
    }


    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_mPrintDocument, TvObjs, LvData, ref _mMaxWidths);
        Core.Utils.PrintMenuItemClick(_mPrintDialog, TvObjs);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_mPrintDocument, TvObjs, LvData, ref _mMaxWidths);
        Core.Utils.PrintPreviewMenuItemClick(_mPrintPreviewDialog, TvObjs);
    }

    #endregion
}