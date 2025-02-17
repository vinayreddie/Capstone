using Capstone.BAL;
using Capstone.Framework;
using Capstone.Models;
using Newtonsoft.Json;
//using RazorPDF;
//using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiQPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.Web.UI;
using System.Net;
using System.Net.Mail;

namespace Capstone.Areas.User.Controllers
{
    [SessionTimeout]
    public class LicenseController : Controller
    {
        LicenseBAL objBAL;
        ApplicationBAL applicationBAL;
        PCPNDTBAL pcpndtBAL;
        APMCEBAL apmceBAL;
        BloodBankBAL bloodbankBAL;
        // GET: User/License
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Questionnaire()
        {
            return View(new LicenseQuestionnaireModel());
        }

        [HttpPost]
        public ActionResult Questionnaire(LicenseQuestionnaireModel model)
        {
            ClearData();

            objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.HospitalTypesList = objBAL.GetHospitalTypes();

            if (model.HasAppliedforAPMCE)
            {
                ViewBag.OfferedServices = objBAL.GetOfferedServices();
            }

            if (model.HasAppliedforPCPNDT)
            {
                UserViewModel objUser = Session["User"] as UserViewModel;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.Name = objUser.FirstName;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.Aadhar = objUser.AadharNumber;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.PAN = objUser.PANNumber;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.StreetName = objUser.StreetName;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.MobileNo = objUser.MobileNumber;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.HouseNumber = objUser.HouseNo;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.DistrictId = objUser.DistrictId;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.MandalId = objUser.MandalId;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.VillageId = objUser.VillageId;
                model.ApplicationModel.PCPNDTModel.ApplicantModel.FormStatus = FormStatus.PartiallySaved;


                List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                objBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                ViewBag.OwnershipTypeList = ownershipTypeList;
                ViewBag.InstitutionTypeList = institutionTypeList;
                ViewBag.FacilityMaster = objBAL.GetFacilityList();
                TempData["DoctorSpecialityList"] = objBAL.GetDoctorSpecialityList();
            }

            if (model.HasAppliedforBloodBank || model.HasAppliedforBloodBankForm27E)
            {
                ViewBag.QualificationList = objBAL.GetQualifications("Blood Bank");
            }
            //if(model.HasAppliedforHomeopathicDrugStore)
            // {

            // }
            return View("ApplicationForm", model);
        }


        #region Applications
        public ViewResult ApplicationForm()
        {
            ClearData();

            objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            LicenseQuestionnaireModel model = new LicenseQuestionnaireModel();


            if (model.HasAppliedforPCPNDT)
            {
                List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                objBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                ViewBag.OwnershipTypeList = ownershipTypeList;
                ViewBag.InstitutionTypeList = institutionTypeList;
                ViewBag.FacilityMaster = objBAL.GetFacilityList();
                ViewBag.OfferedServices = objBAL.GetOfferedServices();
                TempData["DoctorSpecialityList"] = objBAL.GetDoctorSpecialityList();
            }
            return View(model);
        }
        public ViewResult ApplicationView(int TransactionId, string TransactionType)
        {
            objBAL = new LicenseBAL();
            ApplicationModel model = objBAL.GetApplication(TransactionId, Status.All, TransactionType);
            model.TransactionId = TransactionId;
            Session["APMCETransactionId"] = model.TransactionId;
            if (model.PCPNDTModel != null)
            {
                Session["EmployeeList"] = model.PCPNDTModel.EmployeeList;
                if (model.PCPNDTModel.ServiceId == 24)
                    Session["EmployeeListLog"] = model.PCPNDTModel.EmployeeListLog;
            }
            return View(model);
        }
        public ViewResult Edit(int TransactionId, string TransactionType)
        {
            apmceBAL = new APMCEBAL();
            objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.HospitalTypesList = objBAL.GetHospitalTypes();

            LicenseQuestionnaireModel model = new LicenseQuestionnaireModel();

            model.ApplicationModel = objBAL.GetApplication(TransactionId, Status.All, TransactionType);
            if (model.ApplicationModel.APMCEModel != null)
                model.HasAppliedforAPMCE = true;
            if (model.ApplicationModel.PCPNDTModel != null)
                model.HasAppliedforPCPNDT = true;
            if (model.ApplicationModel.BloodBankModel != null)
                model.HasAppliedforBloodBank = true;
            if (model.ApplicationModel.BloodBankForm27EModel != null)
                model.HasAppliedforBloodBankForm27E = true;
            if (model.ApplicationModel.OrganTransplantModel != null)
                model.HasAppliedforOrganTransplantation = true;
            if (model.ApplicationModel.BioCapstoneModel != null)
                model.HasAppliedforBioCapstone = true;
            if (model.ApplicationModel.HomeopathyDrugStore != null)
                model.HasAppliedforHomeopathicDrugStore = true;
            if (model.ApplicationModel.AllopathicDrugModel != null)
            {
                if (model.ApplicationModel.AllopathicDrugModel.ADApplicantModel.ServiceId == 36)
                {
                    model.HasAppliedforDrugStore = true;
                    model.HasAppliedforAllopathicDrugStoreForm19 = true;
                }
                else
                {
                    model.HasAppliedforDrugStore = true;
                    model.HasAppliedforAllopathicDrugStoreForm19C = true;
                }

            }
            Session["ApplicationId"] = model.ApplicationModel.Id;
            if (model.ApplicationModel.APMCEModel != null)
            {
                ViewBag.OfferedServices = objBAL.GetOfferedServicesByHospitalTypeId(model.ApplicationModel.APMCEModel.RegistrationModel.HospitalTypeId);
                Session["APMCETransactionId"] = model.ApplicationModel.APMCEModel.TransactionId;

                // Set file paths
                if (model.ApplicationModel.APMCEModel.Accommadation != null &&
                    model.ApplicationModel.APMCEModel.Accommadation.UploadedFilePath != null)
                    Session["AccommodationFilePath"] = model.ApplicationModel.APMCEModel.Accommadation.UploadedFilePath;


                Session["StaffDetailsList"] = model.ApplicationModel.APMCEModel.StaffDetailsList;
                Session["InfraStructureList"] = model.ApplicationModel.APMCEModel.InfraStructureList;
                Session["OpenAreaFilePath"] = model.ApplicationModel.APMCEModel.EstablishmentModel.OpenAreaFilePath;
                Session["ConstructionAreaFilePath"] = model.ApplicationModel.APMCEModel.EstablishmentModel.ConstructionAreaFilePath;
            }

            if (model.ApplicationModel.PCPNDTModel != null)
            {
                List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                objBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                ViewBag.OwnershipTypeList = ownershipTypeList;
                ViewBag.InstitutionTypeList = institutionTypeList;
                ViewBag.FacilityMaster = objBAL.GetFacilityList();
                TempData["DoctorSpecialityList"] = objBAL.GetDoctorSpecialityList();

                Session["PCPNDTTransactionId"] = model.ApplicationModel.PCPNDTModel.TransactionId;
                Session["EquipmentsList"] = model.ApplicationModel.PCPNDTModel.EquipmentList;
                Session["EmployeeList"] = model.ApplicationModel.PCPNDTModel.EmployeeList;
                Session["OtherUploadsList"] = model.ApplicationModel.PCPNDTModel.DeclarationModel.OtherUploadsList;

                // Set file paths
                Session["AadharCardPath"] = model.ApplicationModel.PCPNDTModel.ApplicantModel.AadharCardPath;
                Session["PANCardPath"] = model.ApplicationModel.PCPNDTModel.ApplicantModel.PANCardPath;
                Session["ApplicantPhotoPath"] = model.ApplicationModel.PCPNDTModel.ApplicantModel.ApplicantPhoto;
                Session["AddressProofPath"] = model.ApplicationModel.PCPNDTModel.FacilityModel.AddressProofPath;
                Session["BuildingLayoutPath"] = model.ApplicationModel.PCPNDTModel.FacilityModel.BuildingLayoutPath;
                Session["OwnershipDocPath"] = model.ApplicationModel.PCPNDTModel.FacilityModel.OwnerShipPath;
                Session["AffidavitDocPath"] = model.ApplicationModel.PCPNDTModel.InstitutionModel.AffidavitDocPath;
                Session["ArticleFilePath"] = model.ApplicationModel.PCPNDTModel.InstitutionModel.ArticleDocPath;
                Session["StudyCertificates"] = model.ApplicationModel.PCPNDTModel.InstitutionModel.StudyCertificateDocPaths;

            }
            if (model.ApplicationModel.BloodBankModel != null)
            {
                Session["BloodBankTransactionId"] = model.ApplicationModel.BloodBankModel.TransactionId;

                ViewBag.QualificationList = objBAL.GetQualifications("Blood Bank");
                Session["BBCApplicantDocumentPath"] = model.ApplicationModel.BloodBankModel.BloodBankApplicantModel.UploadDocument;
                Session["BBCEstablishmentDocumentPath"] = model.ApplicationModel.BloodBankModel.BloodBankEstablishmentModel.AddressProofPath;
                Session["BBCPlanPremises"] = model.ApplicationModel.BloodBankModel.BloodBankAttachments.planPremisesPath;
                Session["BBCOwnerPremises"] = model.ApplicationModel.BloodBankModel.BloodBankAttachments.OwnerPremisesPath;
                Session["BBCIdProff"] = model.ApplicationModel.BloodBankModel.BloodBankAttachments.IdProffPath;

                Session["ItemList"] = model.ApplicationModel.BloodBankModel.BloodBankList;
                Session["BloodBankEquipmentsList"] = model.ApplicationModel.BloodBankModel.EquipmentList;
                Session["BloodBankEmployeeList"] = model.ApplicationModel.BloodBankModel.EmployeeList;


            }
            if (model.ApplicationModel.BloodBankForm27EModel != null)
            {
                Session["BloodBankForm27ETransactionId"] = model.ApplicationModel.BloodBankForm27EModel.TransactionId;

                ViewBag.QualificationList = objBAL.GetQualifications("Blood Bank");
                Session["BBEApplicantDocumentPath"] = model.ApplicationModel.BloodBankForm27EModel.BloodBankApplicantModel.UploadDocument;
                Session["BBEEstablishmentDocumentPath"] = model.ApplicationModel.BloodBankForm27EModel.BloodBankEstablishmentModel.AddressProofPath;
                Session["BBEPlanPremises"] = model.ApplicationModel.BloodBankForm27EModel.BloodBankAttachments.planPremisesPath;
                Session["BBEOwnerPremises"] = model.ApplicationModel.BloodBankForm27EModel.BloodBankAttachments.OwnerPremisesPath;
                Session["BBEIdProff"] = model.ApplicationModel.BloodBankForm27EModel.BloodBankAttachments.IdProffPath;

                Session["BBEItemList"] = model.ApplicationModel.BloodBankForm27EModel.BloodBankList;
                Session["BBEEquipmentsList"] = model.ApplicationModel.BloodBankForm27EModel.EquipmentList;
                Session["BloodBankTechnicalList"] = model.ApplicationModel.BloodBankForm27EModel.TechnicalList;
                Session["BloodBankUploadList"] = model.ApplicationModel.BloodBankForm27EModel.TechnicalModel.UploadDocuments;
            }
            if (model.ApplicationModel.HomeopathyDrugStore != null)
            {
                Session["HomepathyTransactionId"] = model.ApplicationModel.HomeopathyDrugStore.TransactionId;

                Session["HomepathyApplicantUpload"] = model.ApplicationModel.HomeopathyDrugStore.HDApplicantModel.UploadDocument;
                Session["HomepathyRentDocument"] = model.ApplicationModel.HomeopathyDrugStore.HDEstablishment.RentalDocument;
                Session["HomepathyPlanPremisesDocument"] = model.ApplicationModel.HomeopathyDrugStore.HDEstablishment.PlanPremisesDocument;
                Session["HomepathyAddressDocument"] = model.ApplicationModel.HomeopathyDrugStore.HDEstablishment.AddressProff;
                Session["HomepathyLetterDocument"] = model.ApplicationModel.HomeopathyDrugStore.HDDeclaration.CoveringLetter;
            }
            if (model.ApplicationModel.BioCapstoneModel != null)
            {
                Session["BioCapstoneTransactionId"] = model.ApplicationModel.BioCapstoneModel.TransactionId;
                Session["BioCapstoneTreatmentModeList"] = model.ApplicationModel.BioCapstoneModel.TreatmentList;
                Session["BioCapstoneDisposalList"] = model.ApplicationModel.BioCapstoneModel.TreatmentDisposalList;
                Session["QuantityWasteList"] = model.ApplicationModel.BioCapstoneModel.QuantityWasteList;

            }
            if (model.ApplicationModel.AllopathicDrugModel != null)
            {
                if (model.ApplicationModel.AllopathicDrugModel.ServiceId == 36)
                {
                    Session["AD19ApplicationId"] = model.ApplicationModel.Id;
                    Session["AD19TransactionId"] = model.ApplicationModel.AllopathicDrugModel.TransactionId;
                    Session["AD19UploadList"] = model.ApplicationModel.AllopathicDrugModel.ADCompetentPersonModel.uploadedDocuments;
                    Session["AD19DrugsList"] = model.ApplicationModel.AllopathicDrugModel.AllopathicDrugList;
                }
                else
                {
                    Session["AD19CApplicationId"] = model.ApplicationModel.Id;
                    Session["AD19CTransactionId"] = model.ApplicationModel.AllopathicDrugModel.TransactionId;
                    Session["AD19CUploadList"] = model.ApplicationModel.AllopathicDrugModel.ADCompetentPersonModel.uploadedDocuments;
                    Session["AD19CDrugsList"] = model.ApplicationModel.AllopathicDrugModel.AllopathicDrugList;
                }
            }

            //ViewBag.EquipmentsList = objBAL.GetEquipmentTypesList(model.ApplicationModel.APMCEModel.RegistrationModel.HospitalTypeId);

            return View("ApplicationForm", model);
        }
        public ActionResult Applications()
        {
            objBAL = new LicenseBAL();
            int _userId = Session.GetDataFromSession<UserModel>("User").Id;
            List<ApplicationModel> objList = objBAL.GetApplicationList(_userId);
            return View(objList);
        }

        public JsonResult GetHospitalTypes()
        {
            objBAL = new LicenseBAL();
            var hospitalTypes = objBAL.GetHospitalTypes();
            return Json(hospitalTypes);
        }

        public JsonResult GetOfferedServicesByHospitalTypeId(int hospitalTypeId)
        {
            objBAL = new LicenseBAL();
            var offeredServices = objBAL.GetOfferedServicesByHospitalTypeId(hospitalTypeId);
            return Json(offeredServices);
        }
        public JsonResult GetCentresList(int DistrictId=0)
        {
            objBAL = new LicenseBAL();
            var Centres = objBAL.GetCentresList(DistrictId);
            return Json(Centres);
        }
        public JsonResult GetEquipmentBasedonOfferedServiceIds(string offeredServiceIds)
        {
            objBAL = new LicenseBAL();
            var equipment = objBAL.GetEquipmentBasedOnOfferedServices(offeredServiceIds);
            return Json(equipment);
        }

        public PartialViewResult PreviewApplication(ApplicationType applicationType)
        {
            int _applicationId = 0; int _transactionId = 0;
            if (Session["ApplicationId"] != null)
                _applicationId = Session.GetDataFromSession<int>("ApplicationId");
            if (Session["APMCETransactionId"] != null)
                _transactionId = Session.GetDataFromSession<int>("APMCETransactionId");
            Session["applicationType"] = applicationType;
            objBAL = new LicenseBAL();
            ApplicationModel application = objBAL.GetApplication(_transactionId, Status.All, "Transaction");
            return PartialView("_ApplicationPreview", application);
        }

        #region Payment
        public ActionResult Payment(int Id, string applicationNumber)
        {
            ViewBag.ApplicationNumber = applicationNumber;
            ViewBag.Data = Id;
            if (Session["APMCETransactionId"] != null)
                ViewBag.transactionId = Session.GetDataFromSession<int>("APMCETransactionId");
            else
                ViewBag.transactionId = "";
            // objBAL = new LicenseBAL();
            // PaymentModel model = new PaymentModel();
            //// model = objBAL.GetServicesByApplicationID(Id);
            return View("Payment");
        }
        public string GetPaymentDetails(int applicationId)
        {
            objBAL = new LicenseBAL();
            PaymentModel model = new PaymentModel();
            //DataSet ds = objBAL.GetServicesByApplicationID(applicationId);
            DataTable dtFee = objBAL.GetTAMCEFeeDetails(applicationId);
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(dtFee);
            return jsonString;
        }

        public ActionResult MakePayment(PaymentModel model)
        {
            var user = Session.GetDataFromSession<UserModel>("User");
            if (user.LastName != null)
                model.RazorPay.userName = user.FirstName + " " + user.LastName;
            else
                model.RazorPay.userName = user.FirstName;

            model.RazorPay.MobileNumber = user.MobileNumber;
            model.RazorPay.EmailId = user.EmailId;
            model.RazorPay.OrderId = InitiateOrder(model.RazorPay.Amount);
            return View(model);
        }

        private string InitiateOrder(decimal amount)
        {
            try
            {
                applicationBAL = new ApplicationBAL();
                var user = Session.GetDataFromSession<UserModel>("User");
                int districtId = user.DistrictId;//Session.GetDataFromSession<int>("DistrictId");
                string accountId = applicationBAL.GetDistrictPaymentAccountId(districtId);

                int applicationId = Session.GetDataFromSession<int>("ApplicationId");
                int transactionId = Session.GetDataFromSession<int>("APMCETransactionId");

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_live_mU5fUOqcSh21z5", "9YTyWg8eLLMxOunzwX9IaMLe");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount * 100); // Amount should be in sub-units (paise). So multiply amount with 100 to get amount in paise.   - Raj K, 2020-10-02
                options.Add("currency", "INR");
                options.Add("payment_capture", 1);

                Dictionary<string, string> notes = new Dictionary<string, string>();
                notes.Add("ApplicationId", applicationId.ToString());
                notes.Add("TAMCE_TransactionId", transactionId.ToString());
                options.Add("notes", notes);

                List<Dictionary<string, object>> transfers = new List<Dictionary<string, object>>();
                Dictionary<string, object> transfer = new Dictionary<string, object>();
                transfer.Add("account", accountId);     // District Account
                transfer.Add("amount", amount * 100);   // Amount should be in sub-units (paise). So multiply amount with 100 to get amount in paise.   - Raj K, 2021-01-01
                transfer.Add("currency", "INR");
                transfer.Add("notes", notes);
                List<string> linkedAccountNotes = new List<string>();
                linkedAccountNotes.Add("ApplicationId"); linkedAccountNotes.Add("TAMCE_TransactionId");
                transfer.Add("linked_account_notes", linkedAccountNotes);
                transfers.Add(transfer);

                options.Add("transfers", transfers);

                Razorpay.Api.Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ViewResult SubmitApplication(PaymentModel model)
        {
            var orderId = Request.Form["orderid"].ToString();
            var paymentId = Request.Form["paymentid"].ToString();
            var signature = Request.Form["signature"].ToString();
            var userId = Session.GetDataFromSession<UserModel>("User").Id;

            ViewBag.ApplicationNumber = model.ApplicationNumber;
            ViewBag.Amount = model.RazorPay.Amount;

            var transactionId = 0;
            if (Session["APMCETransactionId"] != null)
                transactionId = Session.GetDataFromSession<int>("APMCETransactionId");

            ViewBag.transactionId = transactionId;
            ViewBag.PaymentId = paymentId;

            int _applicationId = Session.GetDataFromSession<int>("ApplicationId");

            //string applicationType= Session.GetDataFromSession<string>("applicationType");
            string _applicationNumber = string.Empty;

            applicationBAL = new ApplicationBAL();
            var paymentSaveId = applicationBAL.SavePayment(transactionId, orderId, paymentId, signature, model.RazorPay.Amount, userId);
            int result = applicationBAL.SubmitApplication(_applicationId, userId, ref _applicationNumber);

            if (result > 0)
                ViewBag.IsApplicationSubmitted = true;
            else
                ViewBag.IsApplicationSubmitted = false;

            return View("PaymentSuccess");

            //NotificationModel notificationModel = new NotificationModel();
            //if (result > 0)
            //{
            //    notificationModel.Title = "Success";
            //    notificationModel.NotificationType = NotificationType.Success;
            //    notificationModel.NotificationMessage = "Application has been submitted successfully"
            //        + "<br>Your application number is <b>" + _applicationNumber + "</b>"
            //        + "<br>Please note this for your future reference";
            //    notificationModel.ShowActionButton = true;
            //    notificationModel.ActionButtonClassType = PopupButtonClass.Success;
            //    notificationModel.ActionButtonText = "View Applications";
            //    notificationModel.ActionName = "Submitted"; //    change action name from Submitted to Payment  -kishore 11-07-17
            //    notificationModel.ControllerName = "Dashboard";//  change Controller from Dashboard to Payment
            //    notificationModel.AreaName = "User";
            //    ClearData();
            //    SendSMS(_applicationId, "Grant");
            //}
            //else
            //{
            //    notificationModel.Title = "Error";
            //    notificationModel.NotificationType = NotificationType.Danger;
            //    notificationModel.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
            //    notificationModel.ShowNonActionButton = true;
            //    notificationModel.NonActionButtonClassType = PopupButtonClass.Danger;
            //    notificationModel.NonActionButtonText = "Okay";
            //}

            //return Json(notificationModel);
        }

        public string GetUserPaymentDetails(int tamceTransactionId)
        {
            applicationBAL = new ApplicationBAL();
            DataTable data = applicationBAL.GetPaymentDetails(tamceTransactionId);
            return JsonConvert.SerializeObject(data);
        }

        public ViewResult PaymentSuccess()
        {
            return View();
        }
        #endregion



        private bool SendSMS(int ApplicationId, string ApplicationType)
        {
            SMSModel smsData = applicationBAL.GetSMSDetails(ApplicationId, 0, 0, ApplicationType);
            string ApplicantMsg = "Hi " + smsData.ApplicantName + "," + " Application Number :  " + smsData.ApplicationNumber + ". Thanks For Applying. Once Department Review the application we will acknowledge you. ";
            string DeptMsg = "Hi " + smsData.DeptUserName + "," + " Application Number :  " + smsData.ApplicationNumber + " ." + " Applicant :" + smsData.ApplicantName + ". Had Applied for Registration ";

            var deliveryStatus = string.Empty;
            bool result = Utitlities.SendSMS(smsData.ApplicantMobileNumber, ApplicantMsg, out deliveryStatus);
            result = Utitlities.SendSMS(smsData.DeptMobile, DeptMsg, out deliveryStatus);

            //UserBAL userBAL = new UserBAL();
            //string result = userBAL.SendSMS(ApplicantMsg,smsData.ApplicantMobileNumber, "", "single");
            //result = userBAL.SendSMS(DeptMsg, smsData.DeptMobile, "", "single");
            return result;
        }
        public ActionResult Print()
        {


            objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();

            LicenseQuestionnaireModel model = new LicenseQuestionnaireModel();
            model.HasAppliedforPCPNDT = true;

            if (model.HasAppliedforPCPNDT)
            {
                List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                objBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                ViewBag.OwnershipTypeList = ownershipTypeList;
                ViewBag.InstitutionTypeList = institutionTypeList;
            }

            model.ApplicationModel = objBAL.GetApplication(175, Status.All, "Transaction");
            //return new RazorPDF.PdfResult(model, "_ApplicationPreview");

            //var pdf = new PdfResult(model, "_ApplicationPreview");
            //pdf.ViewBag.Title = "Report Title";
            //return pdf;
            return null;
        }
        #endregion

        #region PCPNDT Form
        #region Applicant saving 
        public JsonResult SaveApplicantDetails(ApplicantModel model, HttpPostedFileBase ApplicantPhoto, HttpPostedFileBase AadharCardPath, HttpPostedFileBase PANCardPath)
        {
            NotificationModel notification = new NotificationModel();

            if (true)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                int _serviceId = 2;  //PCPNDT Certificate of Registration 
                try
                {
                    #region File Saving
                    var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                        , _serviceId.ToString(), "ApplicantDetails");

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    if (ApplicantPhoto != null)
                    {
                        string _applicantPhotoPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(ApplicantPhoto.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.ApplicantPhoto = _applicantPhotoPath + Path.GetExtension(ApplicantPhoto.FileName);

                        ApplicantPhoto.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _applicantPhotoPath));
                        string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _applicantPhotoPath);
                        System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(ApplicantPhoto.FileName));
                    }
                    else if (Session["ApplicantPhotoPath"] != null)
                    {
                        model.ApplicantPhoto = Session.GetDataFromSession<string>("ApplicantPhotoPath");
                    }


                    if (AadharCardPath != null)
                    {
                        string _aadharCardPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(AadharCardPath.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.AadharCardPath = _aadharCardPath + Path.GetExtension(AadharCardPath.FileName);

                        AadharCardPath.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _aadharCardPath));
                        string aadharcardfilepath = Path.Combine(Server.MapPath("~/Uploads"), _aadharCardPath);
                        System.IO.File.Move(aadharcardfilepath, aadharcardfilepath + Path.GetExtension(AadharCardPath.FileName));
                    }
                    else if (Session["AadharCardPath"] != null)
                    {
                        model.AadharCardPath = Session.GetDataFromSession<string>("AadharCardPath");
                    }

                    if (PANCardPath != null)
                    {
                        string _panCardPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(PANCardPath.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.PANCardPath = _panCardPath + Path.GetExtension(PANCardPath.FileName);

                        PANCardPath.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _panCardPath));
                        string pancardfilepath = Path.Combine(Server.MapPath("~/Uploads"), _panCardPath);
                        System.IO.File.Move(pancardfilepath, pancardfilepath + Path.GetExtension(PANCardPath.FileName));
                    }
                    else if (Session["PANCardPath"] != null)
                    {
                        model.PANCardPath = Session.GetDataFromSession<string>("PANCardPath");
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    notification.Title = "Error";
                    notification.NotificationType = NotificationType.Danger;
                    notification.NotificationMessage = "File names should not be same.";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = "0," + FormStatus.Empty;
                    return Json(notification);
                }
                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();

                int result = objPCPNDTBAL.SaveApplicantDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

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
                // TODO: Return model validations       - Raj, 12-05-2017
                return Json(notification);
            }
        }
        #endregion

        //[HttpPost]
        //public ActionResult SaveEstablishmentDetails(EstablishmentModel model, HttpPostedFileBase AddressProof, HttpPostedFileBase BuildingLayout)
        //{
        //    NotificationModel notification = new NotificationModel();

        //    if (ModelState.IsValid)
        //    {
        //        PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();

        //        int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
        //        int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
        //        FormStatus formStatus = model.FormStatus;
        //        string _applicationNumber = string.Empty;

        //        int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 10-05-2017
        //        model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

        //        #region File Saving
        //        var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
        //            , _serviceId.ToString(), "Establishment");

        //        if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
        //            Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

        //        if (AddressProof != null)
        //        {
        //            string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(AddressProof.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        //            model.AddressProofPath = _addressProofPath + Path.GetExtension(AddressProof.FileName);

        //            AddressProof.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
        //            string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
        //            System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(AddressProof.FileName));
        //        }
        //        else if (Session["AddressProofPath"] != null)
        //        {
        //            model.AddressProofPath = Session.GetDataFromSession<string>("AddressProofPath");
        //        }

        //        if (BuildingLayout != null)
        //        {
        //            string _buildingLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(BuildingLayout.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        //            model.BuildingLayoutPath = _buildingLayoutPath + Path.GetExtension(BuildingLayout.FileName);

        //            BuildingLayout.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath));
        //            string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath);
        //            System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(BuildingLayout.FileName));
        //        }
        //        else if (Session["BuildingLayoutPath"] != null)
        //        {
        //            model.AddressProofPath = Session.GetDataFromSession<string>("BuildingLayoutPath");
        //        }
        //        #endregion

        //        int result = objPCPNDTBAL.SaveEstablishmentDetails(model, ref _applicationId, ref _transactionId,
        //            ref formStatus, ref _applicationNumber);

        //        if (result > 0)
        //        {
        //            Session.SetDataToSession<int>("ApplicationId", _applicationId);
        //            Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

        //            notification.Title = "Success";
        //            notification.NotificationType = NotificationType.Success;
        //            notification.NotificationMessage = "Establishment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
        //            notification.ShowNonActionButton = true;
        //            notification.NonActionButtonClassType = PopupButtonClass.Success;
        //            notification.NonActionButtonText = "Okay";
        //            notification.ReturnData = result.ToString() + "," + formStatus;
        //        }
        //        else
        //        {
        //            notification.Title = "Error";
        //            notification.NotificationType = NotificationType.Danger;
        //            notification.NotificationMessage = "Something went wrong! Please contact Help desk";
        //            notification.ShowNonActionButton = true;
        //            notification.NonActionButtonClassType = PopupButtonClass.Danger;
        //            notification.NonActionButtonText = "Okay";
        //            notification.ReturnData = "0," + FormStatus.Empty;
        //        }

        //        return Json(notification);
        //    }
        //    else
        //    {
        //        // TODO: Return model validations       - Raj, 10-05-2017
        //        return Json(notification);
        //    }
        //}
        [HttpPost]
        #region Facility saving
        public ActionResult SaveFacilityDetails(FacilityModel model, HttpPostedFileBase AddressProof, HttpPostedFileBase BuildingLayout, HttpPostedFileBase Ownership)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 10-05-2017
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                try
                {

                    #region File Saving
                    var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                        , _serviceId.ToString(), "FacilityDetails");

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    if (AddressProof != null)
                    {
                        string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(AddressProof.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.AddressProofPath = _addressProofPath + Path.GetExtension(AddressProof.FileName);

                        AddressProof.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                        string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                        System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(AddressProof.FileName));
                    }
                    else if (Session["AddressProofPath"] != null)
                    {
                        model.AddressProofPath = Session.GetDataFromSession<string>("AddressProofPath");
                    }

                    if (BuildingLayout != null)
                    {
                        string _buildingLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(BuildingLayout.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.BuildingLayoutPath = _buildingLayoutPath + Path.GetExtension(BuildingLayout.FileName);

                        BuildingLayout.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath));
                        string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath);
                        System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(BuildingLayout.FileName));
                    }
                    else if (Session["BuildingLayoutPath"] != null)
                    {
                        model.BuildingLayoutPath = Session.GetDataFromSession<string>("BuildingLayoutPath");
                    }
                    if (Ownership != null)
                    {
                        string _ownershipDocPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(Ownership.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.OwnerShipPath = _ownershipDocPath + Path.GetExtension(Ownership.FileName);

                        Ownership.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _ownershipDocPath));
                        string ownershipFilePath = Path.Combine(Server.MapPath("~/Uploads"), _ownershipDocPath);
                        System.IO.File.Move(ownershipFilePath, ownershipFilePath + Path.GetExtension(Ownership.FileName));

                    }
                    else if (Session["OwnershipDocPath"] != null)
                    {
                        model.OwnerShipPath = Session.GetDataFromSession<string>("OwnershipDocPath");
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    notification.Title = "Error";
                    notification.NotificationType = NotificationType.Danger;
                    notification.NotificationMessage = "File names should not be same.";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = "0," + FormStatus.Empty;
                    return Json(notification);
                }
                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                int result = objPCPNDTBAL.SaveFacilityDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facility details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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

                return Json(notification, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // TODO: Return model validations       - Raj, 12-05-2017
                return Json(notification);
            }
        }
        #endregion

        #region Tests Saving
        public JsonResult SaveTests(TestsModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();

                int result = objPCPNDTBAL.SaveTests(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Test details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 12-05-2017
                return Json(notification);
            }
        }
        #endregion 

        #region Equipment Saving
        [HttpPost]
        public ActionResult AddEquipment(EquipmentModel model, HttpPostedFileBase NocFile, HttpPostedFileBase TransferCertificate, HttpPostedFileBase InstallationFile, HttpPostedFileBase Image, HttpPostedFileBase InvoiceFile)
        {
            if (model.Type == "New")
                ModelStateErrorHandler.RemoveError(ModelState, "Type");
            if (ModelState.IsValid)
            {
                //HttpPostedFileBase _uploadedFile = Request.Files[0];                
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");

                int _serviceId = 2;  // TODO: Set this value from m_service table        - Raj, 10-05-2017
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;


                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId.ToString(), "Equipment");


                //string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                //model.UploadedFilePath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);                
                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (InstallationFile != null)
                {
                    string InstallationFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(InstallationFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.InstallationCerticatePath = InstallationFilePath + Path.GetExtension(InstallationFile.FileName);
                    InstallationFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), InstallationFilePath));
                    string oldInstallationPath = Path.Combine(Server.MapPath("~/Uploads"), InstallationFilePath);
                    System.IO.File.Move(oldInstallationPath, oldInstallationPath + Path.GetExtension(InstallationFile.FileName));
                }
                if (Image != null)
                {
                    string ImagePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(Image.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                    model.ImagePath = ImagePath + Path.GetExtension(Image.FileName);

                    Image.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), ImagePath));


                    string oldImagePath = Path.Combine(Server.MapPath("~/Uploads"), ImagePath);
                    System.IO.File.Move(oldImagePath, oldImagePath + Path.GetExtension(Image.FileName));
                }

                if (model.Type == "Old")
                {
                    string NOCFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(NocFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string TCFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(TransferCertificate.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.NocFilePath = NOCFilePath + Path.GetExtension(NocFile.FileName);
                    model.TransferCertificatePath = TCFilePath + Path.GetExtension(TransferCertificate.FileName);


                    #region Saving files physically
                    //_uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));
                    NocFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), NOCFilePath));
                    TransferCertificate.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), TCFilePath));


                    //string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    //System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));              

                    string oldNOCPath = Path.Combine(Server.MapPath("~/Uploads"), NOCFilePath);
                    System.IO.File.Move(oldNOCPath, oldNOCPath + Path.GetExtension(NocFile.FileName));

                    string oldTCPath = Path.Combine(Server.MapPath("~/Uploads"), TCFilePath);
                    System.IO.File.Move(oldTCPath, oldTCPath + Path.GetExtension(NocFile.FileName));



                    #endregion
                }
                else
                {
                    string InvoiceFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(InvoiceFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.InvoicePath = InvoiceFilePath + Path.GetExtension(InvoiceFile.FileName);
                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    #region Saving file physically 
                    InvoiceFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), InvoiceFilePath));
                    string oldInvoicePath = Path.Combine(Server.MapPath("~/Uploads"), InvoiceFilePath);
                    System.IO.File.Move(oldInvoicePath, oldInvoicePath + Path.GetExtension(InvoiceFile.FileName));
                    #endregion
                }



                List<EquipmentModel> objEquipmentsList;
                if (Session["EquipmentsList"] != null)
                    objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("EquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                else
                    objEquipmentsList = new List<EquipmentModel>();
                objEquipmentsList.Add(model);
                Session.SetDataToSession<List<EquipmentModel>>("EquipmentsList", objEquipmentsList);
                //TempData["EquipmentsList"] = objEquipmentsList;

                return Json(objEquipmentsList.Where(x => x.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }

        }
        public JsonResult DeleteEquipment(int index)
        {
            if (Session["EquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("EquipmentsList");
                List<EquipmentModel> DeletedList = objEquipmentsList.Where(x => x.IsDeleted == true).ToList();
                List<EquipmentModel> NotDeletedList = objEquipmentsList.Where(x => x.IsDeleted == false).ToList();
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                if (NotDeletedList[index].Id == 0)
                    NotDeletedList.RemoveAt(index);
                else
                    NotDeletedList[index].IsDeleted = true;
                objEquipmentsList = DeletedList.Concat(NotDeletedList).ToList();
                Session.SetDataToSession<List<EquipmentModel>>("EquipmentsList", objEquipmentsList);
                //TempData["EquipmentsList"] = objEquipmentsList;
                return Json(objEquipmentsList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public ActionResult SaveEquipments(string ApplicationType, int ExistingApplicationId)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["EquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("EquipmentsList");
                //TempData.Peek("EquipmentsList") as List<EquipmentModel>;
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEquipmentsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                int result = objPCPNDTBAL.SaveEquipments(objEquipmentsList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, ApplicationType, ExistingApplicationId);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetEquipments(int transactionId)
        {
            pcpndtBAL = new PCPNDTBAL();
            List<EquipmentModel> equipmentList = pcpndtBAL.GetEquipments(transactionId);
            Session.SetDataToSession<List<EquipmentModel>>("EquipmentsList", equipmentList);
            return Json(equipmentList);
        }
        #endregion

        #region Facilities Saving
        public JsonResult SaveFacilities(FacilitesModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                int result = objPCPNDTBAL.SaveFacilities(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facilities details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 12-05-2017
                return Json(notification);
            }
        }
        #endregion 

        #region Employee Saving
        [HttpPost]
        public JsonResult AddEmployee(EmployeeViewModel model)
        {
            HttpFileCollectionBase files = Request.Files;
            if (ModelState.IsValid)
            {
                List<EmployeeViewModel> objEmployeeList;
                if (Session["EmployeeList"] != null)
                    objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("EmployeeList");
                // TempData["EmployeeList"] as List<EmployeeViewModel>;
                else
                    objEmployeeList = new List<EmployeeViewModel>();

                // Assign Employee Documents
                model.UploadDocuments = new List<DocumentUploadModel>();
                if (Session["EmployeeUploadedDocuments"] != null)
                {
                    model.UploadDocuments = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeUploadedDocuments");
                    Session["EmployeeUploadedDocuments"] = null;
                }

                #region Save Education Certificates
                var educationCertificates = new List<HttpPostedFileBase>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files.AllKeys[i] == "educationCertificates")
                        educationCertificates.Add(Request.Files[i]);
                }

                DocumentUploadModel educationCertificate;
                foreach (var uploadedFile in educationCertificates)
                {
                    educationCertificate = new DocumentUploadModel();
                    int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                    int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");

                    int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 10-05-2017
                    int userId = Session.GetDataFromSession<UserModel>("User").Id;

                    var uploadsPath = Path.Combine("Applicant", userId.ToString()
                        , _serviceId.ToString(), "Employee");

                    string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    educationCertificate.DocumentPath = _uploadedFilePath + Path.GetExtension(uploadedFile.FileName);
                    educationCertificate.UploadType = "Education Certificate";
                    educationCertificate.UploadedUserId = userId;
                    educationCertificate.ReferenceTable = "t_employee";

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    #region Saving files physically
                    uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                    string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(uploadedFile.FileName));

                    model.UploadDocuments.Add(educationCertificate);

                    #endregion
                }
                #endregion

                objEmployeeList.Add(model);
                Session.SetDataToSession<List<EmployeeViewModel>>("EmployeeList", objEmployeeList);
                //TempData["EmployeeList"] = objEmployeeList;

                return Json(objEmployeeList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult CheckforEmployeeRegistration(string registrationNumber)
        {
            PCPNDTBAL obj = new PCPNDTBAL();
            var employeeRegistrations = obj.CheckforEmployeeRegistration(registrationNumber);
            return Json(new
            {
                EmployeeRegistrations = employeeRegistrations
            });
        }
        public JsonResult GetEmployeeUploads(int index, string source = "New")
        {
            var user = Session.GetDataFromSession<UserModel>("User");

            var sessionKey = string.Empty;
            if (source == "New")
                sessionKey = "EmployeeList";
            else if (source == "Amendment")
                sessionKey = "amtEmployeeList";
            else if (source == "Department")
                sessionKey = "EmployeeListLog"; // used in DepartmentController     - Raj K, 18-01-2018

            if (user.RoleId == 4)
                sessionKey = "EmployeeListLog";     // RoleId : 4 i.e., Dept User. used in DepartmentController     - Raj K, 18-01-2018

            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            if (source == "New")
                employees = Session.GetDataFromSession<List<EmployeeViewModel>>(sessionKey)
                    .Where(item => item.IsDeleted == false).ToList();
            else
                employees = Session.GetDataFromSession<List<EmployeeViewModel>>(sessionKey);

            var uploads = employees[index].UploadDocuments;
            return Json(new {
                EmployeeDescription = employees[index].Name + " - " + employees[index].RegistrationNumber,
                Uploads = uploads
            });
        }
        public JsonResult DeleteEmployee(int index)
        {
            if (Session["EmployeeList"] != null)
            {
                List<EmployeeViewModel> objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("EmployeeList")
                    .Where(item => item.IsDeleted == false).ToList();
                if (objEmployeeList[index].Id == 0)
                    objEmployeeList.RemoveAt(index);
                else
                    objEmployeeList[index].IsDeleted = true;
                Session.SetDataToSession<List<EmployeeViewModel>>("EmployeeList", objEmployeeList);
                //TempData["EmployeeList"] = objEmployeeList;
                return Json(objEmployeeList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public ActionResult SaveEmployees(string ApplicationType, int ExistingApplicationId)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["EmployeeList"] != null)
            {
                List<EmployeeViewModel> objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("EmployeeList");
                //TempData.Peek("EmployeeList") as List<EmployeeViewModel>;
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEmployeeList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                int result = objPCPNDTBAL.SaveEmployees(objEmployeeList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, ApplicationType, ExistingApplicationId);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Employee details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetEmployees(int transactionId)
        {
            pcpndtBAL = new PCPNDTBAL();
            List<EmployeeViewModel> employeeList = pcpndtBAL.GetEmployees(transactionId);
            Session.SetDataToSession<List<EmployeeViewModel>>("EmployeeList", employeeList);
            return Json(employeeList);
        }

        [HttpPost]
        public JsonResult UploadEmployeeDocument(string fileType, HttpPostedFileBase uploadedFile)
        {
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
            List<DocumentUploadModel> uploadedDocsList;
            if (Session["EmployeeUploadedDocuments"] != null)
                uploadedDocsList = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeUploadedDocuments");
            else
                uploadedDocsList = new List<DocumentUploadModel>();

            var user = Session.GetDataFromSession<UserModel>("User");

            DocumentUploadModel uploadedDocument = new DocumentUploadModel();

            int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 10-05-2017

            var uploadsPath = Path.Combine("Applicant", user.Id.ToString()
                , _serviceId.ToString(), "Employee");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            uploadedDocument.DocumentPath = _uploadedFilePath + Path.GetExtension(uploadedFile.FileName);
            uploadedDocument.UploadType = fileType;
            uploadedDocument.ReferenceTable = "t_employee";
            uploadedDocument.UploadedUserId = user.Id;

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            #region Saving files physically
            uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(uploadedFile.FileName));

            uploadedDocsList.Add(uploadedDocument);
            #endregion

            Session["EmployeeUploadedDocuments"] = uploadedDocsList;

            return Json(new
            {
                DocumentsList = uploadedDocsList
            });
        }

        public JsonResult DeleteEmployeeUpload(int index)
        {
            var uploadedDocsList = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeUploadedDocuments");
            uploadedDocsList.RemoveAt(index);
            Session["EmployeeUploadedDocuments"] = uploadedDocsList;
            return Json(uploadedDocsList);
        }

        public JsonResult ClearEmployeeUploads()
        {
            Session["EmployeeUploadedDocuments"] = null;
            return null;
        }

        #endregion

        #region Institution & Ownership saving
        public JsonResult SaveInstitution(InstitutionModel model, HttpPostedFileBase affidavitFile,
            HttpPostedFileBase articleFile, HttpPostedFileBase[] studyCertificateFiles)
        {
            NotificationModel notification = new NotificationModel();

            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
            FormStatus formStatus = model.FormStatus;
            string _applicationNumber = string.Empty;
            int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 06-06-2017
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

            #region File Saving
            var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                , _serviceId.ToString(), "InstitutionDetails");

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            if (model.OwnershipTypeId > 0)
            {
                // Saving Affidavit Doc
                if (affidavitFile != null)
                {
                    string _affidavitDocPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(affidavitFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.AffidavitDocPath = _affidavitDocPath + Path.GetExtension(affidavitFile.FileName);

                    affidavitFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _affidavitDocPath));
                    string affidavitfilepath = Path.Combine(Server.MapPath("~/Uploads"), _affidavitDocPath);
                    System.IO.File.Move(affidavitfilepath, affidavitfilepath + Path.GetExtension(affidavitFile.FileName));
                }
                else if (Session["AffidavitDocPath"] != null)
                {
                    model.AffidavitDocPath = Session.GetDataFromSession<string>("AffidavitDocPath");
                }

                // Saving Study Certificates
                if (studyCertificateFiles != null && studyCertificateFiles.Length > 0)
                {
                    model.StudyCertificateDocPaths = new List<DocumentUploadModel>();
                    DocumentUploadModel docModel;
                    foreach (var file in studyCertificateFiles)
                    {
                        string _certificateDocPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        docModel = new DocumentUploadModel();
                        docModel.ReferenceTable = "t_institution";
                        docModel.DocumentPath = _certificateDocPath + Path.GetExtension(file.FileName);
                        docModel.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                        model.StudyCertificateDocPaths.Add(docModel);

                        file.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _certificateDocPath));
                        string certificatefilepath = Path.Combine(Server.MapPath("~/Uploads"), _certificateDocPath);
                        System.IO.File.Move(certificatefilepath, certificatefilepath + Path.GetExtension(file.FileName));
                    }

                    // Check for any Study Certificates in Session, if so, add them to model
                    if (Session["StudyCertificates"] != null)
                        model.StudyCertificateDocPaths.AddRange(Session.GetDataFromSession<List<DocumentUploadModel>>("StudyCertificates"));
                }
                else
                {
                    model.StudyCertificateDocPaths = Session.GetDataFromSession<List<DocumentUploadModel>>("StudyCertificates");
                }

                if (model.StudyCertificateDocPaths == null || model.StudyCertificateDocPaths.Count(item => item.IsDeleted == false) == 0)
                {
                    notification.Title = "Validation";
                    notification.NotificationType = NotificationType.Warning;
                    notification.NotificationMessage = "Please upload certificates";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Warning;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = model.Id + "," + FormStatus.Empty;
                    return Json(notification);
                }

            }

            if (model.OwnershipTypeId == 2 || model.OwnershipTypeId == 3 || model.OwnershipTypeId == 4 ||
                model.OwnershipTypeId == 5)
            {
                // Saving Article Doc
                if (articleFile != null)
                {
                    string _articleDocPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(articleFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.ArticleDocPath = _articleDocPath + Path.GetExtension(articleFile.FileName);

                    articleFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _articleDocPath));
                    string articlefilepath = Path.Combine(Server.MapPath("~/Uploads"), _articleDocPath);
                    System.IO.File.Move(articlefilepath, articlefilepath + Path.GetExtension(articleFile.FileName));
                }
                else if (Session["ArticleFilePath"] != null)
                {
                    model.ArticleDocPath = Session.GetDataFromSession<string>("ArticleFilePath");
                }
            }

            #endregion



            PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
            int result = objPCPNDTBAL.SaveInstitutionDetails(model, ref _applicationId, ref _transactionId,
                ref formStatus, ref _applicationNumber);
            if (result > 0)
            {
                Session.SetDataToSession<int>("ApplicationId", _applicationId);
                Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);

                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Institution details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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

            return Json(notification, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Declaration saving

        public JsonResult AddOtherUploads(DocumentUploadModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase _uploadedFile = Request.Files[0];

                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");

                int _serviceId = 2;
                model.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;


                var uploadsPath = Path.Combine("Applicant", model.UploadedUserId.ToString()
                    , _serviceId.ToString(), "OtherUploads");

                string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                model.DocumentPath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                #region Saving files physically
                _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                #endregion

                List<DocumentUploadModel> objOtherUploadsList;
                if (Session["OtherUploadsList"] != null)
                    objOtherUploadsList = Session.GetDataFromSession<List<DocumentUploadModel>>("OtherUploadsList");
                // TempData["EmployeeList"] as List<EmployeeViewModel>;
                else
                    objOtherUploadsList = new List<DocumentUploadModel>();
                objOtherUploadsList.Add(model);
                Session.SetDataToSession<List<DocumentUploadModel>>("OtherUploadsList", objOtherUploadsList);

                return Json(objOtherUploadsList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteDeclarationOtherUploads(int index)
        {
            if (Session["OtherUploadsList"] != null)
            {
                List<DocumentUploadModel> objotheruploadlist = Session.GetDataFromSession<List<DocumentUploadModel>>("OtherUploadsList");
                List<DocumentUploadModel> DeletedList = objotheruploadlist.Where(x => x.IsDeleted == true).ToList();
                List<DocumentUploadModel> NotDeletedList = objotheruploadlist.Where(x => x.IsDeleted == false).ToList();

                if (NotDeletedList[index].Id == 0)
                    NotDeletedList.RemoveAt(index);
                else
                    NotDeletedList[index].IsDeleted = true;

                objotheruploadlist = DeletedList.Concat(NotDeletedList).ToList();

                Session.SetDataToSession<List<DocumentUploadModel>>("OtherUploadsList", objotheruploadlist);
                List<DocumentUploadModel> tempuploadlist = objotheruploadlist.Where(item => item.IsDeleted == false).ToList();

                //TempData["EmployeeList"] = objEmployeeList;
                return Json(objotheruploadlist.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public JsonResult SaveDeclarationDetails(DeclarationModel model, HttpPostedFileBase SignatureUpload)
        {
            NotificationModel notification = new NotificationModel();

            if (true)
            {
                if (Session["OtherUploadsList"] != null)
                {
                    List<DocumentUploadModel> objOtherUploadList = Session.GetDataFromSession<List<DocumentUploadModel>>("OtherUploadsList");
                    int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                    objOtherUploadList
                        .ForEach(e =>
                        {
                            e.UploadedUserId = _userId;
                            e.ReferenceTable = "t_Declaration";

                        });
                    model.OtherUploadsList = objOtherUploadList;
                }

                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                int _serviceId = 2;// PCPNdT ServiceId
                if (SignatureUpload != null)
                {

                    var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                                      , _serviceId.ToString(), "Equipment");

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    string SignatureFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(SignatureUpload.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.SignatureDocPath = SignatureFilePath + Path.GetExtension(SignatureUpload.FileName);
                    SignatureUpload.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), SignatureFilePath));
                    string oldSignaturePath = Path.Combine(Server.MapPath("~/Uploads"), SignatureFilePath);
                    System.IO.File.Move(oldSignaturePath, oldSignaturePath + Path.GetExtension(SignatureUpload.FileName));

                }

                int result = objPCPNDTBAL.SaveDeclarationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("PCPNDTTransactionId", _transactionId);
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Declaration details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 20-05-2017

                return Json(notification);
            }
        }
        public JsonResult GetOtherUploads()
        {
            objBAL = new LicenseBAL();
            int _transactionId = Session.GetDataFromSession<int>("PCPNDTTransactionId");
            int _userId = Session.GetDataFromSession<UserModel>("User").Id;
            List<DocumentUploadModel> otherUploads = objBAL.GetOthersUploadData(_transactionId, _userId);

            Session.SetDataToSession<List<DocumentUploadModel>>("OtherUploadsList", otherUploads);

            return Json(otherUploads);
        }
        #endregion

        #region Enclosures Count display
        public int EnclosureCount(string ServiceName)
        {
            int count = 0;
            if (ServiceName == "PCPNDT")
            {
                int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                count = objPCPNDTBAL.GetEnclosuresCnt(_transactionId);
            }
            else if (ServiceName == "APMCE")
            {
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                count = objPCPNDTBAL.GetEnclosuresCnt(_transactionId);
            }
            else if (ServiceName == "BloodBankForm27C")
            {
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                count = objPCPNDTBAL.GetEnclosuresCnt(_transactionId);
            }
            else if (ServiceName == "BloodBankForm27E")
            {
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                PCPNDTBAL objPCPNDTBAL = new PCPNDTBAL();
                count = objPCPNDTBAL.GetEnclosuresCnt(_transactionId);
            }
            return count;
        }
        #endregion
        #endregion

        #region APMCE Form
        public JsonResult SaveRegistrationDetails(RegistrationModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveRegistrationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);
                    Session.SetDataToSession<int>("DistrictId", model.DistrictId);  // this will be used to get account id to make payment  - Raj K, 2021-01-01

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Registration details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
            return null;
        }
        public JsonResult SaveCorrespondingAddressDetails(CorrespondingAddressModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                APMCEBAL objAPMCEBAL = new APMCEBAL();

                int result = objAPMCEBAL.SaveCorrespondingAddressDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Corresponding details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 31-05-2017
                return Json(notification);
            }
        }
        public JsonResult SaveTrustDetails(TrustModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveTrustDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Trust details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
            return null;
        }
        public JsonResult SaveAccommodationDetails(AccommodationModel model, HttpPostedFileBase uploadedFile)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 10-05-2017
                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                var uploadsPath = Path.Combine("Applicant", model.UserId.ToString()
                    , _serviceId.ToString(), "AccommodationDetails");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                if (uploadedFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "Accommodation_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadedFilePath = _uploadedfilePath + Path.GetExtension(uploadedFile.FileName);

                    uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(uploadedFile.FileName));
                }
                else if (Session["AddressProofPath"] != null)
                {
                    model.UploadedFilePath = Session.GetDataFromSession<string>("AccommodationFilePath");
                }

                #endregion

                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveAccommodationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Accommodation details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 22-06-2017
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please clear all validations.";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
                notification.ReturnData = "0," + FormStatus.Empty;
                return Json(notification);
            }
        }
        public JsonResult AddInfraStructure(InfraStructureModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase _uploadedFile = null;
                if (Request.Files.Count > 0)
                    _uploadedFile = Request.Files[0];

                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");

                int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 01-06-2017
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;


                List<InfraStructureModel> objInfraStructureList;
                if (Session["InfraStructureList"] != null)
                    objInfraStructureList = Session.GetDataFromSession<List<InfraStructureModel>>("InfraStructureList");
                else
                    objInfraStructureList = new List<InfraStructureModel>();

                #region already exists Validation
                bool alreadyExists = objInfraStructureList.Any(item => item.EquipmentId == model.EquipmentId);
                if (alreadyExists)
                    return Json(null);
                #endregion

                #region Saving files physically
                if (_uploadedFile != null)
                {
                    var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                        , _serviceId.ToString(), "InfraStructure");

                    string equipmentNameWithoutSpaces = model.Name.Replace(" ","");
                    equipmentNameWithoutSpaces = equipmentNameWithoutSpaces.Replace("/", "_");
                    //string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedFilePath = uploadsPath + "/" + "Infra_" + equipmentNameWithoutSpaces + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedFilePath = _uploadedFilePath.Replace(" ", "");
                    model.UploadedFilePath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                    string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                }
                #endregion

                objInfraStructureList.Add(model);
                Session.SetDataToSession<List<InfraStructureModel>>("InfraStructureList", objInfraStructureList);

                return Json(objInfraStructureList);
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteInfraStructure(int index)
        {
            if (Session["InfraStructureList"] != null)
            {
                List<InfraStructureModel> objInfraStructureList = Session.GetDataFromSession<List<InfraStructureModel>>("InfraStructureList");
                if (objInfraStructureList[index].Id == 0)
                    objInfraStructureList.RemoveAt(index);
                else
                    objInfraStructureList[index].IsDeleted = true;
                Session.SetDataToSession<List<InfraStructureModel>>("InfraStructureList", objInfraStructureList);
                //TempData["EmployeeList"] = objEmployeeList;
                return Json(objInfraStructureList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public JsonResult SaveInfraStructures(ApplicationType applicationType, int ExistingApplicationId)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["InfraStructureList"] != null)
            {
                List<InfraStructureModel> objInfraStructureList = Session.GetDataFromSession<List<InfraStructureModel>>("InfraStructureList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objInfraStructureList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveInfraStructure(objInfraStructureList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType, ExistingApplicationId);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipments & Furniture(InfraStructure) details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetInfraStructures(int transactionId)
        {
            apmceBAL = new APMCEBAL();
            List<InfraStructureModel> infraStructureList = apmceBAL.GetInfraStructures(transactionId);
            Session.SetDataToSession<List<EquipmentModel>>("InfraStructureList", infraStructureList);
            return Json(infraStructureList);
        }
        public JsonResult SaveEstablishmentDetails(EstablishmentModel model, HttpPostedFileBase OpenAreaUploadedFile, HttpPostedFileBase ConstructionAreaUploadedFile)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                int _serviceId = 1;  // TODO: Set this value from m_service table        
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region Files Saving

                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId.ToString(), "EstablishmentDetails");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                #region OpenArea Filesaving
                HttpPostedFileBase OpenAreaUploadedFileNew = Request.Files[0];
                HttpPostedFileBase ConstructionAreaUploadedFileNew = Request.Files[1];
                if (OpenAreaUploadedFileNew != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(OpenAreaUploadedFileNew.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "OpenArea_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.OpenAreaFilePath = _uploadedfilePath + Path.GetExtension(OpenAreaUploadedFileNew.FileName);

                    OpenAreaUploadedFileNew.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(OpenAreaUploadedFileNew.FileName));
                }
                else if (Session["EstablishmentOpenAreaFilePath"] != null)
                {
                    model.OpenAreaFilePath = Session.GetDataFromSession<string>("EstablishmentOpenAreaFilePath");
                }
                #endregion

                #region ConstructionArea File saving

                if (ConstructionAreaUploadedFileNew != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(ConstructionAreaUploadedFileNew.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "ConstructionArea_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.ConstructionAreaFilePath = _uploadedfilePath + Path.GetExtension(ConstructionAreaUploadedFileNew.FileName);

                    ConstructionAreaUploadedFileNew.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(ConstructionAreaUploadedFileNew.FileName));
                }
                else if (Session["EstablishmentConstructionAreaFilePath"] != null)
                {
                    model.ConstructionAreaFilePath = Session.GetDataFromSession<string>("EstablishmentConstructionAreaFilePath");
                }
                #endregion

                #endregion

                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveEstablishmentDetails(model, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Establishment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 22-06-2017
                return Json(notification);
            }
        }

        public JsonResult SaveServicesOfferedDetails(OfferedServicesModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                APMCEBAL objAPMCEBAL = new APMCEBAL();

                int result = objAPMCEBAL.SaveServicesOfferedDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, model.ApplicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Offered Services details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       
                return Json(notification);
            }
        }

        public JsonResult AddStaffDetails(StaffDetailsModel model)
        {
            // if (ModelState.IsValid)
            if (true)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase _uploadedFile = Request.Files[0];
                    int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                    int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");

                    int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 01-06-2017
                    model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                    #region Saving files physically
                    var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                        , _serviceId.ToString(), "StaffDetails");

                    //string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedFilePath = uploadsPath + "/" + model.StaffDesignation.Replace(" ","") +"_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadedFilePath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                    string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                    #endregion
                }
                List<StaffDetailsModel> objStaffDetailsList;
                if (Session["StaffDetailsList"] != null)
                    objStaffDetailsList = Session.GetDataFromSession<List<StaffDetailsModel>>("StaffDetailsList");
                else
                    objStaffDetailsList = new List<StaffDetailsModel>();
                objStaffDetailsList.Add(model);
                Session.SetDataToSession<List<StaffDetailsModel>>("StaffDetailsList", objStaffDetailsList);

                return Json(objStaffDetailsList);
            }
            else
            {
                return Json("Invalid data");
            }
        }

        public JsonResult DeleteStaffDetails(int index)
        {
            if (Session["StaffDetailsList"] != null)
            {
                List<StaffDetailsModel> objStaffDetailsList = Session.GetDataFromSession<List<StaffDetailsModel>>("StaffDetailsList");
                if (objStaffDetailsList[index].Id == 0)
                    objStaffDetailsList.RemoveAt(index);
                else
                    objStaffDetailsList[index].IsDeleted = true;
                Session.SetDataToSession<List<InfraStructureModel>>("StaffDetailsList", objStaffDetailsList);
                return Json(objStaffDetailsList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult SaveStaffDetails(ApplicationType applicationType, int ExistingApplicationId)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["StaffDetailsList"] != null)
            {
                List<StaffDetailsModel> objStaffDetailsList = Session.GetDataFromSession<List<StaffDetailsModel>>("StaffDetailsList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objStaffDetailsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveStaffDetails(objStaffDetailsList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType, ExistingApplicationId);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Staff details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }

        public JsonResult GetStaffDetails(int transactionId)
        {
            apmceBAL = new APMCEBAL();
            List<StaffDetailsModel> staffDetailsList = apmceBAL.GetStaffDetails(transactionId);
            Session.SetDataToSession<List<StaffDetailsModel>>("StaffDetailsList", staffDetailsList);
            return Json(staffDetailsList);
        }

        public JsonResult SaveFacilitiesAvailable(FacilitiesAvailableModel model, HttpPostedFileBase DeclarationStampFile, HttpPostedFileBase OtherInformationDocument)
        {
            NotificationModel notification = new NotificationModel();

            if (true) //if(ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                int _serviceId = 1;  // TODO: Set this value from m_service table        
                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region Files Saving

                var uploadsPath = Path.Combine("Applicant", model.UserId.ToString()
                    , _serviceId.ToString(), "FacilitiesAvailable");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                #region DeclarationStamp File saving

                if (DeclarationStampFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(DeclarationStampFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "DeclarationStamp_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.DeclarationStampFilePath = _uploadedfilePath + Path.GetExtension(DeclarationStampFile.FileName);

                    DeclarationStampFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(DeclarationStampFile.FileName));
                }
                else if (Session["FacilitiesAvailableDeclarationStampFilePath"] != null)
                {
                    model.DeclarationStampFilePath = Session.GetDataFromSession<string>("FacilitiesAvailableDeclarationStampFilePath");
                }
                #endregion

                #region OtherInformationDocument File saving

                if (OtherInformationDocument != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(OtherInformationDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "FacilityOtherInfo_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.OtherInformationDocumentPath = _uploadedfilePath + Path.GetExtension(OtherInformationDocument.FileName);

                    OtherInformationDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(OtherInformationDocument.FileName));
                }
                else if (Session["FacilitiesAvailableOtherInformationDocument"] != null)
                {
                    model.OtherInformationDocumentPath = Session.GetDataFromSession<string>("FacilitiesAvailableOtherInformationDocument");
                }
                #endregion

                #endregion

                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveFacilitiesAvailable(model, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facilities Available details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                return Json(notification, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // TODO: Return model validations       - kishore, 22-06-2017
                return Json(notification);
            }
        }

        public JsonResult SaveAdditionalDocuments(AdditionalDocumentsModel model, HttpPostedFileBase BioCapstoneWastageClearanceFromFile, HttpPostedFileBase PollutionAuthorityLetterByPCBFile,
            HttpPostedFileBase CFOFile, HttpPostedFileBase STPFile, HttpPostedFileBase FEReportFile, HttpPostedFileBase TarifListFile,
            HttpPostedFileBase Establishment_BuildingPlanFile, HttpPostedFileBase HospitalOutSideNamePlateBuildingFile,
            HttpPostedFileBase TariffBoardFile, HttpPostedFileBase FireExhaustiveFile, HttpPostedFileBase HospitalLabOperationTheaterFile)
        {
            NotificationModel notification = new NotificationModel();

            if (true) //if(ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                int _serviceId = 1;  // TODO: Set this value from m_service table        
                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region Files Saving

                var uploadsPath = Path.Combine("Applicant", model.UserId.ToString()
                    , _serviceId.ToString(), "AdditionalDocuments");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                #region BioCapstoneWastageClearance File saving

                if (BioCapstoneWastageClearanceFromFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(BioCapstoneWastageClearanceFromFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "BioCapstoneWastageClearance_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.BioCapstoneWastageClearanceFromFilePath = _uploadedfilePath + Path.GetExtension(BioCapstoneWastageClearanceFromFile.FileName);

                    BioCapstoneWastageClearanceFromFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(BioCapstoneWastageClearanceFromFile.FileName));
                }
                else if (Session["BioCapstoneWastageClearanceFromFilePath"] != null)
                {
                    model.BioCapstoneWastageClearanceFromFilePath = Session.GetDataFromSession<string>("BioCapstoneWastageClearanceFromFilePath");
                }
                #endregion

                #region PollutionAuthorityLetterByPCB File saving

                if (PollutionAuthorityLetterByPCBFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(PollutionAuthorityLetterByPCBFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "PollutionAuthorityLetter_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.PollutionAuthorityLetterByPCBFilePath = _uploadedfilePath + Path.GetExtension(PollutionAuthorityLetterByPCBFile.FileName);

                    PollutionAuthorityLetterByPCBFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(PollutionAuthorityLetterByPCBFile.FileName));
                }
                else if (Session["PollutionAuthorityLetterByPCBFilePath"] != null)
                {
                    model.PollutionAuthorityLetterByPCBFilePath = Session.GetDataFromSession<string>("PollutionAuthorityLetterByPCBFilePath");
                }
                #endregion

                #region CFO(Consent for operation) File saving

                if (CFOFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(CFOFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "CFOFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.CFOFilePath = _uploadedfilePath + Path.GetExtension(CFOFile.FileName);

                    CFOFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(CFOFile.FileName));
                }
                else if (Session["CFOFilePath"] != null)
                {
                    model.CFOFilePath = Session.GetDataFromSession<string>("CFOFilePath");
                }
                #endregion

                #region STP File saving

                if (STPFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(STPFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "STPFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.STPFilePath = _uploadedfilePath + Path.GetExtension(STPFile.FileName);

                    STPFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(STPFile.FileName));
                }
                else if (Session["STPFilePath"] != null)
                {
                    model.STPFilePath = Session.GetDataFromSession<string>("STPFilePath");
                }
                #endregion

                #region FE Report File saving

                if (FEReportFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(FEReportFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "FEReportFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.FEReportFilePath = _uploadedfilePath + Path.GetExtension(FEReportFile.FileName);

                    FEReportFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(FEReportFile.FileName));
                }
                else if (Session["FEReportFilePath"] != null)
                {
                    model.FEReportFilePath = Session.GetDataFromSession<string>("FEReportFilePath");
                }
                #endregion

                #region Tariff List File saving

                if (TarifListFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(TarifListFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "TarifListFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.TarifListFilePath = _uploadedfilePath + Path.GetExtension(TarifListFile.FileName);

                    TarifListFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(TarifListFile.FileName));
                }
                else if (Session["TarifListFilePath"] != null)
                {
                    model.TarifListFilePath = Session.GetDataFromSession<string>("TarifListFilePath");
                }
                #endregion

                #region Establishment-Building Plan File saving

                if (Establishment_BuildingPlanFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(Establishment_BuildingPlanFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "BuildingPlanFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.Establishment_BuildingPlanFilepath = _uploadedfilePath + Path.GetExtension(Establishment_BuildingPlanFile.FileName);

                    Establishment_BuildingPlanFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(Establishment_BuildingPlanFile.FileName));
                }
                else if (Session["Establishment_BuildingPlanFilePath"] != null)
                {
                    model.Establishment_BuildingPlanFilepath = Session.GetDataFromSession<string>("Establishment_BuildingPlanFilePath");
                }
                #endregion

                #region Hospital-OutSide NamePlate Building File saving

                if (HospitalOutSideNamePlateBuildingFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(HospitalOutSideNamePlateBuildingFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "NamePlate_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.HospitalOutSideNamePlateBuildingFilePath = _uploadedfilePath + Path.GetExtension(HospitalOutSideNamePlateBuildingFile.FileName);

                    HospitalOutSideNamePlateBuildingFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(HospitalOutSideNamePlateBuildingFile.FileName));
                }
                else if (Session["HospitalOutSideNamePlateBuildingFilePath"] != null)
                {
                    model.HospitalOutSideNamePlateBuildingFilePath = Session.GetDataFromSession<string>("HospitalOutSideNamePlateBuildingFilePath");
                }
                #endregion

                #region Hospital-TariffBoard File saving

                if (TariffBoardFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(TariffBoardFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "TariffBoardFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.TariffBoardFilePath = _uploadedfilePath + Path.GetExtension(TariffBoardFile.FileName);

                    TariffBoardFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(TariffBoardFile.FileName));
                }
                else if (Session["TariffBoardFilePath"] != null)
                {
                    model.TariffBoardFilePath = Session.GetDataFromSession<string>("TariffBoardFilePath");
                }
                #endregion

                #region Hospital-FireExhaustive File saving

                if (FireExhaustiveFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(FireExhaustiveFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "FireExhaustiveFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.FireExhaustiveFilePath = _uploadedfilePath + Path.GetExtension(FireExhaustiveFile.FileName);

                    FireExhaustiveFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(FireExhaustiveFile.FileName));
                }
                else if (Session["FireExhaustiveFilePath"] != null)
                {
                    model.FireExhaustiveFilePath = Session.GetDataFromSession<string>("FireExhaustiveFilePath");
                }
                #endregion

                #region Hospital-Lab,Operation Theater File saving

                if (HospitalLabOperationTheaterFile != null)
                {
                    //string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(HospitalLabOperationTheaterFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string _uploadedfilePath = uploadsPath + "/" + "OperationTheaterFile_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    _uploadedfilePath = _uploadedfilePath.Replace(" ", "");
                    model.HospitalLabOperationTheaterFilePath = _uploadedfilePath + Path.GetExtension(HospitalLabOperationTheaterFile.FileName);

                    HospitalLabOperationTheaterFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(HospitalLabOperationTheaterFile.FileName));
                }
                else if (Session["HospitalLabOperationTheaterFilePath"] != null)
                {
                    model.HospitalLabOperationTheaterFilePath = Session.GetDataFromSession<string>("HospitalLabOperationTheaterFile");
                }
                #endregion


                #endregion

                APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveAdditionalDocuments(model, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Additional Documents details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                return Json(notification, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // TODO: Return model validations       - kishore, 22-06-2017
                return Json(notification);
            }
        }

        #region APMCE Certificate
        public PartialViewResult GetAPMCECertificate()
        {
            int TransactionId = 1;
            string Type = "Transaction";
            objBAL = new LicenseBAL();
            APMCECertificate model = objBAL.GetAPMCECertificate(TransactionId, Type);
            return PartialView("_APMCETemporaryCertificate", model);
            //return PartialView("_APMCETemporaryCertificate");
        }
        #endregion

        #endregion

        #region Query & Response

        public ViewResult Queries()
        {
            Session["QueryRespondedList"] = null;
            return View();
        }

        public JsonResult GetQueryData()
        {
            objBAL = new LicenseBAL();
            int userId = Session.GetDataFromSession<UserModel>("User").Id;
            QueryResponseViewModel model = objBAL.GetQueryResponseData(userId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRaisedQueryData(int id)
        {
            objBAL = new LicenseBAL();
            QueryModel model = objBAL.GetRaisedQueryData(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddQueryRespondFile(QueryRespondModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase _uploadedFile = null;
                if (Request.Files.Count > 0)
                    _uploadedFile = Request.Files[0];

                //int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                //int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                List<QueryRespondModel> objQueryRespondedList;
                if (Session["QueryRespondedList"] != null)
                    objQueryRespondedList = Session.GetDataFromSession<List<QueryRespondModel>>("QueryRespondedList");
                else
                    objQueryRespondedList = new List<QueryRespondModel>();

                #region Saving files physically
                if (_uploadedFile != null)
                {
                    var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                        , "Responses");

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    string _filePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                    _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _filePath));
                    model.UploadedFilePath = _filePath + Path.GetExtension(_uploadedFile.FileName);

                    string _uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _filePath);
                    System.IO.File.Move(_uploadedfilepath, _uploadedfilepath + Path.GetExtension(_uploadedFile.FileName));
                }
                #endregion

                objQueryRespondedList.Add(model);
                Session.SetDataToSession<List<QueryRespondModel>>("QueryRespondedList", objQueryRespondedList);

                return Json(objQueryRespondedList);
            }
            else
            {
                return Json("Invalid data");
            }
        }

        public JsonResult DeleteQueryResponded(int index)
        {
            if (Session["QueryRespondedList"] != null)
            {
                List<QueryRespondModel> objQueryRespondedList = Session.GetDataFromSession<List<QueryRespondModel>>("QueryRespondedList");
                if (objQueryRespondedList[index].Id == 0)
                    objQueryRespondedList.RemoveAt(index);
                else
                    objQueryRespondedList[index].IsDeleted = true;
                Session.SetDataToSession<List<QueryRespondModel>>("QueryRespondedList", objQueryRespondedList);
                return Json(objQueryRespondedList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult SubmitResponse(string response, int queryId, int transactionId, string applicationType)
        {
            QueryModel model = new QueryModel();
            model.Description = response;
            model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
            model.Type = "Response";
            model.ApplicationType = applicationType;
            model.QueryId = queryId;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase _uploadedFile = Request.Files[0];
                var uploadsPath = Path.Combine("Applicant", model.UserId.ToString()
                    , "Responses");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                string _filePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _filePath));
                model.UploadedFilePath = _filePath + Path.GetExtension(_uploadedFile.FileName);

                string _uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _filePath);
                System.IO.File.Move(_uploadedfilepath, _uploadedfilepath + Path.GetExtension(_uploadedFile.FileName));
            }

            List<QueryRespondModel> queryRespondedList = new List<QueryRespondModel>();
            if (Session["QueryRespondedList"] != null)
            {
                queryRespondedList = Session.GetDataFromSession<List<QueryRespondModel>>("QueryRespondedList");
                queryRespondedList = queryRespondedList.Where(item => item.IsDeleted == false).ToList();
            }
            int resultcount = queryRespondedList.Count;
            int i = 0;
            bool result=false;
            if (queryRespondedList.Count > 0)
            {
                foreach (var objModel in queryRespondedList)
                {
                    model.Description = objModel.Response;
                    model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                    model.Type = "Response";
                    model.ApplicationType = objModel.ApplicationType; //applicationType;
                    model.QueryId = objModel.QueryId;
                    model.TransactionId = objModel.TransactionId;
                    model.UploadedFilePath = objModel.UploadedFilePath;
                    objBAL = new LicenseBAL();
                    result = objBAL.SubmitResponse(model);
                    if (result)
                        i = i+1;
                }
            }
            //objBAL = new LicenseBAL();
            //bool result = objBAL.SubmitResponse(model);
           
            NotificationModel notification = new NotificationModel();
            if (resultcount == i) // result
            {
                Session["QueryRespondedList"] = null;
                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Your response's has been submitted successfully";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Success;
                notification.NonActionButtonText = "Close";
                SendApplicantResponseSMS(0, transactionId, applicationType);
            }
            else
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Oops! Something went wrong... <br>Your response was not submitted, please contact technical support.";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Close";
            }
            return Json(notification);
        }
        private bool SendApplicantResponseSMS(int applicationId, int transactionId, string ApplicationType)
        {
            ApplicationBAL objApplicationBAL = new ApplicationBAL();

            SMSModel smsData = objApplicationBAL.GetSMSDetails(applicationId, transactionId, 0, ApplicationType);
            string DeptMsg = "Hi " + smsData.DeptUserName + "," + " Application Number :  " + smsData.ApplicationNumber + ".  " + "Response for your query has been submitted. Please Login and check the details";
            string ApplicantMsg = "Hi " + smsData.ApplicantName + "," + " Application Number : " + smsData.ApplicationNumber + ". " + "Your Response has been submitted Successfully.";
            string deliveryStatus;
            bool result = Utitlities.SendSMS(smsData.ApplicantMobileNumber, ApplicantMsg, out deliveryStatus);
            result = Utitlities.SendSMS(smsData.DeptMobile, DeptMsg, out deliveryStatus);

            return result;
        }

        #endregion

        #region Master Data
        public JsonResult GetMandals(int id)
        {
            objBAL = new LicenseBAL();
            return Json(objBAL.GetMandalList(id));
        }

        public JsonResult GetVillages(int id)
        {
            objBAL = new LicenseBAL();
            return Json(objBAL.GetVillageList(id));
        }

        public JsonResult GetDistricts()
        {
            objBAL = new LicenseBAL();  
            return Json(objBAL.GetCountries());
        }
        public JsonResult GetEquipmentsList(int HospitalTypeId)
        {
            MasterBAL objBAL = new MasterBAL();
            return Json(objBAL.GetEquipmentTypesList(HospitalTypeId));
        }
        #endregion

        public void ClearData()
        {
            Session["ApplicationId"] = null;
            Session["PCPNDTTransactionId"] = null;
            Session["EquipmentsList"] = null;
            Session["EmployeeList"] = null;
            Session["EmployeeUploadedDocuments"] = null;
            Session["APMCETransactionId"] = null;
            Session["AddressProofPath"] = null;
            Session["BuildingLayoutPath"] = null;
            Session["BloodBankTransactionId"] = null;
            Session["BloodBankForm27ETransactionId"] = null;
            Session["BBEApplicantDocumentPath"] = null;
            Session["BBEItemList"] = null;
            Session["BBEEquipmentsList"] = null;
            Session["BloodBankTechnicalList"] = null;
            Session["BioCapstoneTransactionId"] = null;
            Session["BioCapstoneAddressList"] = null;
            Session["BioCapstoneDisposalList"] = null;
            Session["QuantityWasteList"] = null;
        }

        //public PartialViewResult GetPCPNDTLicense(int transactionId)
        //{
        //    objBAL = new LicenseBAL();
        //    PCPNDTLicenseInfoModel model = objBAL.GetPCPNDTLicenseDetails(transactionId);
        //    return PartialView("_PCPNDTLicense", model);
        //}

        #region APMCE & PCPNDT Rejection
        public PartialViewResult GetRejectLicenseApplications(int TransactionId, string TransactionType, int ApplicationId)
        {
            objBAL = new LicenseBAL();
            DataSet dsItems = objBAL.GetRejectLicense(TransactionId, TransactionType);
            RejectViewModel model = new RejectViewModel();
            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
            {
                if (dsItems.Tables[1].Rows[0]["ServiceId"].ToString() == "1")
                {
                    model.APMCERejection = objBAL.GetAPMCERejectionDetails(ApplicationId);
                    return PartialView("_APMCERejection", model.APMCERejection);
                }
                else if (dsItems.Tables[1].Rows[0]["ServiceId"].ToString() == "2")
                {
                    model.PCPNDTRejection = objBAL.GetPCPNDTRejectionDetails(ApplicationId);
                    return PartialView("_PCPNDTRejection", model.PCPNDTRejection);
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public PartialViewResult GetAPMCERejection(int ApplicationId)
        {
            objBAL = new LicenseBAL();
            APMCERejection model = objBAL.GetAPMCERejectionDetails(ApplicationId);
            return PartialView("_APMCERejection", model);
        }
        public PartialViewResult GetPCPNDTRejection(int ApplicationId)
        {
            //int ApplicationId = 1;
            objBAL = new LicenseBAL();
            PCPNDTRejection model = objBAL.GetPCPNDTRejectionDetails(ApplicationId);
            return PartialView("_PCPNDTRejection", model);
        }
        #endregion

        #region Rennual for PCPNDT
        public ActionResult Renewal(int id, int serviceId, string tableName)
        {
            RenewalModel model = new RenewalModel();
            model.TransactionId = id;
            model.Type = tableName;
            AmendmentBAL objBAL = new AmendmentBAL();
            if (serviceId == 1) // APMCE Renewal
            {
                model.APMCEModel = new APMCEViewModel();
                APMCERenewalModel APMCERenewalModel = new APMCERenewalModel();
                model.APMCEModel = objBAL.GetAPMCEData(model.TransactionId, model.Type);
                model.APMCERenewalModel = objBAL.GetAPMCERenewalDetails(model.TransactionId);
            }
            else if (serviceId == 2)  // PCPNDT Renewal
            {
                PCPNDTViewModel pcpndtModel = new PCPNDTViewModel();
                PCPNDTLicenseInfoModel PCPNDTLicenseModel = new PCPNDTLicenseInfoModel();
                //AmendmentBAL objBAL = new AmendmentBAL();
                model.PCPNDTModel = objBAL.GetPCPNDTData(model.TransactionId, model.Type);
                model.PCPNDTLicenseModel = objBAL.GetPCPNDTLicenseDetails(model.TransactionId);
            }

            return View(model);

        }
        public ActionResult SaveRenewal(int transactionId)
        {
            NotificationModel notification = new NotificationModel();
            UserModel model = new UserModel();
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            AmendmentBAL objBAL = new AmendmentBAL();
            int result = objBAL.SaveRenewal(model, transactionId);
            if (result > 0)
            {
                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Your License has been Renewal.";
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
            }
            return Json(notification);
        }
        #endregion  

        #region Appeal For PCPNDT
        public ActionResult Appeal(int id)
        {
            objBAL = new LicenseBAL();
            ApplicationModel model = objBAL.GetApplication(id, Status.Rejected, "Transaction");
            //int TransactionId = 1;  // Static TransectionId  for Testing Purpose --kishore
            //PCPNDTViewModel pcpndtModel = new PCPNDTViewModel();
            //AmendmentBAL objBAL = new AmendmentBAL();
            //model.PCPNDTModel = objBAL.GetPCPNDTData(TransactionId);
            //model = objBAL.GetRemarks(model,TransactionId);   //Get Remarks for rejection  from department --kishore
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveAppeal(int transactionId, string remarks, string type)
        {
            NotificationModel notification = new NotificationModel();
            int result = 0;
            AmendmentBAL objBAL = new AmendmentBAL();
            applicationBAL = new ApplicationBAL();

            int userId = Session.GetDataFromSession<UserModel>("User").Id;
            result = objBAL.SaveAppeal(transactionId, remarks, userId, type);

            SMSModel smsData = applicationBAL.GetSMSDetails(0, transactionId, 0, "AppealSubmit");
            string ApplicantMsg = "Hi " + smsData.ApplicantName + "," + " Application Number :  " + smsData.ApplicationNumber + ". Thanks For Appealing. Once Department Review the application we will acknowledge you. ";
            string DeptMsg = "Hi " + smsData.DeptUserName + "," + " Application Number :  " + smsData.ApplicationNumber + " ." + " Applicant :" + smsData.ApplicantName + ". Had Appealed for Registration ";

            string deliveryStatus;
            Utitlities.SendSMS(smsData.ApplicantMobileNumber, ApplicantMsg, out deliveryStatus);
            Utitlities.SendSMS(smsData.DeptMobile, DeptMsg, out deliveryStatus);
            //if ( model.APMCEModel !=null)
            //{
            //    model.Id = Session.GetDataFromSession<UserModel>("User").Id;
            //    objBAL = new AmendmentBAL();
            //    result = objBAL.SaveAppealForPCPNDT(model);
            //}
            //if(model.PCPNDTModel!=null)
            //{
            //    model.Id = Session.GetDataFromSession<UserModel>("User").Id;
            //    objBAL = new AmendmentBAL();
            //     result = objBAL.SaveAppealForPCPNDT(model);
            //}

            if (result > 0)
            {
                //notification.Title = "Success";
                //notification.NotificationType = NotificationType.Success;
                //notification.NotificationMessage = "Your appeal has been submitted.";
                ////notification.ShowNonActionButton = true;
                ////notification.NonActionButtonClassType = PopupButtonClass.Success;
                ////notification.NonActionButtonText = "Okay";
                //notification.ShowActionButton = true;
                //notification.ActionButtonText = "Okay";
                //notification.ActionName = "Submitted";
                //notification.ControllerName = "Dashboard";
                //notification.AreaName = "User";

                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Your appeal has been submitted.";
                notification.ShowActionButton = true;
                notification.ActionButtonText = "Okay";
                notification.ActionName = "Index";
                notification.ControllerName = "Dashboard";
                notification.AreaName = "User";

            }
            else
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);

        }

        #endregion

        #region Resubmit For PCPNDT
        public ActionResult Resubmit(int TransactionId, string TransactionType, int ApplicationId)
        {
            LicenseQuestionnaireModel model1 = new LicenseQuestionnaireModel();
            objBAL = new LicenseBAL();

            model1 = objBAL.GetRejectedServices(TransactionId, TransactionType);
            model1.ApplicationModel.ApplicationType = ApplicationType.Resubmit;
            model1.ApplicationModel.ExistingApplicationId = ApplicationId;
            // If PCPNDT is rejcted
            if (model1.HasAppliedforAPMCE == true)
            {

                model1.ApplicationModel.ExistingApplicationNumber = model1.ApplicationModel.ApplicationNumber;
                objBAL = new LicenseBAL();
                ViewBag.DistrictList = objBAL.GetCountries();
                ViewBag.OfferedServices = objBAL.GetOfferedServices();
                ResubmitModel model = new ResubmitModel();
            }
            if (model1.HasAppliedforPCPNDT == true)
            {

                model1.ApplicationModel.ExistingApplicationNumber = model1.ApplicationModel.ApplicationNumber;

                objBAL = new LicenseBAL();
                ViewBag.DistrictList = objBAL.GetCountries();
                ResubmitModel model = new ResubmitModel();
                List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                objBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                ViewBag.OwnershipTypeList = ownershipTypeList;
                ViewBag.InstitutionTypeList = institutionTypeList;
                ViewBag.FacilityMaster = objBAL.GetFacilityList();
                ViewBag.OfferedServices = objBAL.GetOfferedServices();
                TempData["DoctorSpecialityList"] = objBAL.GetDoctorSpecialityList();
            }

            return View("ApplicationForm", model1);
        }
        #endregion



        public PartialViewResult GetViewLicense(int TransactionId, string TableName)
        {
            objBAL = new LicenseBAL();
            DataTable dtItems = objBAL.GetLicenseTypeForLicense(TransactionId, TableName);
            LicenseViewModel model = new LicenseViewModel();
            if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
            {
                model.APMCECertificate = objBAL.GetAPMCECertificate(TransactionId, TableName);


                #region File Download by Chandu 25-06-2021 From HiQPdf (HtmlToPdf) Evaluation
                int IsCertificateSavedInFolder = objBAL.CheckIsCertificateSavedInFolder(TransactionId);
                if (IsCertificateSavedInFolder <= 0)
                {
                    // get the About view HTML code
                    string htmlToConvert = RenderViewAsString("_APMCEPermanentCertificate", model.APMCECertificate);
                    Session["pdf"] = htmlToConvert;
                    // the base URL to resolve relative images and css
                    String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
                    String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertAboutPageToPdf".Length);

                    // instantiate the HiQPdf HTML to PDF converter
                    HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                    htmlToPdfConverter.SerialNumber = null;
                    htmlToPdfConverter.ConvertedHtmlElementSelector = "#testdiv"; // "#divModalBody"; // "#testdiv";
                    htmlToPdfConverter.Document.Header.Enabled = true; // false;

                    // render the HTML code as PDF in memory
                    byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

                    var user = Session.GetDataFromSession<UserModel>("User").Id;
                    //var uploadsPath = Path.Combine("Applicant", user.ToString()
                    //    , "1", "Certificates");
                    var uploadsPath = Path.Combine("Certificates", TransactionId.ToString());

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    string _uploadedfilePath = uploadsPath + "/TAMCECertificate_" + TransactionId.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    // send the PDF file to browser
                    FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
                    fileResult.FileDownloadName = _uploadedfilePath + ".pdf"; //"APMCECertificate.pdf";
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);
                    var uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    ToFile(fileResult, path);

                    // Save filepath in t_transaction table based on TransactionId
                    bool UpdateCertificateDownloadStatus = objBAL.SaveUpdateCertificatePath(TransactionId, fileResult.FileDownloadName, user);

                    string usermailId = "";
                    DataTable dtusermailId = objBAL.GetUserMailId(TransactionId);

                    usermailId=dtusermailId.Rows[0]["Username"].ToString();
                    if (usermailId != null & dtusermailId.Rows.Count > 0)
                        SendEmailwithAttachmentFile(usermailId, usermailId, "TAMCE Certificate", "Hi, Please Find the Attachment!", fileResult.FileDownloadName);
                }
                #endregion

                return PartialView("_APMCEPermanentCertificate", model.APMCECertificate);
            }
            else if (dtItems.Rows[0]["ActType"].ToString() == "PCPNDT")
            {
                model.PCPNDTCertificate = objBAL.GetPCPNDTLicenseDetails(TransactionId, TableName);
                //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                //PCPNDTmodel = model.PCPNDTCertificate;
                return PartialView("_PCPNDTLicense", model.PCPNDTCertificate);
            }
            else if (dtItems.Rows[0]["ActType"].ToString() == "BloodBank")
            {
                model.BloodBankNOCModel = objBAL.GetBloodBankNOC(TransactionId);
                return PartialView("_BloodBankNOC", model.BloodBankNOCModel);
            }
            else
            {
               return null;
            }            
        }

        #region filedownload by Chandu For Physical Save License certificate 25-06-2021

        #region working
        public FileResult DownloadFile(string path, string downloadName)
        {
            return File(Server.MapPath("~/Uploads/" + path), "multipart/form-data", downloadName);
        }

        public string RenderViewAsString(string viewName, object model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render


            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);


            // create a context to render a view based on a model


            ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    new ViewDataDictionary(model),
                    new TempDataDictionary(),
                    stringWriter
                    );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }
        public static void ToFile(FileResult fileResult, string fileName)
        {
            if (fileResult is FileContentResult)
            {
                System.IO.File.WriteAllBytes(fileName, ((FileContentResult)fileResult).FileContents);
            }
            else if (fileResult is FilePathResult)
            {
                System.IO.File.Copy(((FilePathResult)fileResult).FileName, fileName, true); //overwrite file if it already exists
            }
            else if (fileResult is FileStreamResult)
            {
                //from http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file-in-c
                using (var fileStream = System.IO.File.Create(fileName))
                {
                    var fileStreamResult = (FileStreamResult)fileResult;
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                    fileStreamResult.FileStream.CopyTo(fileStream);
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin); //reset position to beginning. If there's any chance the FileResult will be used by a future method, this will ensure it gets left in a usable state - Suggestion by Steven Liekens
                }
            }
            else
            {
                throw new ArgumentException("Unsupported FileResult type");
            }
        }
        #endregion

        #region Not use
        public void DownloadPDF()
        {

            //string HTMLContent = "Hello <b>World</b>";
            string HTMLContent = Session["pdf"].ToString(); 
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile1.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(getPDFNEW(HTMLContent)); // GetPDF(HTMLContent));
            Response.End();
        }        
        public byte[] getPDFNEW(string pHTML)
        {            
            StringReader sr = new StringReader(pHTML.ToString());
            TextReader txtReader = sr;
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                htmlparser.Parse(txtReader);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }
        }
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.ConvertedHtmlElementSelector = "#testdiv";

            MemoryStream ms = new MemoryStream();
            StringReader sr = new StringReader(pHTML);
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  

            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Uploads") + "/Images.pdf", FileMode.Create));
            doc.Open();
            htmlWorker.StartDocument();

            doc.Add(new Paragraph("GIF"));
            Image gif = Image.GetInstance(Server.MapPath("~/Content/images") + "/logogovts.png");
            doc.Add(gif);

            // 5: parse the html into the document  
            htmlWorker.Parse(sr);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }
        #endregion

        #endregion


        #region BloodBank Form 27 C
        public JsonResult SaveBloodBankApplicantDetails(BloodBankApplicantModel model, HttpPostedFileBase uploadedDocument)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "31";
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId, "Applicant");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (uploadedDocument != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadDocument = _addressProofPath + Path.GetExtension(uploadedDocument.FileName);

                    uploadedDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(uploadedDocument.FileName));
                }
                else if (Session["BBCApplicantDocumentPath"] != null)
                {
                    model.UploadDocument = Session.GetDataFromSession<string>("BBCApplicantDocumentPath");
                }
                #endregion

                bloodbankBAL = new BloodBankBAL();
                int result = bloodbankBAL.SaveBloodBankApplicantDetails(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

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
                // TODO: Return model validations       - Jai, 12-08-2017
                return Json(notification);
            }
        }

        public JsonResult SaveBloodBankEstablishment(BloodBankEstablishmentModel model, HttpPostedFileBase uploadedDocument)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "31";
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId, "Establishment");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (uploadedDocument != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.AddressProofPath = _addressProofPath + Path.GetExtension(uploadedDocument.FileName);

                    uploadedDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(uploadedDocument.FileName));
                }
                else if (Session["BBCEstablishmentDocumentPath"] != null)
                {
                    model.AddressProofPath = Session.GetDataFromSession<string>("BBCEstablishmentDocumentPath");
                }
                #endregion

                bloodbankBAL = new BloodBankBAL();
                int result = bloodbankBAL.SaveBloodBankEstablishmentDetails(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Establishment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Jai, 12-08-2017
                return Json(notification);
            }
        }

        // as of now, not using - Raj, 17-08-2017
        public ActionResult SaveBloodBankEquipments(string ApplicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["EquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("EquipmentsList");
                //TempData.Peek("EquipmentsList") as List<EquipmentModel>;
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEquipmentsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });

                bloodbankBAL = new BloodBankBAL();
                int result = bloodbankBAL.SaveBloodBankEquipments(objEquipmentsList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, ApplicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }

        #region BloodBank saving List of items
        public JsonResult AddListofItems(BloodBankListModel model)
        {
            if (true)
            {
                // int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                // int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<BloodBankListModel> objItemList;
                if (Session["ItemList"] != null)
                    objItemList = Session.GetDataFromSession<List<BloodBankListModel>>("ItemList");
                else
                    objItemList = new List<BloodBankListModel>();
                objItemList.Add(model);
                Session.SetDataToSession<List<BloodBankListModel>>("ItemList", objItemList);

                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteListofItems(int index)
        {
            if (Session["ItemList"] != null)
            {
                List<BloodBankListModel> objItemList = Session.GetDataFromSession<List<BloodBankListModel>>("ItemList");
                if (objItemList[index].Id == 0)
                    objItemList.RemoveAt(index);
                else
                    objItemList[index].IsDeleted = true;
                Session.SetDataToSession<List<BloodBankListModel>>("ItemList", objItemList);
                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public JsonResult SaveListofItems(ApplicationType applicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["ItemList"] != null)
            {
                List<BloodBankListModel> objItemList = Session.GetDataFromSession<List<BloodBankListModel>>("ItemList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objItemList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                BloodBankBAL objAPMCEBAL = new BloodBankBAL();
                int result = objAPMCEBAL.SaveListofItems(objItemList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "List of items saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please add atleast one Item to the list";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetListofItems(int transactionId)
        {
            bloodbankBAL = new BloodBankBAL();
            List<BloodBankListModel> listItems = bloodbankBAL.GetBloodBankListItems(transactionId);
            Session.SetDataToSession<List<BloodBankListModel>>("ItemList", listItems);
            return Json(listItems);
        }
        #endregion

        #region BloodBank Saving Equipment Details
        public ActionResult AddBloodBankEquipment(EquipmentModel model)
        {
            if (true)
            {
                // HttpPostedFileBase _uploadedFile = Request.Files[0];

                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                List<EquipmentModel> objEquipmentsList;
                if (Session["BloodBankEquipmentsList"] != null)
                    objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("BloodBankEquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                else
                    objEquipmentsList = new List<EquipmentModel>();
                objEquipmentsList.Add(model);
                Session.SetDataToSession<List<EquipmentModel>>("BloodBankEquipmentsList", objEquipmentsList);
                //TempData["EquipmentsList"] = objEquipmentsList;

                return Json(objEquipmentsList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }

        }
        public JsonResult DeleteBloodBankEquipment(int index)
        {
            if (Session["BloodBankEquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("BloodBankEquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                if (objEquipmentsList[index].Id == 0)
                    objEquipmentsList.RemoveAt(index);
                else
                    objEquipmentsList[index].IsDeleted = true;
                Session.SetDataToSession<List<EquipmentModel>>("EquipmentsList", objEquipmentsList);
                //TempData["EquipmentsList"] = objEquipmentsList;
                return Json(objEquipmentsList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public JsonResult SaveEquipment(ApplicationType applicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BloodBankEquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("BloodBankEquipmentsList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEquipmentsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                BloodBankBAL objAPMCEBAL = new BloodBankBAL();
                int result = objAPMCEBAL.SaveEquipment(objEquipmentsList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipments details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please Add Atleast One Equipment List";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }

        public JsonResult GetBloodBankEquipments(int transactionId)
        {
            bloodbankBAL = new BloodBankBAL();
            List<EquipmentModel> equipmentList = bloodbankBAL.GetBloodBankEquipments(transactionId);
            Session.SetDataToSession<List<EquipmentModel>>("BloodBankEquipmentsList", equipmentList);
            return Json(equipmentList);
        }
        #endregion

        #region BloodBank Declaration saving 
        public JsonResult SaveBloodBankDeclaration(BloodBankAttachments model, HttpPostedFileBase planPremises, HttpPostedFileBase OwnerPremises, HttpPostedFileBase IdProff)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                #region File Saving
                var uploadsPath = Path.Combine("BloodBank", "Declaration");


                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                if (planPremises != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(planPremises.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.planPremisesPath = _addressProofPath + Path.GetExtension(planPremises.FileName);

                    planPremises.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(planPremises.FileName));
                }
                else if (Session["BBCPlanPremises"] != null)
                {
                    model.planPremisesPath = Session.GetDataFromSession<string>("BBCPlanPremises");
                }

                if (OwnerPremises != null)
                {
                    string _buildingLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(OwnerPremises.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.OwnerPremisesPath = _buildingLayoutPath + Path.GetExtension(OwnerPremises.FileName);

                    OwnerPremises.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(OwnerPremises.FileName));
                }
                else if (Session["BBCOwnerPremises"] != null)
                {
                    model.OwnerPremisesPath = Session.GetDataFromSession<string>("BBCOwnerPremises");
                }
                if (IdProff != null)
                {
                    string _IdProfftPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(IdProff.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.IdProffPath = _IdProfftPath + Path.GetExtension(IdProff.FileName);

                    IdProff.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _IdProfftPath));
                    string idfilepath = Path.Combine(Server.MapPath("~/Uploads"), _IdProfftPath);
                    System.IO.File.Move(idfilepath, idfilepath + Path.GetExtension(IdProff.FileName));
                }
                else if (Session["BBCIdProff"] != null)
                {
                    model.IdProffPath = Session.GetDataFromSession<string>("BBCIdProff");
                }
                #endregion

                BloodBankBAL objBAL = new BloodBankBAL();
                int result = objBAL.SaveDeclarationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Declaration details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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

                return Json(notification);
            }
        }
        #endregion

        #region Bloodbank Saving Employee Details
        public JsonResult UploadEmployeeDocument(DocumentUploadModel document)
        {
            HttpPostedFileBase _uploadedFile = Request.Files[0];

            var uploadsPath = Path.Combine("Applicant", "Temp");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            document.DocumentPath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);
            document.ReferenceTable = "t_employee";
            document.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            #region Saving files physically
            _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
            #endregion


            List<DocumentUploadModel> documentList;
            if (Session["EmployeeDocumentList"] != null)
                documentList = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeDocumentList");
            else
                documentList = new List<DocumentUploadModel>();
            documentList.Add(document);
            Session.SetDataToSession<List<DocumentUploadModel>>("EmployeeDocumentList", documentList);

            return Json(documentList);
        }

        // *AddUploads* is Depricated       - Raj, 16-08-2017
        public JsonResult AddUploads(EmployeeModel model)
        {

            if (true)
            {
                // int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                // int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<EmployeeModel> objUploadList;
                if (Session["BloodBankUploadList"] != null)
                    objUploadList = Session.GetDataFromSession<List<EmployeeModel>>("BloodBankUploadList");
                else
                    objUploadList = new List<EmployeeModel>();
                objUploadList.Add(model);
                Session.SetDataToSession<List<EmployeeModel>>("BloodBankUploadList", objUploadList);

                return Json(objUploadList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteEmployeeDocument(int index)
        {
            if (Session["EmployeeDocumentList"] != null)
            {
                List<DocumentUploadModel> objItemList = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeDocumentList");
                if (objItemList[index].Id == 0)
                    objItemList.RemoveAt(index);
                else
                    objItemList[index].IsDeleted = true;
                Session.SetDataToSession<List<DocumentUploadModel>>("EmployeeDocumentList", objItemList);
                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult AddEmployees(EmployeeViewModel model)
        {

            if (true)
            {
                if (Session["EmployeeDocumentList"] != null)
                {
                    model.UploadDocuments = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeDocumentList");
                    Session["EmployeeDocumentList"] = null;
                }

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<EmployeeViewModel> objUploadList;
                if (Session["BloodBankEmployeeList"] != null)
                    objUploadList = Session.GetDataFromSession<List<EmployeeViewModel>>("BloodBankEmployeeList");
                else
                    objUploadList = new List<EmployeeViewModel>();
                objUploadList.Add(model);
                Session.SetDataToSession<List<EmployeeViewModel>>("BloodBankEmployeeList", objUploadList);

                return Json(objUploadList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteBloodbankEmployee(int index)
        {
            if (Session["BloodBankEmployeeList"] != null)
            {
                List<EmployeeViewModel> objItemList = Session.GetDataFromSession<List<EmployeeViewModel>>("BloodBankEmployeeList");
                if (objItemList[index].Id == 0)
                    objItemList.RemoveAt(index);
                else
                    objItemList[index].IsDeleted = true;
                Session.SetDataToSession<List<EmployeeViewModel>>("BloodBankEmployeeList", objItemList);
                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult SaveBloodBankEmployees()
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BloodBankEmployeeList"] != null)
            {
                List<EmployeeViewModel> objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("BloodBankEmployeeList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;
                ApplicationType applicationType = ApplicationType.Grant;
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEmployeeList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                BloodBankBAL objBloodBankBAL = new BloodBankBAL();
                int result = objBloodBankBAL.SaveBloodBankEmployee(objEmployeeList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Employee details saved. <br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Raj, 19-05-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please add at least one Employee List";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }

        public JsonResult GetBloodBankemployees(int transactionId)
        {
            bloodbankBAL = new BloodBankBAL();
            List<EmployeeViewModel> employeeList = bloodbankBAL.GetBloodBankEmployees(transactionId);
            Session.SetDataToSession<List<EmployeeViewModel>>("BloodBankEmployeeList", employeeList);
            return Json(employeeList);
        }

        #endregion



        #endregion

        #region BloodBank Form 27 E

        public JsonResult SaveBloodBankApplicantForm27E(BloodBankApplicantModel model, HttpPostedFileBase UploadDocument)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "32";
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId, "Applicant");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (UploadDocument != null)
                {
                    string _documentsProof = uploadsPath + "/" + Path.GetFileNameWithoutExtension(UploadDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadDocument = _documentsProof + Path.GetExtension(UploadDocument.FileName);

                    UploadDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _documentsProof));
                    string documentfilepath = Path.Combine(Server.MapPath("~/Uploads"), _documentsProof);
                    System.IO.File.Move(documentfilepath, documentfilepath + Path.GetExtension(UploadDocument.FileName));
                }
                else if (Session["BBEApplicantDocumentPath"] != null)
                {
                    model.UploadDocument = Session.GetDataFromSession<string>("BBEApplicantDocumentPath");
                }
                #endregion

                bloodbankBAL = new BloodBankBAL();
                int result = bloodbankBAL.SaveBloodBankApplicantForm27E(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankForm27ETransactionId", _transactionId);

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
                // TODO: Return model validations       - Jai, 22-08-2017
                return Json(notification);
            }
        }
        public JsonResult SaveBloodBankEstablishmentForm27E(BloodBankEstablishmentModel model, HttpPostedFileBase AddressProofPath)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "32";
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId, "Establishment");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (AddressProofPath != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(AddressProofPath.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.AddressProofPath = _addressProofPath + Path.GetExtension(AddressProofPath.FileName);

                    AddressProofPath.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(AddressProofPath.FileName));
                }
                else if (Session["BBEEstablishmentDocumentPath"] != null)
                {
                    model.AddressProofPath = Session.GetDataFromSession<string>("BBEEstablishmentDocumentPath");
                }
                #endregion

                bloodbankBAL = new BloodBankBAL();
                int result = bloodbankBAL.SaveBloodBankEstablishmentForm27E(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankForm27ETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Establishment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Jai, 21-08-2017
                return Json(notification);
            }
        }

        #region BloodBank saving List of items
        public JsonResult AddListofItemForm27E(BloodBankListModel model)
        {
            if (true)
            {
                // int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                // int _transactionId = Session["BloodBankTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankTransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<BloodBankListModel> objItemList;
                if (Session["BBEItemList"] != null)
                    objItemList = Session.GetDataFromSession<List<BloodBankListModel>>("BBEItemList");
                else
                    objItemList = new List<BloodBankListModel>();
                objItemList.Add(model);
                Session.SetDataToSession<List<BloodBankListModel>>("BBEItemList", objItemList);

                return Json(objItemList);
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteListofItemForm27E(int index)
        {
            if (Session["BBEItemList"] != null)
            {
                List<BloodBankListModel> objItemList = Session.GetDataFromSession<List<BloodBankListModel>>("BBEItemList");
                if (objItemList[index].Id == 0)
                    objItemList.RemoveAt(index);
                else
                    objItemList[index].IsDeleted = true;
                Session.SetDataToSession<List<BloodBankListModel>>("BBEItemList", objItemList);
                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public JsonResult SaveBloodBankListofItemForm27E(ApplicationType applicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BBEItemList"] != null)
            {
                List<BloodBankListModel> objItemList = Session.GetDataFromSession<List<BloodBankListModel>>("BBEItemList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objItemList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                BloodBankBAL objAPMCEBAL = new BloodBankBAL();
                int result = objAPMCEBAL.SaveListofItemsForm27E(objItemList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankForm27ETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "List of items saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Jai, 20-08-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please add atleast one Item to the list";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetBloodBankListofItemForm27E(int transactionId)
        {
            bloodbankBAL = new BloodBankBAL();
            List<BloodBankListModel> listItems = bloodbankBAL.GetBloodBankListItemsFomr27E(transactionId);
            Session.SetDataToSession<List<BloodBankListModel>>("BBEItemList", listItems);
            return Json(listItems);
        }
        #endregion

        #region BloodBank Saving Equipment Details
        public ActionResult AddBloodBankEquipmenForm27E(EquipmentModel model)
        {
            if (true)
            {
                // HttpPostedFileBase _uploadedFile = Request.Files[0];

                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                List<EquipmentModel> objEquipmentsList;
                if (Session["BBEEquipmentsList"] != null)
                    objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("BBEEquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                else
                    objEquipmentsList = new List<EquipmentModel>();
                objEquipmentsList.Add(model);
                Session.SetDataToSession<List<EquipmentModel>>("BBEEquipmentsList", objEquipmentsList);
                //TempData["EquipmentsList"] = objEquipmentsList;

                return Json(objEquipmentsList);
            }
            else
            {
                return Json("Invalid data");
            }

        }
        public JsonResult DeleteBloodBankEquipmentForm27E(int index)
        {
            if (Session["BBEEquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("BBEEquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                if (objEquipmentsList[index].Id == 0)
                    objEquipmentsList.RemoveAt(index);
                else
                    objEquipmentsList[index].IsDeleted = true;
                Session.SetDataToSession<List<EquipmentModel>>("EquipmentsList", objEquipmentsList);
                //TempData["EquipmentsList"] = objEquipmentsList;
                return Json(objEquipmentsList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }


        public JsonResult SaveBloodBankEquipmentForm27E(ApplicationType applicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BBEEquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("BBEEquipmentsList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEquipmentsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                BloodBankBAL objAPMCEBAL = new BloodBankBAL();
                int result = objAPMCEBAL.SaveEquipmentForm27E(objEquipmentsList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankForm27ETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipments details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Jai, 03-10-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please Add Atleast one Equpment List";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }

        public JsonResult GetBloodBankEquipmentForm27E(int transactionId)
        {
            bloodbankBAL = new BloodBankBAL();
            List<EquipmentModel> equipmentList = bloodbankBAL.GetBloodBankEquipmentForm27E(transactionId);
            Session.SetDataToSession<List<EquipmentModel>>("BBEEquipmentsList", equipmentList);
            return Json(equipmentList);
        }
        #endregion

        #region BloodBank Declaration saving Form 27E
        public JsonResult SaveBloodBankDeclarationForm27E(BloodBankAttachments model, HttpPostedFileBase planPremises, HttpPostedFileBase OwnerPremises, HttpPostedFileBase IdProff)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                #region File Saving
                var uploadsPath = Path.Combine("BloodBank", "Declaration");


                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                if (planPremises != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(planPremises.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.planPremisesPath = _addressProofPath + Path.GetExtension(planPremises.FileName);

                    planPremises.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(planPremises.FileName));
                }
                else if (Session["BBEPlanPremises"] != null)
                {
                    model.planPremisesPath = Session.GetDataFromSession<string>("BBEPlanPremises");
                }

                if (OwnerPremises != null)
                {
                    string _buildingLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(OwnerPremises.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.OwnerPremisesPath = _buildingLayoutPath + Path.GetExtension(OwnerPremises.FileName);

                    OwnerPremises.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(OwnerPremises.FileName));
                }
                else if (Session["BBEOwnerPremises"] != null)
                {
                    model.OwnerPremisesPath = Session.GetDataFromSession<string>("BBEOwnerPremises");
                }
                if (IdProff != null)
                {
                    string _idLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(IdProff.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.IdProffPath = _idLayoutPath + Path.GetExtension(IdProff.FileName);

                    IdProff.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _idLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _idLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(IdProff.FileName));
                }
                else if (Session["BBEIdProff"] != null)
                {
                    model.IdProffPath = Session.GetDataFromSession<string>("BBEIdProff");
                }
                #endregion

                BloodBankBAL objBAL = new BloodBankBAL();
                int result = objBAL.SaveDeclarationForm27E(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankForm27ETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Declaration details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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

                return Json(notification);
            }
        }
        #endregion

        #region BloodBank Technical
        public JsonResult UploadTechnicalDocument(DocumentUploadModel document)
        {
            HttpPostedFileBase _uploadedFile = Request.Files[0];

            var uploadsPath = Path.Combine("Applicant", "Temp");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            document.DocumentPath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);
            document.ReferenceTable = "t_employee";
            document.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            #region Saving files physically
            _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
            #endregion


            List<DocumentUploadModel> documentList;
            if (Session["BloodBankUploadList"] != null)
                documentList = Session.GetDataFromSession<List<DocumentUploadModel>>("BloodBankUploadList");
            else
                documentList = new List<DocumentUploadModel>();
            documentList.Add(document);
            Session.SetDataToSession<List<DocumentUploadModel>>("BloodBankUploadList", documentList);

            return Json(documentList);
        }

        // *AddUploads* is Depricated       - Jai, 19-08-2017
        public JsonResult AddTechnicalUploads(TechnicalModel model)
        {

            if (true)
            {
                // int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                // int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<TechnicalModel> objUploadList;
                if (Session["BloodBankUploadList"] != null)
                    objUploadList = Session.GetDataFromSession<List<TechnicalModel>>("BloodBankUploadList");
                else
                    objUploadList = new List<TechnicalModel>();
                objUploadList.Add(model);
                Session.SetDataToSession<List<TechnicalModel>>("BloodBankUploadList", objUploadList);

                return Json(objUploadList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteTechnicalDocument(int index)
        {
            if (Session["BloodBankUploadList"] != null)
            {
                List<DocumentUploadModel> objItemList = Session.GetDataFromSession<List<DocumentUploadModel>>("BloodBankUploadList");
                if (objItemList[index].Id == 0)
                    objItemList.RemoveAt(index);
                else
                    objItemList[index].IsDeleted = true;
                Session.SetDataToSession<List<DocumentUploadModel>>("BloodBankUploadList", objItemList);
                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }


        public JsonResult AddTechnicalStaff(TechnicalModel model)
        {

            if (true)
            {
                if (Session["BloodBankUploadList"] != null)
                {
                    model.UploadDocuments = Session.GetDataFromSession<List<DocumentUploadModel>>("BloodBankUploadList");
                    Session["BloodBankUploadList"] = null;
                }

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<TechnicalModel> objUploadList;
                if (Session["BloodBankTechnicalList"] != null)
                    objUploadList = Session.GetDataFromSession<List<TechnicalModel>>("BloodBankTechnicalList");
                else
                    objUploadList = new List<TechnicalModel>();
                objUploadList.Add(model);
                Session.SetDataToSession<List<TechnicalModel>>("BloodBankTechnicalList", objUploadList);

                return Json(objUploadList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteBloodbankTechnicalStaff(int index)
        {
            if (Session["BloodBankTechnicalList"] != null)
            {
                List<TechnicalModel> objItemList = Session.GetDataFromSession<List<TechnicalModel>>("BloodBankTechnicalList");
                if (objItemList[index].Id == 0)
                    objItemList.RemoveAt(index);
                else
                    objItemList[index].IsDeleted = true;
                Session.SetDataToSession<List<TechnicalModel>>("BloodBankTechnicalList", objItemList);
                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult SaveBloodBankTechnicalDetails()
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BloodBankTechnicalList"] != null)
            {
                List<TechnicalModel> objEmployeeList = Session.GetDataFromSession<List<TechnicalModel>>("BloodBankTechnicalList");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BloodBankForm27ETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BloodBankForm27ETransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;
                ApplicationType applicationType = ApplicationType.Grant;
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEmployeeList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                BloodBankBAL objBloodBankBAL = new BloodBankBAL();
                int result = objBloodBankBAL.SaveBloodBankTechnicalDetails(objEmployeeList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, applicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BloodBankForm27ETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Technical details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {
                // TODO: Implement this neatly      - Jai, 03-10-2017
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please add at least one Technical List";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetBloodBankTechnicalDetails(int transactionId)
        {
            bloodbankBAL = new BloodBankBAL();
            List<TechnicalModel> technicalList = bloodbankBAL.GetBloodBankTechnicalStaff(transactionId);
            Session.SetDataToSession<List<TechnicalModel>>("BloodBankTechnicalList", technicalList);
            return Json(technicalList);
        }
        #endregion

        #endregion

        #region Bio Capstone Application 

        #region  Particulars of applicant
        public JsonResult SaveBioCapstoneApplicantDetails(BioCapstoneApplicantViewModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();

                int result = objBioCapstoneBAL.SaveBioApplicantDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Particulars of Applicant details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - kishore, 07-09-2017
                return Json(notification);
            }
        }
        #endregion

        #region Authorisation of activity
        public JsonResult SaveAuthorisationActivity(AuthorisationViewModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                BioCapstoneBAL objSaveBioCapstoneBAL = new BioCapstoneBAL();
                int result = objSaveBioCapstoneBAL.SaveAuthorisationActivity(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Authorisation of Activity details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - kishore, 08-09-2017
                return Json(notification);
            }
        }
        #endregion

        #region Address of Treatment Facility
        public JsonResult SaveBioCapstoneTreatmentDetails(BioCapstoneAddressTreatmentFacilityViewModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();

                int result = objBioCapstoneBAL.SaveBioCapstoneTreatmentDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "BioCapstone Address of Treatment Facility details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - Raj, 12-05-2017
                return Json(notification);
            }
        }
        #endregion

        #region Address of Disposal waste
        public JsonResult SaveBioCapstoneDisposalwaste(BioCapstoneAddressofDisposalWaste model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();

                int result = objBioCapstoneBAL.SaveBioCapstoneDisposalwaste(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "BioCapstone Address of Disposal waste details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - kishore, 06-09-2017
                return Json(notification);
            }
        }
        #endregion

        #region Mode of treatment
        public JsonResult BioCapstoneAdddressUploads(TreatmentModle model)
        {
            HttpPostedFileBase _uploadedFile = Request.Files[0];

            var uploadsPath = Path.Combine("BioCapstone", "Address");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            model.Attachment = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

            //  model.u = Session.GetDataFromSession<UserModel>("User").Id;

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            #region Saving files physically
            _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
            #endregion


            List<TreatmentModle> treatmentModeList;
            if (Session["BioCapstoneTreatmentModeList"] != null)
                treatmentModeList = Session.GetDataFromSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList");
            else
                treatmentModeList = new List<TreatmentModle>();
            treatmentModeList.Add(model);
            Session.SetDataToSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList", treatmentModeList);
            return Json(treatmentModeList.Where(item => item.IsDeleted == false).ToList());
        }
        public JsonResult DeleteAddressDocument(int index)
        {
            if (Session["BioCapstoneTreatmentModeList"] != null)
            {
                List<TreatmentModle> treatmentModeList = Session.GetDataFromSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList");
                if (treatmentModeList[index].Id == 0)
                    treatmentModeList.RemoveAt(index);
                else
                    treatmentModeList[index].IsDeleted = true;
                Session.SetDataToSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList", treatmentModeList);
                return Json(treatmentModeList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public ActionResult SaveModeoftreatment(string ApplicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BioCapstoneTreatmentModeList"] != null)
            {
                List<TreatmentModle> objTreatmentList = Session.GetDataFromSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList");
                //TempData.Peek("EquipmentsList") as List<EquipmentModel>;
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objTreatmentList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();
                int result = objBioCapstoneBAL.SaveModeoftreatment(objTreatmentList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, ApplicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Mode of treatment details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {

                notification.Title = "Warning";
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }

        public JsonResult Gettreatment(int transactionId)
        {
            BioCapstoneBAL objBAL = new BioCapstoneBAL();
            string modeType = "AddressOfTreatment";
            List<TreatmentModle> treatmentList = objBAL.GetTreatment(transactionId, modeType);
            Session.SetDataToSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList", treatmentList);
            return Json(treatmentList);
        }
        #endregion

        #region Mode of treatment and disposal
        public JsonResult BioCapstoneDisposalUploads(TreatmentDisposalModle model)
        {
            HttpPostedFileBase _uploadedFile = Request.Files[0];

            var uploadsPath = Path.Combine("BioCapstone", "Disposal");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            model.Attachment = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

            //  model.u = Session.GetDataFromSession<UserModel>("User").Id;

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            #region Saving files physically
            _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
            #endregion


            List<TreatmentDisposalModle> disposaldocumentList;
            if (Session["BioCapstoneDisposalList"] != null)
                disposaldocumentList = Session.GetDataFromSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList");
            else
                disposaldocumentList = new List<TreatmentDisposalModle>();
            disposaldocumentList.Add(model);
            Session.SetDataToSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList", disposaldocumentList);

            return Json(disposaldocumentList.Where(item => item.IsDeleted == false).ToList());
        }
        public JsonResult DeleteDisposalDocument(int index)
        {
            if (Session["BioCapstoneDisposalList"] != null)
            {
                List<TreatmentDisposalModle> disposalAttachmentList = Session.GetDataFromSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList");
                if (disposalAttachmentList[index].Id == 0)
                    disposalAttachmentList.RemoveAt(index);
                else
                    disposalAttachmentList[index].IsDeleted = true;
                Session.SetDataToSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList", disposalAttachmentList);
                return Json(disposalAttachmentList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public ActionResult SaveModeofTreatmentDisposal(string ApplicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["BioCapstoneDisposalList"] != null)
            {
                List<TreatmentDisposalModle> objTreatmentDisposalList = Session.GetDataFromSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList");
                //TempData.Peek("EquipmentsList") as List<EquipmentModel>;
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objTreatmentDisposalList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();
                int result = objBioCapstoneBAL.SaveModeofTreatmentDisposal(objTreatmentDisposalList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, ApplicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Mode of Treatment and disposal details saved successfully.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {

                notification.Title = "Warning";
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetTreatmentDisposal(int transactionId)
        {
            BioCapstoneBAL objBAL = new BioCapstoneBAL();
            string modeType = "AddressOfTreatmentDisposal";
            List<TreatmentDisposalModle> treatmentDisposalList = objBAL.GetTreatmentDisposal(transactionId, modeType);
            Session.SetDataToSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList", treatmentDisposalList);
            return Json(treatmentDisposalList);
        }
        #endregion

        #region Category an Quantity of Waste
        public JsonResult BioCapstoneWaste(QuantityWasteModel model)
        {
            if (ModelState.IsValid)
            {

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                List<QuantityWasteModel> objItemList;
                if (Session["QuantityWasteList"] != null)
                    objItemList = Session.GetDataFromSession<List<QuantityWasteModel>>("QuantityWasteList");
                else
                    objItemList = new List<QuantityWasteModel>();
                objItemList.Add(model);
                Session.SetDataToSession<List<QuantityWasteModel>>("QuantityWasteList", objItemList);

                return Json(objItemList.Where(item => item.IsDeleted == false).ToList());
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteWaste(int index)
        {
            if (Session["QuantityWasteList"] != null)
            {
                List<QuantityWasteModel> objWasteList = Session.GetDataFromSession<List<QuantityWasteModel>>("QuantityWasteList");
                if (objWasteList[index].Id == 0)
                    objWasteList.RemoveAt(index);
                else
                    objWasteList[index].IsDeleted = true;
                Session.SetDataToSession<List<QuantityWasteModel>>("QuantityWasteList", objWasteList);
                return Json(objWasteList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public ActionResult SaveQuantityofWaste(string ApplicationType)
        {
            NotificationModel notification = new NotificationModel();
            if (Session["QuantityWasteList"] != null)
            {
                List<QuantityWasteModel> objQuantityWasteList = Session.GetDataFromSession<List<QuantityWasteModel>>("QuantityWasteList");
                //TempData.Peek("EquipmentsList") as List<EquipmentModel>;
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = FormStatus.Empty;
                string _applicationNumber = string.Empty;

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objQuantityWasteList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();
                int result = objBioCapstoneBAL.SaveQuantityofWaste(objQuantityWasteList, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber, ApplicationType);

                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Category an Quantity of Waste details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = _transactionId + "," + formStatus;
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
            }
            else
            {

                notification.Title = "Warning";
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public JsonResult GetQuantityWaste(int transactionId)
        {
            BioCapstoneBAL objBAL = new BioCapstoneBAL();
            List<QuantityWasteModel> quantityWasteList = objBAL.GetQuantityWaste(transactionId);
            Session.SetDataToSession<List<QuantityWasteModel>>("QuantityWasteList", quantityWasteList);
            return Json(quantityWasteList);
        }
        #endregion

        #region  Declaration
        public JsonResult SaveBioDeclarationDetails(DeclarationViewModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();

                int result = objBioCapstoneBAL.SaveBioDeclarationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Declaration details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - kishore, 07-09-2017
                return Json(notification);
            }
        }
        #endregion






        //public JsonResult SaveBioCapstoneDetails(BioCapstoneViewModel model)
        //{
        //    NotificationModel notification = new NotificationModel();
        //    if (true)
        //    {
        //        int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
        //        int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
        //        string _ApplicationNumber = string.Empty;
        //        model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

        //        LicenseBAL objBAL = new LicenseBAL();


        //        List<TreatmentModle> TreatmentList = new List<TreatmentModle>();
        //        if (Session["BioCapstoneAddressList"] != null)
        //            TreatmentList = Session.GetDataFromSession<List<TreatmentModle>>("BioCapstoneAddressList");
        //        model.TreatmentList = TreatmentList;

        //        List<TreatmentDisposalModle> TreatmentDisposalList = new List<TreatmentDisposalModle>();
        //        if (Session["BioCapstoneDisposalList"] != null)
        //            TreatmentDisposalList = Session.GetDataFromSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList");
        //        model.TreatmentDisposalList = TreatmentDisposalList;

        //        List<QuantityWasteModel> QuantityWasteList = new List<QuantityWasteModel>();
        //        if (Session["QuantityWasteList"] != null)
        //            QuantityWasteList = Session.GetDataFromSession<List<QuantityWasteModel>>("QuantityWasteList");
        //        model.QuantityWasteList = QuantityWasteList;

        //        UserBAL objBioCapstoneBAL = new UserBAL();
        //        int result = objBAL.SaveBioCapstoneApplicantDetails(model, ref _applicationId, ref _transactionId,ref _ApplicationNumber);
        //        String FileName = "D:\\BioMedial_Test." + "BM";
        //        FileHandling.ReadWrite2Files fH = new FileHandling.ReadWrite2Files();
        //        fH.SerializeObject<BioCapstoneViewModel>(model, FileName);

        //        if (result>0)
        //        {
        //            Session.SetDataToSession<int>("ApplicationId", _applicationId);
        //            Session.SetDataToSession<int>("APMCETransactionId", _transactionId);
        //            notification.Title = "Success";
        //            notification.NotificationType = NotificationType.Success;
        //            notification.NotificationMessage = "BioCapstone Details saved. <br>Your application is <b>" + _ApplicationNumber + "</b>";
        //            notification.ShowActionButton = true;
        //            notification.ActionButtonText = "Go To Submitted";
        //            notification.ActionName = "Submitted";
        //            notification.ControllerName = "Dashboard";
        //            notification.AreaName = "User";
        //            //  notification.NonActionButtonClassType = PopupButtonClass.Success;
        //            // notification.NonActionButtonText = "Okay";

        //        }
        //        else
        //        {
        //            // TODO: Implement this neatly      - Raj, 19-05-2017
        //            notification.Title = "Warning";
        //            notification.NotificationType = NotificationType.Warning;
        //            notification.NotificationMessage = "Please clear error validations";
        //            notification.ShowNonActionButton = true;
        //            notification.NonActionButtonClassType = PopupButtonClass.Warning;
        //            notification.NonActionButtonText = "Okay";
        //        }

        //        return Json(notification);
        //    }

        //}
        public ActionResult GetBioCapstoneDetails()
        {
            FileHandling.ReadWrite2Files fH = new FileHandling.ReadWrite2Files();
            BioCapstoneViewModel objBioCapstoneviewModel = fH.DeSerializeObject<BioCapstoneViewModel>("E:\\BioMedial_Test.BM");

            //string Name = "BM";
            //LicenseBAL objBAL = new LicenseBAL();
            //objBAL.GetBioCapstoneDetails(Name);
            return View();
        }

        #region BioCapstone Applicant saving
        public JsonResult SaveBioCapstoneDetails(BioCapstoneViewModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["BioCapstoneTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("BioCapstoneTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                List<TreatmentModle> TreatmentList = new List<TreatmentModle>();
                if (Session["BioCapstoneTreatmentModeList"] != null)
                    TreatmentList = Session.GetDataFromSession<List<TreatmentModle>>("BioCapstoneTreatmentModeList");
                model.TreatmentList = TreatmentList;

                List<TreatmentDisposalModle> TreatmentDisposalList = new List<TreatmentDisposalModle>();
                if (Session["BioCapstoneDisposalList"] != null)
                    TreatmentDisposalList = Session.GetDataFromSession<List<TreatmentDisposalModle>>("BioCapstoneDisposalList");
                model.TreatmentDisposalList = TreatmentDisposalList;

                List<QuantityWasteModel> QuantityWasteList = new List<QuantityWasteModel>();
                if (Session["QuantityWasteList"] != null)
                    QuantityWasteList = Session.GetDataFromSession<List<QuantityWasteModel>>("QuantityWasteList");
                model.QuantityWasteList = QuantityWasteList;

                BioCapstoneBAL objBioCapstoneBAL = new BioCapstoneBAL();

                int result = objBioCapstoneBAL.SaveBioCapstoneApplicationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("BioCapstoneTransactionId", _transactionId);

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
                // TODO: Return model validations     
                return Json(notification);
            }
        }
        #endregion



        #endregion

        #region Homeopathy
        public JsonResult SaveHomeopathyDetails(HomeopathyDrugStoreViewModel model, HttpPostedFileBase UploadDocument, HttpPostedFileBase RentalDocument, HttpPostedFileBase PlanPremisesDocument, HttpPostedFileBase AddressProff, HttpPostedFileBase CoveringLetter)
        {
            NotificationModel notification = new NotificationModel();

            if (true)
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["HomepathyTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("HomepathyTransactionId");
                string _applicationNumber = string.Empty;
                FormStatus formStatus = model.HDApplicantModel.FormStatus;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                string _serviceId = "35";
                var uploadsPath = Path.Combine("homeopathy", model.CreatedUserId.ToString(), _serviceId, "homeopathy");


                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (UploadDocument != null)
                {
                    string _documentsProof = uploadsPath + "/" + Path.GetFileNameWithoutExtension(UploadDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.HDApplicantModel.UploadDocument = _documentsProof + Path.GetExtension(UploadDocument.FileName);

                    UploadDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _documentsProof));
                    string documentfilepath = Path.Combine(Server.MapPath("~/Uploads"), _documentsProof);
                    System.IO.File.Move(documentfilepath, documentfilepath + Path.GetExtension(UploadDocument.FileName));
                }
                else if (Session["HomepathyApplicantUpload"] != null)
                {
                    model.HDApplicantModel.UploadDocument = Session.GetDataFromSession<string>("HomepathyApplicantUpload");
                }

                if (RentalDocument != null)
                {
                    string _addressProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(RentalDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.HDEstablishment.RentalDocument = _addressProofPath + Path.GetExtension(RentalDocument.FileName);

                    RentalDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(RentalDocument.FileName));
                }
                else if (Session["HomepathyRentDocument"] != null)
                {
                    model.HDEstablishment.RentalDocument = Session.GetDataFromSession<string>("HomepathyRentDocument");
                }

                if (PlanPremisesDocument != null)
                {
                    string _buildingLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(PlanPremisesDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.HDEstablishment.PlanPremisesDocument = _buildingLayoutPath + Path.GetExtension(PlanPremisesDocument.FileName);

                    PlanPremisesDocument.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(PlanPremisesDocument.FileName));
                }
                else if (Session["HomepathyPlanPremisesDocument"] != null)
                {
                    model.HDEstablishment.PlanPremisesDocument = Session.GetDataFromSession<string>("HomepathyPlanPremisesDocument");
                }
                if (AddressProff != null)
                {
                    string _addressLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(AddressProff.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.HDEstablishment.AddressProff = _addressLayoutPath + Path.GetExtension(AddressProff.FileName);

                    AddressProff.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _addressLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _addressLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(AddressProff.FileName));
                }
                else if (Session["HomepathyAddressDocument"] != null)
                {
                    model.HDEstablishment.AddressProff = Session.GetDataFromSession<string>("HomepathyAddressDocument");
                }
                if (CoveringLetter != null)
                {
                    string _letterLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(CoveringLetter.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.HDDeclaration.CoveringLetter = _letterLayoutPath + Path.GetExtension(CoveringLetter.FileName);

                    CoveringLetter.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _letterLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _letterLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(CoveringLetter.FileName));
                }
                else if (Session["HomepathyLetterDocument"] != null)
                {
                    model.HDDeclaration.CoveringLetter = Session.GetDataFromSession<string>("HomepathyLetterDocument");
                }
                #endregion

                objBAL = new LicenseBAL();
                int result = objBAL.SaveHomeopathyDetails(model, ref _applicationId, ref _transactionId,
    ref formStatus, ref _applicationNumber);
                //String FileName = "D:\\Homeopathy_TestDetails." + "HD";
                //FileHandling.ReadWrite2Files fH = new FileHandling.ReadWrite2Files();
                //fH.SerializeObject<HomeopathyDrugStoreViewModel>(model, FileName);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("HomepathyTransactionId", _transactionId);
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Homepathy details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    notification.ShowNonActionButton = true;
                    notification.ActionButtonText = "Go To Submitted";
                    //notification.ActionName = "Submitted";
                    //notification.ControllerName = "Dashboard";
                    //notification.AreaName = "User";
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
                // TODO: Return model validations       - Jai, 29-08-2017
                return Json(notification);
            }
        }

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

            if (true)   //ModelState.IsValid
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

            if (true)   // ModelState.IsValid
            {
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["HomepathyTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("HomepathyTransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;


                #region File Saving
                string _serviceId = "35";
                var uploadsPath = Path.Combine("Document", model.CreatedUserId.ToString()
                    , _serviceId, "Document");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                if (CoveringLetter != null)
                {
                    string _letterProofPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(CoveringLetter.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.CoveringLetter = _letterProofPath + Path.GetExtension(CoveringLetter.FileName);

                    CoveringLetter.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _letterProofPath));
                    string addressfilepath = Path.Combine(Server.MapPath("~/Uploads"), _letterProofPath);
                    System.IO.File.Move(addressfilepath, addressfilepath + Path.GetExtension(CoveringLetter.FileName));
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

        #region Organ Transplant
        public ActionResult OrganTransplant1()
        {
            ClearallSessions();
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            return View();
        }

        public ActionResult SurgicalStaffList(OTStaffDetailsModel StaffDetails, string StaffType)
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
        public ActionResult EquipmentList(OTEquipmentModel Equipments, string EquipmentType)
        {
            List<OTEquipmentModel> OTEquipmentList = new List<OTEquipmentModel>();
            switch (EquipmentType)
            {
                case "anaesthesiology":
                    if (Session["anaesthesiologyEquipment"] != null)
                        OTEquipmentList = Session["anaesthesiologyEquipment"] as List<OTEquipmentModel>;
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
        public ActionResult SubmitOrganTransplant(OrganTransplantViewModel OrganTransplant)
        {
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
            LicenseBAL obj = new LicenseBAL();
            OrganTransplant.Surgical.StaffDetailsList = Session["SurgicalStaffList"] as List<OTStaffDetailsModel>;
            OrganTransplant.CapstoneTeam.StaffDetailsList = Session["CapstoneStaffList"] as List<OTStaffDetailsModel>;
            OrganTransplant.Anaesthesiology.StaffDetailsList = Session["anaesthesiologyStaffList"] as List<OTStaffDetailsModel>;
            OrganTransplant.LaboratoryFacilities.StaffDetailsList = Session["LaboratoryStaffList"] as List<OTStaffDetailsModel>;
            OrganTransplant.ImagingServices.StaffDetailsList = Session["ImagingStaffList"] as List<OTStaffDetailsModel>;
            OrganTransplant.HaematologyServices.StaffDetailsList = Session["HaematologyStaffList"] as List<OTStaffDetailsModel>;

            OrganTransplant.Anaesthesiology.EquipmentsList = Session["anaesthesiologyEquipment"] as List<OTEquipmentModel>;
            OrganTransplant.ICUHDUFacilities.EquipmentsList = Session["FacilitiesEquiment"] as List<OTEquipmentModel>;
            OrganTransplant.LaboratoryFacilities.EquipmentsList = Session["LaboratoryEquipment"] as List<OTEquipmentModel>;
            OrganTransplant.ImagingServices.EquipmentsList = Session["ImagingEquipment"] as List<OTEquipmentModel>;
            OrganTransplant.HaematologyServices.EquipmentsList = Session["HaematologyEquipment"] as List<OTEquipmentModel>;

            OrganTransplant.Anaesthesiology.OperationsList = Session["OperationList"] as List<OTOperationModel>;
            OrganTransplant.CreatedUserID = Session.GetDataFromSession<UserModel>("User").Id;
            bool Result = obj.SaveOrganTransplantation(OrganTransplant, ref _applicationId, ref _transactionId);
            String FileName = "D:\\BioMedial_Test." + "OT";

            FileHandling.ReadWrite2Files fH = new FileHandling.ReadWrite2Files();
            fH.SerializeObject<OrganTransplantViewModel>(OrganTransplant, FileName);
            // OrganTransplantViewModel objsome = fH.DeSerializeObject<OrganTransplantViewModel>("D:\\BioMedial_Test.OT");
            NotificationModel notification = new NotificationModel();
            if (Result)
            {
                Session.SetDataToSession<int>("ApplicationId", _applicationId);
                Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Equipments & Furniture details saved.<br>Your application is <b>" + _applicationId + "</b>";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Success;
                notification.NonActionButtonText = "Okay";

            }
            else
            {
                // TODO: Implement this neatly      
                notification.Title = "Warning";
                notification.NotificationType = NotificationType.Warning;
                notification.NotificationMessage = "Please clear error validations";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Warning;
                notification.NonActionButtonText = "Okay";
            }
            return Json(notification);
        }
        public ActionResult GetOrganTransplantDetails()
        {
            FileHandling.ReadWrite2Files fH = new FileHandling.ReadWrite2Files();
            OrganTransplantViewModel objBioCapstoneviewModel = fH.DeSerializeObject<OrganTransplantViewModel>("D:\\BioMedial_Test.OT");

            string Name = "BM";
            LicenseBAL objBAL = new LicenseBAL();
            // objBAL.GetBioCapstoneDetails(Name);
            return View();
        }
        public JsonResult SaveHospitalDetails(HospitalViewModel hospitalModel)
        {
            NotificationModel notification = new NotificationModel();
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["OTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("OTTransactionId");
            FormStatus formStatus = hospitalModel.FormStatus;
            string _applicationNumber = string.Empty;
            hospitalModel.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            OrganTransplantBAL objBAL = new OrganTransplantBAL();
            int result = objBAL.SaveHospitalDetails(hospitalModel, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
            if (result > 0)
            {
                Session.SetDataToSession<int>("ApplicationId", _applicationId);
                Session.SetDataToSession<int>("OTTransactionId", _transactionId);

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
        public JsonResult SaveSurgicalDetails(SurgicalTeamModel surgicalModel)
        {
            NotificationModel notification = new NotificationModel();
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["OTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("OTTransactionId");
            FormStatus formStatus = surgicalModel.FormStatus;
            string _applicationNumber = string.Empty;
            surgicalModel.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            OrganTransplantBAL objBAL = new OrganTransplantBAL();
            surgicalModel.StaffDetailsList = Session["SurgicalStaffList"] as List<OTStaffDetailsModel>;
            surgicalModel.StaffDetailsList.ForEach(item => item.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id);
            surgicalModel.StaffDetailsList.ForEach(item => item.SectionName = "Surgical");
            int result = objBAL.SaveSurgicalDetails(surgicalModel, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
            if (result > 0)
            {
                Session.SetDataToSession<int>("ApplicationId", _applicationId);
                Session.SetDataToSession<int>("OTTransactionId", _transactionId);

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
        public JsonResult SaveAnaesthesiologyDetails(AnaesthesiologyModel AnaesthesiologyModel)
        {
            NotificationModel notification = new NotificationModel();
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["OTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("OTTransactionId");
            FormStatus formStatus = AnaesthesiologyModel.FormStatus;
            string _applicationNumber = string.Empty;
            AnaesthesiologyModel.UserId = Session.GetDataFromSession<UserModel>("User").Id;
            OrganTransplantBAL objBAL = new OrganTransplantBAL();
            // Staff List
            AnaesthesiologyModel.StaffDetailsList = Session["anaesthesiologyStaffList"] as List<OTStaffDetailsModel>;
            AnaesthesiologyModel.StaffDetailsList.ForEach(item => item.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id);
            AnaesthesiologyModel.StaffDetailsList.ForEach(item => item.SectionName = "Anaesthesiology");
            //Operation List
            AnaesthesiologyModel.OperationsList = Session["OperationList"] as List<OTOperationModel>;
            AnaesthesiologyModel.OperationsList.ForEach(item => item.UserId = Session.GetDataFromSession<UserModel>("User").Id);
            AnaesthesiologyModel.OperationsList.ForEach(item => item.SectionName = "Anaesthesiology");
            //Equipment List
            AnaesthesiologyModel.EquipmentsList = Session["anaesthesiologyEquipment"] as List<OTEquipmentModel>;
            AnaesthesiologyModel.EquipmentsList.ForEach(item => item.UserId = Session.GetDataFromSession<UserModel>("User").Id);
            AnaesthesiologyModel.EquipmentsList.ForEach(item => item.SectionName = "Anaesthesiology");
            int result = objBAL.SaveAnaesthesiologyDetails(AnaesthesiologyModel, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
            if (result > 0)
            {
                Session.SetDataToSession<int>("ApplicationId", _applicationId);
                Session.SetDataToSession<int>("OTTransactionId", _transactionId);

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
        public JsonResult SaveCapstoneDetails(CapstoneTeamModel CapstoneModel)
        {
            NotificationModel notification = new NotificationModel();
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["OTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("OTTransactionId");
            FormStatus formStatus = CapstoneModel.FormStatus;
            string _applicationNumber = string.Empty;
            CapstoneModel.UserId = Session.GetDataFromSession<UserModel>("User").Id;
            OrganTransplantBAL objBAL = new OrganTransplantBAL();
            CapstoneModel.StaffDetailsList = Session["CapstoneStaffList"] as List<OTStaffDetailsModel>;
            CapstoneModel.StaffDetailsList.ForEach(item => item.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id);
            CapstoneModel.StaffDetailsList.ForEach(item => item.SectionName = "Capstone");
            int result = objBAL.SaveCapstoneDetails(CapstoneModel, ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);
            if (result > 0)
            {
                Session.SetDataToSession<int>("ApplicationId", _applicationId);
                Session.SetDataToSession<int>("OTTransactionId", _transactionId);

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
        public string GetOTStaffDetails(string SectionName)
        {
            OrganTransplantBAL bal = new OrganTransplantBAL();
            return JsonConvert.SerializeObject(bal.GetStaffDetails(Session.GetDataFromSession<int>("OTTransactionId"), SectionName));
        }
        public string GetOTOperationsList(string SectionName)
        {
            OrganTransplantBAL bal = new OrganTransplantBAL();
            return JsonConvert.SerializeObject(bal.GetOperations(Session.GetDataFromSession<int>("OTTransactionId"), SectionName));
        }
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

        public ActionResult ExistingUserRegistration()
        {
            return View();
        }

        public ActionResult AddDraft(int transactionId)
        {
            objBAL = new LicenseBAL();
            int _userId = Session.GetDataFromSession<UserModel>("User").Id;
            bool result = objBAL.UpdateUserIdForExistingData(_userId, transactionId);
            return RedirectToAction("Edit", new { TransactionId = transactionId, TransactionType = "Transaction" });
        }

        public ActionResult AddExistingLicenseDetails()
        {
            objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ExistingLicense model = new ExistingLicense();
            model.Id = 0;
            return View(model);
        }
        public JsonResult SaveExistingLicenseDetails(ExistingLicense model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                var user = Session.GetDataFromSession<UserModel>("User");
                int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.Status;
                string _applicationNumber = string.Empty;

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                model.LicenseExpiryDate = model.LicenseIssueDate.AddDays(365);

                APMCEBAL objAPMCEBAL = new APMCEBAL();

                bool IsExistingLicenseNumberExists = false;
                IsExistingLicenseNumberExists = objAPMCEBAL.IsExistingLicenseNumberExists(model.ExistingLicenseNumber,model.ApplicationNumber);
                if(IsExistingLicenseNumberExists)
                {
                    notification.Title = "Error";
                    notification.NotificationType = NotificationType.Danger;
                    notification.NotificationMessage = "Application No./License No. is Already Exists!";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = "0," + FormStatus.Empty;
                    return Json(notification);
                }

                int result = objAPMCEBAL.SaveExistingLicenseDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    if (model.Id == 0)
                    {
                        Session.SetDataToSession<int>("ApplicationId", _applicationId);
                        Session.SetDataToSession<int>("APMCETransactionId", _transactionId);
                        Session.SetDataToSession<int>("DistrictId", model.DistrictID);  // this will be used to get account id to make payment  - Raj K, 2021-01-01

                        notification.Title = "Success";
                        notification.NotificationType = NotificationType.Success;
                        notification.NotificationMessage = "Existing License details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                        notification.ShowNonActionButton = true;
                        notification.NonActionButtonClassType = PopupButtonClass.Success;
                        notification.NonActionButtonText = "Okay";
                        notification.ReturnData = result.ToString() + "," + formStatus;

                        // Send SMS
                        string msgBody = "Hi " + model.Name + "," + " <br/><br/> Your Existing License Details ( " + model.ExistingLicenseNumber + ") Added in TAMCE By " + user.UserName + ".<br/><br/> Please Register with Existing Registration Login From Below URL : <br/><br/> http://tamce.telangana.gov.in/ <br/><br/><br/>Thanks & Regards,<br/>TAMCE Team.<br/> ";
                        string deliveryStatus;
                        bool smsresult = Utitlities.SendSMS(model.MobileNo, msgBody, out deliveryStatus);

                        // Send email
                        string emailBody = "Hi " + model.Name + "," + " <br/><br/> Your Existing License Details ( "+ model.ExistingLicenseNumber +") Added in TAMCE By " + user.UserName + ".<br/><br/> Please Register with Existing Registration Login From Below URL : <br/><br/> http://tamce.telangana.gov.in/ <br/><br/><br/>Thanks & Regards,<br/>TAMCE Team.<br/> ";
                        Utitlities.SendEmail(model.Email,emailBody,"TAMCE Existing License Details" );
                    }
                    else if(model.Id > 0)
                    {
                        notification.NotificationType = NotificationType.Success;
                        notification.ShowActionButton = true;
                        notification.ActionButtonText = "Go to 'Index'";
                        notification.ActionButtonClassType = PopupButtonClass.Success;
                        notification.Title = "Success";
                        notification.NotificationMessage = "Existing License details Updated successfully";
                        notification.AreaName = "User";
                        notification.ControllerName = "License";
                        notification.ActionName = "ExistingLicensesIndex";
                        notification.ReturnData = result.ToString() + "," + formStatus;
                    }
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
            return null;
        }

        public ActionResult ExistingLicensesIndex()
        {
            objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            return View();
        }
        public string GetExistingLicensesIndex()
        {
            APMCEBAL objAPMCEBAL = new APMCEBAL();
            var user = Session.GetDataFromSession<UserModel>("User");
            var data = objAPMCEBAL.GetExistingLicensesIndex(user.Id);
            return JsonConvert.SerializeObject(data);
        }
        public string BindExistingLicenseTrans(int TransactionId)
        {
            APMCEBAL objAPMCEBAL = new APMCEBAL();
            var user = Session.GetDataFromSession<UserModel>("User");
            var data = objAPMCEBAL.BindExistingLicenseTrans(TransactionId);
            return JsonConvert.SerializeObject(data);
        }
        #region Delete Existing License
        public JsonResult DeleteExistingLicense(int Id)
        {
            var User = Session.GetDataFromSession<UserModel>("User");
            //var UserId = Session.GetDataFromSession<UserModel>("User").Id;
            NotificationModel notification = new NotificationModel();
            APMCEBAL objAPMCEBAL = new APMCEBAL();

            //#region Verify - Assigned Inspection Report Submitted by IO.

            //bool IsInspectionReportSubmitted = objBAL.CheckIsInspectionReportSubmitted(Id);
            //if (IsInspectionReportSubmitted)
            //{
            //    notification.Title = "Warning";
            //    notification.NotificationType = NotificationType.Warning;
            //    notification.NotificationMessage = "Inspection Report Already Submitted.So unable to Delete this Inspection!";
            //    notification.ShowNonActionButton = true;
            //    notification.NonActionButtonClassType = PopupButtonClass.Success;
            //    notification.NonActionButtonText = "OK";
            //    return Json(new
            //    {
            //        notification = notification
            //    });
            //}
            //#endregion

            int result = 0;
            if (Id > 0)
            {
                result = objAPMCEBAL.DeleteExistingLicense(Id, User.Id);

                if (result > 0)
                {
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Record Deleted Successfully.";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "OK";
                }
                else
                {
                    notification.Title = "Error";
                    notification.NotificationType = NotificationType.Danger;
                    notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    notification.NonActionButtonText = "OK";
                }
                //  return Json(notification);
                return Json(new
                {
                    notification = notification
                });
            }
            else
            {
                return Json("");
            }
        }
        #endregion

        //[HttpPost]
        //public ActionResult Convert(FormCollection collection)
        //{
        //    HtmlToPdf obj = new HtmlToPdf();
        //   //  pdfConverter = new PdfConverter();
        //    string url = collection["TxtUrl"];
        //    //byte[] pdf = pdfConverter.GetPdfBytesFromUrl(url);

        //    obj.ConvertHtmlToFile(url, url, "RenderedPage.pdf");
        //    FileResult fileResult = new FileContentResult(obj, "application/pdf");
        //    fileResult.FileDownloadName = "RenderedPage.pdf";

        //    return fileResult;
        //}

        #region SendMail with attachment File
        public void SendEmailwithAttachmentFile(string to, string toName, string subject, string body, string attachment)
        {
            string mail = "tamcehelpdesk@gmail.com";
            string password = "support@acs123";
            string FromMail = "tamcehelpdesk@gmail.com";
            //var fromAddress = new MailAddress("Your Gemail Address", "Sender Project System Name");
            //var toAddress = new MailAddress(to, toName);
            string filedetails = Path.Combine(Server.MapPath("~/Uploads"),attachment);
            var att = new Attachment(filedetails);
            //const string fromPassword = "Your pass";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, password)
        };
            using (var message = new MailMessage(FromMail, to)
            {
                Subject = subject,
                Body = body
            })
            {
                message.Attachments.Add(att);
                smtp.Send(message);
            }
        }
        #endregion
        
    }
}