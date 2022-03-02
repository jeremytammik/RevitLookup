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
public class ParametersView : Form, IHaveCollector
{
    private readonly Element _elem;
    private System.Windows.Forms.Button _bnParamEnums;
    private System.Windows.Forms.Button _bnParamEnumsMap;
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
    protected System.Windows.Forms.Button BnOk;
    protected ContextMenu CntxMenuObjId;
    private IContainer components;
    protected object CurObj;
    protected ColumnHeader LvColLabel;
    protected ColumnHeader LvColValue;
    protected System.Windows.Forms.ListView LvData;
    protected MenuItem MnuItemBrowseReflection;

    protected CollectorObj SnoopCollector = new();
    protected System.Windows.Forms.TreeView TvObjs;

    public ParametersView(Element elem, ParameterSet paramSet)
    {
        _elem = elem;

        // this constructor is for derived classes to call
        InitializeComponent();

        // Add Load to update ListView Width
        Core.Utils.AddOnLoadForm(this);

        TvObjs.BeginUpdate();

        AddParametersToTree(paramSet);

        TvObjs.ExpandAll();
        TvObjs.EndUpdate();
    }

    public Document Document
    {
        set => SnoopCollector.Document = value;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParametersView));
        this.TvObjs = new System.Windows.Forms.TreeView();
        this.CntxMenuObjId = new System.Windows.Forms.ContextMenu();
        this._mnuItemCopy = new System.Windows.Forms.MenuItem();
        this.MnuItemBrowseReflection = new System.Windows.Forms.MenuItem();
        this.BnOk = new System.Windows.Forms.Button();
        this.LvData = new System.Windows.Forms.ListView();
        this.LvColLabel = new System.Windows.Forms.ColumnHeader();
        this.LvColValue = new System.Windows.Forms.ColumnHeader();
        this._listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
        this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this._bnParamEnums = new System.Windows.Forms.Button();
        this._toolStrip1 = new System.Windows.Forms.ToolStrip();
        this._toolStripButton1 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton2 = new System.Windows.Forms.ToolStripButton();
        this._toolStripButton3 = new System.Windows.Forms.ToolStripButton();
        this._printDialog = new System.Windows.Forms.PrintDialog();
        this._printDocument = new System.Drawing.Printing.PrintDocument();
        this._printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
        this._bnParamEnumsMap = new System.Windows.Forms.Button();
        this.panel1 = new System.Windows.Forms.Panel();
        this.panel2 = new System.Windows.Forms.Panel();
        this._listViewContextMenuStrip.SuspendLayout();
        this._toolStrip1.SuspendLayout();
        this.panel1.SuspendLayout();
        this.panel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // TvObjs
        // 
        this.TvObjs.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
        this.TvObjs.ContextMenu = this.CntxMenuObjId;
        this.TvObjs.HideSelection = false;
        this.TvObjs.Location = new System.Drawing.Point(3, 3);
        this.TvObjs.Name = "TvObjs";
        this.TvObjs.Size = new System.Drawing.Size(262, 425);
        this.TvObjs.TabIndex = 0;
        this.TvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
        // 
        // CntxMenuObjId
        // 
        this.CntxMenuObjId.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this._mnuItemCopy, this.MnuItemBrowseReflection});
        // 
        // _mnuItemCopy
        // 
        this._mnuItemCopy.Index = 0;
        this._mnuItemCopy.Text = "Copy";
        this._mnuItemCopy.Click += new System.EventHandler(this.ContextMenuClick_Copy);
        // 
        // MnuItemBrowseReflection
        // 
        this.MnuItemBrowseReflection.Index = 1;
        this.MnuItemBrowseReflection.Text = "Browse Using Reflection...";
        this.MnuItemBrowseReflection.Click += new System.EventHandler(this.ContextMenuClick_BrowseReflection);
        // 
        // BnOk
        // 
        this.BnOk.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.BnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.BnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.BnOk.Location = new System.Drawing.Point(268, 4);
        this.BnOk.Name = "BnOk";
        this.BnOk.Size = new System.Drawing.Size(529, 23);
        this.BnOk.TabIndex = 2;
        this.BnOk.Text = "OK";
        this.BnOk.Click += new System.EventHandler(this.m_bnOK_Click);
        // 
        // LvData
        // 
        this.LvData.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.LvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.LvColLabel, this.LvColValue});
        this.LvData.ContextMenuStrip = this._listViewContextMenuStrip;
        this.LvData.FullRowSelect = true;
        this.LvData.GridLines = true;
        this.LvData.HideSelection = false;
        this.LvData.Location = new System.Drawing.Point(271, 3);
        this.LvData.Name = "LvData";
        this.LvData.Size = new System.Drawing.Size(526, 425);
        this.LvData.TabIndex = 3;
        this.LvData.UseCompatibleStateImageBehavior = false;
        this.LvData.View = System.Windows.Forms.View.Details;
        this.LvData.Click += new System.EventHandler(this.DataItemSelected);
        this.LvData.DoubleClick += new System.EventHandler(this.DataItemSelected);
        // 
        // LvColLabel
        // 
        this.LvColLabel.Text = "Field";
        this.LvColLabel.Width = 200;
        // 
        // LvColValue
        // 
        this.LvColValue.Text = "Value";
        this.LvColValue.Width = 800;
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
        // _bnParamEnums
        // 
        this._bnParamEnums.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
        this._bnParamEnums.Location = new System.Drawing.Point(3, 4);
        this._bnParamEnums.Name = "_bnParamEnums";
        this._bnParamEnums.Size = new System.Drawing.Size(130, 23);
        this._bnParamEnums.TabIndex = 4;
        this._bnParamEnums.Text = "Built-in Enums Snoop...";
        this._bnParamEnums.UseVisualStyleBackColor = true;
        this._bnParamEnums.Click += new System.EventHandler(this.OnBnEnumSnoop);
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
        // _bnParamEnumsMap
        // 
        this._bnParamEnumsMap.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
        this._bnParamEnumsMap.Location = new System.Drawing.Point(133, 4);
        this._bnParamEnumsMap.Name = "_bnParamEnumsMap";
        this._bnParamEnumsMap.Size = new System.Drawing.Size(135, 23);
        this._bnParamEnumsMap.TabIndex = 6;
        this._bnParamEnumsMap.Text = "Built-in Enums Map...";
        this._bnParamEnumsMap.UseVisualStyleBackColor = true;
        this._bnParamEnumsMap.Click += new System.EventHandler(this.OnBnEnumMap);
        // 
        // panel1
        // 
        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.panel1.Controls.Add(this.panel2);
        this.panel1.Controls.Add(this.LvData);
        this.panel1.Controls.Add(this.TvObjs);
        this.panel1.Location = new System.Drawing.Point(0, 28);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(800, 460);
        this.panel1.TabIndex = 7;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this._bnParamEnumsMap);
        this.panel2.Controls.Add(this.BnOk);
        this.panel2.Controls.Add(this._bnParamEnums);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel2.Location = new System.Drawing.Point(0, 430);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(800, 30);
        this.panel2.TabIndex = 7;
        // 
        // ParametersView
        // 
        this.AcceptButton = this.BnOk;
        this.CancelButton = this.BnOk;
        this.ClientSize = new System.Drawing.Size(800, 489);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this._toolStrip1);
        this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
        this.MinimumSize = new System.Drawing.Size(650, 200);
        this.Name = "ParametersView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Snoop Parameters";
        this._listViewContextMenuStrip.ResumeLayout(false);
        this._toolStrip1.ResumeLayout(false);
        this._toolStrip1.PerformLayout();
        this.panel1.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panel2;

    #endregion

    private void AddParametersToTree(ParameterSet paramSet)
    {
        TvObjs.Sorted = true;

        // initialize the tree control
        foreach (Parameter tmpObj in paramSet)
        {
            var tmpNode = new TreeNode(Core.Utils.GetParameterObjectLabel(tmpObj))
            {
                Tag = tmpObj
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
        CurObj = e.Node.Tag;
        await SnoopCollector.Collect(CurObj);
        Core.Utils.Display(LvData, SnoopCollector);
    }


    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataItemSelected(object sender, EventArgs e)
    {
        Core.Utils.DataItemSelected(LvData, new ModelessWindowFactory(this, SnoopCollector.Document));
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
        Core.Utils.BrowseReflection(CurObj);
    }


    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBnEnumSnoop(object sender, EventArgs e)
    {
        var enumMap = new Hashtable();

        // TBD: iterating over the Parameters using basic reflection gives us lots
        // of duplicates (not sure why).  Instead, use Enum.GetNames() which will return
        // only unique Enum names.  Then go backward to get the actual BuiltinParam Enum.
        // See TestElements.ParameterEnums() and TestElements.ParameterEnumsNoDup() for an
        // explanation of what is going on.  (jma - 07/24/06)
        var strs = Enum.GetNames(typeof(BuiltInParameter));
        foreach (var str in strs)
        {
            // look up the actual enum from its name
            var paramEnum = (BuiltInParameter) Enum.Parse(typeof(BuiltInParameter), str);

            // see if this Element supports that parameter
            var tmpParam = _elem.get_Parameter(paramEnum);

            if (tmpParam is not null) enumMap.Add(str, tmpParam);
        }

        var form = new ParamEnumSnoopView(enumMap);
        ModelessWindowFactory.Show(form, SnoopCollector.Document, this);
    }


    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBnEnumMap(object sender, EventArgs e)
    {
        var labelStrs = new ArrayList();
        var valueStrs = new ArrayList();

        // TBD: iterating over the Parameters using basic reflection gives us lots
        // of duplicates (not sure why).  Instead, use Enum.GetNames() which will return
        // only unique Enum names.  Then go backward to get the actual BuiltinParam Enum.
        // See TestElements.ParameterEnums() and TestElements.ParameterEnumsNoDup() for an
        // explanation of what is going on.  (jma - 07/24/06)

        var strs = Enum.GetNames(typeof(BuiltInParameter));
        foreach (var str in strs)
        {
            // look up the actual enum from its name
            var paramEnum = (BuiltInParameter) Enum.Parse(typeof(BuiltInParameter), str);

            // see if this Element supports that parameter
            var tmpParam = _elem.get_Parameter(paramEnum);
            if (tmpParam is not null)
            {
                labelStrs.Add(str);
                valueStrs.Add(Core.Utils.GetParameterObjectLabel(tmpParam));
            }
        }

        var dbox = new ParamEnumView(labelStrs, valueStrs);
        dbox.ShowDialog();
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
    private void PrintMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_printDocument, TvObjs, LvData, ref _maxWidths);
        Core.Utils.PrintMenuItemClick(_printDialog, TvObjs);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
    {
        Core.Utils.UpdatePrintSettings(_printDocument, TvObjs, LvData, ref _maxWidths);
        Core.Utils.PrintPreviewMenuItemClick(_printPreviewDialog, TvObjs);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        _currentPrintItem = Core.Utils.Print(TvObjs.SelectedNode.Text, LvData, e, _maxWidths[0], _maxWidths[1], _currentPrintItem);
    }

    #endregion
}