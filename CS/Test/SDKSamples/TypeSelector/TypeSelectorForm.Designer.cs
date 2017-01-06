namespace RevitLookup.Test.SDKSamples.TypeSelector {
    partial class TypeSelectorForm {
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
            if (disposing && (components != null)) {
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
            this.m_tvObjs = new System.Windows.Forms.TreeView();
            this.m_lbSymbols = new System.Windows.Forms.ListBox();
            this.m_bnOK = new System.Windows.Forms.Button();
            this.m_txtElements = new System.Windows.Forms.Label();
            this.m_txtSymbols = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_tvObjs
            // 
            this.m_tvObjs.Location = new System.Drawing.Point(13, 26);
            this.m_tvObjs.Name = "m_tvObjs";
            this.m_tvObjs.Size = new System.Drawing.Size(243, 343);
            this.m_tvObjs.TabIndex = 0;
            this.m_tvObjs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeNodeSelect);
            // 
            // m_lbSymbols
            // 
            this.m_lbSymbols.FormattingEnabled = true;
            this.m_lbSymbols.Location = new System.Drawing.Point(274, 26);
            this.m_lbSymbols.Name = "m_lbSymbols";
            this.m_lbSymbols.Size = new System.Drawing.Size(356, 342);
            this.m_lbSymbols.TabIndex = 1;
            this.m_lbSymbols.SelectedIndexChanged += new System.EventHandler(this.OnTypeChanged);
            // 
            // m_bnOK
            // 
            this.m_bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_bnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_bnOK.Location = new System.Drawing.Point(284, 390);
            this.m_bnOK.Name = "m_bnOK";
            this.m_bnOK.Size = new System.Drawing.Size(75, 23);
            this.m_bnOK.TabIndex = 2;
            this.m_bnOK.Text = "OK";
            this.m_bnOK.UseVisualStyleBackColor = true;
            // 
            // m_txtElements
            // 
            this.m_txtElements.AutoSize = true;
            this.m_txtElements.Location = new System.Drawing.Point(13, 7);
            this.m_txtElements.Name = "m_txtElements";
            this.m_txtElements.Size = new System.Drawing.Size(50, 13);
            this.m_txtElements.TabIndex = 3;
            this.m_txtElements.Text = "Elements";
            // 
            // m_txtSymbols
            // 
            this.m_txtSymbols.AutoSize = true;
            this.m_txtSymbols.Location = new System.Drawing.Point(274, 6);
            this.m_txtSymbols.Name = "m_txtSymbols";
            this.m_txtSymbols.Size = new System.Drawing.Size(46, 13);
            this.m_txtSymbols.TabIndex = 4;
            this.m_txtSymbols.Text = "Symbols";
            // 
            // TypeSelectorForm
            // 
            this.AcceptButton = this.m_bnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_bnOK;
            this.ClientSize = new System.Drawing.Size(642, 425);
            this.Controls.Add(this.m_txtSymbols);
            this.Controls.Add(this.m_txtElements);
            this.Controls.Add(this.m_bnOK);
            this.Controls.Add(this.m_lbSymbols);
            this.Controls.Add(this.m_tvObjs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TypeSelectorForm";
            this.Text = "Select Types for Elements";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView m_tvObjs;
        private System.Windows.Forms.ListBox m_lbSymbols;
        private System.Windows.Forms.Button m_bnOK;
        private System.Windows.Forms.Label m_txtElements;
        private System.Windows.Forms.Label m_txtSymbols;
    }
}