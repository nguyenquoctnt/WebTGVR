using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebStart;

[assembly: PreApplicationStartMethod(typeof(VDMMutiline.SeachAndBook.WebStart), "Register")]

namespace VDMMutiline.SeachAndBook
{
    public class WebStart
    {
        public static void Register()
        {
            List<Config> webStartConfigs = (from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsSubclassOf(typeof (Config)) select (Config) Activator.CreateInstance(t)).ToList();
            WebStartHttpModule.ConfigTasks = webStartConfigs;
            HttpApplication.RegisterModule(typeof(WebStartHttpModule));
           // RouteTable.Routes.MapHubs();
        }
    }
}