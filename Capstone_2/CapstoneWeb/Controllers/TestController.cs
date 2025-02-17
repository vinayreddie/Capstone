using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.BAL;

namespace Capstone.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostHtmlFormData()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult PostHtmlFormData(UserModel model)
        {
            return Content("hello");
        }

        #region Organ Transplant
        public ActionResult OrganTransplant1()
        {
            ClearallSessions();
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            return View();
        }
 
        public ActionResult SurgicalStaffList(OTStaffDetailsModel StaffDetails,string StaffType)
        {
            List<OTStaffDetailsModel> StaffList = new List<OTStaffDetailsModel>();
            switch (StaffType)
            {
                case "surgical":
                    if (Session["SurgicalStaffList"] != null)
                        StaffList = Session["SurgicalStaffList"] as List<OTStaffDetailsModel>;
                    StaffList.Add(StaffDetails);
                    Session["SurgicalStaffList"] = StaffList;
                    break;
                case "Capstone":
                    if (Session["CapstoneStaffList"] != null)
                        StaffList = Session["CapstoneStaffList"] as List<OTStaffDetailsModel>;
                    StaffList.Add(StaffDetails);
                    Session["CapstoneStaffList"] = StaffList;
                    break;
                case "anaesthesiology":
                    if (Session["anaesthesiologyStaffList"] != null)
                        StaffList = Session["anaesthesiologyStaffList"] as List<OTStaffDetailsModel>;
                    StaffList.Add(StaffDetails);
                    Session["anaesthesiologyStaffList"] = StaffList;
                    break;
                case "Laboratory":
                    if (Session["LaboratoryStaffList"] != null)
                        StaffList = Session["LaboratoryStaffList"] as List<OTStaffDetailsModel>;
                    StaffList.Add(StaffDetails);
                    Session["LaboratoryStaffList"] = StaffList;
                    break;
                case "Imaging":
                    if (Session["ImagingStaffList"] != null)
                        StaffList = Session["ImagingStaffList"] as List<OTStaffDetailsModel>;
                    StaffList.Add(StaffDetails);
                    Session["ImagingStaffList"] = StaffList;
                    break;
                case "Haematology":
                    if (Session["HaematologyStaffList"] != null)
                        StaffList = Session["HaematologyStaffList"] as List<OTStaffDetailsModel>;
                    StaffList.Add(StaffDetails);
                    Session["HaematologyStaffList"] = StaffList;
                    break;


            }
            return Json(StaffList);

        }
        public ActionResult EquipmentList(OTEquipmentModel Equipments,string EquipmentType)
        {
            List<OTEquipmentModel> OTEquipmentList = new List<OTEquipmentModel>();
            switch (EquipmentType)
            {
                case "anaesthesiology":
                    if (Session["anaesthesiologyEquipment"] != null)
                        OTEquipmentList= Session["anaesthesiologyEquipment"] as List<OTEquipmentModel>;
                    OTEquipmentList.Add(Equipments);
                    Session["anaesthesiologyEquipment"] = OTEquipmentList;
                    break;
                case "Facilities":
                    if (Session["FacilitiesEquiment"] != null)
                        OTEquipmentList = Session["FacilitiesEquiment"] as List<OTEquipmentModel>;
                    OTEquipmentList.Add(Equipments);
                    Session["FacilitiesEquiment"] = OTEquipmentList;
                    break;
                case "Laboratory":
                    if (Session["LaboratoryEquipment"] != null)
                        OTEquipmentList = Session["LaboratoryEquipment"] as List<OTEquipmentModel>;
                    OTEquipmentList.Add(Equipments);
                    Session["LaboratoryEquipment"] = OTEquipmentList;
                    break;
                case "Imaging":
                    if (Session["ImagingEquipment"] != null)
                        OTEquipmentList = Session["ImagingEquipment"] as List<OTEquipmentModel>;
                    OTEquipmentList.Add(Equipments);
                    Session["ImagingEquipment"] = OTEquipmentList;
                    break;
                case "Haematology":
                    if (Session["HaematologyEquipment"] != null)
                        OTEquipmentList = Session["HaematologyEquipment"] as List<OTEquipmentModel>;
                    OTEquipmentList.Add(Equipments);
                    Session["HaematologyEquipment"] = OTEquipmentList;
                    break;

            }
            return Json(OTEquipmentList);
        }
        public JsonResult OperationList(OTOperationModel Operation)
        {
            List<OTOperationModel> OperationList = new List<OTOperationModel>();
            if (Session["OperationList"] != null)
                OperationList = Session["OperationList"] as List<OTOperationModel>;
            OperationList.Add(Operation);
            Session["OperationList"] = OperationList;
            return Json(OperationList);
        }
        //public void SubmitOrganTransplant(OrganTransplantViewModel OrganTransplant)
        //{
        //    LicenseBAL obj = new LicenseBAL();
        //    OrganTransplant.Surgical.StaffDetailsList= Session["SurgicalStaffList"] as List<OTStaffDetailsModel>;
        //    OrganTransplant.CapstoneTeam.StaffDetailsList= Session["CapstoneStaffList"] as List<OTStaffDetailsModel>;
        //    OrganTransplant.Anaesthesiology.StaffDetailsList= Session["anaesthesiologyStaffList"] as List<OTStaffDetailsModel>;
        //    OrganTransplant.LaboratoryFacilities.StaffDetailsList = Session["LaboratoryStaffList"] as List<OTStaffDetailsModel>;
        //    OrganTransplant.ImagingServices.StaffDetailsList = Session["ImagingStaffList"] as List<OTStaffDetailsModel>;
        //    OrganTransplant.HaematologyServices.StaffDetailsList = Session["HaematologyStaffList"] as List<OTStaffDetailsModel>;

        //    OrganTransplant.Anaesthesiology.EquipmentsList= Session["anaesthesiologyEquipment"] as List<OTEquipmentModel>;
        //    OrganTransplant.ICUHDUFacilities.EquipmentsList = Session["FacilitiesEquiment"] as List<OTEquipmentModel>;
        //    OrganTransplant.LaboratoryFacilities.EquipmentsList= Session["LaboratoryEquipment"] as List<OTEquipmentModel>;
        //    OrganTransplant.ImagingServices.EquipmentsList= Session["ImagingEquipment"] as List<OTEquipmentModel>;
        //    OrganTransplant.HaematologyServices.EquipmentsList= Session["HaematologyEquipment"] as List<OTEquipmentModel>;

        //    OrganTransplant.Anaesthesiology.OperationsList= Session["OperationList"] as List<OTOperationModel>;
        //    bool Result = obj.SaveOrganTransplantation(OrganTransplant);
        //    NotificationModel Notification = new NotificationModel();
        //    if (Result)
        //    {
        //        Notification.Title = "Success";
        //        Notification.NotificationType = NotificationType.Success;
        //        Notification.NotificationMessage = "Applcation Submitted Successfully";

        //    }
        //}
        private void ClearallSessions()
        {
            //Staff 
            Session["SurgicalStaffList"] = null;
            Session["CapstoneStaffList"] = null;
            Session["anaesthesiologyStaffList"] = null;
            Session["LaboratoryStaffList"] = null;
            Session["ImagingStaffList"] = null;
            Session["HaematologyStaffList"] = null;
            //Equipment
            Session["anaesthesiologyEquipment"] = null;
            Session["FacilitiesEquiment"] = null;
            Session["LaboratoryEquipment"] = null;
            Session["ImagingEquipment"] = null;
            Session["HaematologyEquipment"] = null;
            //Operaion
            Session["OperationList"] = null;
        }
        #endregion

        public ActionResult UploadMultipleFiles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test1(HttpPostedFileBase singleFile, 
            IEnumerable<HttpPostedFileBase> multipleFiles)
        {
            return null;
        }

        [HttpPost]
        public ActionResult Test2(HttpPostedFileBase singleFile,
            HttpPostedFileBase[] multipleFiles)
        {
            return null;
        }

        [HttpPost]
        public ActionResult Test3(HttpPostedFileBase singleFile,
            IEnumerable<HttpPostedFileBase> multipleFiles)
        {
            var educationCertificates = new List<HttpPostedFileBase>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (Request.Files.AllKeys[i] == "educationCertificates")
                    educationCertificates.Add(Request.Files[i]);
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var a = Request.Files[i];
            }
            foreach (var item in Request.Files)
            {
                var z = item;
            }
            return null;
        }

        #region ErrorPages
        public ViewResult Error404()
        {
            return View("Error404");
        }
        public ViewResult Error505()
        {
            return View("Error505");
        }


        
        #endregion
    }
}