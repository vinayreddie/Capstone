using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class CabserviceModel
    {
        public string Name { get; set; }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int CountryId { get; set; }
       // public int CountryName { get; set; }

        public int AirportId { get; set; }
        public string AirportName { get; set; }

        //public string Timeslot { get; set; }
 
        public string PNR { get; set; }
       // public string Userid     { get; set; }

        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
       // public DateTime? PickupDate { get; set; }

        public string DropingAddress { get; set; }

       // public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class OtherServiceModel
    {
 
        public int Id { get; set; }
        public int UserId { get; set; }
        

        public string Timeslot { get; set; }

        public string PNR { get; set; }
         
        public string PickupDate { get; set; }
        [Display(Name = "Pickup Address")]

        public string DropingAddress { get; set; }

         public string PostalCode { get; set; }

        public string ServiceId { get; set; }

        public string Name { get; set; }
        // public string PhoneNumber { get; set; }

    }

    #region Exception Models
    public class ExceptionModel
    {
        public string RequestPage { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public int LineNo { get; set; }
        public string ErrorMessage { get; set; }
        public string CustomMessage { get; set; }
        public object ParentParamField { get; set; }
        public object MasterParamField { get; set; }
        public object ChildParamField { get; set; }
        public List<TraceFields> SPTraceFields { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedFrom { get; set; }
        public string DbObject { get; set; }
    }

    public class TraceFields
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
    #endregion

    #region Framework Models
    public class NotificationModel
    {
        public NotificationType NotificationType { get; set; }
        public string NotificationTypeClass => this.NotificationType.ToString();
        public string Title { get; set; }
        public bool HasError { get; set; }
        public string NotificationMessage { get; set; }
        public bool ShowListItems { get; set; }
        public List<string> ListItems { get; set; }
        public string BodyContent { get; set; }
        public bool ShowActionButton { get; set; }
        public string ActionButtonText { get; set; }
        public PopupButtonClass ActionButtonClassType { get; set; }
        public string ActionButtonClass => this.ActionButtonClassType.ToString();
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public bool ShowNonActionButton { get; set; }
        public string NonActionButtonText { get; set; }
        public PopupButtonClass NonActionButtonClassType { get; set; }
        public string NonActionButtonClass => this.NonActionButtonClassType.ToString();
        public object ReturnData { get; set; }
        public NotificationModel()
        {
            this.ListItems = new List<string>();
        }
    }
    #endregion

    #region  Master Models
     
    public class MasterServiceModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Service name is Required")]
        public string Name { get; set; }
        public string ActType { get; set; }

    }
     
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = " Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [StringLength(10, ErrorMessage = "Mobile Number should be 10 digits", MinimumLength = 10)]
        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Mobile is required")]
        public string MobileNumber { get; set; }
        public DateTime DateofBirth { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string EmailId { get; set; }
        //[StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        //[Display(Name = "Aadhar")]
        //[Required(ErrorMessage = "Aadhar Number is required")]

        public string AadharNumber { get; set; }
        //[Required(ErrorMessage = "PanNumber is required")]
        //[RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        //[StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PANNumber { get; set; }

        [Display(Name = "Intake")]
        public DateTime DateofJoin { get; set; }
        public string Address { get; set; }
        public RoleTypes RoleType { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        //[Required(ErrorMessage = "Select Jurisdiction")]
        [Display(Name = "Jurisdiction")]

        public string DesignationName { get; set; }
        public int JurisdictionId { get; set; }
        public string JurisdictionLevel { get; set; }
        //[Required(ErrorMessage = "Select District is required")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Select Mandal is required")]
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        //[Required(ErrorMessage = "Select Village is required")]
        [Display(Name = "Village")]
        public int CollegeId { get; set; }
        public int CountryId { get; set; }
        public int VillageId { get; set; }
        //[Required(ErrorMessage = "Security question is required")]
        [Display(Name = "Security Question")]
        public string SecurityQuestion { get; set; }
        //[Required(ErrorMessage = "Security Answer is required")]
        [Display(Name = "Secuirty Answer")]
        public string SecurityAnswer { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedUserId { get; set; }
        public DateTime LastModifiedOn { get; set; }
        //[Required(ErrorMessage = "Select Role")]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        public RoleModel Role { get; set; }
        //[Required(ErrorMessage = "Street Name  is required")]
        public string StreetName { get; set; }
        //[Required(ErrorMessage = "House No   is required")]
        public string HouseNo { get; set; }
        public string OTP { get; set; }
        public string SaltCode { get; set; }

    }

    public class ContactModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = " Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Query { get; set; }
        public string Password { get; set; }
        [StringLength(10, ErrorMessage = "Mobile Number should be 10 digits", MinimumLength = 10)]
        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Mobile is required")]
        public string MobileNumber { get; set; }
        public DateTime DateofBirth { get; set; }
        [Display(Name = "College Email")]
        [Required(ErrorMessage = "College Email is required")]
        [EmailAddress(ErrorMessage = "Invalid College Email")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Select District is required")]
        [Display(Name = "District")]
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }


        //[StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        //[Display(Name = "Aadhar")]
        //[Required(ErrorMessage = "Aadhar Number is required")]

        //public string AadharNumber { get; set; }
        //[Required(ErrorMessage = "PanNumber is required")]
        //[RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        //[StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        //public string PANNumber { get; set; }
        //public DateTime DateofJoin { get; set; }
        //public string Address { get; set; }
        //public RoleTypes RoleType { get; set; }
        //public int DepartmentId { get; set; }
        //public int DesignationId { get; set; }
        //[Required(ErrorMessage = "Select Jurisdiction")]
        //[Display(Name = "Jurisdiction")]

        //public string DesignationName { get; set; }
        //public int JurisdictionId { get; set; }
        //public string JurisdictionLevel { get; set; }
        //[Required(ErrorMessage = "Select District is required")]
        //[Display(Name = "District")]
        //public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Select Mandal is required")]
        //[Display(Name = "Mandal")]
        //public int MandalId { get; set; }
        //[Required(ErrorMessage = "Select Village is required")]
        //[Display(Name = "Village")]
        //public int VillageId { get; set; }
        //[Required(ErrorMessage = "Security question is required")]
        //[Display(Name = "Security Question")]
        //public string SecurityQuestion { get; set; }
        //[Required(ErrorMessage = "Security Answer is required")]
        //[Display(Name = "Secuirty Answer")]
        //public string SecurityAnswer { get; set; }
        //public int CreatedUserId { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int LastModifiedUserId { get; set; }
        //public DateTime LastModifiedOn { get; set; }
        //[Required(ErrorMessage = "Select Role")]
        //[Display(Name = "Role")]
        //public int RoleId { get; set; }
        //public RoleModel Role { get; set; }
        //[Required(ErrorMessage = "Street Name  is required")]
        //public string StreetName { get; set; }
        //[Required(ErrorMessage = "House No   is required")]
        //public string HouseNo { get; set; }
        //public string OTP { get; set; }
        //public string SaltCode { get; set; }

    }

    public class RoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter RoleName")]
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DefaultAction { get; set; }
        public string DefaultController { get; set; }
        public string DefaultArea { get; set; }
        public List<MenuModel> MenuList { get; set; }
        public List<RoleLinkModel> LinkList { get; set; }
    }
    public class MenuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class RoleLinkModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentLinkId { get; set; }
        public string LinkName { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int OrderNumber { get; set; }
        public string IconClass { get; set; }
    }
    public class DistrictModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
    }
    public class AirportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
         
    }

    public class CollegeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
    }
    
    #endregion

    #region User Model
    public class LoginModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Catpcha is required")]
        public string CaptchaText { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string MobileNumber { get; set; }
    }
    #endregion

    

    #region UserProfile
    public class UserProfileModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int MandalId { get; set; }
        public string MandalName { get; set; }
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public string DOJ { get; set; }
    }

    #endregion

    #region Payment  model
    public class PaymentModel
    {
        public bool HasAppliedforAPMCE { get; set; }
        public bool HasAppliedforPCPNDT { get; set; }
        public bool HasAppliedforBloodBank { get; set; }
        public int PCPNDT { get; set; }
        public int APMCE { get; set; }
        public int BloodBank { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicationNumber { get; set; }
        [Display(Name = "Grant Total")]
        public int SubTotal { get; set; }
        public int TotalCount { get; set; }

        [Display(Name = " Basic Specialty")]
        public string BasicSpecialty { get; set; }
        [Display(Name = "Physiotherapy  Centers")]
        public string PhysiotherapyCenters { get; set; }
        [Display(Name = "Diagnostic Centers (Basic Lab facilities)")]
        public string DiagnosticCenters { get; set; }
        [Display(Name = "Diagnostic Centers with Hi-end equipment")]
        public string DiagnosticCentersHiequipment { get; set; }

        [Display(Name = "Single Facility")]
        public string SingleFacilities { get; set; }
        [Display(Name = "Multiple Facilities")]
        public string MultipleFacilities { get; set; }

        public RazorPayModel RazorPay { get; set; }
        
        public PaymentModel()
        {
            RazorPay = new RazorPayModel();
        }
    }

    public class RazorPayModel
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountInSubUnits => Amount * 100;
        public string userName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
    }

    #endregion
    

    
  

     
}
