using Capstone.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Framework;
using Capstone.Models;
using System.Data;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace Capstone.Areas.User.Controllers
{
    [SessionTimeout]
    public class DashboardController : Controller
    {
        DashboardBAL dashboardBAL;
        DesignationBAL DesignationBal;

        // GET: User/Dashboard
        public ActionResult Index()
        {
           
            return View("Dashboard");
        }

        public string GetUserDashboard()
        {
            dashboardBAL = new DashboardBAL();
            int _userId = Session.GetDataFromSession<UserModel>("User").Id;
            DataSet ds = dashboardBAL.GetUserDashboard(_userId);
            return JsonConvert.SerializeObject(ds);
        }

        public ViewResult Drafts()
        {
            return View();
        }

     

        public ViewResult Submitted()
        {
            return View();
        }

        

        public ViewResult Licenses()
        {
            return View();
        }
        public ViewResult Ammendments()
        {
            return View();
        }
        

        public ViewResult Rejected()
        {
            return View();
        }
         
       
        public ViewResult CabService()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Airport = objBAL.GetAirports();
            return View();
        }
        [HttpPost]
        public ActionResult SaveCabservice(CabserviceModel model)
        {
            if (ModelState.ContainsKey("Id"))
                ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                DesignationBal = new DesignationBAL();
                // model.DepartmentId = Session.GetDataFromSession<UserModel>("User").DepartmentId;
                //if (model.Id == 0)
                  model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                //else
                //    model.LastModifiedUserId = Session.GetDataFromSession<UserModel>("User").CreatedUserId;
                CabserviceComplexViewModel Designation = new CabserviceComplexViewModel();
                List<CabserviceModel> DesignationList = DesignationBal.SaveCabservice(model);
                NotificationModel Notification = new NotificationModel();
                if (DesignationList != null && DesignationList.Count > 0)
                {
                    Notification.Title = "Sucess";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = model.Name + "Saved Successfully .Details Sent your email    ";
                    string emailBody = "Hi " + Session.GetDataFromSession<UserModel>("User").FirstName + "," + " <br/><br/> CabDriverName :  " + "James" + " <br/><br/> ArrivalTIme :  " + "1Am to 4 PM"+ "<br/><br/> Cab No :  " + "454454" + " <br/><br/>Thanks & Regards,<br/>Capstone Team.<br/> ";
                    SendEmail(emailBody, Session.GetDataFromSession<UserModel>("User").EmailId);

                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                Designation.CabserviceList = DesignationList;
                Designation.Notification = Notification;
                return Json(Designation);

            }
            else
            {
                return RedirectToAction("CabService");
            }
        }
        public JsonResult GetCabservice()
        {
             DesignationBal = new DesignationBAL();
            List<CabserviceModel> List = DesignationBal.GetCabservice(Session.GetDataFromSession<UserModel>("User").Id);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

     
        public ViewResult RentHome()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Airport = objBAL.GetAirports();

            return View();
        }
        private void SendEmail(string emailBody, string toEmail)
        {
            

                string mail = "vinayreddyacs@gmail.com"; // "aegisCapstonedev@gmail.com"; //<--Enter your gmail id here
                string password = "piratebrew5"; // "aegis123";//<--Enter gmail password here
                string FromMail = "vinayreddyacs@gmail.com";  //"aegisCapstonedev@gmail.com";
            try{
                using (MailMessage mm = new MailMessage(FromMail, toEmail))
                {
                    mm.Subject = "Cab Service Details";
                    mm.Body = emailBody; //

                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(mail, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            catch (Exception ex)
                    {

                
            }
        }


        //Bank Service
        public ViewResult OtherServices()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Airport = objBAL.GetAirports();

            return View();
        }

        [HttpPost]
        public ActionResult Saveotherservice(OtherServiceModel model)
        {
            if (ModelState.ContainsKey("Id"))
                ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                DesignationBal = new DesignationBAL();
                
                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                model.ServiceId = "1";
                //else
                othererviceComplexViewModel Otherervice = new othererviceComplexViewModel();
                List<OtherServiceModel> OthererviceList = DesignationBal.SaveOtherervice(model);
                NotificationModel Notification = new NotificationModel();
                if (OthererviceList != null && OthererviceList.Count > 0)
                {
                    Notification.Title = "Sucess";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = model.Name + "Saved Successfully";
                    string emailBody = "Hi " + Session.GetDataFromSession<UserModel>("User").FirstName + "," + " <br/><br/> Address :  " + model.DropingAddress+ " <br/><br/> Timeslot :  " + model.Timeslot + "<br/><br/> PickupDate:  " + model.PickupDate + " <br/><br/>Thanks & Regards,<br/>Capstone Team.<br/> ";
                    SendserviceEmail(emailBody, Session.GetDataFromSession<UserModel>("User").EmailId);

                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                Otherervice.OthererviceList = OthererviceList;
                Otherervice.Notification = Notification;
                return Json(Otherervice);

            }
            else
            {
                return RedirectToAction("CabService");
            }
        }
        public JsonResult GetOtherervice()
        {
            DesignationBal = new DesignationBAL();
            List<OtherServiceModel> List = DesignationBal.GetOtherservice(Session.GetDataFromSession<UserModel>("User").Id,1);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ViewResult DrivingServices()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Airport = objBAL.GetAirports();

            return View();
        }

        [HttpPost]
        public ActionResult SaveDrivingServices(OtherServiceModel model)
        {
            if (ModelState.ContainsKey("Id"))
                ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                DesignationBal = new DesignationBAL();

                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                model.ServiceId = "2";
                //else
                othererviceComplexViewModel Otherervice = new othererviceComplexViewModel();
                List<OtherServiceModel> OthererviceList = DesignationBal.SaveOtherervice(model);
                NotificationModel Notification = new NotificationModel();
                if (OthererviceList != null && OthererviceList.Count > 0)
                {
                    Notification.Title = "Sucess";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = model.Name + "Saved Successfully";
                    string emailBody = "Hi " + Session.GetDataFromSession<UserModel>("User").FirstName + "," + " <br/><br/> Address :  " + model.DropingAddress + " <br/><br/> Timeslot :  " + model.Timeslot + "<br/><br/> PickupDate:  " + model.PickupDate + " <br/><br/>Thanks & Regards,<br/>Capstone Team.<br/> ";
                    SendserviceEmail(emailBody, Session.GetDataFromSession<UserModel>("User").EmailId);

                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                Otherervice.OthererviceList = OthererviceList;
                Otherervice.Notification = Notification;
                return Json(Otherervice);

            }
            else
            {
                return RedirectToAction("CabService");
            }
        }
        public JsonResult GetDrivingServices()
        {
            DesignationBal = new DesignationBAL();
            List<OtherServiceModel> List = DesignationBal.GetOtherservice(Session.GetDataFromSession<UserModel>("User").Id, 2);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        //mobility
        public ViewResult MobilityServices()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Airport = objBAL.GetAirports();

            return View();
        }

        [HttpPost]
        public ActionResult SaveMobilityServices(OtherServiceModel model)
        {
            if (ModelState.ContainsKey("Id"))
                ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                DesignationBal = new DesignationBAL();

                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                model.ServiceId = "3";
                //else
                othererviceComplexViewModel Otherervice = new othererviceComplexViewModel();
                List<OtherServiceModel> OthererviceList = DesignationBal.SaveOtherervice(model);
                NotificationModel Notification = new NotificationModel();
                if (OthererviceList != null && OthererviceList.Count > 0)
                {
                    Notification.Title = "Sucess";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = model.Name + "Saved Successfully";
                    string emailBody = "Hi " + Session.GetDataFromSession<UserModel>("User").FirstName + "," + " <br/><br/> Address :  " + model.DropingAddress + " <br/><br/> Timeslot :  " + model.Timeslot + "<br/><br/> PickupDate:  " + model.PickupDate + " <br/><br/>Thanks & Regards,<br/>Capstone Team.<br/> ";
                    SendserviceEmail(emailBody, Session.GetDataFromSession<UserModel>("User").EmailId);

                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                Otherervice.OthererviceList = OthererviceList;
                Otherervice.Notification = Notification;
                return Json(Otherervice);

            }
            else
            {
                return RedirectToAction("CabService");
            }
        }
        public JsonResult GetMobilityServices()
        {
            DesignationBal = new DesignationBAL();
            List<OtherServiceModel> List = DesignationBal.GetOtherservice(Session.GetDataFromSession<UserModel>("User").Id, 3);
            return Json(List, JsonRequestBehavior.AllowGet);
        }


        public ViewResult Offers()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.Airport = objBAL.GetAirports();

            return View();
        }
        private void SendserviceEmail(string emailBody, string toEmail)
        {


            string mail = "vinayreddyacs@gmail.com"; // "aegisCapstonedev@gmail.com"; //<--Enter your gmail id here
            string password = "piratebrew5"; // "aegis123";//<--Enter gmail password here
            string FromMail = "vinayreddyacs@gmail.com";  //"aegisCapstonedev@gmail.com";
            try
            {
                using (MailMessage mm = new MailMessage(FromMail, toEmail))
                {
                    mm.Subject = "Service Details";
                    mm.Body = emailBody; //

                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(mail, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}