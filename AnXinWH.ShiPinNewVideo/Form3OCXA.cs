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
            this.FormClosing += Form3OCXA_FormClosing;
            
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
        }

        void Form3OCXA_FormClosing(object sender, FormClosingEventArgs e)
        {
            //throw new NotImplementedException();
            isecNewVideoA1.Dispose();
        }

        private void Form3OCXA_Load(object sender, EventArgs e)
        {

            isecNewVideoA1.jsStockIn("入库", DateTime.Now.AddDays(-1), 20, 0);
            isecNewVideoA1.jsStockShelf("上架", DateTime.Now.AddHours(-2), 20, 0);
            isecNewVideoA1.jsStockOut("出库", DateTime.Now.AddHours(-1), 20, 0);
        }
    }
}
