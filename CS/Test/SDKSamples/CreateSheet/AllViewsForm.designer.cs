namespace RevitLookup.Test.SDKSamples.CreateSheet {

    partial class AllViewsForm
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
            this.m_grpboxViews = new System.Windows.Forms.GroupBox();
            this.m_tvViews = new System.Windows.Forms.TreeView();
            this.m_grpboxSheet = new System.Windows.Forms.GroupBox();
            this.m_lbTitleBlocks = new System.Windows.Forms.ListBox();
            this.m_txtSheetName = new System.Windows.Forms.Label();
            this.m_ebSheetName = new System.Windows.Forms.TextBox();
            this.m_txtTitleBlocks = new System.Windows.Forms.Label();
            this.m_bnCancel = new System.Windows.Forms.Button();
            this.m_bnOK = new System.Windows.Forms.Button();
            this.m_grpboxViews.SuspendLayout();
            this.m_grpboxSheet.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_grpboxViews
            // 
            this.m_grpboxViews.Controls.Add(this.m_tvViews);
            this.m_grpboxViews.Location = new System.Drawing.Point(12, 12);
            this.m_grpboxViews.Name = "m_grpboxViews";
            this.m_grpboxViews.Size = new System.Drawing.Size(212, 236);
            this.m_grpboxViews.TabIndex = 0;
            this.m_grpboxViews.TabStop = false;
            this.m_grpboxViews.Text = "All Views";
            // 
            // m_tvViews
            // 
            this.m_tvViews.CheckBoxes = true;
            this.m_tvViews.Location = new System.Drawing.Point(6, 19);
            this.m_tvViews.Name = "m_tvViews";
            this.m_tvViews.Size = new System.Drawing.Size(200, 211);
            this.m_tvViews.TabIndex = 0;
            this.m_tvViews.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterCheck_TreeView);
            // 
            // m_grpboxSheet
            // 
            this.m_grpboxSheet.Controls.Add(this.m_lbTitleBlocks);
            this.m_grpboxSheet.Controls.Add(this.m_txtSheetName);
            this.m_grpboxSheet.Controls.Add(this.m_ebSheetName);
            this.m_grpboxSheet.Controls.Add(this.m_txtTitleBlocks);
            this.m_grpboxSheet.Location = new System.Drawing.Point(230, 12);
            this.m_grpboxSheet.Name = "m_grpboxSheet";
            this.m_grpboxSheet.Size = new System.Drawing.Size(217, 236);
            this.m_grpboxSheet.TabIndex = 1;
            this.m_grpboxSheet.TabStop = false;
            this.m_grpboxSheet.Text = "Generate Sheet";
            // 
            // m_lbTitleBlocks
            // 
            this.m_lbTitleBlocks.FormattingEnabled = true;
            this.m_lbTitleBlocks.Location = new System.Drawing.Point(9, 44);
            this.m_lbTitleBlocks.Name = "m_lbTitleBlocks";
            this.m_lbTitleBlocks.Size = new System.Drawing.Size(202, 160);
            this.m_lbTitleBlocks.TabIndex = 6;
            // 
            // m_txtSheetName
            // 
            this.m_txtSheetName.AutoSize = true;
            this.m_txtSheetName.Location = new System.Drawing.Point(6, 213);
            this.m_txtSheetName.Name = "m_txtSheetName";
            this.m_txtSheetName.Size = new System.Drawing.Size(69, 13);
            this.m_txtSheetName.TabIndex = 5;
            this.m_txtSheetName.Text = "Sheet Name:";
            // 
            // m_ebSheetName
            // 
            this.m_ebSheetName.Location = new System.Drawing.Point(78, 210);
            this.m_ebSheetName.Name = "m_ebSheetName";
            this.m_ebSheetName.Size = new System.Drawing.Size(133, 20);
            this.m_ebSheetName.TabIndex = 2;
            this.m_ebSheetName.Text = "Unnamed";
            // 
            // m_txtTitleBlocks
            // 
            this.m_txtTitleBlocks.AutoSize = true;
            this.m_txtTitleBlocks.Location = new System.Drawing.Point(6, 19);
            this.m_txtTitleBlocks.Name = "m_txtTitleBlocks";
            this.m_txtTitleBlocks.Size = new System.Drawing.Size(59, 13);
            this.m_txtTitleBlocks.TabIndex = 3;
            this.m_txtTitleBlocks.Text = "TitleBlocks";
            // 
            // m_bnCancel
            // 
            this.m_bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_bnCancel.Location = new System.Drawing.Point(230, 266);
            this.m_bnCancel.Name = "m_bnCancel";
            this.m_bnCancel.Size = new System.Drawing.Size(75, 23);
            this.m_bnCancel.TabIndex = 4;
            this.m_bnCancel.Text = "&Cancel";
            this.m_bnCancel.UseVisualStyleBackColor = true;
            // 
            // m_bnOK
            // 
            this.m_bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_bnOK.Location = new System.Drawing.Point(149, 266);
            this.m_bnOK.Name = "m_bnOK";
            this.m_bnOK.Size = new System.Drawing.Size(75, 23);
            this.m_bnOK.TabIndex = 3;
            this.m_bnOK.Text = "&OK";
            this.m_bnOK.UseVisualStyleBackColor = true;
            this.m_bnOK.Click += new System.EventHandler(this.OnBnClick_OK);
            // 
            // AllViewsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_bnCancel;
            this.ClientSize = new System.Drawing.Size(461, 301);
            this.Controls.Add(this.m_grpboxSheet);
            this.Controls.Add(this.m_grpboxViews);
            this.Controls.Add(this.m_bnOK);
            this.Controls.Add(this.m_bnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AllViewsForm";
            this.ShowInTaskbar = false;
            this.Text = "Select Views to Place on Sheet";
            this.m_grpboxViews.ResumeLayout(false);
            this.m_grpboxSheet.ResumeLayout(false);
            this.m_grpboxSheet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox   m_grpboxViews;
        private System.Windows.Forms.GroupBox   m_grpboxSheet;
        private System.Windows.Forms.Button     m_bnOK;
        private System.Windows.Forms.Button     m_bnCancel;
        private System.Windows.Forms.TreeView   m_tvViews;
        private System.Windows.Forms.Label      m_txtTitleBlocks;
        private System.Windows.Forms.TextBox    m_ebSheetName;
        private System.Windows.Forms.Label      m_txtSheetName;
        private System.Windows.Forms.ListBox    m_lbTitleBlocks;
    }
}