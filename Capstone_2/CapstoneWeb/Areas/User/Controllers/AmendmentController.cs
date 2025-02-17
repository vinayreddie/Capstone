using Capstone.BAL;
using Capstone.Framework;
using Capstone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Areas.User.Controllers
{
    [SessionTimeout]
    public class AmendmentController : Controller
    {
        // GET: User/Amendments
        public ActionResult Index()
        {
            return View();
        }
        // GET: User/AmendmentQuestionnaire

        public ActionResult Questionnaire(int id, int serviceId,string tableName)
        {
            AmendmentModel model = new AmendmentModel();
            model.TransactionId = id;
            model.Type = tableName;
            if (serviceId == 1 || serviceId==14 || serviceId==17) // APMCE Amendments add this two serviceId's 14 & 17   --kishore
                model.APMCEAmendments = new APMCEAmendmentModel();
            else if(serviceId == 2 || serviceId==26 || serviceId==27)  // PCPNDT appeal Amendments add this two serviceId's 26 & 27   --kishore
                model.PCPNDTAmendments = new PCPNDTAmendmentModel();
            return View(model);
        }
        //[HttpPost]
        //public ActionResult Questionnaire(AmendmentModel model)
        //{
        //    int SID = 2;
        //    ViewBag.serviceID = SID;
        //    TempData["AmendmentsModel"] = model;
        //    return RedirectToAction("Amendment"); 
        //}

        #region Get PCPNDT,APMCE Amendment Data 

        [HttpPost]
        public ActionResult Amendment(AmendmentModel model)
        {
            Session["NOCEquipmentList"] = null;

            if (model.APMCEAmendments != null)
            {
                LicenseBAL objBAL = new LicenseBAL();
                List<InfraStructureModel> InfraStructureList = new List<InfraStructureModel>();

                ViewBag.DistrictList = objBAL.GetCountries();
                ViewBag.OfferedServices = objBAL.GetOfferedServices();
                AmendmentBAL oblamendmentBAL = new AmendmentBAL();
                model.APMCEAmendments.APMCEModel = oblamendmentBAL.GetAPMCEData(model.TransactionId,model.Type);
                if(model.APMCEAmendments.APMCEModel !=null)
                {
                    Session["APMCETransactionId"] = model.APMCEAmendments.APMCEModel.TransactionId;
                    // Set file paths
                    Session["amtAccommodationUpload"] = model.APMCEAmendments.APMCEModel.Accommadation.UploadedFilePath;
                    Session["amtInfraStructureList"] = model.APMCEAmendments.APMCEModel.InfraStructureList;
                    Session["amtEstOpenArea"] = model.APMCEAmendments.APMCEModel.EstablishmentModel.OpenAreaFilePath;
                    Session["amtEstConstructionArea"] = model.APMCEAmendments.APMCEModel.EstablishmentModel.ConstructionAreaFilePath;
                    Session["amtFacilitiesAvailableDeclarationStampFilePath"] = model.APMCEAmendments.APMCEModel.FacilitiesAvailableModel.DeclarationStampFilePath;
                    Session["FacilitiesAvailableOtherInformationDocument"]= model.APMCEAmendments.APMCEModel.FacilitiesAvailableModel.OtherInformationDocumentPath;
                    Session["amtStaffUploadFilePath"] = model.APMCEAmendments.APMCEModel.StaffDetails.UploadedFilePath;
                    Session["amtStaffDetailsList"] = model.APMCEAmendments.APMCEModel.StaffDetailsList;
                }
            }


            if(model.PCPNDTAmendments != null)
            {
                AmendmentBAL oblamendmentBAL=new AmendmentBAL();
                LicenseBAL objBAL = new LicenseBAL();
                List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                objBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                ViewBag.OwnershipTypeList = ownershipTypeList;
                ViewBag.InstitutionTypeList = institutionTypeList;
                ViewBag.FacilityMaster = objBAL.GetFacilityList();
                TempData["DoctorSpecialityList"] = objBAL.GetDoctorSpecialityList();
                model.PCPNDTAmendments.PCPNDTModel = oblamendmentBAL.GetPCPNDTData(model.TransactionId,model.Type);
                if (model.PCPNDTAmendments.PCPNDTModel != null)
                {
                    Session["amtPCPNDTTransactionId"] = model.PCPNDTAmendments.PCPNDTModel.TransactionId;
                    Session["amtEquipmentsList"] = model.PCPNDTAmendments.PCPNDTModel.EquipmentList;
                    Session["amtEmployeeList"] = model.PCPNDTAmendments.PCPNDTModel.EmployeeList;
                    // Set file paths
                    Session["amtAddressProofPath"] = model.PCPNDTAmendments.PCPNDTModel.FacilityModel.AddressProofPath;
                    Session["amtBuildingLayoutPath"] = model.PCPNDTAmendments.PCPNDTModel.FacilityModel.BuildingLayoutPath;
                    Session["amtAffidavitDocPath"] = model.PCPNDTAmendments.PCPNDTModel.InstitutionModel.AffidavitDocPath;
                    Session["amtArticleFilePath"] = model.PCPNDTAmendments.PCPNDTModel.InstitutionModel.ArticleDocPath;
                    Session["amtStudyCertificates"] = model.PCPNDTAmendments.PCPNDTModel.InstitutionModel.StudyCertificateDocPaths;
                }
                if (model.PCPNDTAmendments.InstitutionAmendment == true)
                {
                    Session["InstitutionAmendment"] = 20;  //Institution Amendment  Used By Saving    --kishore 21-06-17
                }
                if (model.PCPNDTAmendments.OwnershipTypeAmendment == true)
                {
                    Session["OwnershipTypeAmendment"] = 19;  //OwnershipType Amendment  Used By Saving    --kishore 21-06-17
                }
                if (model.PCPNDTAmendments.ApplyforNOCofEquipment == true)
                {
                    
                    model.PCPNDTModel.NocforEquipmentModel.TransactionId = model.TransactionId;
                }
            }
            return View(model);
        }

        #endregion
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



        #region PCPNDT Amendment
        #region  Facility Details Saving for PCPNDT Amendment
        public ActionResult SaveFacilityAmendment2(FacilityViewModel facility, TestsModel tests, FacilitesModel facilities)
        {
            //facility.Faclities = "";
            //tests.InvasiveTests = "";
            //tests.NonInvasiveTests = "";
            //tests.Remarks = "";
            //facilities.Tests = "";
            //facilities.Studies = "";
            //facilities.Remarks = "";


            NotificationModel notification = new NotificationModel();
            try
            {

                int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
                // int _serviceId = 18; 
                facility.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                
                AmendmentBAL objPCPNDTBAL = new AmendmentBAL();
                int result = objPCPNDTBAL.SaveFacilityAmendment(facility, facilities, tests, ref _transactionId);

                if (result > 0)
                {

                    Session.SetDataToSession<int>("amtPCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facility details saved.";
                    // notification.ShowNonActionButton = true;
                    //notification.NonActionButtonClassType = PopupButtonClass.Success;
                    //notification.NonActionButtonText = "Okay";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go To License";
                    notification.ActionName = "Licenses";
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
                    notification.ReturnData = "0," + FormStatus.Empty;
                }

                return Json(notification);
            }
            catch
            {
                // TODO: Return model validations       - kishore, 15-06-2017
                return Json(notification);
            }


          
        }
        public ActionResult SaveFacilityAmendment(FacilityViewModel model, HttpPostedFileBase AddressProof, HttpPostedFileBase BuildingLayout)
        {
            NotificationModel notification = new NotificationModel();
            try
            {

                int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
               // int _serviceId = 18; 
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                var uploadsPath = Path.Combine("Applicant", "FacilityDetails");


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
                else if (Session["amtAddressProofPath"] != null)
                {
                    model.AddressProofPath = Session.GetDataFromSession<string>("amtAddressProofPath");
                }

                if (BuildingLayout != null)
                {
                    string _buildingLayoutPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(BuildingLayout.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.BuildingLayoutPath = _buildingLayoutPath + Path.GetExtension(BuildingLayout.FileName);

                    BuildingLayout.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath));
                    string layoutfilepath = Path.Combine(Server.MapPath("~/Uploads"), _buildingLayoutPath);
                    System.IO.File.Move(layoutfilepath, layoutfilepath + Path.GetExtension(BuildingLayout.FileName));
                }
                else if (Session["amtBuildingLayoutPath"] != null)
                {
                    model.AddressProofPath = Session.GetDataFromSession<string>("amtBuildingLayoutPath");
                }
                #endregion

                AmendmentBAL objPCPNDTBAL = new AmendmentBAL();
                int result = 0; // objPCPNDTBAL.SaveFacilityAmendment(model, ref _transactionId);

                if (result > 0)
                {

                    Session.SetDataToSession<int>("amtPCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facility details saved.";
                   // notification.ShowNonActionButton = true;
                    //notification.NonActionButtonClassType = PopupButtonClass.Success;
                    //notification.NonActionButtonText = "Okay";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go To License";
                    notification.ActionName = "Licenses";
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
                    notification.ReturnData = "0," + FormStatus.Empty;
                }

                return Json(notification);
            }
            catch
            {
                // TODO: Return model validations       - kishore, 15-06-2017
                return Json(notification);
            }
        }

        #endregion

        #region  Employee Details  Saving for PCPNDT Amendment
        public JsonResult CheckforEmployeeRegistration(string registrationNumber)
        {
            PCPNDTBAL obj = new PCPNDTBAL();
            var employeeRegistrations = obj.CheckforEmployeeRegistration(registrationNumber);
            return Json(new
            {
                EmployeeRegistrations = employeeRegistrations
            });
        }
        public JsonResult GetEmployeeUploads(int index)
        {
            var employees = Session.GetDataFromSession<List<EmployeeViewModel>>("amtEmployeeList")
                .Where(item => item.IsDeleted == false).ToList();
            var uploads = employees[index].UploadDocuments;
            return Json(new
            {
                EmployeeDescription = employees[index].Name + " - " + employees[index].RegistrationNumber,
                Uploads = uploads
            });
        }
        public JsonResult AddEmployeeAmendment(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                List<EmployeeViewModel> objEmployeeList;
                if (Session["amtEmployeeList"] != null)
                    objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("amtEmployeeList");
                // TempData["EmployeeList"] as List<EmployeeViewModel>;
                else
                    objEmployeeList = new List<EmployeeViewModel>();

                // Assign Employee Documents
                model.UploadDocuments = new List<DocumentUploadModel>();
                if (Session["EmployeeAmendmentUploadedDocuments"] != null)
                {
                    model.UploadDocuments = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeAmendmentUploadedDocuments");
                    Session["EmployeeAmendmentUploadedDocuments"] = null;
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
                    int userId = Session.GetDataFromSession<UserModel>("User").Id;

                    var uploadsPath = Path.Combine("Applicant", "Employee");

                    var _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    educationCertificate.DocumentPath = _uploadedFilePath + Path.GetExtension(uploadedFile.FileName);
                    educationCertificate.UploadType = "Education Certificate";
                    educationCertificate.UploadedUserId = userId;

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    #region Saving files physically
                    uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                    var oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(uploadedFile.FileName));

                    model.UploadDocuments.Add(educationCertificate);

                    #endregion
                }
                #endregion

                objEmployeeList.Add(model);
                Session.SetDataToSession<List<EmployeeViewModel>>("amtEmployeeList", objEmployeeList);
                //TempData["EmployeeList"] = objEmployeeList;

                return Json(objEmployeeList);
            }
            else
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteEmployeeAmendment(int index)   //using this one
        {
            if (Session["amtEmployeeList"] != null)
            {
                List<EmployeeViewModel> objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("amtEmployeeList")
                    .Where(item => item.IsDeleted == false).ToList();
                if (objEmployeeList[index].Id == 0)
                    objEmployeeList.RemoveAt(index);
                else
                    objEmployeeList[index].IsDeleted = true;
                Session.SetDataToSession<List<EmployeeViewModel>>("amtEmployeeList", objEmployeeList);
                return Json(objEmployeeList);
            }
            return Json(null);
        }
        public ActionResult SaveEmployeeDetails(EmployeeViewModel model) // using this one
        {
            NotificationModel notification = new NotificationModel();
            if (Session["amtEmployeeList"] != null)
            {
                List<EmployeeViewModel> objEmployeeList = Session.GetDataFromSession<List<EmployeeViewModel>>("amtEmployeeList");
                int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEmployeeList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                        e.UploadDocuments.ForEach(doc =>
                            {
                                doc.ReferenceTable = "t_employeelog";
                            });
                    });
                AmendmentBAL objBAL = new AmendmentBAL();
                int result = objBAL.SaveEmployeeDetails(objEmployeeList, _transactionId, _userId);

                if (result > 0)
                {
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = " Employee details saved.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go to license";
                    notification.ActionName = "Licenses";
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
                    notification.ReturnData = "0," + FormStatus.Empty;
                }
            }

            return Json(notification);
        }
        public JsonResult GetEmployees(int transactionId)
        {
            AmendmentBAL objBAL = new AmendmentBAL();
            List<EmployeeViewModel> employeeList = objBAL.GetEmployees(transactionId);
            Session.SetDataToSession<List<EmployeeViewModel>>("amtEmployeeList", employeeList);
            return Json(employeeList);
        }
        [HttpPost]
        public JsonResult UploadEmployeeDocument(string fileType, HttpPostedFileBase uploadedFile)
        {
            int _applicationId = Session["ApplicationId"] == null ? 0 : Session.GetDataFromSession<int>("ApplicationId");
            int _transactionId = Session["PCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("PCPNDTTransactionId");
            List<DocumentUploadModel> uploadedDocsList;
            if (Session["EmployeeAmendmentUploadedDocuments"] != null)
                uploadedDocsList = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeAmendmentUploadedDocuments");
            else
                uploadedDocsList = new List<DocumentUploadModel>();

            var user = Session.GetDataFromSession<UserModel>("User");

            DocumentUploadModel uploadedDocument = new DocumentUploadModel();

            var uploadsPath = Path.Combine("Applicant", "Employee");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            uploadedDocument.DocumentPath = _uploadedFilePath + Path.GetExtension(uploadedFile.FileName);
            uploadedDocument.UploadType = fileType;
            uploadedDocument.UploadedUserId = user.Id;

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

            #region Saving files physically
            uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(uploadedFile.FileName));

            uploadedDocsList.Add(uploadedDocument);
            #endregion

            Session["EmployeeAmendmentUploadedDocuments"] = uploadedDocsList;

            return Json(new
            {
                DocumentsList = uploadedDocsList
            });
        }
        public JsonResult DeleteEmployeeUpload(int index)
        {
            var uploadedDocsList = Session.GetDataFromSession<List<DocumentUploadModel>>("EmployeeAmendmentUploadedDocuments");
            uploadedDocsList.RemoveAt(index);
            Session["EmployeeAmendmentUploadedDocuments"] = uploadedDocsList;
            return Json(uploadedDocsList);
        }

        public JsonResult ClearEmployeeUploads()
        {
            Session["EmployeeAmendmentUploadedDocuments"] = null;
            return null;
        }
        #endregion 

        #region  Equipment  Details Saving for PCPNDT Amendment
        [HttpPost]
        public ActionResult AddEquipment(int Index, EquipmentModel model, HttpPostedFileBase NocFile, HttpPostedFileBase TransferCertificate, HttpPostedFileBase InstallationFile, HttpPostedFileBase Image,HttpPostedFileBase InvoiceFile)
        {
            if (model.Type == "New")
                ModelStateErrorHandler.RemoveError(ModelState, "Type");

            if (ModelState.IsValid)
            {
                List<EquipmentModel> objEquipmentsList;
                if (Session["amtEquipmentsList"] != null)
                    objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("amtEquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                else
                    objEquipmentsList = new List<EquipmentModel>();
                var uploadsPath = Path.Combine("Applicant", "Equipment");
                if (Index == -1) // New Equipment
                {                    
                    HttpPostedFileBase _uploadedFile = Request.Files[0];


                    int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
                    model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                    //string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    //model.UploadedFilePath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
                    string InstallationFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(InstallationFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string ImagePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(Image.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.InstallationCerticatePath = InstallationFilePath + Path.GetExtension(InstallationFile.FileName);
                    model.ImagePath = ImagePath + Path.GetExtension(Image.FileName);
                    InstallationFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), InstallationFilePath));
                    Image.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), ImagePath));
                    string oldInstallationPath = Path.Combine(Server.MapPath("~/Uploads"), InstallationFilePath);
                    System.IO.File.Move(oldInstallationPath, oldInstallationPath + Path.GetExtension(InstallationFile.FileName));

                    string oldImagePath = Path.Combine(Server.MapPath("~/Uploads"), ImagePath);
                    System.IO.File.Move(oldImagePath, oldImagePath + Path.GetExtension(Image.FileName));
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

                    //#region Saving files physically
                    //_uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                    //string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                    //System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                    //#endregion


                    objEquipmentsList.Add(model);
                    Session.SetDataToSession<List<EquipmentModel>>("amtEquipmentsList", objEquipmentsList);
                    //TempData["EquipmentsList"] = objEquipmentsList;

                    return Json(objEquipmentsList);
                }
                else // Existing Equipment in List .
                {
                    EquipmentModel TempEquipmentModel = Session.GetDataFromSession<List<EquipmentModel>>("amtEquipmentsList").ToList()[Index];
                    // Invoice File
                    if (InvoiceFile == null)
                        model.InvoicePath = TempEquipmentModel.InvoicePath;
                    else
                    {
                        string InvoiceFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(InvoiceFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.InvoicePath = InvoiceFilePath + Path.GetExtension(InvoiceFile.FileName);
                        InvoiceFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), InvoiceFilePath));
                        string oldInvoicePath = Path.Combine(Server.MapPath("~/Uploads"), InvoiceFilePath);
                        System.IO.File.Move(oldInvoicePath, oldInvoicePath + Path.GetExtension(InvoiceFile.FileName));
                    }
                    //TransferCertificate
                    if (TransferCertificate == null)
                        model.TransferCertificatePath = TempEquipmentModel.TransferCertificatePath;
                    else
                    {
                        string TransferFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(TransferCertificate.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.TransferCertificatePath = TransferFilePath + Path.GetExtension(TransferCertificate.FileName);
                        TransferCertificate.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), TransferFilePath));
                        string oldTransferPath = Path.Combine(Server.MapPath("~/Uploads"), TransferFilePath);
                        System.IO.File.Move(oldTransferPath, oldTransferPath + Path.GetExtension(TransferCertificate.FileName));
                    }
                    //Installation File
                    if (InstallationFile == null)
                        model.InstallationCerticatePath = TempEquipmentModel.InstallationCerticatePath;
                    else
                    {
                        string InstallationPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(InstallationFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.InstallationCerticatePath = InstallationPath + Path.GetExtension(InstallationFile.FileName);
                        InstallationFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), InstallationPath));
                        string oldInstallaionPath = Path.Combine(Server.MapPath("~/Uploads"), InstallationPath);
                        System.IO.File.Move(oldInstallaionPath, oldInstallaionPath + Path.GetExtension(InstallationFile.FileName));
                    }
                    //NOC File
                    if (NocFile == null)
                        model.NocFilePath = TempEquipmentModel.NocFilePath;
                    else
                    {
                        string NOCPath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(NocFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.NocFilePath = NOCPath + Path.GetExtension(NocFile.FileName);
                        NocFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), NOCPath));
                        string oldNOCPath = Path.Combine(Server.MapPath("~/Uploads"), NOCPath);
                        System.IO.File.Move(oldNOCPath, oldNOCPath + Path.GetExtension(NocFile.FileName));
                    }
                    //Image File
                    if (Image == null)
                        model.ImagePath = TempEquipmentModel.ImagePath;
                    else
                    {
                        string ImagePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(Image.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        model.ImagePath = ImagePath + Path.GetExtension(Image.FileName);
                        Image.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), ImagePath));
                        string oldImagePath = Path.Combine(Server.MapPath("~/Uploads"), ImagePath);
                        System.IO.File.Move(oldImagePath, oldImagePath + Path.GetExtension(Image.FileName));
                    }
                    objEquipmentsList.RemoveAt(Index);
                    objEquipmentsList.Insert (Index, model);
                    Session.SetDataToSession<List<EquipmentModel>>("amtEquipmentsList", objEquipmentsList);
                    //TempData["EquipmentsList"] = objEquipmentsList;

                    return Json(objEquipmentsList);
                }   
                
            }
            else
            {
                return Json("Invalid data");
            }


        }
        public JsonResult EditEquipment(int index)
        {
              List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("amtEquipmentsList");
              EquipmentModel objEquipment = objEquipmentsList[index];
            return Json(objEquipment);
           
        }
        public JsonResult DeleteEquipment(int index)
        {
            if (Session["amtEquipmentsList"] != null)
            {
                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("amtEquipmentsList");
                //TempData["EquipmentsList"] as List<EquipmentModel>;
                if (objEquipmentsList[index].Id == 0)
                    objEquipmentsList.RemoveAt(index);
                else
                    objEquipmentsList[index].IsDeleted = true;
                Session.SetDataToSession<List<EquipmentModel>>("amtEquipmentsList", objEquipmentsList);
                return Json(objEquipmentsList);
            }
            return Json(null);
        }
        public ActionResult SaveEquipments()
        {
            NotificationModel notification = new NotificationModel();
            if (Session["amtEquipmentsList"] != null)
            {
                int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");

                List<EquipmentModel> objEquipmentsList = Session.GetDataFromSession<List<EquipmentModel>>("amtEquipmentsList");
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objEquipmentsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                AmendmentBAL objbal = new AmendmentBAL();
                int result = objbal.SaveEquipments(objEquipmentsList, _transactionId, _userId);

                if (result > 0)
                {
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipment details saved.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go to license";
                    notification.ActionName = "Licenses";
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
        public JsonResult GetEquipments(int transactionId)
        {
            AmendmentBAL objBAL = new AmendmentBAL();
            List<EquipmentModel> equipmentList = objBAL.GetEquipments(transactionId);
            Session.SetDataToSession<List<EquipmentModel>>("EquipmentsList", equipmentList);
            return Json(equipmentList);
        }

        #endregion

        #region  Institution Details Saving for PCPNDT Amendment
        [HttpPost]
        public JsonResult SaveInstitutionAmendment(InstitutionModel model, HttpPostedFileBase affidavitFile,
             HttpPostedFileBase articleFile, HttpPostedFileBase[] studyCertificateFiles)
        {

            NotificationModel notification = new NotificationModel();
            int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
          
          
            AmendmentBAL objamendmentbal = new AmendmentBAL();
         
            if (Session["InstitutionAmendment"]!=null)
            {
              //  int institutionid = (int)Session["InstitutionAmendment"];
                int result = objamendmentbal.SaveInstitutionDetails(model, ref _transactionId);
                if (result > 0)
                {

                    Session.SetDataToSession<int>("amtPCPNDTTransactionId", _transactionId);
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Your Amendment is saved Successfully.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go to license";
                    notification.ActionName = "Licenses";
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
                    notification.ReturnData = "0," + FormStatus.Empty;
                }

                //clear Seassion Data 
                Session["InstitutionAmendment"] = null;
               
              // Json(notification);
            }
            if (Session["OwnershipTypeAmendment"] != null)
            {

                #region File Saving

                var uploadsPath = Path.Combine("Applicant", "OwnershipDetails");
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
                    else if (Session["amtAffidavitDocPath"] != null)
                    {
                        model.AffidavitDocPath = Session.GetDataFromSession<string>("amtAffidavitDocPath");
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
                        if (Session["amtStudyCertificates"] != null)
                            model.StudyCertificateDocPaths.AddRange(Session.GetDataFromSession<List<DocumentUploadModel>>("amtStudyCertificates"));
                    }
                    else
                    {
                        model.StudyCertificateDocPaths = Session.GetDataFromSession<List<DocumentUploadModel>>("amtStudyCertificates");
                    }
                    if(model.StudyCertificateDocPaths != null && model.StudyCertificateDocPaths.Count > 0)
                    {
                        model.StudyCertificateDocPaths
                            .ForEach(item => {
                                item.ReferenceTable = "t_institution";
                            });
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
                        //System.IO.File.Move(articlefilepath, articlefilepath + Path.GetExtension(articleFile.FileName));
                    }
                    else if (Session["amtArticleFilePath"] != null)
                    {
                        model.ArticleDocPath = Session.GetDataFromSession<string>("amtArticleFilePath");
                    }
                }

                #endregion
                int result = objamendmentbal.SaveOwnershipDetails(model, ref _transactionId);
                if (result > 0)
                {

                    Session.SetDataToSession<int>("amtPCPNDTTransactionId", _transactionId);
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Your Amendment is saved Successfully.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go to license";
                    notification.ActionName = "Licenses";
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
                    notification.ReturnData = "0," + FormStatus.Empty;
                }
                //return Json(notification);
            }
            //clear Seassion Data 
           
            Session["OwnershipTypeAmendment"] = null;
            return Json(notification,JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteStudyCertificate(int id)
        {
            var studyCertificates = Session.GetDataFromSession<List<DocumentUploadModel>>("amtStudyCertificates");
            studyCertificates.Where(item => item.Id == id).First().IsDeleted = true;
            studyCertificates.Where(item => item.Id == id).First().
                LastModifiedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            Session.SetDataToSession<List<DocumentUploadModel>>("amtStudyCertificates", studyCertificates);

            return Json(studyCertificates.Where(item => item.IsDeleted == false).ToList());
        }
        #endregion

        #region Tests Details Saving for PCPNDT Amendment
        public JsonResult SaveTestsAmendment(TestsModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {

                int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                AmendmentBAL objAmendmentBAL = new AmendmentBAL();

                int result = objAmendmentBAL.SaveTestsAmendment(model, ref _transactionId);

                if (result > 0)
                {

                    Session.SetDataToSession<int>("amtPCPNDTTransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Test details saved.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go to license";
                    notification.ActionName = "Licenses";
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

        #region Facilities Details Saving for PCPNDT Amendment 
        public JsonResult SaveFacilitiesamendment(FacilitesModel model)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {

                int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                AmendmentBAL objAmendmentBAL = new AmendmentBAL();
                int result = objAmendmentBAL.SaveFacilitiesAmendment(model, ref _transactionId);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("amtPCPNDTTransactionId", _transactionId);
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facilities details saved.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go to license";
                    notification.ActionName = "Licenses";
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
   
        #region License Data Search and  Cancel
        public ActionResult SearchLicenseByLicenseNo(CancelLicenseModel model)
        {
            NotificationModel notification = new NotificationModel();
          //  CancelLicenseModel objlicensecancel = new CancelLicenseModel();
            AmendmentBAL objAmendmentBAL = new AmendmentBAL();
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            //   int licenseno = 12345;   //Static license number by testing purpose    --kishore 23-06-17
            model = objAmendmentBAL.GetLicenseSearch(model);
            if(model != null)
            {

                //notification.Title = "Success";
                //notification.NotificationType = NotificationType.Success;
                //notification.NotificationMessage = "Check Your License Details.";
                //notification.ShowNonActionButton = true;
                //notification.NonActionButtonClassType = PopupButtonClass.Success;
                //notification.NonActionButtonText = "Okay";
                return Json(model);
            }
            else
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "License Number Does Not Exist.";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Okay";
                return Json(notification);
            }
           // return View(notification);
        }  

        public ActionResult PCPNDTLicenseCancelAmendment(CancelLicenseModel model)
        {
            NotificationModel notification = new NotificationModel();
            //UserModel model = new UserModel();
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            int _transactionId = Session["amtPCPNDTTransactionId"] == null ? 0 : Session.GetDataFromSession<int>("amtPCPNDTTransactionId");
            AmendmentBAL objBAL = new AmendmentBAL();
            int result = objBAL.PCPNDTLicenseCancel(model, _transactionId);
            if (result > 0)
            {
                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Your License has been canceled.";
                notification.ShowActionButton = true;
                notification.ActionButtonText = "Go to license";
                notification.ActionName = "Licenses";
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

        public JsonResult CheckForAmendment(int transactionId)
        {
            AmendmentBAL objAmendmentBAL = new AmendmentBAL();
            List<string> serviceList = objAmendmentBAL.CheckForAmendment(transactionId);
            return Json(serviceList);
        }

        #region NOC for Equipment amendment
        public string GetEquipmentList(int TransactionId)
        {
            LicenseBAL objBal = new LicenseBAL();
            DataTable dt = objBal.GetEquipmentList(TransactionId);
            return JsonConvert.SerializeObject(dt);
        }
        public JsonResult AddEquipments(NOCforquipmentModel model)
        {
            List<NOCforquipmentModel> objEquipmentList = new List<NOCforquipmentModel>();
            if (Session["NOCEquipmentList"] != null)
                objEquipmentList = Session["NOCEquipmentList"] as List<NOCforquipmentModel>;
            objEquipmentList.Add(model);
            Session["NOCEquipmentList"] = objEquipmentList;
            return Json(objEquipmentList);
        }
        public JsonResult DeleteEquipmentNOC(int Index)
        {
            List<NOCforquipmentModel> objEquipmentList = new List<NOCforquipmentModel>();
            if (Session["NOCEquipmentList"] != null)
                objEquipmentList = Session["NOCEquipmentList"] as List<NOCforquipmentModel>;
            objEquipmentList.RemoveAt(Index);
            Session["NOCEquipmentList"] = objEquipmentList;
            return Json(objEquipmentList);
        }
        public JsonResult SaveEquipmentNOC()
        {
            NotificationModel notification = new NotificationModel();
            LicenseBAL objBal = new LicenseBAL();
            List<NOCforquipmentModel> NOCEquipmentList = Session["NOCEquipmentList"] as List<NOCforquipmentModel>;
            NOCEquipmentList.ForEach(item => item.UserId = Session.GetDataFromSession<UserModel>("User").Id);

            bool Result = objBal.SaveEquipmentNOC(NOCEquipmentList);
            if (Result)
            {
                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "NOC Application for Equipment details saved.";
                notification.ShowActionButton = true;
                notification.ActionButtonText = "Go to license";
                notification.ActionName = "Licenses";
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
                notification.ReturnData = "0," + FormStatus.Empty;
            }
            return Json(notification);
        }
       
        public string GetAmmendments()
        {
            LicenseBAL objBal = new LicenseBAL();
            int _userId = Session.GetDataFromSession<UserModel>("User").Id;
            DataTable dt = objBal.GetAmmendments(_userId);
            return JsonConvert.SerializeObject(dt);
        }

      

        public PartialViewResult GetNOCCertificate(int AmendmentId)
        {
            LicenseBAL objBal = new LicenseBAL();
            NOCforquipmentModel objNocCertificate = objBal.GetNOCCertificateData(AmendmentId);
                return PartialView("_NOCCertificate", objNocCertificate);
           
        }
        #endregion
        #endregion

        #region APMCE Amendments

        #region Save Registration Details
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
                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                //APMCEBAL objAPMCEBAL = new APMCEBAL();
                int result = objAPMCEBAL.SaveRegistrationDetails(model, ref _applicationId, ref _transactionId,
                    ref formStatus, ref _applicationNumber);
                if (result > 0)
                {
                    Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    Session.SetDataToSession<int>("APMCETransactionId", _transactionId);
                    Session.SetDataToSession<int>("DistrictId", model.DistrictId);  // this will be used to get account id to make payment  - Raj K, 2021-01-01

                    //notification.Title = "Success";
                    //notification.NotificationType = NotificationType.Success;
                    //notification.NotificationMessage = "Registration details saved.<br>Your application is <b>" + _applicationNumber + "</b>";
                    //notification.ShowNonActionButton = true;
                    //notification.NonActionButtonClassType = PopupButtonClass.Success;
                    //notification.NonActionButtonText = "Okay";
                    //notification.ReturnData = result.ToString() + "," + formStatus;
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Amendment- Registration details Saved successfully.";
                    //notification.ShowActionButton = true;
                    //notification.ActionButtonText = "Go To License";
                    //notification.ActionName = "Licenses";
                    //notification.ControllerName = "Dashboard";
                    //notification.AreaName = "User";
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
        #endregion 

        #region Save Corresponding Address Details
        public JsonResult SaveCorrespondingAddressDetails(CorrespondingAddressModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
               
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                AmendmentBAL objAPMCEBAL = new AmendmentBAL();

                int result = objAPMCEBAL.SaveCorrespondingAddressDetails(model, ref _transactionId);
                if (result > 0)
                {
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Corresponding details amendment saved successfully.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go To License";
                    notification.ActionName = "Licenses";
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
            else
            {
                // TODO: Return model validations       - kishore, 17-07-2017
                return Json(notification);
            }
        }
        #endregion 

        #region Save Accommoduation Details
        public JsonResult SaveAccommodationDetails(AccommodationModel model, HttpPostedFileBase uploadedFile)
        {
            NotificationModel notification = new NotificationModel();

            if (ModelState.IsValid)
            {
              
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                int _serviceId = 1;   //static id passed --kishore
                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region File Saving
                var uploadsPath = Path.Combine("Applicant", model.UserId.ToString()
                    , _serviceId.ToString(), "AccommodationDetails");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                if (uploadedFile != null)
                {
                    string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadedFilePath = _uploadedfilePath + Path.GetExtension(uploadedFile.FileName);

                    uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(uploadedFile.FileName));
                }
                else if (Session["amtAccommodationUpload"] != null)
                {
                    model.UploadedFilePath = Session.GetDataFromSession<string>("amtAccommodationUpload");
                }

                #endregion

                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                int result = objAPMCEBAL.SaveAccommodationDetails(model,  ref _transactionId);

                if (result > 0)
                {
                  

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Accommodation details amendment saved successfully.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go To License";
                    notification.ActionName = "Licenses";
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
            else
            {
                // TODO: Return model validations       
                return Json(notification);
            }
        }
        #endregion

        #region Save Trust Details 
        public JsonResult SaveTrustDetails(TrustModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
               

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                int result = objAPMCEBAL.SaveTrustDetails(model, ref _transactionId);
                  
                if (result > 0)
                {
                    

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Trust details saved.";
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
            }
            return null;
        }
        #endregion

        #region Save Infrastructure Details Amendment
        public JsonResult AddInfraStructure(InfraStructureModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase _uploadedFile = Request.Files[0];

              
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");

                int _serviceId = 1;  // TODO: Set this value from m_service table        - Raj, 01-06-2017
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region Saving files physically
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId.ToString(), "InfraStructure");

                string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                model.UploadedFilePath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                #endregion

                List<InfraStructureModel> objInfraStructureList;
                if (Session["amtInfraStructureList"] != null)
                    objInfraStructureList = Session.GetDataFromSession<List<InfraStructureModel>>("amtInfraStructureList");
                else
                    objInfraStructureList = new List<InfraStructureModel>();
                objInfraStructureList.Add(model);
                Session.SetDataToSession<List<InfraStructureModel>>("amtInfraStructureList", objInfraStructureList);

                return Json(objInfraStructureList);
            }
            else
            {
                return Json("Invalid data");
            }
        }
        public JsonResult DeleteInfraStructureAmendment(int index)
        {
            if (Session["amtInfraStructureList"] != null)
            {
                List<InfraStructureModel> objInfraStructureList = Session.GetDataFromSession<List<InfraStructureModel>>("amtInfraStructureList");
                if (objInfraStructureList[index].Id == 0)
                    objInfraStructureList.RemoveAt(index);
                else
                    objInfraStructureList[index].IsDeleted = true;
                Session.SetDataToSession<List<InfraStructureModel>>("amtInfraStructureList", objInfraStructureList);
                //TempData["EmployeeList"] = objEmployeeList;
                return Json(objInfraStructureList);//.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }
        public JsonResult SaveInfraStructures()
        {
            NotificationModel notification = new NotificationModel();
            if (Session["amtInfraStructureList"] != null)
            {
                List<InfraStructureModel> objInfraStructureList = Session.GetDataFromSession<List<InfraStructureModel>>("amtInfraStructureList");
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objInfraStructureList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });
                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                int result = objAPMCEBAL.SaveInfraStructure(objInfraStructureList,  ref _transactionId);
                if (result > 0)
                {
                    
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Equipments & Furniture details saved.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go To License";
                    notification.ActionName = "Licenses";
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
                    notification.ReturnData = "0," + FormStatus.Empty;
                }
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
        #endregion

        #region Save Service Offered by Details 
        public JsonResult SaveServicesOfferedDetails(OfferedServicesModel model)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                AmendmentBAL objAPMCEBAL = new AmendmentBAL();

                int result = objAPMCEBAL.SaveServicesOfferedDetails(model,  ref _transactionId);

                if (result > 0)
                {
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Offered Services details amendment saved successfully.";
                    //notification.ShowActionButton = true;
                    //notification.ActionButtonText = "Go To License";
                    //notification.ActionName = "Licenses";
                    //notification.ControllerName = "Dashboard";
                    //notification.AreaName = "User";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "Okay";
                    notification.ReturnData = result.ToString() + "," +FormStatus.Empty;
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

        #region Save Facility Available Details 
        public JsonResult SaveFacilitiesAvailable(FacilitiesAvailableModel model, HttpPostedFileBase DeclarationStampFile, HttpPostedFileBase OtherInformationDocument)
        {
            NotificationModel notification = new NotificationModel();

            if (true) //if(ModelState.IsValid)
            {
               
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                FormStatus formStatus = model.FormStatus;
                string _applicationNumber = string.Empty;
                int _serviceId = 7;  // TODO: Set this value from m_service table        
                model.UserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region Files Saving

                var uploadsPath = Path.Combine("Applicant", model.UserId.ToString()
                    , _serviceId.ToString(), "FacilitiesAvailable");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                #region DeclarationStamp File saving

                if (DeclarationStampFile != null)
                {
                    string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(DeclarationStampFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
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
                    string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(OtherInformationDocument.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
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

                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                int result = objAPMCEBAL.SaveFacilitiesAvailable(model,  ref _transactionId);
                if (result > 0)
                {
                  

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Facilities Available details saved.";
                    notification.ShowActionButton = true;
                    notification.ActionButtonText = "Go To License";
                    notification.ActionName = "Licenses";
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

        #region Save Staff Details

        public JsonResult AddStaffDetails(StaffDetailsModel model)
        {
            // if (ModelState.IsValid)
            if (true)
            {
                HttpPostedFileBase _uploadedFile = Request.Files[0];
                
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");

                int _serviceId = 1;  // TODO: Set this value from m_service table        
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                #region Saving files physically
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId.ToString(), "StaffDetails");

                //string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string _uploadedFilePath = uploadsPath + "/" + model.StaffDesignation.Replace(" ","") + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                model.UploadedFilePath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

                string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
                System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
                #endregion

                List<StaffDetailsModel> objStaffDetailsList;
                if (Session["amtStaffDetailsList"] != null)
                    objStaffDetailsList = Session.GetDataFromSession<List<StaffDetailsModel>>("amtStaffDetailsList");
                else
                    objStaffDetailsList = new List<StaffDetailsModel>();
                objStaffDetailsList.Add(model);
                Session.SetDataToSession<List<StaffDetailsModel>>("amtStaffDetailsList", objStaffDetailsList);

                return Json(objStaffDetailsList);
            }
            else
            {
                return Json("Invalid data");
            }
        }
        
        public JsonResult DeleteStaffDetailsAmendment(int index)
        {
            if (Session["amtStaffDetailsList"] != null)
            {
                List<StaffDetailsModel> objStaffDetailsList = Session.GetDataFromSession<List<StaffDetailsModel>>("amtStaffDetailsList");
                if (objStaffDetailsList[index].Id == 0)
                    objStaffDetailsList.RemoveAt(index);
                else
                    objStaffDetailsList[index].IsDeleted = true;
                Session.SetDataToSession<List<InfraStructureModel>>("amtStaffDetailsList", objStaffDetailsList);
                return Json(objStaffDetailsList);
            }
            return Json(null);
        }

        public JsonResult SaveStaffDetailsOLD(StaffDetailsModel model, HttpPostedFileBase uploadedFile)
        {
            NotificationModel notification = new NotificationModel();
          
            if (true)
            {
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
                int _serviceId = 6;
                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                #region Files Saving
                var uploadsPath = Path.Combine("Applicant", model.CreatedUserId.ToString()
                    , _serviceId.ToString(), "StaffDetails");

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                if (uploadedFile != null)
                {
                    string _uploadedfilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    model.UploadedFilePath = _uploadedfilePath + Path.GetExtension(uploadedFile.FileName);
                    uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath));
                    string uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                    System.IO.File.Move(uploadedfilepath, uploadedfilepath + Path.GetExtension(uploadedFile.FileName));
                }
                else if (Session["amtStaffUploadFilePath"] != null)
                {
                    model.UploadedFilePath = Session.GetDataFromSession<string>("amtStaffUploadFilePath");
                }
                #endregion
                int result = objAPMCEBAL.SaveStaffDetailsOLD(model,  ref _transactionId);
                if (result > 0)
                {
                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Staff details saved.";
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
            }
            else
            {
                // TODO: Return model validations       
                return Json(notification);
            }
        }

        public JsonResult SaveStaffDetails()
        {
            NotificationModel notification = new NotificationModel();
            if (Session["amtStaffDetailsList"] != null)
            {
                List<StaffDetailsModel> objStaffDetailsList = Session.GetDataFromSession<List<StaffDetailsModel>>("amtStaffDetailsList");                
                int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
               

                int _userId = Session.GetDataFromSession<UserModel>("User").Id;
                objStaffDetailsList
                    .ForEach(e =>
                    {
                        e.CreatedUserId = _userId;
                    });


                AmendmentBAL objAPMCEBAL = new AmendmentBAL();
                int result = objAPMCEBAL.SaveStaffDetails(objStaffDetailsList, ref _transactionId);

                if (result > 0)
                {
                    //Session.SetDataToSession<int>("ApplicationId", _applicationId);
                    //Session.SetDataToSession<int>("APMCETransactionId", _transactionId);

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = "Staff details amendment saved successfully <b>";
                    //notification.ShowActionButton = true;
                    //notification.ActionButtonText = "Go To License";
                    //notification.ActionName = "Licenses";
                    //notification.ControllerName = "Dashboard";
                    //notification.AreaName = "User";
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

        #endregion

        #region License Data Search and  Cancel
        public ActionResult SearchAPMCELicenseByLicenseNo(CancelLicenseModel model)
        {
            NotificationModel notification = new NotificationModel();
            //  CancelLicenseModel objlicensecancel = new CancelLicenseModel();
            AmendmentBAL objAmendmentBAL = new AmendmentBAL();
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            //   int licenseno = 12345;   //Static license number by testing purpose    --kishore 23-06-17
            model = objAmendmentBAL.GetLicenseSearch(model);
            if (model != null)
            {

                //notification.Title = "Success";
                //notification.NotificationType = NotificationType.Success;
                //notification.NotificationMessage = "Check Your License Details.";
                //notification.ShowNonActionButton = true;
                //notification.NonActionButtonClassType = PopupButtonClass.Success;
                //notification.NonActionButtonText = "Okay";
                return Json(model);
            }
            else
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "License Number Does Not Exist.";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Okay";
                return Json(notification);
            }
            // return View(notification);
        }

        public ActionResult APMCELicenseCancelAmendment(CancelLicenseModel model)
        {
            NotificationModel notification = new NotificationModel();
            //UserModel model = new UserModel();
            model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;
            int _transactionId = Session["APMCETransactionId"] == null ? 0 : Session.GetDataFromSession<int>("APMCETransactionId");
            AmendmentBAL objBAL = new AmendmentBAL();
            int result = objBAL.PCPNDTLicenseCancel(model, _transactionId);
            if (result > 0)
            {
                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Your License has been canceled.";
                notification.ShowActionButton = true;
                notification.ActionButtonText = "Go to license";
                notification.ActionName = "Licenses";
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

        #endregion

       
    }
}