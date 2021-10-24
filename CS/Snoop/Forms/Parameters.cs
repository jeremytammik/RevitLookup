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
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Revit.DB;


namespace RevitLookup.Snoop.Forms
{
	/// <summary>
	/// Summary description for Object form.
	/// </summary>
	public class Parameters : System.Windows.Forms.Form, IHaveCollector
	{
        protected System.Windows.Forms.Button           m_bnOK;
        protected System.Windows.Forms.TreeView         m_tvObjs;
        protected System.Windows.Forms.ContextMenu      m_cntxMenuObjId;
        protected System.Windows.Forms.MenuItem         m_mnuItemBrowseReflection;
		protected System.Windows.Forms.ListView			m_lvData;
		protected System.Windows.Forms.ColumnHeader		m_lvCol_label;
		protected System.Windows.Forms.ColumnHeader		m_lvCol_value;
        private   Button                                m_bnParamEnums;
        private   Button                                m_bnParamEnumsMap;
       
        protected Snoop.Collectors.CollectorObj         m_snoopCollector            = new Snoop.Collectors.CollectorObj();
        protected System.Object							m_curObj;

        Element                                         m_elem;
        private   ContextMenuStrip                      listViewContextMenuStrip;
        private   System.Windows.Forms.MenuItem         m_mnuItemCopy;
        private   ToolStripMenuItem                     copyToolStripMenuItem;
        private   ToolStrip                             toolStrip1;
        private   ToolStripButton                       toolStripButton1;
        private   ToolStripButton                       toolStripButton2;
        private   PrintDialog                           m_printDialog;
        private   System.Drawing.Printing.PrintDocument m_printDocument;
        private   PrintPreviewDialog                    m_printPreviewDialog;
        private   IContainer                            components;
        private   Int32[]                               m_maxWidths;
        private   ToolStripButton                       toolStripButton3;       
        private   Int32                                 m_currentPrintItem           = 0;
		
		public
		Parameters(Element elem, ParameterSet paramSet)
		{
		    m_elem = elem;
		    
		        // this constructor is for derived classes to call
            InitializeComponent();

			// Add Load to update ListView Width
            Utils.AddOnLoadForm(this);

            m_tvObjs.BeginUpdate();

            AddParametersToTree(paramSet);
            
			m_tvObjs.ExpandAll();
            m_tvObjs.EndUpdate();
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
		protected void
		InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Parameters));
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
            this.m_bnParamEnums = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.m_printDialog = new System.Windows.Forms.PrintDialog();
            this.m_printDocument = new System.Drawing.Printing.PrintDocument();
            this.m_printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.m_bnParamEnumsMap = new System.Windows.Forms.Button();
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
            this.m_tvObjs.Location = new System.Drawing.Point(11, 38);
            this.m_tvObjs.Name = "m_tvObjs";
            this.m_tvObjs.Size = new System.Drawing.Size(248, 415);
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
            this.m_bnOK.Location = new System.Drawing.Point(476, 459);
            this.m_bnOK.Name = "m_bnOK";
            this.m_bnOK.Size = new System.Drawing.Size(75, 23);
            this.m_bnOK.TabIndex = 2;
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
            this.m_lvData.Location = new System.Drawing.Point(275, 38);
            this.m_lvData.Name = "m_lvData";
            this.m_lvData.Size = new System.Drawing.Size(504, 415);
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
            // m_bnParamEnums
            // 
            this.m_bnParamEnums.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_bnParamEnums.Location = new System.Drawing.Point(11, 459);
            this.m_bnParamEnums.Name = "m_bnParamEnums";
            this.m_bnParamEnums.Size = new System.Drawing.Size(130, 23);
            this.m_bnParamEnums.TabIndex = 4;
            this.m_bnParamEnums.Text = "Built-in Enums Snoop...";
            this.m_bnParamEnums.UseVisualStyleBackColor = true;
            this.m_bnParamEnums.Click += new System.EventHandler(this.OnBnEnumSnoop);
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
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
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
            // m_bnParamEnumsMap
            // 
            this.m_bnParamEnumsMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_bnParamEnumsMap.Location = new System.Drawing.Point(153, 459);
            this.m_bnParamEnumsMap.Name = "m_bnParamEnumsMap";
            this.m_bnParamEnumsMap.Size = new System.Drawing.Size(130, 23);
            this.m_bnParamEnumsMap.TabIndex = 6;
            this.m_bnParamEnumsMap.Text = "Built-in Enums Map...";
            this.m_bnParamEnumsMap.UseVisualStyleBackColor = true;
            this.m_bnParamEnumsMap.Click += new System.EventHandler(this.OnBnEnumMap);
            // 
            // Parameters
            // 
            this.AcceptButton = this.m_bnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_bnOK;
            this.ClientSize = new System.Drawing.Size(800, 489);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.m_bnParamEnumsMap);
            this.Controls.Add(this.m_bnParamEnums);
            this.Controls.Add(this.m_lvData);
            this.Controls.Add(this.m_bnOK);
            this.Controls.Add(this.m_tvObjs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 200);
            this.Name = "Parameters";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Snoop Parameters";
            this.listViewContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        protected void
        AddParametersToTree(ParameterSet paramSet)
        {
            m_tvObjs.Sorted = true;

                // initialize the tree control
			foreach (Parameter tmpObj in paramSet) {
			    TreeNode tmpNode = new TreeNode(Utils.GetParameterObjectLabel(tmpObj))
			        {
			            Tag = tmpObj
			        };
			    m_tvObjs.Nodes.Add(tmpNode);
            }
        }

        public void SetDocument(Document document)
        {
            m_snoopCollector.SourceDocument = document;
        }

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void TreeNodeSelected(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            m_curObj = e.Node.Tag;
            await m_snoopCollector.Collect(m_curObj);
            Snoop.Utils.Display(m_lvData, m_snoopCollector);
        }    


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void
        DataItemSelected(object sender, System.EventArgs e)
        {
            Snoop.Utils.DataItemSelected(m_lvData, new ModelessWindowFactory(this, m_snoopCollector.SourceDocument));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        ContextMenuClick_Copy (object sender, System.EventArgs e)
        {
            if (m_tvObjs.SelectedNode != null)
            {
                Utils.CopyToClipboard(m_lvData);
            }  
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        ContextMenuClick_BrowseReflection(object sender, System.EventArgs e)
        {
            Snoop.Utils.BrowseReflection(m_curObj);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        OnBnEnumSnoop(object sender, EventArgs e)
        {                           
            Hashtable    enumMap        = new Hashtable();                       
            
                // TBD: iterating over the Parameters using basic reflection gives us lots
                // of duplicates (not sure why).  Instead, use Enum.GetNames() which will return
                // only unique Enum names.  Then go backward to get the actual BuiltinParam Enum.
                // See TestElements.ParameterEnums() and TestElements.ParameterEnumsNoDup() for an
                // explanation of what is going on.  (jma - 07/24/06)
            string [] strs = System.Enum.GetNames(typeof(BuiltInParameter));
            foreach (string str in strs) {
                    // look up the actual enum from its name
                BuiltInParameter paramEnum = (BuiltInParameter)System.Enum.Parse(typeof(BuiltInParameter), str);
                
                    // see if this Element supports that parameter
                Parameter tmpParam = m_elem.get_Parameter(paramEnum);
                
                if (tmpParam != null) {                                   
                    enumMap.Add(str, tmpParam);
                }
            }

            RevitLookup.Snoop.Forms.ParamEnumSnoop form = new RevitLookup.Snoop.Forms.ParamEnumSnoop(enumMap);
            ModelessWindowFactory.Show(form, m_snoopCollector.SourceDocument, this);                       
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        OnBnEnumMap(object sender, EventArgs e)
        {
            ArrayList labelStrs = new ArrayList();
            ArrayList valueStrs = new ArrayList();                                     
               
            // TBD: iterating over the Parameters using basic reflection gives us lots
            // of duplicates (not sure why).  Instead, use Enum.GetNames() which will return
            // only unique Enum names.  Then go backward to get the actual BuiltinParam Enum.
            // See TestElements.ParameterEnums() and TestElements.ParameterEnumsNoDup() for an
            // explanation of what is going on.  (jma - 07/24/06)

            string [] strs = System.Enum.GetNames(typeof(BuiltInParameter));
            foreach (string str in strs)
            {
                // look up the actual enum from its name
                BuiltInParameter paramEnum = (BuiltInParameter)System.Enum.Parse(typeof(BuiltInParameter), str);

                // see if this Element supports that parameter
                Parameter tmpParam = m_elem.get_Parameter(paramEnum);
                if (tmpParam != null)
                {
                    labelStrs.Add(str);
                    valueStrs.Add(Utils.GetParameterObjectLabel(tmpParam));
                }
            }

            RevitLookup.Snoop.Forms.ParamEnum dbox = new RevitLookup.Snoop.Forms.ParamEnum(labelStrs, valueStrs);
            dbox.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        CopyToolStripMenuItem_Click (object sender, EventArgs e)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintMenuItem_Click (object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(m_printDocument, m_tvObjs, m_lvData, ref m_maxWidths);
            Utils.PrintMenuItemClick(m_printDialog, m_tvObjs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintPreviewMenuItem_Click (object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(m_printDocument, m_tvObjs, m_lvData, ref m_maxWidths);
            Utils.PrintPreviewMenuItemClick(m_printPreviewDialog, m_tvObjs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintDocument_PrintPage (object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            m_currentPrintItem = Utils.Print(m_tvObjs.SelectedNode.Text, m_lvData, e, m_maxWidths[0], m_maxWidths[1], m_currentPrintItem);
        }
        #endregion

        private void m_bnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            Dispose();
        }
    }
}
