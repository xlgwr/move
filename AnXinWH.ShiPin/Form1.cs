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
        static List<ZXVNMS_TCamera> _list_tcamera;
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
            //attr
            _list_tcamera = new List<ZXVNMS_TCamera>();
            _userid = "";
            _cmsIP = "";
            _m_curPlayHandle = -1;
            _data = new IntPtr();
            _pUser = new IntPtr();
            //initdevice
            initDevice(comm.getConfigHost());
            //initcomp
            initcomp(comboBox1);

            this.Text = _userid + "," + _cmsIP;
        }
        void initwith()
        {
            gbtop1.Width = this.Width - gbtop1.Left - 20;

            gbtop2.Top = gbtop1.Top + gbtop1.Height;
            gbtop2.Left = gbtop1.Left;
            gbtop2.Width = gbtop1.Width;
            gbtop2.Height = this.Height - gbtop2.Top - 50;

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
                        var tmpCamer = new ZXVNMS_TCamera();
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
            button1.Enabled = false;
            string msg = "正在打开中。。。";
            SetMsg(lbl0Msg, msg);
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!string.IsNullOrEmpty(comboBox1.Text))
                {
                    if (OnPlayVideo(comboBox1.Text.Trim(), pictureBox1.Handle))
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
            if (comboBox1.SelectedItem != null)
            {

                setName(comboBox1, lbl2Name);
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
                        SetMsg(lbl, item.device_name);
                        return;
                    }
                }
                SetMsg(lbl, "");
            }

        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            setName(comboBox1, lbl2Name);
        }
    }
}
