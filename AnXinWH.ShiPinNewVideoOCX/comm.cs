using System;
using System.Collections.Generic;
using System.Text;

namespace AnXinWH.ShiPinNewVideoOCX
{
    public class comm
    {
        public static configHost getConfigHost()
        {
            try
            {
                var tmpconfig = new configHost();

                tmpconfig.cmsip = "192.168.1.4";// System.Configuration.ConfigurationManager.AppSettings["cmsip"].ToString();
                tmpconfig.cmsPort = 6002;//int.Parse(System.Configuration.ConfigurationManager.AppSettings["cmsPort"]);
                tmpconfig.userName = "system";// System.Configuration.ConfigurationManager.AppSettings["userName"].ToString();
                tmpconfig.pswd = "system";// System.Configuration.ConfigurationManager.AppSettings["pswd"].ToString();

                tmpconfig.byChannel = 0;
                tmpconfig.byStream = 0;

                tmpconfig.ValidateType = 0;
                tmpconfig.UserMacAddr = "";
                tmpconfig.UserUsbKey = "";
                tmpconfig.Bound = 0;


                return tmpconfig;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    public class configHost
    {
        public string cmsip { get; set; }
        public int cmsPort { get; set; }
        public string userName { get; set; }
        public string pswd { get; set; }
        public int ValidateType { get; set; }
        public string UserMacAddr { get; set; }
        public string UserUsbKey { get; set; }
        public int Bound { get; set; }


        public byte byChannel { get; set; }
        public byte byStream { get; set; }

        
    }
}
