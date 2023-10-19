using VDMMutiline.Core.Mvc.ViewEngines;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VDMMutiline.Core.MobileViewEnginesStarter), "Start")]
namespace VDMMutiline.Core
{
    public static class MobileViewEnginesStarter
    {
        public static void Start()
        {
            ViewEngines.Engines.Insert(0, new MobileCapableRazorViewEngine());
        }
    }
}
