namespace AudioBooker
{
    partial class Form_Audiobooker2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Audiobooker2));
            this.photographBookReader = new Audiobooker.controls.PhotographBookReader();
            this.recordingControls = new Audiobooker.controls.RecordingControls();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // photographBookReader
            // 
            this.photographBookReader.AllowDrop = true;
            this.photographBookReader.BackColor = System.Drawing.SystemColors.ControlDark;
            this.photographBookReader.Location = new System.Drawing.Point(66, 87);
            this.photographBookReader.Name = "photographBookReader";
            this.photographBookReader.Size = new System.Drawing.Size(1009, 704);
            this.photographBookReader.TabIndex = 1;
            // 
            // recordingControls
            // 
            this.recordingControls.Location = new System.Drawing.Point(1489, 49);
            this.recordingControls.Margin = new System.Windows.Forms.Padding(6);
            this.recordingControls.Name = "recordingControls";
            this.recordingControls.Size = new System.Drawing.Size(436, 188);
            this.recordingControls.TabIndex = 0;
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(1507, 276);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(418, 771);
            this.txtConsole.TabIndex = 1;
            this.txtConsole.Text = "";
            // 
            // Form_Audiobooker2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1966, 1134);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.photographBookReader);
            this.Controls.Add(this.recordingControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form_Audiobooker2";
            this.Text = "Form_Audiobooker2";
            this.Load += new System.EventHandler(this.Form_Audiobooker2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_Audiobooker2_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private Audiobooker.controls.PhotographBookReader photographBookReader;
        private Audiobooker.controls.RecordingControls recordingControls;
        private System.Windows.Forms.RichTextBox txtConsole;
    }
}