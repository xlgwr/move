namespace AnXinWH.ShiPinTianDy
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.com1Ip = new System.Windows.Forms.ComboBox();
            this.btn1Logon = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt1ID = new System.Windows.Forms.TextBox();
            this.btn2Connect = new System.Windows.Forms.Button();
            this.btn3Play = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_video = new System.Windows.Forms.Panel();
            this.cboScreen = new System.Windows.Forms.ComboBox();
            this.cboStream = new System.Windows.Forms.ComboBox();
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.cboChannel = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // com1Ip
            // 
            this.com1Ip.FormattingEnabled = true;
            this.com1Ip.Items.AddRange(new object[] {
            "192.168.1.110"});
            this.com1Ip.Location = new System.Drawing.Point(43, 22);
            this.com1Ip.Name = "com1Ip";
            this.com1Ip.Size = new System.Drawing.Size(153, 20);
            this.com1Ip.TabIndex = 0;
            this.com1Ip.Text = "192.168.1.110";
            // 
            // btn1Logon
            // 
            this.btn1Logon.Location = new System.Drawing.Point(202, 21);
            this.btn1Logon.Name = "btn1Logon";
            this.btn1Logon.Size = new System.Drawing.Size(75, 23);
            this.btn1Logon.TabIndex = 2;
            this.btn1Logon.Text = "Logon";
            this.btn1Logon.UseVisualStyleBackColor = true;
            this.btn1Logon.Click += new System.EventHandler(this.btn1Logon_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP:";
            // 
            // txt1ID
            // 
            this.txt1ID.Location = new System.Drawing.Point(43, 49);
            this.txt1ID.Name = "txt1ID";
            this.txt1ID.ReadOnly = true;
            this.txt1ID.Size = new System.Drawing.Size(153, 21);
            this.txt1ID.TabIndex = 1;
            // 
            // btn2Connect
            // 
            this.btn2Connect.Location = new System.Drawing.Point(202, 47);
            this.btn2Connect.Name = "btn2Connect";
            this.btn2Connect.Size = new System.Drawing.Size(75, 23);
            this.btn2Connect.TabIndex = 2;
            this.btn2Connect.Text = "Connect";
            this.btn2Connect.UseVisualStyleBackColor = true;
            this.btn2Connect.Click += new System.EventHandler(this.btn2Connect_Click);
            // 
            // btn3Play
            // 
            this.btn3Play.Location = new System.Drawing.Point(202, 76);
            this.btn3Play.Name = "btn3Play";
            this.btn3Play.Size = new System.Drawing.Size(75, 23);
            this.btn3Play.TabIndex = 2;
            this.btn3Play.Text = "Play";
            this.btn3Play.UseVisualStyleBackColor = true;
            this.btn3Play.Click += new System.EventHandler(this.btn3Play_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "ID:";
            // 
            // m_video
            // 
            this.m_video.Location = new System.Drawing.Point(12, 158);
            this.m_video.Name = "m_video";
            this.m_video.Size = new System.Drawing.Size(537, 335);
            this.m_video.TabIndex = 4;
            // 
            // cboScreen
            // 
            this.cboScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScreen.FormattingEnabled = true;
            this.cboScreen.Items.AddRange(new object[] {
            "S1",
            "S4",
            "S9",
            "S16"});
            this.cboScreen.Location = new System.Drawing.Point(53, 105);
            this.cboScreen.Name = "cboScreen";
            this.cboScreen.Size = new System.Drawing.Size(67, 20);
            this.cboScreen.TabIndex = 19;
            // 
            // cboStream
            // 
            this.cboStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStream.FormattingEnabled = true;
            this.cboStream.Items.AddRange(new object[] {
            "MAIN",
            "SUB"});
            this.cboStream.Location = new System.Drawing.Point(126, 105);
            this.cboStream.Name = "cboStream";
            this.cboStream.Size = new System.Drawing.Size(68, 20);
            this.cboStream.TabIndex = 18;
            // 
            // cboMode
            // 
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.FormattingEnabled = true;
            this.cboMode.Items.AddRange(new object[] {
            "TCP",
            "UDP",
            "MULTICAST"});
            this.cboMode.Location = new System.Drawing.Point(126, 76);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(68, 20);
            this.cboMode.TabIndex = 17;
            // 
            // cboChannel
            // 
            this.cboChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChannel.FormattingEnabled = true;
            this.cboChannel.Items.AddRange(new object[] {
            "CH1",
            "CH2",
            "CH3",
            "CH4",
            "CH5",
            "CH6",
            "CH7",
            "CH8",
            "CH9",
            "CH10",
            "CH11",
            "CH12",
            "CH13",
            "CH14",
            "CH15",
            "CH16"});
            this.cboChannel.Location = new System.Drawing.Point(53, 76);
            this.cboChannel.Name = "cboChannel";
            this.cboChannel.Size = new System.Drawing.Size(67, 20);
            this.cboChannel.TabIndex = 16;
            this.cboChannel.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.com1Ip);
            this.groupBox1.Controls.Add(this.cboScreen);
            this.groupBox1.Controls.Add(this.txt1ID);
            this.groupBox1.Controls.Add(this.cboStream);
            this.groupBox1.Controls.Add(this.btn1Logon);
            this.groupBox1.Controls.Add(this.cboMode);
            this.groupBox1.Controls.Add(this.btn2Connect);
            this.groupBox1.Controls.Add(this.cboChannel);
            this.groupBox1.Controls.Add(this.btn3Play);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(537, 140);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 524);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_video);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox com1Ip;
        private System.Windows.Forms.Button btn1Logon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt1ID;
        private System.Windows.Forms.Button btn2Connect;
        private System.Windows.Forms.Button btn3Play;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel m_video;
        private System.Windows.Forms.ComboBox cboScreen;
        private System.Windows.Forms.ComboBox cboStream;
        private System.Windows.Forms.ComboBox cboMode;
        private System.Windows.Forms.ComboBox cboChannel;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

