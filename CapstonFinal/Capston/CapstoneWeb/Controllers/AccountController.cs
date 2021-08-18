using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Capstone.Models;
using Capstone.BAL;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Collections.Generic;
using Capstone.Framework;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Capstone;

namespace CapstoneWeb.Controllers
{

    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        #region Global
        UserModel user;
        UserBAL userBAL;
        UserModel userModel;
        #endregion
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
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
            return this.RedirectToAction("Home", "Home");
        }

        //POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
       {
            if (!ModelState.IsValid)
                return View(model);

            if (model.CaptchaText != null)
            {
                if (Session["captchaSession"] != null)
                {
                    var captchsessionvalue = Session["captchaSession"].ToString();
                    if (model.CaptchaText.ToString() != captchsessionvalue.ToString())
                    {
                        return Json(new
                        {
                            errormessage = "Invalid Captcha Entered!"
                        });
                    }
                }
            }

            UserBAL objBAL = new UserBAL();
           // string SaltCode = objBAL.GetSaltCode(model.Username);
            //if (SaltCode == null)
            //    return View(model);
            //model.Password = model.Password.Substring(2);
            //model.Password = model.Password.Substring(0,model.Password.Length-3);
            //model.Password = model.Password + SaltCode;
            UserViewModel user = objBAL.ValidateUser(model);

            if (user == null)
            {
                return Json(new
                {
                    errormessage = "Invalid Username/Password"
                });
            }
            else
            {
                Session["captchaSession"] = null;
                Session["User"] = user;

                #region Is Mobile Number Valid ,confirmation Checking 
                //if (user.IsMobileNoVerified == false)
                //{
                //    return Json(new
                //    {
                //        redirecturl = Url.Action("UpdatevalidMobileNoDetails", "Home",
                //        new { Area = "" })
                //    });
                //}
                //else
                //{
                //    return Json(new
                //    {
                //        redirecturl = Url.Action(user.Role.DefaultAction, user.Role.DefaultController,
                //        new { Area = user.Role.DefaultArea })
                //    });
                //}
                #endregion

                return Json(new
                {
                    redirecturl = Url.Action(user.Role.DefaultAction, user.Role.DefaultController,
                        new { Area = user.Role.DefaultArea })
                    //redirecturl = Url.Action("Dashboard1", "Dashboard",
                    //    new { Area = "Admin"})
                });
            }
        }
        [AllowAnonymous]
        public ActionResult LoginLink()
        {


            UserModel user = Session.GetDataFromSession<UserModel>("User");
            return RedirectToAction(user.Role.DefaultAction, user.Role.DefaultController,
              new { Area = user.Role.DefaultArea });
            // return View();
        }

        private JsonResult RedirectToMobileNoUpdateSwitchPage()
        {
            return Json(new
            {
                url = Url.Action("UpdatevalidMobileNoDetails", "Home").ToString()
            });
        }

        private JsonResult DirectLogin(LoginModel login, string returnUrl)
        {
            UserBAL userBAL = new UserBAL();
            UserViewModel user = userBAL.ValidateUser(login);
            if (user == null)
                return Json("Invalid Login");
            else
            {
                Session["captchaSession"] = null;
                string url = string.Empty;
                Session.SetDataToSession<UserViewModel>("User", user);
                
                if (!string.IsNullOrEmpty(returnUrl))
                    url = returnUrl;
                else
                    url = Url.Action(user.Role.DefaultAction, user.Role.DefaultController,
                    new { Area = user.Role.DefaultArea }).ToString();

                return Json(new
                {
                    url
                });
            }
        }

        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/VerifyCode

        #region DeptAdminAddUser        
        //[AllowAnonymous]
        //public ActionResult AddUser()
        //{
        //    userBAL = new UserBAL();
        //    List<RoleModel> roleList = new List<RoleModel>();
        //    if (Session["User"] != null)
        //    {
        //        userModel = (UserModel)Session["User"];
        //        roleList = userBAL.GetDesignationList(userModel.DepartmentId);
        //        ViewData["RoleList"] = roleList;
        //        if (userModel.RoleType == RoleTypes.SuperAdmin)
        //        {
        //            ViewBag.RoleType = userModel.RoleType;
        //        }

        //        else if (userModel.RoleType == RoleTypes.DepartmentAdmin)
        //            return View();
        //    }
        //    return View();
        //}

        public JsonResult AddDepartmentUser(UserModel user)
        {
            NotificationModel Notification = new NotificationModel();
            userBAL = new UserBAL();
            string status = "";
            if (Session["User"] != null)
                userModel = (UserModel)Session["User"];
            DataTable objDeptUser = new DataTable();
            //if (user.Id == 0)
            //{
            //    objDeptUser = userBAL.CheckUserExistOrNot(user);

            //}
            if (objDeptUser != null && objDeptUser.Rows.Count > 0)
            {
                status = "User Already Existed";
            }
            else
            {
                user.Password = user.UserName;
                user.CreatedUserId = userModel.Id;
                user.DepartmentId = userModel.DepartmentId;
                userBAL = new UserBAL();
                bool result = userBAL.AddUser(user);
                if (result == true)
                {
                    status = user.Id == 0 ? "Saved Successfully" : "Updated Successfully";
                    Notification.Title = "Success";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = status;
                    Notification.ShowActionButton = true;
                    Notification.ActionButtonText = "Ok";
                    Notification.ActionName = "AddUser";
                    Notification.ControllerName = "Account";
                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Technical Problem While Saving";
                    Notification.ShowActionButton = false;
                }
            }

            return Json(Notification);
        }
        //[AllowAnonymous]
        //public JsonResult GetDepartmentUsersList()
        //{
        //    List<DepartmentViewModel> DeptUsersList = new List<DepartmentViewModel>();

        //    UserModel User = Session["user"] as UserModel;
        //    userBAL = new UserBAL();
        //    DeptUsersList = userBAL.GetDepartmentUsers(4, User.DepartmentId);
        //    Session["DeptUsersList"] = DeptUsersList;
        //    return Json(DeptUsersList);
        //}

        //public JsonResult EditDeptUser(int DeptUserId)
        //{
        //    List<DepartmentViewModel> DeptUsersList = new List<DepartmentViewModel>();
        //    DepartmentViewModel DeptUser = new DepartmentViewModel();
        //    DeptUsersList = Session["DeptUsersList"] as List<DepartmentViewModel>;
        //    DeptUser = DeptUsersList.Where(item => item.usermodel.Id == DeptUserId).FirstOrDefault();
        //    return Json(DeptUser);


        //}
        #endregion

        public ActionResult Signout()
        {
            // Clearing the session        - vinay, 31-05-2017
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();

            AuthenticationManager.SignOut();
            //return Response.RedirectPermanent(Url.Action("Index", "Home"));
            return RedirectToAction("Home", "Home");
        }

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

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            userBAL = new UserBAL();
            ViewBag.DistrictList = userBAL.GetCountries();
            return View("Register");
        }

        

        //
        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(UserModel objuser)       //async Task<ActionResult>
        //{
        //    NotificationModel Notification = new NotificationModel();
        //    UserBAL userBAL = new UserBAL();
        //    string result = userBAL.ValidateUser(objuser);
        //    DataTable dt = new DataTable();
        //    if (result.Length != 0)
        //    {
        //        Notification.Title = "Information";
        //        Notification.NotificationType = NotificationType.Success;
        //        Notification.NotificationMessage = result;
        //        return Json(Notification);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            #region Email Sent
        //            if(objuser.EmailId == null)
        //            {
        //                Notification.Title = "Error";
        //                Notification.NotificationType = NotificationType.Danger;
        //                Notification.NotificationMessage = "Please Enter Email!";
        //                Notification.ShowNonActionButton = true;
        //                Notification.NonActionButtonClassType = PopupButtonClass.Danger;
        //                Notification.NonActionButtonText = "Okay";
        //                return Json(Notification);
        //            }
                        
        //                int regID = 0;
        //                objuser.Password = "Applicant@123";
        //                regID = userBAL.SaveUserRegistratin(objuser); // modified by kishore 27-06-2017 
        //                if (regID > 0)
        //                {
        //                    #region Newly added For Offline exsting LicenseNumbers Registrations on 14-01-2021
        //                    int existingTAMCEApplicationId = 0;
        //                    if (Session["ExistingTAMCEApplicationId"] != null)
        //                    {
        //                        existingTAMCEApplicationId = (Int32)Session["ExistingTAMCEApplicationId"];
        //                        if (existingTAMCEApplicationId > 0)
        //                        {
        //                            bool updateuserId = userBAL.updateUserToTAMCEapplication(existingTAMCEApplicationId, regID);
        //                        }
        //                        Session["ExistingTAMCEApplicationId"] = null;
        //                    }
        //                #endregion

        //                // Send SMS
        //                string msgBody = "Hi " + objuser.FirstName + "," + " Your UserName :  " + objuser.EmailId + " Password :  " + "Applicant@123" + "  From Capstone Team(TAMCE). ";
        //                string deliveryStatus;
        //                bool smsresult = Utitlities.SendSMS(objuser.MobileNumber, msgBody, out deliveryStatus);

        //                // Send email
        //                string emailBody = "Hi " + objuser.FirstName + "," + " <br/><br/> LoginName :  " + objuser.EmailId + "<br/><br/> Password :  " + "Applicant@123" + "  .<br/><br/>Thanks & Regards,<br/>Capstone Team.<br/> ";
        //                SendEmail(emailBody, objuser.EmailId);

        //                Notification.Title = "Success";
        //                    Notification.NotificationType = NotificationType.Success;
        //                    Notification.NotificationMessage = "Successfully Registered. Login credentials are sent to your email and mobile";
        //                    Notification.ShowActionButton = true;
        //                    Notification.ActionButtonText = "Login";
        //                    Notification.ActionName = "Login";
        //                    Notification.ControllerName = "Account";
        //                    Notification.AreaName = "";
        //                    return Json(Notification);
        //                }
        //                else
        //                {
        //                    Notification.Title = "Error";
        //                    Notification.NotificationType = NotificationType.Danger;
        //                    Notification.NotificationMessage = "Something went wrong! Please contact Help desk";
        //                    Notification.ShowNonActionButton = true;
        //                    Notification.NonActionButtonClassType = PopupButtonClass.Danger;
        //                    Notification.NonActionButtonText = "Okay";
        //                    Notification.ReturnData = "0," + FormStatus.Empty;
        //                    return Json(Notification);
        //                }
        //            // end using
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
        //            exception.MasterParamField = objuser;
        //            Logger.LogError(exception);
        //            NotificationModel notification = new NotificationModel();
        //            Notification.Title = "Error";
        //            Notification.NotificationType = NotificationType.Danger;
        //            Notification.NotificationMessage = "Something went wrong! Please contact Help desk";
        //            Notification.ShowNonActionButton = true;
        //            Notification.NonActionButtonClassType = PopupButtonClass.Danger;
        //            Notification.NonActionButtonText = "Okay";
        //            Notification.ReturnData = "0," + FormStatus.Empty;
        //            return Json(Notification);
        //        }
        //    }
        //}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveContact(ContactModel objuser)       //async Task<ActionResult>
        {
            NotificationModel Notification = new NotificationModel();
            UserBAL userBAL = new UserBAL();
            
             
                try
                {
                    #region Email Sent
                    if (objuser.EmailId == null)
                    {
                        Notification.Title = "Error";
                        Notification.NotificationType = NotificationType.Danger;
                        Notification.NotificationMessage = "Please Enter Email!";
                        Notification.ShowNonActionButton = true;
                        Notification.NonActionButtonClassType = PopupButtonClass.Danger;
                        Notification.NonActionButtonText = "Okay";
                        return Json(Notification);
                    }

                    int regID = 0;
                    objuser.Password = "Applicant@123";
                    regID = userBAL.SaveContact(objuser); // modified by kishore 27-06-2017 
                    if (regID > 0)
                    { 
                        Notification.Title = "Success";
                        Notification.NotificationType = NotificationType.Success;
                        Notification.NotificationMessage = "Successfully Submitted";
                        Notification.ShowActionButton = true;
                        Notification.ActionButtonText = "Ok";
                        Notification.ActionName = "Home";
                        Notification.ControllerName = "Home";
                        Notification.AreaName = "";
                        return Json(Notification);
                    }
                    else
                    {
                        Notification.Title = "Error";
                        Notification.NotificationType = NotificationType.Danger;
                        Notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                        Notification.ShowNonActionButton = true;
                        Notification.NonActionButtonClassType = PopupButtonClass.Danger;
                        Notification.NonActionButtonText = "Okay";
                        Notification.ReturnData = "0," + FormStatus.Empty;
                        return Json(Notification);
                    }
                    // end using
                    #endregion
                }
                catch (Exception ex)
                {
                    ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                    exception.MasterParamField = objuser;
                    Logger.LogError(exception);
                    NotificationModel notification = new NotificationModel();
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                    Notification.ShowNonActionButton = true;
                    Notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    Notification.NonActionButtonText = "Okay";
                    Notification.ReturnData = "0," + FormStatus.Empty;
                    return Json(Notification);
                }
            
        }

        private void SendEmail(string emailBody, string toEmail)
        {
            string mail = "tamcehelpdesk@gmail.com"; // "aegisCapstonedev@gmail.com"; //<--Enter your gmail id here
            string password = "support@acs123"; // "aegis123";//<--Enter gmail password here
            string FromMail = "tamcehelpdesk@gmail.com";  //"aegisCapstonedev@gmail.com";

            using (MailMessage mm = new MailMessage(FromMail, toEmail))
            {
                mm.Subject = " Application : Your Login Credentials";
                mm.Body = emailBody; //

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(mail, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpPost]
        public PartialViewResult ForgotPassword()
        {
            UserModel user = new UserModel();
            userBAL = new UserBAL();
            if (Session["User"] != null)
                user = (UserModel)Session["User"];            
            user.OTP = Session.GetDataFromSession<string>("OTP");
            user.Id = Session.GetDataFromSession<int>("ResetUserId");

            return PartialView("_ForgetPassword", user);
        }

        //
        // POST: /Account/ForgotPassword

        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        public ActionResult ResetUserPassword(int Id, string Password) 
        {
            NotificationModel Notification = new NotificationModel();
            UserBAL userBAL = new UserBAL();
           
            bool result= userBAL.ResetPassword(Id,Password);
            if (result)
            {
                
                Notification.Title = "Success";
                Notification.NotificationType = NotificationType.Success;
                Notification.NotificationMessage = "Your Password has been changed";
                Notification.ShowActionButton = true;
                Notification.NonActionButtonClassType = PopupButtonClass.Success;
                Notification.NonActionButtonText = "Okay";
                Notification.ActionButtonText = "Login";
                Notification.ActionName = "Login";
                Notification.ControllerName = "Account";
                Notification.AreaName = "";
            }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "something went wrong! Please contact Help desk";
                    Notification.ShowNonActionButton = true;
                    Notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    Notification.NonActionButtonText = "Okay";
                    Notification.ReturnData = "0," + FormStatus.Empty;
                }
                return Json(Notification);
            }

            
        

        [AllowAnonymous]
        
        public ActionResult ForgotPasswordRecovery(UserModel objuser)       //async Task<ActionResult>
        {
            NotificationModel Notification = new NotificationModel();
            UserBAL userBAL = new UserBAL();
            if (Session["User"] != null)
                user = (UserModel)Session["User"];
            UserModel model = userBAL.ForgetPassword(objuser.EmailId);   //objuser.Id,
            try
            {
                if (model.UserName != null)
                {
                    string mail = "aegisconsultingservice@gmail.com"; //<--Enter your gmail id here
                    string password = "aegis123";//<--Enter gmail password here
                    string FromMail = "aegisconsultingservice@gmail.com";
                    using (MailMessage mm = new MailMessage(FromMail, objuser.EmailId))
                    {

                        mm.Subject = "Capstone Application : Your Login Credentials";
                        mm.Body = "Hi " + objuser.FirstName + "," + " <br/><br/> LoginName :  " + model.UserName + "<br/><br/> Password :  " + model.Password + " .<br/><br/>Thanks & Regards,<br/>Capstone Team.<br/> ";

                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(mail, password);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }// end using
                    Notification.Title = "Success";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = "Please check your email to your Login Credentials";
                    Notification.ShowActionButton = true;
                    Notification.NonActionButtonClassType = PopupButtonClass.Success;
                    Notification.NonActionButtonText = "Okay";
                    Notification.ActionButtonText = "Login";
                    Notification.ActionName = "Login";
                    Notification.ControllerName = "Account";
                    Notification.AreaName = "";
                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "EmailId went wrong! Please contact Help desk";
                    Notification.ShowNonActionButton = true;
                    Notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    Notification.NonActionButtonText = "Okay";
                    Notification.ReturnData = "0," + FormStatus.Empty;
                }
                return Json(Notification);
            }

            catch (Exception ex)
            {
                throw ex;
            }
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
            if (userId == null)
            {
                return View("Error");
            }
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
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
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

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Home", "Home");
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
            return RedirectToAction("Home", "Home");
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
        #endregion
        [AllowAnonymous]
        public ActionResult GetDistrictsList()
        {
            userBAL = new UserBAL();
            //ViewBag.DistrictList = userBAL.GetCountries();  
            List<DistrictModel> DistrictList = userBAL.GetCountries(); 
            return Json(DistrictList);

        }
        #region Profile Details
        [AllowAnonymous]
        public PartialViewResult EditProfile()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Collegelst = objBAL.GetCollege();
            userBAL = new UserBAL();
            if (Session["User"] != null)
                userModel = (UserModel)Session["User"];
            UserViewModel model = userBAL.ProfileDetails(userModel.Id);
            model.Role = new RoleModel();
            model.Role.Name = Session.GetDataFromSession<UserModel>("User").Role.Name;
            Session.SetDataToSession<int>("UserPhoto", model.UserPhoto);
            ViewBag.RoleName = model.Role.Name;
            return PartialView("_EditProfile", model);
        }

        public JsonResult GetUserProfile()
        {
            userBAL = new UserBAL();
            if (Session["User"] != null)
                userModel = (UserModel)Session["User"];
            UserViewModel model = userBAL.ProfileDetails(userModel.Id);
            return Json(model);
        }
        [AllowAnonymous]
        public JsonResult UpdateProfile(UserViewModel model, HttpPostedFileBase UserPhoto)
        {
            NotificationModel notification = new NotificationModel();
            
            model.Role = new RoleModel();
            int userId = Session.GetDataFromSession<UserModel>("User").Id;
            model.LastModifiedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            UserViewModel user= Session.GetDataFromSession<UserViewModel>("User");
           

            try
            {
                #region File Saving
              
                var uploadsPath = Path.Combine("User", "Profile");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                if (model.UserPhoto != null && model.UserPhoto!="undefined")
                {
                    string _applicantPhotoPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(UserPhoto.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UserPhoto = _applicantPhotoPath + Path.GetExtension(UserPhoto.FileName);

                    UserPhoto.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _applicantPhotoPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _applicantPhotoPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(UserPhoto.FileName));
                }
                else if (Session["UserPhoto"] != null)
                {
                    model.UserPhoto = Session.GetDataFromSession<string>("UserPhoto");
                }
              

                #endregion
            }
            catch (Exception ex)
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Error in the uploaded Photo";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Okay";
              
                return Json(notification);
            }


            userBAL = new UserBAL();
            int result = userBAL.SaveProfileDetails(model, ref userId);
            if (result > 0)
            {
                user.UserPhoto = model.UserPhoto;
                Session.SetDataToSession<UserViewModel>("User", user);
                Session.SetDataToSession<int>("UserId", userId);

                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Profile details saved";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Success;
                notification.NonActionButtonText = "Okay";
            }
            else
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Okay";
                notification.ReturnData = "0," + FormStatus.Empty;
            }

            return Json(notification);
            //}
            //return null;
        }
        #endregion

        public JsonResult CheckForUserName(string UserName)
        {
            userBAL = new UserBAL();
            UserModel User = userBAL.CheckForUserName(UserName);
            if (User.EmailId != null)
            {
                User.OTP = Utitlities.GenerateNumber();
                string subject = User.OTP + " is your verification code for secure access";
                string MailBody = "Hi, Greetings!<br/> Your OTP:" + "<b>" + User.OTP + "</b><br/><br/>Once you have verified the code, you'll be prompted to set a new password immediately. This is to ensure that only you have access to your account.";
                Session.SetDataToSession<string>("OTP", User.OTP);
                Session.SetDataToSession<string>("ResetUserId", User.Id);
                Utitlities.SendEmail(User.EmailId, MailBody, subject);

            }
           
            return Json(User);

        }

        //public ActionResult SearchApplicationNo(ApplicationModel model)   //ApplicationModel model
        //{
        //    NotificationModel notification = new NotificationModel();
        //    userBAL = new UserBAL();
        //    List<ApplicationModel> objList = userBAL.ApplicationTrack(model.ApplicationNumber, Status.All);
        //    //objList = userBAL.ApplicationTrack(model.ApplicationNumber,Status.All);
        //    if (objList.Count > 0)
        //    {
        //        //notification.Title = "Success";
        //        //notification.NotificationType = NotificationType.Success;
        //        //notification.NotificationMessage = "Check Your License Details.";
        //        //notification.ShowNonActionButton = true;
        //        //notification.NonActionButtonClassType = PopupButtonClass.Success;
        //        //notification.NonActionButtonText = "Okay";
        //        return Json(objList);
        //    }
        //    else
        //    {
        //        notification.Title = "Error";
        //        notification.NotificationType = NotificationType.Danger;
        //        notification.NotificationMessage = "License Number Does Not Exist.";
        //        notification.ShowNonActionButton = true;
        //        notification.NonActionButtonClassType = PopupButtonClass.Danger;
        //        notification.NonActionButtonText = "Okay";
        //        return Json(notification);
        //    }
        //    // return View(notification);
        //}

        //public JsonResult ThirdPartyVerification(string DistrictId, string MandalId, string VillageId, string LicenseNumber)
        //{
        //    int districtId = Convert.ToInt32(DistrictId);
        //    int mandalId = Convert.ToInt32(MandalId);
        //    int villageId = Convert.ToInt32(VillageId);


        //    userBAL = new UserBAL();
        //    List<ThirdPartyVerification> model = userBAL.ThirdPartyVerification(districtId, mandalId, villageId, LicenseNumber);
        //    return Json(model);
        //}

        //public JsonResult GetMandals(int id)
        //{
        //    LicenseBAL objBAL = new LicenseBAL();
        //    return Json(objBAL.GetMandalList(id));
        //}

        //public JsonResult GetVillages(int id)
        //{
        //    LicenseBAL objBAL = new LicenseBAL();
        //    return Json(objBAL.GetVillageList(id));
        //}

      //  #region ExistingLicense
        
        public PartialViewResult GenerateOTP(string MobileNumber, int transactionId)
        {
            string OTP = Utitlities.GenerateNumber();
            Session.SetDataToSession<string>("OTP", OTP);
            ViewBag.OTP = OTP;
            ViewBag.TransactionId = transactionId;

            string deliveryStatus;
            string otpMessage = OTP + " is your verification code for secure access";
            bool result = Utitlities.SendSMS(MobileNumber, otpMessage, out deliveryStatus);

            if(Session["existingLicenseEmailId"] != null)
            {
                string emailinfo = Session["existingLicenseEmailId"] as string;
                SendEmail(otpMessage, emailinfo); // SentEmail Added Newly For if OTP not delivered to MobileNumber,OTP send to Email also for alternative Solution
            }
            Session["existingLicenseEmailId"] = null;
            return PartialView("~/Areas/User/Views/Shared/_GenerateOTP.cshtml");

        }


        public PartialViewResult GenerateOTP1(string MobileNumber,int transactionId,string EmailId)
        {
            string OTP = Utitlities.GenerateNumber();
            Session.SetDataToSession<string>("OTP", OTP);
            ViewBag.OTP = OTP;
            ViewBag.TransactionId = transactionId;

            string deliveryStatus;
            string otpMessage = OTP + " is your verification code for secure access";
            bool result = Utitlities.SendSMS(MobileNumber, otpMessage , out deliveryStatus);
            SendEmail(otpMessage, EmailId); // SentEmail Added Newly For if OTP not delivered to MobileNumber,OTP send to Email also for alternative Solution
            return PartialView("~/Areas/User/Views/Shared/_GenerateOTP.cshtml");

        }

        #region Change password
        public ActionResult ChangePassword()
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            LoginModel model = new LoginModel();
            model.Username = user.FirstName;
            model.Id = user.Id;
            return View(model);
        }
        public ActionResult ChangeUserPassword(int Id, string Password, string userName)
        {
            NotificationModel Notification = new NotificationModel();
            UserBAL userBAL = new UserBAL();

            bool result = userBAL.ResetPassword(Id, Password);
            if (result)
            {
                Notification.Title = "Success";
                Notification.NotificationType = NotificationType.Success;
                Notification.NotificationMessage = "Your Password has been changed";
                Notification.ShowActionButton = true;
                Notification.NonActionButtonClassType = PopupButtonClass.Success;
                Notification.NonActionButtonText = "Okay";
                Notification.ActionButtonText = "Login";
                Notification.ActionName = "Login";
                Notification.ControllerName = "Account";
                Notification.AreaName = "";

                if (userName != null || userName != "")
                {
                    string emailBody = "Sir,<br/><br/> Password is Changed !" + " <br/><br/> LoginName :  " + userName + "<br/><br/> Password :  " + Password + "  .<br/><br/>Thanks & Regards,<br/> Team.<br/> ";
                    SendEmail(emailBody, userName);
                }
            }
            else
            {
                Notification.Title = "Error";
                Notification.NotificationType = NotificationType.Danger;
                Notification.NotificationMessage = "something went wrong!";
                Notification.ShowNonActionButton = true;
                Notification.NonActionButtonClassType = PopupButtonClass.Danger;
                Notification.NonActionButtonText = "Okay";
                Notification.ReturnData = "0," + FormStatus.Empty;
            }
            return Json(Notification);
        }
        #endregion
    }
}