using Capstone.BAL;
using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace Capstone.Areas.User.Controllers
{
    [SessionTimeout]
    public class ApplicationController : Controller
    {
        LicenseBAL licenseBAL;
        PCPNDTBAL pcpndtBAL;
        UserModel user;
        ApplicationBAL applicationBAL;
        // GET: User/Application
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetAcknowledge(int applicationId ,string TableName)
        {
            applicationBAL = new ApplicationBAL();
            AcknowledgeModel model = applicationBAL.GetAcknowledgeDetails(applicationId, TableName);
            //return PartialView("_Acknowledgement", model);
            return PartialView("_TAMCEForm2Certificate", model);
        }
        public void ClearData()
        {
            Session["ApplicationId"] = null;
            Session["PCPNDTTransactionId"] = null;
            Session["EquipmentsList"] = null;
            Session["EmployeeList"] = null;
            Session["OtherUploadsList"] = null;
            Session["APMCETransactionId"] = null;
            Session["AddressProofPath"] = null;
            Session["BuildingLayoutPath"] = null;
            Session["InfraStructureList"] = null;
            Session["StaffDetailsList"] = null;
            Session["BioCapstoneAddressList"] = null;
            Session["BioCapstoneDisposalList"] = null;
            Session["QuantityWasteList"] = null;
            Session["HomepathyApplicantUpload"] = null;
            Session["HomepathyRentDocument"] = null;
            Session["HomepathyPlanPremisesDocument"] = null;
            Session["HomepathyAddressDocument"] = null;
            Session["HomepathyLetterDocument"] = null;



        }

        public JsonResult DeleteStudyCertificate(int id)
        {
            var studyCertificates = Session.GetDataFromSession<List<DocumentUploadModel>>("StudyCertificates");
            studyCertificates.Where(item => item.Id == id).First().IsDeleted = true;
            studyCertificates.Where(item => item.Id == id).First().
                LastModifiedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            Session.SetDataToSession<List<DocumentUploadModel>>("StudyCertificates", studyCertificates);

            return Json(studyCertificates.Where(item => item.IsDeleted == false).ToList());
        }

        public JsonResult Allopathic(AllopathicDrugStoreViewModel AllopathicDrugStore)
        {
            NotificationModel notification = new NotificationModel();

            applicationBAL = new ApplicationBAL();
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
            FormStatus formStatus = AllopathicDrugStore.ADApplicantModel.FormStatus;
            string _applicationNumber = string.Empty;
            if (Request.Files[0] != null)
            {
                HttpPostedFileBase _uploadedFile = Request.Files[0];
                var uploadsPath = Path.Combine("Applicant", AllopathicDrugStore.uploadForm, "Applicant");
                string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                AllopathicDrugStore.ADApplicantModel.UploadDocument = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                #region Saving files physically
                _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                #endregion
            }

            if (Session["User"] != null)
            {
                user = (UserModel)Session["User"];
            }
            List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();
            if (Session["AD19UploadList"] != null)
                UploadList = Session["AD19UploadList"] as List<DocumentUploadModel>;
            AllopathicDrugStore.ADCompetentPersonModel.uploadedDocuments = UploadList;
            List<string> DrugsList = new List<string>();
            if (Session["AD19DrugsList"] != null)
                DrugsList = Session["AD19DrugsList"] as List<string>;
           // AllopathicDrugStore.ADDrugNameModel.Drugs = DrugsList;

            int result = applicationBAL.SaveAllopathicDrugDetails(AllopathicDrugStore, user.Id,
                ref _applicationId, ref _transactionId, ref formStatus, ref _applicationNumber);

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
        public JsonResult AddDocuments(DocumentUploadModel Upload)
        {
            HttpPostedFileBase _uploadedFile = Request.Files[0];
            var uploadsPath = "";
            uploadsPath = Path.Combine("Applicant", "CompetentPerson");
            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            Upload.DocumentPath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
            #region Saving files physically
            _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
            #endregion

            List<DocumentUploadModel> UploadList;// = new List<DocumentUploadModel>();
            if (Upload.ReferenceId == 1)
            {
                if (Session["AD19UploadList"] != null)
                    UploadList = Session.GetDataFromSession<List<DocumentUploadModel>>("AD19UploadList");
                else
                    UploadList = new List<DocumentUploadModel>();
                UploadList.Add(Upload);
                Session.SetDataToSession<List<DocumentUploadModel>>("AD19UploadList", UploadList);

                // UploadList = Session["AD19UploadList"] as List<DocumentUploadModel>;
                //Upload.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                //UploadList.Add(Upload);
                //Session["AD19UploadList"] = UploadList;
            }
            else
            {
                UploadList = new List<DocumentUploadModel>();
                if (Session["AD19cUploadList"] != null)
                    UploadList = Session["AD19cUploadList"] as List<DocumentUploadModel>;
                Upload.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                UploadList.Add(Upload);
                Session["AD19cUploadList"] = UploadList;
            }
            return Json(UploadList);
        }
        public JsonResult DeleteAllopathicDrugStoreUploads(int index)
        {
            if (Session["AD19UploadList"] != null)
            {
                List<DocumentUploadModel> objDocumentList = Session.GetDataFromSession<List<DocumentUploadModel>>("AD19UploadList");
                if (objDocumentList[index].Id == 0)
                    objDocumentList.RemoveAt(index);
                else
                    objDocumentList[index].IsDeleted = true;
                Session.SetDataToSession<List<DocumentUploadModel>>("AD19UploadList", objDocumentList);
                return Json(objDocumentList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult DeleteAllopathicAddedDrugs(int index)
        {
            if (Session["AD19DrugsList"] != null)
            {
                List<AllopathicDrugName> DrugsList = Session.GetDataFromSession<List<AllopathicDrugName>>("AD19DrugsList");
                if (DrugsList[index].Id == 0)
                    DrugsList.RemoveAt(index);
                else
                    DrugsList[index].IsDeleted = true;
                Session.SetDataToSession<List<DocumentUploadModel>>("AD19DrugsList", DrugsList);
                return Json(DrugsList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        #region
        #region  Particulars of applicant
        public JsonResult SaveADForm19Applicant(ApplicantViewModel model)
        {
            NotificationModel notification = new NotificationModel();
            model.FileType = "";
            if (ModelState.ContainsKey("FileType"))
                ModelState["FileType"].Errors.Clear();
            if (ModelState.IsValid)
            {
                int _applicationId=0;
                int _transactionId=0;
                if (model.ServiceId == 36)
                {
                    _applicationId = Session["AD19ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19ApplicationId");
                    _transactionId = Session["AD19TransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19TransactionId");

                }
                else
                {
                    _applicationId = Session["AD19CApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CApplicationId");
                    _transactionId = Session["AD19CTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CTransactionId");

                }
                //int _applicantId = Session["AD19ApplicantId"] == null ? 0 : Session.GetDataFromSession<int>("AD19ApplicantId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;

                if (model.ApplicantUpload != "undefined")
                {
                    HttpPostedFileBase _uploadedFile = Request.Files[0];
                    var uploadsPath = Path.Combine("Allopathic", "Applicant");
                    string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadDocument = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                    #region Saving files physically
                    _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                    string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                    #endregion


                }



                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                applicationBAL = new ApplicationBAL();

                int result = applicationBAL.SaveADApplicantDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    if (model.ServiceId == 36)
                    {
                        Session.SetDataToSession<int>("AD19ApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19TransactionId", _transactionId);
                    }
                    else
                    {
                        Session.SetDataToSession<int>("AD19CApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19CTransactionId", _transactionId);
                    }
                   
                  //  Session.SetDataToSession<int>("AD19ApplicantId", _applicantId);

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
                // TODO: Return model validations       - siva, 07-09-2017
                return Json(notification);
            }
        }

        public JsonResult SaveADForm19Pharmacy(AllopathicPharmacyViewModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = 0;
                int _transactionId = 0;
                int _pharmacyId = 0;

                if (model.ServiceId == 36)
                {
                     _applicationId = Session["AD19ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19ApplicationId");
                   _transactionId = Session["AD19TransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19TransactionId");
                  _pharmacyId = Session["AD19PharmacyId"] == null ? 0 : Session.GetDataFromSession<int>("AD19PharmacyId");


                }
                else
                {
                    _applicationId = Session["AD19CApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CApplicationId");
                    _transactionId = Session["AD19CTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CTransactionId");
                    _pharmacyId = Session["AD19CPharmacyId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CPharmacyId");

                }
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                applicationBAL = new ApplicationBAL();

                int result = applicationBAL.SaveADPharmacyDetails(model, ref _applicationId, ref _transactionId, ref _pharmacyId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    if (model.ServiceId == 36)
                    {
                        Session.SetDataToSession<int>("AD19ApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19TransactionId", _transactionId);
                        Session.SetDataToSession<int>("AD19PharmacyId", _pharmacyId);
                    }
                    else
                    {
                        Session.SetDataToSession<int>("AD19CApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19CTransactionId", _transactionId);
                        Session.SetDataToSession<int>("AD19CPharmacyId", _pharmacyId);
                    }
                  

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Pharmacy details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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
                // TODO: Return model validations       - siva, 07-09-2017
                return Json(notification);
            }
        }
        public JsonResult SaveADForm19Competent(ApplicantViewModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.ContainsKey("model.UploadDocument"))
                ModelState["model.UploadDocument"].Errors.Clear();
            if (ModelState.ContainsKey("FileType"))
                ModelState["FileType"].Errors.Clear();
            int _DocumentsCount = 0;
            if (ModelState.IsValid)
            {

                int _applicationId = 0;
                int _transactionId = 0;
                int _competentId = 0;
                if (model.ServiceId == 36)
                {
                    _applicationId = Session["AD19ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19ApplicationId");
                    _transactionId = Session["AD19TransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19TransactionId");
                    _competentId = Session["AD19CompetentId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CompetentId");
                }else
                {
                    _applicationId = Session["AD19CApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CApplicationId");
                    _transactionId = Session["AD19CTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CTransactionId");
                    _competentId = Session["AD19CCompetentId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CCompetentId");

                }
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                applicationBAL = new ApplicationBAL();

                List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();

                if (model.ServiceId == 36)
                {
                    if (Session["AD19UploadList"] != null)
                        UploadList = Session["AD19UploadList"] as List<DocumentUploadModel>;
                    model.uploadedDocuments = UploadList;
                }
                else
                {
                    if (Session["AD19cUploadList"] != null)
                        UploadList = Session["AD19cUploadList"] as List<DocumentUploadModel>;
                    model.uploadedDocuments = UploadList;
                }

                if (UploadList.Count > 0)
                {
                    _DocumentsCount = UploadList.Count;
                }

                int result = applicationBAL.SaveADCompetentDetails(model, ref _applicationId, ref _transactionId, ref _competentId,
                    ref formStatus, ref _applicationNumber, _DocumentsCount);
                if (result > 0)
                {
                    if (model.ServiceId == 36)
                    {
                        Session.SetDataToSession<int>("AD19ApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19TransactionId", _transactionId);
                        Session.SetDataToSession<int>("AD19CompetentId", _competentId);
                    }
                    else
                    {
                        Session.SetDataToSession<int>("AD19CApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19CTransactionId", _transactionId);
                        Session.SetDataToSession<int>("AD19CCompetentId", _competentId);
                    }
                   

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
                // TODO: Return model validations       - siva, 07-09-2017
                return Json(notification);
            }
        }
        public JsonResult SaveADDrugDetails(int ServiceId)
        {
            NotificationModel notification = new NotificationModel();
            int _applicationId = 0;
            int _transactionId = 0;
            if (ServiceId == 36)
            {
                _applicationId = Session["AD19ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19ApplicationId");
                _transactionId = Session["AD19TransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19TransactionId");

            }
            else
            {
                _applicationId = Session["AD19CApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CApplicationId");
                _transactionId = Session["AD19CTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CTransactionId");

            }
            FormStatus formStatus = FormStatus.Empty;
            string _applicationNumber = string.Empty;


            int UserId = Session.GetDataFromSession<UserModel>("User").Id;

            applicationBAL = new ApplicationBAL();

            List<AllopathicDrugName> DrugsList = new List<AllopathicDrugName>();

            if (ServiceId == 36)
            {
                if (Session["AD19DrugsList"] != null)
                    DrugsList = Session["AD19DrugsList"] as List<AllopathicDrugName>;

            }
            else
            {
                if (Session["AD19cDrugsList"] != null)
                    DrugsList = Session["AD19cDrugsList"] as List<AllopathicDrugName>;

            }



            int result = applicationBAL.SaveADDrugDetails(DrugsList, ref _applicationId, ref _transactionId, ServiceId, UserId, ref formStatus, ref _applicationNumber);
            if (result > 0)
            {
                if (ServiceId == 36)
                {
                    Session.SetDataToSession<int>("AD19ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("AD19TransactionId", _transactionId);
                }
                else
                {
                    Session.SetDataToSession<int>("AD19CApplicationId", _applicationId);
                    Session.SetDataToSession<int>("AD19CTransactionId", _transactionId);
                }
                

                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Drug Details Saved.<br>Your application is <b>" + _applicationNumber + "</b>";
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

        public JsonResult SaveADDeclaration(AllopathicDeclaration model,int ServiceId)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
                int _applicationId = 0;
                int _transactionId = 0; 
                int _declarationId = 0;
                if (ServiceId == 36)
                {
                     _applicationId = Session["AD19ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19ApplicationId");
                   _transactionId = Session["AD19TransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19TransactionId");
                    _declarationId = Session["AD19DeclarationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19DeclarationId");

                }
                else
                {
                  _applicationId = Session["AD19CApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CApplicationId");
                   _transactionId = Session["AD19CTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CTransactionId");
                    _declarationId = Session["AD19CDeclarationId"] == null ? 0 : Session.GetDataFromSession<int>("AD19CDeclarationId");

                }
                 FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                applicationBAL = new ApplicationBAL();

                int result = applicationBAL.SaveADDeclaration(model, ref _applicationId, ref _transactionId, ref _declarationId,  ServiceId, ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    if (ServiceId == 36)
                    {
                        Session.SetDataToSession<int>("AD19ApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19TransactionId", _transactionId);
                        Session.SetDataToSession<int>("AD19DeclarationId", _declarationId);
                    }
                    else
                    {
                        Session.SetDataToSession<int>("AD19CApplicationId", _applicationId);
                        Session.SetDataToSession<int>("AD19CTransactionId", _transactionId);
                        Session.SetDataToSession<int>("AD19CDeclarationId", _declarationId);
                    }
                   

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
                // TODO: Return model validations       - siva, 08-09-2017
                return Json(notification);
            }
        }




        #endregion
        #endregion

        public JsonResult AddDrugs(string DrugName, string Type)
        {
            List<AllopathicDrugName> DrugsList = new List<AllopathicDrugName>();
            if (Type == "AD19Drug")
            {
                if (Session["AD19DrugsList"] != null)
                    DrugsList = Session["AD19DrugsList"] as List<AllopathicDrugName>;
                AllopathicDrugName Drug = new AllopathicDrugName();
                Drug.Name = DrugName;
                DrugsList.Add(Drug);
                Session["AD19DrugsList"] = DrugsList;
            }
            else
            {
                if (Session["AD19cDrugsList"] != null)
                    DrugsList = Session["AD19cDrugsList"] as List<AllopathicDrugName>;
                AllopathicDrugName Drug = new AllopathicDrugName();
                Drug.Name = DrugName;
                DrugsList.Add(Drug);
                Session["AD19cDrugsList"] = DrugsList;
            }
            return Json(DrugsList);
        }


        public PartialViewResult AllTAMCEformsCertificateDetails(int TransactionId=0, string TableName=null,int FormId=0)
        {
            TAMCEMultiFormsViewModel objModel = new TAMCEMultiFormsViewModel();
            //APMCEAckModel model = new APMCEAckModel();
            applicationBAL = new ApplicationBAL();
            //TransactionId = 1091;
            objModel= applicationBAL.GetAllTAMCEformsCertificateDetails(TransactionId, TableName);
            if(FormId == 1)
            return PartialView("_TAMCEForm1Certificate", objModel.tamceCertificate);//.tamceAckModel);
            else if (FormId == 2)
                return PartialView("_TAMCEForm2Certificate", objModel.tamceCertificate); //.tamceAckModel);
            else if (FormId == 3)
                return PartialView("_TAMCEForm3Certificate", objModel.tamceCertificate);
            else if (FormId == 4)
                return PartialView("_TAMCEForm4Certificate", objModel.tamceCertificate);
            else if (FormId == 5)
                return PartialView("_TAMCEForm5Certificate", objModel.tamceCertificate);
            else if (FormId == 6)
                return PartialView("_TAMCEForm6Certificate", objModel.tamceCertificate);
            else if (FormId == 7)
                return PartialView("_TAMCEForm7Certificate", objModel.tamceCertificate);
            else
                return PartialView("_TAMCEForm1Certificate", objModel.tamceAckModel);
        }

        public PartialViewResult GenerateTemparoryCertificate(int TransactionId, string TableName)
        {
            APMCECertificate objModel = new APMCECertificate();
            applicationBAL = new ApplicationBAL();
            objModel = applicationBAL.GetTemparoryCertificateDetails(TransactionId, TableName);
            return PartialView("_TAMCEForm3Certificate", objModel);
        }

        public PartialViewResult GenerateRejectCertificate(int TransactionId, string TableName)
        {
            APMCECertificate objModel = new APMCECertificate();
            applicationBAL = new ApplicationBAL();
            objModel = applicationBAL.GetRejectCertificateDetails(TransactionId, TableName);
            return PartialView("_TAMCEForm5Certificate", objModel);
        }

        public PartialViewResult GetAcknowledgeCertificate(int TransactionId, string TableName,bool IsMissingDocuments,string AllFileNames)
        {
            applicationBAL = new ApplicationBAL();
            AcknowledgeModel model = applicationBAL.GetAcknowledgeCertificateDetails(TransactionId, TableName);
            model.HasMissingDocuments = IsMissingDocuments;

            string[] strArrayOne = new string[] { "" };
            strArrayOne = AllFileNames.Split(',');
            model.ServiceDetails = new List<string>();
            foreach (string serivcerow in strArrayOne)
            {
                if(serivcerow.ToString() != "null")
                    model.ServiceDetails.Add(serivcerow.ToString());
            }

            return PartialView("_TAMCEForm2Certificate", model);
        }
    }

}