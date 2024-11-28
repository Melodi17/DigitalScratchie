namespace DigitalScratchie
{
    partial class ScratchForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScratchForm
            // 
            this.AutoScaleDimensions = new SizeF(13F, 32F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(731, 743);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScratchForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ScratchForm";
            this.Paint += this.Form1_Paint;
            this.MouseDown += this.ScratchForm_MouseDown;
            this.MouseMove += this.ScratchForm_MouseMove;
            this.MouseUp += this.ScratchForm_MouseUp;
            this.ResumeLayout(false);
        }

        #endregion
    }
}
