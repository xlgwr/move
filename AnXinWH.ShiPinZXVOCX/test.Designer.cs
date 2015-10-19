namespace AnXinWH.ShiPinZXVOCX
{
    partial class test
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(test));
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.picVideo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlVideo
            // 
            this.pnlVideo.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pnlVideo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlVideo.BackgroundImage")));
            this.pnlVideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlVideo.Location = new System.Drawing.Point(8, 9);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(418, 316);
            this.pnlVideo.TabIndex = 5;
            // 
            // picVideo
            // 
            this.picVideo.BackColor = System.Drawing.SystemColors.Control;
            this.picVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picVideo.Location = new System.Drawing.Point(0, 0);
            this.picVideo.Name = "picVideo";
            this.picVideo.Size = new System.Drawing.Size(442, 339);
            this.picVideo.TabIndex = 4;
            this.picVideo.TabStop = false;
            // 
            // test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlVideo);
            this.Controls.Add(this.picVideo);
            this.Name = "test";
            this.Size = new System.Drawing.Size(442, 339);
            ((System.ComponentModel.ISupportInitialize)(this.picVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlVideo;
        public System.Windows.Forms.PictureBox picVideo;

    }
}
