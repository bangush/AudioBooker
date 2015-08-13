namespace MyControlz {
    partial class RecordingControls {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.Devices = new System.Windows.Forms.ComboBox();
            this.btnRec = new System.Windows.Forms.Button();
            this.focusDiverter = new System.Windows.Forms.Panel();
            this.btnRefocus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Device";
            // 
            // Devices
            // 
            this.Devices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Devices.FormattingEnabled = true;
            this.Devices.Location = new System.Drawing.Point(81, 85);
            this.Devices.Margin = new System.Windows.Forms.Padding(4);
            this.Devices.Name = "Devices";
            this.Devices.Size = new System.Drawing.Size(388, 24);
            this.Devices.TabIndex = 13;
            this.Devices.SelectedIndexChanged += new System.EventHandler(this.Devices_SelectedIndexChanged_1);
            // 
            // btnRec
            // 
            this.btnRec.BackColor = System.Drawing.Color.Red;
            this.btnRec.Location = new System.Drawing.Point(15, 12);
            this.btnRec.Name = "btnRec";
            this.btnRec.Size = new System.Drawing.Size(75, 44);
            this.btnRec.TabIndex = 12;
            this.btnRec.Text = "Rec";
            this.btnRec.UseVisualStyleBackColor = false;
            this.btnRec.Click += new System.EventHandler(this.btnRec_Click_1);
            // 
            // focusDiverter
            // 
            this.focusDiverter.Location = new System.Drawing.Point(33, 29);
            this.focusDiverter.Name = "focusDiverter";
            this.focusDiverter.Size = new System.Drawing.Size(14, 10);
            this.focusDiverter.TabIndex = 17;
            // 
            // btnRefocus
            // 
            this.btnRefocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRefocus.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRefocus.Location = new System.Drawing.Point(488, 29);
            this.btnRefocus.Name = "btnRefocus";
            this.btnRefocus.Size = new System.Drawing.Size(105, 99);
            this.btnRefocus.TabIndex = 18;
            this.btnRefocus.Text = "Refocus";
            this.btnRefocus.UseVisualStyleBackColor = false;
            this.btnRefocus.Click += new System.EventHandler(this.btnRefocus_Click);
            // 
            // RecordingControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRefocus);
            this.Controls.Add(this.btnRec);
            this.Controls.Add(this.focusDiverter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Devices);
            this.Name = "RecordingControls";
            this.Size = new System.Drawing.Size(616, 157);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Devices;
        private System.Windows.Forms.Button btnRec;
        private System.Windows.Forms.Panel focusDiverter;
        private System.Windows.Forms.Button btnRefocus;
    }
}
