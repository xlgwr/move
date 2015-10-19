namespace AnXinWH.ShiPinTianDyOCX
{
    partial class ISECVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ISECVideo));
            this.m_video = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // m_video
            // 
            this.m_video.AutoSize = true;
            this.m_video.BackColor = System.Drawing.SystemColors.ControlDark;
            this.m_video.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_video.BackgroundImage")));
            this.m_video.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.m_video.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_video.Location = new System.Drawing.Point(0, 0);
            this.m_video.Name = "m_video";
            this.m_video.Size = new System.Drawing.Size(600, 500);
            this.m_video.TabIndex = 21;
            // 
            // ISECVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_video);
            this.Name = "ISECVideo";
            this.Size = new System.Drawing.Size(600, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel m_video;


    }
}
