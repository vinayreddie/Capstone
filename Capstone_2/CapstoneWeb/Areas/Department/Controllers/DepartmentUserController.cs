using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.BAL;
using Capstone.Models;
using System.IO;
using Capstone.Framework;
using HiQPdf;
using System.Data;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Drawing;
using System.Web.UI;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
//using System.IO.Compression.FileSystem;
using Ionic.Zip;
using System.Globalization;
using System.Net;

namespace CapstoneWeb.Areas.Department.Controllers
{
    [SessionTimeout]
    public class DepartmentUserController : Controller
    {
        // GET: Department/DepartmentUser
        DepartmentUserBAL objBAL;
        public ActionResult ListofApplications(string Type)
        {
            objBAL = new DepartmentUserBAL();
            if (Type != null)
                Session["Type"] = Type;
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            List<TransactionViewModel> ApplicationList = objBAL.GetListofApplications(user.DesignationId, user.DistrictId, user.MandalId, user.VillageId, Session["Type"].ToString(), user.Id);
            return View(ApplicationList);
        }

        public ActionResult ListofForwardedApplications(string Type)
        {
            objBAL = new DepartmentUserBAL();
            if (Type != null)
                Session["Type"] = Type;
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            List<TransactionViewModel> ApplicationList = objBAL.GetListofApplications(user.DesignationId, user.DistrictId, user.MandalId, user.VillageId, Session["Type"].ToString(), user.Id);
            return View(ApplicationList);
        }

        //public JsonResult GetApplications()
        //{
        //    objBAL = new DepartmentUserBAL();
        //    UserModel user = Session.GetDataFromSession<UserModel>("User");
        //    return Json(objBAL.GetListofApplications(user.DesignationId,user.DistrictId, user.MandalId,user.VillageId), JsonRequestBehavior.AllowGet);//TODO : change this after user managment
        //}
        public ActionResult Approval(int TransactionId, int ServiceId, int AId, int TSId)
        {
            Session["QueryRaisedList"] = null;
            //Clear sessions;
            Session["APMCETransactionId"] = TransactionId;
            if (AId > 0)
            {
                return RedirectToAction("AmendmentApproval", new { TId = TransactionId, SId = ServiceId, AId = AId, TSId = TSId });
            }
            else
            {
                Session["UploadList"] = null;
                objBAL = new DepartmentUserBAL();
                LicenseBAL LBAL = new LicenseBAL();
                UserModel user = Session.GetDataFromSession<UserModel>("User");
                ApprovalComplexViewModel Approval = objBAL.ApprovalSceenOnloadData(TransactionId, user.DesignationId, ServiceId, user.Id); // //TODO : change transactionid,designationid (get this login user session)            
                Approval.Approval = new ApprovalsModel();
                Approval.Approval.TransactionId = TransactionId;
                Approval.TranServiceId = ServiceId;
                if (ServiceId == 2 || ServiceId == 29 || ServiceId == 30)
                {//PCPNDT ,Resubmit
                    Approval.PCPNDTModel = LBAL.GetPCPNDTData(TransactionId, "Transaction");
                    Session["EmployeeListLog"] = Approval.PCPNDTModel.EmployeeList;
                }
                else if (ServiceId == 1)//APMCE
                    Approval.APMCEModel = LBAL.GetAPMCEData(TransactionId, "Transaction");
                else if (ServiceId == 31)//Blood bank Form 27C
                    Approval.BloodBankModel = LBAL.GetBloodBankData(TransactionId);
                else if (ServiceId == 32) // BloodBank Form 27E
                {
                    Approval.BloodBankForm27EModel = LBAL.GetForm27EBloodBankData(TransactionId);
                }
                return View(Approval);
            }
        }

        public PartialViewResult LicensePreview(int transactionId)
        {
            string Type = "Transaction"; //Passing  static type   --kishore 17-10-2018
            LicenseBAL licenceBAL = new LicenseBAL();
            DataTable dtItems = licenceBAL.GetLicenseType(transactionId, Type);
            LicenseViewModel model = new LicenseViewModel();
            if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
            {
                model.APMCECertificate = licenceBAL.GetAPMCECertificate(transactionId, Type);
                return PartialView("_APMCETemporaryCertificate", model);
            }
            else if (dtItems.Rows[0]["ActType"].ToString() == "PCPNDT")
            {

                model.PCPNDTCertificate = licenceBAL.GetPCPNDTLicenseDetails(transactionId, Type);
                //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                //PCPNDTmodel = model.PCPNDTCertificate;
                return PartialView("_PCPNDTLicense", model.PCPNDTCertificate);
            }
            else
            {
                return null;
            }
        }

        public ActionResult InspectionApproval(ApprovalsModel approval, string Submit, List<InspectionModel> InspectionList, string serializedString, string HasInspectionPrivilege) //
        {
            LicenseBAL licenceBAL = new LicenseBAL();
            string Type = "Transaction"; 
            List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();            
            string Path = "";
            objBAL = new DepartmentUserBAL();
            //LicenseBAL licenseBAL = new LicenseBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            #region Check APMCE or PCPNDT
            
            DataTable dtItems = licenceBAL.GetLicenseType(approval.TransactionId, Type);
            LicenseViewModel model = new LicenseViewModel();
            string ApplicationType = dtItems.Rows[0]["ActType"].ToString();
            #endregion

            #region InspectionReport
            if (Submit == "Forward")
            {
                approval.status = Status.Forward;
                if (ApplicationType != "PCPNDT")
                {
                    if (Session["UploadList"] != null)
                        UploadList = Session["UploadList"] as List<DocumentUploadModel>;

                    InspectionList = null;
                }
                else if (ApplicationType == "PCPNDT")
                {
                    if (HasInspectionPrivilege == "True")
                    {
                        Path = GeneratePDF(serializedString);
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
            }
            else if (Submit == "Return")
            {
                approval.status = Status.Return;
            }
            else if (Submit == "Approve")
            {
                // Added On 01-07-2021 by Chandu
                approval.status = Status.Approved;
                if (ApplicationType != "PCPNDT" || ApplicationType == "APMCE")
                {
                    if (Session["UploadList"] != null)
                        UploadList = Session["UploadList"] as List<DocumentUploadModel>;
                     InspectionList = null;
                }
            }
            else if (Submit == "Reject")
            {
                approval.status = Status.Rejected;
            }
            #endregion

            approval.UserId = user.Id;


            bool Result = objBAL.SaveApprovals(approval, user.DesignationId, InspectionList, UploadList, approval.TransactionId, Path);

            if (approval.status == Status.Approved)
            {

                if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
                {
                    model.APMCECertificate = licenceBAL.GetAPMCECertificate(approval.TransactionId, Type);

                    #region File Download by Chandu 25-06-2021 From HiQPdf (HtmlToPdf) Evaluation
                    int IsCertificateSavedInFolder = licenceBAL.CheckIsCertificateSavedInFolder(approval.TransactionId);
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
                        var uploadsPath = System.IO.Path.Combine("Certificates", approval.TransactionId.ToString());

                        if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                            Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                        string _uploadedfilePath = uploadsPath + "/TAMCECertificate_" + approval.TransactionId.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        // send the PDF file to browser
                        FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
                        fileResult.FileDownloadName = _uploadedfilePath + ".pdf"; //"APMCECertificate.pdf";
                        var path = System.IO.Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);
                        var uploadedfilepath = System.IO.Path.Combine(Server.MapPath("~/Uploads"), _uploadedfilePath);
                        ToFile(fileResult, path);

                        // Save filepath in t_transaction table based on TransactionId
                        bool UpdateCertificateDownloadStatus = licenceBAL.SaveUpdateCertificatePath(approval.TransactionId, fileResult.FileDownloadName, user.Id);

                        string usermailId = "";
                        DataTable dtusermailId = licenceBAL.GetUserMailId(approval.TransactionId);

                        usermailId = dtusermailId.Rows[0]["Username"].ToString();
                        
                        if (usermailId != null & dtusermailId.Rows.Count > 0)
                            SendEmailwithAttachmentFile(usermailId, usermailId, "TAMCE Certificate", "Hi, Please Find the Attachment!", fileResult.FileDownloadName);

                    }
                    #endregion

                    return PartialView("_APMCEPermanentCertificate", model.APMCECertificate);
                }
                else if (dtItems.Rows[0]["ActType"].ToString() == "PCPNDT")
                {

                    model.PCPNDTCertificate = licenceBAL.GetPCPNDTLicenseDetails(approval.TransactionId, Type);
                    SendApprovalSMS(0, approval.TransactionId, approval.status, "Grant");
                    //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                    //PCPNDTmodel = model.PCPNDTCertificate;
                    return PartialView("_PCPNDTLicense", model.PCPNDTCertificate);
                }
                else if (dtItems.Rows[0]["ActType"].ToString() == "BloodBank")
                {
                    model.BloodBankNOCModel = licenceBAL.GetBloodBankNOC(approval.TransactionId);
                    return PartialView("_BloodBankNOC", model.BloodBankNOCModel);
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
                    SendApprovalSMS(0, approval.TransactionId, approval.status, "Grant");
                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                return Json(Notification);
            }
        }

        private bool SendApprovalSMS(int ApplicationId, int TransactionId, Status status, string applicationType)
        {
            SMSModel smsData = new SMSModel();
            objBAL = new DepartmentUserBAL();

            ApplicationBAL objApplicationBAL = new ApplicationBAL();


            string applicantMSG = "";
            string DeptMSG = "";
            if (status == Status.Return)
            {
                smsData = objApplicationBAL.GetSMSDetails(0, TransactionId, 2, applicationType);
                applicantMSG = "Hi " + smsData.ApplicantName + "," + " Application Number :  " + smsData.ApplicationNumber + ".  " + smsData.PrevDeptName + " has " + status + "ed the Application to " + smsData.DeptUserName;
                DeptMSG = "Hi " + smsData.DeptUserName + "," + " Application Number :  " + smsData.ApplicationNumber + " ." + " Applicant :" + smsData.ApplicantName + "." + smsData.PrevDeptName + " has " + status + "ed the Application";

            }
            else if (status == Status.Forward)
            {
                smsData = objApplicationBAL.GetSMSDetails(0, TransactionId, 1, applicationType);
                applicantMSG = "Hi " + smsData.ApplicantName + "," + " Application Number :  " + smsData.ApplicationNumber + ".  " + smsData.PrevDeptName + " has " + status + "ed the Application to " + smsData.DeptUserName;
                DeptMSG = "Hi " + smsData.DeptUserName + "," + " Application Number :  " + smsData.ApplicationNumber + " ." + " Applicant :" + smsData.ApplicantName + ". " + smsData.PrevDeptName + " has Forwarded the Application.";

            }
            else if (status == Status.Approved || status == Status.Rejected)
            {
                smsData = objApplicationBAL.GetSMSDetails(0, TransactionId, 1, applicationType);
                applicantMSG = "Hi " + smsData.ApplicantName + "," + " Application Number :  " + smsData.ApplicationNumber + ". Your Registraion has been " + status + ". Please Login and check the details ";
            }



            UserBAL userBAL = new UserBAL();
            string deliveryStatus;
            bool result = Utitlities.SendSMS(smsData.ApplicantMobileNumber, applicantMSG, out deliveryStatus);
            // userBAL.SendSMS(applicantMSG, smsData.ApplicantMobileNumber, "", "single");
            if (DeptMSG != "")
            {
                result = Utitlities.SendSMS(smsData.DeptMobile, DeptMSG, out deliveryStatus);
                //userBAL.SendSMS(DeptMSG, smsData.DeptMobile, "", "single");
            }

            return result;
        }


        private bool SendDeptQuerySMS(int ApplicationId, int TransactionId, string applicationType)
        {
            ApplicationBAL objApplicationBAL = new ApplicationBAL();

            SMSModel smsData = objApplicationBAL.GetSMSDetails(ApplicationId, TransactionId, 0, applicationType);
            string ApplicantMsg = "Hi " + smsData.ApplicantName + "," + " Application Number :  " + smsData.ApplicationNumber + ".  " + "Query has been raised by " + smsData.DeptUserName + ". Please Login and check the details";
            string deliveryStatus;
            bool result = Utitlities.SendSMS(smsData.ApplicantMobileNumber, ApplicantMsg, out deliveryStatus);

            return result;
        }
        private string GeneratePDF(string serializedString)
        {
            string pdfString = Decode(serializedString);
            string htmlString = pdfString;

            // htmlString = "< Header > < h1 align = \"center\" > PCPNDT Inspection Report (CHFW)  </ h1 >     < h2 align = \"center\" > Application No </ h2 ></ Header >" + htmlString;

            // htmlString = htmlString.Replace("hidden", "align=\"right\" hidden");

            // htmlString = htmlString.Replace("legend", "legend align=\"center\" ");

            // SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf(820, 1060);

            // SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            // SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString);

            htmlString = "<Header> <h1 align=\"center\"> PCPNDT Inspection Report (CHFW)  </h1> <h2 align=\"center\"> Application No </h2></Header>" + htmlString;
            htmlString = htmlString.Replace("hidden", "align=\"right\" hidden");
            htmlString = htmlString.Replace("legend", "legend align=\"center\" ");

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf(820, 1060);

            SelectPdf.PdfDocument doc = new SelectPdf.PdfDocument();
            //SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(htmlString);
            SelectPdf.PdfMargins m = new SelectPdf.PdfMargins(15, 2, 15, 2);
            doc.Margins = m;
            //doc.Margins = m;
            doc.Append(converter.ConvertHtmlString(htmlString));



            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".pdf";



            var path = Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);


            System.IO.File.WriteAllBytes(path, ((FileContentResult)fileResult).FileContents);
            return fileResult.FileDownloadName;
        }

        public JsonResult AddQueryRaiseFile(QueryModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase _uploadedFile = null;
                if (Request.Files.Count > 0)
                    _uploadedFile = Request.Files[0];

                model.CreatedUserId = Session.GetDataFromSession<UserModel>("User").Id;

                List<QueryModel> objQueryRaisedList;
                if (Session["QueryRaisedList"] != null)
                    objQueryRaisedList = Session.GetDataFromSession<List<QueryModel>>("QueryRaisedList");
                else
                    objQueryRaisedList = new List<QueryModel>();

                #region Saving files physically
                if (_uploadedFile != null)
                {
                    //HttpPostedFileBase _uploadedFile = Request.Files[0];
                    var uploadsPath = Path.Combine("Department", "9", "QueryUploads"); //TODO Change DesignationID  Mounika -23-05-2017

                    if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                    string _filePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                    _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _filePath));
                    model.UploadedFilePath = _filePath + Path.GetExtension(_uploadedFile.FileName);

                    string _uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _filePath);
                    System.IO.File.Move(_uploadedfilepath, _uploadedfilepath + Path.GetExtension(_uploadedFile.FileName));
                }
                #endregion

                objQueryRaisedList.Add(model);
                Session.SetDataToSession<List<QueryModel>>("QueryRaisedList", objQueryRaisedList);

                return Json(objQueryRaisedList);
            }
            else
            {
                return Json("Invalid data");
            }
        }

        public JsonResult DeleteQueryRaise(int index)
        {
            if (Session["QueryRaisedList"] != null)
            {
                List<QueryModel> objQueryRaisedList = Session.GetDataFromSession<List<QueryModel>>("QueryRaisedList");
                if (objQueryRaisedList[index].Id == 0)
                    objQueryRaisedList.RemoveAt(index);
                else
                    objQueryRaisedList[index].IsDeleted = true;
                Session.SetDataToSession<List<QueryModel>>("QueryRaisedList", objQueryRaisedList);
                return Json(objQueryRaisedList.Where(item => item.IsDeleted == false).ToList());
            }
            return Json(null);
        }

        public JsonResult SubmitQuery(string Query, int TransactionId, string ApplicationType)
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            LicenseBAL objLicenseBal = new LicenseBAL();
            QueryModel QueryModel = new QueryModel();
            QueryModel.Description = Query;
            QueryModel.TransactionId = TransactionId;
            QueryModel.DepartmentId = user.DepartmentId;
            QueryModel.UserId = user.Id;
            QueryModel.Type = "Query";
            QueryModel.ApplicationType = ApplicationType;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase _uploadedFile = Request.Files[0];
                var uploadsPath = Path.Combine("Department", "9", "QueryUploads"); //TODO Change DesignationID  Mounika -23-05-2017

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));

                string _filePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _filePath));
                QueryModel.UploadedFilePath = _filePath + Path.GetExtension(_uploadedFile.FileName);

                string _uploadedfilepath = Path.Combine(Server.MapPath("~/Uploads"), _filePath);
                System.IO.File.Move(_uploadedfilepath, _uploadedfilepath + Path.GetExtension(_uploadedFile.FileName));
            }

            #region Newly added for multiple upload
            List<QueryModel> queryRaiseList = new List<QueryModel>();
            if (Session["QueryRaisedList"] != null)
            {
                queryRaiseList = Session.GetDataFromSession<List<QueryModel>>("QueryRaisedList");
                queryRaiseList = queryRaiseList.Where(item => item.IsDeleted == false).ToList();
            }
            int resultcount = queryRaiseList.Count;
            int i = 0;
            bool result = false;
            if (queryRaiseList.Count > 0)
            {
                foreach (var objModel in queryRaiseList)
                {
                    QueryModel.Description = objModel.Description;
                    QueryModel.TransactionId = objModel.TransactionId;
                    QueryModel.DepartmentId = user.DepartmentId;
                    QueryModel.UserId = user.Id;
                    QueryModel.Type = "Query";
                    QueryModel.ApplicationType = objModel.ApplicationType;
                    QueryModel.UploadedFilePath = objModel.UploadedFilePath;
                    objLicenseBal = new LicenseBAL();
                    result = objLicenseBal.SubmitResponse(QueryModel);
                    if (result)
                        i = i + 1;
                }
            }
            #endregion
            //bool result = objLicenseBal.SubmitResponse(QueryModel);
            NotificationModel notification = new NotificationModel();
            if (resultcount == i) // result
            {
                notification.Title = "Success";
                notification.NotificationType = NotificationType.Success;
                notification.NotificationMessage = "Your Query has been submitted successfully";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Success;
                notification.NonActionButtonText = "Close";
                SendDeptQuerySMS(0, TransactionId, ApplicationType);
            }
            else
            {
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Oops! Something went wrong... <br>Your Query was not submitted, please contact technical support.";
                notification.ShowNonActionButton = true;
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.NonActionButtonText = "Close";
            }
            return Json(notification);

        }

        public JsonResult GetQureyResponsebyTransactionId(int TransactionId, string TransactionType)
        {
            objBAL = new DepartmentUserBAL();
            return Json(objBAL.GetQureyResponsebyTransactionId(TransactionId, TransactionType), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddDocuments(DocumentUploadModel Upload)
        {
            HttpPostedFileBase _uploadedFile = Request.Files[0];
            var uploadsPath = Path.Combine("Deparment", "InspectionReports");

            string _uploadedFilePath = uploadsPath + "/" + Path.GetFileNameWithoutExtension(_uploadedFile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            Upload.DocumentPath = _uploadedFilePath + Path.GetExtension(_uploadedFile.FileName);

            if (!Directory.Exists(Server.MapPath("~/Uploads/" + uploadsPath)))
                Directory.CreateDirectory(Server.MapPath("~/Uploads/" + uploadsPath));
            #region Saving files physically
            _uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath));

            string oldfilepath = Path.Combine(Server.MapPath("~/Uploads"), _uploadedFilePath);
            System.IO.File.Move(oldfilepath, oldfilepath + Path.GetExtension(_uploadedFile.FileName));
            #endregion
            List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();
            if (Session["UploadList"] != null)
                UploadList = Session["UploadList"] as List<DocumentUploadModel>;
            Upload.UploadedUserId = Session.GetDataFromSession<UserModel>("User").Id;

            if(UploadList.Any(item=>item.UploadType == "Inspection") != true) // Added On 01-07-2021 by Chandu
                UploadList.Add(Upload);
            Session["UploadList"] = UploadList;
            return Json(UploadList);
        }

        public string GetDeptUserInspectionQuestions(int TransactionId, string Type)
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            DataSet ds = objBAL.GetDeptUserInspectionQuestions(TransactionId, user.Id, Type);
            DataTable dt = ds.Tables[0];
            return JsonConvert.SerializeObject(ds);
        }


        #region Amendment Approval 
        public ActionResult ListofAmendment(string Type)
        {
            if (Type != null)
                Session["Type"] = Type;
            objBAL = new DepartmentUserBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            List<TransactionViewModel> ApplicationList = objBAL.GetListofApplications(user.DesignationId, user.DistrictId, user.MandalId, user.VillageId, Session["Type"].ToString(), user.Id);
            return View(ApplicationList);
        }
        public ActionResult AmendmentApproval(int TId, int SId, int AId, int TSId) // AId= AmendmentId,TId=TransactionId,SId=ServiceId
        {
            List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();
            objBAL = new DepartmentUserBAL();
            LicenseBAL LBAL = new LicenseBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            ApprovalComplexViewModel Approval = new ApprovalComplexViewModel();

            Approval = objBAL.AmendmentApprovalOnloadData(AId, user.DesignationId, SId);
            Approval.Approval = new ApprovalsModel();
            Approval.Approval.TransactionId = TId;
            Approval.Approval.AmendmentId = AId;
            Approval.ServiceId = SId;
            Approval.TranServiceId = TSId;
            if (TSId == 2) //PCPNDT
                Approval.PCPNDTModel = LBAL.GetPCPNDTData(AId, "Amendment");
            else if (TSId == 1)
                Approval.APMCEModel = LBAL.GetAPMCEData(TId, "Amendment");
            switch (SId)
            {
                case 18: //Add/delete Facility
                    Approval.PCPNDTModel.FacilityLogModel = objBAL.GetFacility(AId);
                    Approval.PCPNDTModel.TestsModelLog = objBAL.GetPCPNDTTests(AId, "InDirect");
                    Approval.PCPNDTModel.FacilitiesModellog = objBAL.GetFacilitiesforTests(AId, "InDirect");
                    break;
                case 21:
                    {//PCPNDT Lists of Tests/Procedures
                        Approval.PCPNDTModel.TestsModelLog = objBAL.GetPCPNDTTests(AId, "Direct");
                    }
                    break;
                case 23: //PCPNDT Facilities available
                    Approval.PCPNDTModel.FacilitiesModellog = objBAL.GetFacilitiesforTests(AId, "Direct");
                    break;
                case 22://PCPNDT Equipment details
                    Approval.PCPNDTModel.EquipmentListLog = objBAL.GetEquipments(AId);
                    break;
                case 24:
                    {
                        // PCPNDT Employee Details
                        Approval.PCPNDTModel.EmployeeListLog = objBAL.GetEmployees(AId);
                        Session["EmployeeListLog"] = Approval.PCPNDTModel.EmployeeListLog;
                    }
                    break;
                case 19://PCPNDT Type of Ownership

                    Approval.PCPNDTModel.InstitutionModelLog = objBAL.GetOwnership(AId);
                    break;
                case 20://  PCPNDT Type of Institution
                    Approval.PCPNDTModel.InstitutionModelLog = objBAL.GetInstitution(AId);

                    break;
                case 28: //PCPNDT Cancellation of License
                    Approval.PCPNDTModel.cancelLiceseModel = objBAL.GetCancelLicenseDetails(TId);
                    break;
                case 27: //PCPNDT Appeal against rejection Renewal
                    {
                        PCPNDTViewModel model = objBAL.GetPCPNDTAppealReason(AId, TId);
                        Approval.PCPNDTModel.ReasonforAppeal = model.ReasonforAppeal;
                        Approval.PCPNDTModel.RejectionRemarks = model.RejectionRemarks;
                    }
                    break;
                case 26: //PCPNDT Appeal against rejection Grant
                    {
                        PCPNDTViewModel model = objBAL.GetPCPNDTAppealReason(AId, TId);
                        Approval.PCPNDTModel.ReasonforAppeal = model.ReasonforAppeal;
                        Approval.PCPNDTModel.RejectionRemarks = model.RejectionRemarks;
                    }
                    break;
                case 3: //APMCE Corresponding Details
                    Approval.APMCEModel.CorrespondingAddressLog = objBAL.GetAPMCECorrespondentDetails(AId);
                    break;
                case 4: //APMCE Accomodation Details
                    Approval.APMCEModel.AccommadationLog = objBAL.GetAPMCEAccomodationDetails(AId);
                    break;
                case 5: //APMCE Types of Services
                    Approval.APMCEModel.OfferedServicesLog = objBAL.GetAPMCEOfferedServices(AId);
                    break;
                case 6: //APMCE Employee Details
                    Approval.APMCEModel.StaffDetailsLog = objBAL.GetAPMCEStaffDetails(AId);
                    break;
                case 8: //APMCE Infrastructure
                    Approval.APMCEModel.InfraStructureListLog = objBAL.GetEquipmentAndFurniture(AId);
                    break;
                case 7: //APMCE Infrastructure
                    Approval.APMCEModel.FacilitiesAvailableLogModel = objBAL.GetAPMCEFacilitiesAvailable(AId);
                    break;
                case 14: //APMCE Appeal against rejection Grant
                    {
                        APMCEViewModel model = objBAL.GetAPMCEAppealReason(AId, TId);
                        Approval.APMCEModel.ReasonforAppeal = model.ReasonforAppeal;
                        Approval.APMCEModel.RejectionRemarks = model.RejectionRemarks;
                    }
                    break;
                case 17: //APMCE  Appeal against rejection Renewal
                    {
                        APMCEViewModel model = objBAL.GetAPMCEAppealReason(AId, TId);
                        Approval.APMCEModel.ReasonforAppeal = model.ReasonforAppeal;
                        Approval.APMCEModel.RejectionRemarks = model.RejectionRemarks;
                    }
                    break;
                case 38: // NOC of Equipment
                    {
                        Approval.PCPNDTModel.NocforEquipmentModel = objBAL.GetNOCofEquipment(AId);
                    }
                    break;
                case 39: // Hospital Name Change in Registration Details
                    {
                        Approval.APMCEModel.RegistrationModelLog = objBAL.GetAPMCERegistrationDetails(AId);
                    }
                    break;
            }
            return View(Approval);
        }
        [HttpPost]
        public ActionResult AmendmentApproval(ApprovalsModel approval, string Submit, List<InspectionModel> InspectionList, string serializedString, string HasInspectionPrivilege)
        {
            string Type = "Amendment";
            string Path = "";
            List<DocumentUploadModel> UploadList = new List<DocumentUploadModel>();
            objBAL = new DepartmentUserBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            #region Check APMCE or PCPNDT
            LicenseBAL licenceBAL = new LicenseBAL();
            DataTable dtItems = licenceBAL.GetLicenseType(approval.AmendmentId, Type);
            LicenseViewModel model = new LicenseViewModel();
            string ApplicationType = dtItems.Rows[0]["ActType"].ToString();
            #endregion
            if (Submit == "Forward")
            {
                approval.status = Status.Forward;
                if (ApplicationType != "PCPNDT")
                {
                    if (Session["UploadList"] != null)
                        UploadList = Session["UploadList"] as List<DocumentUploadModel>;

                    InspectionList = null;
                }
                else if (ApplicationType == "PCPNDT")
                {
                    if (HasInspectionPrivilege == "True")
                    {

                        if (InspectionList != null && InspectionList.Count > 0)
                        {
                            Path = GeneratePDF(serializedString);
                            InspectionList.ForEach(item => item.DepartmentUserId = user.Id);
                        }

                        if (Session["UploadList"] != null)
                            UploadList = Session["UploadList"] as List<DocumentUploadModel>;
                    }
                    else
                    {
                        UploadList = null;
                        InspectionList = null;
                    }
                }
            }

            else if (Submit == "Return")
                approval.status = Status.Return;
            else if (Submit == "Approve")
                approval.status = Status.Approved;
            else if (Submit == "Reject")
                approval.status = Status.Rejected;
            approval.UserId = user.Id;

            if (Session["UploadList"] != null)
                UploadList = Session["UploadList"] as List<DocumentUploadModel>;

            bool Result = objBAL.SaveAmedmentApprovals(approval, user.DesignationId, InspectionList, UploadList, approval.TransactionId, Path);

            //SendApprovalSMS(approval.AmendmentId, approval.status,"Appeal");
            if (approval.status == Status.Approved)
            {

                if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
                {
                    model.APMCECertificate = licenceBAL.GetAPMCECertificate(approval.TransactionId, Type);
                    return PartialView("_APMCEPermanentCertificate", model.APMCECertificate);
                }
                else if (dtItems.Rows[0]["ActType"].ToString() == "PCPNDT")
                {

                    model.PCPNDTCertificate = licenceBAL.GetPCPNDTLicenseDetails(approval.TransactionId, Type);
                    SendApprovalSMS(0, approval.TransactionId, approval.status, "Grant");
                    //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                    //PCPNDTmodel = model.PCPNDTCertificate;
                    return PartialView("_PCPNDTLicense", model.PCPNDTCertificate);
                }
                else if (dtItems.Rows[0]["ActType"].ToString() == "BloodBank")
                {
                    model.BloodBankNOCModel = licenceBAL.GetBloodBankNOC(approval.TransactionId);
                    return PartialView("_BloodBankNOC", model.BloodBankNOCModel);
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
                    Notification.ActionName = "ListofAmendment";
                    Notification.ControllerName = "DepartmentUser";
                    Notification.AreaName = "Department";
                    SendApprovalSMS(0, approval.TransactionId, approval.status, "Appeal");

                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                return Json(Notification);
            }
        }

        public PartialViewResult GetAmendment(int AmendmentId, int ServiceId, int TransactionId)
        {
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            LicenseBAL LBAL = new LicenseBAL();

            APMCEViewModel tamce = new APMCEViewModel();
            tamce= LBAL.GetAPMCEData(AmendmentId, "Amendment");

            PCPNDTViewModel pcpndt = new PCPNDTViewModel();
            pcpndt = LBAL.GetPCPNDTData(AmendmentId, "Amendment");
            string partialView = "";
            switch (ServiceId)
            {
                case 18: //Add/delete Facility
                    {
                        pcpndt.FacilityLogModel = objBAL.GetFacility(AmendmentId);
                        pcpndt.TestsModelLog = objBAL.GetPCPNDTTests(AmendmentId, "InDirect");
                        pcpndt.FacilitiesModellog = objBAL.GetFacilitiesforTests(AmendmentId, "InDirect");
                        partialView = "_PCPNDTFacilities";


                    }
                    break;
                case 19://PCPNDT Type of Ownership
                    {
                        pcpndt.InstitutionModelLog = objBAL.GetOwnership(AmendmentId);
                        partialView = "_PCPNDTOwnership";

                    }
                    break;
                case 20://  PCPNDT Type of Institution
                    pcpndt.InstitutionModelLog = objBAL.GetInstitution(AmendmentId);
                    partialView = "_PCPNDTInstitution";
                    InstitutionViewModel institution = pcpndt.InstitutionModelLog;
                    List<OwnershipTypeModel> ownershipTypeList = new List<OwnershipTypeModel>();
                    List<InstitutionTypeModel> institutionTypeList = new List<InstitutionTypeModel>();
                    LBAL.GetOwnershipMasterData(ref ownershipTypeList, ref institutionTypeList);
                    ViewBag.OwnershipTypeList = ownershipTypeList;
                    ViewBag.InstitutionTypeList = institutionTypeList;
                    break;
                case 21:
                    {//PCPNDT Lists of Tests/Procedures
                        pcpndt.TestsModelLog = objBAL.GetPCPNDTTests(AmendmentId, "Direct");
                        partialView = "_PCPNDTTests";
                    }
                    break;
                case 22://PCPNDT Equipment details
                    pcpndt.EquipmentListLog = objBAL.GetEquipments(AmendmentId);
                    partialView = "_PCPNDTEquipmentDetails";
                    break;
                case 23: //PCPNDT Facilities available
                    {
                        pcpndt.FacilitiesModellog = objBAL.GetFacilitiesforTests(AmendmentId, "Direct");
                        partialView = "_PCPNDTFacilitiesforTests";
                    }

                    break;

                case 24:
                    {
                        // PCPNDT Employee Details
                        pcpndt.EmployeeListLog = objBAL.GetEmployees(AmendmentId);
                        Session["EmployeeListLog"] = pcpndt.EmployeeListLog;
                        partialView = "_PCPNDTEmployeeDetails";
                    }
                    break;
                case 25:
                    {
                        LicenseBAL licenceBAL = new LicenseBAL();
                        PCPNDTLicenseInfoModel pcpndtlicense = licenceBAL.GetPCPNDTLicenseDetails(AmendmentId, "Amendment");
                        //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                        //PCPNDTmodel = model.PCPNDTCertificate;
                        return PartialView("_PCPNDTLicense", pcpndtlicense);
                    }
                    break;
                case 28: //PCPNDT Cancellation of License
                    pcpndt.cancelLiceseModel = objBAL.GetCancelLicenseDetails(TransactionId);
                    partialView = "_CancelLicense";
                    break;

                case 38: // NOC of Equipment
                    {
                        pcpndt.NocforEquipmentModel = objBAL.GetNOCofEquipment(AmendmentId);
                        partialView = "_PCPNDTNOCforEquipmentView";
                    }
                    break;
            }
            if (ServiceId == 38)
            {
                return PartialView(partialView, pcpndt.NocforEquipmentModel);
            }
            else
                return PartialView(partialView, pcpndt);
        }

        #endregion

        #region PDF       
        //public ActionResult StandartPDF()
        //{

        //    var makeInspectionSession = "siva";

        //    var root = Server.MapPath("~/Uploads/Department/InspectionReports/");
        //    var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
        //    var path = Path.Combine(root, pdfname);
        //    path = Path.GetFullPath(path);

        //    var report = new Rotativa.ViewAsPdf("StandartPDF", makeInspectionSession) { FileName = "Inspection.pdf", SaveOnServerPath = path };
        //    return report;

        //}


        public static void PdfFile(string Event, string Message)
        {
            //Logger.PrintPDF("", "hi");
            //string FileLocation = ConfigurationManager.AppSettings["InspectionFile"];
            //if (FileLocation == "")
            //    FileLocation = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Department/InspectionReports/");
            //string FileName = Event + ".txt";

            //StreamWriter sw = "ff"; //File.AppendText(FileLocation + FileName);
            //sw.WriteLine("");
            //sw.WriteLine("====================" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            //sw.WriteLine(Message.ToString());
            //sw.Flush();
            //sw.Close();
        }



        //protected void PdfFile(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Document pdfDoc = new Document(PageSize.A4, 25, 10, 25, 10);
        //        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //        pdfDoc.Open();
        //        Paragraph Text = new Paragraph("This is test file");
        //        pdfDoc.Add(Text);
        //        pdfWriter.CloseStream = false;
        //        pdfDoc.Close();
        //        Response.Buffer = true;
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment;filename=Example.pdf");
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.Write(pdfDoc);
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    { Response.Write(ex.Message); }
        //}

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

        public ActionResult ConvertAboutPageToPdf()
        {
            int TransactionId = 1;
            int ServiceId = 1;
            Session["UploadList"] = null;
            objBAL = new DepartmentUserBAL();
            LicenseBAL LBAL = new LicenseBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            ApprovalComplexViewModel Approval = objBAL.ApprovalSceenOnloadData(TransactionId, user.DesignationId, ServiceId, user.Id); // //TODO : change transactionid,designationid (get this login user session)            
            Approval.Approval = new ApprovalsModel();
            Approval.Approval.TransactionId = TransactionId;
            Approval.ServiceId = ServiceId;
            if (ServiceId == 2) //PCPNDT
                Approval.PCPNDTModel = LBAL.GetPCPNDTData(TransactionId, "Transaction");
            else if (ServiceId == 1)
                Approval.APMCEModel = LBAL.GetAPMCEData(TransactionId, "Transaction");


            // get the About view HTML code
            string htmlToConvert = RenderViewAsString("Apporoval", null);

            // the base URL to resolve relative images and css
            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertAboutPageToPdf".Length);

            // instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.ConvertedHtmlElementSelector = "#testdiv";
            htmlToPdfConverter.Document.Header.Enabled = false;

            // render the HTML code as PDF in memory
            byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "AboutMvcViewToPdf2.pdf";
            var path = Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);
            ToFile(fileResult, path);

            return fileResult;
        }

        public void GetAndReturnSerializedString()
        {
            //http://jsbin.com/ejuru
            string serializedString = Request.Params["serializedString"];
            string pdfString = Decode(serializedString);
            Session["pdf"] = pdfString;
            DownloadPDF();
        }
        string Decode(string input)
        {
            input = Server.HtmlDecode(input);
            return input;
        }

        public ActionResult DownloadPDF()
        {
            string serializedString = Session["pdf"].ToString();
            string pdfString = Decode(serializedString);
            // read parameters from the webpage
            string htmlString = pdfString; // Session["pdf"].ToString();
            //string baseUrl = collection["TxtBaseUrl"];

            //string pdf_page_size = collection["DdlPageSize"];
            // SelectPdf.PdfPageSize pageSize = (SelectPdf.PdfPageSize)Enum.Parse(typeof(SelectPdf.PdfPageSize),
            //  pdf_page_size, true);

            //string pdf_orientation = collection["DdlPageOrientation"];
            // SelectPdf.PdfPageOrientation pdfOrientation =
            //  (SelectPdf.PdfPageOrientation)Enum.Parse(typeof(SelectPdf.PdfPageOrientation),
            //  pdf_orientation, true);

            //int webPageWidth = 1024;
            //try
            //{
            //    webPageWidth = Convert.ToInt32(collection["TxtWidth"]);
            //}
            //catch { }

            //int webPageHeight = 0;
            //try
            //{
            //    webPageHeight = Convert.ToInt32(collection["TxtHeight"]);
            //}
            //catch { }

            // instantiate a html to pdf converter object
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            // set converter options
            //converter.Options.PdfPageSize = pageSize;
            //converter.Options.PdfPageOrientation = pdfOrientation;
            //converter.Options.WebPageWidth = webPageWidth;
            //converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString, null);
            SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(htmlString);

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document2.pdf";
            var path = Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);

            System.IO.File.WriteAllBytes(path, ((FileContentResult)fileResult).FileContents);

            ToFile(fileResult, path);
            return fileResult;

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

        public FileResult Export(string status)
        {
            string FileName = "";
            objBAL = new DepartmentUserBAL();
            UserViewModel user = Session.GetDataFromSession<UserViewModel>("User");
            List<TransactionViewModel> ApplicationList = objBAL.GetListofApplications(user.DesignationId, user.DistrictId, 0, 0, "ForwardAppToday", user.Id);
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("ServiceName"),
                                            new DataColumn("ApplicantName"),
                                            new DataColumn("Status"),
                                            new DataColumn("CurrentDesignation"),new DataColumn("ExpiryDate")
                                                         });

            if (status == "-Select-")
            {
                FileName = "_TotalApplications.xlsx";

            }
            else if (status == "Forwarded")
            {
                status = "Forward";
                FileName = "_ForwardedApplications.xlsx";
                ApplicationList = ApplicationList.Where(x => x.StatusName == status).ToList();
            }
            else
            {

                FileName = "_ApprovedApplications.xlsx";
                ApplicationList = ApplicationList.Where(x => x.StatusName == status).ToList();
            }

            foreach (TransactionViewModel transaction in ApplicationList)
            {
                dt.Rows.Add(transaction.ServiceName, transaction.ApplicantName, transaction.StatusName, transaction.CurrentDesignationName, transaction.LicenseExpiryDate);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", user.UserName + FileName);
                }
            }
        }

        public ActionResult DownloadEnclosures(int transactionId)
        {
            try
            {
                LicenseBAL LBAL = new LicenseBAL();
                PCPNDTViewModel objPCPNDTData = LBAL.GetPCPNDTData(transactionId, "Transaction");
                //APMCEViewModel objAPMCEDocData
                UserViewModel user = Session.GetDataFromSession<UserViewModel>("User");

                using (ZipFile zip = new ZipFile())
                {
                    //zip.AlternateEncodingUsage = ZipOption.AsNecessary;       // giving reference error
                    //zip.AddDirectoryByName("31_Demo/Files");

                    //Applicant Docs
                    List<string> ApplicantDocs = new List<string>();
                    ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.ApplicantModel.ApplicantPhoto));
                    ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.ApplicantModel.AadharCardPath));
                    ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.ApplicantModel.PANCardPath));
                    foreach (string doc in ApplicantDocs)
                    {
                        zip.AddFile(doc, user.UserName + "/" + objPCPNDTData.ApplicationNumber + "/" + "ApplicantDocuments");
                    }

                    //Facility Docs
                    List<string> FacilityDocs = new List<string>();
                    FacilityDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.FacilityModel.AddressProofPath));
                    FacilityDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.FacilityModel.BuildingLayoutPath));
                    FacilityDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.FacilityModel.OwnerShipPath));
                    foreach (string doc in FacilityDocs)
                    {
                        zip.AddFile(doc, user.UserName + "/" + objPCPNDTData.ApplicationNumber + "/" + "FacilityDocuments");
                    }


                    //Equipment Docs
                    List<string> EquipmentDocs = new List<string>();
                    if (objPCPNDTData.EquipmentList != null && objPCPNDTData.EquipmentList.Count > 0)
                    {
                        for (int i = 0; i < objPCPNDTData.EquipmentList.Count; i++)
                        {
                            if (objPCPNDTData.EquipmentList[i].InvoicePath != null && objPCPNDTData.EquipmentList[i].InvoicePath != "")
                            {
                                EquipmentDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.EquipmentList[i].InvoicePath));
                            }
                            if (objPCPNDTData.EquipmentList[i].NocFilePath != null && objPCPNDTData.EquipmentList[i].NocFilePath != "")
                            {
                                EquipmentDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.EquipmentList[i].NocFilePath));

                            }
                            if (objPCPNDTData.EquipmentList[i].TransferCertificatePath != null && objPCPNDTData.EquipmentList[i].TransferCertificatePath != "")
                            {
                                EquipmentDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.EquipmentList[i].TransferCertificatePath));
                            }
                            if (objPCPNDTData.EquipmentList[i].InstallationCerticatePath != null && objPCPNDTData.EquipmentList[i].InstallationCerticatePath != "")
                            {
                                EquipmentDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.EquipmentList[i].InstallationCerticatePath));
                            }
                            if (objPCPNDTData.EquipmentList[i].ImagePath != null && objPCPNDTData.EquipmentList[i].ImagePath != "")
                            {
                                EquipmentDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.EquipmentList[i].ImagePath));
                            }
                        }
                        foreach (string doc in EquipmentDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objPCPNDTData.ApplicationNumber + "/" + "EquipmentDocuments");
                        }
                    }




                    //Employee Docs

                    List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
                    employees = Session.GetDataFromSession<List<EmployeeViewModel>>("EmployeeListLog")
                        .Where(item => item.IsDeleted == false).ToList();

                    if (employees != null && employees.Count > 0)
                    {
                        for (int i = 0; i < employees.Count; i++)
                        {
                            //List<string> EmployeeDocs = new List<string>();
                            foreach (DocumentUploadModel document in employees[i].UploadDocuments)
                            {


                                zip.AddFile(Server.MapPath("~/Uploads/" + document.DocumentPath), user.UserName + "/" + objPCPNDTData.ApplicationNumber + "/" + "EmployeeDocuments" + "/" + employees[i].Name);
                            }

                        }
                    }

                    //Ownership Docs
                    List<string> OwnershipDocs = new List<string>();
                    OwnershipDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.InstitutionModel.AffidavitDocPath));
                    OwnershipDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.InstitutionModel.ArticleDocPath));

                    if (objPCPNDTData.InstitutionModel.StudyCertificateDocPaths != null && objPCPNDTData.InstitutionModel.StudyCertificateDocPaths.Count > 0)
                    {
                        for (int i = 0; i < objPCPNDTData.InstitutionModel.StudyCertificateDocPaths.Count; i++)
                        {
                            if (objPCPNDTData.InstitutionModel.StudyCertificateDocPaths[i].DocumentPath != null)
                            {
                                OwnershipDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.InstitutionModel.StudyCertificateDocPaths[i].DocumentPath));
                            }
                        }
                    }
                    foreach (string doc in OwnershipDocs)
                    {
                        zip.AddFile(doc, user.UserName + "/" + objPCPNDTData.ApplicationNumber + "/" + "OwnershipDocuments");
                    }


                    //Declaration Docs
                    List<string> DeclarationDocs = new List<string>();
                    DeclarationDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.DeclarationModel.SignatureDocPath));
                    if (objPCPNDTData.DeclarationModel.OtherUploadsList != null && objPCPNDTData.DeclarationModel.OtherUploadsList.Count > 0)
                    {
                        for (int i = 0; i < objPCPNDTData.DeclarationModel.OtherUploadsList.Count; i++)
                        {
                            DeclarationDocs.Add(Server.MapPath("~/Uploads/" + objPCPNDTData.DeclarationModel.OtherUploadsList[i].DocumentPath));
                        }

                    }
                    foreach (string doc in DeclarationDocs)
                    {
                        zip.AddFile(doc, user.UserName + "/" + objPCPNDTData.ApplicationNumber + "/" + "DeclarationDocuments");
                    }

                    if (zip.Count > 0)
                    {
                        string zipName = String.Format("PCPNDT_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd"));
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            zip.Save(memoryStream);
                            return File(memoryStream.ToArray(), "application/zip", zipName);
                        }
                    }
                    return File("error", "");
                }
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "DownloadApplicantDocs";
                exception.CustomMessage = "Transaction Id : " + transactionId + " Exception : " + ex.Message;
                Logger.LogError(exception);
                return RedirectToAction("Approval", new { TransactionId = transactionId, ServiceId = 1, AId = 0, TSId = 1 });
            }
        }
        public ActionResult DownloadDepartmentDocs(int transactionId)
        {
            try
            {
                LicenseBAL LBAL = new LicenseBAL();
                int DocuentsRoleTypeId = 1; // For Department Side docs
                DataTable objTAMCEFilesData = LBAL.GetTAMCEUploadedDocsData(transactionId, DocuentsRoleTypeId);
                if (objTAMCEFilesData != null && objTAMCEFilesData.Rows.Count > 0)
                {
                    UserViewModel user = Session.GetDataFromSession<UserViewModel>("User");
                    using (ZipFile zip = new ZipFile())
                    {
                        List<string> DepartmentDocs = new List<string>();
                        foreach (DataRow rowFileData in objTAMCEFilesData.Rows)
                        {
                            if (!string.IsNullOrEmpty(rowFileData["DocumentPath"].ToString()))
                                DepartmentDocs.Add(Server.MapPath("~/Uploads/" + rowFileData["DocumentPath"].ToString()));
                        }
                        if (DepartmentDocs != null && DepartmentDocs.Count > 0)
                        {
                            int i = 0;
                            foreach (string doc in DepartmentDocs)
                            {
                                if (DepartmentDocs[0].ToString() != null && i==0)
                                {
                                    i = 1;
                                    zip.AddFile(doc, user.UserName + "/" + transactionId + "/" + "DepartmentDocuments");
                                }
                                if (DepartmentDocs.Any(item => item.ToString() != doc))
                                    zip.AddFile(doc, user.UserName + "/" + transactionId + "/" + "DepartmentDocuments");
                            }
                        }

                        if (zip.Count > 0)
                        {
                            string zipName = String.Format("TAMCE_Department_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd"));
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                zip.Save(memoryStream);
                                return File(memoryStream.ToArray(), "application/zip", zipName);
                            }
                        }
                        return File("error", "");
                    }
                }
                else
                    return RedirectToAction("Approval", new { TransactionId = transactionId, ServiceId = 1, AId = 0, TSId = 1 });
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "DownloadDepartmentDocs";
                exception.CustomMessage = "Transaction Id : " + transactionId + " Exception : " + ex.Message;
                Logger.LogError(exception);
                return RedirectToAction("Approval", new { TransactionId = transactionId, ServiceId = 1, AId = 0, TSId = 1 });
            }
        }
        public ActionResult DownloadRaiseQueryDocs(int transactionId)
        {
            try
            {
                LicenseBAL LBAL = new LicenseBAL();
                int DocuentsRoleTypeId = 2; // For Raise Query Docs
                DataTable objTAMCEFilesData = LBAL.GetTAMCEUploadedDocsData(transactionId, DocuentsRoleTypeId);
                if (objTAMCEFilesData != null && objTAMCEFilesData.Rows.Count > 0)
                {
                    UserViewModel user = Session.GetDataFromSession<UserViewModel>("User");
                    using (ZipFile zip = new ZipFile())
                    {
                        List<string> DepartmentDocs = new List<string>();
                        foreach (DataRow rowFileData in objTAMCEFilesData.Rows)
                        {
                            if (!string.IsNullOrEmpty(rowFileData["DocumentPath"].ToString()))
                                DepartmentDocs.Add(Server.MapPath("~/Uploads/" + rowFileData["DocumentPath"].ToString()));
                        }
                        if (DepartmentDocs != null && DepartmentDocs.Count > 0)
                        {
                            int i = 0;
                            foreach (string doc in DepartmentDocs)
                            {
                                if (DepartmentDocs[0].ToString() != null && i == 0)
                                {
                                    i = 1;
                                    zip.AddFile(doc, user.UserName + "/" + transactionId + "/" + "RaiseQueryDocuments");
                                }
                                if (DepartmentDocs.Any(item => item.ToString() != doc))
                                    zip.AddFile(doc, user.UserName + "/" + transactionId + "/" + "RaiseQueryDocuments");
                            }
                        }

                        if (zip.Count > 0)
                        {
                            string zipName = String.Format("TAMCE_RaiseQuery_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd"));
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                zip.Save(memoryStream);
                                return File(memoryStream.ToArray(), "application/zip", zipName);
                            }
                        }
                        return File("error", "");
                    }
                }
                else
                    return RedirectToAction("Approval", new { TransactionId = transactionId, ServiceId = 1, AId = 0, TSId = 1 });
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "DownloadRaiseQueryDocs";
                exception.CustomMessage = "Transaction Id : " + transactionId + " Exception : " + ex.Message;
                Logger.LogError(exception);
                //return null;
                return RedirectToAction("Approval", new { TransactionId = transactionId, ServiceId = 1, AId = 0, TSId = 1 });
            }
        }
        public ActionResult DownloadEnclosuresNew(int transactionId)
        {
            try
            {
                LicenseBAL LBAL = new LicenseBAL();
                APMCEViewModel objAPMCEFilesData = LBAL.GetAPMCEFilesData(transactionId, "Transaction");
                UserViewModel user = Session.GetDataFromSession<UserViewModel>("User");
                using (ZipFile zip = new ZipFile())
                {
                    #region  Applicant Docs
                    List<string> ApplicantDocs = new List<string>();
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.ApplicantPhoto))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.ApplicantPhoto));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.AadharCardPath))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.AadharCardPath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.PANCardPath))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.PANCardPath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.HospitalPhoto))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.HospitalPhoto));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.BioCapstoneWaste))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.BioCapstoneWaste));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.TariffBoardPhoto))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.TariffBoardPhoto));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.ConsultantsListPhoto))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.ConsultantsListPhoto));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.FireExtinguisherPhoto))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.FireExtinguisherPhoto));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.RegistrationModel.FireNOC))
                        ApplicantDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.RegistrationModel.FireNOC));

                    if (ApplicantDocs != null && ApplicantDocs.Count > 0)
                    {
                        foreach (string doc in ApplicantDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "ApplicantDocuments");
                        }
                    }
                    #endregion

                    #region Corresponding Address Details
                    List<string> CorrespondingAddressDocs = new List<string>();
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.AadharCardPath))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.AadharCardPath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.PANCardPath))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.PANCardPath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.EducationCertificate))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.EducationCertificate));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.IMAMembership))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.IMAMembership));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.TAXReceipt))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.TAXReceipt));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.THANA_APNA_Registration))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.THANA_APNA_Registration));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.APMCR_TSMCR_MCI))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.APMCR_TSMCR_MCI));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.CorrespondentPhoto))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.CorrespondentPhoto));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.CorrespondingAddress.CorrespondentSignature))
                        CorrespondingAddressDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.CorrespondingAddress.CorrespondentSignature));

                    if (CorrespondingAddressDocs != null && CorrespondingAddressDocs.Count > 0)
                    {
                        foreach (string doc in CorrespondingAddressDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "CorrespondingAddressDocuments");
                        }
                    }
                    #endregion

                    #region Accommodation details
                    List<string> AccommadationDocs = new List<string>();
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.Accommadation.UploadedFilePath))
                        AccommadationDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.Accommadation.UploadedFilePath));
                    if (AccommadationDocs != null && AccommadationDocs.Count > 0)
                    {
                        foreach (string doc in AccommadationDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "AccommadationDocuments");
                        }
                    }
                    #endregion

                    #region Establishment Details
                    List<string> EstablishmentDocs = new List<string>();
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.EstablishmentModel.OpenAreaFilePath))
                        EstablishmentDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.EstablishmentModel.OpenAreaFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.EstablishmentModel.ConstructionAreaFilePath))
                        EstablishmentDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.EstablishmentModel.ConstructionAreaFilePath));
                    if (EstablishmentDocs != null && EstablishmentDocs.Count > 0)
                    {
                        foreach (string doc in EstablishmentDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "EstablishmentDocuments");
                        }
                    }
                    #endregion

                    #region Staff Details
                    List<string> StaffDocs = new List<string>();
                    if (objAPMCEFilesData.StaffDetailsList != null && objAPMCEFilesData.StaffDetailsList.Count > 0)
                    {
                        foreach (var staffDoc in objAPMCEFilesData.StaffDetailsList)
                        {
                            if (!string.IsNullOrEmpty(staffDoc.UploadedFilePath))
                                StaffDocs.Add(Server.MapPath("~/Uploads/" + staffDoc.UploadedFilePath));
                        }
                    }
                    if (StaffDocs != null && StaffDocs.Count > 0)
                    {
                        foreach (string doc in StaffDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "StaffDocuments");
                        }
                    }
                    #endregion

                    #region Equipment Furniture Details
                    List<string> EquipmentFurnitureDocs = new List<string>();
                    if (objAPMCEFilesData.InfraStructureList != null && objAPMCEFilesData.InfraStructureList.Count > 0)
                    {
                        foreach (var EquipmentDoc in objAPMCEFilesData.InfraStructureList)
                        {
                            if (!string.IsNullOrEmpty(EquipmentDoc.UploadedFilePath) && EquipmentDoc.UploadedFilePath.ToString() != "undefined")
                                EquipmentFurnitureDocs.Add(Server.MapPath("~/Uploads/" + EquipmentDoc.UploadedFilePath));
                        }
                    }
                    if (EquipmentFurnitureDocs != null && EquipmentFurnitureDocs.Count > 0)
                    {
                        foreach (string doc in EquipmentFurnitureDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "EquipmentFurnitureDocuments");
                        }
                    }
                    #endregion

                    #region Facilities Avaliable
                    List<string> FacilitiesDocs = new List<string>();
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.FacilitiesAvailableModel.DeclarationStampFilePath))
                        FacilitiesDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.FacilitiesAvailableModel.DeclarationStampFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.FacilitiesAvailableModel.OtherInformationDocumentPath))
                        FacilitiesDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.FacilitiesAvailableModel.OtherInformationDocumentPath));
                    if (FacilitiesDocs != null && FacilitiesDocs.Count > 0)
                    {
                        foreach (string doc in FacilitiesDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "FacilitiesDocuments");
                        }
                    }
                    #endregion

                    #region Additional Documents Details
                    List<string> AdditionalDocs = new List<string>();
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.BioCapstoneWastageClearanceFromFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.BioCapstoneWastageClearanceFromFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.PollutionAuthorityLetterByPCBFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.PollutionAuthorityLetterByPCBFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.CFOFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.CFOFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.STPFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.STPFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.FEReportFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.FEReportFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.TarifListFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.TarifListFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.Establishment_BuildingPlanFilepath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.Establishment_BuildingPlanFilepath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.HospitalOutSideNamePlateBuildingFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.HospitalOutSideNamePlateBuildingFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.TariffBoardFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.TariffBoardFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.FireExhaustiveFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.FireExhaustiveFilePath));
                    if (!string.IsNullOrEmpty(objAPMCEFilesData.AdditionalDocumentsModel.HospitalLabOperationTheaterFilePath))
                        AdditionalDocs.Add(Server.MapPath("~/Uploads/" + objAPMCEFilesData.AdditionalDocumentsModel.HospitalLabOperationTheaterFilePath));
                    if (AdditionalDocs != null && AdditionalDocs.Count > 0)
                    {
                        foreach (string doc in AdditionalDocs)
                        {
                            zip.AddFile(doc, user.UserName + "/" + objAPMCEFilesData.APMCECertificateModel.ApplicationNumber + "/" + "AdditionalDocuments");
                        }
                    }
                    #endregion

                    if (zip.Count > 0)
                    {
                        string zipName = String.Format("TAMCE_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd"));
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            zip.Save(memoryStream);
                            return File(memoryStream.ToArray(), "application/zip", zipName);
                        }
                    }
                    return File("error", "");
                }
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "DownloadEnclosuresNew";
                exception.CustomMessage = "Transaction Id : " + transactionId + " Exception : "+ ex.Message;
                Logger.LogError(exception);
                //return null;
                return RedirectToAction("Approval", new { TransactionId = transactionId, ServiceId = 1, AId = 0, TSId = 1 });
            }

        }


        #region Start Inspection
        public ActionResult VerifyInspectionDetails(int TransactionId, int ServiceId, int AId, int TSId)
        {
            Session["UploadList"] = null;
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            LicenseBAL LBAL = new LicenseBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            ApprovalComplexViewModel Approval = objBAL.ApprovalSceenOnloadData(TransactionId, user.DesignationId, ServiceId, user.Id); // //TODO : change transactionid,designationid (get this login user session)            
            Approval.Approval = new ApprovalsModel();
            Approval.Approval.TransactionId = TransactionId;

            Approval.TranServiceId = ServiceId;
            if (ServiceId == 1)//TAMCE
                Approval.APMCEModel = LBAL.GetAPMCEData(TransactionId, "Transaction");

            Approval.tamceFacilityModel = new TAMCEFacilityModel();

            Approval.ApplicationNumber = Approval.APMCEModel.APMCECertificateModel.ApplicationNumber;
            return View(Approval);
        }
        #endregion

        #region Application Details Status wise
        public ActionResult StatuswiseApplicationDetailsIndex()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            return View();
        }
        public string GetStatuswiseApplicationDetailsIndex(DateTime FromDate,DateTime ToDate, int DistrictId = 0)
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            var dt = DateTime.Parse(FromDate.ToShortDateString());
            string fDate = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var dt1 = DateTime.Parse(ToDate.ToShortDateString());
            string eDate = dt1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var data = objBAL.GetStatuswiseApplicationDetailsIndex(user.Id,user.RoleId, fDate, eDate, DistrictId);
            if (data != null && data.Rows.Count > 0)
                Session["dtStatuswiseApplicationDetails"] = data;

            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;

            return JsonConvert.SerializeObject(data);
        }
        public ActionResult ExportData()
        {
            DateTime? fromDate = null, toDate = null;
            DataTable dt = new DataTable();
            //dt.TableName = "Employee";
            if (Session["dtStatuswiseApplicationDetails"] != null)
                dt = Session["dtStatuswiseApplicationDetails"] as DataTable;

            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            string sheetName= "AppliationDetails";

            if (fromDate != null && toDate != null)
                sheetName = "From_" + String.Format("{0:dd-MM-yyyy}", fromDate) + "_To_" + String.Format("{0:dd-MM-yyyy}", toDate);
                            
                dt.TableName = sheetName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= StatuswiseAppliationDetails.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("StatuswiseApplicationDetailsIndex", "DepartmentUser");
        }
        #endregion

        #region District-wise New Centres
        public ActionResult DistrictwiseNewCentresReport()
        {
            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            return View();
        }
        public string GetDistrictwiseNewCentresReport(DateTime FromDate, DateTime ToDate,int DistrictId=0)
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            var dt = DateTime.Parse(FromDate.ToShortDateString());
            string fDate = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var dt1 = DateTime.Parse(ToDate.ToShortDateString());
            string eDate = dt1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var data = objBAL.GetDistrictwiseNewCentresReport(user.Id, user.RoleId, fDate, eDate,DistrictId);
            if (data != null && data.Rows.Count > 0)
                Session["dtDistrictwiseNewCentresReport"] = data;

            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;

            return JsonConvert.SerializeObject(data);
        }
        public ActionResult ExportNewCentres()
        {
            DateTime? fromDate = null, toDate = null;
            DataTable dt = new DataTable();
            //dt.TableName = "Employee";
            if (Session["dtDistrictwiseNewCentresReport"] != null)
                dt = Session["dtDistrictwiseNewCentresReport"] as DataTable;

            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            string sheetName = "DistrictwiseNewCentres";

            if (fromDate != null && toDate != null)
                sheetName = "From_" + String.Format("{0:dd-MM-yyyy}", fromDate) + "_To_" + String.Format("{0:dd-MM-yyyy}", toDate);

            dt.TableName = sheetName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= DistrictwiseCentresApplicationsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("StatuswiseApplicationDetailsIndex", "DepartmentUser");
        }
        #endregion

        #region District-wise Hospitals and Services
        public ActionResult DistrictwiseCentresReport()
        {
            int districtid = 0;
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            if (user.RoleId == 4)
                districtid = user.DistrictId;

            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.CentresList = objBAL.GetCentresList(districtid);
            return View();
        }
        public string GetDistrictwiseCentresReport(int DistrictId = 0, int CentreId = 0)
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            var data = objBAL.GetDistrictwiseCentresReport(DistrictId,CentreId);
            if (data != null && data.Rows.Count > 0)
                Session["dtDistrictwiseCentresReport"] = data;            
            return JsonConvert.SerializeObject(data);
        }
        public ActionResult ExportCentres()
        {
            DataTable dt = new DataTable();
            if (Session["dtDistrictwiseCentresReport"] != null)
                dt = Session["dtDistrictwiseCentresReport"] as DataTable;

            string sheetName = "Centres_ServiceDetails";            

            dt.TableName = sheetName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= CentresServiceDetailsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("DistrictwiseCentresReport", "DepartmentUser");
        }
        #endregion

        #region District-wise HospitalLicenses and Details 
        public ActionResult DistrictwiseHospitalLicensesReport()
        {
            int districtid = 0;
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            if (user.RoleId == 4)
                districtid = user.DistrictId;

            LicenseBAL objBAL = new LicenseBAL();
            ViewBag.DistrictList = objBAL.GetCountries();
            //ViewBag.CentresList = objBAL.GetCentresList(districtid);
            return View();
        }
        public string GetDistrictwiseHospitalLicensesReport(int DistrictId = 0, int CentreId = 0)
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            var data = objBAL.GetDistrictwiseCentresLicensesReport(DistrictId, CentreId);
            if (data != null && data.Rows.Count > 0)
                Session["dtDistrictwiseHospitalLicensesReport"] = data;
            return JsonConvert.SerializeObject(data);
        }
        public ActionResult ExportHospitalLicenses()
        {
            DataTable dt = new DataTable();
            if (Session["dtDistrictwiseHospitalLicensesReport"] != null)
                dt = Session["dtDistrictwiseHospitalLicensesReport"] as DataTable;

            string sheetName = "Hospital_LicensesDetails";

            dt.TableName = sheetName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= HospitalLicensesDetailsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("DistrictwiseHospitalLicensesReport", "DepartmentUser");
        }
        public ViewResult ApplicationView(int TransactionId, string TransactionType)
        {
            LicenseBAL objBAL = new LicenseBAL();
            ApplicationModel model = objBAL.GetApplication(TransactionId, Status.All, TransactionType);
            model.TransactionId = TransactionId;
            if (model.PCPNDTModel != null)
            {
                Session["EmployeeList"] = model.PCPNDTModel.EmployeeList;
                if (model.PCPNDTModel.ServiceId == 24)
                    Session["EmployeeListLog"] = model.PCPNDTModel.EmployeeListLog;
            }
            return View(model);
        }
        #endregion

        #region SendMail with attachment File
        public void SendEmailwithAttachmentFile(string to, string toName, string subject, string body, string attachment)
        {
            string mail = "tamcehelpdesk@gmail.com";
            string password = "support@acs123";
            string FromMail = "tamcehelpdesk@gmail.com";
            //var fromAddress = new MailAddress("Your Gemail Address", "Sender Project System Name");
            //var toAddress = new MailAddress(to, toName);
            string filedetails = Path.Combine(Server.MapPath("~/Uploads"), attachment);
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
