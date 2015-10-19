using AnXinWH.ShiPin;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnXinWH.ShiPinZXVOCX
{
    public class comm
    {
        public static configHost getConfigHost()
        {
            try
            {
                var tmpconfig = new configHost();

                tmpconfig.cmsip = "192.168.1.26";// System.Configuration.ConfigurationManager.AppSettings["cmsip"].ToString();
                tmpconfig.cmsPort = 8000;//int.Parse(System.Configuration.ConfigurationManager.AppSettings["cmsPort"]);
                tmpconfig.userName = "007";// System.Configuration.ConfigurationManager.AppSettings["userName"].ToString();
                tmpconfig.pswd = "888888";// System.Configuration.ConfigurationManager.AppSettings["pswd"].ToString();


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
}
