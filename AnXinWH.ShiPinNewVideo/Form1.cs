using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AnXinWH.ShiPinNewVideo
{
    public partial class Form1 : Form
    {

        IntPtr hPreView = IntPtr.Zero;
        IntPtr hLogin = IntPtr.Zero;
        IntPtr _currPlayfile = IntPtr.Zero;
        IntPtr _hfile = IntPtr.Zero;
        TMCC.tmVideoInCfg_t videoIn = new TMCC.tmVideoInCfg_t();

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            initForm();
            initVideo();

        }

        private void initForm()
        {
            //throw new NotImplementedException();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Text = DateTime.Now.AddHours(-7).ToString();

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";

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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.button1.Enabled = false;

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
                _sss = new Dictionary<string, TMCC.tmFindFileCfg_t>();
                listBox1.Items.Clear();


                TMCC.tmFindConditionCfg_t ConditionCfg = new TMCC.tmFindConditionCfg_t();
                TMCC.tmFindFileCfg_t FileCfg = new TMCC.tmFindFileCfg_t();


                var startTime = dateTimePicker1.Value;
                var endTime = dateTimePicker2.Value;

                FileCfg.dwSize = (UInt32)Marshal.SizeOf(FileCfg);

                ConditionCfg.dwSize = (UInt32)Marshal.SizeOf(ConditionCfg);
                ConditionCfg.byChannel = 0;		//通道
                ConditionCfg.byFileType = 0xFF;		//录像文件类型
                ConditionCfg.bySearchImage = 0;
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


                IntPtr p1 = Marshal.AllocHGlobal(Marshal.SizeOf(ConditionCfg));
                Marshal.StructureToPtr(ConditionCfg, p1, false);

                IntPtr p2 = Marshal.AllocHGlobal(Marshal.SizeOf(FileCfg));
                Marshal.StructureToPtr(FileCfg, p2, true);


                _hfile = TMCC.TMCC_FindFirstFile(hLogin, p1, p2);

                TMCC.tmFindFileCfg_t anotherP = (TMCC.tmFindFileCfg_t)Marshal.PtrToStructure(p2, typeof(TMCC.tmFindFileCfg_t));


                _sss.Add(anotherP.sFileName, anotherP);
                listBox1.Items.Add(anotherP.sFileName);

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

                    _sss.Add(anotherP.sFileName, anotherP);
                    listBox1.Items.Add(anotherP.sFileName);

                } while (true);
                TMCC.TMCC_FindCloseFile(_hfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw ex;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                this.button1.Enabled = true;
            }
        }

        public Dictionary<string, AnXinWH.ShiPinNewVideo.TMCC.tmFindFileCfg_t> _sss { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {

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
        void playOldFileListName(string name)
        {
            try
            {
                if (_sss.ContainsKey(name))
                {

                    closeAll();

                    var anotherP = _sss[name];

                    TMCC.tmPlayConditionCfg_t struCond = new TMCC.tmPlayConditionCfg_t();

                    struCond.init();
                    struCond.dwSize = (UInt32)Marshal.SizeOf(struCond);

                    struCond.byChannel = 0;
                    struCond.time.struStartTime = anotherP.struStartTime;
                    struCond.time.struStopTime = anotherP.struStopTime;

                    IntPtr p4 = Marshal.AllocHGlobal(Marshal.SizeOf(struCond));
                    Marshal.StructureToPtr(struCond, p4, false);

                    IntPtr p6 = TMCC.TMCC_OpenFile(hLogin, p4, this.pictureBox1.Handle);

                    _currPlayfile = p6;

                    TMCC.tmPlayControlCfg_t cfg = new TMCC.tmPlayControlCfg_t();

                    cfg.dwSize = (UInt32)Marshal.SizeOf(cfg);
                    cfg.dwCommand = 0;


                    var iflag = TMCC.TMCC_ControlFile(p6, ref cfg);

                    if (iflag == 0)
                    {
                        lbl0Msg.Text = "播放视频成功,时间:" + name;
                    }
                    else
                    {
                        lbl0Msg.Text = "播放视频失败,时间:" + name + "请再次尝试.谢谢.";
                    }

                }
                else
                {
                    MessageBox.Show(name + "不存在.请选择正确的文件名。");
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
                    _currPlayfile = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }


        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var tmpname = listBox1.SelectedItem.ToString();
                lbl0Msg.Text = "回放视频中." + tmpname;
                playOldFileListName(tmpname);
            }
        }

        public string tmpmsg { get; set; }


        TMCC.StreamCallback streamback = null;
        TMCC.AvFrameCallback frameback = null;
        TMCC.TMCC_CONNECT_CALLBACK CALLBACKs = null;
        TMCC.fnStreamReadCallBackDelegate fnStreamReadCallBackDelegate = null;

        int iTemp = 0;
        public void StreamDataCallBack(IntPtr hTmCC, ref TMCC.tmRealStreamInfo_t pStreamInfo, IntPtr context)
        {
            if (pStreamInfo.byFrameType == 0)//视频
            {
                iTemp++;
                String str = "[" + Convert.ToString(pStreamInfo.iSamplesPerSec) + "x" + Convert.ToString(pStreamInfo.iBitsPerSample) + "] ";
                var strTitle = "ClientDemo C# " + str;
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
        int testpos = 100;
        private void playOnMini(int pos)
        {
            var tmpflag = -1;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (_currPlayfile != null)
                {
                    tmpflag = TMCC.Avdec_SetCurrentTime(_currPlayfile, TMCC.PLAY_CONTROL_SEEKTIME,pos);

                    if (tmpflag==0)
                    {
                       // MessageBox.Show("Test");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {

            int tmpos = 5 * 1000;
            playOnMini(tmpos);
        }

    }
}
