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

        bool hLoginp = false;
        static configHost _getConfigHost = comm.getConfigHostN();

        static List<lisVideo> _lisIn { get; set; }
        static Dictionary<string, video> _dicAlarm { get; set; }

        public string _receiveNo { get; private set; }

        static DateTime _InstartTime = DateTime.Now;
        static DateTime _InendTime = DateTime.Now;


        static DateTime _OutstartTime = DateTime.Now;
        static DateTime _OutendTime = DateTime.Now;


        static DateTime _ShelfstartTime = DateTime.Now;
        static DateTime _ShelfendTime = DateTime.Now;


        //collback
        string tmpmsg { get; set; }

        TMCC.StreamCallback streamback = null;
        TMCC.AvFrameCallback frameback = null;
        int iTemp = 0;

        int _seekSeconds { get; set; }

        bool _playNext { get; set; }
        int _stopDiffMill { get; set; }
        int _stopDiffMillNext { get; set; }
        bool _currPlayfilep { get; set; }

        DateTime _pretmpVideoEnd { get; set; }
        DateTime _nextStartPlay { get; set; }
        DateTime _nextEndPlay { get; set; }
        int _trackBar1alltime { get; set; }
        int _trackBar1currtime { get; set; }


        string _notice { get; set; }
        string _videoname { get; set; }
        int m_iPlaySpeed { get; set; }
        string _currMsg { get; set; }
        TMCC.tmFindFileCfg_t _currPlayfileConfigList { get; set; }
        #endregion

        #region public att
        public byte _byChannel { get; private set; }


        public byte _byInChannel { get; private set; }
        public string _jsStockInDate { get; private set; }
        public string _jsStockInseekSeconds { get; private set; }
        public string _jsStockInNotic { get; private set; }

        public byte _byOutChannel { get; private set; }
        public string _jsStockOutDate { get; private set; }
        public string _jsStockOutNotic { get; private set; }
        public string _jsStockOutseekSeconds { get; private set; }

        public byte _byShelfChannel { get; private set; }
        public string _jsStockShelfDate { get; private set; }
        public string _jsStockShelfNotic { get; private set; }
        public string _jsStockShelfseekSeconds { get; private set; }

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
        #region for litbox
        private void play_DoubleClick(ListBox lis, string notice)
        {
            if (lis.SelectedItem != null)
            {
                _isDoublePlay = true;
                var tmpname = lis.SelectedItem.ToString();
                lbl0Msg.Text = notice + ", 播放视频:" + tmpname;
                if (_dicAlarm.ContainsKey(tmpname))
                {
                    var tmpvideo = _dicAlarm[tmpname];
                    _lisIn = new List<lisVideo>();
                    _lisIn = getListMoveFromStartAnd(tmpvideo.start.AddSeconds(-tmpvideo.seekSeconds), tmpvideo.start.AddSeconds(tmpvideo.seekSeconds), tmpvideo.title, tmpvideo.bychange);
                    if (_lisIn.Count > 0)
                    {
                        _notice = tmpvideo.title;
                        _startTime = tmpvideo.start.AddSeconds(-tmpvideo.seekSeconds);
                        _endTime = tmpvideo.start.AddSeconds(tmpvideo.seekSeconds);
                        _byChannel = tmpvideo.bychange;
                        playOldFileListName(_lisIn[0], _notice, _startTime, _endTime, _byChannel, this.pictureBox1.Handle);
                        //return;
                    }
                }
                lbl0Msg.Text = notice + ":" + tmpname + ",没有找到对应视频记录.";
            }
        }
        #endregion

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

            _byChannel = 0;

            _jsStockInDate = null;
            _jsStockInNotic = null;
            _jsStockOutDate = null;
            _jsStockOutNotic = null;
            _jsStockShelfDate = null;
            _jsStockShelfNotic = null;

            //list doublie
            listBox1.DoubleClick += listBox1_DoubleClick;

            _fullplay = new FullPlay(this);
        }

        void listBox1_DoubleClick(object sender, EventArgs e)
        {
            play_DoubleClick(listBox1, "报警");
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
                hLoginp = false;
                TMCC.TMCC_DisConnect(hLogin);
                MessageBox.Show("登陆设备失败");

            }
            else
            {
                TMCC.TMCC_Connect(hLogin, ref Info, true);
                hLoginp = true;
            }
        }
        void playOldFileListName(lisVideo tmpvideo, string notice, DateTime start, DateTime endtime, byte byChannel, IntPtr hwind)
        {
            try
            {
                closeAll();
                _playNow = false;

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

                IntPtr p6 = TMCC.TMCC_OpenFile(hLogin, p4, hwind);

                _currPlayfile = p6;
                _notice = notice;
                _byChannel = byChannel;
                _videoname = tmpvideo.nameVideo;
                _currPlayfileConfigList = anotherP;

                m_iPlaySpeed = 0;
                var iflag = TMCC.Avdec_PlayToDo(p6, TMCC.PLAY_CONTROL_PLAY, 0);

                if (iflag == 0)
                {
                    _currPlayfilep = true;

                    var getFileStatus = TMCC.Avdec_GetTmPlayStateCfg_t(_currPlayfile);
                    _trackBar1alltime = Convert.ToInt32(getFileStatus.dwTotalTimes / 1000);
                    _trackBar1currtime = Convert.ToInt32(getFileStatus.dwCurrentTimes / 1000);

                    var diffsecStart = start - tmpVideoStart;

                    var diffsecEnd = DateTime.Compare(endtime, tmpVideoEnd);

                    if (diffsecEnd > 0)
                    {
                        _playNext = true;
                        _nextStartPlay = tmpVideoEnd.AddSeconds(1);
                        _nextEndPlay = endtime;

                        _stopDiffMill = Convert.ToInt32(getFileStatus.dwTotalTimes / 1000);
                    }
                    else
                    {
                        _playNext = false;
                        var stopDiffMill = endtime - tmpVideoStart;
                        _stopDiffMill = Convert.ToInt32(stopDiffMill.TotalMilliseconds / 1000);
                    }


                    if (diffsecStart.TotalMilliseconds > 0)
                    {


                        timer1.Enabled = true;
                        _currMsg = notice + "\n播放成功.";//,时间
                        playOnMini(diffsecStart.TotalMilliseconds);
                    }

                    //trackBar1.Maximum = 0;
                    //trackBar1.Value = 0;
                }
                else
                {
                    _currMsg = notice + "\n播放失败,请再次尝试.谢谢.";

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
                _trackBar1currtime = Convert.ToInt32(getFileStatus.dwCurrentTimes / 1000) + 3;

                var st = _currPlayfileConfigList.struStartTime;
                var startTime = st.wYear.ToString() + "-" + st.byMonth.ToString() + "-" + st.byDay + " " + st.byHour + ":" + st.byMinute + ":" + st.bySecond;

                if (_trackBar1alltime > 0)
                {
                    if (_trackBar1currtime >= _trackBar1alltime)
                    {
                        if (_playNext)
                        {
                            lbl0Msg.Text = _currMsg + ",正在加载中。。。。。。。";
                            _playNext = false;
                            if (_lisIn.Count >= 1)
                            {
                                _startTime = _nextStartPlay;
                                _endTime = _nextEndPlay;
                                playOldFileListName(_lisIn[1], _notice, _startTime, _endTime, _byChannel, this.pictureBox1.Handle);
                            }
                        }
                        else
                        {
                            TMCC.TMCC_CloseFile(_currPlayfile);
                            timer1.Enabled = false;
                            _currPlayfilep = false;
                            pictureBox1.Refresh();
                            _fullplay.TopMost = false;
                            _fullplay.Close();
                            lbl0Msg.Text = _notice + ",已播放完成.";
                        }
                        return;
                    }
                    else
                    {
                        if (_trackBar1currtime >= _stopDiffMill)
                        {
                            TMCC.TMCC_CloseFile(_currPlayfile);
                            timer1.Enabled = false;
                            _currPlayfilep = false;
                            pictureBox1.Refresh();
                            _fullplay.TopMost = false;
                            _fullplay.Close();
                            lbl0Msg.Text = _notice + ",已播放完成.";
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
        public void closeAll()
        {
            try
            {

                if (hPreView != null)
                {
                    TMCC.TMCC_CloseStream(hPreView);
                    TMCC.TMCC_ClearDisplay(hPreView);
                }

                //close
                if (_currPlayfilep)
                {
                    TMCC.TMCC_CloseFile(_currPlayfile);
                    _currPlayfilep = false;
                    pictureBox1.Refresh();
                }
                _currMsg = "";
                timer1.Enabled = false;
                SetMsg(lbl0Msg, _currMsg);
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
        void SetMsg(Control lbl, string msg)
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


        void startPlayNow(IntPtr hwind)
        {
            try
            {
                _lisIn = new List<lisVideo>();
                _playNow = true;
                closeAll();
                var ret = 0;
                ret = TMCC.TMCC_SetAutoReConnect(hPreView, true);
                ret = TMCC.TMCC_SetDisplayShow(hPreView, true);
                ret = TMCC.TMCC_SetStreamBufferTime(hPreView, uint.Parse("0"));
                streamback = StreamDataCallBack;
                ret = TMCC.TMCC_RegisterStreamCallBack(hPreView, streamback, hPreView);
                frameback = AvFrameCallBack;
                ret = TMCC.TMCC_RegisterAVFrameCallBack(hPreView, frameback, hPreView);
                ret = TMCC.TMCC_SetImageOutFmt(hPreView, 3);
                TMCC.tmPlayRealStreamCfg_t stream = new TMCC.tmPlayRealStreamCfg_t();
                stream.Init();
                stream.dwSize = (UInt32)Marshal.SizeOf(stream);

                stream.szAddress = Get(32, _getConfigHost.cmsip.ToCharArray());
                stream.szTurnAddress = Get(32, _getConfigHost.cmsip.ToCharArray());
                stream.szUser = Get(32, _getConfigHost.userName.ToCharArray());
                stream.szPass = Get(32, _getConfigHost.pswd.ToCharArray());
                stream.iPort = _getConfigHost.cmsPort;


                stream.byChannel = _getConfigHost.byChannel;// byte.Parse("0");
                stream.byStream = _getConfigHost.byStream;// byte.Parse("0");

                ret = TMCC.TMCC_ConnectStream(hPreView, ref stream, hwind);
                var error = TMCC.TMCC_GetLastError();

                if (ret != TMCC.TMCC_ERR_SUCCESS)
                {
                    SetMsg(lbl0Msg, "预览视频失败。");// + DateTime.Now.ToString());
                    //MessageBox.Show("预览视频失败");
                }
                else
                {
                    SetMsg(lbl0Msg, "预览实时视频成功。");// + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #region for js todo
        public void SetReceiptNo(object o)
        {
            if (o != null)
            {
                _receiveNo = o.ToString();
                SetMsg(groupBox1, _receiveNo);
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
        /// <summary>
        /// 报警标题,时间点,左右偏移秒,通道号|N|....|N|
        /// </summary>
        /// <param name="videosAlarm"></param>
        public void jsStockAlarm(object videosAlarm)
        {
            try
            {
                _dicAlarm = new Dictionary<string, video>();

                var tmpSplit = videosAlarm.ToString().Split('|');
                foreach (var item in tmpSplit)
                {
                    var tmpSplit2 = item.Split(',');

                    if (tmpSplit2.Length < 4)
                    {
                        continue;
                    }
                    var tmpkey = tmpSplit2[0] + "." + tmpSplit2[1];
                    if (!_dicAlarm.ContainsKey(tmpkey))
                    {
                        var tmpvideo = new video();
                        tmpvideo.bychange = byte.Parse(tmpSplit2[3]);
                        tmpvideo.seekSeconds = Int32.Parse(tmpSplit2[2]);
                        tmpvideo.start = DateTime.Parse(tmpSplit2[1]);
                        tmpvideo.title = tmpSplit2[0];
                        _dicAlarm.Add(tmpkey, tmpvideo);
                    }
                    else
                    {
                        continue;
                    }

                }
                listBox1.Items.Clear();
                // update list
                foreach (var item in _dicAlarm)
                {
                    listBox1.Items.Add(item.Key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void jsStockIn(object title, object start, object seekSeconds, object bychange)
        {
            try
            {
                _byInChannel = byte.Parse(bychange.ToString());
                _jsStockInseekSeconds = seekSeconds.ToString();

                _jsStockInDate = start.ToString();
                _jsStockInNotic = title.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void jsStockOut(object title, object start, object seekSeconds, object bychange)
        {
            try
            {
                _byOutChannel = byte.Parse(bychange.ToString());
                _jsStockOutseekSeconds = seekSeconds.ToString();

                _jsStockOutDate = start.ToString();
                _jsStockOutNotic = title.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void jsStockShelf(object title, object start, object seekSeconds, object bychange)
        {
            try
            {
                _byShelfChannel = byte.Parse(bychange.ToString());
                _jsStockShelfseekSeconds = seekSeconds.ToString();

                _jsStockShelfDate = start.ToString();
                _jsStockShelfNotic = title.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void jsStartVideo(object o)
        {
            if (o != null)
            {
                byte byChannel = 0;
                byte.TryParse(o.ToString(), out byChannel);
                _getConfigHost.byChannel = byChannel;
                //initVideo();
            }
        }
        string jsSetTimeStockIn(object title, object start, object seekSeconds, object bychange)
        {
            var tmpoff = 10;
            byte tmpbyChangel = 0;

            try
            {
                if (!hLoginp)
                {
                    initVideo();

                }

                SetMsg(groupBox1, _receiveNo);

                if (start != null)
                {
                    DateTime tmpStart = DateTime.Now.AddDays(-1);
                    if (DateTime.TryParse(start.ToString(), out tmpStart))
                    {
                        var tmpoff_flag = Int32.TryParse(seekSeconds.ToString(), out tmpoff);

                        _seekSeconds = tmpoff;
                        _InstartTime = tmpStart.AddSeconds(-tmpoff);
                        _InendTime = tmpStart.AddSeconds(tmpoff);
                    }
                    else
                    {
                        return title.ToString() + " 开始时间有误：" + start.ToString();
                    }

                    var notice = title.ToString() + ":";// + _InstartTime.ToString("yyyy-MM-dd HH:mm:ss") + "----> " + _InendTime.ToString("yyyy-MM-dd HH:mm:ss"); ;

                    byte.TryParse(bychange.ToString(), out tmpbyChangel);

                    _lisIn = getListMoveFromStartAnd(_InstartTime, _InendTime, title.ToString(), tmpbyChangel);

                    if (_lisIn.Count > 0)
                    {
                        var tmpname = _lisIn[0];
                        lbl0Msg.Text = notice + ", 回放视频中." + tmpname;

                        _startTime = _InstartTime;
                        _endTime = _InendTime;
                        playOldFileListName(tmpname, notice, _startTime, _endTime, tmpbyChangel, this.pictureBox1.Handle);
                    }
                    else
                    {
                        closeAll();
                        lbl0Msg.Text = notice + ", 无历史记录.";
                        return "无历史记录";
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
                _currMsg = _notice + "\n快速 " + m_iPlaySpeed + "X 播放成功.";//,时间

            }
            else
            {
                _currMsg = _notice + "\n快速 " + m_iPlaySpeed + "X 播放失败,请再次尝试.谢谢.";
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
                    _currMsg = _notice + "\n播放视频成功.";//,时间
                }
                else
                {
                    _currMsg = _notice + "\n播放视频失败,请再次尝试.谢谢.";
                }

            }
            else
            {
                _currMsg = _notice + "\n视频播放没有播放.";
            }
            lbl0Msg.Text = _currMsg;

        }





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

                    if (_trackBar1currtime <= _trackBar1alltime)
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



        private void button2_Click(object sender, EventArgs e)
        {
            //playOnMini(120 * 1000);
            jsSetTimeStockIn("回放测试", dateTimePicker1.Value, 20, 0);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //if (trackBar1.Value != _trackBar1currtime)
            //{
            //    uint dvalue = (uint)trackBar1.Value * 1000;
            //    playOnMini(dvalue);
            //}
        }

        private void btn0StockIn_Click(object sender, EventArgs e)
        {
            jsSetTimeStockIn(_jsStockInNotic, _jsStockInDate, _jsStockInseekSeconds, _byInChannel);
        }

        private void btn1Shelf_Click(object sender, EventArgs e)
        {
            jsSetTimeStockIn(_jsStockShelfNotic, _jsStockShelfDate, _jsStockShelfseekSeconds, _byShelfChannel);
        }

        private void btn2StockOut_Click(object sender, EventArgs e)
        {
            jsSetTimeStockIn(_jsStockOutNotic, _jsStockOutDate, _jsStockOutseekSeconds, _byOutChannel);
        }

        public void btn0Now_Click(object sender, EventArgs e)
        {
            startPlayNow(pictureBox1.Handle);
        }
        #region user32.dll

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern Int32 ShowWindow(Int32 hwnd, Int32 nCmdShow);
        public const Int32 SW_SHOW = 5; public const Int32 SW_HIDE = 0;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        private static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, ref Rectangle lpvParam, Int32 fuWinIni);
        public const Int32 SPIF_UPDATEINIFILE = 0x1;
        public const Int32 SPI_SETWORKAREA = 47;
        public const Int32 SPI_GETWORKAREA = 48;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern Int32 FindWindow(string lpClassName, string lpWindowName);

        #endregion
        /// <summary>  
        /// 设置全屏或这取消全屏  
        /// </summary>  
        /// <param name="fullscreen">true:全屏 false:恢复</param>  
        /// <param name="rectOld">设置的时候，此参数返回原始尺寸，恢复时用此参数设置恢复</param>  
        /// <returns>设置结果</returns>  
        public Boolean SetFormFullScreen(Boolean fullscreen)//, ref Rectangle rectOld
        {
            Rectangle rectOld = Rectangle.Empty;
            Int32 hwnd = 0;
            hwnd = FindWindow("Shell_TrayWnd", null);//获取任务栏的句柄

            if (hwnd == 0) return false;

            if (fullscreen)//全屏
            {
                ShowWindow(hwnd, SW_HIDE);//隐藏任务栏

                SystemParametersInfo(SPI_GETWORKAREA, 0, ref rectOld, SPIF_UPDATEINIFILE);//get  屏幕范围
                Rectangle rectFull = Screen.PrimaryScreen.Bounds;//全屏范围
                SystemParametersInfo(SPI_SETWORKAREA, 0, ref rectFull, SPIF_UPDATEINIFILE);//窗体全屏幕显示
            }
            else//还原 
            {
                ShowWindow(hwnd, SW_SHOW);//显示任务栏

                SystemParametersInfo(SPI_SETWORKAREA, 0, ref rectOld, SPIF_UPDATEINIFILE);//窗体还原
            }
            return true;
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //if (_isDoublePlay)
                //{
                //    return;
                //}
                if (_lisIn.Count > 0)
                {
                    m_IsFullScreen = !m_IsFullScreen;//点一次全屏，再点还原。  
                    this.SuspendLayout();
                    _fullplay = new FullPlay(this);
                    if (m_IsFullScreen)//全屏 ,按特定的顺序执行
                    {
                        SetFormFullScreen(m_IsFullScreen);
                        _fullplay.FormBorderStyle = FormBorderStyle.None;
                        _fullplay.WindowState = FormWindowState.Maximized;
                        _fullplay.TopMost = true;
                        _fullplay.Show();
                        playOldFileListName(_lisIn[0], _notice, _startTime, _endTime, _byChannel, _fullplay.pictureBox1.Handle);

                    }
                    //else//还原，按特定的顺序执行——窗体状态，窗体边框，设置任务栏和工作区域
                    //{
                    //    _fullplay.WindowState = FormWindowState.Normal;
                    //    _fullplay.FormBorderStyle = FormBorderStyle.Sizable;
                    //    SetFormFullScreen(m_IsFullScreen);
                    //    _fullplay.TopMost = false;
                    //    closeAll();
                    //    _fullplay.Close();
                    //}

                }
                else
                {
                    if (_playNow)
                    {
                        m_IsFullScreen = !m_IsFullScreen;//点一次全屏，再点还原。  
                        this.SuspendLayout();
                        _fullplay = new FullPlay(this);
                        if (m_IsFullScreen)//全屏 ,按特定的顺序执行
                        {
                            SetFormFullScreen(m_IsFullScreen);
                            _fullplay.FormBorderStyle = FormBorderStyle.None;
                            _fullplay.WindowState = FormWindowState.Maximized;
                            _fullplay.TopMost = true;
                            _fullplay.Show();
                            startPlayNow(_fullplay.pictureBox1.Handle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_IsFullScreen = false;
                closeAll();
                _fullplay.TopMost = true;
                _fullplay.Close();
                SetFormFullScreen(m_IsFullScreen);
                _playNow = false;
            }



        }


        public int _oldLeft { get; set; }

        public int _oldTop { get; set; }

        public int _oldheight { get; set; }

        public int _oldwidth { get; set; }

        public Boolean m_IsFullScreen = false;//标记是否全屏

        public static FullPlay _fullplay;

        public bool _playNow { get; set; }

        public bool _isDoublePlay { get; set; }

        public DateTime _endTime { get; set; }

        public DateTime _startTime { get; set; }
    }

}
