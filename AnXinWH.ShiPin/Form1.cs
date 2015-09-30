using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AnXinWH.ShiPin
{
    public partial class Form1 : Form
    {
        static List<ZXVNMS_Camera2> _list_tcamera;
        static string _userid;
        static string _cmsIP;
        static int _m_curPlayHandle;

        IntPtr _data;
        IntPtr _pUser;



        IntPtr _pUser0 = new IntPtr(0);
        public Form1()
        {
            InitializeComponent();
            try
            {
                //
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Resize += Form1_Resize;
                //initwith
                initwith();
                //init
                initfirst();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        void Form1_Resize(object sender, EventArgs e)
        {
            initwith();
            //throw new NotImplementedException();
        }
        //initform
        void initfirst()
        {
            //
            lbl2Name.Text = "";
            lbl0Msg.Text = "";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Text = DateTime.Now.AddHours(-7).ToString();

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            //attr
            _list_tcamera = new List<ZXVNMS_Camera2>();
            _userid = "";
            _cmsIP = "";
            _m_curPlayHandle = -1;
            _data = new IntPtr();
            _pUser = new IntPtr();
            //initdevice
            initDevice(comm.getConfigHost());
            //initcomp
            initcomp(comb0CameraID);

            this.Text = _userid + "," + _cmsIP;
        }
        void initwith()
        {
            gbtop1.Width = this.Width - gbtop1.Left - 20;

            gbtop2.Top = gbtop1.Top + gbtop1.Height;
            gbtop2.Left = gbtop1.Left;
            gbtop2.Width = gbtop1.Width;
            gbtop2.Height = this.Height - gb00Bottom.Height - gbtop2.Top - 35;

        }

        void StreamCallback(int handle, int dataType, ref IntPtr data, int size, ref IntPtr pUser)
        {

        }
        void initcomp(ComboBox cb)
        {
            cb.Items.Clear();
            foreach (var item in _list_tcamera)
            {
                cb.Items.Add(item.device_id);
            }
        }
        void initDevice(configHost tconfig)
        {
            try
            {
                var tmpinit = ZxvnmsSDKApi.ZXVNMS_Init();
                var tmpCallback = ZxvnmsSDKApi.ZXVNMS_SetVideoStreamCallback(StreamCallback, _pUser0);
                var tmpLogin = ZxvnmsSDKApi.ZXVNMS_InitSession(tconfig.cmsip,
                    tconfig.cmsPort,
                    tconfig.userName,
                    tconfig.pswd,
                    tconfig.ValidateType,
                    tconfig.UserMacAddr,
                    tconfig.UserUsbKey,
                    tconfig.Bound);
                _userid = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetUserID());
                _cmsIP = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetCMSIP());

                if (ZxvnmsSDKApi.ZXVNMS_QueryDevices(ZXVNMS_DevType.CAMERA) == 0)
                {
                    while (ZxvnmsSDKApi.ZXVNMS_MoveNext() != -1)
                    {
                        var tmpCamer = new ZXVNMS_Camera2();
                        tmpCamer.device_id = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_Camera.device_id));
                        tmpCamer.device_name = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_Camera.device_name));
                        tmpCamer.address = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_Camera.address));
                        tmpCamer.control_port = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_Camera.control_port));

                        _list_tcamera.Add(tmpCamer);
                    }
                }
                //MessageBox.Show(userid.ToString());
            }
            catch (Exception ex)
            {
                ZxvnmsSDKApi.ZXVNMS_Free();
                MessageBox.Show(ex.Message);
            }
        }
        bool OnPlayVideo(string cameraID, IntPtr handle)
        {
            try
            {
                if (_m_curPlayHandle >= 0)
                {
                    ZxvnmsSDKApi.ZXVNMS_StopVideo(_m_curPlayHandle);
                    _m_curPlayHandle = -1;
                }
                int vh = ZxvnmsSDKApi.ZXVNMS_PlayVideo(cameraID, handle);
                if (vh >= 0)
                {
                    _m_curPlayHandle = vh;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }

            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comb0CameraID.Text))
            {
                comb0CameraID.Focus();
                return;
            }
            button1.Enabled = false;
            string msg = "正在打开中。。。";
            SetMsg(lbl0Msg, msg);
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!string.IsNullOrEmpty(comb0CameraID.Text))
                {
                    if (OnPlayVideo(comb0CameraID.Text.Trim(), pictureBox1.Handle))
                    {
                        msg = "打开成功。。。";
                        SetMsg(lbl0Msg, msg);
                    }
                    else
                    {
                        msg = "打开失败。。。";
                        SetMsg(lbl0Msg, msg);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button1.Enabled = true;
                Cursor.Current = Cursors.Default;
            }



        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ZxvnmsSDKApi.ZXVNMS_Free();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comb0CameraID.SelectedItem != null)
            {

                setName(comb0CameraID, lbl2Name);
            }
        }
        void SetMsg(Label lbl, string msg)
        {
            this.Invoke(new Action(delegate()
            {
                lbl.Text = msg;
            }));
        }
        void setName(ComboBox cb, Label lbl)
        {
            SetMsg(lbl, "");
            if (!string.IsNullOrEmpty(cb.Text))
            {
                foreach (var item in _list_tcamera)
                {
                    if (item.device_id.Equals(cb.Text))
                    {
                        _device_id = item.device_id;
                        SetMsg(lbl, item.device_name);
                        return;
                    }
                }
                SetMsg(lbl, "");
            }

        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            setName(comb0CameraID, lbl2Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tmpmsg = "";
            try
            {
                if (string.IsNullOrEmpty(comb0CameraID.Text))
                {
                    comb0CameraID.Focus();
                    return;
                }
                var tmpList = new List<ZXVNMS_RecordFile2>();


                var tmpret = ZxvnmsSDKApi.ZXVNMS_QueryRecordFileEx(
                    comb0CameraID.Text,
                    dateTimePicker1.Text,
                    dateTimePicker2.Text,
                    0,
                    0
                    );
                if (tmpret != 0)
                {
                    IntPtr tmpErrmsg = new IntPtr();
                    IntPtr size = new IntPtr(100);
                    ZxvnmsSDKApi.ZXVNMS_GetErrorInfo(tmpret, tmpErrmsg, size);

                    tmpmsg = "Error:" + size + "," + tmpErrmsg;
                    SetMsg(lbl0Msg, tmpmsg);
                    MessageBox.Show(tmpmsg);
                    return;
                }

                int i = 0;
                while (ZxvnmsSDKApi.ZXVNMS_MoveNext() != -1)
                {
                    i++;

                    ZXVNMS_RecordFile2 tmprec = new ZXVNMS_RecordFile2();
                    tmprec.id = i;
                    tmprec.locale = "中心存储";


                    tmprec.filename = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_RecordFile.filename));
                    tmprec.size = (Convert.ToDouble(Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_RecordFile.size))) / (1024 * 1024)).ToString();

                    tmprec.starttime = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_RecordFile.starttime));
                    tmprec.endtime = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_RecordFile.endtime));


                    tmpList.Add(tmprec);

                }
                if (i > 0)
                {
                    foreach (var item in tmpList)
                    {
                        var tmpitem = item.id + "," + item.starttime + "," + item.endtime + "," + item.size + "," + item.filename;
                        comb2Moves.Items.Add(tmpitem);
                    }
                    comb2Moves.SelectedIndex = 0;
                }
                tmpmsg = "查询完成，共有:" + i + "条记录.";
                SetMsg(lbl0Msg, tmpmsg);

            }
            catch (Exception ex)
            {
                tmpmsg = ex.Message;
                SetMsg(lbl0Msg, tmpmsg);
                MessageBox.Show(tmpmsg);
            }


        }

        public string _device_id { get; set; }

        private void comb2Moves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comb2Moves.SelectedItem != null)
            {
                var tmpM = comb2Moves.Text.Split(',');

                var tmpmsg = "当前选择：" + tmpM[0] + ",开始时间：" + tmpM[1] + ",结束时间：" + tmpM[2] + ",大小：" + tmpM[3] + "M";
                _commSelectMove = tmpmsg;
                SetMsg(lbl0Msg, tmpmsg);
            }

        }


        public static IntPtr percent = Marshal.AllocHGlobal(8);
        public static IntPtr datarate = Marshal.AllocHGlobal(8);
        public static IntPtr remaintime = Marshal.AllocHGlobal(8);
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ZxvnmsSDKApi.ZXVNMS_GetFilePercent(_m_downloadHandle, percent);
                ZxvnmsSDKApi.ZXVNMS_GetVideoDataRate(_m_downloadHandle, datarate);
                ZxvnmsSDKApi.ZXVNMS_GetDownloadRemainTime(_m_downloadHandle, remaintime);


                var tmppercent = Marshal.ReadInt32(percent);
                var tmpdatarate = Marshal.ReadInt32(datarate);
                var tmpremaintime = Marshal.ReadInt32(remaintime);

                tmpmsg = "speed:" + tmpdatarate + " B/S,percent:" + tmppercent + "%,remaintime:" + tmpremaintime + "s" + "," + _commSelectMove;
                SetMsg(lbl0Msg, tmpmsg);
                var tmpValue = Convert.ToDouble(tmppercent);
                progressBar1.Value = Convert.ToInt32(Math.Ceiling(tmpValue));

                if (tmpValue >= 100)
                {
                    if (_m_downloadHandle >= 0)
                    {
                        ZxvnmsSDKApi.ZXVNMS_StopFileDownload(_m_downloadHandle);
                        _m_downloadHandle = -1;
                    }
                    timer1.Enabled = false;
                    btn5Down.Enabled = true;
                    tmpmsg = "下载完成。" + _fullfilename;
                    SetMsg(lbl0Msg, tmpmsg);
                }


            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                btn5Down.Enabled = true;
                tmpmsg = ex.Message;
                SetMsg(lbl0Msg, tmpmsg);
                MessageBox.Show(tmpmsg);
            }

        }
        private void btn5Down_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(comb2Moves.Text))
                {
                    comb2Moves.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(comb0CameraID.Text))
                {
                    comb0CameraID.Focus();
                    return;
                }
                if (_m_downloadHandle >= 0)
                {
                    ZxvnmsSDKApi.ZXVNMS_StopFileDownload(_m_downloadHandle);
                    _m_downloadHandle = -1;
                }
                var tmpM = comb2Moves.Text.Split(',');
                var filename = tmpM[4];
                var size = Convert.ToInt32(Convert.ToDouble(tmpM[3]) * 1024 * 1024);

                var tmpIndex = comb2Moves.Text.LastIndexOf('/');
                var fullfilename = comb2Moves.Text.Substring(tmpIndex + 1);

                SaveFileDialog tmpsavefiledialog = new SaveFileDialog();
                tmpsavefiledialog.FileName = fullfilename;
                tmpsavefiledialog.Filter = "zv Files(*.zv)|*.zv||";

                if (tmpsavefiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fullfilename = tmpsavefiledialog.FileName;

                    _fullfilename = fullfilename;

                    tmpmsg = "开始下载中。。。";
                    SetMsg(lbl0Msg, tmpmsg);

                    btn5Down.Enabled = false;

                    progressBar1.Visible = true;
                    progressBar1.Value = 0;
                    progressBar1.Maximum = 100;

                    var tmpcameraId = comb0CameraID.Text;
                    _m_downloadHandle = ZxvnmsSDKApi.ZXVNMS_DownloadFile(
                        tmpcameraId,
                        filename,
                        size,
                        0,
                        fullfilename
                        );

                    if (_m_downloadHandle < 0)
                    {
                        IntPtr tmpErrmsg = new IntPtr();
                        IntPtr buflen = new IntPtr(0);
                        ZxvnmsSDKApi.ZXVNMS_GetErrorInfo(_m_downloadHandle, tmpErrmsg, buflen);

                        tmpmsg = "Error:下载失败，" + _m_downloadHandle + "," + tmpErrmsg;
                        SetMsg(lbl0Msg, tmpmsg);
                        MessageBox.Show(tmpmsg);
                        btn5Down.Enabled = true;
                        return;
                    }
                    else
                    {
                        timer1.Enabled = true;
                    }
                }


            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                btn5Down.Enabled = true;


                progressBar1.Visible = false;
                progressBar1.Value = 0;
                progressBar1.Maximum = 100;

                tmpmsg = ex.Message;
                SetMsg(lbl0Msg, tmpmsg);
                MessageBox.Show(tmpmsg);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(comb2Moves.Text))
                {
                    comb2Moves.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(comb0CameraID.Text))
                {
                    comb0CameraID.Focus();
                    return;
                }
                btn3PlayFile.Enabled = false;

                var tmpM = comb2Moves.Text.Split(',');
                var filename = tmpM[4];
                var size = Convert.ToInt32(Convert.ToDouble(tmpM[3]) * 1024 * 1024);

                var comMsg = tmpM[0] + ",开始时间：" + tmpM[1] + ",结束时间：" + tmpM[2] + ",大小：" + tmpM[3] + "M";
                tmpmsg = "打开中：" + comMsg;


                SetMsg(lbl0Msg, tmpmsg);

                if (_m_curPlayHandle >= 0)
                {
                    ZxvnmsSDKApi.ZXVNMS_StopVideo(_m_curPlayHandle);
                    _m_curPlayHandle = -1;
                }

                if (_m_playfileHandle >= 0)
                {
                    ZxvnmsSDKApi.ZXVNMS_StopFilePlay(_m_playfileHandle);
                    _m_playfileHandle = -1;
                }


                _m_playfileHandle = ZxvnmsSDKApi.ZXVNMS_PlayFile(
                    comb0CameraID.Text,
                    filename,
                    size,
                    0,
                    pictureBox1.Handle
                    );

                if (_m_playfileHandle >= 0)
                {
                    tmpmsg = "打开成功：" + comMsg;
                }
                else
                {
                    tmpmsg = "打开失败：" + comMsg;
                }

                SetMsg(lbl0Msg, tmpmsg);
            }
            catch (Exception ex)
            {
                tmpmsg = ex.Message;
                SetMsg(lbl0Msg, tmpmsg);
                MessageBox.Show(tmpmsg);
            }
            finally
            {
                btn3PlayFile.Enabled = true;
            }
        }

        public string tmpmsg { get; set; }

        public int _m_playfileHandle { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            gb00Bottom.Visible = false;

            comb0CameraID.SelectedIndex = 0;

            progressBar1.Visible = false;
            progressBar1.Value = 0;
            progressBar1.Maximum = 100;
        }




        public int _m_downloadHandle { get; set; }


        public string _commSelectMove { get; set; }

        public string _fullfilename { get; set; }
    }
}
