namespace Audiobooker.controls {
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
            this.Devices = new System.Windows.Forms.ComboBox();
            this.btnRec = new System.Windows.Forms.Button();
            this.focusDiverter = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Devices
            // 
            this.Devices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Devices.FormattingEnabled = true;
            this.Devices.Location = new System.Drawing.Point(30, 122);
            this.Devices.Margin = new System.Windows.Forms.Padding(8);
            this.Devices.Name = "Devices";
            this.Devices.Size = new System.Drawing.Size(372, 39);
            this.Devices.TabIndex = 13;
            this.Devices.SelectedIndexChanged += new System.EventHandler(this.Devices_SelectedIndexChanged_1);
            // 
            // btnRec
            // 
            this.btnRec.BackColor = System.Drawing.Color.Red;
            this.btnRec.Location = new System.Drawing.Point(30, 23);
            this.btnRec.Margin = new System.Windows.Forms.Padding(6);
            this.btnRec.Name = "btnRec";
            this.btnRec.Size = new System.Drawing.Size(141, 85);
            this.btnRec.TabIndex = 12;
            this.btnRec.Text = "Rec";
            this.btnRec.UseVisualStyleBackColor = false;
            this.btnRec.Click += new System.EventHandler(this.btnRec_Click_1);
            // 
            // focusDiverter
            // 
            this.focusDiverter.Location = new System.Drawing.Point(66, 56);
            this.focusDiverter.Margin = new System.Windows.Forms.Padding(6);
            this.focusDiverter.Name = "focusDiverter";
            this.focusDiverter.Size = new System.Drawing.Size(28, 19);
            this.focusDiverter.TabIndex = 17;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(206, 50);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(71, 32);
            this.lblTime.TabIndex = 18;
            this.lblTime.Text = "0:00";
            // 
            // RecordingControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnRec);
            this.Controls.Add(this.focusDiverter);
            this.Controls.Add(this.Devices);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "RecordingControls";
            this.Size = new System.Drawing.Size(425, 188);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Devices;
        private System.Windows.Forms.Button btnRec;
        private System.Windows.Forms.Panel focusDiverter;
        private System.Windows.Forms.Label lblTime;
    }
}
