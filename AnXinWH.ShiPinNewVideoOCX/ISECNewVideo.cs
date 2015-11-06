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

    [Guid("66F195EB-3753-45FA-93A6-6B58F33DBC85")]
    public partial class ISECNewVideo : UserControl, IObjectSafety, IDisposable
    {
        #region att
        IntPtr hPreView = IntPtr.Zero;
        IntPtr hLogin = IntPtr.Zero;
        IntPtr _currPlayfile = IntPtr.Zero;
        IntPtr _hfile = IntPtr.Zero;

        static Dictionary<string, videoCfg> _dicIn { get; set; }
        static Dictionary<string, videoCfg> _dicOut { get; set; }
        static Dictionary<string, videoCfg> _dicShelf { get; set; }

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
        public ISECNewVideo()
        {
            InitializeComponent();

            try
            {
                this.Disposed += ISECNewVideo_Disposed;
                listBox1.DoubleClick += listBox1_DoubleClick;
                listBox2.DoubleClick += listBox2_DoubleClick;
                listBox3.DoubleClick += listBox3_DoubleClick;

                initForm();
                initVideo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void listBox3_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            play_DoubleClick(listBox3, _dicOut, "出库视频");
        }

        void listBox2_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            play_DoubleClick(listBox2, _dicShelf, "上架视频");
        }

        void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            play_DoubleClick(listBox1, _dicIn, "入库视频");
        }
        #region function
        private void initForm()
        {
            //throw new NotImplementedException();
            lbl0Msg.Text = "";

            _InstartTime = DateTime.Now.AddHours(-1);
            _InendTime = DateTime.Now;

            //dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            //dateTimePicker1.Text = DateTime.Now.AddHours(-7).ToString();

            //dateTimePicker2.Format = DateTimePickerFormat.Custom;
            //dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";

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
            Info.pIp = Get(32, "192.168.1.4".ToCharArray());//txtIP.Text
            Info.szPass = Get(32, "system".ToCharArray());
            Info.szUser = Get(32, "system".ToCharArray());
            Info.iPort = Convert.ToInt32("6002".Trim());//"6002"txPort.Text
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

        void playOldFileListName(string name, Dictionary<string, videoCfg> dic, string notice)
        {
            try
            {
                if (dic.ContainsKey(name))
                {

                    closeAll();

                    var anotherP = dic[name].fileCfg;

                    TMCC.tmPlayConditionCfg_t struCond = new TMCC.tmPlayConditionCfg_t();

                    struCond.dwSize = (UInt32)Marshal.SizeOf(struCond);

                    struCond.byChannel = 0;
                    struCond.struStartTime = anotherP.struStartTime;
                    struCond.struStopTime = anotherP.struStopTime;

                    IntPtr p4 = Marshal.AllocHGlobal(Marshal.SizeOf(struCond));
                    Marshal.StructureToPtr(struCond, p4, false);

                    IntPtr p6 = TMCC.TMCC_OpenFile(hLogin, p4, this.pictureBox1.Handle);

                    _currPlayfile = p6;
                    _notice = notice;
                    _videoname = name;

                    m_iPlaySpeed = 0;
                    var iflag = TMCC.Avdec_PlayToDo(p6, TMCC.PLAY_CONTROL_PLAY, 0);

                    if (iflag == 0)
                    {
                        lbl0Msg.Text = notice + ",播放视频成功:" + name;//,时间
                    }
                    else
                    {
                        lbl0Msg.Text = notice + ",播放视频失败:" + name + ",请再次尝试.谢谢.";
                    }

                }
                else
                {
                    MessageBox.Show(notice + "," + name + "不存在.请选择正确的文件名。");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(notice + ",Play Error:" + ex.Message);
                //throw ex;
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
        public string jsSetTimeIn(object start, object end)
        {
            try
            {
                if (start != null && end != null)
                {
                    DateTime tmpStart = DateTime.Now;

                    DateTime tmpEnd = DateTime.Now;
                    if (DateTime.TryParse(start.ToString(), out tmpStart))
                    {
                        _InstartTime = tmpStart;
                        //dateTimePicker1.Value = startTime;
                    }
                    else
                    {
                        return "入库视频 开始时间有误：" + start.ToString();
                    }

                    if (DateTime.TryParse(end.ToString(), out tmpEnd))
                    {
                        _InendTime = tmpEnd;
                        //dateTimePicker2.Value = endTime;
                    }
                    else
                    {
                        return "入库视频 结束时间有误：" + end.ToString();
                    }
                    var notice = _InstartTime.ToString("yyyy-MM-dd HH:mm:ss") + "----> " + _InendTime.ToString("yyyy-MM-dd HH:mm:ss"); ;

                    _dicIn = getListMoveFromStartAnd(_InstartTime, _InendTime, listBox1, "入库视频");

                    if (listBox1.Items.Count > 0 && _dicIn.Count > 0)
                    {
                        listBox1.SelectedIndex = 0;
                        listBox1_DoubleClick(null, null);
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

        public string jsSetTimeOut(object start, object end)
        {
            try
            {
                if (start != null && end != null)
                {
                    DateTime tmpStart = DateTime.Now;

                    DateTime tmpEnd = DateTime.Now;
                    if (DateTime.TryParse(start.ToString(), out tmpStart))
                    {
                        _OutstartTime = tmpStart;
                        //dateTimePicker1.Value = startTime;
                    }
                    else
                    {
                        return "出库视频 开始时间有误：" + start.ToString();
                    }

                    if (DateTime.TryParse(end.ToString(), out tmpEnd))
                    {
                        _OutendTime = tmpEnd;
                        //dateTimePicker2.Value = endTime;
                    }
                    else
                    {
                        return "出库视频 结束时间有误：" + end.ToString();
                    }
                    var notice = _OutstartTime.ToString("yyyy-MM-dd HH:mm:ss") + "----> " + _OutendTime.ToString("yyyy-MM-dd HH:mm:ss"); ;

                    _dicOut = getListMoveFromStartAnd(_OutstartTime, _OutendTime, listBox3, "出库视频");

                    if (listBox3.Items.Count > 0)
                    {
                        listBox3.SelectedIndex = 0;
                        //listBox2_DoubleClick(null, null);
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

        public string jsSetTimeShelf(object start, object end)
        {
            try
            {
                if (start != null && end != null)
                {
                    DateTime tmpStart = DateTime.Now;

                    DateTime tmpEnd = DateTime.Now;
                    if (DateTime.TryParse(start.ToString(), out tmpStart))
                    {
                        _ShelfstartTime = tmpStart;
                        //dateTimePicker1.Value = startTime;
                    }
                    else
                    {
                        return "上架视频 开始时间有误：" + start.ToString();
                    }

                    if (DateTime.TryParse(end.ToString(), out tmpEnd))
                    {
                        _ShelfendTime = tmpEnd;
                        //dateTimePicker2.Value = endTime;
                    }
                    else
                    {
                        return "上架视频 结束时间有误：" + end.ToString();
                    }
                    var notice = _ShelfstartTime.ToString("yyyy-MM-dd HH:mm:ss") + "----> " + _ShelfendTime.ToString("yyyy-MM-dd HH:mm:ss"); ;

                    _dicShelf = getListMoveFromStartAnd(_ShelfstartTime, _ShelfendTime, listBox2, "出库视频");

                    if (listBox2.Items.Count > 0)
                    {
                        listBox2.SelectedIndex = 0;
                        //listBox2_DoubleClick(null, null);
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

        private Dictionary<string, videoCfg> getListMoveFromStartAnd(DateTime startTime, DateTime endTime, ListBox tmplis, string notice)
        {
            var tmpdic = new Dictionary<string, videoCfg>();
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

                tmplis.Items.Clear();


                TMCC.tmFindConditionCfg_t ConditionCfg = new TMCC.tmFindConditionCfg_t();
                TMCC.tmFindFileCfg_t FileCfg = new TMCC.tmFindFileCfg_t();


                ConditionCfg.dwSize = (UInt32)Marshal.SizeOf(ConditionCfg);
                ConditionCfg.byChannel = 0;		//通道
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
                ConditionCfg.sServerAddress = "192.168.1.4";//.Get(32, txtIP.Text.ToCharArray());// string.Format("{0}", txtIP.Text.ToCharArray());
                ConditionCfg.sUserName = "system";//.Get(32, txUser.Text.ToCharArray());// string.Format("{0}", txUser.Text.ToCharArray());
                ConditionCfg.sUserPass = "system";//.Get(32, txPswd.Text.ToCharArray());// string.Format("{0}", txPswd.Text.ToCharArray());

                FileCfg.dwSize = (UInt32)Marshal.SizeOf(FileCfg);

                IntPtr p1 = Marshal.AllocHGlobal(Marshal.SizeOf(ConditionCfg));
                Marshal.StructureToPtr(ConditionCfg, p1, false);

                IntPtr p2 = Marshal.AllocHGlobal(Marshal.SizeOf(FileCfg));
                Marshal.StructureToPtr(FileCfg, p2, true);


                _hfile = TMCC.TMCC_FindFirstFile(hLogin, p1, p2);

                TMCC.tmFindFileCfg_t anotherP = (TMCC.tmFindFileCfg_t)Marshal.PtrToStructure(p2, typeof(TMCC.tmFindFileCfg_t));

                ///*************************************
                tmpfilename = "视频" + (tmpdic.Count + 1).ToString();

                tmpvideo = new videoCfg();
                tmpvideo.filename = anotherP.sFileName;
                tmpvideo.fileCfg = anotherP;
                tmpdic.Add(tmpfilename, tmpvideo);

                tmplis.Items.Add(tmpfilename);
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
                    tmpdic.Add(tmpfilename, tmpvideoDO);

                    tmplis.Items.Add(tmpfilename);
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
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
                stream.szAddress = Get(32, "192.168.1.4".ToCharArray());
                stream.szTurnAddress = Get(32, "192.168.1.4".ToCharArray());
                stream.szUser = Get(32, "system".ToCharArray());
                stream.szPass = Get(32, "system".ToCharArray());
                stream.iPort = Convert.ToInt32(6002);
                stream.byChannel = byte.Parse("0");
                stream.byStream = byte.Parse("0");

                ret = TMCC.TMCC_ConnectStream(hPreView, ref stream, pictureBox1.Handle);
                var error = TMCC.TMCC_GetLastError();

                if (ret != TMCC.TMCC_ERR_SUCCESS)
                {
                    SetMsg(lbl0Msg, "预览视频失败。" + DateTime.Now.ToString());
                    MessageBox.Show("预览视频失败");
                }
                else
                {
                    SetMsg(lbl0Msg, "预览实时视频成功。" + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void play_DoubleClick(ListBox lis, Dictionary<string, videoCfg> dic, string notice)
        {
            if (lis.SelectedItem != null)
            {
                var tmpname = lis.SelectedItem.ToString();
                lbl0Msg.Text = notice + ", 回放视频中." + tmpname;
                playOldFileListName(tmpname, dic, notice);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed += 1;
            if (m_iPlaySpeed >= 15)
            {
                m_iPlaySpeed = 15;
            }

            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_FAST, m_iPlaySpeed);

            if (iflag == 0)
            {
                lbl0Msg.Text = _notice + ",快速 " + m_iPlaySpeed + "X 播放视频成功:" + _videoname;//,时间
            }
            else
            {
                lbl0Msg.Text = _notice + ",快速 " + m_iPlaySpeed + "X 播放视频失败:" + _videoname + ",请再次尝试.谢谢.";
            }
        }

        private void toolMenuPlay1_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed = 0;
            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_PLAY, 0);

            if (iflag == 0)
            {
                lbl0Msg.Text = _notice + ",播放视频成功:" + _videoname;//,时间
            }
            else
            {
                lbl0Msg.Text = _notice + ",播放视频失败:" + _videoname + ",请再次尝试.谢谢.";
            }
        }



        public string _notice { get; set; }

        public string _videoname { get; set; }

        private void toolStop1_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed = 0;
            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_STOP, 0);

            if (iflag == 0)
            {
                lbl0Msg.Text = _notice + ",停止播放视频成功:" + _videoname;//,时间
            }
            else
            {
                lbl0Msg.Text = _notice + ",停止播放视频失败:" + _videoname + ",请再次尝试.谢谢.";
            }
        }

        private void toolMenuStopPlay2_Click(object sender, EventArgs e)
        {
            m_iPlaySpeed = 0;
            var iflag = TMCC.Avdec_PlayToDo(_currPlayfile, TMCC.PLAY_CONTROL_PAUSE, 0);

            if (iflag == 0)
            {
                lbl0Msg.Text = _notice + ",暂停播放视频成功:" + _videoname;//,时间
            }
            else
            {
                lbl0Msg.Text = _notice + ",暂停播放视频失败:" + _videoname + ",请再次尝试.谢谢.";
            }
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
                lbl0Msg.Text = _notice + ",慢速 " + m_iPlaySpeed + "X 播放视频成功:" + _videoname;//,时间
            }
            else
            {
                lbl0Msg.Text = _notice + ",慢速 " + m_iPlaySpeed + "X 播放视频失败:" + _videoname + ",请再次尝试.谢谢.";
            }
        }
    }

    public class videoCfg
    {
        public string filename { get; set; }
        public TMCC.tmFindFileCfg_t fileCfg { get; set; }
    }
}
