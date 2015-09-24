using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnXinWH.ShiPin
{
    public class comm
    {
        public static configHost getConfigHost()
        {
            try
            {
                var tmpconfig = new configHost();

                tmpconfig.cmsip = System.Configuration.ConfigurationManager.AppSettings["cmsip"].ToString();
                tmpconfig.cmsPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["cmsPort"]);
                tmpconfig.userName = System.Configuration.ConfigurationManager.AppSettings["userName"].ToString();
                tmpconfig.pswd = System.Configuration.ConfigurationManager.AppSettings["pswd"].ToString();


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
