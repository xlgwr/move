using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnXinWH.ShiPinNewVideoOCX
{
    public partial class FullPlay : Form
    {
        ISECNewVideoA _a;
        public FullPlay()
        {
            InitializeComponent();
        }
        public FullPlay(ISECNewVideoA a)
        {

            InitializeComponent();
            _a = a;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            _a.m_IsFullScreen = false;
            _a.closeAll();
            this.Close();
            _a.SetFormFullScreen(_a.m_IsFullScreen);
            if (_a._playNow)
            {
                _a.btn0Now_Click(null, null);
            }


        }
    }
}
