using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using System.IO;
using Capstone.BAL;
using Capstone.Models;
using Capstone.Framework;
using System.Data;
using Newtonsoft.Json;

namespace Capstone.Areas.User.Controllers
{    
    public class TestController : Controller
    {
        // GET: User/Test
        DashboardBAL dashboardBAL;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsers()
        {
            return View("Index");
        }
        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("Index") { FileName = "somename" };
        }

        #region TransactionTrack
        public ActionResult TransactionTrackList(int TransactionId, string TransactionType)
        {
            dashboardBAL = new DashboardBAL();
            //UserModel user = Session.GetDataFromSession<UserModel>("User");
            ApplicationTrackModel appTrack = dashboardBAL.GetApplicationTrack(TransactionId, TransactionType);
            if (appTrack.APMCETrackModel == null)
                appTrack.APMCETrackModel = new TransactionTrackModel();
            if (appTrack.PCPNDTTrackModel == null)
                appTrack.PCPNDTTrackModel = new TransactionTrackModel();
            if (appTrack.BloodBankForm27CModel == null)
                appTrack.BloodBankForm27CModel = new TransactionTrackModel();
            if (appTrack.bbForm27EModel == null)
                appTrack.bbForm27EModel = new TransactionTrackModel();
            return View(appTrack);
            //HACK NEED TO CHECK ONCE 
        }
        #endregion


        public ActionResult GetPartialView(ApprovalsModel approval, string Submit, List<InspectionModel> InspectionList, string serializedString, string HasInspectionPrivilege)
        {
            #region
            List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();
            string Path = "";
            DepartmentUserBAL objBAL = new DepartmentUserBAL();

            UserModel user = Session.GetDataFromSession<UserModel>("User");
            #region Test
            if (Submit == "Forward")
            {
                approval.status = Status.Forward;
                if (HasInspectionPrivilege == "True")
                {
                    //Path = GeneratePDF(serializedString);
                    if (InspectionList != null && InspectionList.Count > 0)
                        InspectionList.ForEach(item => item.DepartmentUserId = user.Id);
                    if (Session["UploadList"] != null)
                        UploadList = Session["UploadList"] as List<DocumentUploadModel>;
                }
                else
                {
                    UploadList = null;
                    InspectionList = null;
                }
            }
            else if (Submit == "Return")
                approval.status = Status.Return;
            else if (Submit == "Approve")
            {
                approval.status = Status.Approved;
                UploadList = null; InspectionList = null;
            }

            else if (Submit == "Reject")
                approval.status = Status.Rejected;
            #endregion
            approval.UserId = user.Id;


            bool Result = objBAL.SaveApprovals(approval, user.DesignationId, InspectionList, UploadList, approval.TransactionId, Path);

            if (approval.status == Status.Approved)
            {
                string Type = "Transaction";// Passing static type  --kishore 17-10-2017
                LicenseBAL licenceBAL = new LicenseBAL();
                DataTable dtItems = licenceBAL.GetLicenseType(approval.TransactionId, Type);
                LicenseViewModel model = new LicenseViewModel();
                if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
                {
                    model.APMCECertificate = licenceBAL.GetAPMCECertificate(approval.TransactionId, Type);
                    return PartialView("_APMCETemporaryCertificate", model);
                }
                else if (dtItems.Rows[0]["ActType"].ToString() == "PCPNDT")
                {
                   
                    model.PCPNDTCertificate = licenceBAL.GetPCPNDTLicenseDetails(approval.TransactionId, Type);
                    //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                    //PCPNDTmodel = model.PCPNDTCertificate;
                    return PartialView("_PCPNDTLicense", model.PCPNDTCertificate);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                NotificationModel Notification = new NotificationModel();
                if (Result)
                {
                    Notification.Title = "Success";
                    Notification.NotificationType = NotificationType.Success;
                    Notification.NotificationMessage = "Application " + approval.status + "ed Successfully";
                    Notification.ShowActionButton = true;
                    Notification.ActionButtonText = "Goto List";
                    Notification.ActionName = "ListofApplications";
                    Notification.ControllerName = "DepartmentUser";
                    Notification.AreaName = "Department";
                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }

                return Json(Notification);
            }
            #endregion



            //return PartialView("_Test");
        }

        public ActionResult BioCapstone()
        {
            return View();
        }

        public ActionResult Test1()
        {
            OrganTransplantViewModel model = new OrganTransplantViewModel();
            return View(model);
        }
        public ActionResult HomeopathyTest()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            HomeopathyDrugStoreViewModel model = new HomeopathyDrugStoreViewModel();



            return View(model);
        }

        #region  Organ Transplant Application
        public JsonResult addLaboratoryFacilityStaffDetails(OTStaffDetailsModel model)
        {
            if (true)
            {

                //model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<OTStaffDetailsModel> objStaffList;
                if (Session["OTStaffDetailsModel"] != null)
                    objStaffList = Session.GetDataFromSession<List<OTStaffDetailsModel>>("OTStaffDetailsModel");
                else
                    objStaffList = new List<OTStaffDetailsModel>();
                objStaffList.Add(model);
                Session.SetDataToSession<List<OTStaffDetailsModel>>("OTStaffDetailsModel", objStaffList);

                return Json(objStaffList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteLaboratoryFacilityStaffDetails(int index)
        {
            if (Session["OTStaffDetailsModel"] != null)
            {
                List<OTStaffDetailsModel> objStaffList = Session.GetDataFromSession<List<OTStaffDetailsModel>>("OTStaffDetailsModel");
                if (objStaffList[index].Id == 0)
                    objStaffList.RemoveAt(index);
                else
                    objStaffList[index].IsDeleted = true;
                Session.SetDataToSession<List<OTStaffDetailsModel>>("OTStaffDetailsModel", objStaffList);
                return Json(objStaffList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult addLaboratoryEquipments(OTEquipmentModel model)
        {
            if (true)
            {

                //model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<OTEquipmentModel> objEquipmentList;
                if (Session["OTEquipmentModel"] != null)
                    objEquipmentList = Session.GetDataFromSession<List<OTEquipmentModel>>("OTEquipmentModel");
                else
                    objEquipmentList = new List<OTEquipmentModel>();
                objEquipmentList.Add(model);
                Session.SetDataToSession<List<OTEquipmentModel>>("OTEquipmentModel", objEquipmentList);

                return Json(objEquipmentList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteLaboratoryEquipments(int index)
        {
            if (Session["OTEquipmentModel"] != null)
            {
                List<OTEquipmentModel> objEquipmentList = Session.GetDataFromSession<List<OTEquipmentModel>>("OTEquipmentModel");
                if (objEquipmentList[index].Id == 0)
                    objEquipmentList.RemoveAt(index);
                else
                    objEquipmentList[index].IsDeleted = true;
                Session.SetDataToSession<List<OTEquipmentModel>>("OTEquipmentModel", objEquipmentList);
                return Json(objEquipmentList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

       
        #endregion
        #region Homeopathy
        public JsonResult SaveHomeopathyApplicant(ApplicantViewModel model, HttpPostedFileBase UploadDocument)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["HomepathyTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("HomepathyTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "35";
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId, "Applicant");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (UploadDocument != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(UploadDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadDocument = _addressProofPath + Path.GetExtension(UploadDocument.FileName);

                    UploadDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(UploadDocument.FileName));
                }
                else if (Session["HomepathyApplicantUpload"] != null)
                {
                    model.UploadDocument = Session.GetDataFromSession<string>("HomepathyApplicantUpload");
                }
                #endregion

                LicenseBAL objBAL = new LicenseBAL();
                int result = objBAL.SaveHomeopathyApplicant(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("HomepathyTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Applicant details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = result.ToString() + "," + formStatus;
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
            }
            else
            {
                // TODO: Return model validations       - Jai, 08-09-2017
                return Json(notification);
            }
        }
        public JsonResult SaveHomeopathyEstablishment(HomeopathyEstablishmentViewModel model, HttpPostedFileBase RentalDocument, HttpPostedFileBase PlanPremisesDocument, HttpPostedFileBase AddressProff)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["HomepathyTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("HomepathyTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "35";
                var uploadsPath = Path.Combine("Establishment", model.CreatedUserId.ToString()
                    , _serviceId, "Establishment");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (RentalDocument != null)
                {
                    string _rentProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(RentalDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.RentalDocument = _rentProofPath + Path.GetExtension(RentalDocument.FileName);

                    RentalDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _rentProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _rentProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(RentalDocument.FileName));
                }
                else if (Session["HomepathyRentDocument"] != null)
                {
                    model.RentalDocument = Session.GetDataFromSession<string>("HomepathyRentDocument");
                }
                if (PlanPremisesDocument != null)
                {
                    string _premisesProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(PlanPremisesDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.PlanPremisesDocument = _premisesProofPath + Path.GetExtension(PlanPremisesDocument.FileName);

                    PlanPremisesDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _premisesProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _premisesProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(PlanPremisesDocument.FileName));
                }
                else if (Session["HomepathyPlanPremisesDocument"] != null)
                {
                    model.PlanPremisesDocument = Session.GetDataFromSession<string>("HomepathyPlanPremisesDocument");
                }
                if (AddressProff != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(AddressProff.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.AddressProff = _addressProofPath + Path.GetExtension(AddressProff.FileName);

                    AddressProff.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(AddressProff.FileName));
                }
                else if (Session["HomepathyAddressDocument"] != null)
                {
                    model.AddressProff = Session.GetDataFromSession<string>("HomepathyAddressDocument");
                }
                #endregion

                LicenseBAL objBAL = new LicenseBAL();
                int result = objBAL.SaveHomeopathyEstablishment(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("HomepathyTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Homeopthy Establishment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = result.ToString() + "," + formStatus;
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
            }
            else
            {
                // TODO: Return model validations       - Jai, 08-09-2017
                return Json(notification);
            }
        }
        public JsonResult SaveHomeopathyCompetentDetails(ApplicantViewModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["HomepathyTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("HomepathyTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                LicenseBAL objBAL = new LicenseBAL();
                int result = objBAL.SaveHomeopathyCompetent(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("HomepathyTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Applicant details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = result.ToString() + "," + formStatus;
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
            }
            else
            {
                // TODO: Return model validations       - Jai, 08-09-2017
                return Json(notification);
            }
        }
        public JsonResult SaveHomeopathyDeclaration(HomeopathyDeclaration model, HttpPostedFileBase CoveringLetter)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["HomepathyTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("HomepathyTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "35";
                var uploadsPath = Path.Combine("Declaration", model.CreatedUserId.ToString()
                    , _serviceId, "Declaration");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (CoveringLetter != null)
                {
                    string _letterProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(CoveringLetter.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.CoveringLetter = _letterProofPath + Path.GetExtension(CoveringLetter.FileName);

                    CoveringLetter.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _letterProofPath));
                    string letterfilepath = Path.Combine(Server.MapPath("~/Uploads"), _letterProofPath);
                    System.IO.File.Move(letterfilepath, letterfilepath + Path.GetExtension(CoveringLetter.FileName));
                }
                else if (Session["HomepathyLetterDocument"] != null)
                {
                    model.CoveringLetter = Session.GetDataFromSession<string>("HomepathyLetterDocument");
                }
                #endregion

                LicenseBAL objBAL = new LicenseBAL();
                int result = objBAL.SaveHomeopathyDeclaration(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("HomepathyTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Applicant details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = result.ToString() + "," + formStatus;
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
            }
            else
            {
                // TODO: Return model validations       - Jai, 08-09-2017
                return Json(notification);
            }
        }
        #endregion


        public string Payment(int Id)
        {
            LicenseBAL objBAL = new LicenseBAL();
            PaymentModel model = new PaymentModel();
            DataSet dt = objBAL.GetServicesByApplicationID(Id);

            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(dt);
            return jsonString;
            
        }
    }

}

