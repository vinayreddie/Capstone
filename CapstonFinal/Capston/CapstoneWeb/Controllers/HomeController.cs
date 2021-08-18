using Capstone.BAL;
using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        #region Global
        MasterBAL masterBal;

        #endregion
        public ActionResult Index()
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();
            return View("Home");
        }
        public ActionResult Home()
        {
            // Clearing the session        - vinay, 31-05-2017
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();
            return View();
        }

        [HttpPost]
        public PartialViewResult ForgotPassword()
        {
            UserModel user = new UserModel();
            UserBAL userBAL = new UserBAL();
            if (Session["User"] != null)
                user = (UserModel)Session["User"];
            user.OTP = Session.GetDataFromSession<string>("OTP");
            user.Id = Session.GetDataFromSession<int>("ResetUserId");

            return PartialView("_ForgetPassword", user);
        }

        public ActionResult Registration()
        {
            return View();
        }
        //POST: /Home/Registration
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registration(UserModel objuser)
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult EODB()
        {
            UserBAL userBAL = new UserBAL();
            ViewBag.DistrictList = userBAL.GetCountries();
            return View("EODB");
        }
        public ActionResult Contact()
        {
            UserBAL userBAL = new UserBAL();
            ViewBag.College = userBAL.GetCollege();

            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ViewResult ProcedureChecklist()
        {
            return View();
        }

        public FileResult Download(string name)
        {
            return File(Server.MapPath("~/Documents/ProcedureChecklist/" + name + ".pdf"), "application/pdf");
            //return File("~/Documents/ProcedureChecklist/"+name+".pdf", "application/pdf");
        }
        public PartialViewResult Form1CertificateDetails()
        {
            return PartialView("_TAMCEForm1Certificate");
        }
        public ViewResult ApplicationForm1Details()
        {
            return View();
        }

        public ActionResult TamceDemoVideo()
        {
            return View();
        }

        public ActionResult SLADays()
        {
            return View();
        }

        public ActionResult SiteMapDetails()
        {
            return View();
        }

        #region Captcha in Login
        public ActionResult CaptchaIndex()
        {
            //CaptchaHelper captchaHelper = new CaptchaHelper();
            return File(DrawByte(), "image/jpeg");
        }
        public byte[] DrawByte()
        {
            byte[] returnByte = { };
            Bitmap bitmapImage = new Bitmap(150, 30, PixelFormat.Format32bppArgb);

            //  
            // Here we generate random string  
            string key = getRandomString();

            //  
            // key string adding to Session 
            HttpContext.Session.Add("captchSession", key);
            Session["captchaSession"] = key;
            //  
            // Creating image with key  
            using (Graphics g = Graphics.FromImage(bitmapImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                Rectangle rect = new Rectangle(0, 0, 150, 30);
                HatchBrush hBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                g.FillRectangle(hBrush, rect);
                hBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Red, Color.Black);
                float fontSize = 20;
                Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Strikeout);
                float x = 10;
                float y = 1;
                PointF fPoint = new PointF(x, y);
                g.DrawString(key, font, hBrush, fPoint);

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmapImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    returnByte = ms.ToArray();
                }
            }
            return returnByte;
        }
        private string getRandomString()
        {

            string returnString = string.Empty;
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            Random rand = new Random();

            int length = rand.Next(5, 8);
            for (int i = 0; i < length; i++)
            {
                int pos = rand.Next(0, 62);
                returnString += letters[pos].ToString();
            }
            return returnString;
        }
        #endregion

        public ActionResult UpdatevalidMobileNoDetails()
        {
            UserViewModel user = Session.GetDataFromSession<UserViewModel>("User");
            LoginModel model = new LoginModel();
            model.Username = user.UserName;
            model.Id = user.Id;
            return View(model);
        }

        public string GenerateOTP(string MobileNumber, string userName,int userId)
        {
            string OTP = Utitlities.GenerateNumber();
            Session.SetDataToSession<string>("OTP", OTP);
            ViewBag.OTP = OTP;
            ViewBag.userName = userName;

            string deliveryStatus;
            string otpMessage = OTP + " is your verification code for secure access";
            bool result = Utitlities.SendSMS(MobileNumber, otpMessage , out deliveryStatus);
            if(deliveryStatus == "Success")
                OTPSendEmail(otpMessage, userName);
            return OTP;
        }

        private void OTPSendEmail(string emailBody, string toEmail)
        {
            string mail = "tamcehelpdesk@gmail.com"; 
            string password = "support@acs123";
            string FromMail = "tamcehelpdesk@gmail.com"; 

            using (MailMessage mm = new MailMessage(FromMail, toEmail))
            {
                mm.Subject = emailBody; 
                mm.Body = emailBody; 

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

        public ActionResult VerifyUpdateMobileNumber(int Id, string MobileNumber, string userName)
        {
            NotificationModel Notification = new NotificationModel();
            UserBAL userBAL = new UserBAL();
            UserViewModel authenticatedUser = Session.GetDataFromSession<UserViewModel>("User");

            bool result = userBAL.VerifyUpdateMobileNumber(Id, MobileNumber,userName);
            if (result)
            {                
                if (userName != null || userName != "")
                {
                    string emailBody = "Sir,<br/><br/> MobileNumber is Verified and Updated !" + "<br/><br/>Thanks & Regards,<br/>TAMCE Team.<br/> ";
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
            if (authenticatedUser != null)
            {
                return Json(new
                {
                    redirecturl = Url.Action(authenticatedUser.Role.DefaultAction, authenticatedUser.Role.DefaultController,
                        new { Area = authenticatedUser.Role.DefaultArea })
                });
            }
            else
                return Json(Notification);
        }
        private void SendEmail(string emailBody, string toEmail)
        {
            string mail = "tamcehelpdesk@gmail.com"; 
            string password = "support@acs123";
            string FromMail = "tamcehelpdesk@gmail.com";

            using (MailMessage mm = new MailMessage(FromMail, toEmail))
            {
                mm.Subject = "TAMCE Application : Mobile Number Verify Status";
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
    }
}