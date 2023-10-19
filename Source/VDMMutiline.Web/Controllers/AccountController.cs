using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using VDMMutiline.Core.UI;
using VDMMutiline.Core.Models;
using VDMMutiline.Core;
using CaptchaMvc.Attributes;
using static VDMMutiline.SharedComponent.Constants.Constant;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class AccountController : BaseController
    {
        private ActSignInManager _signInManager;
        private VDMUserManager _userManager;
        public AccountController()
        {

        }
        public AccountController(VDMUserManager userManager, ActSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        private ActSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ActSignInManager>();
            }

            set
            {
                _signInManager = value;
            }
        }
        private VDMUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<VDMUserManager>();
            }

            set
            {
                _userManager = value;
            }
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult ProfileMember()
        {
            return View(CurrentUser);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProfileMember(VDMUser model, FormCollection collection)
        {

            var userdao = new AspNetUserDao();
            var user = UserManager.FindById(CurrentUser.Id);
            user.DisplayName = model.DisplayName;
            user.PhoneNumber = model.PhoneNumber;
            user.Location = model.Location;
            var iconlink = UploadFile.UpLoadFile(Request.Files["fileAvatar"], "Upload/Profile/");
            if (!string.IsNullOrEmpty(iconlink))
            {
                user.Avatar = iconlink;
            }
            UserManager.Update(user);
            CurrentUser = user;


            return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxLoadListLogin(GridFilterSetting<EventLogInfo> gridFilterSetting)
        {

            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };

            var param = new EventLogParam() { PagingInfo = pagininfo, EventLogFilter = new EventLogFilter() { Search = CurrentUser.UserName } };
            var bizparam = new EventLogBiz() { };

            bizparam.GetAll(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.EventLogInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                WriteLogIn("Sai tài khoản hoặc mật khẩu.", LogType.Error, model.UserName);
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu.");
            }
            else
            {
                var paramasp = new AspNetUsersParam();
                var biz = new AspNetUsersBiz();
                var objcheck = biz.GetInfoByLoginName(model.UserName);
                if (objcheck == null)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else
                {
                    if (objcheck.LockoutEnabled == false)
                    {
                        var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
                        WriteLoglogin(result, model);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                {
                                    var objuser = HttpContext.GetOwinContext().GetUserManager<VDMUserManager>().FindByName(model.UserName);
                                    Session["CurrentUser"] = objuser;
                                    if (objuser.SiteId > 0)
                                    {
                                        return RedirectToLocal(returnUrl);
                                    }
                                    else
                                    {
                                        var groupbyuserparam = new AspNetUserGroupsParam() { userId = objuser.Id };
                                        var bizgroup = new AspNetUserGroupsBiz();
                                        bizgroup.GetAllByusername(groupbyuserparam);
                                        var listidquyen = groupbyuserparam.AspNetUserGroupsInfos.Select(z => z.GroupId).ToList();

                                        if (listidquyen.Count(z => z == 1) > 0 || listidquyen.Count(z => z == 23) > 0)
                                            return RedirectToLocal(returnUrl);
                                        else
                                            return RedirectToAction("selectTempleate");
                                    }

                                }
                            case SignInStatus.LockedOut:
                                return View("Lockout");
                            case SignInStatus.RequiresVerification:
                                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                            case SignInStatus.Failure:
                            default:
                                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu.");
                                break;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn chưa được mở khóa, hãy liên hệ quản trị viên.");
                    }
                }


            }
            ViewBag.erro = "";
            foreach (var item in ModelState.Values)
            {
                foreach (var items in item.Errors)
                {
                    ViewBag.erro += items.ErrorMessage + "</br>";
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdatePassWord(int? Id, string PasswordHash, string PasswordConfirm)
        {
            try
            {
                if (((Id.HasValue && !string.IsNullOrWhiteSpace(PasswordHash)) && !string.IsNullOrWhiteSpace(PasswordConfirm)) && PasswordHash.Equals(PasswordConfirm))
                {
                    PasswordHash = Uri.UnescapeDataString(PasswordHash);
                    PasswordConfirm = Uri.UnescapeDataString(PasswordConfirm);
                    UserManager.RemovePassword<VDMUser, int>(Id.Value);
                    UserManager.AddPassword<VDMUser, int>(Id.Value, PasswordHash);
                    return Json(new { data = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = false }, JsonRequestBehavior.AllowGet);
            }
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }
        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var result = UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
                    if (result.Result.Succeeded)
                    {
                        return Json(new { isSuccess = true, mess = "Password changed!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { isSuccess = false, mess = result.Result.Errors.ToArray() }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            var bizgroup = new AspNetGroupsBiz();
            var paramgroup = new AspNetGroupsParam();
            bizgroup.GetAll(paramgroup);
            ViewBag.Groups = paramgroup.AspNetGroupsInfos;
            return View(model);
        }
        //
        // POST: /Account/Register
        [HttpPost, CaptchaVerify("Sai mã bảo vệ!")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Erro = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (!UserNameExists(model.UserName))
                    {
                        var user = new VDMUser
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            RegisterDate = DateTime.Now,
                            DisplayName = model.DisplayName,
                            Status = UserStatus.Online,
                            PhoneNumber = model.PhoneNumber,
                            Location = model.Location,
                            RegisterIP = IPHelper.GetIPAddress(Request),
                            LockoutEnabled = false,
                            Isdelete = false,
                            SiteId = 1,
                        };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var groupparam = new AspNetUserGroupsParam();
                            var biz = new AspNetUserGroupsBiz();
                            var objuser = HttpContext.GetOwinContext().GetUserManager<VDMUserManager>().FindByName(model.UserName);
                            AspNetUserGroup sitem = new AspNetUserGroup();
                            sitem.GroupId = 18;
                            sitem.UserId = objuser.Id;
                            groupparam.AspNetUserGroups = sitem;
                            biz.Insert(groupparam);
                            // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            //Session["CurrentUser"] = objuser;
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            //string er = "";
                            //foreach (var item in result.Errors)
                            //{
                            //    er += item + "<br>";
                            //}
                            //ViewBag.Erro = er;
                        }
                        AddErrors(result);
                    }
                    else
                    {
                        ViewBag.Erro = "Tên đăng nhập đã tồn tại.";
                        return View(model);
                    }
                }
                catch
                {

                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                string er = "";
                foreach (var item in errors)
                {
                    er += item[0].ErrorMessage + "<br>";
                }
                ViewBag.Erro = er;
            }
            return View(model);

        }
        public bool UserNameExists(string UserName)
        {
            var dao = new AspNetUsersBiz();
            return dao.Checkuser(UserName);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult EmailExists()
        {
            string userName = Request["Email"].ToString();
            var dao = new AspNetUsersBiz();
            var reult = dao.CheckMail(userName);
            return Json(!reult);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult PhoneExists()
        {
            string userName = Request["PhoneNumber"].ToString();
            var dao = new AspNetUsersBiz();
            var reult = dao.CheckPhone(userName);
            return Json(!reult);
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == default(int) || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Khôi phục mật khẩu", "Xin mời bấm vào đây để khôi phục mật khẩu <a href=\"" + callbackUrl + "\">đây</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new VDMUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        public ActionResult LogOff(bool? allow)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        //GET: Account/ListUser
        [AllowAnonymous]
        public ActionResult ListUser(string returnUrl)
        {
            var context = new IdentityDbContext();
            var alluser = context.Users.ToList();
            ViewBag.returnUrl = returnUrl;
            return View(alluser);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ListUser(RegisterViewModel model, string returnUrl)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {

            }
            return View();
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        private JsonResult WriteLoglogin(SignInStatus state, LoginViewModel LoginModel)
        {
            var jsr = new JsonResult();
            switch (state)
            {
                case SignInStatus.LockedOut:
                    WriteLogIn("Tài khoản đã bị khóa.", LogType.Warning, LoginModel.UserName);
                    jsr = Json(new { Success = false, msg = "Tài khoản đã bị khóa." });
                    break;
                case SignInStatus.Failure:
                    WriteLogIn("Sai tài khoản hoặc mật khẩu.", LogType.Error, LoginModel.UserName);
                    jsr = Json(new { Success = false, msg = "Sai tài khoản hoặc mật khẩu." });
                    break;
                default:
                    {
                        // log.Status = true;
                        VDMUser objUser = UserManager.FindByName(LoginModel.UserName);

                        if (objUser != null)
                        {
                            try
                            {
                                objUser.LastLogin = DateTime.Now;
                                UserManager.Update(objUser);
                                WriteLogIn("Đăng nhập thành công.", LogType.Success, LoginModel.UserName);
                            }
                            catch (Exception exception1)
                            {
                                WriteLogIn(exception1.Message, LogType.Error, objUser.UserName);
                            }
                        }

                        jsr = Json(new { Success = true, msg = "Đăng nhập thành công." });
                        break;
                    }

            }
            return jsr;
        }


        #endregion

        #region Select site templeate
        [AllowAnonymous]
        public ActionResult selectTempleate()
        {
            var objuser = HttpContext.GetOwinContext().GetUserManager<VDMUserManager>().FindByName(HttpContext.User.Identity.Name);
            if (objuser == null)
            {
                return RedirectToAction("Login");
            }
            var siteinforparam = new SiteInfoParam() { SiteInfoFilter = new SiteInfoFilter() };
            var siteinforbiz = new SiteInfoBiz();
            siteinforbiz.Search(siteinforparam);
            siteinforparam.SiteInfos = siteinforparam.SiteInfos.Where(z => z.Actives == true).ToList();
            return View(siteinforparam);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult selectTempleate(FormCollection forminput)
        {
            var objuser = HttpContext.GetOwinContext().GetUserManager<VDMUserManager>().FindByName(HttpContext.User.Identity.Name);
            if (objuser == null)
            {
                return RedirectToAction("Login");
            }
            var siteid = forminput.GetValue("siteid");
            var param = new AspNetUsersParam()
            {
                AspNetUsersInfo = new AspNetUserInfo
                {
                    Id = objuser.Id,
                    SiteId = siteid != null ? Utility.ConvertToInt(siteid.AttemptedValue) : 0,
                },

            };
            try
            {
                SiteInfoBiz siteinfor = new SiteInfoBiz();
                siteinfor.CopyContenSite(siteid != null ? Utility.ConvertToInt(siteid.AttemptedValue) : 0,
                    objuser.UserName);
            }
            catch
            {

            }
            var biz = new AspNetUsersBiz();
            biz.UpdateSiteID(param);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
        #endregion
    }
}