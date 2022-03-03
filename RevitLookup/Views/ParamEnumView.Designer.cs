using RevitLookup.Views.Utils;

namespace RevitLookup.Views {
    partial class ParamEnumView {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null)) {
                components.Dispose( );
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamEnumView));
            this.listView = new System.Windows.Forms.ListView();
            this.m_colEnum = new System.Windows.Forms.ColumnHeader();
            this.m_colVal = new System.Windows.Forms.ColumnHeader();
            this.listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_bnOk = new System.Windows.Forms.Button();
            this.m_printDialog = new System.Windows.Forms.PrintDialog();
            this.m_printDocument = new System.Drawing.Printing.PrintDocument();
            this.m_printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listViewContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.m_colEnum, this.m_colVal});
            this.listView.ContextMenuStrip = this.listViewContextMenuStrip;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(3, 3);
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(503, 422);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnColumnClick);
            // 
            // m_colEnum
            // 
            this.m_colEnum.Text = "Enum";
            this.m_colEnum.Width = 280;
            // 
            // m_colVal
            // 
            this.m_colVal.Text = "Value";
            this.m_colVal.Width = 802;
            // 
            // listViewContextMenuStrip
            // 
            this.listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.copyToolStripMenuItem});
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
            // m_bnOk
            // 
            this.m_bnOk.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.m_bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_bnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnOk.Location = new System.Drawing.Point(3, 431);
            this.m_bnOk.Name = "m_bnOk";
            this.m_bnOk.Size = new System.Drawing.Size(503, 23);
            this.m_bnOk.TabIndex = 1;
            this.m_bnOk.Text = "OK";
            this.m_bnOk.UseVisualStyleBackColor = true;
            this.m_bnOk.Click += new System.EventHandler(this.ButtonOk_Click);
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
            this.m_printPreviewDialog.Icon = ((System.Drawing.Icon) (resources.GetObject("m_printPreviewDialog.Icon")));
            this.m_printPreviewDialog.Name = "m_printPreviewDialog";
            this.m_printPreviewDialog.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.toolStripButton1, this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(509, 25);
            this.toolStrip1.TabIndex = 2;
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
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.m_bnOk);
            this.panel1.Controls.Add(this.listView);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(509, 458);
            this.panel1.TabIndex = 3;
            // 
            // ParamEnumView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_bnOk;
            this.ClientSize = new System.Drawing.Size(509, 486);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(525, 200);
            this.Name = "ParamEnumView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enum Mappings";
            this.listViewContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panel1;

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader       m_colEnum;
        private System.Windows.Forms.ColumnHeader       m_colVal;
        private System.Windows.Forms.Button m_bnOk;
        
        private ListViewColumnSorter    colSorter;
        private System.Windows.Forms.ContextMenuStrip   listViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem  copyToolStripMenuItem;
        private System.Windows.Forms.PrintDialog        m_printDialog;
        private System.Drawing.Printing.PrintDocument   m_printDocument;
        private System.Windows.Forms.PrintPreviewDialog m_printPreviewDialog;
        private System.Windows.Forms.ToolStrip          toolStrip1;
        private System.Windows.Forms.ToolStripButton    toolStripButton1;
        private System.Windows.Forms.ToolStripButton    toolStripButton2;        
    }
}