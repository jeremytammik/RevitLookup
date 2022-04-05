// Copyright 2003-2022 by Autodesk, Inc. 
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

namespace RevitLookup.Views;

/// <summary>
///     This Form makes use of the built-in abilities of Reflection and the PropertyGrid
///     class to automatically list out the data of an object.  Because we don't have much
///     control, it will get output in the order that it chooses (or alphabetical) and
///     most of the items will appear as Greyed-out, read-only items.  But, we don't have
///     to go write a SnoopCollector for every item in the .NET system.
/// </summary>
public class GenericPropGridView : Form
{
    /// <summary>
    ///     Required designer variable.
    /// </summary>
    private readonly Container _components = null;

    private Button _bnCancel;
    private Button _bnOk;
    private ContextMenu _mnuContext;
    private MenuItem _mnuItemShowClassInfo;
    private MenuItem _mnuItemShowObjInfo;
    private PropertyGrid _pgProps;

    public GenericPropGridView(object obj)
    {
        // Required for Windows Form Designer support
        InitializeComponent();

        // Add Load to update ListView Width
        Core.Utils.AddOnLoadForm(this);
        _pgProps.SelectedObject = obj; // This all we need to do for Reflection to kick in
    }

    /// <summary>
    ///     Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing) _components?.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///     Required method for Designer support - do not modify
    ///     the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericPropGridView));
        this._pgProps = new System.Windows.Forms.PropertyGrid();
        this._mnuContext = new System.Windows.Forms.ContextMenu();
        this._mnuItemShowObjInfo = new System.Windows.Forms.MenuItem();
        this._mnuItemShowClassInfo = new System.Windows.Forms.MenuItem();
        this._bnOk = new System.Windows.Forms.Button();
        this._bnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // _pgProps
        // 
        this._pgProps.Anchor =
            ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                                                   System.Windows.Forms.AnchorStyles.Right)));
        this._pgProps.ContextMenu = this._mnuContext;
        this._pgProps.Cursor = System.Windows.Forms.Cursors.Hand;
        this._pgProps.LineColor = System.Drawing.SystemColors.ScrollBar;
        this._pgProps.Location = new System.Drawing.Point(16, 16);
        this._pgProps.Name = "_pgProps";
        this._pgProps.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
        this._pgProps.Size = new System.Drawing.Size(602, 384);
        this._pgProps.TabIndex = 0;
        // 
        // _mnuContext
        // 
        this._mnuContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this._mnuItemShowObjInfo, this._mnuItemShowClassInfo});
        this._mnuContext.Popup += new System.EventHandler(this.OnMenuContextPopup);
        // 
        // _mnuItemShowObjInfo
        // 
        this._mnuItemShowObjInfo.Index = 0;
        this._mnuItemShowObjInfo.Text = "Show Object Info...";
        this._mnuItemShowObjInfo.Click += new System.EventHandler(this.OnShowObjInfo);
        // 
        // _mnuItemShowClassInfo
        // 
        this._mnuItemShowClassInfo.Index = 1;
        this._mnuItemShowClassInfo.Text = "Show Class Info...";
        this._mnuItemShowClassInfo.Click += new System.EventHandler(this.OnShowClassInfo);
        // 
        // _bnOk
        // 
        this._bnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this._bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this._bnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnOk.Location = new System.Drawing.Point(236, 416);
        this._bnOk.Name = "_bnOk";
        this._bnOk.Size = new System.Drawing.Size(75, 23);
        this._bnOk.TabIndex = 1;
        this._bnOk.Text = "OK";
        this._bnOk.Click += new System.EventHandler(this.ButtonOK_Click);
        // 
        // _bnCancel
        // 
        this._bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this._bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this._bnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnCancel.Location = new System.Drawing.Point(324, 416);
        this._bnCancel.Name = "_bnCancel";
        this._bnCancel.Size = new System.Drawing.Size(75, 23);
        this._bnCancel.TabIndex = 3;
        this._bnCancel.Text = "Cancel";
        this._bnCancel.Click += new System.EventHandler(this.ButtonOK_Click);
        // 
        // GenericPropGridView
        // 
        this.AcceptButton = this._bnOk;
        this.CancelButton = this._bnCancel;
        this.ClientSize = new System.Drawing.Size(634, 454);
        this.Controls.Add(this._bnCancel);
        this.Controls.Add(this._bnOk);
        this.Controls.Add(this._pgProps);
        this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
        this.MinimumSize = new System.Drawing.Size(650, 200);
        this.Name = "GenericPropGridView";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "PropGrid";
        this.ResumeLayout(false);
    }

    #endregion


    /// <summary>
    ///     User chose "Show Object Info..." from the context menu.  Allow them to browse
    ///     using Reflection for the sub-object selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnShowObjInfo(object sender, EventArgs e)
    {
        var selObj = _pgProps.SelectedGridItem.Value;

        if (selObj is null)
        {
            MessageBox.Show("Value is null.");
        }
        else
        {
            var pgForm = new GenericPropGridView(selObj);
            pgForm.Text = $"{_pgProps.SelectedGridItem.Label} (Object Info: {selObj.GetType()})";
            pgForm.ShowDialog();
        }
    }

    /// <summary>
    ///     User chose "Show Class Info..." from the context menu.  Allow them to browse
    ///     using Reflection for the class of the sub-object selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnShowClassInfo(object sender, EventArgs e)
    {
        var selObj = _pgProps.SelectedGridItem.Value;

        if (selObj is null)
        {
            MessageBox.Show("Value is null.");
        }
        else
        {
            var pgForm = new GenericPropGridView(selObj.GetType());
            pgForm.Text = $"{_pgProps.SelectedGridItem.Label} (System.Type = {selObj.GetType().FullName})";
            pgForm.ShowDialog();
        }
    }

    /// <summary>
    ///     disable any menu options if there is no current item selected in the PropGrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMenuContextPopup(object sender, EventArgs e)
    {
        var enabled = _pgProps.SelectedGridItem is not null;
        _mnuItemShowObjInfo.Enabled = enabled;
        _mnuItemShowClassInfo.Enabled = enabled;
    }

    private void ButtonOK_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
        Dispose();
    }
}