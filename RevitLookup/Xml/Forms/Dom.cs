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
using System.Xml;
using RevitLookup.Snoop;

namespace RevitLookup.Xml.Forms
{
	/// <summary>
	/// UI Test for navigating the XML Dom
	/// </summary>
	public class Dom : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TreeView       _mTvDom;
        private System.Windows.Forms.ListView       _mLvData;
        private System.Windows.Forms.RadioButton    _mRbNodeNameOnly;
        private System.Windows.Forms.RadioButton    _mRbNodeAndText;
        private System.Windows.Forms.GroupBox       _mGrpLabelDisplay;
        private System.Windows.Forms.GroupBox       _mGrpNodeDisplay;
        private System.Windows.Forms.CheckBox       _mCbHideCommentNodes;
        private System.Windows.Forms.CheckBox       _mCbHideTextNodes;
        private System.Windows.Forms.Button         _mBnParent;
        private System.Windows.Forms.Button         _mBnOwnerDoc;
        private System.Windows.Forms.Button         _mBnPrevSibling;
        private System.Windows.Forms.Button         _mBnNextSibling;
        private System.Windows.Forms.Button         _mBnFirstChild;
        private System.Windows.Forms.Button         _mBnLastChild;
        private System.Windows.Forms.Button         _mBnDocElem;
        private System.Windows.Forms.TextBox        _mEbXpathPattern;
        private System.Windows.Forms.Button         _mBnSelectSingleNode;
        private System.Windows.Forms.Button         _mBnXpathClear;
        private System.Windows.Forms.Button         _mBnSelectNodes;
        private System.Windows.Forms.Button         _mBnOk;
        private System.Windows.Forms.ColumnHeader   _mLvColLabel;
        private System.Windows.Forms.ColumnHeader   _mLvColValue;
        private System.Windows.Forms.ImageList      _mImgListTree;
        private System.Windows.Forms.Label          _mTxtXpathPattern;
        private System.Windows.Forms.GroupBox       _mGrpXpath;
        
        private System.Xml.XmlDocument              _mXmlDoc = null;
        private Snoop.Collectors.CollectorXmlNode   _mSnoopCollector = new Snoop.Collectors.CollectorXmlNode();
        
        private System.ComponentModel.IContainer components;

		public
		Dom(System.Xml.XmlDocument xmlDoc)
		{
		    _mXmlDoc = xmlDoc;
		    
			InitializeComponent();
			
			LoadTree();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Dom));
            this._mTvDom = new System.Windows.Forms.TreeView();
            this._mImgListTree = new System.Windows.Forms.ImageList(this.components);
            this._mLvData = new System.Windows.Forms.ListView();
            this._mLvColLabel = new System.Windows.Forms.ColumnHeader();
            this._mLvColValue = new System.Windows.Forms.ColumnHeader();
            this._mRbNodeNameOnly = new System.Windows.Forms.RadioButton();
            this._mRbNodeAndText = new System.Windows.Forms.RadioButton();
            this._mGrpLabelDisplay = new System.Windows.Forms.GroupBox();
            this._mGrpNodeDisplay = new System.Windows.Forms.GroupBox();
            this._mCbHideTextNodes = new System.Windows.Forms.CheckBox();
            this._mCbHideCommentNodes = new System.Windows.Forms.CheckBox();
            this._mBnParent = new System.Windows.Forms.Button();
            this._mBnOwnerDoc = new System.Windows.Forms.Button();
            this._mBnPrevSibling = new System.Windows.Forms.Button();
            this._mBnNextSibling = new System.Windows.Forms.Button();
            this._mBnFirstChild = new System.Windows.Forms.Button();
            this._mBnLastChild = new System.Windows.Forms.Button();
            this._mTxtXpathPattern = new System.Windows.Forms.Label();
            this._mEbXpathPattern = new System.Windows.Forms.TextBox();
            this._mBnSelectSingleNode = new System.Windows.Forms.Button();
            this._mBnSelectNodes = new System.Windows.Forms.Button();
            this._mBnOk = new System.Windows.Forms.Button();
            this._mBnXpathClear = new System.Windows.Forms.Button();
            this._mGrpXpath = new System.Windows.Forms.GroupBox();
            this._mBnDocElem = new System.Windows.Forms.Button();
            this._mGrpNodeDisplay.SuspendLayout();
            this._mGrpXpath.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tvDom
            // 
            this._mTvDom.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right);
            this._mTvDom.HideSelection = false;
            this._mTvDom.ImageList = this._mImgListTree;
            this._mTvDom.Location = new System.Drawing.Point(16, 16);
            this._mTvDom.Name = "_mTvDom";
            this._mTvDom.Size = new System.Drawing.Size(336, 416);
            this._mTvDom.TabIndex = 0;
            this._mTvDom.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
            // 
            // m_imgListTree
            // 
            this._mImgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this._mImgListTree.ImageSize = new System.Drawing.Size(16, 16);
            this._mImgListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_imgListTree.ImageStream")));
            this._mImgListTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // m_lvData
            // 
            this._mLvData.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Right);
            this._mLvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       this._mLvColLabel,
                                                                                       this._mLvColValue});
            this._mLvData.FullRowSelect = true;
            this._mLvData.GridLines = true;
            this._mLvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._mLvData.Location = new System.Drawing.Point(368, 16);
            this._mLvData.MultiSelect = false;
            this._mLvData.Name = "_mLvData";
            this._mLvData.Size = new System.Drawing.Size(416, 328);
            this._mLvData.TabIndex = 1;
            this._mLvData.View = System.Windows.Forms.View.Details;
            this._mLvData.Click += new System.EventHandler(this.DataItemSelected);
            // 
            // m_lvColLabel
            // 
            this._mLvColLabel.Text = "Field";
            this._mLvColLabel.Width = 200;
            // 
            // m_lvColValue
            // 
            this._mLvColValue.Text = "Value";
            this._mLvColValue.Width = 750;
            // 
            // m_rbNodeNameOnly
            // 
            this._mRbNodeNameOnly.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mRbNodeNameOnly.Checked = true;
            this._mRbNodeNameOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mRbNodeNameOnly.Location = new System.Drawing.Point(384, 376);
            this._mRbNodeNameOnly.Name = "_mRbNodeNameOnly";
            this._mRbNodeNameOnly.Size = new System.Drawing.Size(152, 24);
            this._mRbNodeNameOnly.TabIndex = 2;
            this._mRbNodeNameOnly.TabStop = true;
            this._mRbNodeNameOnly.Text = "Node Name Only";
            this._mRbNodeNameOnly.CheckedChanged += new System.EventHandler(this.OnRbChanged_LabelDisplay);
            // 
            // m_rbNodeAndText
            // 
            this._mRbNodeAndText.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mRbNodeAndText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mRbNodeAndText.Location = new System.Drawing.Point(384, 400);
            this._mRbNodeAndText.Name = "_mRbNodeAndText";
            this._mRbNodeAndText.Size = new System.Drawing.Size(168, 24);
            this._mRbNodeAndText.TabIndex = 4;
            this._mRbNodeAndText.Text = "Node Name and Value";
            this._mRbNodeAndText.CheckedChanged += new System.EventHandler(this.OnRbChanged_LabelDisplay);
            // 
            // m_grpLabelDisplay
            // 
            this._mGrpLabelDisplay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mGrpLabelDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mGrpLabelDisplay.Location = new System.Drawing.Point(368, 360);
            this._mGrpLabelDisplay.Name = "_mGrpLabelDisplay";
            this._mGrpLabelDisplay.Size = new System.Drawing.Size(200, 72);
            this._mGrpLabelDisplay.TabIndex = 5;
            this._mGrpLabelDisplay.TabStop = false;
            this._mGrpLabelDisplay.Text = "Label Display";
            // 
            // m_grpNodeDisplay
            // 
            this._mGrpNodeDisplay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mGrpNodeDisplay.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                           this._mCbHideTextNodes,
                                                                                           this._mCbHideCommentNodes});
            this._mGrpNodeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mGrpNodeDisplay.Location = new System.Drawing.Point(584, 360);
            this._mGrpNodeDisplay.Name = "_mGrpNodeDisplay";
            this._mGrpNodeDisplay.Size = new System.Drawing.Size(200, 72);
            this._mGrpNodeDisplay.TabIndex = 6;
            this._mGrpNodeDisplay.TabStop = false;
            this._mGrpNodeDisplay.Text = "Node Display";
            // 
            // m_cbHideTextNodes
            // 
            this._mCbHideTextNodes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mCbHideTextNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mCbHideTextNodes.Location = new System.Drawing.Point(16, 40);
            this._mCbHideTextNodes.Name = "_mCbHideTextNodes";
            this._mCbHideTextNodes.Size = new System.Drawing.Size(176, 24);
            this._mCbHideTextNodes.TabIndex = 1;
            this._mCbHideTextNodes.Text = "Hide Text Nodes";
            this._mCbHideTextNodes.CheckedChanged += new System.EventHandler(this.OnCbChanged_NodeDisplay);
            // 
            // m_cbHideCommentNodes
            // 
            this._mCbHideCommentNodes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mCbHideCommentNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mCbHideCommentNodes.Location = new System.Drawing.Point(16, 16);
            this._mCbHideCommentNodes.Name = "_mCbHideCommentNodes";
            this._mCbHideCommentNodes.Size = new System.Drawing.Size(176, 24);
            this._mCbHideCommentNodes.TabIndex = 0;
            this._mCbHideCommentNodes.Text = "Hide Comment Nodes";
            this._mCbHideCommentNodes.CheckedChanged += new System.EventHandler(this.OnCbChanged_NodeDisplay);
            // 
            // m_bnParent
            // 
            this._mBnParent.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnParent.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnParent.Location = new System.Drawing.Point(16, 448);
            this._mBnParent.Name = "_mBnParent";
            this._mBnParent.TabIndex = 7;
            this._mBnParent.Text = "Parent";
            this._mBnParent.Click += new System.EventHandler(this.OnBnParent);
            // 
            // m_bnOwnerDoc
            // 
            this._mBnOwnerDoc.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnOwnerDoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnOwnerDoc.Location = new System.Drawing.Point(104, 448);
            this._mBnOwnerDoc.Name = "_mBnOwnerDoc";
            this._mBnOwnerDoc.TabIndex = 8;
            this._mBnOwnerDoc.Text = "Owner Doc";
            this._mBnOwnerDoc.Click += new System.EventHandler(this.OnBnOwnerDoc);
            // 
            // m_bnPrevSibling
            // 
            this._mBnPrevSibling.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnPrevSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnPrevSibling.Location = new System.Drawing.Point(192, 448);
            this._mBnPrevSibling.Name = "_mBnPrevSibling";
            this._mBnPrevSibling.TabIndex = 9;
            this._mBnPrevSibling.Text = "Prev Sibling";
            this._mBnPrevSibling.Click += new System.EventHandler(this.OnBnPrevSibling);
            // 
            // m_bnNextSibling
            // 
            this._mBnNextSibling.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnNextSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnNextSibling.Location = new System.Drawing.Point(280, 448);
            this._mBnNextSibling.Name = "_mBnNextSibling";
            this._mBnNextSibling.TabIndex = 10;
            this._mBnNextSibling.Text = "Next Sibling";
            this._mBnNextSibling.Click += new System.EventHandler(this.OnBnNextSibling);
            // 
            // m_bnFirstChild
            // 
            this._mBnFirstChild.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnFirstChild.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnFirstChild.Location = new System.Drawing.Point(368, 448);
            this._mBnFirstChild.Name = "_mBnFirstChild";
            this._mBnFirstChild.TabIndex = 11;
            this._mBnFirstChild.Text = "First Child";
            this._mBnFirstChild.Click += new System.EventHandler(this.OnBnFirstChild);
            // 
            // m_bnLastChild
            // 
            this._mBnLastChild.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnLastChild.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnLastChild.Location = new System.Drawing.Point(456, 448);
            this._mBnLastChild.Name = "_mBnLastChild";
            this._mBnLastChild.TabIndex = 12;
            this._mBnLastChild.Text = "Last Child";
            this._mBnLastChild.Click += new System.EventHandler(this.OnBnLastChild);
            // 
            // m_txtXpathPattern
            // 
            this._mTxtXpathPattern.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mTxtXpathPattern.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mTxtXpathPattern.Location = new System.Drawing.Point(32, 512);
            this._mTxtXpathPattern.Name = "_mTxtXpathPattern";
            this._mTxtXpathPattern.Size = new System.Drawing.Size(64, 23);
            this._mTxtXpathPattern.TabIndex = 14;
            this._mTxtXpathPattern.Text = "Expression:";
            // 
            // m_ebXpathPattern
            // 
            this._mEbXpathPattern.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mEbXpathPattern.Location = new System.Drawing.Point(88, 24);
            this._mEbXpathPattern.Name = "_mEbXpathPattern";
            this._mEbXpathPattern.Size = new System.Drawing.Size(424, 20);
            this._mEbXpathPattern.TabIndex = 15;
            this._mEbXpathPattern.Text = "";
            // 
            // m_bnSelectSingleNode
            // 
            this._mBnSelectSingleNode.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnSelectSingleNode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnSelectSingleNode.Location = new System.Drawing.Point(128, 56);
            this._mBnSelectSingleNode.Name = "_mBnSelectSingleNode";
            this._mBnSelectSingleNode.Size = new System.Drawing.Size(120, 23);
            this._mBnSelectSingleNode.TabIndex = 16;
            this._mBnSelectSingleNode.Text = "Select Single Node";
            this._mBnSelectSingleNode.Click += new System.EventHandler(this.OnBnSelectSingleNode);
            // 
            // m_bnSelectNodes
            // 
            this._mBnSelectNodes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnSelectNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnSelectNodes.Location = new System.Drawing.Point(256, 56);
            this._mBnSelectNodes.Name = "_mBnSelectNodes";
            this._mBnSelectNodes.Size = new System.Drawing.Size(120, 23);
            this._mBnSelectNodes.TabIndex = 17;
            this._mBnSelectNodes.Text = "Select Nodes";
            this._mBnSelectNodes.Click += new System.EventHandler(this.OnBnSelectNodes);
            // 
            // m_bnOk
            // 
            this._mBnOk.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this._mBnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._mBnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnOk.Location = new System.Drawing.Point(704, 544);
            this._mBnOk.Name = "_mBnOk";
            this._mBnOk.TabIndex = 18;
            this._mBnOk.Text = "OK";
            // 
            // m_bnXpathClear
            // 
            this._mBnXpathClear.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnXpathClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnXpathClear.Location = new System.Drawing.Point(384, 56);
            this._mBnXpathClear.Name = "_mBnXpathClear";
            this._mBnXpathClear.Size = new System.Drawing.Size(120, 23);
            this._mBnXpathClear.TabIndex = 19;
            this._mBnXpathClear.Text = "Clear";
            this._mBnXpathClear.Click += new System.EventHandler(this.OnBnClear);
            // 
            // m_grpXpath
            // 
            this._mGrpXpath.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mGrpXpath.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                     this._mEbXpathPattern,
                                                                                     this._mBnXpathClear,
                                                                                     this._mBnSelectNodes,
                                                                                     this._mBnSelectSingleNode});
            this._mGrpXpath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mGrpXpath.Location = new System.Drawing.Point(16, 488);
            this._mGrpXpath.Name = "_mGrpXpath";
            this._mGrpXpath.Size = new System.Drawing.Size(520, 88);
            this._mGrpXpath.TabIndex = 20;
            this._mGrpXpath.TabStop = false;
            this._mGrpXpath.Text = "XPath Expressions";
            // 
            // m_bnDocElem
            // 
            this._mBnDocElem.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this._mBnDocElem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._mBnDocElem.Location = new System.Drawing.Point(544, 448);
            this._mBnDocElem.Name = "_mBnDocElem";
            this._mBnDocElem.Size = new System.Drawing.Size(80, 23);
            this._mBnDocElem.TabIndex = 21;
            this._mBnDocElem.Text = "Doc Element";
            this._mBnDocElem.Click += new System.EventHandler(this.OnBnDocElement);
            // 
            // Dom
            // 
            this.AcceptButton = this._mBnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._mBnOk;
            this.ClientSize = new System.Drawing.Size(800, 591);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this._mBnDocElem,
                                                                          this._mBnOk,
                                                                          this._mTxtXpathPattern,
                                                                          this._mBnLastChild,
                                                                          this._mBnFirstChild,
                                                                          this._mBnNextSibling,
                                                                          this._mBnPrevSibling,
                                                                          this._mBnOwnerDoc,
                                                                          this._mBnParent,
                                                                          this._mGrpNodeDisplay,
                                                                          this._mRbNodeAndText,
                                                                          this._mRbNodeNameOnly,
                                                                          this._mLvData,
                                                                          this._mTvDom,
                                                                          this._mGrpLabelDisplay,
                                                                          this._mGrpXpath});
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "Dom";
            this.Text = "XML DOM Tree";
            this._mGrpNodeDisplay.ResumeLayout(false);
            this._mGrpXpath.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// This will populate the UI TreeView with all the nodes from the DOM tree
        /// </summary>
        
        private void
        LoadTree()
        {
            _mTvDom.BeginUpdate();  // suppress redraw events
            _mTvDom.Nodes.Clear();
            
            MakeTree(_mXmlDoc, null);

            _mTvDom.ExpandAll();
            if (_mTvDom.Nodes.Count > 0)
                _mTvDom.SelectedNode = _mTvDom.Nodes[0];    // make first one selected

            _mTvDom.EndUpdate();		// flushes redraw events
        }

        /// <summary>
        /// Recursive function to walk the tree and populate all the nodes
        /// </summary>
        /// <param name="xmlNode">The "root" of this portion of the tree</param>
        /// <param name="parentNode">The parent node to attach to or null for the root</param>
        
        private void
        MakeTree(System.Xml.XmlNode xmlNode, TreeNode parentNode)
        {
            XmlNodeType nType = xmlNode.NodeType;
            
                // bail early if user doesn't want this type of node displayed
            if (((nType == XmlNodeType.Comment) && (_mCbHideCommentNodes.Checked)) ||
                ((nType == XmlNodeType.Text) && (_mCbHideTextNodes.Checked)))
                return;

                // get image index and label to use in the TreeNode
            int imageIndex = GetImageIndex(nType);
            string labelStr = FormatLabel(xmlNode);
            
            TreeNode treeNode = new TreeNode(labelStr);
            treeNode.Tag = xmlNode;
            treeNode.ImageIndex = imageIndex;
            treeNode.SelectedImageIndex = imageIndex;
            
            if (parentNode == null)
                _mTvDom.Nodes.Add(treeNode);    // This is the root node
            else
                parentNode.Nodes.Add(treeNode);
                        
                // get attributes of this node
            XmlAttributeCollection atts = xmlNode.Attributes;
            if (atts != null) {
                foreach (XmlAttribute tmpAtt in atts) {
                    MakeTree(tmpAtt, treeNode);
                }
            }
                // now recursively go to the children of this node
            if (xmlNode.HasChildNodes) {
                foreach (XmlNode childNode in xmlNode.ChildNodes) {
                    MakeTree(childNode, treeNode);
                }
            }
        }

        /// <summary>
        /// Match a particular Image with the XML Node type.  A different Icon will
        /// be used in the UI tree for each type of node.
        /// </summary>
        /// <param name="nType">The node type</param>
        /// <returns>index into the ImageList assigned to the TreeView</returns>
        
        private int
        GetImageIndex(XmlNodeType nType)
        {
            int imageIndex = 0;
            
                // associate the correct image with this type of node
            if (nType == XmlNodeType.Document)
                imageIndex = 0;
            else if (nType == XmlNodeType.Attribute)
                imageIndex = 1;
            else if (nType == XmlNodeType.CDATA)
                imageIndex = 2;
            else if (nType == XmlNodeType.Comment)
                imageIndex = 3;
            else if (nType == XmlNodeType.DocumentType)
                imageIndex = 4;
            else if (nType == XmlNodeType.Element)
                imageIndex = 5;
            else if (nType == XmlNodeType.Entity)
                imageIndex = 6;
            else if (nType == XmlNodeType.DocumentFragment)
                imageIndex = 7;
            else if (nType == XmlNodeType.ProcessingInstruction)
                imageIndex = 8;
            else if (nType == XmlNodeType.EntityReference)
                imageIndex = 9;
            else if (nType == XmlNodeType.Text)
                imageIndex = 10;
            else if (nType == XmlNodeType.XmlDeclaration)
                imageIndex = 11;
                
                    // TBD: Not sure what when the rest of these come up yet?
                    // I will reserve a spot in case they become significant
            else if (nType == XmlNodeType.EndElement)
                imageIndex = 12;
            else if (nType == XmlNodeType.EndEntity)
                imageIndex = 12;
            else if (nType == XmlNodeType.None)
                imageIndex = 12;
            else if (nType == XmlNodeType.Notation)
                imageIndex = 12;
            else if (nType == XmlNodeType.SignificantWhitespace)
                imageIndex = 12;
            else if (nType == XmlNodeType.Whitespace)
                imageIndex = 12;
            else {
                Debug.Assert(false);
                imageIndex = 12;
            }
            
            return imageIndex;
        }
        
        /// <summary>
        /// Allow the user prefernces to affect how we format the label for the tree node.
        /// </summary>
        /// <param name="node">The node to format</param>
        /// <returns>formatted string according to user preferences</returns>
        
        private string
        FormatLabel(XmlNode node)
        {
            string labelStr;
            
            if (_mRbNodeNameOnly.Checked)
                labelStr = node.Name;
            else if (_mRbNodeAndText.Checked) {
                if ((node.NodeType == XmlNodeType.Element) || (node.NodeType == XmlNodeType.Attribute))
                    labelStr = $"{node.Name} ({GetTextLabelValue(node)})";
                else
                    labelStr = $"{node.Name} ({node.Value})";
            }
            else {
                Debug.Assert(false, "Unknown radio button!");   // Someone must have added a button we don't know about!
                labelStr = string.Empty;
            }
            
            return labelStr;
        }
        
        /// <summary>
        /// Retrieve the text value for a given node
        /// </summary>
        /// <param name="node">Node to look at</param>
        /// <returns>Text value of the node (the XmlText Child Node of the one passed in)</returns>
        
        private string
        GetTextLabelValue(XmlNode node)
        {
            XmlNode txtNode = node.FirstChild;
            if ((txtNode != null) && (txtNode.NodeType == XmlNodeType.Text)) {
                return txtNode.Value;
            }
            else {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Display the data values associated with a selected node in the tree.
        /// </summary>
        /// <param name="node">Currently selected node</param>
        
        private void
        Display(XmlNode node)
        {
            SetButtonModes(node);
                                   
            _mSnoopCollector.Collect(node);
            Snoop.Utils.Display(_mLvData, _mSnoopCollector);
        }
        
        /// <summary>
        /// Do a preview check to see which navigation buttons are going to work.  Disable
        /// the ones that will not.
        /// </summary>
        /// <param name="node">Currently selected node</param>
        
        private void
        SetButtonModes(XmlNode node)
        {
            XmlNode tmpNode;
            
            tmpNode = node.ParentNode;
            _mBnParent.Enabled = (tmpNode != null);
            
            tmpNode = node.OwnerDocument;
            _mBnOwnerDoc.Enabled = (tmpNode != null);

            tmpNode = node.PreviousSibling;
            _mBnPrevSibling.Enabled = (tmpNode != null);

            tmpNode = node.NextSibling;
            _mBnNextSibling.Enabled = (tmpNode != null);
            
            tmpNode = node.FirstChild;
            _mBnFirstChild.Enabled = (tmpNode != null);

            tmpNode = node.LastChild;
            _mBnLastChild.Enabled = (tmpNode != null);
        }

        /// <summary>
        /// The user selected a UI TreeNode.  Update the Display based on the new selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        TreeNodeSelected(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            XmlNode curNode = (XmlNode)e.Node.Tag;
            Display(curNode);
        }

        // UI Callbacks when buttons in the Form are pressed
        
        private void
        OnBnParent(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.ParentNode);
        }

        private void
        OnBnOwnerDoc(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.OwnerDocument);
        }

        private void
        OnBnPrevSibling(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.PreviousSibling);
        }

        private void
        OnBnNextSibling(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.NextSibling);
        }

        private void
        OnBnFirstChild(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.FirstChild);
        }

        private void
        OnBnLastChild(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.LastChild);
        }
        
        private void
        OnBnDocElement(object sender, System.EventArgs e)
        {
            XmlElement elem = _mXmlDoc.DocumentElement;
            MoveToNewNodeInTree(elem);
        }

        /// <summary>
        /// Based on a user-specified XPath expression, try to find a matching
        /// node.  If found, change the background of the label to a different
        /// color and make that item the current selection in the UI Tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        OnBnSelectSingleNode(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;

            try {
                XmlNode newNode = curNode.SelectSingleNode(_mEbXpathPattern.Text);
                
                if (newNode != null) {
                    _mTvDom.BeginUpdate();
                   
                    SetSelectedNode(_mTvDom.Nodes, newNode);
                    MoveToNewNodeInTree(newNode);
                    
                    _mTvDom.EndUpdate();
                }
                else {
                    MessageBox.Show("No node matches the pattern.", "XPath Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (System.Xml.XPath.XPathException ex) {
                MessageBox.Show(ex.Message, "XPath Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Based on a user-specified XPath expression, try to find any matching
        /// nodes.  If found, change the background of the labels to a different
        /// color and make the first item the current selection in the UI Tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void
        OnBnSelectNodes(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)_mTvDom.SelectedNode.Tag;

            try {
                XmlNodeList selNodes = curNode.SelectNodes(_mEbXpathPattern.Text);
                if ((selNodes != null) && (selNodes.Count > 0)) {
                    _mTvDom.BeginUpdate();
                    
                    SetSelectedNodes(_mTvDom.Nodes, selNodes);
                    MoveToNewNodeInTree(selNodes[0]);
                    
                    _mTvDom.EndUpdate();
                }
                else {
                    MessageBox.Show("No nodes match the pattern.", "XPath Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Xml.XPath.XPathException ex) {
                MessageBox.Show(ex.Message, "XPath Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        
        private void
        OnBnClear(object sender, System.EventArgs e)
        {
            _mEbXpathPattern.Text = string.Empty;
            ClearSelectedNodes(_mTvDom.Nodes);
        }

        private void
        OnRbChanged_LabelDisplay(object sender, System.EventArgs e)
        {
            LoadTree();     // reload the tree with our new display preference set
        }

        private void
        OnCbChanged_NodeDisplay(object sender, System.EventArgs e)
        {
            LoadTree();     // reload the tree with our new display preference set
        }
        
        private void
        DataItemSelected(object sender, System.EventArgs e)
        {
            Snoop.Utils.DataItemSelected(_mLvData, new ModelessWindowFactory(this, null));
        }
        
        /// <summary>
        /// One of the navigation buttons ("Parent", "First Child", etc) was picked.  Based on the
        /// XmlNode that those functions returned, find the corresponding UI TreeNode and make it
        /// the currently selected one.
        /// </summary>
        /// <param name="newXmlNode">The XmlNode that should now be selected</param>
        
        private void
        MoveToNewNodeInTree(XmlNode newXmlNode)
        {
            Debug.Assert(newXmlNode != null);   // we should have checked out OK in SetButtonModes()
            
            TreeNode newTreeNode = FindTreeNodeFromXmlNodeTag(_mTvDom.Nodes, newXmlNode);
            if (newTreeNode != null) {
                _mTvDom.SelectedNode = newTreeNode;
            }
            else {
                MessageBox.Show("The node exist in the XML DOM, but not in the UI tree.\nPerhaps you have Text or Comment nodes turned off?",
                    "MgdDbg", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// Sometimes we have the Xml DOM node and we need to find where that is in our
        /// UI tree.  This function will brute-force search the UI tree looking for a
        /// TreeNode.Tag that matches the XmlNode we are searching for.  Note: this function
        /// is recursive.
        /// </summary>
        /// <param name="treeNodes">The current collection of nodes to look through</param>
        /// <param name="xmlNode">The XmlNode we are searching for.</param>
        /// <returns>The found TreeNode or null, if not found</returns>
        
        private TreeNode
        FindTreeNodeFromXmlNodeTag(TreeNodeCollection treeNodes, XmlNode xmlNode)
        {
            XmlNode tmpXmlNode = null;
            
                // walk the list of tree nodes looking for a match in the attached
                // Tag object.
            foreach (TreeNode tNode in treeNodes) {
                tmpXmlNode = (XmlNode)tNode.Tag;
                if (tmpXmlNode == xmlNode)
                    return tNode;       // found it
                    
                    // Didn't find it, but this node may have children, so recursively
                    // look for it here.
                if (tNode.Nodes.Count > 0) {
                        // if we find it on the recursive call, pop back out,
                        // otherwise continue searching at this level
                    TreeNode recursiveNode = null;
                    recursiveNode = FindTreeNodeFromXmlNodeTag(tNode.Nodes, xmlNode);
                    if (recursiveNode != null)
                        return recursiveNode;
                }
            }
            
            return null;
        }

        /// <summary>
        /// When nodes are selected by XPath expressions, we highlighted them by changing
        /// the background color.  Go through and reset them to normal.  This function handles
        /// one level of the tree at a time and then goes recursive.
        /// </summary>
        /// <param name="treeNodes">The "root" of this protion of the tree</param>
        
        private void
        ClearSelectedNodes(TreeNodeCollection treeNodes)
        {
            foreach (TreeNode tNode in treeNodes) {
                tNode.BackColor = System.Drawing.Color.Empty;  
                if (tNode.Nodes.Count > 0) {
                    ClearSelectedNodes(tNode.Nodes);
                }
            }
        }
        
        /// <summary>
        /// Change the background color of the matching node
        /// </summary>
        /// <param name="treeNodes">The "root" of this section of the tree</param>
        /// <param name="selNode">The XmlNode we are trying to find</param>
        
        private void
        SetSelectedNode(TreeNodeCollection treeNodes, XmlNode selNode)
        {
            foreach (TreeNode tNode in treeNodes) {
                if (selNode == (XmlNode)tNode.Tag)
                    tNode.BackColor = System.Drawing.Color.LightSkyBlue;
                else
                    tNode.BackColor = System.Drawing.Color.Empty;
                    
                if (tNode.Nodes.Count > 0) {
                    SetSelectedNode(tNode.Nodes, selNode);
                }
            }
        }
        
        /// <summary>
        /// Same as SetSelectedNode(), but with a set of matches.
        /// NOTE: You cannot manually add to an XmlNodeList, so I couldn't
        /// have the above function call this one.
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="selNodes"></param>
        
        private void
        SetSelectedNodes(TreeNodeCollection treeNodes, XmlNodeList selNodes)
        {
            foreach (TreeNode tNode in treeNodes) {
                if (NodeListContains(selNodes, (XmlNode)tNode.Tag))
                    tNode.BackColor = System.Drawing.Color.LightSkyBlue;
                else
                    tNode.BackColor = System.Drawing.Color.Empty;
                    
                if (tNode.Nodes.Count > 0) {
                    SetSelectedNodes(tNode.Nodes, selNodes);
                }
            }
        }
        
        /// <summary>
        /// Is a given node part of the nodeSet?
        /// </summary>
        /// <param name="nodeSet">Set of nodes to search</param>
        /// <param name="findNode">Node we are searching for</param>
        /// <returns>true if found, false if not</returns>
        
        private bool
        NodeListContains(XmlNodeList nodeSet, XmlNode findNode)
        {
            foreach (XmlNode tmpNode in nodeSet) {
                if (tmpNode == findNode)
                    return true;
            }
            
            return false;
        }


	}
}
