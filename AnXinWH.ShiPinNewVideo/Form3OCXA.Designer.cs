namespace AnXinWH.ShiPinNewVideo
{
    partial class Form3OCXA
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
            this.isecNewVideoA1 = new AnXinWH.ShiPinNewVideoOCX.ISECNewVideoA();
            this.SuspendLayout();
            // 
            // isecNewVideoA1
            // 
            this.isecNewVideoA1._isScroll = false;
            this.isecNewVideoA1._playNow = false;
            this.isecNewVideoA1.Location = new System.Drawing.Point(12, 8);
            this.isecNewVideoA1.Name = "isecNewVideoA1";
            this.isecNewVideoA1.Size = new System.Drawing.Size(914, 620);
            this.isecNewVideoA1.TabIndex = 0;
            this.isecNewVideoA1.Load += new System.EventHandler(this.isecNewVideoA1_Load);
            // 
            // Form3OCXA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 625);
            this.Controls.Add(this.isecNewVideoA1);
            this.Name = "Form3OCXA";
            this.Text = "Form3OCXA";
            this.Load += new System.EventHandler(this.Form3OCXA_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ShiPinNewVideoOCX.ISECNewVideoA isecNewVideoA1;
    }
}