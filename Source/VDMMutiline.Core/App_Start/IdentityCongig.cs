using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using VDMMutiline.Core.Models;

namespace VDMMutiline.Core
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await ConfigSMTPasync(message);
        }

        // send email via smtp service
        private async Task ConfigSMTPasync(IdentityMessage message)
        {
            // Create the message:
            // Plug in your email service here to send an email.
            var host = System.Configuration.ConfigurationManager.AppSettings["EmailSmtpAddress"];
            var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailSmtpPort"]);
            var ssl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EmailEnableSSL"]);
            var emailsend = System.Configuration.ConfigurationManager.AppSettings["EmailFromAddress"];
            var emailaccount = System.Configuration.ConfigurationManager.AppSettings["EmailSmtpUserName"];
            var passwordsend = System.Configuration.ConfigurationManager.AppSettings["EmailSmtpPassword"];

            // Configure the client:
            SmtpClient client = new SmtpClient(host)
            {
                Port = port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };


            // Creatte the credentials:
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(emailaccount, passwordsend);
            client.EnableSsl = ssl;
            client.Credentials = credentials;

            var mail = new MailMessage(emailsend, message.Destination)
            {
                Subject = message.Subject,
                Body = message.Body
            };
            mail.IsBodyHtml = true;
            await client.SendMailAsync(mail);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
    public class VDMUserManager : UserManager<VDMUser, int>
    {
        public VDMUserManager(IUserStore<VDMUser, int> store)
            : base(store)
        {
        }

        public static VDMUserManager Create(IdentityFactoryOptions<VDMUserManager> options, IOwinContext context)
        {
            var manager = new VDMUserManager(new UserStore(context.Get<VDMDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<VDMUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,

            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                // RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<VDMUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<VDMUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<VDMUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ActSignInManager : SignInManager<VDMUser, int>
    {
        public ActSignInManager(VDMUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(VDMUser user)
        {
            return user.GenerateUserIdentityAsync((VDMUserManager)UserManager);
        }

        public static ActSignInManager Create(IdentityFactoryOptions<ActSignInManager> options, IOwinContext context)
        {
            return new ActSignInManager(context.GetUserManager<VDMUserManager>(), context.Authentication);
        }
        //public override Task<SignInStatus> PasswordSignIn(string userName, string password, bool isPersistent, bool shouldLockout)
        //{
        //    var user = await UserManager.FindByNameAsync(userName);
        //    var result = base.PasswordSignInAsync(user.Email, password, isPersistent, shouldLockout);
        //    return result;
        //}
        //public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        //{
        //    var user = UserManager.FindByName(userName);
        //    return base.PasswordSignInAsync(user.Email, password, isPersistent, shouldLockout);
        //}
    }

    public enum UserStatus
    {
        Online = 0,
        Busy = 1,
        Invisible = 2
    }
}
