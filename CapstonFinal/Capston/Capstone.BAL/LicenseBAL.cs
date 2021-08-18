using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Framework;
using System.Web;



namespace Capstone.BAL
{
    public class LicenseBAL : MasterBAL
    {
        
        LicenseDAL objDAL;
       // ApplicationDAL objApplicationDAL;

        //public List<ApplicationModel> GetApplicationList(int userId)
        //{
        //    objDAL = new LicenseDAL();
        //    DataTable dtItems = objDAL.GetApplicationList(userId);
        //    if (dtItems == null)
        //        return null;

        //    List<ApplicationModel> objList = new List<ApplicationModel>();
        //    ApplicationModel application = new ApplicationModel();
        //    foreach (DataRow row in dtItems.Rows)
        //    {
        //        application = new ApplicationModel();
        //        application.Id = Convert.ToInt32(row["Id"]);
        //        application.ApplicationNumber = row["ApplicationNumber"].ToString();
        //        application.SubmittedOn = Convert.ToDateTime(row["SubmittedOn"]);
        //        application.Status = (Status)Convert.ToInt32(row["Status"]);
        //        objList.Add(application);
        //    }

        //    return objList;
        //}

        //public DataTable GetApplicantDetailsForPayment(int applicationId)
        //{
        //    objDAL = new LicenseDAL();
        //    return objDAL.GetApplicantDetailsForPayment(applicationId);
        //}
 

          
    }
}
