#region Header
//
// Copyright 2003-2020 by Autodesk, Inc. 
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

namespace RevitLookup.Snoop.Forms
{
	/// <summary>
	/// This Form makes use of the built-in abilities of Reflection and the PropertyGrid
	/// class to automatically list out the data of an object.  Because we don't have much
	/// control, it will get output in the order that it chooses (or alphabetical) and
	/// most of the items will appear as Greyed-out, read-only items.  But, we don't have
	/// to go write a SnoopCollector for every item in the .NET system.
	/// </summary>
	
	public class GenericPropGrid : System.Windows.Forms.Form
	{
        private System.Windows.Forms.PropertyGrid   m_pgProps;
        private System.Windows.Forms.Button         m_bnOK;
        private System.Windows.Forms.Button         m_bnCancel;
        private System.Windows.Forms.ContextMenu    m_mnuContext;
        private System.Windows.Forms.MenuItem       m_mnuItemShowObjInfo;
        private System.Windows.Forms.MenuItem       m_mnuItemShowClassInfo;
                
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public
		GenericPropGrid(object obj)
		{
			    // Required for Windows Form Designer support
			InitializeComponent();
			
            m_pgProps.SelectedObject = obj; // This all we need to do for Reflection to kick in
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void
		Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericPropGrid));
			this.m_pgProps = new System.Windows.Forms.PropertyGrid();
			this.m_mnuContext = new System.Windows.Forms.ContextMenu();
			this.m_mnuItemShowObjInfo = new System.Windows.Forms.MenuItem();
			this.m_mnuItemShowClassInfo = new System.Windows.Forms.MenuItem();
			this.m_bnOK = new System.Windows.Forms.Button();
			this.m_bnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_pgProps
			// 
			this.m_pgProps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_pgProps.ContextMenu = this.m_mnuContext;
			this.m_pgProps.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_pgProps.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.m_pgProps.Location = new System.Drawing.Point(16, 16);
			this.m_pgProps.Name = "m_pgProps";
			this.m_pgProps.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			this.m_pgProps.Size = new System.Drawing.Size(472, 384);
			this.m_pgProps.TabIndex = 0;
			// 
			// m_mnuContext
			// 
			this.m_mnuContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.m_mnuItemShowObjInfo,
            this.m_mnuItemShowClassInfo});
			this.m_mnuContext.Popup += new System.EventHandler(this.OnMenuContextPopup);
			// 
			// m_mnuItemShowObjInfo
			// 
			this.m_mnuItemShowObjInfo.Index = 0;
			this.m_mnuItemShowObjInfo.Text = "Show Object Info...";
			this.m_mnuItemShowObjInfo.Click += new System.EventHandler(this.OnShowObjInfo);
			// 
			// m_mnuItemShowClassInfo
			// 
			this.m_mnuItemShowClassInfo.Index = 1;
			this.m_mnuItemShowClassInfo.Text = "Show Class Info...";
			this.m_mnuItemShowClassInfo.Click += new System.EventHandler(this.OnShowClassInfo);
			// 
			// m_bnOK
			// 
			this.m_bnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.m_bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_bnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_bnOK.Location = new System.Drawing.Point(171, 416);
			this.m_bnOK.Name = "m_bnOK";
			this.m_bnOK.Size = new System.Drawing.Size(75, 23);
			this.m_bnOK.TabIndex = 1;
			this.m_bnOK.Text = "OK";
			// 
			// m_bnCancel
			// 
			this.m_bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.m_bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_bnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_bnCancel.Location = new System.Drawing.Point(259, 416);
			this.m_bnCancel.Name = "m_bnCancel";
			this.m_bnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_bnCancel.TabIndex = 3;
			this.m_bnCancel.Text = "Cancel";
			// 
			// GenericPropGrid
			// 
			this.AcceptButton = this.m_bnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_bnCancel;
			this.ClientSize = new System.Drawing.Size(504, 454);
			this.Controls.Add(this.m_bnCancel);
			this.Controls.Add(this.m_bnOK);
			this.Controls.Add(this.m_pgProps);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        /// User chose "Show Object Info..." from the context menu.  Allow them to browse
        /// using Reflection for the sub-object selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        OnShowObjInfo(object sender, System.EventArgs e)
        {
            object selObj = m_pgProps.SelectedGridItem.Value;

            if (selObj == null)
                MessageBox.Show("Value is null.");
            else {
                GenericPropGrid pgForm = new GenericPropGrid(selObj);
                pgForm.Text = string.Format("{0} (Object Info: {1})", m_pgProps.SelectedGridItem.Label, selObj.GetType());
                pgForm.ShowDialog();
            }
        }

        /// <summary>
        /// User chose "Show Class Info..." from the context menu.  Allow them to browse
        /// using Reflection for the class of the sub-object selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        OnShowClassInfo(object sender, System.EventArgs e)
        {
            object selObj = m_pgProps.SelectedGridItem.Value;

            if (selObj == null)
                MessageBox.Show("Value is null.");
            else {
                GenericPropGrid pgForm = new GenericPropGrid(selObj.GetType());
                pgForm.Text = string.Format("{0} (System.Type = {1})", m_pgProps.SelectedGridItem.Label, selObj.GetType().FullName);
                pgForm.ShowDialog();
            }
        }

        /// <summary>
        /// disable any menu options if there is no current item selected in the PropGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        OnMenuContextPopup(object sender, System.EventArgs e)
        {
            bool enabled = (m_pgProps.SelectedGridItem == null) ? false : true;
            m_mnuItemShowObjInfo.Enabled = enabled;
            m_mnuItemShowClassInfo.Enabled = enabled;
        }
        
	}
}
