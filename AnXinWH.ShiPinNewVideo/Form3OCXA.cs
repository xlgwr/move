using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnXinWH.ShiPinNewVideo
{
    public partial class Form3OCXA : Form
    {
        public Form3OCXA()
        {
            InitializeComponent();
        }

        private void Form3OCXA_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            //isecNewVideoA1.jsSetTimeStockIn(DateTime.Now.AddDays(-1),20,0);
        }
    }
}
