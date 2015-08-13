namespace IlyaFranker {
    partial class Form_Franker {
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
            this.txtArea = new System.Windows.Forms.RichTextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.webBrowserControl = new System.Windows.Forms.WebBrowser();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.segmentProgressView = new MyControlz.SegmentProgressView();
            this.recordingControls = new MyControlz.RecordingControls();
            this.lblReverse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtArea
            // 
            this.txtArea.Location = new System.Drawing.Point(12, 25);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(608, 532);
            this.txtArea.TabIndex = 0;
            this.txtArea.Text = "";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(1020, 704);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(198, 17);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Where did I save that last file?";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // webBrowserControl
            // 
            this.webBrowserControl.Location = new System.Drawing.Point(636, 12);
            this.webBrowserControl.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserControl.Name = "webBrowserControl";
            this.webBrowserControl.Size = new System.Drawing.Size(582, 636);
            this.webBrowserControl.TabIndex = 15;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 9);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(607, 10);
            this.progressBar.TabIndex = 16;
            // 
            // segmentProgressView
            // 
            this.segmentProgressView.CurSegment = null;
            this.segmentProgressView.Location = new System.Drawing.Point(702, 654);
            this.segmentProgressView.Name = "segmentProgressView";
            this.segmentProgressView.Size = new System.Drawing.Size(445, 47);
            this.segmentProgressView.TabIndex = 14;
            // 
            // recordingControls
            // 
            this.recordingControls.Location = new System.Drawing.Point(13, 564);
            this.recordingControls.Name = "recordingControls";
            this.recordingControls.Size = new System.Drawing.Size(617, 157);
            this.recordingControls.TabIndex = 13;
            // 
            // lblReverse
            // 
            this.lblReverse.AutoSize = true;
            this.lblReverse.Location = new System.Drawing.Point(654, 708);
            this.lblReverse.Name = "lblReverse";
            this.lblReverse.Size = new System.Drawing.Size(75, 17);
            this.lblReverse.TabIndex = 17;
            this.lblReverse.Text = "lblReverse";
            // 
            // Form_Franker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 734);
            this.Controls.Add(this.lblReverse);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.webBrowserControl);
            this.Controls.Add(this.segmentProgressView);
            this.Controls.Add(this.recordingControls);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtArea);
            this.Name = "Form_Franker";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtArea;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private MyControlz.RecordingControls recordingControls;
        private MyControlz.SegmentProgressView segmentProgressView;
        private System.Windows.Forms.WebBrowser webBrowserControl;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblReverse;
    }
}

