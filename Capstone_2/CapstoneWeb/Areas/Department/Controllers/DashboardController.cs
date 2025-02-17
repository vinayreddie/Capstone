using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.BAL;
using Newtonsoft.Json;
using Capstone.Framework;
using Capstone.Models;
using System.Data;

namespace CapstoneWeb.Areas.Department.Controllers
{
    [SessionTimeout]
    public class DashboardController : Controller
    {
        // GET: Department/Dashboard
        #region Global
        DashboardBAL objBAL;
        #endregion
        public ActionResult Dashboard()
        {
            return View();
        }      
        public string BindCounts()
       {
            objBAL = new DashboardBAL();
            DataSet ds = objBAL.GetDeptUserDashboadCounts(Session.GetDataFromSession<UserModel>("User"));
            return JsonConvert.SerializeObject(ds);
        }
        public ActionResult DepartmentAdmin()
        {
            return View();
        }
        public ActionResult ApprovedList()
        {
            return View();
        }
        public ActionResult GetApprovedList()
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            AmendmentBAL objBAL = new AmendmentBAL();
            List<TransactionViewModel> approvelist = new List<TransactionViewModel>();
            approvelist = objBAL.GetApprovedList(user);
            return Json(approvelist);
           
        }
        public ActionResult RejectedList()
        {
            return View();
        }
        public ActionResult GetRejectedList()
        {
            UserModel user = Session.GetDataFromSession<UserModel>("User");
            AmendmentBAL objBAL = new AmendmentBAL();
            List<TransactionViewModel> rejectedlist = new List<TransactionViewModel>();
            rejectedlist = objBAL.GetRejectedList(user);
            return Json(rejectedlist);

        }
    }
}