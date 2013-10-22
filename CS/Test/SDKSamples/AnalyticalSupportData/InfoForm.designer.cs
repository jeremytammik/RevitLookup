//
// (C) Copyright 1994-2006 by Autodesk, Inc. All rights reserved
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

namespace RevitLookup.Test.SDKSamples.AnalyticalSupportData
{
    partial class InfoForm
    {
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.m_bnOK = new System.Windows.Forms.Button( );
            this.m_dataGrid = new System.Windows.Forms.DataGridView( );
            this.m_colId = new System.Windows.Forms.DataGridViewTextBoxColumn( );
            this.m_colTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn( );
            this.m_colSupportType = new System.Windows.Forms.DataGridViewTextBoxColumn( );
            this.m_colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn( );
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrid)).BeginInit( );
            this.SuspendLayout( );
            // 
            // m_bnOK
            // 
            this.m_bnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.m_bnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_bnOK.Location = new System.Drawing.Point(304, 361);
            this.m_bnOK.Margin = new System.Windows.Forms.Padding(2);
            this.m_bnOK.Name = "m_bnOK";
            this.m_bnOK.Size = new System.Drawing.Size(68, 21);
            this.m_bnOK.TabIndex = 0;
            this.m_bnOK.Text = "OK";
            this.m_bnOK.UseVisualStyleBackColor = true;
            // 
            // m_dataGrid
            // 
            this.m_dataGrid.AllowUserToAddRows = false;
            this.m_dataGrid.AllowUserToDeleteRows = false;
            this.m_dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_dataGrid.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.m_dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_colId,
            this.m_colTypeName,
            this.m_colSupportType,
            this.m_colRemark});
            this.m_dataGrid.Location = new System.Drawing.Point(11, 11);
            this.m_dataGrid.Margin = new System.Windows.Forms.Padding(2);
            this.m_dataGrid.Name = "m_dataGrid";
            this.m_dataGrid.ReadOnly = true;
            this.m_dataGrid.RowHeadersVisible = false;
            this.m_dataGrid.RowTemplate.Height = 24;
            this.m_dataGrid.Size = new System.Drawing.Size(652, 339);
            this.m_dataGrid.TabIndex = 1;
            // 
            // m_colId
            // 
            this.m_colId.HeaderText = "Element Id";
            this.m_colId.Name = "m_colId";
            this.m_colId.ReadOnly = true;
            // 
            // m_colTypeName
            // 
            this.m_colTypeName.HeaderText = "Element Type";
            this.m_colTypeName.Name = "m_colTypeName";
            this.m_colTypeName.ReadOnly = true;
            this.m_colTypeName.Width = 200;
            // 
            // m_colSupportType
            // 
            this.m_colSupportType.HeaderText = "Support Type";
            this.m_colSupportType.Name = "m_colSupportType";
            this.m_colSupportType.ReadOnly = true;
            this.m_colSupportType.Width = 150;
            // 
            // m_colRemark
            // 
            this.m_colRemark.HeaderText = "Remark";
            this.m_colRemark.Name = "m_colRemark";
            this.m_colRemark.ReadOnly = true;
            this.m_colRemark.Width = 200;
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 393);
            this.Controls.Add(this.m_dataGrid);
            this.Controls.Add(this.m_bnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoForm";
            this.ShowInTaskbar = false;
            this.Text = "Analytical Support Data";
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrid)).EndInit( );
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_bnOK;
        private System.Windows.Forms.DataGridView m_dataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colSupportType;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colRemark;
    }
}