﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using AnXinWH.ShiPin;

namespace AnXinWH.ShiPinZXVOCX
{
    [Guid("63C42A64-B9A6-4583-B6D0-10DC639D5408")]
    public partial class ISECVideoZXV : UserControl, IObjectSafety, IDisposable
    {
        #region attr
        static List<ZXVNMS_Camera2> _list_tcamera;
        static string _userid;
        static string _cmsIP;
        public int _m_curPlayHandle { get; private set; }
        public int _m_downloadHandle { get; private set; }
        public string _commSelectMove { get; private set; }
        public int _m_playfileHandle { get; private set; }
        public int _fileStreamHandle { get; set; }
        public string _device_id { get; set; }
        public string _fullfilename { get; private set; }

        public static IntPtr _data = Marshal.AllocHGlobal(1024);
        public static IntPtr _pUser = Marshal.AllocHGlobal(1024);

        public static IntPtr percent = Marshal.AllocHGlobal(8);
        public static IntPtr datarate = Marshal.AllocHGlobal(8);
        public static IntPtr remaintime = Marshal.AllocHGlobal(8);

        IntPtr _pUser0 = new IntPtr(0);
        #endregion
        public ISECVideoZXV()
        {
            InitializeComponent();
            try
            {
                this.Disposed += ISECVideoZXV_Disposed;

                //init

                initfirst();

                //init set
                comb0CameraID.SelectedIndex = 0;
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                progressBar1.Maximum = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            trackBar1.Visible = false;
            //attr
            _list_tcamera = new List<ZXVNMS_Camera2>();
            _userid = "";
            _cmsIP = "";
            _m_curPlayHandle = -1;
            _m_playfileHandle = -1;
            _m_downloadHandle = -1;
            //initdevice
            initDevice(comm.getConfigHost());
            //initcomp
            initcomp(comb0CameraID);

            this.Text = _userid + "," + _cmsIP;
        }
        void initDevice(configHost tconfig)
        {
            try
            {
                var tmpinit = ZxvnmsSDKApi.ZXVNMS_Init();
                //var tmpCallback = ZxvnmsSDKApi.ZXVNMS_SetVideoStreamCallback(StreamCallback, _pUser0);
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

                checkStop(false);

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

        void initcomp(ComboBox cb)
        {
            cb.Items.Clear();
            foreach (var item in _list_tcamera)
            {
                cb.Items.Add(item.device_id);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comb0CameraID.Text))
            {
                comb0CameraID.Focus();
                return;
            }
            button1.Enabled = false;
            string msg = "正在打开实时视频中。。。";
            SetMsg(lbl0Msg, msg);
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!string.IsNullOrEmpty(comb0CameraID.Text))
                {
                    if (OnPlayVideo(comb0CameraID.Text.Trim(), pictureBox1.Handle))
                    {
                        msg = "打开实时视频成功。。。";
                        SetMsg(lbl0Msg, msg);
                    }
                    else
                    {
                        msg = "打开实时视频失败。。。";
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


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comb0CameraID.SelectedItem != null)
            {

                setName(comb0CameraID, lbl2Name);
            }
        }
        void SetMsg(Label lbl, string msg)
        {
            lbl.Text = msg;
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
            try
            {
                if (string.IsNullOrEmpty(comb0CameraID.Text))
                {
                    comb0CameraID.Focus();
                    tmpmsg = "Error:请选择摄像头。";
                    SetMsg(lbl0Msg, tmpmsg);
                    return;
                }
                if (dateTimePicker2.Value > DateTime.Now)
                {
                    dateTimePicker2.Focus();
                    tmpmsg = "Error:结束时间大于当前时间。" + DateTime.Now;
                    SetMsg(lbl0Msg, tmpmsg);
                    return;
                }
                if (dateTimePicker1.Value > dateTimePicker2.Value)
                {
                    dateTimePicker1.Focus();
                    tmpmsg = "Error:开始时间大于结束时间。";
                    SetMsg(lbl0Msg, tmpmsg);
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
                    IntPtr tmpErrmsg = Marshal.AllocHGlobal(1024);
                    IntPtr size = Marshal.AllocHGlobal(1024);
                    ZxvnmsSDKApi.ZXVNMS_GetErrorInfo(tmpret, tmpErrmsg, size);

                    var tmptmpErrmsg = Marshal.PtrToStringAnsi(tmpErrmsg);
                    var tmpsize = Marshal.ReadInt32(size);
                    tmpmsg = "Error:" + tmpsize + "," + tmptmpErrmsg;
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
                    comb2Moves.Items.Clear();
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
                MessageBox.Show(tmpmsg);
                SetMsg(lbl0Msg, tmpmsg);
            }


        }

        private void comb2Moves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comb2Moves.SelectedItem != null)
            {
                var tmpM = comb2Moves.Text.Split(',');

                var tmpmsg = "当前选择：" + tmpM[0] + ",时间：" + tmpM[1] + "-->" + tmpM[2] + ",视频大小：" + tmpM[3] + "M";
                _commSelectMove = tmpmsg;
                SetMsg(lbl0Msg, tmpmsg);
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

                progressBar1.Visible = false;
                progressBar1.Value = 0;
                progressBar1.Maximum = 100;

                var tmpM = comb2Moves.Text.Split(',');
                var filename = tmpM[4];
                var size = Convert.ToInt32(Convert.ToDouble(tmpM[3]) * 1024 * 1024);

                _comMsg = tmpM[0] + ",时间：" + tmpM[1] + "-->" + tmpM[2] + ",视频大小：" + tmpM[3] + "M";
                tmpmsg = "打开回放视频中：" + _comMsg;


                SetMsg(lbl0Msg, tmpmsg);

                checkStop(false);

                _m_playfileHandle = ZxvnmsSDKApi.ZXVNMS_PlayFile(
                    comb0CameraID.Text,
                    filename,
                    size,
                    0,
                    pictureBox1.Handle
                    );

                trackBar1.Value = 0;
                if (_m_playfileHandle >= 0)
                {
                    tmpmsg = "打开回放视频成功：" + _comMsg;
                    trackBar1.Visible = true;
                    timer2.Enabled = true;

                }
                else
                {
                    tmpmsg = "打开回放视频失败：" + _comMsg;
                    trackBar1.Visible = false;
                    timer2.Enabled = false;
                }

                SetMsg(lbl0Msg, tmpmsg);
            }
            catch (Exception ex)
            {
                tmpmsg = ex.Message;
                trackBar1.Value = 0;
                timer2.Enabled = false;

                SetMsg(lbl0Msg, tmpmsg);
                MessageBox.Show(tmpmsg);
            }
            finally
            {
                btn3PlayFile.Enabled = true;
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

            getPercent(_m_downloadHandle, "下载完成。", true);
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            getPercent(_m_playfileHandle, "播放完成。", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_handid"></param>
        /// <param name="msg"></param>
        /// <param name="isdown"></param>
        void getPercent(int _handid, string msg, bool isdown)
        {
            try
            {
                ZxvnmsSDKApi.ZXVNMS_GetFilePercent(_handid, percent);
                ZxvnmsSDKApi.ZXVNMS_GetVideoDataRate(_handid, datarate);

                var tmppercent = Marshal.ReadInt32(percent);
                var tmpdatarate = Marshal.ReadInt32(datarate) / 128;

                var tmpValue = Convert.ToDouble(tmppercent);
                var setTmpValue = Convert.ToInt32(Math.Ceiling(tmpValue));

                if (tmpValue >= 100)
                {
                    if (isdown)
                    {
                        if (_handid >= 0)
                        {
                            ZxvnmsSDKApi.ZXVNMS_StopFileDownload(_handid);
                            _m_downloadHandle = -1;

                        }
                        timer1.Enabled = false;
                        btn5Down.Enabled = true;
                        progressBar1.Value = 100;
                        tmpmsg = msg + _fullfilename;
                    }
                    else
                    {
                        if (_handid >= 0)
                        {
                            ZxvnmsSDKApi.ZXVNMS_StopFilePlay(_handid);
                            _m_playfileHandle = -1;
                        }
                        timer2.Enabled = false;
                        trackBar1.Visible = false;
                        trackBar1.Value = 0;
                        tmpmsg = msg + _comMsg;
                    }


                    SetMsg(lbl0Msg, tmpmsg);
                }
                else
                {
                    if (isdown)
                    {
                        ZxvnmsSDKApi.ZXVNMS_GetDownloadRemainTime(_handid, remaintime);

                        var tmpremaintime = Marshal.ReadInt32(remaintime);

                        tmpmsg = "speed:" + tmpdatarate + " KB/S,percent:" + tmppercent + "%,remaintime:" + tmpremaintime + "s" + "," + _commSelectMove;
                        progressBar1.Value = setTmpValue;
                    }
                    else
                    {
                        tmpmsg = "已播放:" + tmppercent + "%, speed:" + tmpdatarate + " KB/S，" + _comMsg;

                        trackBar1.Value = setTmpValue;
                    }

                    SetMsg(lbl0Msg, tmpmsg);
                }


            }
            catch (Exception ex)
            {
                if (isdown)
                {
                    timer1.Enabled = false;
                    btn5Down.Enabled = true;
                }
                {
                    timer2.Enabled = false;
                }

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
                checkStop(true);

                var tmpM = comb2Moves.Text.Split(',');
                var filename = tmpM[4];
                var size = Convert.ToInt32(Convert.ToDouble(tmpM[3]) * 1024 * 1024);

                var tmpIndex = comb2Moves.Text.LastIndexOf('/');
                var fullfilename = comb2Moves.Text.Substring(tmpIndex + 1);

                SaveFileDialog tmpsavefiledialog = new SaveFileDialog();
                tmpsavefiledialog.FileName = fullfilename;
                tmpsavefiledialog.Filter = "zv Files(*.zv)|*.zv|Other|*.*";

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
                        IntPtr tmpErrmsg = Marshal.AllocHGlobal(1024);
                        IntPtr size2 = Marshal.AllocHGlobal(1024);
                        ZxvnmsSDKApi.ZXVNMS_GetErrorInfo(_m_downloadHandle, tmpErrmsg, size2);

                        var tmptmpErrmsg = Marshal.PtrToStringAnsi(tmpErrmsg);
                        var tmpsize = Marshal.ReadInt32(size2);
                        tmpmsg = "Error:" + tmpsize + "," + tmptmpErrmsg;
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

        void checkStop(bool isDown)
        {

            if (_m_curPlayHandle >= 0)
            {
                ZxvnmsSDKApi.ZXVNMS_StopVideo(_m_curPlayHandle);
                _m_curPlayHandle = -1;
            }

            if (_m_playfileHandle >= 0)
            {
                ZxvnmsSDKApi.ZXVNMS_StopFilePlay(_m_playfileHandle);
                _m_playfileHandle = -1;
                timer2.Enabled = false;
            }
            if (isDown)
            {
                if (_m_downloadHandle >= 0)
                {
                    ZxvnmsSDKApi.ZXVNMS_StopFileDownload(_m_downloadHandle);
                    _m_downloadHandle = -1;
                    timer1.Enabled = false;
                }
            }

        }
        void ISECVideoZXV_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            checkStop(true);
            ZxvnmsSDKApi.ZXVNMS_Free();
        }
        #region IObjectSafety 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;


        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
                            (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
                            (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion


        public string tmpmsg { get; set; }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (_m_playfileHandle > 0)
                {
                    if (trackBar1.Value == 100)
                    {
                        SetMsg(lbl0Msg, "Notice:已播放完毕." + _comMsg);
                        return;
                    }
                    var tmpLayout = ZxvnmsSDKApi.ZXVNMS_SetFilePlayOffset(_m_playfileHandle, trackBar1.Value);
                    SetMsg(lbl0Msg, "从" + trackBar1.Value + "%开始播放，" + _comMsg);

                }
                else
                {
                    SetMsg(lbl0Msg, "Error:视频未开始回放。" + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        public string _comMsg { get; set; }


    }
}
