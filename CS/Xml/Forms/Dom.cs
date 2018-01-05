#region Header
//
// Copyright 2003-2018 by Autodesk, Inc. 
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

namespace RevitLookup.Xml.Forms
{
	/// <summary>
	/// UI Test for navigating the XML Dom
	/// </summary>
	public class Dom : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TreeView       m_tvDom;
        private System.Windows.Forms.ListView       m_lvData;
        private System.Windows.Forms.RadioButton    m_rbNodeNameOnly;
        private System.Windows.Forms.RadioButton    m_rbNodeAndText;
        private System.Windows.Forms.GroupBox       m_grpLabelDisplay;
        private System.Windows.Forms.GroupBox       m_grpNodeDisplay;
        private System.Windows.Forms.CheckBox       m_cbHideCommentNodes;
        private System.Windows.Forms.CheckBox       m_cbHideTextNodes;
        private System.Windows.Forms.Button         m_bnParent;
        private System.Windows.Forms.Button         m_bnOwnerDoc;
        private System.Windows.Forms.Button         m_bnPrevSibling;
        private System.Windows.Forms.Button         m_bnNextSibling;
        private System.Windows.Forms.Button         m_bnFirstChild;
        private System.Windows.Forms.Button         m_bnLastChild;
        private System.Windows.Forms.Button         m_bnDocElem;
        private System.Windows.Forms.TextBox        m_ebXpathPattern;
        private System.Windows.Forms.Button         m_bnSelectSingleNode;
        private System.Windows.Forms.Button         m_bnXpathClear;
        private System.Windows.Forms.Button         m_bnSelectNodes;
        private System.Windows.Forms.Button         m_bnOk;
        private System.Windows.Forms.ColumnHeader   m_lvColLabel;
        private System.Windows.Forms.ColumnHeader   m_lvColValue;
        private System.Windows.Forms.ImageList      m_imgListTree;
        private System.Windows.Forms.Label          m_txtXpathPattern;
        private System.Windows.Forms.GroupBox       m_grpXpath;
        
        private System.Xml.XmlDocument              m_xmlDoc = null;
        private Snoop.Collectors.CollectorXmlNode   m_snoopCollector = new Snoop.Collectors.CollectorXmlNode();
        
        private System.ComponentModel.IContainer components;

		public
		Dom(System.Xml.XmlDocument xmlDoc)
		{
		    m_xmlDoc = xmlDoc;
		    
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
            this.m_tvDom = new System.Windows.Forms.TreeView();
            this.m_imgListTree = new System.Windows.Forms.ImageList(this.components);
            this.m_lvData = new System.Windows.Forms.ListView();
            this.m_lvColLabel = new System.Windows.Forms.ColumnHeader();
            this.m_lvColValue = new System.Windows.Forms.ColumnHeader();
            this.m_rbNodeNameOnly = new System.Windows.Forms.RadioButton();
            this.m_rbNodeAndText = new System.Windows.Forms.RadioButton();
            this.m_grpLabelDisplay = new System.Windows.Forms.GroupBox();
            this.m_grpNodeDisplay = new System.Windows.Forms.GroupBox();
            this.m_cbHideTextNodes = new System.Windows.Forms.CheckBox();
            this.m_cbHideCommentNodes = new System.Windows.Forms.CheckBox();
            this.m_bnParent = new System.Windows.Forms.Button();
            this.m_bnOwnerDoc = new System.Windows.Forms.Button();
            this.m_bnPrevSibling = new System.Windows.Forms.Button();
            this.m_bnNextSibling = new System.Windows.Forms.Button();
            this.m_bnFirstChild = new System.Windows.Forms.Button();
            this.m_bnLastChild = new System.Windows.Forms.Button();
            this.m_txtXpathPattern = new System.Windows.Forms.Label();
            this.m_ebXpathPattern = new System.Windows.Forms.TextBox();
            this.m_bnSelectSingleNode = new System.Windows.Forms.Button();
            this.m_bnSelectNodes = new System.Windows.Forms.Button();
            this.m_bnOk = new System.Windows.Forms.Button();
            this.m_bnXpathClear = new System.Windows.Forms.Button();
            this.m_grpXpath = new System.Windows.Forms.GroupBox();
            this.m_bnDocElem = new System.Windows.Forms.Button();
            this.m_grpNodeDisplay.SuspendLayout();
            this.m_grpXpath.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tvDom
            // 
            this.m_tvDom.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right);
            this.m_tvDom.HideSelection = false;
            this.m_tvDom.ImageList = this.m_imgListTree;
            this.m_tvDom.Location = new System.Drawing.Point(16, 16);
            this.m_tvDom.Name = "m_tvDom";
            this.m_tvDom.Size = new System.Drawing.Size(336, 416);
            this.m_tvDom.TabIndex = 0;
            this.m_tvDom.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
            // 
            // m_imgListTree
            // 
            this.m_imgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.m_imgListTree.ImageSize = new System.Drawing.Size(16, 16);
            this.m_imgListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_imgListTree.ImageStream")));
            this.m_imgListTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // m_lvData
            // 
            this.m_lvData.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Right);
            this.m_lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       this.m_lvColLabel,
                                                                                       this.m_lvColValue});
            this.m_lvData.FullRowSelect = true;
            this.m_lvData.GridLines = true;
            this.m_lvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.m_lvData.Location = new System.Drawing.Point(368, 16);
            this.m_lvData.MultiSelect = false;
            this.m_lvData.Name = "m_lvData";
            this.m_lvData.Size = new System.Drawing.Size(416, 328);
            this.m_lvData.TabIndex = 1;
            this.m_lvData.View = System.Windows.Forms.View.Details;
            this.m_lvData.Click += new System.EventHandler(this.DataItemSelected);
            // 
            // m_lvColLabel
            // 
            this.m_lvColLabel.Text = "Field";
            this.m_lvColLabel.Width = 200;
            // 
            // m_lvColValue
            // 
            this.m_lvColValue.Text = "Value";
            this.m_lvColValue.Width = 250;
            // 
            // m_rbNodeNameOnly
            // 
            this.m_rbNodeNameOnly.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_rbNodeNameOnly.Checked = true;
            this.m_rbNodeNameOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_rbNodeNameOnly.Location = new System.Drawing.Point(384, 376);
            this.m_rbNodeNameOnly.Name = "m_rbNodeNameOnly";
            this.m_rbNodeNameOnly.Size = new System.Drawing.Size(152, 24);
            this.m_rbNodeNameOnly.TabIndex = 2;
            this.m_rbNodeNameOnly.TabStop = true;
            this.m_rbNodeNameOnly.Text = "Node Name Only";
            this.m_rbNodeNameOnly.CheckedChanged += new System.EventHandler(this.OnRbChanged_LabelDisplay);
            // 
            // m_rbNodeAndText
            // 
            this.m_rbNodeAndText.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_rbNodeAndText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_rbNodeAndText.Location = new System.Drawing.Point(384, 400);
            this.m_rbNodeAndText.Name = "m_rbNodeAndText";
            this.m_rbNodeAndText.Size = new System.Drawing.Size(168, 24);
            this.m_rbNodeAndText.TabIndex = 4;
            this.m_rbNodeAndText.Text = "Node Name and Value";
            this.m_rbNodeAndText.CheckedChanged += new System.EventHandler(this.OnRbChanged_LabelDisplay);
            // 
            // m_grpLabelDisplay
            // 
            this.m_grpLabelDisplay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_grpLabelDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_grpLabelDisplay.Location = new System.Drawing.Point(368, 360);
            this.m_grpLabelDisplay.Name = "m_grpLabelDisplay";
            this.m_grpLabelDisplay.Size = new System.Drawing.Size(200, 72);
            this.m_grpLabelDisplay.TabIndex = 5;
            this.m_grpLabelDisplay.TabStop = false;
            this.m_grpLabelDisplay.Text = "Label Display";
            // 
            // m_grpNodeDisplay
            // 
            this.m_grpNodeDisplay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_grpNodeDisplay.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                           this.m_cbHideTextNodes,
                                                                                           this.m_cbHideCommentNodes});
            this.m_grpNodeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_grpNodeDisplay.Location = new System.Drawing.Point(584, 360);
            this.m_grpNodeDisplay.Name = "m_grpNodeDisplay";
            this.m_grpNodeDisplay.Size = new System.Drawing.Size(200, 72);
            this.m_grpNodeDisplay.TabIndex = 6;
            this.m_grpNodeDisplay.TabStop = false;
            this.m_grpNodeDisplay.Text = "Node Display";
            // 
            // m_cbHideTextNodes
            // 
            this.m_cbHideTextNodes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_cbHideTextNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_cbHideTextNodes.Location = new System.Drawing.Point(16, 40);
            this.m_cbHideTextNodes.Name = "m_cbHideTextNodes";
            this.m_cbHideTextNodes.Size = new System.Drawing.Size(176, 24);
            this.m_cbHideTextNodes.TabIndex = 1;
            this.m_cbHideTextNodes.Text = "Hide Text Nodes";
            this.m_cbHideTextNodes.CheckedChanged += new System.EventHandler(this.OnCbChanged_NodeDisplay);
            // 
            // m_cbHideCommentNodes
            // 
            this.m_cbHideCommentNodes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_cbHideCommentNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_cbHideCommentNodes.Location = new System.Drawing.Point(16, 16);
            this.m_cbHideCommentNodes.Name = "m_cbHideCommentNodes";
            this.m_cbHideCommentNodes.Size = new System.Drawing.Size(176, 24);
            this.m_cbHideCommentNodes.TabIndex = 0;
            this.m_cbHideCommentNodes.Text = "Hide Comment Nodes";
            this.m_cbHideCommentNodes.CheckedChanged += new System.EventHandler(this.OnCbChanged_NodeDisplay);
            // 
            // m_bnParent
            // 
            this.m_bnParent.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnParent.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnParent.Location = new System.Drawing.Point(16, 448);
            this.m_bnParent.Name = "m_bnParent";
            this.m_bnParent.TabIndex = 7;
            this.m_bnParent.Text = "Parent";
            this.m_bnParent.Click += new System.EventHandler(this.OnBnParent);
            // 
            // m_bnOwnerDoc
            // 
            this.m_bnOwnerDoc.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnOwnerDoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnOwnerDoc.Location = new System.Drawing.Point(104, 448);
            this.m_bnOwnerDoc.Name = "m_bnOwnerDoc";
            this.m_bnOwnerDoc.TabIndex = 8;
            this.m_bnOwnerDoc.Text = "Owner Doc";
            this.m_bnOwnerDoc.Click += new System.EventHandler(this.OnBnOwnerDoc);
            // 
            // m_bnPrevSibling
            // 
            this.m_bnPrevSibling.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnPrevSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnPrevSibling.Location = new System.Drawing.Point(192, 448);
            this.m_bnPrevSibling.Name = "m_bnPrevSibling";
            this.m_bnPrevSibling.TabIndex = 9;
            this.m_bnPrevSibling.Text = "Prev Sibling";
            this.m_bnPrevSibling.Click += new System.EventHandler(this.OnBnPrevSibling);
            // 
            // m_bnNextSibling
            // 
            this.m_bnNextSibling.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnNextSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnNextSibling.Location = new System.Drawing.Point(280, 448);
            this.m_bnNextSibling.Name = "m_bnNextSibling";
            this.m_bnNextSibling.TabIndex = 10;
            this.m_bnNextSibling.Text = "Next Sibling";
            this.m_bnNextSibling.Click += new System.EventHandler(this.OnBnNextSibling);
            // 
            // m_bnFirstChild
            // 
            this.m_bnFirstChild.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnFirstChild.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnFirstChild.Location = new System.Drawing.Point(368, 448);
            this.m_bnFirstChild.Name = "m_bnFirstChild";
            this.m_bnFirstChild.TabIndex = 11;
            this.m_bnFirstChild.Text = "First Child";
            this.m_bnFirstChild.Click += new System.EventHandler(this.OnBnFirstChild);
            // 
            // m_bnLastChild
            // 
            this.m_bnLastChild.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnLastChild.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnLastChild.Location = new System.Drawing.Point(456, 448);
            this.m_bnLastChild.Name = "m_bnLastChild";
            this.m_bnLastChild.TabIndex = 12;
            this.m_bnLastChild.Text = "Last Child";
            this.m_bnLastChild.Click += new System.EventHandler(this.OnBnLastChild);
            // 
            // m_txtXpathPattern
            // 
            this.m_txtXpathPattern.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_txtXpathPattern.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_txtXpathPattern.Location = new System.Drawing.Point(32, 512);
            this.m_txtXpathPattern.Name = "m_txtXpathPattern";
            this.m_txtXpathPattern.Size = new System.Drawing.Size(64, 23);
            this.m_txtXpathPattern.TabIndex = 14;
            this.m_txtXpathPattern.Text = "Expression:";
            // 
            // m_ebXpathPattern
            // 
            this.m_ebXpathPattern.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_ebXpathPattern.Location = new System.Drawing.Point(88, 24);
            this.m_ebXpathPattern.Name = "m_ebXpathPattern";
            this.m_ebXpathPattern.Size = new System.Drawing.Size(424, 20);
            this.m_ebXpathPattern.TabIndex = 15;
            this.m_ebXpathPattern.Text = "";
            // 
            // m_bnSelectSingleNode
            // 
            this.m_bnSelectSingleNode.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnSelectSingleNode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnSelectSingleNode.Location = new System.Drawing.Point(128, 56);
            this.m_bnSelectSingleNode.Name = "m_bnSelectSingleNode";
            this.m_bnSelectSingleNode.Size = new System.Drawing.Size(120, 23);
            this.m_bnSelectSingleNode.TabIndex = 16;
            this.m_bnSelectSingleNode.Text = "Select Single Node";
            this.m_bnSelectSingleNode.Click += new System.EventHandler(this.OnBnSelectSingleNode);
            // 
            // m_bnSelectNodes
            // 
            this.m_bnSelectNodes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnSelectNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnSelectNodes.Location = new System.Drawing.Point(256, 56);
            this.m_bnSelectNodes.Name = "m_bnSelectNodes";
            this.m_bnSelectNodes.Size = new System.Drawing.Size(120, 23);
            this.m_bnSelectNodes.TabIndex = 17;
            this.m_bnSelectNodes.Text = "Select Nodes";
            this.m_bnSelectNodes.Click += new System.EventHandler(this.OnBnSelectNodes);
            // 
            // m_bnOk
            // 
            this.m_bnOk.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.m_bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_bnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnOk.Location = new System.Drawing.Point(704, 544);
            this.m_bnOk.Name = "m_bnOk";
            this.m_bnOk.TabIndex = 18;
            this.m_bnOk.Text = "OK";
            // 
            // m_bnXpathClear
            // 
            this.m_bnXpathClear.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnXpathClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnXpathClear.Location = new System.Drawing.Point(384, 56);
            this.m_bnXpathClear.Name = "m_bnXpathClear";
            this.m_bnXpathClear.Size = new System.Drawing.Size(120, 23);
            this.m_bnXpathClear.TabIndex = 19;
            this.m_bnXpathClear.Text = "Clear";
            this.m_bnXpathClear.Click += new System.EventHandler(this.OnBnClear);
            // 
            // m_grpXpath
            // 
            this.m_grpXpath.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_grpXpath.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                     this.m_ebXpathPattern,
                                                                                     this.m_bnXpathClear,
                                                                                     this.m_bnSelectNodes,
                                                                                     this.m_bnSelectSingleNode});
            this.m_grpXpath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_grpXpath.Location = new System.Drawing.Point(16, 488);
            this.m_grpXpath.Name = "m_grpXpath";
            this.m_grpXpath.Size = new System.Drawing.Size(520, 88);
            this.m_grpXpath.TabIndex = 20;
            this.m_grpXpath.TabStop = false;
            this.m_grpXpath.Text = "XPath Expressions";
            // 
            // m_bnDocElem
            // 
            this.m_bnDocElem.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.m_bnDocElem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnDocElem.Location = new System.Drawing.Point(544, 448);
            this.m_bnDocElem.Name = "m_bnDocElem";
            this.m_bnDocElem.Size = new System.Drawing.Size(80, 23);
            this.m_bnDocElem.TabIndex = 21;
            this.m_bnDocElem.Text = "Doc Element";
            this.m_bnDocElem.Click += new System.EventHandler(this.OnBnDocElement);
            // 
            // Dom
            // 
            this.AcceptButton = this.m_bnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_bnOk;
            this.ClientSize = new System.Drawing.Size(800, 591);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.m_bnDocElem,
                                                                          this.m_bnOk,
                                                                          this.m_txtXpathPattern,
                                                                          this.m_bnLastChild,
                                                                          this.m_bnFirstChild,
                                                                          this.m_bnNextSibling,
                                                                          this.m_bnPrevSibling,
                                                                          this.m_bnOwnerDoc,
                                                                          this.m_bnParent,
                                                                          this.m_grpNodeDisplay,
                                                                          this.m_rbNodeAndText,
                                                                          this.m_rbNodeNameOnly,
                                                                          this.m_lvData,
                                                                          this.m_tvDom,
                                                                          this.m_grpLabelDisplay,
                                                                          this.m_grpXpath});
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "Dom";
            this.Text = "XML DOM Tree";
            this.m_grpNodeDisplay.ResumeLayout(false);
            this.m_grpXpath.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// This will populate the UI TreeView with all the nodes from the DOM tree
        /// </summary>
        
        private void
        LoadTree()
        {
            m_tvDom.BeginUpdate();  // suppress redraw events
            m_tvDom.Nodes.Clear();
            
            MakeTree(m_xmlDoc, null);

            m_tvDom.ExpandAll();
            if (m_tvDom.Nodes.Count > 0)
                m_tvDom.SelectedNode = m_tvDom.Nodes[0];    // make first one selected

            m_tvDom.EndUpdate();		// flushes redraw events
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
            if (((nType == XmlNodeType.Comment) && (m_cbHideCommentNodes.Checked)) ||
                ((nType == XmlNodeType.Text) && (m_cbHideTextNodes.Checked)))
                return;

                // get image index and label to use in the TreeNode
            int imageIndex = GetImageIndex(nType);
            string labelStr = FormatLabel(xmlNode);
            
            TreeNode treeNode = new TreeNode(labelStr);
            treeNode.Tag = xmlNode;
            treeNode.ImageIndex = imageIndex;
            treeNode.SelectedImageIndex = imageIndex;
            
            if (parentNode == null)
                m_tvDom.Nodes.Add(treeNode);    // This is the root node
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
            
            if (m_rbNodeNameOnly.Checked)
                labelStr = node.Name;
            else if (m_rbNodeAndText.Checked) {
                if ((node.NodeType == XmlNodeType.Element) || (node.NodeType == XmlNodeType.Attribute))
                    labelStr = string.Format("{0} ({1})", node.Name, GetTextLabelValue(node));
                else
                    labelStr = string.Format("{0} ({1})", node.Name, node.Value);
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
                                   
            m_snoopCollector.Collect(node);
            Snoop.Utils.Display(m_lvData, m_snoopCollector);
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
            m_bnParent.Enabled = (tmpNode != null);
            
            tmpNode = node.OwnerDocument;
            m_bnOwnerDoc.Enabled = (tmpNode != null);

            tmpNode = node.PreviousSibling;
            m_bnPrevSibling.Enabled = (tmpNode != null);

            tmpNode = node.NextSibling;
            m_bnNextSibling.Enabled = (tmpNode != null);
            
            tmpNode = node.FirstChild;
            m_bnFirstChild.Enabled = (tmpNode != null);

            tmpNode = node.LastChild;
            m_bnLastChild.Enabled = (tmpNode != null);
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
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.ParentNode);
        }

        private void
        OnBnOwnerDoc(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.OwnerDocument);
        }

        private void
        OnBnPrevSibling(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.PreviousSibling);
        }

        private void
        OnBnNextSibling(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.NextSibling);
        }

        private void
        OnBnFirstChild(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.FirstChild);
        }

        private void
        OnBnLastChild(object sender, System.EventArgs e)
        {
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;
            MoveToNewNodeInTree(curNode.LastChild);
        }
        
        private void
        OnBnDocElement(object sender, System.EventArgs e)
        {
            XmlElement elem = m_xmlDoc.DocumentElement;
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
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;

            try {
                XmlNode newNode = curNode.SelectSingleNode(m_ebXpathPattern.Text);
                
                if (newNode != null) {
                    m_tvDom.BeginUpdate();
                   
                    SetSelectedNode(m_tvDom.Nodes, newNode);
                    MoveToNewNodeInTree(newNode);
                    
                    m_tvDom.EndUpdate();
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
            XmlNode curNode = (XmlNode)m_tvDom.SelectedNode.Tag;

            try {
                XmlNodeList selNodes = curNode.SelectNodes(m_ebXpathPattern.Text);
                if ((selNodes != null) && (selNodes.Count > 0)) {
                    m_tvDom.BeginUpdate();
                    
                    SetSelectedNodes(m_tvDom.Nodes, selNodes);
                    MoveToNewNodeInTree(selNodes[0]);
                    
                    m_tvDom.EndUpdate();
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
            m_ebXpathPattern.Text = string.Empty;
            ClearSelectedNodes(m_tvDom.Nodes);
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
            Snoop.Utils.DataItemSelected(m_lvData);
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
            
            TreeNode newTreeNode = FindTreeNodeFromXmlNodeTag(m_tvDom.Nodes, newXmlNode);
            if (newTreeNode != null) {
                m_tvDom.SelectedNode = newTreeNode;
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
