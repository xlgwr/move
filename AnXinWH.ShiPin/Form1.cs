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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var dd = "";
                var tmpinit = ZxvnmsSDKApi.ZXVNMS_Init();
                var tmpLogin = ZxvnmsSDKApi.ZXVNMS_InitSession("192.168.1.26",
                    8000,
                    "admin",
                    "888888",
                    0,
                    "",
                    "", 0);
                var userid = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetUserID());
                var tmpTest = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetCMSIP());
                var tmpCamera = "";
                if (ZxvnmsSDKApi.ZXVNMS_QueryDevices(ZXVNMS_DevType.CAMERA)==0)
                {
                    while (ZxvnmsSDKApi.ZXVNMS_MoveNext()!=-1)
                    {
                        string camera_id = Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_Camera.device_id));
                        string camera_name=Marshal.PtrToStringAnsi(ZxvnmsSDKApi.ZXVNMS_GetValueStr(ZXVNMS_Camera.device_name));
                        tmpCamera += camera_id + "," + camera_name + "\n\t";

                    }
                }
                MessageBox.Show(userid.ToString());
            }
            catch (Exception ex)
            {
                ZxvnmsSDKApi.ZXVNMS_Free();
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ZxvnmsSDKApi.ZXVNMS_Free();
        }
    }
}
