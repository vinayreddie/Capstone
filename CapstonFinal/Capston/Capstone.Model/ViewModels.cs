using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Capstone.Models;

namespace Capstone.Models
{
    public class DepartmentAdminViewModel
    {
        // public DepartmentViewModel Department { get; set; }
        public UserViewModel DepartmentAdmin { get; set; }

        
    }


    public class UserViewModel : UserModel
    {
        public string DesginationName { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
        public string UserPhoto { get; set; }
        public bool IsMobileNoVerified { get; set; }
    }



     

    #region Designation View Model
    public class DesignationComplexViewModel
    {
        public NotificationModel Notification { get; set; }
    }
    public class CabserviceComplexViewModel
    {
        public List<CabserviceModel> CabserviceList { get; set; }
        public NotificationModel Notification { get; set; }
    }
    public class othererviceComplexViewModel
    {
        public List<OtherServiceModel> OthererviceList { get; set; }
        public NotificationModel Notification { get; set; }
    }
    #endregion

    
 

    

     

    

  
}
    