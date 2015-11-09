namespace AnXinWH.ShiPinNewVideo
{
    partial class Form2OCX
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
            this.isecNewVideo1 = new AnXinWH.ShiPinNewVideoOCX.ISECNewVideo();
            this.SuspendLayout();
            // 
            // isecNewVideo1
            // 
            this.isecNewVideo1._currMsg = null;
            this.isecNewVideo1._isScroll = false;
            this.isecNewVideo1._notice = null;
            this.isecNewVideo1._videoname = null;
            this.isecNewVideo1.Location = new System.Drawing.Point(12, 11);
            this.isecNewVideo1.m_iPlaySpeed = 0;
            this.isecNewVideo1.Name = "isecNewVideo1";
            this.isecNewVideo1.Size = new System.Drawing.Size(811, 659);
            this.isecNewVideo1.TabIndex = 0;
            this.isecNewVideo1.tmpmsg = null;
            // 
            // Form2OCX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 677);
            this.Controls.Add(this.isecNewVideo1);
            this.Name = "Form2OCX";
            this.Text = "Form2OCX";
            this.Load += new System.EventHandler(this.Form2OCX_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ShiPinNewVideoOCX.ISECNewVideo isecNewVideo1;
    }
}