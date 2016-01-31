namespace Audiobooker.controls
{
    partial class PhotographBookReader
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PhotographBookReader
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.DoubleBuffered = true;
            this.Name = "PhotographBookReader";
            this.Size = new System.Drawing.Size(1009, 704);
            this.Load += new System.EventHandler(this.PhotographBookReader_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PhotographBookReader_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PhotographBookReader_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PhotographBookReader_Paint);
            this.Resize += new System.EventHandler(this.PhotographBookReader_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
