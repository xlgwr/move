﻿namespace AnXinWH.ShiPin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbtop2 = new System.Windows.Forms.GroupBox();
            this.comb0CameraID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl0Msg = new System.Windows.Forms.Label();
            this.gb0head = new System.Windows.Forms.GroupBox();
            this.comb3StreamId = new System.Windows.Forms.ComboBox();
            this.btn0FileStream = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comb2Moves = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl2Name = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn5Down = new System.Windows.Forms.Button();
            this.btn3PlayFile = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.gb00Bottom = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbtop2.SuspendLayout();
            this.gb0head.SuspendLayout();
            this.gb00Bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "打开实时视频";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(920, 405);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // gbtop2
            // 
            this.gbtop2.Controls.Add(this.pictureBox1);
            this.gbtop2.Location = new System.Drawing.Point(4, 199);
            this.gbtop2.Name = "gbtop2";
            this.gbtop2.Size = new System.Drawing.Size(926, 425);
            this.gbtop2.TabIndex = 2;
            this.gbtop2.TabStop = false;
            // 
            // comb0CameraID
            // 
            this.comb0CameraID.FormattingEnabled = true;
            this.comb0CameraID.Location = new System.Drawing.Point(410, 23);
            this.comb0CameraID.Name = "comb0CameraID";
            this.comb0CameraID.Size = new System.Drawing.Size(158, 20);
            this.comb0CameraID.TabIndex = 3;
            this.comb0CameraID.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comb0CameraID.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(363, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "摄像头：";
            // 
            // lbl0Msg
            // 
            this.lbl0Msg.AutoSize = true;
            this.lbl0Msg.ForeColor = System.Drawing.Color.Red;
            this.lbl0Msg.Location = new System.Drawing.Point(25, 160);
            this.lbl0Msg.Name = "lbl0Msg";
            this.lbl0Msg.Size = new System.Drawing.Size(23, 12);
            this.lbl0Msg.TabIndex = 5;
            this.lbl0Msg.Text = "msg";
            // 
            // gb0head
            // 
            this.gb0head.Controls.Add(this.comb3StreamId);
            this.gb0head.Controls.Add(this.btn0FileStream);
            this.gb0head.Controls.Add(this.progressBar1);
            this.gb0head.Controls.Add(this.dateTimePicker2);
            this.gb0head.Controls.Add(this.dateTimePicker1);
            this.gb0head.Controls.Add(this.comb2Moves);
            this.gb0head.Controls.Add(this.label4);
            this.gb0head.Controls.Add(this.lbl2Name);
            this.gb0head.Controls.Add(this.label3);
            this.gb0head.Controls.Add(this.label2);
            this.gb0head.Controls.Add(this.lbl0Msg);
            this.gb0head.Controls.Add(this.btn5Down);
            this.gb0head.Controls.Add(this.btn3PlayFile);
            this.gb0head.Controls.Add(this.button2);
            this.gb0head.Controls.Add(this.button1);
            this.gb0head.Controls.Add(this.comb0CameraID);
            this.gb0head.Controls.Add(this.label1);
            this.gb0head.Location = new System.Drawing.Point(4, 2);
            this.gb0head.Name = "gb0head";
            this.gb0head.Size = new System.Drawing.Size(920, 191);
            this.gb0head.TabIndex = 6;
            this.gb0head.TabStop = false;
            // 
            // comb3StreamId
            // 
            this.comb3StreamId.FormattingEnabled = true;
            this.comb3StreamId.Location = new System.Drawing.Point(738, 135);
            this.comb3StreamId.Name = "comb3StreamId";
            this.comb3StreamId.Size = new System.Drawing.Size(127, 20);
            this.comb3StreamId.TabIndex = 11;
            // 
            // btn0FileStream
            // 
            this.btn0FileStream.Location = new System.Drawing.Point(738, 92);
            this.btn0FileStream.Name = "btn0FileStream";
            this.btn0FileStream.Size = new System.Drawing.Size(108, 34);
            this.btn0FileStream.TabIndex = 10;
            this.btn0FileStream.Text = "获取视频流";
            this.btn0FileStream.UseVisualStyleBackColor = true;
            this.btn0FileStream.Click += new System.EventHandler(this.btn0FileStream_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(25, 132);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(580, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(410, 61);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(158, 21);
            this.dateTimePicker2.TabIndex = 8;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(186, 61);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(158, 21);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // comb2Moves
            // 
            this.comb2Moves.FormattingEnabled = true;
            this.comb2Moves.Location = new System.Drawing.Point(410, 100);
            this.comb2Moves.Name = "comb2Moves";
            this.comb2Moves.Size = new System.Drawing.Size(195, 20);
            this.comb2Moves.TabIndex = 7;
            this.comb2Moves.SelectedIndexChanged += new System.EventHandler(this.comb2Moves_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(351, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "结束时间：";
            // 
            // lbl2Name
            // 
            this.lbl2Name.AutoSize = true;
            this.lbl2Name.Location = new System.Drawing.Point(574, 27);
            this.lbl2Name.Name = "lbl2Name";
            this.lbl2Name.Size = new System.Drawing.Size(41, 12);
            this.lbl2Name.TabIndex = 6;
            this.lbl2Name.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "开始时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(351, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "选择视频：";
            // 
            // btn5Down
            // 
            this.btn5Down.Location = new System.Drawing.Point(611, 126);
            this.btn5Down.Name = "btn5Down";
            this.btn5Down.Size = new System.Drawing.Size(108, 34);
            this.btn5Down.TabIndex = 0;
            this.btn5Down.Text = "下载视频";
            this.btn5Down.UseVisualStyleBackColor = true;
            this.btn5Down.Click += new System.EventHandler(this.btn5Down_Click);
            // 
            // btn3PlayFile
            // 
            this.btn3PlayFile.Location = new System.Drawing.Point(611, 93);
            this.btn3PlayFile.Name = "btn3PlayFile";
            this.btn3PlayFile.Size = new System.Drawing.Size(108, 34);
            this.btn3PlayFile.TabIndex = 0;
            this.btn3PlayFile.Text = "回放视频";
            this.btn3PlayFile.UseVisualStyleBackColor = true;
            this.btn3PlayFile.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(186, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 34);
            this.button2.TabIndex = 0;
            this.button2.Text = "查询视频";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(157, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 37);
            this.button4.TabIndex = 2;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(238, 18);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 37);
            this.button5.TabIndex = 3;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(324, 18);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 37);
            this.button6.TabIndex = 2;
            this.button6.Text = "button4";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(405, 18);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 37);
            this.button7.TabIndex = 3;
            this.button7.Text = "button5";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(484, 18);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 37);
            this.button8.TabIndex = 2;
            this.button8.Text = "button4";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(565, 18);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 37);
            this.button9.TabIndex = 3;
            this.button9.Text = "button5";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // gb00Bottom
            // 
            this.gb00Bottom.Controls.Add(this.button5);
            this.gb00Bottom.Controls.Add(this.button9);
            this.gb00Bottom.Controls.Add(this.button4);
            this.gb00Bottom.Controls.Add(this.button8);
            this.gb00Bottom.Controls.Add(this.button6);
            this.gb00Bottom.Controls.Add(this.button7);
            this.gb00Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb00Bottom.Location = new System.Drawing.Point(0, 630);
            this.gb00Bottom.Name = "gb00Bottom";
            this.gb00Bottom.Size = new System.Drawing.Size(942, 67);
            this.gb00Bottom.TabIndex = 4;
            this.gb00Bottom.TabStop = false;
            this.gb00Bottom.Text = "视频操作";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 697);
            this.Controls.Add(this.gb00Bottom);
            this.Controls.Add(this.gbtop2);
            this.Controls.Add(this.gb0head);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbtop2.ResumeLayout(false);
            this.gb0head.ResumeLayout(false);
            this.gb0head.PerformLayout();
            this.gb00Bottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gbtop2;
        private System.Windows.Forms.ComboBox comb0CameraID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl0Msg;
        private System.Windows.Forms.GroupBox gb0head;
        private System.Windows.Forms.Label lbl2Name;
        private System.Windows.Forms.ComboBox comb2Moves;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn3PlayFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox gb00Bottom;
        private System.Windows.Forms.Button btn5Down;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn0FileStream;
        private System.Windows.Forms.ComboBox comb3StreamId;
    }
}

