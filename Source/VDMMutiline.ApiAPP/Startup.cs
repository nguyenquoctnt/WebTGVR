using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using VDMMutiline.Core;
using VDMMutiline.Core.Models;

[assembly: OwinStartup(typeof(VDMMutiline.ApiAPP.Startup))]
namespace VDMMutiline.ApiAPP
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(VDMDbContext.Create);
            app.CreatePerOwinContext<VDMUserManager>(VDMUserManager.Create);
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/VDMMutiline/login"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(120),
                AllowInsecureHttp = true,
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
            }
            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var userManager = HttpContext.Current.GetOwinContext().GetUserManager<VDMUserManager>();
                //   UserManager<IdentityUser> userManager = context.OwinContext.GetUserManager<UserManager<IdentityUser>>();
                VDMUser user;
                try
                {

                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch (Exception ex)
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }
                if (user != null)
                {
                    ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                                                            user,
                                                            DefaultAuthenticationTypes.ExternalBearer);
                    identity.AddClaim(new Claim("UserId", user.Id.ToString()));   //<= The UserId add here as claim
                    identity.AddClaim(new Claim("UserName", user.UserName));  //<= The UserName add here as claim
                    var ticket = new AuthenticationTicket(identity, properties);
                    context.Validated(ticket);

                    context.Validated(identity);
                    context.Request.Context.Authentication.SignIn(identity);
                   

                }
                else
                {
                    context.SetError("invalid_grant", "Invalid UserId or password'");
                    context.Rejected();
                }
            }
        }


       

    
        public static AuthenticationProperties CreateProperties(string userName, string Id)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }, { "UserId", Id }
            };
            return new AuthenticationProperties(data);
        }
        //private static VDMUserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        //{
        //    var userStore = new UserStore<IdentityUser>(context.Get<VDMDbContext>());
        //    var owinManager = new UserManager<IdentityUser>(userStore);
        //    return owinManager;
        //}

    }
}