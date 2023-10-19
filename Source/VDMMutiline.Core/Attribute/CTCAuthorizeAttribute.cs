using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
namespace VDMMutiline.Core.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CTCAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            //if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    base.HandleUnauthorizedRequest(filterContext);
            //}
            //else
            //{
            //    filterContext.Result = new RedirectToRouteResult(new
            //            RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
            //}
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonResult result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json",
                    Data = new { IsAuthenticated = false, HasPermision = false, Message = "Bạn không có quyền truy cập." }
                };

                if (HttpContext.Current.Session["CurrentUser"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                             RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                }
                else
                    filterContext.Result = result;

            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
                string str = filterContext.RouteData.Values["controller"].ToString().ToLower();
                string str2 = filterContext.RouteData.Values["action"].ToString().ToLower();
                if (("account".Equals(str) && "logoff".Equals(str2)) || ("home".Equals(str) && "index".Equals(str2)))
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                    {
                        if (HttpContext.Current.Session["CurrentUser"] == null)
                        {
                            filterContext.Result = new RedirectToRouteResult(new
                                     RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                        }
                        else
                            base.HandleUnauthorizedRequest(filterContext);

                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                                RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                    }
                    //RouteValueDictionary routeValues = new RouteValueDictionary();
                    //routeValues.Add("controller", "UnAuthorize");
                    //routeValues.Add("action", "Index");
                    //filterContext.Result = new RedirectToRouteResult(routeValues);
                }
            }
        }
    }
}
