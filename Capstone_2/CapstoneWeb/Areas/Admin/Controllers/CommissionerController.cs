using Capstone.BAL;
using Capstone.Framework;
using Capstone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneWeb.Areas.Admin.Controllers
{
    [SessionTimeout]
    public class CommissionerController : Controller
    {
        // GET: Admin/Commissioner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CommissionerDashboard()
        {
            return View();
        }
        public JsonResult  CommissionerDashboardCount()
        {
            int districtId = -1;
           // string type = "R";
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetCommissionerPCPNDTData();
              return Json(dt,JsonRequestBehavior.AllowGet);
            //string jsonString = string.Empty;
            //jsonString = JsonConvert.SerializeObject(dt);
            //return jsonString;
            //return View();
        }
        public JsonResult CommissionerDashboardDetails(int districtId,int serviceId,int mandalId,int villageId)
        {
            DepartmentBAL DepartmentBAL = new DepartmentBAL();
            List<GraphModel> dt = DepartmentBAL.GetCommissionerDashboardDetails(districtId, serviceId, mandalId,villageId);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CommissionerDashBoardView(int did,string T)
        {
            TempData["Type"]= T;
            TempData["DistrictId"] = did;
            return View();
        }
        public JsonResult GetCommissionerDashboard()
        {

            string type =(string) TempData["Type"];
            int districtId = (Int32)TempData["DistrictId"];
            ViewBag.type = type;
            DashboardBAL dashboardBAL = new DashboardBAL();
            // int _userId = Session.GetDataFromSession<UserModel>("User").Id;
            DataTable dt = dashboardBAL.GetCommissionerDashoboard(type, districtId);
            //string colName = type;
            ///////more than one columns take the count

            ///////more than one column case use below code inside a loop with count of columns
            //dt.Columns.Add(colName, typeof(string));
            //////////////add new row after adding columns

            //dt[colName] = "value";
            //dt.Rows.Add(dr1.ItemArray);

           // return Json(new { data : JsonConvert.SerializeObject(dt), type : type }); //JsonConvert.SerializeObject(dt);
            return Json(new
            {
                data = JsonConvert.SerializeObject(dt), type = type });
              }
    }
}