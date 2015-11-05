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
    [Guid("3EC03FF2-2A84-4C78-AA24-74DC207E125D")]
    public partial class ISECNewVideoNow : UserControl, IObjectSafety, IDisposable
    {
        #region att
        IntPtr hPreView = IntPtr.Zero;
        IntPtr hLogin = IntPtr.Zero;
        IntPtr _currPlayfile = IntPtr.Zero;
        IntPtr _hfile = IntPtr.Zero;

        public string ReceiveNo { get; private set; }
        //collback
        public string tmpmsg { get; set; }

        TMCC.StreamCallback streamback = null;
        TMCC.AvFrameCallback frameback = null;
        int iTemp = 0;

        #endregion
        public ISECNewVideoNow()
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

                startPlayNow();
            }
        }
        void startPlayNow()
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

        void closeAll()
        {
            try
            {

                if (hPreView != null)
                {
                    TMCC.TMCC_CloseStream(hPreView);
                    TMCC.TMCC_ClearDisplay(hPreView);
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
    }
}
