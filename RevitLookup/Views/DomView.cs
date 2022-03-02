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
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using RevitLookup.Core;
using RevitLookup.Core.Collectors;

namespace RevitLookup.Views;

/// <summary>
///     UI Test for navigating the XML Dom
/// </summary>
public class DomView : Form
{
    private readonly CollectorXmlNode _mSnoopCollector = new();
    private readonly XmlDocument _mXmlDoc;
    private Button _bnDocElem;
    private Button _bnFirstChild;
    private Button _bnLastChild;
    private Button _bnNextSibling;
    private Button _bnOk;
    private Button _bnOwnerDoc;
    private Button _bnParent;
    private Button _bnPrevSibling;
    private Button _bnSelectNodes;
    private Button _bnSelectSingleNode;
    private Button _bnXpathClear;
    private CheckBox _cbHideCommentNodes;
    private CheckBox _cbHideTextNodes;
    private TextBox _ebXpathPattern;
    private GroupBox _grpLabelDisplay;
    private GroupBox _grpNodeDisplay;
    private GroupBox _grpXpath;
    private ImageList _imgListTree;
    private ColumnHeader _lvColLabel;
    private ColumnHeader _lvColValue;
    private ListView _lvData;
    private RadioButton _rbNodeAndText;
    private RadioButton _rbNodeNameOnly;
    private TreeView _tvDom;
    private Label _txtXpathPattern;
    private IContainer components;

    public DomView(XmlDocument xmlDoc)
    {
        _mXmlDoc = xmlDoc;
        InitializeComponent();
        LoadTree();
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
        this._tvDom = new System.Windows.Forms.TreeView();
        this._imgListTree = new System.Windows.Forms.ImageList(this.components);
        this._lvData = new System.Windows.Forms.ListView();
        this._lvColLabel = new System.Windows.Forms.ColumnHeader();
        this._lvColValue = new System.Windows.Forms.ColumnHeader();
        this._rbNodeNameOnly = new System.Windows.Forms.RadioButton();
        this._rbNodeAndText = new System.Windows.Forms.RadioButton();
        this._grpLabelDisplay = new System.Windows.Forms.GroupBox();
        this._grpNodeDisplay = new System.Windows.Forms.GroupBox();
        this._cbHideTextNodes = new System.Windows.Forms.CheckBox();
        this._cbHideCommentNodes = new System.Windows.Forms.CheckBox();
        this._bnParent = new System.Windows.Forms.Button();
        this._bnOwnerDoc = new System.Windows.Forms.Button();
        this._bnPrevSibling = new System.Windows.Forms.Button();
        this._bnNextSibling = new System.Windows.Forms.Button();
        this._bnFirstChild = new System.Windows.Forms.Button();
        this._bnLastChild = new System.Windows.Forms.Button();
        this._txtXpathPattern = new System.Windows.Forms.Label();
        this._ebXpathPattern = new System.Windows.Forms.TextBox();
        this._bnSelectSingleNode = new System.Windows.Forms.Button();
        this._bnSelectNodes = new System.Windows.Forms.Button();
        this._bnOk = new System.Windows.Forms.Button();
        this._bnXpathClear = new System.Windows.Forms.Button();
        this._grpXpath = new System.Windows.Forms.GroupBox();
        this._bnDocElem = new System.Windows.Forms.Button();
        this._grpNodeDisplay.SuspendLayout();
        this._grpXpath.SuspendLayout();
        this.SuspendLayout();
        // 
        // _tvDom
        // 
        this._tvDom.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this._tvDom.HideSelection = false;
        this._tvDom.ImageIndex = 0;
        this._tvDom.ImageList = this._imgListTree;
        this._tvDom.Location = new System.Drawing.Point(16, 16);
        this._tvDom.Name = "_tvDom";
        this._tvDom.SelectedImageIndex = 0;
        this._tvDom.Size = new System.Drawing.Size(336, 416);
        this._tvDom.TabIndex = 0;
        this._tvDom.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeNodeSelected);
        // 
        // _imgListTree
        // 
        this._imgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
        this._imgListTree.ImageSize = new System.Drawing.Size(16, 16);
        this._imgListTree.TransparentColor = System.Drawing.Color.Transparent;
        // 
        // _lvData
        // 
        this._lvData.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right)));
        this._lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this._lvColLabel, this._lvColValue});
        this._lvData.FullRowSelect = true;
        this._lvData.GridLines = true;
        this._lvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this._lvData.Location = new System.Drawing.Point(368, 16);
        this._lvData.MultiSelect = false;
        this._lvData.Name = "_lvData";
        this._lvData.Size = new System.Drawing.Size(416, 328);
        this._lvData.TabIndex = 1;
        this._lvData.UseCompatibleStateImageBehavior = false;
        this._lvData.View = System.Windows.Forms.View.Details;
        this._lvData.Click += new System.EventHandler(this.DataItemSelected);
        // 
        // _lvColLabel
        // 
        this._lvColLabel.Text = "Field";
        this._lvColLabel.Width = 200;
        // 
        // _lvColValue
        // 
        this._lvColValue.Text = "Value";
        this._lvColValue.Width = 750;
        // 
        // _rbNodeNameOnly
        // 
        this._rbNodeNameOnly.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._rbNodeNameOnly.Checked = true;
        this._rbNodeNameOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._rbNodeNameOnly.Location = new System.Drawing.Point(384, 376);
        this._rbNodeNameOnly.Name = "_rbNodeNameOnly";
        this._rbNodeNameOnly.Size = new System.Drawing.Size(152, 24);
        this._rbNodeNameOnly.TabIndex = 2;
        this._rbNodeNameOnly.TabStop = true;
        this._rbNodeNameOnly.Text = "Node Name Only";
        this._rbNodeNameOnly.CheckedChanged += new System.EventHandler(this.OnRbChanged_LabelDisplay);
        // 
        // _rbNodeAndText
        // 
        this._rbNodeAndText.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._rbNodeAndText.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._rbNodeAndText.Location = new System.Drawing.Point(384, 400);
        this._rbNodeAndText.Name = "_rbNodeAndText";
        this._rbNodeAndText.Size = new System.Drawing.Size(168, 24);
        this._rbNodeAndText.TabIndex = 4;
        this._rbNodeAndText.Text = "Node Name and Value";
        this._rbNodeAndText.CheckedChanged += new System.EventHandler(this.OnRbChanged_LabelDisplay);
        // 
        // _grpLabelDisplay
        // 
        this._grpLabelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._grpLabelDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._grpLabelDisplay.Location = new System.Drawing.Point(368, 360);
        this._grpLabelDisplay.Name = "_grpLabelDisplay";
        this._grpLabelDisplay.Size = new System.Drawing.Size(200, 72);
        this._grpLabelDisplay.TabIndex = 5;
        this._grpLabelDisplay.TabStop = false;
        this._grpLabelDisplay.Text = "Label Display";
        // 
        // _grpNodeDisplay
        // 
        this._grpNodeDisplay.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._grpNodeDisplay.Controls.Add(this._cbHideTextNodes);
        this._grpNodeDisplay.Controls.Add(this._cbHideCommentNodes);
        this._grpNodeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._grpNodeDisplay.Location = new System.Drawing.Point(584, 360);
        this._grpNodeDisplay.Name = "_grpNodeDisplay";
        this._grpNodeDisplay.Size = new System.Drawing.Size(200, 72);
        this._grpNodeDisplay.TabIndex = 6;
        this._grpNodeDisplay.TabStop = false;
        this._grpNodeDisplay.Text = "Node Display";
        // 
        // _cbHideTextNodes
        // 
        this._cbHideTextNodes.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._cbHideTextNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._cbHideTextNodes.Location = new System.Drawing.Point(16, 40);
        this._cbHideTextNodes.Name = "_cbHideTextNodes";
        this._cbHideTextNodes.Size = new System.Drawing.Size(176, 24);
        this._cbHideTextNodes.TabIndex = 1;
        this._cbHideTextNodes.Text = "Hide Text Nodes";
        this._cbHideTextNodes.CheckedChanged += new System.EventHandler(this.OnCbChanged_NodeDisplay);
        // 
        // _cbHideCommentNodes
        // 
        this._cbHideCommentNodes.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._cbHideCommentNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._cbHideCommentNodes.Location = new System.Drawing.Point(16, 16);
        this._cbHideCommentNodes.Name = "_cbHideCommentNodes";
        this._cbHideCommentNodes.Size = new System.Drawing.Size(176, 24);
        this._cbHideCommentNodes.TabIndex = 0;
        this._cbHideCommentNodes.Text = "Hide Comment Nodes";
        this._cbHideCommentNodes.CheckedChanged += new System.EventHandler(this.OnCbChanged_NodeDisplay);
        // 
        // _bnParent
        // 
        this._bnParent.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnParent.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnParent.Location = new System.Drawing.Point(16, 448);
        this._bnParent.Name = "_bnParent";
        this._bnParent.Size = new System.Drawing.Size(75, 23);
        this._bnParent.TabIndex = 7;
        this._bnParent.Text = "Parent";
        this._bnParent.Click += new System.EventHandler(this.OnBnParent);
        // 
        // _bnOwnerDoc
        // 
        this._bnOwnerDoc.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnOwnerDoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnOwnerDoc.Location = new System.Drawing.Point(104, 448);
        this._bnOwnerDoc.Name = "_bnOwnerDoc";
        this._bnOwnerDoc.Size = new System.Drawing.Size(75, 23);
        this._bnOwnerDoc.TabIndex = 8;
        this._bnOwnerDoc.Text = "Owner Doc";
        this._bnOwnerDoc.Click += new System.EventHandler(this.OnBnOwnerDoc);
        // 
        // _bnPrevSibling
        // 
        this._bnPrevSibling.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnPrevSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnPrevSibling.Location = new System.Drawing.Point(192, 448);
        this._bnPrevSibling.Name = "_bnPrevSibling";
        this._bnPrevSibling.Size = new System.Drawing.Size(75, 23);
        this._bnPrevSibling.TabIndex = 9;
        this._bnPrevSibling.Text = "Prev Sibling";
        this._bnPrevSibling.Click += new System.EventHandler(this.OnBnPrevSibling);
        // 
        // _bnNextSibling
        // 
        this._bnNextSibling.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnNextSibling.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnNextSibling.Location = new System.Drawing.Point(280, 448);
        this._bnNextSibling.Name = "_bnNextSibling";
        this._bnNextSibling.Size = new System.Drawing.Size(75, 23);
        this._bnNextSibling.TabIndex = 10;
        this._bnNextSibling.Text = "Next Sibling";
        this._bnNextSibling.Click += new System.EventHandler(this.OnBnNextSibling);
        // 
        // _bnFirstChild
        // 
        this._bnFirstChild.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnFirstChild.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnFirstChild.Location = new System.Drawing.Point(368, 448);
        this._bnFirstChild.Name = "_bnFirstChild";
        this._bnFirstChild.Size = new System.Drawing.Size(75, 23);
        this._bnFirstChild.TabIndex = 11;
        this._bnFirstChild.Text = "First Child";
        this._bnFirstChild.Click += new System.EventHandler(this.OnBnFirstChild);
        // 
        // _bnLastChild
        // 
        this._bnLastChild.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnLastChild.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnLastChild.Location = new System.Drawing.Point(456, 448);
        this._bnLastChild.Name = "_bnLastChild";
        this._bnLastChild.Size = new System.Drawing.Size(75, 23);
        this._bnLastChild.TabIndex = 12;
        this._bnLastChild.Text = "Last Child";
        this._bnLastChild.Click += new System.EventHandler(this.OnBnLastChild);
        // 
        // _txtXpathPattern
        // 
        this._txtXpathPattern.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._txtXpathPattern.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._txtXpathPattern.Location = new System.Drawing.Point(32, 512);
        this._txtXpathPattern.Name = "_txtXpathPattern";
        this._txtXpathPattern.Size = new System.Drawing.Size(64, 23);
        this._txtXpathPattern.TabIndex = 14;
        this._txtXpathPattern.Text = "Expression:";
        // 
        // _ebXpathPattern
        // 
        this._ebXpathPattern.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._ebXpathPattern.Location = new System.Drawing.Point(88, 24);
        this._ebXpathPattern.Name = "_ebXpathPattern";
        this._ebXpathPattern.Size = new System.Drawing.Size(424, 20);
        this._ebXpathPattern.TabIndex = 15;
        // 
        // _bnSelectSingleNode
        // 
        this._bnSelectSingleNode.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnSelectSingleNode.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnSelectSingleNode.Location = new System.Drawing.Point(128, 56);
        this._bnSelectSingleNode.Name = "_bnSelectSingleNode";
        this._bnSelectSingleNode.Size = new System.Drawing.Size(120, 23);
        this._bnSelectSingleNode.TabIndex = 16;
        this._bnSelectSingleNode.Text = "Select Single Node";
        this._bnSelectSingleNode.Click += new System.EventHandler(this.OnBnSelectSingleNode);
        // 
        // _bnSelectNodes
        // 
        this._bnSelectNodes.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnSelectNodes.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnSelectNodes.Location = new System.Drawing.Point(256, 56);
        this._bnSelectNodes.Name = "_bnSelectNodes";
        this._bnSelectNodes.Size = new System.Drawing.Size(120, 23);
        this._bnSelectNodes.TabIndex = 17;
        this._bnSelectNodes.Text = "Select Nodes";
        this._bnSelectNodes.Click += new System.EventHandler(this.OnBnSelectNodes);
        // 
        // _bnOk
        // 
        this._bnOk.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this._bnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnOk.Location = new System.Drawing.Point(704, 544);
        this._bnOk.Name = "_bnOk";
        this._bnOk.Size = new System.Drawing.Size(75, 23);
        this._bnOk.TabIndex = 18;
        this._bnOk.Text = "OK";
        // 
        // _bnXpathClear
        // 
        this._bnXpathClear.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnXpathClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnXpathClear.Location = new System.Drawing.Point(384, 56);
        this._bnXpathClear.Name = "_bnXpathClear";
        this._bnXpathClear.Size = new System.Drawing.Size(120, 23);
        this._bnXpathClear.TabIndex = 19;
        this._bnXpathClear.Text = "Clear";
        this._bnXpathClear.Click += new System.EventHandler(this.OnBnClear);
        // 
        // _grpXpath
        // 
        this._grpXpath.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._grpXpath.Controls.Add(this._ebXpathPattern);
        this._grpXpath.Controls.Add(this._bnXpathClear);
        this._grpXpath.Controls.Add(this._bnSelectNodes);
        this._grpXpath.Controls.Add(this._bnSelectSingleNode);
        this._grpXpath.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._grpXpath.Location = new System.Drawing.Point(16, 488);
        this._grpXpath.Name = "_grpXpath";
        this._grpXpath.Size = new System.Drawing.Size(520, 88);
        this._grpXpath.TabIndex = 20;
        this._grpXpath.TabStop = false;
        this._grpXpath.Text = "XPath Expressions";
        // 
        // _bnDocElem
        // 
        this._bnDocElem.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this._bnDocElem.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this._bnDocElem.Location = new System.Drawing.Point(544, 448);
        this._bnDocElem.Name = "_bnDocElem";
        this._bnDocElem.Size = new System.Drawing.Size(80, 23);
        this._bnDocElem.TabIndex = 21;
        this._bnDocElem.Text = "Doc Element";
        this._bnDocElem.Click += new System.EventHandler(this.OnBnDocElement);
        // 
        // DomView
        // 
        this.AcceptButton = this._bnOk;
        this.CancelButton = this._bnOk;
        this.ClientSize = new System.Drawing.Size(800, 591);
        this.Controls.Add(this._bnDocElem);
        this.Controls.Add(this._bnOk);
        this.Controls.Add(this._txtXpathPattern);
        this.Controls.Add(this._bnLastChild);
        this.Controls.Add(this._bnFirstChild);
        this.Controls.Add(this._bnNextSibling);
        this.Controls.Add(this._bnPrevSibling);
        this.Controls.Add(this._bnOwnerDoc);
        this.Controls.Add(this._bnParent);
        this.Controls.Add(this._grpNodeDisplay);
        this.Controls.Add(this._rbNodeAndText);
        this.Controls.Add(this._rbNodeNameOnly);
        this.Controls.Add(this._lvData);
        this.Controls.Add(this._tvDom);
        this.Controls.Add(this._grpLabelDisplay);
        this.Controls.Add(this._grpXpath);
        this.MinimumSize = new System.Drawing.Size(650, 200);
        this.Name = "DomView";
        this.Text = "XML DOM Tree";
        this._grpNodeDisplay.ResumeLayout(false);
        this._grpXpath.ResumeLayout(false);
        this._grpXpath.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    /// <summary>
    ///     This will populate the UI TreeView with all the nodes from the DOM tree
    /// </summary>
    private void LoadTree()
    {
        _tvDom.BeginUpdate(); // suppress redraw events
        _tvDom.Nodes.Clear();

        MakeTree(_mXmlDoc, null);

        _tvDom.ExpandAll();
        if (_tvDom.Nodes.Count > 0)
            _tvDom.SelectedNode = _tvDom.Nodes[0]; // make first one selected

        _tvDom.EndUpdate(); // flushes redraw events
    }

    /// <summary>
    ///     Recursive function to walk the tree and populate all the nodes
    /// </summary>
    /// <param name="xmlNode">The "root" of this portion of the tree</param>
    /// <param name="parentNode">The parent node to attach to or null for the root</param>
    private void MakeTree(XmlNode xmlNode, TreeNode parentNode)
    {
        var nType = xmlNode.NodeType;

        // bail early if user doesn't want this type of node displayed
        if (nType == XmlNodeType.Comment && _cbHideCommentNodes.Checked || nType == XmlNodeType.Text && _cbHideTextNodes.Checked)
            return;

        // get image index and label to use in the TreeNode
        var imageIndex = GetImageIndex(nType);
        var labelStr = FormatLabel(xmlNode);

        var treeNode = new TreeNode(labelStr)
        {
            Tag = xmlNode,
            ImageIndex = imageIndex,
            SelectedImageIndex = imageIndex
        };

        if (parentNode is null)
            _tvDom.Nodes.Add(treeNode); // This is the root node
        else
            parentNode.Nodes.Add(treeNode);

        // get attributes of this node
        var attributes = xmlNode.Attributes;
        if (attributes is not null)
            foreach (XmlAttribute tmpAtt in attributes)
                MakeTree(tmpAtt, treeNode);
        // now recursively go to the children of this node
        if (xmlNode.HasChildNodes)
            foreach (XmlNode childNode in xmlNode.ChildNodes)
                MakeTree(childNode, treeNode);
    }

    /// <summary>
    ///     Match a particular Image with the XML Node type.  A different Icon will
    ///     be used in the UI tree for each type of node.
    /// </summary>
    /// <param name="nType">The node type</param>
    /// <returns>index into the ImageList assigned to the TreeView</returns>
    private int GetImageIndex(XmlNodeType nType)
    {
        int imageIndex;
        switch (nType)
        {
            // associate the correct image with this type of node
            case XmlNodeType.Document:
                imageIndex = 0;
                break;
            case XmlNodeType.Attribute:
                imageIndex = 1;
                break;
            case XmlNodeType.CDATA:
                imageIndex = 2;
                break;
            case XmlNodeType.Comment:
                imageIndex = 3;
                break;
            case XmlNodeType.DocumentType:
                imageIndex = 4;
                break;
            case XmlNodeType.Element:
                imageIndex = 5;
                break;
            case XmlNodeType.Entity:
                imageIndex = 6;
                break;
            case XmlNodeType.DocumentFragment:
                imageIndex = 7;
                break;
            case XmlNodeType.ProcessingInstruction:
                imageIndex = 8;
                break;
            case XmlNodeType.EntityReference:
                imageIndex = 9;
                break;
            case XmlNodeType.Text:
                imageIndex = 10;
                break;
            case XmlNodeType.XmlDeclaration:
                imageIndex = 11;
                break;
            // TBD: Not sure what when the rest of these come up yet?
            // I will reserve a spot in case they become significant
            case XmlNodeType.EndElement:
            case XmlNodeType.EndEntity:
            case XmlNodeType.None:
            case XmlNodeType.Notation:
            case XmlNodeType.SignificantWhitespace:
            case XmlNodeType.Whitespace:
                imageIndex = 12;
                break;
            default:
                Debug.Assert(false);
                imageIndex = 12;
                break;
        }

        return imageIndex;
    }

    /// <summary>
    ///     Allow the user prefernces to affect how we format the label for the tree node.
    /// </summary>
    /// <param name="node">The node to format</param>
    /// <returns>formatted string according to user preferences</returns>
    private string FormatLabel(XmlNode node)
    {
        string labelStr;

        if (_rbNodeNameOnly.Checked)
        {
            labelStr = node.Name;
        }
        else if (_rbNodeAndText.Checked)
        {
            if (node.NodeType == XmlNodeType.Element || node.NodeType == XmlNodeType.Attribute)
                labelStr = $"{node.Name} ({GetTextLabelValue(node)})";
            else
                labelStr = $"{node.Name} ({node.Value})";
        }
        else
        {
            Debug.Assert(false, "Unknown radio button!"); // Someone must have added a button we don't know about!
            labelStr = string.Empty;
        }

        return labelStr;
    }

    /// <summary>
    ///     Retrieve the text value for a given node
    /// </summary>
    /// <param name="node">Node to look at</param>
    /// <returns>Text value of the node (the XmlText Child Node of the one passed in)</returns>
    private string GetTextLabelValue(XmlNode node)
    {
        var txtNode = node.FirstChild;
        return txtNode is {NodeType: XmlNodeType.Text} ? txtNode.Value : string.Empty;
    }

    /// <summary>
    ///     Display the data values associated with a selected node in the tree.
    /// </summary>
    /// <param name="node">Currently selected node</param>
    private void Display(XmlNode node)
    {
        SetButtonModes(node);
        _mSnoopCollector.Collect(node);
        Core.Utils.Display(_lvData, _mSnoopCollector);
    }

    /// <summary>
    ///     Do a preview check to see which navigation buttons are going to work.  Disable
    ///     the ones that will not.
    /// </summary>
    /// <param name="node">Currently selected node</param>
    private void SetButtonModes(XmlNode node)
    {
        var tmpNode = node.ParentNode;
        _bnParent.Enabled = tmpNode is not null;

        tmpNode = node.OwnerDocument;
        _bnOwnerDoc.Enabled = tmpNode is not null;

        tmpNode = node.PreviousSibling;
        _bnPrevSibling.Enabled = tmpNode is not null;

        tmpNode = node.NextSibling;
        _bnNextSibling.Enabled = tmpNode is not null;

        tmpNode = node.FirstChild;
        _bnFirstChild.Enabled = tmpNode is not null;

        tmpNode = node.LastChild;
        _bnLastChild.Enabled = tmpNode is not null;
    }

    /// <summary>
    ///     The user selected a UI TreeNode.  Update the Display based on the new selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TreeNodeSelected(object sender, TreeViewEventArgs e)
    {
        var curNode = (XmlNode) e.Node.Tag;
        Display(curNode);
    }

    // UI Callbacks when buttons in the Form are pressed

    private void OnBnParent(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;
        MoveToNewNodeInTree(curNode.ParentNode);
    }

    private void OnBnOwnerDoc(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;
        MoveToNewNodeInTree(curNode.OwnerDocument);
    }

    private void OnBnPrevSibling(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;
        MoveToNewNodeInTree(curNode.PreviousSibling);
    }

    private void OnBnNextSibling(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;
        MoveToNewNodeInTree(curNode.NextSibling);
    }

    private void OnBnFirstChild(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;
        MoveToNewNodeInTree(curNode.FirstChild);
    }

    private void OnBnLastChild(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;
        MoveToNewNodeInTree(curNode.LastChild);
    }

    private void OnBnDocElement(object sender, EventArgs e)
    {
        var elem = _mXmlDoc.DocumentElement;
        MoveToNewNodeInTree(elem);
    }

    /// <summary>
    ///     Based on a user-specified XPath expression, try to find a matching
    ///     node.  If found, change the background of the label to a different
    ///     color and make that item the current selection in the UI Tree.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBnSelectSingleNode(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;

        try
        {
            var newNode = curNode.SelectSingleNode(_ebXpathPattern.Text);

            if (newNode is not null)
            {
                _tvDom.BeginUpdate();

                SetSelectedNode(_tvDom.Nodes, newNode);
                MoveToNewNodeInTree(newNode);

                _tvDom.EndUpdate();
            }
            else
            {
                MessageBox.Show("No node matches the pattern.", "XPath Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (XPathException ex)
        {
            MessageBox.Show(ex.Message, "XPath Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }

    /// <summary>
    ///     Based on a user-specified XPath expression, try to find any matching
    ///     nodes.  If found, change the background of the labels to a different
    ///     color and make the first item the current selection in the UI Tree.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBnSelectNodes(object sender, EventArgs e)
    {
        var curNode = (XmlNode) _tvDom.SelectedNode.Tag;

        try
        {
            var selNodes = curNode.SelectNodes(_ebXpathPattern.Text);
            if (selNodes is {Count: > 0})
            {
                _tvDom.BeginUpdate();

                SetSelectedNodes(_tvDom.Nodes, selNodes);
                MoveToNewNodeInTree(selNodes[0]);

                _tvDom.EndUpdate();
            }
            else
            {
                MessageBox.Show("No nodes match the pattern.", "XPath Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (XPathException ex)
        {
            MessageBox.Show(ex.Message, "XPath Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }

    private void OnBnClear(object sender, EventArgs e)
    {
        _ebXpathPattern.Text = string.Empty;
        ClearSelectedNodes(_tvDom.Nodes);
    }

    private void OnRbChanged_LabelDisplay(object sender, EventArgs e)
    {
        LoadTree(); // reload the tree with our new display preference set
    }

    private void OnCbChanged_NodeDisplay(object sender, EventArgs e)
    {
        LoadTree(); // reload the tree with our new display preference set
    }

    private void DataItemSelected(object sender, EventArgs e)
    {
        Core.Utils.DataItemSelected(_lvData, new ModelessWindowFactory(this, null));
    }

    /// <summary>
    ///     One of the navigation buttons ("Parent", "First Child", etc) was picked.  Based on the
    ///     XmlNode that those functions returned, find the corresponding UI TreeNode and make it
    ///     the currently selected one.
    /// </summary>
    /// <param name="newXmlNode">The XmlNode that should now be selected</param>
    private void MoveToNewNodeInTree(XmlNode newXmlNode)
    {
        Debug.Assert(newXmlNode is not null); // we should have checked out OK in SetButtonModes()

        var newTreeNode = FindTreeNodeFromXmlNodeTag(_tvDom.Nodes, newXmlNode);
        if (newTreeNode is not null)
            _tvDom.SelectedNode = newTreeNode;
        else
            MessageBox.Show("The node exist in the XML DOM, but not in the UI tree.\nPerhaps you have Text or Comment nodes turned off?",
                "MgdDbg", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    ///     Sometimes we have the Xml DOM node and we need to find where that is in our
    ///     UI tree.  This function will brute-force search the UI tree looking for a
    ///     TreeNode.Tag that matches the XmlNode we are searching for.  Note: this function
    ///     is recursive.
    /// </summary>
    /// <param name="treeNodes">The current collection of nodes to look through</param>
    /// <param name="xmlNode">The XmlNode we are searching for.</param>
    /// <returns>The found TreeNode or null, if not found</returns>
    private TreeNode FindTreeNodeFromXmlNodeTag(TreeNodeCollection treeNodes, XmlNode xmlNode)
    {
        XmlNode tmpXmlNode = null;

        // walk the list of tree nodes looking for a match in the attached
        // Tag object.
        foreach (TreeNode tNode in treeNodes)
        {
            tmpXmlNode = (XmlNode) tNode.Tag;
            if (tmpXmlNode == xmlNode)
                return tNode; // found it

            // Didn't find it, but this node may have children, so recursively
            // look for it here.
            if (tNode.Nodes.Count > 0)
            {
                // if we find it on the recursive call, pop back out,
                // otherwise continue searching at this level
                TreeNode recursiveNode = null;
                recursiveNode = FindTreeNodeFromXmlNodeTag(tNode.Nodes, xmlNode);
                if (recursiveNode is not null) return recursiveNode;
            }
        }

        return null;
    }

    /// <summary>
    ///     When nodes are selected by XPath expressions, we highlighted them by changing
    ///     the background color.  Go through and reset them to normal.  This function handles
    ///     one level of the tree at a time and then goes recursive.
    /// </summary>
    /// <param name="treeNodes">The "root" of this protion of the tree</param>
    private void ClearSelectedNodes(TreeNodeCollection treeNodes)
    {
        foreach (TreeNode tNode in treeNodes)
        {
            tNode.BackColor = Color.Empty;
            if (tNode.Nodes.Count > 0) ClearSelectedNodes(tNode.Nodes);
        }
    }

    /// <summary>
    ///     Change the background color of the matching node
    /// </summary>
    /// <param name="treeNodes">The "root" of this section of the tree</param>
    /// <param name="selNode">The XmlNode we are trying to find</param>
    private void SetSelectedNode(TreeNodeCollection treeNodes, XmlNode selNode)
    {
        foreach (TreeNode tNode in treeNodes)
        {
            tNode.BackColor = selNode == (XmlNode) tNode.Tag
                ? Color.LightSkyBlue
                : Color.Empty;

            if (tNode.Nodes.Count > 0) SetSelectedNode(tNode.Nodes, selNode);
        }
    }

    /// <summary>
    ///     Same as SetSelectedNode(), but with a set of matches.
    ///     NOTE: You cannot manually add to an XmlNodeList, so I couldn't
    ///     have the above function call this one.
    /// </summary>
    /// <param name="treeNodes"></param>
    /// <param name="selNodes"></param>
    private void SetSelectedNodes(TreeNodeCollection treeNodes, XmlNodeList selNodes)
    {
        foreach (TreeNode tNode in treeNodes)
        {
            tNode.BackColor = NodeListContains(selNodes, (XmlNode) tNode.Tag) ? Color.LightSkyBlue : Color.Empty;
            if (tNode.Nodes.Count > 0) SetSelectedNodes(tNode.Nodes, selNodes);
        }
    }

    /// <summary>
    ///     Is a given node part of the nodeSet?
    /// </summary>
    /// <param name="nodeSet">Set of nodes to search</param>
    /// <param name="findNode">Node we are searching for</param>
    /// <returns>true if found, false if not</returns>
    private bool NodeListContains(XmlNodeList nodeSet, XmlNode findNode)
    {
        return nodeSet
            .Cast<XmlNode>()
            .Any(tmpNode => tmpNode == findNode);
    }
}