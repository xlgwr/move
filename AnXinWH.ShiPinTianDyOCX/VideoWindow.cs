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

    [Guid("A60D3D0D-B894-47D2-AE08-35627F0CE5E7")]
    public partial class VideoWindow : UserControl, IObjectSafety
    {
        int g_iLogonID = -1;
        UInt32 g_uConnID = UInt32.MaxValue;

        public VideoWindow()
        {
            InitializeComponent();
            this.Resize += VideoWindow_Resize;
        }

        void VideoWindow_Resize(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            pnlVideo.Left = picVideo.Left + 1;
            pnlVideo.Top = picVideo.Top + 1;
            pnlVideo.Width = picVideo.Width - 2;
            pnlVideo.Height = picVideo.Height - 2;

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


        void StartPlay(UInt32 _uConnID)
        {
            RECT rc = new RECT();
            NVSSDK.NetClient_StopPlay(_uConnID);//停止播放视频
            int iRet = NVSSDK.NetClient_StartPlay(_uConnID, this.Handle, rc, 0);//开始播放视频
            if (iRet >= 0)
            {
                MessageBox.Show("StartPlay success!\n");
            }
            else
            {
                MessageBox.Show("StartPlay failed!\n");
            }
        }
        UInt32 StartRecv(int _iLogonID)
        {
            UInt32 uConnID = UInt32.MaxValue;
            CLIENTINFO clientinfo = new CLIENTINFO();
            clientinfo.m_iNetMode = 1;// NETMODE_TCP;
            clientinfo.m_iServerID = _iLogonID;
            clientinfo.m_iChannelNo = 0;//预览通道号
            clientinfo.m_iStreamNO = 0;// MAIN_STREAM;
            int iRet = NVSSDK.NetClient_StartRecv(ref uConnID, ref clientinfo, null);//建立视频连接
            if (iRet == 0)
            {
                MessageBox.Show("StartRecv success!\n");
            }
            else
            {
                MessageBox.Show("StartRecv failed!\n");
            }

            return uConnID;
        }
        public static ushort LOWORD(uint value) { return (ushort)(value & 0xFFFF); }
        public static ushort HIWORD(uint value) { return (ushort)(value >> 16); } 
        public static byte LOWBYTE(ushort value) { return (byte)(value & 0xFF); } 
        public static byte HIGHBYTE(ushort value) { return (byte)(value >> 8); }

        void Notify_Main(int _iLogonID, int _iWparam, int _iLParam, int _iUserData)
        {
            int iMsgType = _iWparam & 0xFFFF;
            switch (iMsgType)
            {
                case SDKConstMsg.WCM_LOGON_NOTIFY:
                    {
                        MessageBox.Show("WCM_LOGON_NOTIFY\n");
                        if (_iLParam == SDKConstMsg.LOGON_SUCCESS)
                        {
                            MessageBox.Show("Logon success!\n");
                            g_uConnID = StartRecv(_iLogonID);//连接视频
                        }
                        else
                        {
                            MessageBox.Show("Logon failed!\n");
                        }
                    }
                    break;
                case SDKConstMsg.WCM_VIDEO_HEAD:
                    {
                        MessageBox.Show("WCM_VIDEO_HEAD\n");
                        StartPlay(g_uConnID);//播放视频
                    }
                    break;
                default:
                    break;
            }
        }

        int main()
        {
           
            return 0;

        }
    }
}
