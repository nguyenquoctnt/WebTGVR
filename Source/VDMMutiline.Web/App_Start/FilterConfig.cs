﻿using VDMMutiline.Core.Attribute;
using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CTCAuthorize());
        }
    }
}
