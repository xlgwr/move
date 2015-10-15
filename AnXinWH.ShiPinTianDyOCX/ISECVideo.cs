using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using frameworkApiTianDy;

namespace AnXinWH.ShiPinTianDyOCX
{

    [Guid("EF5FEA44-AD86-429E-B13E-FA94F5F06193")]
    public partial class ISECVideo : UserControl, IObjectSafety
    {
        delegate void D(object obj);

        //当前登录状态结构体
        CLIENTINFO m_cltInfo;
        //视频窗口对应的连接状态结构体数组
        CONNECT_STATE[] m_conState;
        //当前视频窗口标记，从0开始
        int m_iCurrentFrame = 0;
        #region att
        private string _ip;

        public string Ip
        {
            get { return _ip; }
            private set { _ip = value; }
        }
        private string _CamerID;

        public string CamerID
        {
            get { return _CamerID; }
            private set { _CamerID = value; }
        }
        private string[] _cboChannel;

        public string[] CboChannel
        {
            get { return _cboChannel; }
            private set { _cboChannel = value; }
        }
        private string[] _cboMode;

        public string[] CboMode
        {
            get { return _cboMode; }
            private set { _cboMode = value; }
        }
        private string[] _cboScreen;

        public string[] CboScreen
        {
            get { return _cboScreen; }
            private set { _cboScreen = value; }
        }
        private string[] _cbocboStream;

        public string[] CbocboStream
        {
            get { return _cbocboStream; }
            private set { _cbocboStream = value; }
        }

        #endregion
        public ISECVideo()
        {
            InitializeComponent();

            //
            //this.Resize += Form1_Resize;
            //this.Disposed += ISECVideo_Disposed;
            //initfrist
            initFirst();
            //initwith
            //initwith(m_video);
            m_cltInfo.m_iServerID = -1;
            StartUp();
        }

        void ISECVideo_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int iRet;
            //停止接受视频数据
            iRet = NVSSDK.NetClient_StopCaptureData(m_conState[m_iCurrentFrame].m_uiConID);

            //停止播放某路视频
            iRet = NVSSDK.NetClient_StopPlay(m_conState[m_iCurrentFrame].m_uiConID);
            m_video.Invalidate(true);
        }
        private void initFirst()
        {
            CboChannel = "CH1,CH2,CH3,CH4,CH5,CH6,CH7,CH8,CH9,CH10,CH11,CH12,CH13,CH14,CH15,CH16".Split(',');
            CboMode = "TCP,UDP,MULTICAST".Split(',');
            CboScreen = "S1,S4,S9,S16".Split(',');
            CboScreen = "MAIN,SUB".Split(',');
            IsPlay = false;
        }

        private void StartUp()
        {
            //设置客户端和主控端所用的默认网络端口
            NVSSDK.NetClient_SetPort(3000, 6000);

            //设置消息通知ID
            NVSSDK.NetClient_SetMSGHandle(SDKConstMsg.WM_MAIN_MESSAGE, this.Handle, SDKConstMsg.MSG_PARACHG, SDKConstMsg.MSG_ALARM);

            //启动SDK
            NVSSDK.NetClient_Startup();

            //初始化NSLook库
            NVSSDK.NSLook_Startup();

            //创建视频窗口对象
            m_conState = new CONNECT_STATE[1];

            //初始化连接状态结构体
            m_conState[0].m_iChannelNO = -1;
            m_conState[0].m_iLogonID = -1;
            m_conState[0].m_uiConID = UInt32.MaxValue;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            initwith(m_video);
        }

        private void initwith(GroupBox gbtop1, Control gbtop2)
        {
            gbtop1.Width = this.Width - gbtop1.Left - 20;

            gbtop2.Top = gbtop1.Top + gbtop1.Height;
            gbtop2.Left = gbtop1.Left;
            gbtop2.Width = gbtop1.Width;
            gbtop2.Height = this.Height - gbtop2.Top - 50;
            //throw new NotImplementedException();
        }
        private void initwith(Control gbtop1)
        {
            gbtop1.Width = this.Width - gbtop1.Left - 20;
            gbtop1.Height = this.Height - gbtop1.Top - 20;
            //throw new NotImplementedException();
        }
        #region tiandy
        private void InitWindowStates()
        {
            //btn1Logon.Text = "Logon";
            //btn2Connect.Text = "Connect";
            //btn3Play.Text = "Play";
        }
        //WCM_VIDEO_HEAD消息处理函数
        private void VideoArrive()
        {
            RECT rect = new RECT();

            //视频到达后开始播放   
            NVSSDK.NetClient_StartPlay(m_conState[m_iCurrentFrame].m_uiConID, m_video.Handle, rect, 0);

            //修改视频状态信息
            GetWindowStates();
        }

        //设置窗口对应的状态信息
        private void GetWindowStates()
        {

            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //正在录像
            if (NVSSDK.NetClient_GetCaptureStatus(uiConID) == 1)
            {
                //btnRecord.Text = "Stop";
            }
            Int32 iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;
            Int32 iComPortCounts = 2;
            Int32 iComPortEnabledStatus = 0;

            //获得前端设备的串口号个数
            NVSSDK.NetClient_GetComPortCounts(iLogonID, ref iComPortCounts, ref iComPortEnabledStatus);

        }

        //更新窗口对应的状态信息
        private void SetWindowStates()
        {
            //正在播放时，设置窗口对应的状态信息；否则，清空窗口对应的状态信息
            if (NVSSDK.NetClient_GetPlayingStatus(m_conState[m_iCurrentFrame].m_uiConID) == SDKConstMsg.PLAYER_PLAYING)
            {
                GetWindowStates();
            }
            else
            {
                InitWindowStates();
            }
        }

        //WCM_VIDEO_DISCONNECT消息处理函数
        private void VideoDisconnect(UInt32 _uiConID)
        {
            bool isCurrentFrame = false;

            //视频被强制断开后，刷新对应窗口显示
            if (m_conState[0].m_uiConID == _uiConID)
            {
                //停止一路视频接收
                NVSSDK.NetClient_StopRecv(_uiConID);
                m_conState[m_iCurrentFrame].m_iChannelNO = -1;
                m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;
                m_video.Invalidate(true);

            }


            //如果是当前选中窗口，则更新其状态显示信息
            if (isCurrentFrame == true)
            {
                GetWindowStates();
            }
        }

        //WCM_ERR_ORDER消息处理函数
        private void NetDisconnect(string _strIP)
        {
            string strMSG = "连接到网络视频服务器";
            strMSG += _strIP;
            strMSG += "的网络意外断开！";
            MessageBox.Show(strMSG);
        }

        //WCM_ERR_DATANET消息处理函数
        private void NetDataError(string _strIP)
        {
            string strMSG = "网络视频服务器";
            strMSG += _strIP;
            strMSG += "的连接数达到最大！";
            MessageBox.Show(strMSG);
        }

        //WCM_RECORD_ERR消息处理函数
        private void RecordError(UInt32 _uiConID)
        {
            bool isCurrentFrame = false;
            //连接ID为_uiCon的窗口停止录像
            if (m_conState[0].m_uiConID == _uiConID)
            {
                //停止将收到的数据写入文件
                NVSSDK.NetClient_StopCaptureFile(_uiConID);

                isCurrentFrame = true;
            }

            //如果当前窗口录像错误，则更新录像按钮的Caption为Record
            if (isCurrentFrame == true)
            {
                //btnRecord.Text = "Record";
            }
            MessageBox.Show("Record error !");
        }
        //重写消息处理函数，以处理自定义消息
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            //WM_MAIN_MESSAGE为自定义的系统消息
            if (m.Msg == SDKConstMsg.WM_MAIN_MESSAGE)
            {
                //自定义消息处理函数
                //OnMainMessage(m.WParam, m.LParam);
                OnMessagePro(m.WParam, m.LParam);
                //this.Notify(m.WParam, m.LParam);
                var dd = "";
            }

            //默认消息处理函数
            base.DefWndProc(ref m);
        }
        public void OnMessagePro(IntPtr wParam, IntPtr lParam)
        {
            //wParam的低16位是消息的类型；
            int iMsgType = wParam.ToInt32() & 0xFFFF;
            //lParam，网络视频服务器NVS的信息结构体NVS_IPAndID地址
            //Marshal.PtrToStructure函数将Intptr地址转化为结构体
            //NVS_IPAndID  ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

            switch (iMsgType)
            {
                //登陆状态消息 
                //param1 登陆IP
                //param2 登陆ID
                //param3 登陆状态
                case 29:
                    {
                        MessageBox.Show(" Download interrupt");
                        break;
                    }
                case SDKConstMsg.WCM_LOGON_NOTIFY:
                    {
                        NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

                        int i = wParam.ToInt32();
                        LogonNotify(ipAndID.m_pIP.ToCharArray(), ipAndID.m_pID, wParam.ToInt32() >> 16);
                        break;
                    }


                //视频头消息，当收到视频头时产生。
                //lParam，网络视频服务器NVS的信息结构体NVS_IPAndID地址；
                //wParamHi低8位表示通道号；
                //wParamHi高8位表示码流类型；
                case SDKConstMsg.WCM_VIDEO_HEAD:
                    VideoArrive();
                    break;

                //视频被强制断开消息，当前的视频连接被代理强制断开后产生该消息。
                //param1,视频连接ID号
                case SDKConstMsg.WCM_VIDEO_DISCONNECT:
                    VideoDisconnect((UInt32)lParam.ToInt32());
                    break;

                //网络命令断开消息，当网络连接意外断开时产生。
                //param1，网络视频服务器的IP地址；
                case SDKConstMsg.WCM_ERR_ORDER:
                    {
                        NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

                        NetDisconnect(ipAndID.m_pIP);
                        break;
                    }


                //网络数据错误，当连接超过最大数后将产生此消息。
                //param1，网络视频服务器的IP地址；
                case SDKConstMsg.WCM_ERR_DATANET:
                    {
                        NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

                        NetDataError(ipAndID.m_pIP);
                        break;
                    }

                //录像错误消息，当视频录像出现错误时产生。
                //param1，视频连接ID号
                case SDKConstMsg.WCM_RECORD_ERR:
                    RecordError((UInt32)lParam.ToInt32());
                    break;

                default:
                    break;
            }
        }
        //WCM_LOGON_NOTIFY消息处理函数
        private void LogonNotify(char[] _cIP, string _strID, int iLogonState)
        {
            //iLogonState 登陆状态
            switch (iLogonState)
            {
                case SDKConstMsg.LOGON_SUCCESS://登陆成功显示设备ID号
                    {
                        m_cltInfo.m_cRemoteIP = _cIP;
                        _isPlay = Connect();
                        break;
                    }
                case SDKConstMsg.LOGON_FAILED:
                case SDKConstMsg.LOGON_ING:
                case SDKConstMsg.LOGON_RETRY:
                case SDKConstMsg.NOT_LOGON:
                case SDKConstMsg.LOGON_TIMEOUT://登陆失败
                    {
                        m_cltInfo.m_iServerID = -1;
                        MessageBox.Show("Logon failed!");
                        break;
                    }
            }
        }
        #endregion


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
        public object LogonAndPlay(object obj)
        {
            try
            {
                Ip = obj.ToString();
                return Logon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }

        }
        public object Play(object objIp)
        {
            int iRet;
            try
            {
                if (IsPlay)
                {
                    //停止接受视频数据
                    iRet = NVSSDK.NetClient_StopCaptureData(m_conState[m_iCurrentFrame].m_uiConID);

                    //停止播放某路视频
                    iRet = NVSSDK.NetClient_StopPlay(m_conState[m_iCurrentFrame].m_uiConID);
                    m_video.Invalidate(true);
                    IsPlay = false;
                }
                else
                {
                    RECT rect = new RECT();

                    //开始播放视频
                    iRet = NVSSDK.NetClient_StartPlay
                    (
                        m_conState[m_iCurrentFrame].m_uiConID,
                        m_video.Handle,
                        rect,
                        0
                    );
                    if (iRet == 0)
                    {
                        IsPlay = true ;
                    }
                    else
                    {
                        Ip = objIp.ToString();
                        Logon();
                        //Connect();
                    }
                }
                return IsPlay;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }

        }
        public void Start(object obj)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ShowTime));
                t.Start(obj.ToString() + "，线程：" + i.ToString() + "," + DateTime.Now.ToString());
            }
        }
        void ShowTime(object obj)
        {
            if (this.InvokeRequired)
            {
                D d = new D(DelegateShowTime);
                this.Invoke(d, obj);
            }
            else
            {
                this.Text = obj.ToString();
            }


        }


        void DelegateShowTime(object obj)
        {
            this.Text = obj.ToString();
        }

        private string Logon()
        {
            string strUser = "admin";
            string strPwd = "admin";

            string strProxy = "";
            string strProxyID = "";

            int iPort = 3000;
            int iRet;

            //登录指定的网络视频服务器
            iRet = NVSSDK.NetClient_Logon(strProxy, Ip, strUser, strPwd, strProxyID, iPort);
            if (iRet < 0)
            {
                m_cltInfo.m_iServerID = -1;
                MessageBox.Show("Logon failed !");
                return "error";
            }
            else
            {
                m_cltInfo.m_iServerID = iRet;
            }
            return "error";
        }
        private bool Connect()
        {
            m_cltInfo.m_iChannelNo = 0;
            m_cltInfo.m_iNetMode = 1;
            m_cltInfo.m_iStreamNO = 0;

            m_cltInfo.m_cNetFile = new char[255];
            m_cltInfo.m_cRemoteIP = new char[16];
            Array.Copy(Ip.ToCharArray(), m_cltInfo.m_cRemoteIP, Ip.Length);
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //获得当前窗口对应的视频播放状态
            int iRet = NVSSDK.NetClient_GetPlayingStatus(uiConID);

            //如果正在播放视频，不进行连接操作
            if (iRet != SDKConstMsg.PLAYER_PLAYING)
            {
                int iChannelNum = 0;

                //获得当前窗口连接的网络视频服务器最大通道数
                NVSSDK.NetClient_GetChannelNum(m_cltInfo.m_iServerID, ref iChannelNum);

                //判断是否超过最大通道号
                if (m_cltInfo.m_iChannelNo >= iChannelNum)
                {
                    MessageBox.Show("Max Channel is " + iChannelNum);
                    return false;
                }
                //开始接收一路视频数据	
                iRet = NVSSDK.NetClient_StartRecv(ref uiConID, ref m_cltInfo, null);

                //操作失败，清除结构体m_conState的信息
                if (iRet < 0)
                {
                    m_conState[m_iCurrentFrame].m_iLogonID = -1;
                    m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;
                    m_conState[m_iCurrentFrame].m_iChannelNO = -1;
                    MessageBox.Show("Connect failed !");
                    return false;
                }
                //操作成功，更新结构体m_conState的信息
                m_conState[m_iCurrentFrame].m_iLogonID = m_cltInfo.m_iServerID;
                m_conState[m_iCurrentFrame].m_iChannelNO = m_cltInfo.m_iChannelNo;
                m_conState[m_iCurrentFrame].m_uiConID = uiConID;
                m_conState[m_iCurrentFrame].m_iStreamNO = m_cltInfo.m_iStreamNO;

                //开始导出收到的数据
                NVSSDK.NetClient_StartCaptureData(uiConID);
                if (iRet == 1)
                {
                    RECT rect = new RECT();

                    //开始播放某路视频
                    var iplay = NVSSDK.NetClient_StartPlay(uiConID, m_video.Handle, rect, 0);

                    GetWindowStates();

                    return true;
                }

            }
            {
                return true;
            }
            return false;
        }

        private bool _isPlay;

        public bool IsPlay
        {
            get { return _isPlay; }
            private set { _isPlay = value; }
        }
    }
}
