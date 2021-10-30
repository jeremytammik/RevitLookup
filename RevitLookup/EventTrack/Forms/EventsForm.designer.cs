namespace RevitLookup.EventTrack.Forms
{
    partial class EventsForm {
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
            this.m_grpAppEvents = new System.Windows.Forms.GroupBox();
            this.m_cbAppEventsOn = new System.Windows.Forms.CheckBox();
            this.m_bnOk = new System.Windows.Forms.Button();
            this.m_tabCtrl = new System.Windows.Forms.TabControl();
            this.m_tabPageSys = new System.Windows.Forms.TabPage();
            this.m_bnCancel = new System.Windows.Forms.Button();
            this.m_grpDocEvents = new System.Windows.Forms.GroupBox();
            this.m_cbDocEventsOn = new System.Windows.Forms.CheckBox();
            this.m_grpAppEvents.SuspendLayout();
            this.m_tabCtrl.SuspendLayout();
            this.m_tabPageSys.SuspendLayout();
            this.m_grpDocEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_grpAppEvents
            // 
            this.m_grpAppEvents.Controls.Add(this.m_cbAppEventsOn);
            this.m_grpAppEvents.Location = new System.Drawing.Point(6, 6);
            this.m_grpAppEvents.Name = "m_grpAppEvents";
            this.m_grpAppEvents.Size = new System.Drawing.Size(131, 48);
            this.m_grpAppEvents.TabIndex = 0;
            this.m_grpAppEvents.TabStop = false;
            this.m_grpAppEvents.Text = "Application Events";
            // 
            // m_cbAppEventsOn
            // 
            this.m_cbAppEventsOn.AutoSize = true;
            this.m_cbAppEventsOn.Location = new System.Drawing.Point(16, 19);
            this.m_cbAppEventsOn.Name = "m_cbAppEventsOn";
            this.m_cbAppEventsOn.Size = new System.Drawing.Size(40, 17);
            this.m_cbAppEventsOn.TabIndex = 1;
            this.m_cbAppEventsOn.Text = "On";
            this.m_cbAppEventsOn.UseVisualStyleBackColor = true;
            // 
            // m_bnOk
            // 
            this.m_bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_bnOk.Location = new System.Drawing.Point(115, 206);
            this.m_bnOk.Name = "m_bnOk";
            this.m_bnOk.Size = new System.Drawing.Size(75, 23);
            this.m_bnOk.TabIndex = 5;
            this.m_bnOk.Text = "OK";
            this.m_bnOk.UseVisualStyleBackColor = true;
            this.m_bnOk.Click += new System.EventHandler(this.event_OnBnOkClick);
            // 
            // m_tabCtrl
            // 
            this.m_tabCtrl.Controls.Add(this.m_tabPageSys);
            this.m_tabCtrl.Location = new System.Drawing.Point(12, 12);
            this.m_tabCtrl.Name = "m_tabCtrl";
            this.m_tabCtrl.SelectedIndex = 0;
            this.m_tabCtrl.Size = new System.Drawing.Size(365, 188);
            this.m_tabCtrl.TabIndex = 6;
            // 
            // m_tabPageSys
            // 
            this.m_tabPageSys.Controls.Add(this.m_grpDocEvents);
            this.m_tabPageSys.Controls.Add(this.m_grpAppEvents);
            this.m_tabPageSys.Location = new System.Drawing.Point(4, 22);
            this.m_tabPageSys.Name = "m_tabPageSys";
            this.m_tabPageSys.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabPageSys.Size = new System.Drawing.Size(357, 162);
            this.m_tabPageSys.TabIndex = 0;
            this.m_tabPageSys.Text = "System";
            this.m_tabPageSys.UseVisualStyleBackColor = true;
            // 
            // m_bnCancel
            // 
            this.m_bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_bnCancel.Location = new System.Drawing.Point(198, 206);
            this.m_bnCancel.Name = "m_bnCancel";
            this.m_bnCancel.Size = new System.Drawing.Size(75, 23);
            this.m_bnCancel.TabIndex = 7;
            this.m_bnCancel.Text = "Cancel";
            this.m_bnCancel.UseVisualStyleBackColor = true;
            // 
            // m_grpDocEvents
            // 
            this.m_grpDocEvents.Controls.Add(this.m_cbDocEventsOn);
            this.m_grpDocEvents.Location = new System.Drawing.Point(6, 87);
            this.m_grpDocEvents.Name = "m_grpDocEvents";
            this.m_grpDocEvents.Size = new System.Drawing.Size(131, 48);
            this.m_grpDocEvents.TabIndex = 1;
            this.m_grpDocEvents.TabStop = false;
            this.m_grpDocEvents.Text = "Document Events";
            // 
            // m_cbDocEventsOn
            // 
            this.m_cbDocEventsOn.AutoSize = true;
            this.m_cbDocEventsOn.Location = new System.Drawing.Point(16, 19);
            this.m_cbDocEventsOn.Name = "m_cbDocEventsOn";
            this.m_cbDocEventsOn.Size = new System.Drawing.Size(40, 17);
            this.m_cbDocEventsOn.TabIndex = 1;
            this.m_cbDocEventsOn.Text = "On";
            this.m_cbDocEventsOn.UseVisualStyleBackColor = true;
            // 
            // EventsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_bnCancel;
            this.ClientSize = new System.Drawing.Size(389, 241);
            this.Controls.Add(this.m_bnCancel);
            this.Controls.Add(this.m_tabCtrl);
            this.Controls.Add(this.m_bnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EventsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Events";
            this.m_grpAppEvents.ResumeLayout(false);
            this.m_grpAppEvents.PerformLayout();
            this.m_tabCtrl.ResumeLayout(false);
            this.m_tabPageSys.ResumeLayout(false);
            this.m_grpDocEvents.ResumeLayout(false);
            this.m_grpDocEvents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_grpAppEvents;
        private System.Windows.Forms.CheckBox m_cbAppEventsOn;
        private System.Windows.Forms.Button m_bnOk;
        private System.Windows.Forms.TabControl m_tabCtrl;
        private System.Windows.Forms.TabPage m_tabPageSys;
        private System.Windows.Forms.Button m_bnCancel;
        private System.Windows.Forms.GroupBox m_grpDocEvents;
        private System.Windows.Forms.CheckBox m_cbDocEventsOn;
    }
}