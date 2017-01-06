namespace RevitLookup.Test.Forms
{
    partial class Levels
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
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_levlv = new System.Windows.Forms.ListView();
            this.m_colName = new System.Windows.Forms.ColumnHeader();
            this.m_colElev = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // m_btnOk
            // 
            this.m_btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.m_btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_btnOk.Location = new System.Drawing.Point(114, 277);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(75, 21);
            this.m_btnOk.TabIndex = 1;
            this.m_btnOk.Text = "OK";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
            // 
            // m_levlv
            // 
            this.m_levlv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_levlv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_colName,
            this.m_colElev});
            this.m_levlv.FullRowSelect = true;
            this.m_levlv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.m_levlv.HideSelection = false;
            this.m_levlv.Location = new System.Drawing.Point(12, 12);
            this.m_levlv.MultiSelect = false;
            this.m_levlv.Name = "m_levlv";
            this.m_levlv.Size = new System.Drawing.Size(278, 259);
            this.m_levlv.TabIndex = 2;
            this.m_levlv.UseCompatibleStateImageBehavior = false;
            this.m_levlv.View = System.Windows.Forms.View.Details;
            // 
            // m_colName
            // 
            this.m_colName.Text = "Name";
            this.m_colName.Width = 148;
            // 
            // m_colElev
            // 
            this.m_colElev.Text = "Elevation";
            this.m_colElev.Width = 150;
            // 
            // Levels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btnOk;
            this.ClientSize = new System.Drawing.Size(302, 310);
            this.Controls.Add(this.m_levlv);
            this.Controls.Add(this.m_btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Levels";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Level";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.ListView m_levlv;
        private System.Windows.Forms.ColumnHeader m_colName;
        private System.Windows.Forms.ColumnHeader m_colElev;
    }
}