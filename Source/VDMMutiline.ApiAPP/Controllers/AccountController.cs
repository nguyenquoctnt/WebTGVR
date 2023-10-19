using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using VDMMutiline.Core;
using VDMMutiline.Core.Models;

namespace VDMMutiline.ApiAPP.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        [HttpPost]
        [Route("authen/login")]
        public IHttpActionResult Login(Request.Login login)
        {
            try
            {
           

                string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                string baseAddress = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                Token token = new Token();
                using (var client = new HttpClient())
                {
                    var form = new Dictionary<string, string>               {
                       {"grant_type", "password"},
                       {"username", login.Username},
                       {"password", login.Pass},
                    };
                    var tokenResponse = client.PostAsync(baseAddress + "/VDMMutiline/login", new FormUrlEncodedContent(form)).Result;
                    token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;



                    var identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;

                    var test = ClaimsPrincipal.Current.Identities;
                 var a=   HttpContext.Current.User.Identity.Name;

                    string userName;
                    string userId;
                    if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                    {
                        userName = HttpContext.Current.User.Identity.Name;
                        userId = HttpContext.Current.User.Identity.GetUserId();
                    }

                    var userManager = HttpContext.Current.GetOwinContext().GetUserManager<VDMUserManager>();
                   // var obj = userManager.FindByName()

                    var act = (VDMUser)HttpContext.Current.Session["CurrentUser"];
                    if(act!=null)
                    {
                        //act.Token = token.AccessToken;
                        return Json(new { Success = true, UserInfo = act,Token= token.AccessToken });
                    }    
                    else
                        return Json(new { Success = false});
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, message = ex.Message });
            }
        }
    }
}
