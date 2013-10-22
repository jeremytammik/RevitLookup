namespace RevitLookup.ExIm
{
    partial class BrowseCategory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
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
        private void InitializeComponent ()
        {
            this.m_treeView = new System.Windows.Forms.TreeView();
            this.m_OkBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_treeView
            // 
            this.m_treeView.Location = new System.Drawing.Point(12, 12);
            this.m_treeView.Name = "m_treeView";
            this.m_treeView.Size = new System.Drawing.Size(268, 211);
            this.m_treeView.TabIndex = 0;
            this.m_treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_treeView_AfterSelect);
            // 
            // m_OkBtn
            // 
            this.m_OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_OkBtn.Location = new System.Drawing.Point(103, 231);
            this.m_OkBtn.Name = "m_OkBtn";
            this.m_OkBtn.Size = new System.Drawing.Size(75, 23);
            this.m_OkBtn.TabIndex = 1;
            this.m_OkBtn.Text = "Ok";
            this.m_OkBtn.UseVisualStyleBackColor = true;
            // 
            // BrowseCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.m_OkBtn);
            this.Controls.Add(this.m_treeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BrowseCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Category of Schedule to Import";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView m_treeView;
        private System.Windows.Forms.Button m_OkBtn;
    }
}