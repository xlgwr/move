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
            this.btnTest = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolMenuPlay1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuStopPlay2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuQuickPlay0 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSlowPlay1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn0StockIn = new System.Windows.Forms.Button();
            this.btn1Shelf = new System.Windows.Forms.Button();
            this.btn2StockOut = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl0Msg);
            this.groupBox2.Controls.Add(this.btnTest);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Location = new System.Drawing.Point(4, -4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(905, 52);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // lbl0Msg
            // 
            this.lbl0Msg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl0Msg.ForeColor = System.Drawing.Color.Red;
            this.lbl0Msg.Location = new System.Drawing.Point(4, 12);
            this.lbl0Msg.Name = "lbl0Msg";
            this.lbl0Msg.Size = new System.Drawing.Size(898, 36);
            this.lbl0Msg.TabIndex = 21;
            this.lbl0Msg.Text = "msg";
            this.lbl0Msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(778, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(121, 36);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "回放测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.button2_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(778, 54);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(124, 21);
            this.dateTimePicker1.TabIndex = 5;
            this.dateTimePicker1.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(724, 517);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
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
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point(179, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(730, 547);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn0StockIn
            // 
            this.btn0StockIn.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn0StockIn.Location = new System.Drawing.Point(20, 60);
            this.btn0StockIn.Name = "btn0StockIn";
            this.btn0StockIn.Size = new System.Drawing.Size(121, 48);
            this.btn0StockIn.TabIndex = 0;
            this.btn0StockIn.Text = "入库";
            this.btn0StockIn.UseVisualStyleBackColor = true;
            this.btn0StockIn.Click += new System.EventHandler(this.btn0StockIn_Click);
            // 
            // btn1Shelf
            // 
            this.btn1Shelf.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn1Shelf.Location = new System.Drawing.Point(20, 114);
            this.btn1Shelf.Name = "btn1Shelf";
            this.btn1Shelf.Size = new System.Drawing.Size(121, 48);
            this.btn1Shelf.TabIndex = 2;
            this.btn1Shelf.Text = "上架";
            this.btn1Shelf.UseVisualStyleBackColor = true;
            this.btn1Shelf.Click += new System.EventHandler(this.btn1Shelf_Click);
            // 
            // btn2StockOut
            // 
            this.btn2StockOut.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn2StockOut.Location = new System.Drawing.Point(20, 168);
            this.btn2StockOut.Name = "btn2StockOut";
            this.btn2StockOut.Size = new System.Drawing.Size(121, 48);
            this.btn2StockOut.TabIndex = 3;
            this.btn2StockOut.Text = "出库";
            this.btn2StockOut.UseVisualStyleBackColor = true;
            this.btn2StockOut.Click += new System.EventHandler(this.btn2StockOut_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 254);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(173, 340);
            this.listBox1.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(3, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 21);
            this.label1.TabIndex = 22;
            this.label1.Text = "报警";
            // 
            // ISECNewVideoA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btn2StockOut);
            this.Controls.Add(this.btn1Shelf);
            this.Controls.Add(this.btn0StockIn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ISECNewVideoA";
            this.Size = new System.Drawing.Size(912, 620);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btn0StockIn;
        private System.Windows.Forms.Button btn1Shelf;
        private System.Windows.Forms.Button btn2StockOut;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
    }
}
