namespace AudioBooker {
    partial class Form_Audiobooker {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Audiobooker));
            this.txtArea = new System.Windows.Forms.RichTextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.recordingControls = new Audiobooker.controls.RecordingControls();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.webBrowserControl = new System.Windows.Forms.WebBrowser();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.soundEffectPanel9 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel1 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel8 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel5 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel2 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel6 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel4 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel3 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel7 = new Audiobooker.controls.SoundEffectPanel();
            this.soundEffectPanel10 = new Audiobooker.controls.SoundEffectPanel();
            this.segmentProgressView = new Audiobooker.controls.SegmentProgressView();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtArea
            // 
            this.txtArea.Location = new System.Drawing.Point(24, 33);
            this.txtArea.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(1176, 1052);
            this.txtArea.TabIndex = 0;
            this.txtArea.Text = "";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(1518, 1372);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(393, 32);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Where did I save that last file?";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // recordingControls
            // 
            this.recordingControls.Location = new System.Drawing.Point(24, 1100);
            this.recordingControls.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.recordingControls.Name = "recordingControls";
            this.recordingControls.Size = new System.Drawing.Size(1228, 304);
            this.recordingControls.TabIndex = 13;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(1242, 23);
            this.tabControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1218, 1184);
            this.tabControl.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.webBrowserControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 40);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Size = new System.Drawing.Size(1210, 1140);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Document";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // webBrowserControl
            // 
            this.webBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserControl.Location = new System.Drawing.Point(6, 6);
            this.webBrowserControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.webBrowserControl.MinimumSize = new System.Drawing.Size(40, 39);
            this.webBrowserControl.Name = "webBrowserControl";
            this.webBrowserControl.Size = new System.Drawing.Size(1198, 1128);
            this.webBrowserControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.soundEffectPanel9);
            this.tabPage2.Controls.Add(this.soundEffectPanel1);
            this.tabPage2.Controls.Add(this.soundEffectPanel8);
            this.tabPage2.Controls.Add(this.soundEffectPanel5);
            this.tabPage2.Controls.Add(this.soundEffectPanel2);
            this.tabPage2.Controls.Add(this.soundEffectPanel6);
            this.tabPage2.Controls.Add(this.soundEffectPanel4);
            this.tabPage2.Controls.Add(this.soundEffectPanel3);
            this.tabPage2.Controls.Add(this.soundEffectPanel7);
            this.tabPage2.Controls.Add(this.soundEffectPanel10);
            this.tabPage2.Location = new System.Drawing.Point(4, 40);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage2.Size = new System.Drawing.Size(1210, 1140);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sound effects";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // soundEffectPanel9
            // 
            this.soundEffectPanel9.Location = new System.Drawing.Point(14, 835);
            this.soundEffectPanel9.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel9.Mp3Filename = null;
            this.soundEffectPanel9.Name = "soundEffectPanel9";
            this.soundEffectPanel9.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel9.TabIndex = 5;
            // 
            // soundEffectPanel1
            // 
            this.soundEffectPanel1.Location = new System.Drawing.Point(12, 12);
            this.soundEffectPanel1.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel1.Mp3Filename = null;
            this.soundEffectPanel1.Name = "soundEffectPanel1";
            this.soundEffectPanel1.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel1.TabIndex = 1;
            // 
            // soundEffectPanel8
            // 
            this.soundEffectPanel8.Location = new System.Drawing.Point(14, 732);
            this.soundEffectPanel8.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel8.Mp3Filename = null;
            this.soundEffectPanel8.Name = "soundEffectPanel8";
            this.soundEffectPanel8.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel8.TabIndex = 5;
            // 
            // soundEffectPanel5
            // 
            this.soundEffectPanel5.Location = new System.Drawing.Point(14, 424);
            this.soundEffectPanel5.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel5.Mp3Filename = null;
            this.soundEffectPanel5.Name = "soundEffectPanel5";
            this.soundEffectPanel5.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel5.TabIndex = 5;
            // 
            // soundEffectPanel2
            // 
            this.soundEffectPanel2.Location = new System.Drawing.Point(12, 114);
            this.soundEffectPanel2.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel2.Mp3Filename = null;
            this.soundEffectPanel2.Name = "soundEffectPanel2";
            this.soundEffectPanel2.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel2.TabIndex = 2;
            // 
            // soundEffectPanel6
            // 
            this.soundEffectPanel6.Location = new System.Drawing.Point(12, 527);
            this.soundEffectPanel6.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel6.Mp3Filename = null;
            this.soundEffectPanel6.Name = "soundEffectPanel6";
            this.soundEffectPanel6.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel6.TabIndex = 5;
            // 
            // soundEffectPanel4
            // 
            this.soundEffectPanel4.Location = new System.Drawing.Point(14, 322);
            this.soundEffectPanel4.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel4.Mp3Filename = null;
            this.soundEffectPanel4.Name = "soundEffectPanel4";
            this.soundEffectPanel4.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel4.TabIndex = 4;
            // 
            // soundEffectPanel3
            // 
            this.soundEffectPanel3.Location = new System.Drawing.Point(12, 217);
            this.soundEffectPanel3.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel3.Mp3Filename = null;
            this.soundEffectPanel3.Name = "soundEffectPanel3";
            this.soundEffectPanel3.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel3.TabIndex = 3;
            // 
            // soundEffectPanel7
            // 
            this.soundEffectPanel7.Location = new System.Drawing.Point(14, 630);
            this.soundEffectPanel7.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel7.Mp3Filename = null;
            this.soundEffectPanel7.Name = "soundEffectPanel7";
            this.soundEffectPanel7.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel7.TabIndex = 5;
            // 
            // soundEffectPanel10
            // 
            this.soundEffectPanel10.Location = new System.Drawing.Point(14, 938);
            this.soundEffectPanel10.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.soundEffectPanel10.Mp3Filename = null;
            this.soundEffectPanel10.Name = "soundEffectPanel10";
            this.soundEffectPanel10.Size = new System.Drawing.Size(672, 91);
            this.soundEffectPanel10.TabIndex = 5;
            // 
            // segmentProgressView
            // 
            this.segmentProgressView.CurSegment = null;
            this.segmentProgressView.Location = new System.Drawing.Point(1310, 1246);
            this.segmentProgressView.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.segmentProgressView.Name = "segmentProgressView";
            this.segmentProgressView.Size = new System.Drawing.Size(890, 91);
            this.segmentProgressView.TabIndex = 15;
            // 
            // Form_Audiobooker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2484, 1422);
            this.Controls.Add(this.segmentProgressView);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.recordingControls);
            this.Controls.Add(this.linkLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form_Audiobooker";
            this.Text = "Form1";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtArea;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Audiobooker.controls.RecordingControls recordingControls;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel9;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel1;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel2;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel3;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel10;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel4;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel5;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel8;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel6;
        private Audiobooker.controls.SoundEffectPanel soundEffectPanel7;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser webBrowserControl;
        private Audiobooker.controls.SegmentProgressView segmentProgressView;
    }
}

