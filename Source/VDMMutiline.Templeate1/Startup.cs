using System;
using VDMMutiline.Core;
using VDMMutiline.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(VDMMutiline.Templeate1.Startup))]
namespace VDMMutiline.Templeate1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // app.MapSignalR();
            ConfigureAuth(app);
        }
        public void ConfigureAuth(IAppBuilder app)
        {

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(VDMDbContext.Create);
            app.CreatePerOwinContext<VDMUserManager>(VDMUserManager.Create);
            app.CreatePerOwinContext<ActSignInManager>(ActSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<VDMUserManager, VDMUser, int>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: id => id.GetUserId<int>())
                },
                CookieName = "CTCAuthentication",
                //  CookieDomain = System.Configuration.ConfigurationManager.AppSettings["Domain"],
                ExpireTimeSpan = new TimeSpan(0, 20, 10)

            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: System.Configuration.ConfigurationManager.AppSettings["FbAppKey"],
            //   appSecret: System.Configuration.ConfigurationManager.AppSettings["FbAppSecrec"]);

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = System.Configuration.ConfigurationManager.AppSettings["GoogleAppKey"],
            //    ClientSecret = System.Configuration.ConfigurationManager.AppSettings["GoogleAppSecrec"]
            //});
        }
    }
}
