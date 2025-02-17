using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Capstone.BAL;
using System.Data;
using Newtonsoft.Json;
using Capstone.Framework;

namespace CapstoneWeb.Areas.Department.Controllers
{
    [SessionTimeout]
    public class InspectionController : Controller
    {
        // GET: Department/Inspection
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TESTCROR()
        {
            return View();
        }
        public ActionResult TestInspection()
        {
            return View();
        }

       //public string GetDeptUserInspectionQuestions(int TransactionId, int DepartmentUserId, int FacilityId)
       //{
       //     DepartmentUserBAL objBAL = new DepartmentUserBAL();
       //     DataSet ds = objBAL.GetDeptUserInspectionQuestions(TransactionId, DepartmentUserId,FacilityId);
       //     DataTable dt = ds.Tables[0];
       //     return JsonConvert.SerializeObject(ds);
       // }
        public ActionResult InspectionList(List<InspectionModel> Inspection, int TransactionId)
         {
            Inspection.ForEach(item => { item.DepartmentUserId = 1; });
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            bool status= objBAL.SaveInspectionFacilitiesQuestions(Inspection, TransactionId);
           
            NotificationModel Notification = new NotificationModel();
           if(status==true)
            {
                Notification.Title = "Success";
                Notification.NotificationType = NotificationType.Success;
                Notification.NotificationMessage = "Saved Successfully";
               
            }
          
            else
            {
                Notification.Title = "Error";
                Notification.NotificationType = NotificationType.Danger;
                Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
               
            }
            return Json(Notification);

        }

        public ActionResult AddInspectionDetails(int TransactionId, int ServiceId, int AId, int TSId)
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
            
            return View(Approval);
        }

       
    }
}