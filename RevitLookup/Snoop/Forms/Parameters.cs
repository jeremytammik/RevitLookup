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
        protected Button           MBnOk;
        protected TreeView         MTvObjs;
        protected ContextMenu      MCntxMenuObjId;
        protected MenuItem         MMnuItemBrowseReflection;
		protected ListView			MLvData;
		protected ColumnHeader		MLvColLabel;
		protected ColumnHeader		MLvColValue;
        private   Button                                _mBnParamEnums;
        private   Button                                _mBnParamEnumsMap;
       
        protected Collectors.CollectorObj         MSnoopCollector            = new Collectors.CollectorObj();
        protected Object							MCurObj;

        private Element                                         _mElem;
        private   ContextMenuStrip                      _listViewContextMenuStrip;
        private   MenuItem         _mMnuItemCopy;
        private   ToolStripMenuItem                     _copyToolStripMenuItem;
        private   ToolStrip                             _toolStrip1;
        private   ToolStripButton                       _toolStripButton1;
        private   ToolStripButton                       _toolStripButton2;
        private   PrintDialog                           _mPrintDialog;
        private   System.Drawing.Printing.PrintDocument _mPrintDocument;
        private   PrintPreviewDialog                    _mPrintPreviewDialog;
        private   IContainer                            components;
        private   Int32[]                               _mMaxWidths;
        private   ToolStripButton                       _toolStripButton3;       
        private   Int32                                 _mCurrentPrintItem           = 0;
		
		public
		Parameters(Element elem, ParameterSet paramSet)
		{
		    _mElem = elem;
		    
		        // this constructor is for derived classes to call
            InitializeComponent();

			// Add Load to update ListView Width
            Utils.AddOnLoadForm(this);

            MTvObjs.BeginUpdate();

            AddParametersToTree(paramSet);
            
			MTvObjs.ExpandAll();
            MTvObjs.EndUpdate();
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
            this.MTvObjs = new System.Windows.Forms.TreeView();
            this.MCntxMenuObjId = new System.Windows.Forms.ContextMenu();
            this._mMnuItemCopy = new System.Windows.Forms.MenuItem();
            this.MMnuItemBrowseReflection = new System.Windows.Forms.MenuItem();
            this.MBnOk = new System.Windows.Forms.Button();
            this.MLvData = new System.Windows.Forms.ListView();
            this.MLvColLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MLvColValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._mBnParamEnums = new System.Windows.Forms.Button();
            this._toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this._mPrintDialog = new System.Windows.Forms.PrintDialog();
            this._mPrintDocument = new System.Drawing.Printing.PrintDocument();
            this._mPrintPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this._mBnParamEnumsMap = new System.Windows.Forms.Button();
            this._listViewContextMenuStrip.SuspendLayout();
            this._toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tvObjs
            // 
            this.MTvObjs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MTvObjs.ContextMenu = this.MCntxMenuObjId;
            this.MTvObjs.HideSelection = false;
            this.MTvObjs.Location = new System.Drawing.Point(11, 38);
            this.MTvObjs.Name = "MTvObjs";
            this.MTvObjs.Size = new System.Drawing.Size(248, 415);
            this.MTvObjs.TabIndex = 0;
            this.MTvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
            // 
            // m_cntxMenuObjId
            // 
            this.MCntxMenuObjId.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._mMnuItemCopy,
            this.MMnuItemBrowseReflection});
            // 
            // m_mnuItemCopy
            // 
            this._mMnuItemCopy.Index = 0;
            this._mMnuItemCopy.Text = "Copy";
            this._mMnuItemCopy.Click += new System.EventHandler(this.ContextMenuClick_Copy);
            // 
            // m_mnuItemBrowseReflection
            // 
            this.MMnuItemBrowseReflection.Index = 1;
            this.MMnuItemBrowseReflection.Text = "Browse Using Reflection...";
            this.MMnuItemBrowseReflection.Click += new System.EventHandler(this.ContextMenuClick_BrowseReflection);
            // 
            // m_bnOK
            // 
            this.MBnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.MBnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MBnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.MBnOk.Location = new System.Drawing.Point(476, 459);
            this.MBnOk.Name = "MBnOk";
            this.MBnOk.Size = new System.Drawing.Size(75, 23);
            this.MBnOk.TabIndex = 2;
            this.MBnOk.Text = "OK";
            this.MBnOk.Click += new System.EventHandler(this.m_bnOK_Click);
            // 
            // m_lvData
            // 
            this.MLvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MLvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MLvColLabel,
            this.MLvColValue});
            this.MLvData.ContextMenuStrip = this._listViewContextMenuStrip;
            this.MLvData.FullRowSelect = true;
            this.MLvData.GridLines = true;
            this.MLvData.HideSelection = false;
            this.MLvData.Location = new System.Drawing.Point(275, 38);
            this.MLvData.Name = "MLvData";
            this.MLvData.Size = new System.Drawing.Size(504, 415);
            this.MLvData.TabIndex = 3;
            this.MLvData.UseCompatibleStateImageBehavior = false;
            this.MLvData.View = System.Windows.Forms.View.Details;
            this.MLvData.Click += new System.EventHandler(this.DataItemSelected);
            this.MLvData.DoubleClick += new System.EventHandler(this.DataItemSelected);
            // 
            // m_lvCol_label
            // 
            this.MLvColLabel.Text = "Field";
            this.MLvColLabel.Width = 200;
            // 
            // m_lvCol_value
            // 
            this.MLvColValue.Text = "Value";
            this.MLvColValue.Width = 800;
            // 
            // listViewContextMenuStrip
            // 
            this._listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._copyToolStripMenuItem});
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
            // m_bnParamEnums
            // 
            this._mBnParamEnums.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._mBnParamEnums.Location = new System.Drawing.Point(11, 459);
            this._mBnParamEnums.Name = "_mBnParamEnums";
            this._mBnParamEnums.Size = new System.Drawing.Size(130, 23);
            this._mBnParamEnums.TabIndex = 4;
            this._mBnParamEnums.Text = "Built-in Enums Snoop...";
            this._mBnParamEnums.UseVisualStyleBackColor = true;
            this._mBnParamEnums.Click += new System.EventHandler(this.OnBnEnumSnoop);
            // 
            // toolStrip1
            // 
            this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripButton1,
            this._toolStripButton2,
            this._toolStripButton3});
            this._toolStrip1.Location = new System.Drawing.Point(0, 0);
            this._toolStrip1.Name = "_toolStrip1";
            this._toolStrip1.Size = new System.Drawing.Size(800, 25);
            this._toolStrip1.TabIndex = 5;
            this._toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this._toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton1.Image = global::RevitLookup.Properties.Resources.Print;
            this._toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton1.Name = "_toolStripButton1";
            this._toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this._toolStripButton1.Text = "Print";
            this._toolStripButton1.Click += new System.EventHandler(this.PrintMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this._toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton2.Image = global::RevitLookup.Properties.Resources.Preview;
            this._toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton2.Name = "_toolStripButton2";
            this._toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this._toolStripButton2.Text = "Print Preview";
            this._toolStripButton2.Click += new System.EventHandler(this.PrintPreviewMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this._toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton3.Image = global::RevitLookup.Properties.Resources.Copy;
            this._toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton3.Name = "_toolStripButton3";
            this._toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this._toolStripButton3.Text = "Copy To Clipboard";
            this._toolStripButton3.Click += new System.EventHandler(this.ContextMenuClick_Copy);
            // 
            // m_printDialog
            // 
            this._mPrintDialog.Document = this._mPrintDocument;
            this._mPrintDialog.UseEXDialog = true;
            // 
            // m_printDocument
            // 
            this._mPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
            // 
            // m_printPreviewDialog
            // 
            this._mPrintPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this._mPrintPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this._mPrintPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this._mPrintPreviewDialog.Document = this._mPrintDocument;
            this._mPrintPreviewDialog.Enabled = true;
            this._mPrintPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("m_printPreviewDialog.Icon")));
            this._mPrintPreviewDialog.Name = "_mPrintPreviewDialog";
            this._mPrintPreviewDialog.Visible = false;
            // 
            // m_bnParamEnumsMap
            // 
            this._mBnParamEnumsMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._mBnParamEnumsMap.Location = new System.Drawing.Point(153, 459);
            this._mBnParamEnumsMap.Name = "_mBnParamEnumsMap";
            this._mBnParamEnumsMap.Size = new System.Drawing.Size(130, 23);
            this._mBnParamEnumsMap.TabIndex = 6;
            this._mBnParamEnumsMap.Text = "Built-in Enums Map...";
            this._mBnParamEnumsMap.UseVisualStyleBackColor = true;
            this._mBnParamEnumsMap.Click += new System.EventHandler(this.OnBnEnumMap);
            // 
            // Parameters
            // 
            this.AcceptButton = this.MBnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.MBnOk;
            this.ClientSize = new System.Drawing.Size(800, 489);
            this.Controls.Add(this._toolStrip1);
            this.Controls.Add(this._mBnParamEnumsMap);
            this.Controls.Add(this._mBnParamEnums);
            this.Controls.Add(this.MLvData);
            this.Controls.Add(this.MBnOk);
            this.Controls.Add(this.MTvObjs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 200);
            this.Name = "Parameters";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Snoop Parameters";
            this._listViewContextMenuStrip.ResumeLayout(false);
            this._toolStrip1.ResumeLayout(false);
            this._toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        protected void
        AddParametersToTree(ParameterSet paramSet)
        {
            MTvObjs.Sorted = true;

                // initialize the tree control
			foreach (Parameter tmpObj in paramSet) {
			    TreeNode tmpNode = new TreeNode(Utils.GetParameterObjectLabel(tmpObj))
			        {
			            Tag = tmpObj
			        };
			    MTvObjs.Nodes.Add(tmpNode);
            }
        }

        public void SetDocument(Document document)
        {
            MSnoopCollector.SourceDocument = document;
        }

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void TreeNodeSelected(object sender, TreeViewEventArgs e)
        {
            MCurObj = e.Node.Tag;
            await MSnoopCollector.Collect(MCurObj);
            Utils.Display(MLvData, MSnoopCollector);
        }    


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void
        DataItemSelected(object sender, EventArgs e)
        {
            Utils.DataItemSelected(MLvData, new ModelessWindowFactory(this, MSnoopCollector.SourceDocument));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        ContextMenuClick_Copy (object sender, EventArgs e)
        {
            if (MTvObjs.SelectedNode != null)
            {
                Utils.CopyToClipboard(MLvData);
            }  
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        ContextMenuClick_BrowseReflection(object sender, EventArgs e)
        {
            Utils.BrowseReflection(MCurObj);
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
            string [] strs = Enum.GetNames(typeof(BuiltInParameter));
            foreach (string str in strs) {
                    // look up the actual enum from its name
                BuiltInParameter paramEnum = (BuiltInParameter)Enum.Parse(typeof(BuiltInParameter), str);
                
                    // see if this Element supports that parameter
                Parameter tmpParam = _mElem.get_Parameter(paramEnum);
                
                if (tmpParam != null) {                                   
                    enumMap.Add(str, tmpParam);
                }
            }

            ParamEnumSnoop form = new ParamEnumSnoop(enumMap);
            ModelessWindowFactory.Show(form, MSnoopCollector.SourceDocument, this);                       
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

            string [] strs = Enum.GetNames(typeof(BuiltInParameter));
            foreach (string str in strs)
            {
                // look up the actual enum from its name
                BuiltInParameter paramEnum = (BuiltInParameter)Enum.Parse(typeof(BuiltInParameter), str);

                // see if this Element supports that parameter
                Parameter tmpParam = _mElem.get_Parameter(paramEnum);
                if (tmpParam != null)
                {
                    labelStrs.Add(str);
                    valueStrs.Add(Utils.GetParameterObjectLabel(tmpParam));
                }
            }

            ParamEnum dbox = new ParamEnum(labelStrs, valueStrs);
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
            if (MLvData.SelectedItems.Count > 0)
            {
                Utils.CopyToClipboard(MLvData.SelectedItems[0], false);
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
            Utils.UpdatePrintSettings(_mPrintDocument, MTvObjs, MLvData, ref _mMaxWidths);
            Utils.PrintMenuItemClick(_mPrintDialog, MTvObjs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintPreviewMenuItem_Click (object sender, EventArgs e)
        {
            Utils.UpdatePrintSettings(_mPrintDocument, MTvObjs, MLvData, ref _mMaxWidths);
            Utils.PrintPreviewMenuItemClick(_mPrintPreviewDialog, MTvObjs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void
        PrintDocument_PrintPage (object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            _mCurrentPrintItem = Utils.Print(MTvObjs.SelectedNode.Text, MLvData, e, _mMaxWidths[0], _mMaxWidths[1], _mCurrentPrintItem);
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
