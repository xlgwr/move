using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;

namespace AnXinWH.ShiPinNewVideoOCX
{

    [Guid("8C8EC5F2-3C4E-48E1-A389-F5ECCB1B8178")]
    public partial class ISECNewVideoA : UserControl, IObjectSafety, IDisposable
    {
        #region att
        IntPtr hPreView = IntPtr.Zero;
        IntPtr hLogin = IntPtr.Zero;
        IntPtr _currPlayfile = IntPtr.Zero;
        IntPtr _hfile = IntPtr.Zero;

        public static configHost _getConfigHost = comm.getConfigHostN();

        static List<lisVideo> _lisIn { get; set; }

        public string ReceiveNo { get; private set; }

        static DateTime _InstartTime = DateTime.Now;
        static DateTime _InendTime = DateTime.Now;


        static DateTime _OutstartTime = DateTime.Now;
        static DateTime _OutendTime = DateTime.Now;


        static DateTime _ShelfstartTime = DateTime.Now;
        static DateTime _ShelfendTime = DateTime.Now;


        //collback
        public string tmpmsg { get; set; }

        TMCC.StreamCallback streamback = null;
        TMCC.AvFrameCallback frameback = null;
        int iTemp = 0;

        #endregion
        public ISECNewVideoA()
        {
            InitializeComponent();

            try
            {
                this.Disposed += ISECNewVideo_Disposed;
                initForm();
                initVideo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #region function
        private void initForm()
        {
            //throw new NotImplementedException();
            lbl0Msg.Text = "";
            _isScroll = false;

            _InstartTime = DateTime.Now.AddHours(-1);
            _InendTime = DateTime.Now;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Value = DateTime.Now.AddHours(-3);

        }

        private void initVideo()
        {
            //throw new NotImplementedException();
            hPreView = TMCC.TMCC_Init(5); //实时流句柄
            hLogin = TMCC.TMCC_Init(0); //控制连接句柄

            //设置超时值为秒
            TMCC.TMCC_SetTimeOut(hLogin, 3000);

            //设置断开重连
            TMCC.TMCC_SetAutoReConnect(hLogin, true);

            login();
        }
        void login()
        {
            TMCC.tmConnectInfo_t Info = new TMCC.tmConnectInfo_t();
            Info.Init();
            Info.dwSize = (UInt32)Marshal.SizeOf(Info);
            Info.pIp = Get(32, _getConfigHost.cmsip.ToCharArray());//txtIP.Text"192.168.1.4"
            Info.szPass = Get(32, _getConfigHost.pswd.ToCharArray());// "system"
            Info.szUser = Get(32, _getConfigHost.userName.ToCharArray()); //"system"
            Info.iPort = _getConfigHost.cmsPort;//"6002"txPort.Text
            Info.iUserLevel = 0;

            var ret = TMCC.TMCC_Connect(hLogin, ref Info, false);
            if (ret != TMCC.TMCC_ERR_SUCCESS)
            {
                TMCC.TMCC_DisConnect(hLogin);
                MessageBox.Show("登陆设备失败");
            }
            else
            {
                TMCC.TMCC_Connect(hLogin, ref Info, true);
            }
        }
        void playOldFileListName(lisVideo tmpvideo, string notice, DateTime start, DateTime endtime, byte byChannel)
        {
            try
            {
                closeAll();

                var anotherP = tmpvideo.videoCfg.fileCfg;

                TMCC.tmPlayConditionCfg_t struCond = new TMCC.tmPlayConditionCfg_t();

                struCond.dwSize = (UInt32)Marshal.SizeOf(struCond);

                struCond.byChannel = byChannel; //_getConfigHost.byChannel;

                struCond.struStartTime = anotherP.struStartTime;
                struCond.struStopTime = anotherP.struStopTime;

                struCond.byBufferBeforePlay = 1;
                struCond.dwBufferSizeBeforePlay = 1024 * 1024 * 20;

                DateTime tmpVideoStart = new DateTime(struCond.struStartTime.wYear, struCond.struStartTime.byMonth, struCond.struStartTime.byDay,
                                               struCond.struStartTime.byHour, struCond.struStartTime.byMinute, struCond.struStartTime.bySecond,
                                               (Int32)struCond.struStartTime.dwMicroSecond);
                DateTime tmpVideoEnd = new DateTime(struCond.struStopTime.wYear, struCond.struStopTime.byMonth, struCond.struStopTime.byDay,
                                           struCond.struStopTime.byHour, struCond.struStopTime.byMinute, struCond.struStopTime.bySecond,
                                           (Int32)struCond.struStopTime.dwMicroSecond);

                IntPtr p4 = Marshal.AllocHGlobal(Marshal.SizeOf(struCond));
                Marshal.StructureToPtr(struCond, p4, false);

                IntPtr p6 = TMCC.TMCC_OpenFile(hLogin, p4, this.pictureBox1.Handle);

                _currPlayfile = p6;
                _notice = notice;
                _videoname = tmpvideo.nameVideo;
                _currPlayfileConfigList = anotherP;

                m_iPlaySpeed = 0;
                var iflag = TMCC.Avdec_PlayToDo(p6, TMCC.PLAY_CONTROL_PLAY, 0);

                if (iflag == 0)
                {
                    var diffsecStart = start - tmpVideoStart;
                    var stopDiffMill = endtime - tmpVideoStart;
                    var diffsecEnd = endtime - tmpVideoEnd;
                    _currPlayfilep = true;

                    var getFileStatus = TMCC.Avdec_GetTmPlayStateCfg_t(_currPlayfile);
                    _trackBar1alltime = Convert.ToInt32(getFileStatus.dwTotalTimes / 1000);
                    _trackBar1currtime = Convert.ToInt32(getFileStatus.dwCurrentTimes / 1000);
                    _stopDiffMill = Convert.ToInt32(stopDiffMill.TotalMilliseconds / 1000);

                    if (diffsecStart.TotalMilliseconds > 0)
                    {
                        if (diffsecEnd.TotalMilliseconds > 0)
                        {
                            _playNext = true;
                        }
                        playOnMini(diffsecStart.TotalMilliseconds);
                    }

                    //trackBar1.Maximum = 0;
                    //trackBar1.Value = 0;
                    timer1.Enabled = true;

                    _currMsg = notice + "\n【" + tmpvideo.nameVideo + "】播放成功.";//,时间
                }
                else
                {
                    _currMsg = notice + "\n【" + tmpvideo.nameVideo + "】播放失败,请再次尝试.谢谢.";

                }
                lbl0Msg.Text = _currMsg;

            }
            catch (Exception ex)
            {
                _currPlayfilep = false;
                MessageBox.Show(notice + ",Play Error:" + ex.Message);
                //throw ex;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_isScroll)
            {
                return;
            }
            if (_currPlayfilep)
            {
                var getFileStatus = TMCC.Avdec_GetTmPlayStateCfg_t(_currPlayfile);
                _trackBar1alltime = Convert.ToInt32(getFileStatus.dwTotalTimes / 1000);
                _trackBar1currtime = Convert.ToInt32(getFileStatus.dwCurrentTimes / 1000);

                var st = _currPlayfileConfigList.struStartTime;
                var startTime = st.wYear.ToString() + "-" + st.byMonth.ToString() + "-" + st.byDay + " " + st.byHour + ":" + st.byMinute + ":" + st.bySecond;

                if (_trackBar1alltime > 0)
                {
                    if (_trackBar1currtime >= _trackBar1alltime)
                    {
                        timer1.Enabled = false;
                        _currPlayfilep = false;
                        TMCC.TMCC_CloseFile(_currPlayfile);
                        lbl0Msg.Text = _currMsg + ",已播放完成.";
                        return;
                    }
                    else
                    {
                        if (_trackBar1currtime >= _stopDiffMill)
                        {
                            timer1.Enabled = false;
                            _currPlayfilep = false;
                            TMCC.TMCC_CloseFile(_currPlayfile);
                            lbl0Msg.Text = _currMsg + ",已播放完成.";
                            return;
                        }
                    }
                    lbl0Msg.Text = _currMsg;// +"开始时间:" + startTime.ToString() + ",总时间:" + _trackBar1alltime + " 秒,\n 已播放:" + _trackBar1currtime + " 秒";
                }
                else
                {
                    lbl0Msg.Text = _currMsg;
                }
            }
            else
            {
                timer1.Enabled = false;
            }
        }
        void closeAll()
        {
            try
            {

                if (hPreView != null)
                {
                    TMCC.TMCC_CloseStream(hPreView);
                    TMCC.TMCC_ClearDisplay(hPreView);
                }

                //close
                if (_currPlayfile != null)
                {
                    TMCC.TMCC_CloseFile(_currPlayfile);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }


        }
        char[] Get(int size, char[] arr)
        {
            char[] total = new char[size];
            Array.Copy(arr, total, arr.Length);
            return total;
        }
        void SetMsg(Label lbl, string msg)
        {
            lbl.Text = msg;
        }
        public void StreamDataCallBack(IntPtr hTmCC, ref TMCC.tmRealStreamInfo_t pStreamInfo, IntPtr context)
        {
            if (pStreamInfo.byFrameType == 0)//视频
            {
                iTemp++;
                String str = "[" + Convert.ToString(pStreamInfo.iSamplesPerSec) + "x" + Convert.ToString(pStreamInfo.iBitsPerSample) + "] ";
                var strTitle = "ISEC Video " + str;
                for (int i = 0; i < iTemp; i++)
                {
                    strTitle += ">";
                }

                if (iTemp == 22)
                {
                    iTemp = 0;
                }
            }
        }

        public void AvFrameCallBack(IntPtr hTmCC, ref TMCC.tmAvImageInfo_t pFrame, IntPtr context)
        {
            if (pFrame.video == 1)//视频
            {
                // string str = "解码输出:" + Convert.ToString(pFrame.bufsize0);
                // tip(str);
            }
        }
        public int fnStreamReadCallBackDelegate_1(IntPtr hTmCC, IntPtr dwResult, IntPtr context)
        {

            return 1;
        }

        #endregion


        #region for js todo
        public void SetReceiptNo(object o)
        {
            if (o != null)
            {
                ReceiveNo = o.ToString();
                groupBox1.Text = ReceiveNo;
            }
        }
        public string jsGetVersion()
        {
            try
            {
                AssemblyName name = Assembly.GetExecutingAssembly().GetName();
                return name.Version.ToString();

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string jsSetTimeStockIn(object start, object seekSeconds, object bychange)
        {
            var tmpoff = 10;
            byte tmpbyChangel = 0;
            try
            {
                if (start != null)
                {
                    DateTime tmpStart = DateTime.Now.AddDays(-1);
                    if (DateTime.TryParse(start.ToString(), out tmpStart))
                    {
                        var tmpoff_flag = Int32.TryParse(seekSeconds.ToString(), out tmpoff);

                        _stockInSeekSeconds = tmpoff;
                        _InstartTime = tmpStart.AddSeconds(-tmpoff);
                        _InendTime = tmpStart.AddSeconds(tmpoff);
                    }
                    else
                    {
                        return "入库视频 开始时间有误：" + start.ToString();
                    }

                    var notice = "入库视频:" + _InstartTime.ToString("yyyy-MM-dd HH:mm:ss") + "----> " + _InendTime.ToString("yyyy-MM-dd HH:mm:ss"); ;

                    byte.TryParse(bychange.ToString(), out tmpbyChangel);

                    _lisIn = getListMoveFromStartAnd(_InstartTime, _InendTime, "入库视频", tmpbyChangel);

                    if (_lisIn.Count > 0)
                    {
                        var tmpname = _lisIn[0];
                        lbl0Msg.Text = notice + ", 回放视频中." + tmpname;
                        playOldFileListName(tmpname, notice, _InstartTime, _InendTime, tmpbyChangel);
                    }
                    else
                    {
                        lbl0Msg.Text = notice + ", 无历史记录." ;
                    }

                    return "ok";
                }
                return "Error.提供的数据有误。";

            }
            catch (Exception ex)
            {
                return "jsError:" + ex.Message;
            }

        }
        #endregion
        void ISECNewVideo_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            closeAll();
            //TMCC.TMCC_Done(hPreView);
            //TMCC.TMCC_Done(hLogin);     
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

        private List<lisVideo> getListMoveFromStartAnd(DateTime startTime, DateTime endTime, string notice, byte byChannel)
        {
            var tmpdic = new List<lisVideo>();
            videoCfg tmpvideo = null;
            var tmpfilename = "视频" + (tmpdic.Count + 1).ToString();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //this.button1.Enabled = false;


                if (startTime > DateTime.Now)
                {
                    //dateTimePicker2.Focus();
                    tmpmsg = notice + " Error:结束时间大于当前时间。" + DateTime.Now;
                    SetMsg(lbl0Msg, tmpmsg);
                    return tmpdic;
                }
                if (startTime > endTime)
                {
                    //dateTimePicker1.Focus();
                    tmpmsg = notice + " Error:开始时间大于结束时间。";
                    SetMsg(lbl0Msg, tmpmsg);
                    return tmpdic;
                }

                TMCC.tmFindConditionCfg_t ConditionCfg = new TMCC.tmFindConditionCfg_t();
                TMCC.tmFindFileCfg_t FileCfg = new TMCC.tmFindFileCfg_t();


                ConditionCfg.dwSize = (UInt32)Marshal.SizeOf(ConditionCfg);
                ConditionCfg.byChannel = byChannel; //_getConfigHost.byChannel;		//通道
                ConditionCfg.byFileType = 0xFF;		//录像文件类型
                ConditionCfg.bySearchAllTime = 1;


                /*起始时间*/
                ConditionCfg.struStartTime.wYear = Convert.ToUInt16(startTime.Year);// 2015;		//年
                ConditionCfg.struStartTime.byMonth = Convert.ToByte(startTime.Month);// 10;		//月
                ConditionCfg.struStartTime.byDay = Convert.ToByte(startTime.Day);//30;		//日
                ConditionCfg.struStartTime.byHour = Convert.ToByte(startTime.Hour);//0;  		//时
                ConditionCfg.struStartTime.byMinute = Convert.ToByte(startTime.Minute);//0;        	//分
                ConditionCfg.struStartTime.bySecond = Convert.ToByte(startTime.Second);//0;
                /*结束时间*/
                ConditionCfg.struStopTime.wYear = Convert.ToUInt16(endTime.Year);		//年
                ConditionCfg.struStopTime.byMonth = Convert.ToByte(endTime.Month);   		//月
                ConditionCfg.struStopTime.byDay = Convert.ToByte(endTime.Day); ;   		//日
                ConditionCfg.struStopTime.byHour = Convert.ToByte(endTime.Hour); ;  		//时
                ConditionCfg.struStopTime.byMinute = Convert.ToByte(endTime.Minute); ;   		//分
                ConditionCfg.struStopTime.bySecond = Convert.ToByte(endTime.Second); ;

                ConditionCfg.byEnableServer = 0;
                ConditionCfg.byOldServer = 1;
                ConditionCfg.byBackupData = 0;
                ConditionCfg.dwServerPort = 6002;

                ConditionCfg.sServerAddress = _getConfigHost.cmsip;// "192.168.1.4";//.Get(32, txtIP.Text.ToCharArray());// string.Format("{0}", txtIP.Text.ToCharArray());
                ConditionCfg.sUserName = _getConfigHost.userName;// "system";//.Get(32, txUser.Text.ToCharArray());// string.Format("{0}", txUser.Text.ToCharArray());
                ConditionCfg.sUserPass = _getConfigHost.pswd;// "system";//.Get(32, txPswd.Text.ToCharArray());// string.Format("{0}", txPswd.Text.ToCharArray());

                FileCfg.dwSize = (UInt32)Marshal.SizeOf(FileCfg);

                IntPtr p1 = Marshal.AllocHGlobal(Marshal.SizeOf(ConditionCfg));
                Marshal.StructureToPtr(ConditionCfg, p1, false);

                IntPtr p2 = Marshal.AllocHGlobal(Marshal.SizeOf(FileCfg));
                Marshal.StructureToPtr(FileCfg, p2, true);


                _hfile = TMCC.TMCC_FindFirstFile(hLogin, p1, p2);

                TMCC.tmFindFileCfg_t anotherP = (TMCC.tmFindFileCfg_t)Marshal.PtrToStructure(p2, typeof(TMCC.tmFindFileCfg_t));

                ///*************************************
                tmpfilename = "视频" + (tmpdic.Count + 1).ToString();


                if (string.IsNullOrEmpty(anotherP.sFileName))
                {
                    return tmpdic;
                }
                tmpvideo = new videoCfg();
                tmpvideo.filename = anotherP.sFileName;
                tmpvideo.fileCfg = anotherP;
                lisVideo tmpListVideo = new lisVideo();
                tmpListVideo.nameVideo = tmpfilename;
                tmpListVideo.videoCfg = tmpvideo;
                tmpdic.Add(tmpListVideo);

                ///*************************************

                do
                {
                    IntPtr p3 = Marshal.AllocHGlobal(Marshal.SizeOf(anotherP));
                    Marshal.StructureToPtr(FileCfg, p3, false);

                    var rest = TMCC.TMCC_FindNextFile(_hfile, p3);

                    if (rest != TMCC.TMCC_ERR_SUCCESS)
                    {
                        break;
                    }
                    anotherP = (TMCC.tmFindFileCfg_t)Marshal.PtrToStructure(p3, typeof(TMCC.tmFindFileCfg_t));

                    ///*************************************
                    tmpfilename = "视频" + (tmpdic.Count + 1).ToString();
                    var tmpvideoDO = new videoCfg();
                    tmpvideoDO.filename = anotherP.sFileName;
                    tmpvideoDO.fileCfg = anotherP;
                    lisVideo tmpListVideoN = new lisVideo();
                    tmpListVideoN.nameVideo = tmpfilename;
                    tmpListVideoN.videoCfg = tmpvideoDO;
                    tmpdic.Add(tmpListVideoN);
                    ///*************************************

                } while (true);


                TMCC.TMCC_FindCloseFile(_hfile);


                return tmpdic;
            }
            catch (Exception ex)
            {
                MessageBox.Show(notice + ": " + ex.Message);
                return tmpdic;
                //throw ex;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                //this.button1.Enabled = true;
            }
        }

        #region no



        private void ISECNewVideo_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void lbl0Msg_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed += 2;
            if (m_iPlaySpeed >= 10)
            {
                m_iPlaySpeed = 10;
            }

            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_FAST, m_iPlaySpeed);

            if (iflag == 0)
            {
                _currMsg = _notice + "\n【" + _videoname + "】 ,快速 " + m_iPlaySpeed + "X 播放成功.";//,时间

            }
            else
            {
                _currMsg = _notice + "\n【" + _videoname + "】 ,快速 " + m_iPlaySpeed + "X 播放失败,请再次尝试.谢谢.";
            }
            lbl0Msg.Text = _currMsg;
        }

        private void toolMenuPlay1_Click(object sender, EventArgs e)
        {
            if (_currPlayfilep)
            {
                m_iPlaySpeed = 0;
                var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_PLAY, 0);

                if (iflag == 0)
                {
                    _currMsg = _notice + "\n【" + _videoname + "】 ,播放视频成功.";//,时间
                }
                else
                {
                    _currMsg = _notice + "\n【" + _videoname + "】 ,播放视频失败,请再次尝试.谢谢.";
                }

            }
            else
            {
                _currMsg = _notice + "\n【" + _videoname + "】 ,视频播放没有播放.";
            }
            lbl0Msg.Text = _currMsg;

        }



        public string _notice { get; set; }

        public string _videoname { get; set; }

        private void toolStop1_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed = 0;
            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_STOP, 0);

            if (iflag == 0)
            {
                _currMsg = _notice + "\n【" + _videoname + "】停止播放视频成功.";//,时间
            }
            else
            {
                _currMsg = _notice + "\n【" + _videoname + "】停止播放视频失败,请再次尝试.谢谢.";
            }
            lbl0Msg.Text = _currMsg;
        }

        private void toolMenuStopPlay2_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed = 0;
            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_PAUSE, 0);

            if (iflag == 0)
            {
                _currMsg = _notice + "\n【" + _videoname + "】暂停播放视频成功.";//,时间
            }
            else
            {
                _currMsg = _notice + "\n【" + _videoname + "】暂停播放视频失败,请再次尝试.谢谢.";
            }
            lbl0Msg.Text = _currMsg;
        }

        public int m_iPlaySpeed { get; set; }

        private void toolSlowPlay1_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed += 1;
            if (m_iPlaySpeed >= 5)
            {
                m_iPlaySpeed = 5;
            }

            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_SLOW, m_iPlaySpeed);

            if (iflag == 0)
            {
                _currMsg = _notice + "\n【" + _videoname + "】慢速 " + m_iPlaySpeed + "X 播放视频成功.";//,时间
            }
            else
            {
                _currMsg = _notice + "\n【" + _videoname + "】慢速 " + m_iPlaySpeed + "X 播放视频失败,请再次尝试.谢谢.";
            }
            lbl0Msg.Text = _currMsg;
        }
        private void playOnMini(Double pos)
        {
            var tmpflag = -1;
            try
            {
                if (_currPlayfile != null)
                {

                    if (_trackBar1currtime < _trackBar1alltime)
                    {

                        tmpflag = TMCC.Avdec_SetCurrentTime(_currPlayfile, TMCC.PLAY_CONTROL_SEEKTIME, (uint)pos);

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }

        }

        public string _currMsg { get; set; }

        public TMCC.tmFindFileCfg_t _currPlayfileConfigList { get; set; }

        private void trackBar1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void trackBar1_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            //timer1.Enabled = false;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            // timer1.Enabled = true;
        }

        public bool _isScroll { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            toolMenuPlay1_Click(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            toolMenuStopPlay2_Click(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            toolSlowPlay1_Click(null, null);
        }

        public int _trackBar1alltime { get; set; }

        public int _trackBar1currtime { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            //playOnMini(120 * 1000);
            jsSetTimeStockIn(dateTimePicker1.Value, 20, 0);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //if (trackBar1.Value != _trackBar1currtime)
            //{
            //    uint dvalue = (uint)trackBar1.Value * 1000;
            //    playOnMini(dvalue);
            //}
        }

        public int _stockInSeekSeconds { get; set; }

        public bool _playNext { get; set; }

        public int _stopDiffMill { get; set; }

        public bool _currPlayfilep { get; set; }
    }

}
