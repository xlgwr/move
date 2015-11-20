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
            //XH:卸货,IST:入库,CZ:称重,SST:上架,CY:抽样,OST:出库,TT:验证
            isecNewVideoA1.SetReceiptNo("仓单号：A123456,RFID:814226");
            isecNewVideoA1.jsStockXH("卸货视频1", DateTime.Now.AddDays(-1), 0);
            isecNewVideoA1.jsStockIn("入库视频2", DateTime.Now.AddHours(-2), 0);
            isecNewVideoA1.jsStockCZ("称重视频3", DateTime.Now.AddHours(-3), 0);
            isecNewVideoA1.jsStockShelf("上架视频4", DateTime.Now.AddHours(-4), 0);
            isecNewVideoA1.jsStockCY("抽样视频5", DateTime.Now.AddHours(-5), 0);
            isecNewVideoA1.jsStockOut("出库视频6", DateTime.Now.AddHours(-6), 0);
            isecNewVideoA1.jsStockTT("验证视频7", DateTime.Now.AddHours(-7), 0);

            var tmpAlarm = "";
            for (int i = 0; i < 100; i++)
            {
                tmpAlarm += i + "报警" + "," + DateTime.Now.AddHours(-i).ToString("yyyy-MM-dd HH:mm:ss") + ",20,0|";
            }
            isecNewVideoA1.jsStockAlarm(tmpAlarm);
        }

        private void isecNewVideoA1_Load(object sender, EventArgs e)
        {

        }
    }
}
