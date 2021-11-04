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
using System.ComponentModel;
using System.Windows.Forms;
using RevitLookup.Snoop;

namespace RevitLookup.Forms
{
	/// <summary>
	///     This Form makes use of the built-in abilities of Reflection and the PropertyGrid
	///     class to automatically list out the data of an object.  Because we don't have much
	///     control, it will get output in the order that it chooses (or alphabetical) and
	///     most of the items will appear as Greyed-out, read-only items.  But, we don't have
	///     to go write a SnoopCollector for every item in the .NET system.
	/// </summary>
	public class GenericPropGrid : Form
    {
	    /// <summary>
	    ///     Required designer variable.
	    /// </summary>
	    private readonly Container _components = null;

        private Button _mBnCancel;
        private Button _mBnOk;
        private ContextMenu _mMnuContext;
        private MenuItem _mMnuItemShowClassInfo;
        private MenuItem _mMnuItemShowObjInfo;
        private PropertyGrid _mPgProps;

        public
            GenericPropGrid(object obj)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Add Load to update ListView Width
            Utils.AddOnLoadForm(this);

            _mPgProps.SelectedObject = obj; // This all we need to do for Reflection to kick in
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void
            Dispose(bool disposing)
        {
            if (disposing)
            {
                _components?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericPropGrid));
            this._mPgProps = new System.Windows.Forms.PropertyGrid();
            this._mMnuContext = new System.Windows.Forms.ContextMenu();
            this._mMnuItemShowObjInfo = new System.Windows.Forms.MenuItem();
            this._mMnuItemShowClassInfo = new System.Windows.Forms.MenuItem();
            this._mBnOk = new System.Windows.Forms.Button();
            this._mBnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_pgProps
            // 
            this._mPgProps.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                            | System.Windows.Forms.AnchorStyles.Left)
                                                                           | System.Windows.Forms.AnchorStyles.Right)));
            this._mPgProps.ContextMenu = this._mMnuContext;
            this._mPgProps.Cursor = System.Windows.Forms.Cursors.Hand;
            this._mPgProps.LineColor = System.Drawing.SystemColors.ScrollBar;
            this._mPgProps.Location = new System.Drawing.Point(16, 16);
            this._mPgProps.Name = "_mPgProps";
            this._mPgProps.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this._mPgProps.Size = new System.Drawing.Size(472, 384);
            this._mPgProps.TabIndex = 0;
            // 
            // m_mnuContext
            // 
            this._mMnuContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this._mMnuItemShowObjInfo,
                this._mMnuItemShowClassInfo
            });
            this._mMnuContext.Popup += new System.EventHandler(this.OnMenuContextPopup);
            // 
            // m_mnuItemShowObjInfo
            // 
            this._mMnuItemShowObjInfo.Index = 0;
            this._mMnuItemShowObjInfo.Text = "Show Object Info...";
            this._mMnuItemShowObjInfo.Click += new System.EventHandler(this.OnShowObjInfo);
            // 
            // m_mnuItemShowClassInfo
            // 
            this._mMnuItemShowClassInfo.Index = 1;
            this._mMnuItemShowClassInfo.Text = "Show Class Info...";
            this._mMnuItemShowClassInfo.Click += new System.EventHandler(this.OnShowClassInfo);
            // 
            // m_bnOK
            // 
            this._mBnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._mBnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._mBnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnOk.Location = new System.Drawing.Point(171, 416);
            this._mBnOk.Name = "_mBnOk";
            this._mBnOk.Size = new System.Drawing.Size(75, 23);
            this._mBnOk.TabIndex = 1;
            this._mBnOk.Text = "OK";
            this._mBnOk.Click += new System.EventHandler(this.m_bnOK_Click);
            // 
            // m_bnCancel
            // 
            this._mBnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._mBnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._mBnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnCancel.Location = new System.Drawing.Point(259, 416);
            this._mBnCancel.Name = "_mBnCancel";
            this._mBnCancel.Size = new System.Drawing.Size(75, 23);
            this._mBnCancel.TabIndex = 3;
            this._mBnCancel.Text = "Cancel";
            this._mBnCancel.Click += new System.EventHandler(this.m_bnOK_Click);
            // 
            // GenericPropGrid
            // 
            this.AcceptButton = this._mBnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._mBnCancel;
            this.ClientSize = new System.Drawing.Size(504, 454);
            this.Controls.Add(this._mBnCancel);
            this.Controls.Add(this._mBnOk);
            this.Controls.Add(this._mPgProps);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 250);
            this.Name = "GenericPropGrid";
            this.ShowInTaskbar = false;
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
            var selObj = _mPgProps.SelectedGridItem.Value;

            if (selObj == null)
            {
                MessageBox.Show("Value is null.");
            }
            else
            {
                var pgForm = new GenericPropGrid(selObj);
                pgForm.Text = $"{_mPgProps.SelectedGridItem.Label} (Object Info: {selObj.GetType()})";
                pgForm.ShowDialog();
            }
        }

        /// <summary>
        ///     User chose "Show Class Info..." from the context menu.  Allow them to browse
        ///     using Reflection for the class of the sub-object selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            OnShowClassInfo(object sender, EventArgs e)
        {
            var selObj = _mPgProps.SelectedGridItem.Value;

            if (selObj == null)
            {
                MessageBox.Show("Value is null.");
            }
            else
            {
                var pgForm = new GenericPropGrid(selObj.GetType());
                pgForm.Text = $"{_mPgProps.SelectedGridItem.Label} (System.Type = {selObj.GetType().FullName})";
                pgForm.ShowDialog();
            }
        }

        /// <summary>
        ///     disable any menu options if there is no current item selected in the PropGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
            OnMenuContextPopup(object sender, EventArgs e)
        {
            var enabled = _mPgProps.SelectedGridItem == null ? false : true;
            _mMnuItemShowObjInfo.Enabled = enabled;
            _mMnuItemShowClassInfo.Enabled = enabled;
        }

        private void m_bnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            Dispose();
        }
    }
}