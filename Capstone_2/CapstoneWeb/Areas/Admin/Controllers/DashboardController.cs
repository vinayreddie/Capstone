using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.BAL;
using System.Data;
using Newtonsoft.Json;

namespace CapstoneWeb.Areas.Admin.Controllers
{
    [SessionTimeout]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View("Dashboard");
        }
        public ActionResult Graphs()
        {
            return View("Graphs");
        }

        #region TAMCE Dashboard Related Actions

        public JsonResult GetCommissionerTAMCEData(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetCommissionerTAMCEData(FromDate,ToDate);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public string GetCommissionerTAMCEDrillDownData(int statusId, int DistrictId = 0, int mandalId = 0, int villageId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dt = DepartmentBAL.CommissionerDashboardServicewise(statusId, DistrictId, mandalId, villageId,FromDate,ToDate);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }

        public ActionResult TAMCEApplicationsView(int statusId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            return View();
        }
        public string GetCommissionerTAMCEApplications()
        {
            DateTime? fromDate=null, toDate=null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int villageId = (int)TempData["villageId"];
            if(TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetCommissionerTAMCEApplications(statusId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }

        public JsonResult GetCommissionerLicensedApplicationsForTAMCE(string type)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetCommissionerLicensedApplicationsForTAMCE(type);
            return Json(dt, JsonRequestBehavior.AllowGet);

        }

        #endregion


        public JsonResult GetCommissionerPCPNDTData()
       {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetCommissionerPCPNDTData();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        
        #region Servicewise Applications
        public string GetCommissionerPCPNDTDrillDownData(int statusId, int DistrictId=0, int mandalId=0, int villageId=0)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dt = DepartmentBAL.CommissionerDashboardServicewise(statusId,DistrictId, mandalId, villageId);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }
        #endregion

        public string GetDistrictWiseLicensedApplications(string Type,string ApplicationType)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dataSet = DepartmentBAL.GetDistrictWiseLicensedApplications(Type, ApplicationType);
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }
        public string GetExpiredLicenses()
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataSet dataSet = DepartmentBAL.GetExpiredLicenses();
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }

        #region Cumulative Licensed Applications

        public string GetCommissionerLicensedApplications(string type)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dataSet = DepartmentBAL.GetCommissionerLicensedApplications(type);
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }
        //public string GetCumulativeLicensedApplications(int serviceId)
        //{
        //    DepartmentBAL DepartmentBAL = new DepartmentBAL();
        //    DataTable dt = DepartmentBAL.GetCumulativeLicensedApplications(serviceId);
        //    string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
        //    return json;
        //}
        public string GetCumulativeLicensedApplicationsView()
        {
            int serviceId = (int)TempData["ServiceId"];
            int districtId = (int)TempData["DistrictId"];
            int mandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["VillageId"];
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dt = DepartmentBAL.GetCumulativeLicensedApplicationsView(serviceId, districtId, mandalId, villageId);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }
        public JsonResult GetCumulativeLicensedApplications(int districtId, int serviceId, int mandalId, int villageId)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetCommissionerDashboardDetails(districtId, serviceId, mandalId, villageId);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CommissionerDashBoardView(int T, int did,int mid,int vid)
        {
            TempData["ServiceId"] = T;
            TempData["DistrictId"] = did;
            TempData["MandalId"] = mid;
            TempData["VillageId"] = vid;
            return View();
        }

        #endregion
        #region BindMaps
        public string BindMapsCount()
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dt = DepartmentBAL.BindMapsCount();
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }
        public JsonResult BindMaps()
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dt = DepartmentBAL.BindMaps(2);
            List<MapModel> list = new List<MapModel>();
           if(dt!=null && dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    MapModel map = new MapModel();
                    map.Name = dr["Name"].ToString();
                    map.Latitude = dr["Latitude"].ToString();
                    map.Longitude = dr["Longitude"].ToString();
                    map.Address = dr["Address"].ToString();
                    map.ImagePath = dr["ImagePath"].ToString();
                    list.Add(map);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region Pending Applications
        public string GetPendingApplications(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dataSet = DepartmentBAL.GetPendingApplications(FromDate, ToDate);
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }

        public string GetDistrictWisePendingApplicationsByDeptUser(int DeptUserId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dataSet = DepartmentBAL.GetDistrictWisePendingApplicationsByDeptUser(DeptUserId,FromDate, ToDate);
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }
        public JsonResult GetPendingApplicationsDistrictwise(int districtId, int serviceId, int mandalId, int villageId)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetPendingApplicationsDistrictwise(districtId, serviceId, mandalId, villageId);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        public ActionResult PendingApplicationsView(int T,int did,int mid,int vid)
        {
            TempData["ServiceId"] = T;
            TempData["DistrictId"] = did;
            TempData["MandalId"] = mid;
            TempData["VillageId"] = vid;
            return View();
        }
        public string GetPendingApplicationsView()
        {
            int serviceId = (int)TempData["ServiceId"];
            int districtId=(int) TempData["DistrictId"];
            int mandalId=(int) TempData["MandalId"];
            int villageId=(int) TempData["VillageId"];
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dt = DepartmentBAL.GetPendingApplications(serviceId,districtId,mandalId,villageId);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }
        #region License Expiry Applications
        public string GetLicenseExpiryApplicationsCount()
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            DataTable dataSet = DepartmentBAL.GetLicenseExpireApplicationsCount();
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }
        #endregion

        #region AppealApplications

        public ActionResult GetAppeals()
        {
            return View("AppealApplications");
        }
        public string GetAppealApplications()
        {
            int DistrictId;
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            int RoleId = Session.GetDataFromSession<UserModel>("User").RoleId;
            if (RoleId == 12)
                DistrictId = 0;
            else
                DistrictId = Session.GetDataFromSession<UserModel>("User").DistrictId;
            DataTable dt = DepartmentBAL.GetAppealApplications(DistrictId,0,null);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);

        }
       public ActionResult GetDistrictAppeals(int DId)
        {
            return View();
        }
        public string GetAppealsDistrictWise(int DistrictId)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            
            DataTable dt = DepartmentBAL.GetAppealApplications(DistrictId, 0, null);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
        public PartialViewResult PreviewApplication(int TransactionId)
        {
            LicenseBAL objBAL = new LicenseBAL();
            ApplicationModel application = objBAL.GetApplication(TransactionId, Status.All, "Amendment");
            return PartialView("_AppealPreview",application);
        }

        public ActionResult AppealApproval(ApprovalsModel approval, string Submit, string HasInspectionPrivilege)
        {
            string Type = "Amendment";
            string ApplicationType = "PCPNDT";
            DepartmentUserBAL  objBAL = new DepartmentUserBAL();
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            LicenseBAL licenceBAL = new LicenseBAL();


            if (Submit == "Approve")
                approval.status = Status.Approved;
            else if (Submit == "Reject")
                approval.status = Status.Rejected;
            approval.UserId = user.Id;

            LicenseViewModel model = new LicenseViewModel();

            bool Result = licenceBAL.AppealApproval(approval);

            //SendApprovalSMS(approval.AmendmentId, approval.status,"Appeal");
            if (approval.status == Status.Approved)
            {
                model.PCPNDTCertificate = licenceBAL.GetPCPNDTLicenseDetails(approval.TransactionId, Type);
               // SendApprovalSMS(0, approval.TransactionId, approval.status, "Grant");
                //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                //PCPNDTmodel = model.PCPNDTCertificate;
                return PartialView("~/Views/Shared/_PCPNDTLicense.cshtml", model.PCPNDTCertificate);
               
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
                   // SendApprovalSMS(0, approval.TransactionId, approval.status, "Appeal");

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
        #endregion

        public ActionResult PCPNDTApplicationsView(int statusId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["villageId"] = villageId;
            return View();
        }
        public string GetCommissionerPCPNDTApplications()
        {
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int villageId= (int)TempData["villageId"];
            DateTime? fromDate = null, toDate = null;
            
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];
            DataTable dt = objBal.GetCommissionerPCPNDTApplications(statusId, villageId);
            return JsonConvert.SerializeObject(dt);
        }
        public ActionResult ApprovedAmendments(string Type,int DistrictId)
        {
            TempData["Type"] = Type;
            TempData["DistrictId"] = DistrictId;
            return View();
        }
        public string GetCommissionerApprovedAmendments()
        {
            string type = (string)TempData["Type"];
            int DistrictId = (int)TempData["DistrictId"];
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetCommissionerApprovedAmendments(type,DistrictId);
            return JsonConvert.SerializeObject(dt);
        }
        public ActionResult ApprovedRegistrations(string Type,int DistrictId)
        {
            TempData["Type"] = Type;
            TempData["DistrictId"] = DistrictId;
            return View("ApprovedLicenses");
        }
        public string GetCommissionerApprovedRegistrations()
        {
            string type = (string)TempData["Type"];
            int DistrictId = (int)TempData["DistrictId"];
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetCommissionerApprovedRegistrations(type,DistrictId);
            return JsonConvert.SerializeObject(dt);
        }

        public ActionResult ExpiredApplications(string Type, int DistrictId)
        {
            TempData["Type"] = Type;
            TempData["DistrictId"] = DistrictId;
            return View();
        }
        public string GetCommissionerExpiredApplications()
        {
            string type = (string)TempData["Type"];
            int DistrictId = (int)TempData["DistrictId"];
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetCommissionerExpiredApplications(type, DistrictId);
            return JsonConvert.SerializeObject(dt);
        }
        public ActionResult PCPNDTPendingApplicationsView(int deptUserId, int DistrictId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["deptUserId"] = deptUserId;
            TempData["DistrictId"] = DistrictId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            return View();
        }
        public string GetPCPNDTPendingApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int deptUserId = (int)TempData["deptUserId"];
            int DistrictId = (int)TempData["DistrictId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];
            DataTable dt = objBal.GetCommissionerPCPNDTPendingApplications(deptUserId, DistrictId,fromDate,toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public PartialViewResult ViewApplication(int _transactionId)
        {
            
           LicenseBAL objBAL = new LicenseBAL();
            ApplicationModel application = objBAL.GetApplication(_transactionId, Status.All, "Transaction");
            return PartialView("~/Areas/User/Views/Shared/_ApplicationPreview.cshtml", application);
        }

        #region Dashboard 1 and actions details
        public ActionResult Dashboard1()
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            ViewBag.DistrictId = user.DistrictId;
            return View();
        }
        public string BindDashboardCounts(int DistrictId=0)
        {
            DashboardBAL objBAL = new DashboardBAL();
            DataSet ds = objBAL.GetAdminDashboadCounts(DistrictId);
            return JsonConvert.SerializeObject(ds);
        }

        #region Received Today Applications
        public ActionResult TAMCEReceivedTodayApplicationsView(string statusId, int DistrictId, int MandalId, int villageId)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetReceivedTodayTAMCEApplications()
        {
            DepartmentBAL objBal = new DepartmentBAL();
            string statusId = (string)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];

            DataTable dt = objBal.GetReceivedTodayTAMCEApplications(statusId, DistrictId, MandalId, villageId);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Submitted Applications
        public ActionResult TAMCESubmittedApplicationsView(int statusId, int DistrictId, int MandalId,int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetSubmittedTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId,DistrictId,MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindSubmittedTAMCEApplications(int StatusId, int DistrictId,int MandalId,int villageId,DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Draft Applications
        public ActionResult TAMCEDraftApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetDraftTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindDraftTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Rejected Applications
        public ActionResult TAMCERejectedApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetRejectedTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindRejectedTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Forward Applications
        public ActionResult TAMCEForwardApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetForwardTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindForwardTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Raise Query Applications
        public ActionResult TAMCERaisedQueriesApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetRaisedQueriesTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindRaisedQueriesTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Returned Applications
        public ActionResult TAMCEReturnedApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetReturnedTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindReturnedTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Suspended Applications
        public ActionResult TAMCESuspendedApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetSuspendedTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindSuspendedTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region ReturnForward Applications
        public ActionResult TAMCEReturnForwardApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetReturnForwardTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindReturnForwardTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Cancelled Applications
        public ActionResult TAMCECancelledApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetCancelledTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindCancelledTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Approved Applications
        public ActionResult TAMCEApprovedApplicationsView(int statusId, int DistrictId, int MandalId, int villageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            TempData["statusId"] = statusId;
            TempData["DistrictId"] = DistrictId;
            TempData["MandalId"] = MandalId;
            TempData["villageId"] = villageId;
            TempData["FromDate"] = FromDate;
            TempData["ToDate"] = ToDate;
            ViewBag.DistrictId = DistrictId;
            return View();
        }
        public string GetApprovedTAMCEApplications()
        {
            DateTime? fromDate = null, toDate = null;
            DepartmentBAL objBal = new DepartmentBAL();
            int statusId = (int)TempData["statusId"];
            int DistrictId = (int)TempData["DistrictId"];
            int MandalId = (int)TempData["MandalId"];
            int villageId = (int)TempData["villageId"];
            if (TempData["FromDate"] != null)
                fromDate = Convert.ToDateTime(TempData["FromDate"]);
            if (TempData["ToDate"] != null)
                toDate = (DateTime)TempData["ToDate"];

            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(statusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        public string BindApprovedTAMCEApplications(int StatusId, int DistrictId, int MandalId, int villageId, DateTime? fromDate, DateTime? toDate)
        {
            DepartmentBAL objBal = new DepartmentBAL();
            DataTable dt = objBal.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId, MandalId, villageId, fromDate, toDate);
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #endregion

    }
}