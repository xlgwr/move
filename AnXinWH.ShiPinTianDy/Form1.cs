using frameworkApiTianDy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AnXinWH.ShiPinTianDy
{
    public partial class Form1 : Form
    {

        //当前登录状态结构体
        CLIENTINFO m_cltInfo;
        //视频窗口对应的连接状态结构体数组
        CONNECT_STATE[] m_conState;
        //当前视频窗口标记，从0开始
        int m_iCurrentFrame = 0;
        public Form1()
        {
            InitializeComponent();

            //
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Resize += Form1_Resize;
            //initwith
            initwith(groupBox1, m_video);
            m_cltInfo.m_iServerID = -1;
            StartUp();
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


            cboChannel.SelectedIndex = 0;
            cboMode.SelectedIndex = 0;
            cboScreen.SelectedIndex = 1;
            cboStream.SelectedIndex = 0;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            initwith(groupBox1,m_video);
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

        //WCM_VIDEO_HEAD消息处理函数
        private void VideoArrive()
        {
            RECT rect = new RECT();

            //视频到达后开始播放   
            NVSSDK.NetClient_StartPlay(m_conState[m_iCurrentFrame].m_uiConID, m_video.Handle, rect, 0);
            btn3Play.Text = "Stop";

            //修改视频状态信息
            GetWindowStates();
        }

        //设置窗口对应的状态信息
        private void GetWindowStates()
        {
            btn1Logon.Text = "Logoff";
            btn2Connect.Text = "Disconnect";
            btn3Play.Text = "Stop";
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
                        txt1ID.Text = _strID;
                        btn1Logon.Text = "Logoff";


                        break;
                    }
                case SDKConstMsg.LOGON_FAILED:
                case SDKConstMsg.LOGON_ING:
                case SDKConstMsg.LOGON_RETRY:
                case SDKConstMsg.NOT_LOGON:
                case SDKConstMsg.LOGON_TIMEOUT://登陆失败
                    {
                        m_cltInfo.m_iServerID = -1;
                        txt1ID.Text = "";
                        MessageBox.Show("Logon failed!");
                        btn1Logon.Text = "Logon";
                        break;
                    }
            }
        }
        private void btn1Logon_Click(object sender, EventArgs e)
        {
            if (btn1Logon.Text == "Logon")//登陆
            {
                string strProxy = "";
                string strIP = com1Ip.Text;
                string strUser = "admin";
                string strPwd = "admin";
                string strProxyID = "";
                int iPort = 3000;
                int iRet;

                //登录指定的网络视频服务器
                iRet = NVSSDK.NetClient_Logon(strProxy, strIP, strUser, strPwd, strProxyID, iPort);
                if (iRet < 0)
                {
                    m_cltInfo.m_iServerID = -1;
                    MessageBox.Show("Logon failed !");
                    return;
                }
                m_cltInfo.m_iServerID = iRet;
                btn1Logon.Text = "Logoff";
            }
            else //注销
            {
                btn1Logon.Text = "Logon";
                int iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;
                if (iLogonID < 0)//如果当前窗口没有登陆，不进行操作
                {
                    return;
                }

                //注销当前窗口对应的用户登录
                NVSSDK.NetClient_Logoff(iLogonID);
                if (m_cltInfo.m_iServerID == iLogonID)
                {
                    txt1ID.Text = "";
                }
                //更新对应的窗口信息
                if (m_conState[0].m_iLogonID == iLogonID)
                {
                    m_conState[0].m_iLogonID = -1;
                    m_conState[0].m_iChannelNO = -1;
                    m_conState[0].m_uiConID = UInt32.MaxValue;
                    m_video.Invalidate(true);
                }

                //清空当前视频窗口的状态信息
                InitWindowStates();
            }
        }

        private void InitWindowStates()
        {
            btn1Logon.Text = "Logon";
            btn2Connect.Text = "Connect";
            btn3Play.Text = "Play";
        }

        private void btn2Connect_Click(object sender, EventArgs e)
        {
            if (btn2Connect.Text == "Connect")//连接操作
            {
                m_cltInfo.m_iChannelNo = cboChannel.SelectedIndex;
                m_cltInfo.m_iNetMode = cboMode.SelectedIndex + 1;
                m_cltInfo.m_iStreamNO = cboStream.SelectedIndex;

                m_cltInfo.m_cNetFile = new char[255];
                m_cltInfo.m_cRemoteIP = new char[16];
                Array.Copy(com1Ip.Text.ToCharArray(), m_cltInfo.m_cRemoteIP, com1Ip.Text.Length);
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
                        cboChannel.SelectedIndex = iChannelNum - 1;
                        return;
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
                        return;
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
                        NVSSDK.NetClient_StartPlay(uiConID, m_video.Handle, rect, 0);
                        btn3Play.Text = "Stop";
                        GetWindowStates();
                    }
                    btn2Connect.Text = "Disconnect";
                }
            }
            else //断开操作
            {
                NVSSDK.NetClient_StopRecv(m_conState[m_iCurrentFrame].m_uiConID);//停止一路视频接收
                m_conState[m_iCurrentFrame].m_iChannelNO = -1;//修改当前窗口的通道号和连接ID
                m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;
                m_video.Invalidate(true);//刷新当前窗口，并更新其状态信息
                btn2Connect.Text = "Connect";

                //清空当前视频窗口的状态信息
                InitWindowStates();
                btn1Logon.Text = "Logoff";
            }
        }

        private void btn3Play_Click(object sender, EventArgs e)
        {
            //当前窗口没有连接，退出
            if (m_conState[m_iCurrentFrame].m_uiConID == UInt32.MaxValue)
            {
                return;
            }
            string strCaption = btn3Play.Text;
            int iRet;
            if (strCaption == "Play") //显示视频
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
                    btn3Play.Text = "Stop";
                }
            }
            else //停止播放
            {
                //停止接受视频数据
                iRet = NVSSDK.NetClient_StopCaptureData(m_conState[m_iCurrentFrame].m_uiConID);

                //停止播放某路视频
                iRet = NVSSDK.NetClient_StopPlay(m_conState[m_iCurrentFrame].m_uiConID);
                m_video.Invalidate(true);
                btn3Play.Text = "Play";
            }
        }
    }
}
