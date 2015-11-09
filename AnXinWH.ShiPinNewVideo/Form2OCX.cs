using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnXinWH.ShiPinNewVideo
{
    public partial class Form2OCX : Form
    {
        public Form2OCX()
        {
            InitializeComponent();
        }

        private void Form2OCX_Load(object sender, EventArgs e)
        {
            isecNewVideo1.jsSetTimeOut(DateTime.Now.AddDays(-1), DateTime.Now);
            isecNewVideo1.jsSetTimeShelf(DateTime.Now.AddDays(-1), DateTime.Now);
            isecNewVideo1.jsSetTimeIn(DateTime.Now.AddDays(-1), DateTime.Now);

        }
    }
}
