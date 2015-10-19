namespace AnXinWH.ShiPin
{
    partial class Form2
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
            this.isecVideoZXV1 = new AnXinWH.ShiPinZXVOCX.ISECVideoZXV();
            this.SuspendLayout();
            // 
            // isecVideoZXV1
            // 
            this.isecVideoZXV1._device_id = null;
            this.isecVideoZXV1._fileStreamHandle = 0;
            this.isecVideoZXV1.Location = new System.Drawing.Point(12, 12);
            this.isecVideoZXV1.Name = "isecVideoZXV1";
            this.isecVideoZXV1.Size = new System.Drawing.Size(861, 630);
            this.isecVideoZXV1.TabIndex = 0;
            this.isecVideoZXV1.tmpmsg = null;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 643);
            this.Controls.Add(this.isecVideoZXV1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private ShiPinZXVOCX.ISECVideoZXV isecVideoZXV1;
    }
}