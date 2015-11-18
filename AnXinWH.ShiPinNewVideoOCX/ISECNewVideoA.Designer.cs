namespace AnXinWH.ShiPinNewVideoOCX
{
    partial class ISECNewVideoA
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ISECNewVideoA));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl0Msg = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolMenuPlay1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuStopPlay2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuQuickPlay0 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSlowPlay1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl0Msg);
            this.groupBox2.Location = new System.Drawing.Point(4, -4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(797, 65);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // lbl0Msg
            // 
            this.lbl0Msg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl0Msg.ForeColor = System.Drawing.Color.Red;
            this.lbl0Msg.Location = new System.Drawing.Point(4, 14);
            this.lbl0Msg.Name = "lbl0Msg";
            this.lbl0Msg.Size = new System.Drawing.Size(787, 48);
            this.lbl0Msg.TabIndex = 10;
            this.lbl0Msg.Text = "msg";
            this.lbl0Msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl0Msg.Click += new System.EventHandler(this.lbl0Msg_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(784, 474);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuPlay1,
            this.toolMenuStopPlay2,
            this.toolMenuQuickPlay0,
            this.toolSlowPlay1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 92);
            // 
            // toolMenuPlay1
            // 
            this.toolMenuPlay1.Name = "toolMenuPlay1";
            this.toolMenuPlay1.Size = new System.Drawing.Size(124, 22);
            this.toolMenuPlay1.Text = "正常播放";
            this.toolMenuPlay1.Click += new System.EventHandler(this.toolMenuPlay1_Click);
            // 
            // toolMenuStopPlay2
            // 
            this.toolMenuStopPlay2.Name = "toolMenuStopPlay2";
            this.toolMenuStopPlay2.Size = new System.Drawing.Size(124, 22);
            this.toolMenuStopPlay2.Text = "暂停播放";
            this.toolMenuStopPlay2.Click += new System.EventHandler(this.toolMenuStopPlay2_Click);
            // 
            // toolMenuQuickPlay0
            // 
            this.toolMenuQuickPlay0.Name = "toolMenuQuickPlay0";
            this.toolMenuQuickPlay0.Size = new System.Drawing.Size(124, 22);
            this.toolMenuQuickPlay0.Text = "快速播放";
            this.toolMenuQuickPlay0.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolSlowPlay1
            // 
            this.toolSlowPlay1.Name = "toolSlowPlay1";
            this.toolSlowPlay1.Size = new System.Drawing.Size(124, 22);
            this.toolSlowPlay1.Text = "慢速播放";
            this.toolSlowPlay1.Click += new System.EventHandler(this.toolSlowPlay1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(4, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(797, 591);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(439, 520);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 35);
            this.button2.TabIndex = 12;
            this.button2.Text = "回放测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(224, 524);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 23);
            this.dateTimePicker1.TabIndex = 13;
            // 
            // ISECNewVideoA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ISECNewVideoA";
            this.Size = new System.Drawing.Size(804, 660);
            this.Load += new System.EventHandler(this.ISECNewVideo_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbl0Msg;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolMenuQuickPlay0;
        private System.Windows.Forms.ToolStripMenuItem toolMenuPlay1;
        private System.Windows.Forms.ToolStripMenuItem toolMenuStopPlay2;
        private System.Windows.Forms.ToolStripMenuItem toolSlowPlay1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}
