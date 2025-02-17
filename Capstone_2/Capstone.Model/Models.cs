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
    public class QualificationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ActType { get; set; }
    }
    public class DocumentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DocumentType DocumentTypeId { get; set; }
        public string DocumentPath { get; set; }
        public int CreatedUserId { get; set; }
        public int LastModifiedUserId { get; set; }

    }
    public class MasterServiceModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Service name is Required")]
        public string Name { get; set; }
        public string ActType { get; set; }

    }
    public class DepartmentModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Name is required")]
        public string Name { get; set; }
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }
        [Display(Name = "House No.")]
        public string HouseNumber { get; set; }
        [Display(Name = "PIN Code")]
        public string PinCode { get; set; }
        [Required(ErrorMessage = "Please Upload Logo")]
        [Display(Name = "Logo Path")]
        public string Logo { get; set; }
        // [Required(ErrorMessage = "Please Upload Logo")]
        // public HttpPostedFile FileUpload { get; set; }
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
    public class MandalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DistrictId { get; set; }
    }
    public class VillageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int MandalId { get; set; }
    }
    public class DesignationModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Designation Name is required")]
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedUserId { get; set; }
        public int LastModifiedUserId { get; set; }
    }
    public class OwnershipTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class InstitutionTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class DoctorSpecialityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class FacilityMasterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InspectionPartialViewName { get; set; }
    }
    public class OfferedServiceMasterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OfferedServiceEquipmentsMasterModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Hospital Type")]
        public int HospitalTypeId { get; set; }
        public int  EquipmentId { get; set; }
        [Display(Name = "Equipments")]
        public string EquipmentIds { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
    }

    public class HospitalTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class EquipmentTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HospitalTypeId { get; set; }
        public bool IsActive { get; set; }
    }
    public class CentresModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public bool IsActive { get; set; }
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

    #region License Models
    public class LicenseQuestionnaireModel
    {
        public bool HasAppliedforAPMCE { get; set; }
        public bool HasAppliedforPCPNDT { get; set; }
        //public bool HasAppliedforPCPNDT11 { get; set; }
        public bool HasAppliedforBloodBank { get; set; }
        //public bool HasAppliedforBloodBank11 { get; set; }
        public bool HasAppliedforBloodBankForm27E { get; set; }
        //public bool HasAppliedforBloodBankForm27E11 { get; set; }
        public bool HasAppliedforDrugStore { get; set; }
        public bool HasAppliedforBioCapstone { get; set; }

        public bool HasAppliedforAllopathicDrugStoreForm19 { get; set; }
        public bool HasAppliedforAllopathicDrugStoreForm19C { get; set; }

        public bool HasAppliedforAyurvedaDrugStore { get; set; }
        public bool HasAppliedforHomeopathicDrugStore { get; set; }
        public bool HasAppliedforUnaniDrugStore { get; set; }
        public bool HasAppliedforOrganTransplantation { get; set; }
        //public bool HasAppliedforOrganTransplantation11 { get; set; }

        public ApplicationModel ApplicationModel { get; set; }

        public LicenseQuestionnaireModel()
        {
            this.ApplicationModel = new ApplicationModel();
        }
    }
    #endregion

    #region Amendment Models
    public class PCPNDTAmendmentModel
    {
        public PCPNDTViewModel PCPNDTModel { get; set; }
        public bool FacilityAmendment { get; set; }
        public bool OwnershipTypeAmendment { get; set; }
        public bool InstitutionAmendment { get; set; }
        public bool TestsProceduresAmendment { get; set; }
        public bool EquipmentAmendment { get; set; }
        public bool FacilitiesAmendment { get; set; }
        public bool EmployeesAmendment { get; set; }
        public bool LicenseCancellation { get; set; }
        public bool ApplyforNOCofEquipment { get; set; }
    }

    public class APMCEAmendmentModel
    {
        public APMCEViewModel APMCEModel { get; set; }
        public bool LicenseCancellation { get; set; }

        public bool HospitalAmendment { get; set; }
        public bool CorrespondentAmendment { get; set; }
        public bool AccomodationAmendment { get; set; }
        public bool TrustAmendment { get; set; }
        public bool ServicesAmendment { get; set; }
        public bool EmployeeAemndment { get; set; }
        public bool SpecialtiesAvailableAmendment { get; set; }
        public bool EquipmentFurnitureAmendment { get; set; }
        public bool LaborAmendment { get; set; }
        public bool OperationTheatreAmendment { get; set; }
        public bool DiagnosticCenterAmendment { get; set; }
    }
    public class RenewalModel
    {
        public PCPNDTViewModel PCPNDTModel { get; set; }
        public APMCEViewModel APMCEModel { get; set; }
        public PCPNDTLicenseInfoModel PCPNDTLicenseModel { get; set; }
        public APMCERenewalModel APMCERenewalModel { get; set; }
        public int TransactionId { get; set; }
        public string Type { get; set; }

    }
    public class AppealModel
    {
        public APMCEViewModel APMCEModel { get; set; }
        public PCPNDTViewModel PCPNDTModel { get; set; }
        public int CreatedUserId { get; set; }
    }
    public class ResubmitModel
    {
        public PCPNDTViewModel PCPNDTModel { get; set; }
        public string ExistingApplicationNo { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public ResubmitModel()
        {
            this.PCPNDTModel = new PCPNDTViewModel();
        }

    }
    public class AmendmentModel
    {
        public PCPNDTAmendmentModel PCPNDTAmendments { get; set; }

        public APMCEAmendmentModel APMCEAmendments { get; set; }

        // Old
        public bool HasAppliedforCorrespondent { get; set; }
        public bool HasAppliedforAccomodation { get; set; }
        public bool HasAppliedforServices { get; set; }
        public bool HasAppliedforEmployee { get; set; }
        public bool HasAppliedforSpecialtiesAvailable { get; set; }
        public bool HasAppliedforEquipmentFurniture { get; set; }
        public bool HasAppliedforLabor { get; set; }
        public bool HasAppliedforOperationTheatre { get; set; }
        public bool HasAppliedforDiagnosticCenter { get; set; }
        public CorrespondingAddressViewModel CorrespondingAddressViewModel { get; set; }
        public TrustViewModel TrustViewModel { get; set; }
        public InfraStructureModel InfraStructureModel { get; set; }


        public bool HasAppliedforRegistration { get; set; }
        public bool HasAppliedforFacilities { get; set; }
        public bool HasAppliedforFacilityAddress { get; set; }
        public bool HasAppliedforOwnership { get; set; }
        public bool HasAppliedforInstitution { get; set; }
        public bool HasAppliedforProcedures { get; set; }
        public bool HasAppliedforEquipment { get; set; }
        public bool HasAppliedforFacilitiesavailable { get; set; }
        public bool HasAppliedforEmployeeDetails { get; set; }
        public bool HasAppliedforCancellation { get; set; }
        public ApplicantViewModel ApplicantModel { get; set; }
        public TestsModel TestsModel { get; set; }
        public EquipmentModel EquipmentModel { get; set; }
        public FacilitesModel FacilitiesModel { get; set; }
        public FacilityViewModel FacilityModel { get; set; }
        public EmployeeViewModel EmployeeModel { get; set; }
        public List<EmployeeViewModel> EmployeeList { get; set; }
        public List<EquipmentModel> EquipmentList { get; set; }

        public PCPNDTViewModel PCPNDTModel { get; set; }
        public InstitutionViewModel InstitutionModel { get; set; }
        public DeclarationModel DeclarationModel { get; set; }
        public int TransactionId { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
        public AmendmentModel()
        {
            this.CorrespondingAddressViewModel = new CorrespondingAddressViewModel();
            this.TrustViewModel = new TrustViewModel();
            this.InfraStructureModel = new InfraStructureModel();
            this.FacilityModel = new FacilityViewModel();
            this.EmployeeModel = new EmployeeViewModel();
            this.PCPNDTModel = new PCPNDTViewModel();
            this.ApplicantModel = new ApplicantViewModel();
            this.TestsModel = new TestsModel();
            this.EquipmentModel = new EquipmentModel();
            this.FacilitiesModel = new FacilitesModel();
            this.InstitutionModel = new InstitutionViewModel();
            this.DeclarationModel = new DeclarationModel();
            // APMCE Amendments
            this.APMCEModel = new APMCEViewModel();

        }

        public APMCEViewModel APMCEModel { get; set; }

    }

    #endregion

    #region Application & Transaction Models
    public class ApplicationModel
    {
        public UserModel usermodel { get; set; }
        public int TransactionId { get; set; }
        public int Id { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime SubmittedOn { get; set; }
        public Status Status { get; set; }
        public APMCEViewModel APMCEModel { get; set; }
        public PCPNDTViewModel PCPNDTModel { get; set; }
        public ExistingPCPNDTViewModel ExistingPCPNDTModel { get; set; }
        public BloodBankViewModel BloodBankModel { get; set; }
        public BloodBankViewModel BloodBankForm27EModel { get; set; }
        public HomeopathyDrugStoreViewModel HomeopathyDrugStore { get; set; }
        public AllopathicDrugStoreViewModel AllopathicDrugModel { get; set; }
        public string ExistingApplicationNumber { get; set; }
        public ApplicationType ApplicationType { get; set; }

        public int ExistingApplicationId { get; set; }
        public BioCapstoneViewModel BioCapstoneModel { get; set; }
        public OrganTransplantViewModel OrganTransplantModel { get; set; }
        public ApplicationModel()
        {
            this.APMCEModel = new APMCEViewModel();
            this.PCPNDTModel = new PCPNDTViewModel();
            this.BloodBankModel = new BloodBankViewModel();
            this.BloodBankForm27EModel = new BloodBankViewModel();
            this.HomeopathyDrugStore = new HomeopathyDrugStoreViewModel();
            this.AllopathicDrugModel = new AllopathicDrugStoreViewModel();
            this.BioCapstoneModel = new BioCapstoneViewModel();
            this.OrganTransplantModel = new OrganTransplantViewModel();
            this.AllopathicDrugModel = new AllopathicDrugStoreViewModel();
            this.ExistingPCPNDTModel = new ExistingPCPNDTViewModel();
        }
    }
    public class TransactionModel
    {
        public string ServiceType { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int CurrentDesignationId { get; set; }
        public int DistrictId { get; set; }
        public int MandalId { get; set; }
        public int VillageId { get; set; }
        public Status Status { get; set; }
    }
    // added Pj

    public class ApplicationTrackModel
    {
        public bool HasTrackforAPMCE { get; set; }
        public bool HasTrackforPCPNDT { get; set; }
        public bool HasTrackForBloodBank27C { get; set; }
        public bool HasTrackForBloodBank27E { get; set; }
        public PCPNDTRejection PCPNDTRejection { get; set; }
        public int ApplicationId { get; set; }
        public TransactionTrackModel PCPNDTTrackModel { get; set; }
        public TransactionTrackModel APMCETrackModel { get; set; }
        public TransactionTrackModel BloodBankForm27CModel { get; set; }
        public TransactionTrackModel bbForm27EModel { get; set; }
    }

    public class TransactionTrackModel
    {
        //public TransactionGraphicalModel GraphicalModel { get; set; }
        public List<DesignationModel> GraphicalListModel { get; set; }
        public List<TransactionHistoryModel> TransactionHistory { get; set; }
        public int CurrentDesignationId { get; set; }
        public string ServiceName { get; set; }
    }

    public class TransactionGraphicalModel
    {
        // used for Graphical show        
    }

    public class TransactionHistoryModel
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
    }
    public class ApprovalsModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int AmendmentId { get; set; }
        public int UserId { get; set; }
        public Status status { get; set; }
        [Required(ErrorMessage = "Remarks Required")]
        public string Remarks { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectionFile { get; set; }
        public List<InspectionModel> InspectionList { get; set; }

    }
    #endregion

    #region APMCE Models
    public class RegistrationModel
    {
        public int Id { get; set; }
        // [Required(ErrorMessage = "Facility Type  is required")]
        [Display(Name = "Facility Type")]
        public string FacilityType { get; set; }
        //[Required(ErrorMessage = "Hospital Name  is required")]

        [Display(Name = "Hospital Type")]
        public int HospitalTypeId { get; set; }

        [Display(Name = "Clinic Type")]
        public string ClinicType { get; set; }

        [Display(Name = "Bed Strength")]
        public string BedStrength { get; set; }

        [Display(Name = "Speciality")]
        public string Speciality { get; set; }

        [Display(Name = "Hospital Name")]
        public string Name { get; set; }
        [Display(Name = "License")]
        public string LicenseNumber { get; set; }
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        // [Required(ErrorMessage = "Street  is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        // [Required(ErrorMessage = "House Number  is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //  [Required(ErrorMessage = "PIN Code  is required")]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public FormStatus FormStatus { get; set; }
        public int CreatedUserId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }

        public string ApplicantPhoto { get; set; }
        [Display(Name = "Aadhar Card")]
        public string AadharCardPath { get; set; }
        [Display(Name = "PAN Card")]
        public string PANCardPath { get; set; }
        public string HospitalPhoto { get; set; }
        public string TariffBoardPhoto { get; set; }
        public string BioCapstoneWaste { get; set; }
        public string ConsultantsListPhoto { get; set; }
        public string FireExtinguisherPhoto { get; set; }
        public string FireNOC { get; set; }
        public int AmendmentSNo { get; set; }
        [Display(Name = "Building Height")]
        public int BuildingHeight { get; set; }
    }
    public class CorrespondingAddressModel
    {
        public int Id { get; set; }
        // [Required(ErrorMessage = "Please enter a Name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Pleasee select a District")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        // [Required(ErrorMessage = "Pleasee select a Mandal")]
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        // [Required(ErrorMessage = "Pleasee select a Village")]
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        // [Required(ErrorMessage = "House Number is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //  [Required(ErrorMessage = "Street Name is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        // [Required(ErrorMessage = "PIN Code is required")]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public FormStatus FormStatus { get; set; }
        public int CreatedUserId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }


        
        [Display(Name = "Aadhar Card")]
        public string AadharCardPath { get; set; }
        [Display(Name = "PAN Card")]
        public string PANCardPath { get; set; }
        public string CorrespondentPhoto { get; set; }
        public string EducationCertificate { get; set; }
        public string IMAMembership { get; set; }
        public string TAXReceipt { get; set; }
        //public string ApplicantPhoto { get; set; }
        public string THANA_APNA_Registration { get; set; }
        public string APMCR_TSMCR_MCI { get; set; }
        public string CorrespondentSignature { get; set; }
    }
    public class TrustModel
    {
        public int Id { get; set; }
        // [Required(ErrorMessage = "Please enter a Name")]
        public string Name { get; set; }
        // [Required(ErrorMessage = "Please select a District")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Please select a Mandal")]
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        // [Required(ErrorMessage = "Please select a Village")]
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        //[Required(ErrorMessage = "House No is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        // [Required(ErrorMessage = "Street is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        // [Required(ErrorMessage = "PIN Code is required")]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        // [Required(ErrorMessage = "Establishment Date is required")]
        [Display(Name = "Establishment Date")]
        public DateTime EstablishedDate { get; set; }
        public FormStatus FormStatus { get; set; }
        public int CreatedUserId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        
    }

    public class AccommodationModel
    {
        public int Id { get; set; }
        // [Required(ErrorMessage = " Select Establishment Type")]
        [Display(Name = "Establishment Owned by")]
        public string EstablishementType { get; set; }
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        public string UploadedFilePath { get; set; }
        public FormStatus FormStatus { get; set; }
        public int UserId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class EstablishmentModel
    {
        public int Id { get; set; }
        [Display(Name = "Establishment Date")]
        public DateTime EstablishmentDate { get; set; }
        [Required(ErrorMessage = "Open Area is required")]
        [Display(Name = "Open Area")]
        public decimal OpenArea { get; set; }
        [Required(ErrorMessage = "Construction Area is required")]
        [Display(Name = "Construction Area")]
        public decimal ConstructionArea { get; set; }
        // [Required(ErrorMessage = "OpenAreaFilePath is required")]
        public string OpenAreaFilePath { get; set; }
        // [Required(ErrorMessage = "ConstructionAreaFilePath is required")]
        public string ConstructionAreaFilePath { get; set; }
        public FormStatus FormStatus { get; set; }
        public int CreatedUserId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class OfferedServicesModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Bed Strength")]
        [Display(Name = "Bed Strength")]
        public int BedStrength { get; set; }
        [Display(Name = "Offered Services")]
        [Required(ErrorMessage = "Please select at least 1 Service")]
        public string OfferedServices { get; set; }
        public string OfferedServiceIDs { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public int AmendmentSNo { get; set; }
    }
    public class StaffDetailsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        [Display(Name = "Designation")]
        public string StaffDesignation { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Mobilenumber must be numeric")]
        [StringLength(10, ErrorMessage = "Mobile Number should be 10 digits", MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Display(Name = "Speciality")]
        public string Speciality { get; set; }


        //public int SpecialtyId { get; set; }
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please upload a file")]
        public string UploadedFilePath { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class InfraStructureModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Select Equipment Name")]
        [Display(Name = "Equipment Name")]
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Model")]
        [Required(ErrorMessage = "Please enter Model")]
        public string ItemModel { get; set; }
        [Required(ErrorMessage = "Please enter Remarks")]
        public string Remarks { get; set; }

        //[Required(ErrorMessage = "Please upload a file")]
        [Display(Name = "Upload")]
        public string UploadedFilePath { get; set; }
        public FormStatus FormStatus { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUserId { get; set; }
    }
    public class FacilitiesAvailableModel
    {
        public int Id { get; set; }
        [Display(Name = "Labor room with pediatriccare Facility")]
        public bool HasLaborRoom { get; set; }
        [Display(Name = "Operation Theater")]
        public bool HasOperationTheater { get; set; }
        [Display(Name = "Diagnostic Facilities includeing clinical loboratory and Imaging facilities")]
        public bool HasDiagnosticFacility { get; set; }
        [Display(Name = "Declaration Stamp")]
        public bool HasDeclarationStamp { get; set; }
        [Display(Name = "Declaration Non Judicial Stamp Paper with notarized")]
        public string DeclarationStampFilePath { get; set; }
        public bool AnyOtherInformation { get; set; }
        [Display(Name = "Other Information")]
        public string OtherInformationDescription { get; set; }
        public string OtherInformationDocumentPath { get; set; }
        public int UserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public int EnclosureCnt { get; set; }

        public int OperationTheatreCount { get; set; }
    }

    public class AdditionalDocumentsModel
    {
        public int Id { get; set; }

        [Display(Name = "Bio-Capstone Wastage Clearance form from Telangana Government")]
        public string BioCapstoneWastageClearanceFromFilePath { get; set; }

        [Display(Name = "Pollution Authority Letter by PCB(Pollution Control Board)")]
        public string PollutionAuthorityLetterByPCBFilePath { get; set; }

        [Display(Name = "For above 25 beds need to upload CFO(Consent for operation)")]
        public string CFOFilePath { get; set; }

        [Display(Name = "For above 25 beds need to upload STP Document ")]
        public string STPFilePath { get; set; }

        [Display(Name = "FE Report ")]
        public string FEReportFilePath { get; set; }

        [Display(Name = "Tariff List")]
       // [Required(ErrorMessage = "Please upload a tariff file")]
        public string TarifListFilePath { get; set; }

        //[Required(ErrorMessage = "Please upload Building & Building plan file")]
        [Display(Name = "Height of the Building & Building plan Upload")]
        public string Establishment_BuildingPlanFilepath { get; set; }

        [Display(Name = "Hospital - Name plate With Building")]
        public string HospitalOutSideNamePlateBuildingFilePath { get; set; }

        [Display(Name = "Hospital -Tarif Board")]
        public string TariffBoardFilePath { get; set; }

        [Display(Name = "Hospital-Fire Exhaustive")]
        public string FireExhaustiveFilePath { get; set; }

        [Display(Name = "Hospital / Lab / Operation Theater")]
        public string HospitalLabOperationTheaterFilePath { get; set; }
        public int UserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public int EnclosureCnt { get; set; }
        [Display(Name = "Valid Upto")]
        public DateTime BioCapstoneValidupto { get; set; }
        [Display(Name = "Valid Upto")]
        public DateTime PollutionAuthorityValidupto { get; set; }
        [Display(Name = "Valid Upto")]
        public DateTime FireNOCValidupto { get; set; }
    }
    public class APMCEAckModel
    {
        public string ApplicationType { get; set; }
        public string ApplicantNameAddress { get; set; }
        public string IssuingAuthority { get; set; }
        public string AppropriateAuthority { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ReceivedPlace { get; set; }
    }
    public class APMCERejection
    {
        public string ApplicationType { get; set; }
        public string FacilityNameAddress { get; set; }
        public string ReasonsOfRejection { get; set; }
        public string AppropriateAuthority { get; set; }
    }
    public class APMCERenewalModel
    {
        public string ApplicationNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicantNameAddress { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime RenewalValidDate { get; set; }
        public List<string> ServiceDetails { get; set; }
        public string District { get; set; }
        public APMCERenewalModel()
        {
            this.ServiceDetails = new List<string>();
        }
    }
    #endregion

    #region PCPNDT Models
    public class ApplicantModel
    {
        public int Id { get; set; }
        // [Required(ErrorMessage = "Please enter Name")]
        //@"^[a-zA-Z]+$"
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please select Role")]
        [Display(Name = "Role")]
        public string ApplicantRole { get; set; }
        [Display(Name = "Role (Other)")]
        //[Required(ErrorMessage = "Please enter Role (Other)")]
        public string ApplicantRoleOther { get; set; }
        [Display(Name = "District")]
        //[Required(ErrorMessage = "Please select District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        // [Required(ErrorMessage = "Please Enter Mobile")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Mobilenumber must be numeric")]
        [StringLength(10, ErrorMessage = "Mobile Number should be 10 digits", MinimumLength = 10)]
        public string MobileNo { get; set; }

        [StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        public string Aadhar { get; set; }

        [RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        [StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PAN { get; set; }
        //[Required(ErrorMessage = "Please enter H. No.")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "Please enter Street")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }

        //[Required(ErrorMessage = "Please enter PIN Code")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PINCode")]
        public string PINCode { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public string OwnershipType { get; set; }
        [Required(ErrorMessage = "Please select File Type")]
        public string FileType { get; set; }
        [Required(ErrorMessage = "Please Upload Document")]
        public string ApplicantUpload { get; set; }
        public string UploadDocument { get; set; }
        public string ApplicantPhoto { get; set; }
        public List<DocumentUploadModel> uploadedDocuments { get; set; }

        [Display(Name = "Aadhar Card")]
        public string AadharCardPath { get; set; }
        [Display(Name = "PAN Card")]
        public string PANCardPath { get; set; }
        public ApplicantModel()
        {
            this.uploadedDocuments = new List<DocumentUploadModel>();
        }
    }

    public class FacilityModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please select District")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Please select Mandal")]
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        //[Required(ErrorMessage = "Please select Village")]
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        //[Required(ErrorMessage = "Street Name is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        //[Required(ErrorMessage = "House Number is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "PIN Code")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PINCode")]
        public string PINCode { get; set; }
        //[Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, ErrorMessage = "Phone number should be 10 digits", MinimumLength = 10)]
        public string Phone { get; set; }
        //[Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Telegraph { get; set; }
        public string Telex { get; set; }
        //[Required(ErrorMessage = "Please select at least one Facility")]
        public string Faclities { get; set; }
        public string AddressProofType { get; set; }

        [Display(Name = "Address Proof")]
        public string AddressProofPath { get; set; }

        [Display(Name = "Building Layout")]
        public string BuildingLayoutPath { get; set; }

        public string OwnershipType { get; set; }
        [Display(Name = "Facility Ownership")]
        public string OwnerShipPath { get; set; }

        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class TestsModel
    {
        public int Id { get; set; }
        public string InvasiveTests { get; set; }
        public string NonInvasiveTests { get; set; }
        public string Remarks { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class EquipmentModel
    {
        public int Id { get; set; }
        public int EquipmentLogId { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please enter Make")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Please enter Model")]
        [Display(Name = "Model")]
        public string MachineModel { get; set; }
        //[Required(ErrorMessage = "Please enter Serial No.")]
        [Display(Name = "Serial No.")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Please enter Type")]
        public string Type { get; set; }
        [Display(Name = "Upload File")]
        //[Required(ErrorMessage = "Please upload a file")]
        public string UploadedFilePath { get; set; }
        [Display(Name = "NOC")]
        public string NocFilePath { get; set; }
        [Display(Name = "Transfer Certificate")]
        public string TransferCertificatePath { get; set; }
        [Display(Name = "Installation Certificate")]
        public string InstallationCerticatePath { get; set; }
        [Display(Name = "Invoice File")]
        public string InvoicePath { get; set; }
        [Display(Name = "Photo")]
        public string ImagePath { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public FormStatus FormStatus { get; set; }

    }

    public class NOCforquipmentModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int AmendmentId { get; set; }
        public string Type { get; set; }
        [Display(Name = "Equipment")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please Select Equipment")]
        public int EquipmentId { get; set; }
        public string DistrictName { get; set; }
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        public string EquipmentName { get; set; }

        [Display(Name = "Other State")]
        public string OtherState { get; set; }
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public string LicenseNumber { get; set; }
        public string Address { get; set; }
        public string Date { get; set; }
        public string DistrictState { get; set; }
        public string FacilityName { get; set; }
        public EquipmentModel EquipmentDetails { get; set; }

    }
    public class FacilitesModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Tests { get; set; }
        public string Studies { get; set; }
        public string Remarks { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class EmployeeModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter Name")]
        public int EmployeeLogId { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Select Designation")]
        [Display(Name = "Sub Designation")]
        public string SubDesignation { get; set; }
        public int DesignationId { get; set; }
        public int QualificationId { get; set; }
        public string QualificationName { get; set; }
        //[Required(ErrorMessage = "Please enter Experience")] 
        public string Experience { get; set; }
        // [Required(ErrorMessage = "Enter Years")]
        [Range(0, 100, ErrorMessage = "Value can't be more than 100")]
        public int ExpYears { get; set; }
        // [Required(ErrorMessage = "Enter Months")]
        [Range(0, 12, ErrorMessage = "Value can't be more than 12")]
        public int ExpMonths { get; set; }
        //[Required(ErrorMessage = "Enter Days")]
        [Range(0, 31, ErrorMessage = "Enter number between 0 to 31")]
        public int ExpDays { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
        [Display(Name = "Reg. No.")]
        //[Required(ErrorMessage = "Please enter Reg. No.")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Upload File")]
        //[Required(ErrorMessage = "Please upload file")]
        public string UploadedFilePath { get; set; }
        [Required(ErrorMessage = "Please upload files")]
        public List<DocumentUploadModel> UploadDocuments { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public FormStatus FormStatus { get; set; }

        public EmployeeModel()
        {
            this.UploadDocuments = new List<DocumentUploadModel>();
        }

    }
    public class TechnicalModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        public int QualificationId { get; set; }
        [Required(ErrorMessage = "Please Select Qulification")]
        public string Qualification { get; set; }
        //[Required(ErrorMessage = "Please enter Experience")] 
        public string Experience { get; set; }
        [Required(ErrorMessage = "Enter Years")]
        [Range(0, 100, ErrorMessage = "Value can't be more than 100")]
        public int ExpYears { get; set; }
        [Required(ErrorMessage = "Enter Months")]
        [Range(0, 12, ErrorMessage = "Value can't be more than 12")]
        public int ExpMonths { get; set; }
        [Required(ErrorMessage = "Enter Days")]
        [Range(0, 31, ErrorMessage = "Value can't be more than 31")]
        public int ExpDays { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Responsibility is required")]
        public string Responsibility { get; set; }
        [Required(ErrorMessage = "Upload Documents is required")]
        public List<DocumentUploadModel> UploadDocuments { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public FormStatus FormStatus { get; set; }
        public TechnicalModel()
        {
            this.UploadDocuments = new List<DocumentUploadModel>();
        }
    }
    public class InstitutionModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please select Ownership")]
        [Display(Name = "Ownership")]
        public int OwnershipTypeId { get; set; }
        //[Required(ErrorMessage = "Please select Institution")]
        [Display(Name = "Institution")]
        public int InstitutionTypeId { get; set; }
        //[Required(ErrorMessage = "Please enter Total Work Area")]
        // [Display(Name = "Total Work Area")]
        public decimal TotalWorkArea { get; set; }
        public string AffidavitDocPath { get; set; }
        public string ArticleDocPath { get; set; }
        public string InstitutionOthers { get; set; }
        public string OwnershipOthers { get; set; }
        public List<DocumentUploadModel> StudyCertificateDocPaths { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class DeclarationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // [Required(ErrorMessage = "SonOf is required")]
        public string SonOf { get; set; }
        // [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        //[Required(ErrorMessage = "ResidentOf is required")]
        public string ResidentOf { get; set; }
        // [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }
        public string Organization { get; set; }
        public DateTime Date { get; set; }
        //[Required(ErrorMessage ="Place is required")]
        public string Place { get; set; }
        // [Required(ErrorMessage = "Signature is required")]
        public string Signature { get; set; }
        public int EnclosureCnt { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public int OtherUploadFileTypeId { get; set; }
        public string OtherUploadName { get; set; }
        public string OtherUploadsDocument { get; set; }
        public string OtherUploadText { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Upload Signature")]
        public string SignatureDocPath { get; set; }
        public DocumentUploadModel DocumentUploadModel { get; set; }
        public List<DocumentUploadModel> OtherUploadsList { get; set; }
    }
    public class PCPNDTAckModel
    {
        public string ApplicationType { get; set; }
        public string Facilities { get; set; }
        public string ApplicantNameAddress { get; set; }
        public string IssuingAuthority { get; set; }
        public string AppropriateAutority { get; set; }
        public DateTime ReceivedDate { get; set; }

    }
    public class BloodBankAckModel
    {
        public int Id { get; set; }
        public string ApplicationType { get; set; }
        public string FormName { get; set; }
        public string BloodBankForm { get; set; }
        public string NameAddress { get; set; }
        public DateTime Date { get; set; }
    }
    public class PCPNDTRejection
    {
        public string ApplicationType { get; set; }
        public string Facilities { get; set; }
        public string FacilityNameAddress { get; set; }
        public string IssuingAuthority { get; set; }
        public string AppropriateAuthority { get; set; }
        public string ReasonsOfRemarks { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ReceivedPlace { get; set; }
    }
    public class PCPNDTLicenseInfoModel
    {
        public string IssuingAuthority { get; set; }
        public string AppropriateAuthority { get; set; }
        public string Facilities { get; set; }
        public string ApplicantNameAddress { get; set; }
        public string TestRemarks { get; set; }
        public string FacilitiesRemarks { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseIssuedDate { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public List<string> InvasiveTests { get; set; }
        public List<string> NonInvasiveTests { get; set; }
        public List<string> Tests { get; set; }
        public List<string> Studies { get; set; }
        public List<EquipmentModel> EquipmentList { get; set; }
        public List<EmployeeViewModel> EmployeeList { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ServiceId { get; set; }
    }
    #endregion

    #region BloodBank Models
    public class BloodBankApplicantModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter Owner Name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please select Role")]
        [Display(Name = "Role")]
        public string ApplicantRole { get; set; }
        [Display(Name = "Role (Other)")]
        //[Required(ErrorMessage = "Please enter Role (Other)")]
        public string ApplicantRoleOther { get; set; }
        [Display(Name = "District")]
        //[Required(ErrorMessage = "Please select District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        [StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        //[Required(ErrorMessage = "Please enter Aadhar Number")]
        public string Aadhar { get; set; }
        //[Required(ErrorMessage = "Please enter PAN ")]
        [RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        [StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PAN { get; set; }
        //[Required(ErrorMessage = "Please enter H. No.")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "Please enter Street")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }

        //[Required(ErrorMessage = "Please enter PIN Code")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        //[Required(ErrorMessage = "Ownership Type is required")]
        public string OwnershipType { get; set; }
        //[Required(ErrorMessage = "Upload Document is required")]
        public string UploadDocument { get; set; }

    }
    public class BloodBankEstablishmentModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Display(Name = "District")]
        //[Required(ErrorMessage = "Please select District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }
        //[Required(ErrorMessage = "Street is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        [Display(Name = "H. No.")]
        //[Required(ErrorMessage = "House Number is required")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "Address Proof is required")]
        [Display(Name = "Address Proof")]
        public string AddressProofPath { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
    public class BloodBankListModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public int LastModifiedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
    }
    public class BloodBankAttachments
    {
        public int Id { get; set; }
        public DateTime InspectionDate { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateTime DeclareDate { get; set; }
        public string IdProffPath { get; set; }
        public string OwnerPremisesPath { get; set; }
        public string planPremisesPath { get; set; }
        public int CreatedUserId { get; set; }
        public string Place { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int EnclosureCnt { get; set; }
    }
    public class BloodBankNOCModel
    {
        public int Id { get; set; }
        public string ApplicationNo { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Name { get; set; }
        public string HospitalNameAndAddress { get; set; }
        public string BloodbankFrom { get; set; }
        public DateTime Date { get; set; }

    }
    #endregion
    #region Homeopathy Models
    public class HomeopathyEstablishment
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Display(Name = "Establishment Owned By")]
        public string OwnedBy { get; set; }
        [Display(Name = "District")]
        //[Required(ErrorMessage = "Please select District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        //[Required(ErrorMessage = "Please enter H. No.")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "Please enter Street")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        //[Required(ErrorMessage = "Please enter PIN Code")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public string Rent { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime ToDate { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string AddressProff { get; set; }
        public string RentalDocument { get; set; }
        public string PlanPremisesDocument { get; set; }
    }
    public class HomeopathyDeclaration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoveringLetter { get; set; }
        public DateTime Date { get; set; }
        public string Signature { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int CreatedUserId { get; set; }
    }
    #endregion
    #region Allopathic Models
    public class AllopathicPharmacy
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Display(Name = "Establishment Owned By")]
        public string OwnedBy { get; set; }
        [Display(Name = "District")]
        //[Required(ErrorMessage = "Please select District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        //[Required(ErrorMessage = "Please Enter Mobile")]
        // [RegularExpression("^[0-9]*$", ErrorMessage = "The Mobilenumber must be numeric")]
        //[StringLength(10, ErrorMessage = "Mobile Number should be 10 digits", MinimumLength = 10)]
        public string MobileNo { get; set; }
        //[Required(ErrorMessage = "Please enter H. No.")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        //[Required(ErrorMessage = "Please enter Street")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        //[Required(ErrorMessage = "Please enter PIN Code")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public string Rent { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fromdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ToDate { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string AddressProff { get; set; }
        public string RentalDocument { get; set; }
        public string PlanPremisesDocument { get; set; }
    }
    public class AllopathicDrugName
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        ////public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        ////public int LastModifiedUserId { get; set; }
        ////public FormStatus FormStatus { get; set; }
        ////public List<string> Drugs { get; set; }
    }
    public class AllopathicDeclaration
    {
        public int Id { get; set; }
        public bool HasAppliedSpecialStorage { get; set; }

        public string TextArea { get; set; }
        public string Designation { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Sign Required")]
        public string Sign { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
    #endregion

    #region Workflow Models
    public class ServiceModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Service Name is Required")]
        public int ServiceId { get; set; }
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Required Documents are mandatory")]
        public string RequiredDocId { get; set; }
        [Required(ErrorMessage = "Approval Documents are mandatory")]
        public string ApprovalDocId { get; set; }
        public bool HasFixedFee { get; set; }
        public bool HasDepartmentRaisedFee { get; set; }
        public bool HasAutogenerated { get; set; }
        [Required(ErrorMessage = "Please Enter Formula")]
        public string Formula { get; set; }

        [Required(ErrorMessage = "Please Enter Fee")]
        public int Fee { get; set; }
        public int CreatedUserId { get; set; }
        public int LastModifiedUserId { get; set; }


    }
    public class WorkFlowModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int WorkflowType { get; set; }
        [Required(ErrorMessage = "From Officer is Required")]
        public int FromRoleId { get; set; }
        [Required(ErrorMessage = "To Officer is Required")]
        public int ToRoleId { get; set; }
        public bool HasInspectionPrevilege { get; set; }
        public bool HasAutoSlidePrevilege { get; set; }
        [Required(ErrorMessage = "Please enter SLA")]
        public int SLA { get; set; }
        public bool HasApprovalPrevilege { get; set; }
        public bool HasRaisedQueryPrevilege { get; set; }
        public bool HasReturnPrevilege { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUserId { get; set; }
        public int LastModifiedUserId { get; set; }
    }
    #endregion 

    #region Query Response Models
    public class QueryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int QueryId { get; set; }
        public string Description { get; set; }
        public string UploadedFilePath { get; set; }
        public int TransactionId { get; set; }
        public string ApplicationType { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ResponseModel
    {
        public string Response { get; set; }
    }

    public class QueryRespondModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int QueryId { get; set; }
        public string Description { get; set; }
        public string UploadedFilePath { get; set; }
        public int TransactionId { get; set; }
        public string ApplicationType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Response { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
    #endregion

    #region License Cancel Model
    public class CancelLicenseModel
    {
        public string LicenseNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IsseuDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ExpireDate { get; set; }
        public string ApplicantName { get; set; }
        public string CenterName { get; set; }
        public int CreatedUserId { get; set; }
    }
    #endregion

    #region Organ Transplant models
    public class HospitalModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Hospital Name")]
        public int TransacionId { get; set; }
        public string HospitalName { get; set; }
        [Display(Name = "District")]
        [Required(ErrorMessage = "Please Select District")]
        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Please Select Mandal")]
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        [Required(ErrorMessage = "Please Select Village")]
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        [Required(ErrorMessage = "Please Enter StreetName")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Please Enter H. No.")]
        [Display(Name = "H. No.")]
        public string HouseNo { get; set; }
        [Display(Name = "PIN Code")]
        [Required(ErrorMessage = "Please Enter PIN Code")]
        public string Pincode { get; set; }
        [Display(Name = "Phone No.")]
        [Required(ErrorMessage = "Please Enter Phone No.")]
        public string PhoneNo { get; set; }
        [Display(Name = "Total Bed Strength")]
        [Required(ErrorMessage = "Please Enter Bed Strength")]
        public int TotalBedStrength { get; set; }
        [Display(Name = "Annual Budjet")]
        [Required(ErrorMessage = "Please Enter Anuual Budjet")]
        public int AnnualBudjet { get; set; }
        [Display(Name = "Name of Disciplines")]
        [Required(ErrorMessage = "Please Enter Name of Disciplines")]
        public string Nameofdisciplines { get; set; }
        [Display(Name = "Patient Turn Over Per Year")]
        [Required(ErrorMessage = "Please Enter Patient Turn Over")]
        public int PatientTurnoverPerYear { get; set; }
        [Display(Name = "Govt/Pvt")]
        [Required(ErrorMessage = "Please Select")]
        public string Govtorpvt { get; set; }
        [Display(Name = "Teaching/NonTeaching")]
        [Required(ErrorMessage = "Please Select")]
        public string TeacherNonTeach { get; set; }
        [Display(Name = "Road")]
        [Required(ErrorMessage = "Please Select")]
        public string ByRoad { get; set; }
        [Display(Name = "Rail")]
        [Required(ErrorMessage = "Please Select")]
        public string ByRail { get; set; }
        [Display(Name = "Air")]
        [Required(ErrorMessage = "Please Select")]
        public string ByAir { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }

    }
    public class SurgicalTeamModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        [Display(Name = "No. of Beds")]
        [Required(ErrorMessage = "Please Enter No. of Beds")]
        public int NumberofBeds { get; set; }
        [Display(Name = "No.of Operations Per Year")]
        [Required(ErrorMessage = "Please Enter  Operations")]
        public int NoofOperationsPerYear { get; set; }
        public int CreatedUserId { get; set; }
        public OTStaffDetailsModel StaffDetails { get; set; }
        public FormStatus FormStatus { get; set; }
        public List<OTStaffDetailsModel> StaffDetailsList { get; set; }
    }
    public class CapstoneTeamModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter  NO. of Beds")]
        [Display(Name = "NO. of Beds")]
        public int NoofBeds { get; set; }
        [Required(ErrorMessage = "Please Enter Patient Turnover")]
        [Display(Name = "Patient Turnover")]

        public int PatientTurnover { get; set; }
        [Required(ErrorMessage = "Please Enter Transplant Patients")]
        [Display(Name = "Transplant Patients")]
        public int TransplantPatients { get; set; }
        public OTStaffDetailsModel StaffDetails { get; set; }
        public List<OTStaffDetailsModel> StaffDetailsList { get; set; }
        public int UserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
    public class AnaesthesiologyModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Operation Theatres")]
        [Display(Name = "Operation Theatres")]
        public int OperationTheatres { get; set; }
        [Required(ErrorMessage = "Please Enter Emergency Operation Theatres")]
        [Display(Name = "Emergency Operation Theatres")]
        public int EmergencyOperationTheatres { get; set; }
        [Required(ErrorMessage = "Please Enter Transplant Operation Theatres")]
        [Display(Name = "Transplant Operation Theatres")]
        public int TransplantOperationTheatres { get; set; }
        public OTStaffDetailsModel StaffDetails { get; set; }
        public List<OTStaffDetailsModel> StaffDetailsList { get; set; }
        public OTOperationModel Operation { get; set; }
        public List<OTOperationModel> OperationsList { get; set; }
        public OTEquipmentModel Equipments { get; set; }
        public List<OTEquipmentModel> EquipmentsList { get; set; }
        public FormStatus FormStatus { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
    public class ICUHDUFacilities
    {
        public int Id { get; set; }
        public string IsICUHDUPresent { get; set; }
        [Required(ErrorMessage = "Please Enter ICUBeds")]
        public int ICUBeds { get; set; }
        [Required(ErrorMessage = "Please Enter Trained")]
        public int Trained { get; set; }
        [Required(ErrorMessage = "Please Enter Nurses")]
        public int Nurses { get; set; }
        [Required(ErrorMessage = "Please Enter Technicians")]
        public int Technicians { get; set; }
        public OTEquipmentModel Equipments { get; set; }
        public List<OTEquipmentModel> EquipmentsList { get; set; }
        [Required(ErrorMessage = "Please Enter Other Supportive Facilities")]
        [Display(Name = "Other Supportive Facilities")]
        public string OtherSupportiveFacilities { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
    public class LaboratoryFacilitiesModel
    {
        public int Id { get; set; }
        public OTStaffDetailsModel StaffDetailsModel { get; set; }
        public List<OTStaffDetailsModel> StaffDetailsList { get; set; }
        public OTEquipmentModel EquipmentModel { get; set; }
        public List<OTEquipmentModel> EquipmentsList { get; set; }
        [Required(ErrorMessage = "Please Enter Investigations")]
        public string Investigations { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
    public class ImagingServicesModel
    {
        public int Id { get; set; }
        public OTStaffDetailsModel StaffDetailsModel { get; set; }
        public List<OTStaffDetailsModel> StaffDetailsList { get; set; }
        public OTEquipmentModel EquipmentModel { get; set; }
        public List<OTEquipmentModel> EquipmentsList { get; set; }
        [Required(ErrorMessage = "Please Enter Investigations")]
        public string Investigations { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    public class HaematologyServiesModel
    {
        public int Id { get; set; }
        public OTStaffDetailsModel StaffDetailsModel { get; set; }
        public List<OTStaffDetailsModel> StaffDetailsList { get; set; }
        public OTEquipmentModel EquipmentModel { get; set; }
        public List<OTEquipmentModel> EquipmentsList { get; set; }
        [Required(ErrorMessage = "Please Enter Investigations")]
        public string Investigations { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    public class OrganTransplantFacilitiesModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Select")]
        [Display(Name = "BloodBank Facilities")]
        public bool BloodBankFacilities { get; set; }
        [Required(ErrorMessage = "Please Select")]
        [Display(Name = "Dialysis Facilities")]
        public bool DialysisFacilities { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Nephrologist { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Neurologist { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool NeuroSurgeon { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Urologist { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool GISurgeon { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Paediatrician { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Physicotherapist { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool SocialWorker { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Immunologist { get; set; }
        [Required(ErrorMessage = "Please Select")]
        public bool Cardiologist { get; set; }
        public int CreatedUserId { get; set; }

    }
    public class OTStaffDetailsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Select StaffType")]
        public int TransactionId { get; set; }
        public string StaffType { get; set; }
        [Required(ErrorMessage = "Please Enter Designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Please Enter No. of Members")]
        [Display(Name = "No. of Members")]
        public int NoOfMembers { get; set; }
        [Required(ErrorMessage = "Please Enter Organ")]
        public string Organ { get; set; }
        public string SectionName { get; set; }

        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class OTEquipmentModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Equipment Name")]
        [Display(Name = "Equipment Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter No. of Euipments")]
        [Display(Name = "No. of Equipments")]
        public int NoofEquipments { get; set; }
        public string SectionName { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
    }
    public class OTOperationModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter OperationName")]
        [Display(Name = "Operation Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Operations Performed")]
        [Display(Name = "No. of Operations")]
        public int OperationsPerformed { get; set; }
        public string SectionName { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }

    }

    #endregion 
    public class DocumentUploadModel
    {
        public int Id { get; set; }
        public string ReferenceTable { get; set; }
        public int ReferenceId { get; set; }
        public string DocumentPath { get; set; }
        public bool IsDeleted { get; set; }
        public int UploadedUserId { get; set; }
        public string UploadedUserName { get; set; }
        public string UploadType { get; set; }
        public int LastModifiedUserId { get; set; }
        public DateTime UploadedDate { get; set; }
    }
    public class DocumentTypeModel
    {
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
    }

    #region InspectionModels
    public class InspectionModel
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int DepartmentUserId { get; set; }
        public int TransactionId { get; set; }
    }

    #endregion

    //old
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    #region Transaction Models
    public class RaiseQueryModel
    {
        public int QueryId { get; set; }
        public int ServiceId { get; set; }
        public int RaisedBy { get; set; }
        public int RaisedTo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public int RespondedBy { get; set; }
    }
    public class ServiceModelold
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Please Enter Service Name")]
        public string ServiceName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Please Select Required Documents")]
        public string RequiredDocId { get; set; }
        [Required(ErrorMessage = "Please Select Approval Documents")]
        public string ApprovalDocId { get; set; }
        public bool FixedFee { get; set; }
        public bool DepartmentRaised { get; set; }
        public bool Autogenerated { get; set; }
        [Required(ErrorMessage = "Please Enter Formula")]
        public string Formula { get; set; }

        [Required(ErrorMessage = "Please Enter Fee")]
        public int Fee { get; set; }
        public int CreatedBy { get; set; }

        public int EntrepreneurId { get; set; }

    }
    public class WorkFlowModelold
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Please select From Officer")]
        public int FromRoleId { get; set; }
        [Required(ErrorMessage = "Please select To Officer")]
        public int ToRoleId { get; set; }
        public bool InspectionReport { get; set; }
        public bool AutoSlide { get; set; }
        [Required(ErrorMessage = "Please enter SLA")]
        public int SLA { get; set; }
        public bool IsApprovalAuthority { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ApplicantTransactionModel
    {
        public int TransactionId { get; set; }
        public int EnterpreneurId { get; set; }
        public int ServiceId { get; set; }
        public int StageId { get; set; }
        public string ApplicantId { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseIssueDate { get; set; }
        public string LicenseExpiryDate { get; set; }
        public string ApprovalAuthority { get; set; }
    }
    public class ApprovalStatusModel
    {
        public int ApprovalStatusId { get; set; }
        public int ApplicationTransactionId { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }

    }

    public class PCPNDTUserRegDetails
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter UserName")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Please Select District")]
        public int DistrictID { get; set; }

        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Please Select Mandal")]
        public int MandalID { get; set; }

        public string MandalName { get; set; }

        [Required(ErrorMessage = "Please Select Village")]
        public int VillageID { get; set; }

        public string VillageName { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string Pincode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class ApplicantdetailsModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Applicant Name")]
        public string ApplicantName { get; set; }

        [Required(ErrorMessage = "Please Select Role")]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        [Required(ErrorMessage = "Please Enter Aadhaar Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Aadhaar Number must be numeric")]
        [StringLength(12, ErrorMessage = "The Mobile must contains 10 digits", MinimumLength = 12)]
        public string AadhaarNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Pancard Number")]
        public string PancardNumber { get; set; }

        [Required(ErrorMessage = "Please Select District")]
        public int DistrictID { get; set; }

        [Required(ErrorMessage = "Please Select Mandal")]
        public int MandalID { get; set; }

        [Required(ErrorMessage = "Please Select Village")]
        public int VillageID { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }

        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }

        public string Pincode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class OwnershipdetailsModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Select District")]
        public int DistrictID { get; set; }

        [Required(ErrorMessage = "Please Select Mandal")]
        public int MandalID { get; set; }

        [Required(ErrorMessage = "Please Select Village")]
        public int VillageID { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
        public string Pincode { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter EmailID")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Mobilenumber must be numeric")]
        [StringLength(10, ErrorMessage = "The Mobile must contains 10 digits", MinimumLength = 10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please Select Ownership")]
        public int OwnershipID { get; set; }
        public int OwnershipTypeId { get; set; }

        public string OwnershipName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class FacilityDetailsModelOld
    {
        public int UserID { get; set; }

        public int FacilityId { get; set; }
        public int FacilityTypeID { get; set; }
        public string FacilityTypeName { get; set; }
        public string FacilityName { get; set; }
        public int DistrictID { get; set; }
        public int MandalID { get; set; }
        public int VillageID { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }

        public string Name { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please Enter Correct Email Address")]
        public string emailID { get; set; }
        public string Telephoneno { get; set; }

        public string Fax { get; set; }
        public string Telegraphic { get; set; }
        public string Telex { get; set; }

        public string Pincode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public int InstitutionID { get; set; }

        public string InstitutionType { get; set; }
        public string InstitutionName { get; set; }

        public int TotalWorkArea { get; set; }
    }

    public class RequiredDetailsForApprovalsModel
    {
        public int UserID { get; set; }
        public int TestfacilityID { get; set; }

        public int TestSubtypeID { get; set; }
        public bool Amniocentesis { get; set; }

        public bool BioChemical { get; set; }
        public bool ChorionicVilliAspiration { get; set; }

        public bool Chromosomal { get; set; }

        public bool molecularStudies { get; set; }

        public bool UltraSonography { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class EquipmentDetailsModel
    {
        public int UserID { get; set; }
        public int EquipmentID { get; set; }
        public string EquipName { get; set; }
        public string EquipMake { get; set; }
        public string EquipModel { get; set; }
        public int Quentity { get; set; }
        public string Remarks { get; set; }
        public int ReasonID { get; set; }

        public string ReasonDetails { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class FacilitiesAvailableDetailsModel
    {

        public int UserID { get; set; }
        public int FacilityTypeId { get; set; }
        public string FacilityTypeName { get; set; }

        public string Invasive { get; set; }

        public string NonInvasive { get; set; }
        public string Tests { get; set; }
        public string Studies { get; set; }
        public int FacilitySubTypeId { get; set; }
        public string FacilitySubTypeName { get; set; }
        public bool Ultrasound { get; set; }
        public bool Amniocentesis { get; set; }
        public bool Chorionicvilliaspiration { get; set; }
        public bool Foetoscopy { get; set; }
        public bool FoetalBiopsy { get; set; }
        public bool Cordocentesis { get; set; }

        public bool ChromosomalStudies { get; set; }

        public bool BiochemicalStudies { get; set; }
        public bool MolecularStudies { get; set; }
        public bool PreimplantationGenetic { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class EmployeeDetailsModel
    {
        public int UserID { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpQualification { get; set; }
        public string EmpExperience { get; set; }

        public string EmpRegistrationNo { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<EmployeeDetailsModel> EmployeeList { get; set; }
        public string Attachments { get; set; }

        public string fileDocName { get; set; }

        public string filepath { get; set; }
    }
    #endregion

    #region Custom Models
    public class DeleteWorkflowModel //using in DepartmentController -Mounika 
    {
        public List<RoleModel> RoleList { get; set; }
        public List<WorkFlowViewModel> WorkflowList { get; set; }
    }
    public class ApplicationsList
    {
        public int TransactionId { get; set; }
        public int RegistrationId { get; set; }
        public int EnterpreneurId { get; set; }
        public string EnterprenuerName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ApplicantId { get; set; }
        public int ApprovalStatusId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string FirmName { get; set; }
    }
    #endregion

    #region Questionaarie,Department,Approvlstatus,Service Details
    public class QuestionnaireModel
    {
        public int Id { get; set; }
        public int ConstitutionUnitId { get; set; }
        public int SectorEnterpriseId { get; set; }
        public string UnitName { get; set; }
        public int ProposedEmployement { get; set; }
        public int LocationDistrictId { get; set; }
        public int LocationMandalId { get; set; }
        public int LocationVillageId { get; set; }
        public int UnitLocationId { get; set; }
        public int AppliationTypeId { get; set; }
        public decimal TotalLandExtent { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal BuildingHeight { get; set; }
        public int LineofActivity { get; set; }
        public decimal LandValue { get; set; }
        public decimal BuildingValue { get; set; }
        public decimal PlantValue { get; set; }
        public decimal ProjectCost { get; set; }
        public string EnterpriseCategory { get; set; }
        public char PolutionCategory { get; set; }
        public int PowerRequirement { get; set; }
        public decimal WaterRequiredperDay { get; set; }
        public bool WaterfromBorewell { get; set; }
        public bool WaterfromHMWSSB { get; set; }
        public bool WaterfromReiversCanals { get; set; }
        public bool StoreKerosine { get; set; }
        public bool IsGeneratorReqired { get; set; }
        public bool FallinMunicipal { get; set; }
        public bool IsthereFellTrees { get; set; }
        public int NoofFellTrees { get; set; }
        public int NonExemptedTrees { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }


        public char Pol_Indus { get; set; }
        public int Application_Status { get; set; }
        public string Application_Number { get; set; }
        public string UID_NoNew { get; set; }


    }       //Added all columns from the database, siva

    public class DepartmentDetails
    {
        public int DeptId { get; set; }
        public string Dept_Name { get; set; }
        public int DistrictId { get; set; }
        public int UnderDeptId { get; set; }
        public string Dept_Full_Name { get; set; }
        public string Dept_Description { get; set; }
        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public int Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }

        public string Mobile_Number { get; set; }

        public string Email { get; set; }
        public string Designation { get; set; }

    }
    //Added all columns from the database, siva

    public class ApprovolStatusModel
    {
        public int EnterpreneuId { get; set; }
        public int ServiceId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int StatusDescription { get; set; }
        public int Status { get; set; }
        public string UploadFile { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }

    public class ServiceDetails
    {
        public int ServiceId { get; set; }
        public int QuestionnaireId { get; set; }
        public string ServiceName { get; set; }
        public string Fee { get; set; }
        public string FeeCalculation { get; set; }
    }

    #endregion

    #region Registration Parameters
    public class UserRegisterInfoModel
    {
        public string Prefix { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please Enter Correct Email Address")]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailID { get; set; }

        public string Address { get; set; }

        public int DistrictID { get; set; }

        public string District { get; set; }

        [Required(ErrorMessage = "Please Select Location")]
        public string Location { get; set; }

        public int MandalID { get; set; }
        public int VillageID { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [Display(Name = "Mobile")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Mobilenumber must be numeric")]
        [StringLength(10, ErrorMessage = "The Mobile must contains 10 digits", MinimumLength = 10)]
        public string MobileNumber { get; set; }


        [Required(ErrorMessage = "Please Enter Aadhaar Number")]
        [MinLength(12)]
        [MaxLength(12)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Aadhaar Number must be numeric")]
        [StringLength(12, ErrorMessage = "The Mobile must contains 10 digits", MinimumLength = 12)]
        public string AadhaarNumber { get; set; }


        [Required(ErrorMessage = "Please Enter PAN Number")]
        [StringLength(10, ErrorMessage = "The PAN Number must contains 10 digits", MinimumLength = 10)]
        //[RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Please Enter Correct PAN Number")]
        public string PANNumber { get; set; }

        [Required(ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ReenterPassword { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public int Pincode { get; set; }
        public string SecurityQuestion { get; set; }
        public string Answer { get; set; }

        public string CreatedBy { get; set; }

    }

    #endregion

    #region Entrepreneur Details
    public class EntrepreneurDetails
    {
        public int EntrepreneurID { get; set; }
        public string IndustrialUndertakingName { get; set; }
        public string PromoterName { get; set; }
        public string SoWoPromoter { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int MandalId { get; set; }
        public int VillageId { get; set; }
        public string StreetName { get; set; }
        public string DoorNo { get; set; }
        public string Pincode { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int OrganisationType { get; set; }
        public string TelephoneNO { get; set; }
        public string ProposalFor { get; set; }
        public string Caste { get; set; }
        public string Diffabled { get; set; }
        public string DirectMale { get; set; }
        public string DirectFemale { get; set; }
        public string IndirectMale { get; set; }
        public string IndirectFemlae { get; set; }
        public string WomenEntrepreneur { get; set; }
        public string Minory { get; set; }
        public string PlantValue { get; set; }
        public string LandValue { get; set; }
        public string BuildingValue { get; set; }
        public string TotalValue { get; set; }
        public string RegistrationCategory { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationDate { get; set; }
        public string FactoryType { get; set; }
    }

    #endregion

    #region AttachmentDetails
    public class AttachmentDetails
    {
        public string SelfCertificationForm { get; set; }
        public string RegistrationDeed { get; set; }
        public string MutationOrder { get; set; }
        public string BuildingPlan { get; set; }
        public string CombinedSitePlan { get; set; }
        public string PartnershipDetailsAOA { get; set; }
        public string processFlowChart { get; set; }
        public string PANorAADHAR { get; set; }

        public string AttachmentType { get; set; }
        public string Document { get; set; }
    }


    #endregion

    #region Department Approval details
    public class DeptApprovalDetails
    {
        public int DeptID { get; set; }
        public int RoleId { get; set; }
        public string DeptName { get; set; }

        public string Requiredinfo { get; set; }

        public int ApprovalDecision { get; set; }

        public bool ApplyforApproval { get; set; }

        public decimal Fee { get; set; }

        public decimal Amount { get; set; }
        public int Entrepreneur_ID { get; set; }
        public string EnterpreneurName { get; set; }
        public string IndustryName { get; set; }
        public int ServiceID { get; set; }

    }
    #endregion

    #region Business rule engine details
    public class BusinessRUleEngineDetails
    {
        public string ServiceName { get; set; }

        public string user1 { get; set; }

        public string user2 { get; set; }

        public int SLA { get; set; }

        public string MandaldocNames { get; set; }

        public string AdditionalDocs { get; set; }

        public string FeeDetails { get; set; }

        public decimal Fees { get; set; }

        public string Category { get; set; }

        public bool FixedFee { get; set; }

        public bool DepartmentRaised { get; set; }

        public bool Autogenerated { get; set; }
    }
    #endregion

    #region DeptUser
    public class DeptUser
    {
        public int JurisdictionId { get; set; }
        public int DistrictId { get; set; }
        public int MandalId { get; set; }
        public int VillageId { get; set; }
        public int RoleId { get; set; }
        public int DeptId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }

    }


    #endregion

    #region Grievance Details
    public class Grievance
    {
        public string IN { get; set; }
        public string DeptName { get; set; }
        public string DistName { get; set; }
        public string Email { get; set; }
        public int Mobile { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Upload { get; set; }

    }
    #endregion

    #region Raw MAterial allotment Details

    public class RawMaterialAllotmentDetails
    {
        [Required]
        public int AdhaarNumber { get; set; }

        [Required]
        public string AddressInfo { get; set; }
        [Required]
        public string ApplicationType { get; set; }

        [Required]
        public string UnitName { get; set; }

        [Required]
        public string RawMaterialAllotmentList { get; set; }

        [Required]
        public string RequirementDetails { get; set; }

        [Required]
        public string UsageDetails { get; set; }

        [Required]
        public int DistrictID { get; set; }

        [Required]
        public string DistrictName { get; set; }

        public int MandalID { get; set; }

        public string MandalName { get; set; }

        [Required]
        public string AllotmentOrder { get; set; }

        [Required]
        public string VATInfo { get; set; }

        [Required]
        public string CFOInfo { get; set; }

        [Required]
        public string RegisterInfo { get; set; }

        [Required]
        public string Boilerinfo { get; set; }

        [Required]
        public string Processflowchart { get; set; }

        [Required]
        public string ProductionProof { get; set; }

        public string StatusInfo { get; set; }

    }

    #endregion

    #region Land,Power,PCB,Helpdesk,DeptUserapproval,RaiseQuery,Addinspection,Applicationform 
    public class LandDetailsViewModel
    {
        [Required]
        public string LocationOfFactory { get; set; }
        [Required]
        public string ApplicationType { get; set; }
        public int? PlotNumber { get; set; }
        public string SectionedLayoutNo { get; set; }
        public string LandMasterPlan { get; set; }
        public string BuildingHeight { get; set; }
        public string RoadWidending { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string KMLFile { get; set; }
        public string LocationName { get; set; }
        public string ShowingTheSite { get; set; }
        public string SurveyNUmber { get; set; }
        public string Revenuesketchissued { get; set; }
        public string SitePlan { get; set; }
        public string SurveyNo { get; set; }
        public string District { get; set; }
        public string Mandal { get; set; }
        public string Village { get; set; }
        public string Grampanchayat { get; set; }
        public int PinCode { get; set; }
        public string EMail { get; set; }
        public string Telephone { get; set; }
        public string SiteArea { get; set; }
        public int ProposedArea { get; set; }
        public int TotalBuiltupArea { get; set; }
        public string RoadApproach { get; set; }
        public string TypeofApproachRoad { get; set; }
        public string LandLocationfalls { get; set; }
        public int BuildingApproval { get; set; }
        public string Activity { get; set; }
        public string CategoryofIndustry { get; set; }
        public string Yes { get; set; }
        public string No { get; set; }

    }
    public class PowerModel
    {
        public string KVA { get; set; }
        public string KWHP { get; set; }
        public string TransformerCapacity { get; set; }
        public string RequiredVoltage { get; set; }
        public string OtherServices { get; set; }
        public string PerDay { get; set; }
        public string PerMonth { get; set; }
        public DateTime? ExpectedMonthYear { get; set; }
        public DateTime? DateofPowerSupply { get; set; }
    }
    public class PCBDetails
    {
        public string Process { get; set; }
        public string Washings { get; set; }
        public string BBD { get; set; }
        public bool CTB { get; set; }
        public string Domestic { get; set; }
        public string Total { get; set; }
        public string Capacity { get; set; }
        public string FCD { get; set; }
        public string FSD { get; set; }
        public string Height { get; set; }
        public string Diameters { get; set; }
        public string APCEquipement { get; set; }
        public string ECSD { get; set; }
        public string QOE { get; set; }
        public string UG { get; set; }
        public string CE { get; set; }
        public string Waste { get; set; }
        public string CategoryRules { get; set; }
        public string QuanityGenerated { get; set; }
        public string ST { get; set; }
        public string Disposal { get; set; }
    }
    public class HelpDeskRegistrationModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string FeedbackType { get; set; }
        public string MailId { get; set; }
        public string Upload { get; set; }
        public string Comments { get; set; }

    }
    public class HelpDeskStatus
    {
        public string FeedbackType { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ReferenceNo { get; set; }
    }
    public class HelpDeskSummary
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class deptUserApplicationView : EntrepreneurDetails
    {
        public int UIDNo { get; set; }
        public string NameOfIndustrialL { get; set; }
        public string NameOfPromoter { get; set; }
        public string SonOf { get; set; }
        public string ProposedLocationofFactory { get; set; }
        public string SurveyNo { get; set; }
        public string NameofDistrictGrampanchayat { get; set; }
        public string Village { get; set; }
        public string Mandal { get; set; }
        public string District { get; set; }
        public int PinCode { get; set; }
        public string EmailID { get; set; }
        public string Telephone { get; set; }
        public string SightAreaDocuments { get; set; }
        public string ProposedAreaDocuments { get; set; }
        public string Totalbuiltuparea { get; set; }
        public string approachroad { get; set; }
        public string TypeofApproachRoad { get; set; }
        public string Landcomesunder { get; set; }
        public string Casetype { get; set; }
        public string CategoryofIndustry { get; set; }
        public string Status { get; set; }
        public string RoleName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public string UploadFile { get; set; }

    }
    public class RaiseQuery
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string RaisedBy { get; set; }
        public string RaisedOn { get; set; }
        public string RaisedTo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
    }
    public class AddInspection
    {
        public int DocumentID { get; set; }
        public string DocumentPath { get; set; }
        public string UserType { get; set; }
    }
    public class ApplicationFormModel
    {
        public int ConstitutionUnitId { get; set; }
        public int SectorEnterpriseId { get; set; }
        public string TotalExtentLand { get; set; }
        public string ProposedLocation { get; set; }
        public int LocationDistrictId { get; set; }
        public int LocationMandalId { get; set; }
        public int LocationVillageId { get; set; }
        public string CreatedBy { get; set; }
        public string Document { get; set; }
        public bool IsPCPNDT { get; set; }
        public bool IsAPMCE { get; set; }

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
    #region Existing User Registration Parameters for Re-activate License
    public class ExistingUserModel  //Added only required colums --Mounika
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public int RoleId { get; set; }
        public int UsertypeId { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseIssueDate { get; set; }
        public string LicenseExpiryDate { get; set; }
        public string Address { get; set; }
        public string AadhaarNumber { get; set; }
        public string PANNumber { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string StreetName { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string OTPNumber { get; set; }
        public int DepartmentID { get; set; }
    }

    #endregion

    #region APMCE Application Form

    public class RegistrationTypeDetailsModel
    {

        public string Name { get; set; }
        public int DistrictID { get; set; }
        public int MandalID { get; set; }
        public int VillageID { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string Pincode { get; set; }
        public string LicenceNumber { get; set; }
        public int RegistrationType { get; set; }
    }
    public class CorrespondantDetailsModel
    {
        [Required(ErrorMessage = "Please Enter NAME")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Select District")]
        public string DistrictName { get; set; }
        public int DistrictID { get; set; }
        [Required(ErrorMessage = "Please Select Mandal")]

        public string MandalName { get; set; }
        public int MandalID { get; set; }
        [Required(ErrorMessage = "Please Select Village")]

        public string VillageName { get; set; }
        public int VillageID { get; set; }
        [Required(ErrorMessage = "Please Enter StreetName")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Please Enter HouseNo")]
        public string HouseNo { get; set; }
        [Required(ErrorMessage = "Please Enter Pincode")]
        public string Pincode { get; set; }
        public string EstablishmentDate { get; set; }
    }
    public class SocietyDetailsModel
    {
        [Required(ErrorMessage = "Please Enter NAME")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Select District")]
        public int DistrictID { get; set; }
        [Required(ErrorMessage = "Please Select Mandal")]
        public int MandalID { get; set; }
        [Required(ErrorMessage = "Please Select Village")]
        public int VillageID { get; set; }
        [Required(ErrorMessage = "Please Enter StreetName")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Please Enter HouseNo")]
        public string HouseNo { get; set; }
        [Required(ErrorMessage = "Please Enter Pincode")]
        public int Pincode { get; set; }
        // [Required(ErrorMessage = "Please Enter Establishment Date")]
        public DateTime EstablishmentDate { get; set; }
    }
    public class AccomodationDetailsModel
    {
        [Required(ErrorMessage = "Please Select Establishment")]
        public int AccomodationOwnedBy { get; set; }
        [Required(ErrorMessage = "Please Select From Date")]
        public DateTime From { get; set; }
        // [Required(ErrorMessage = "Please Select To Date")]
        public DateTime AccomodationTo { get; set; }
        [Required(ErrorMessage = "Please Upload Document")]
        public string DocumentUpload { get; set; }
    }
    public class EstablishmentDetailsModelOld
    {
        [Required(ErrorMessage = "Please Enter EstablishmentDate")]
        public string EstablishmentDate { get; set; }
        [Required(ErrorMessage = "Please Enter Open Area")]
        public string OpenArea { get; set; }
        [Required(ErrorMessage = "Please Enter Construction Area")]
        public string ConstructionArea { get; set; }
        public string OpenAreaUploadDock { get; set; }
        public string ConstructionAreaUploadDock { get; set; }

    }
    public class ServiceOfferedByModel
    {
        public string BedStrength { get; set; }
        public int ServiceOfferedBy { get; set; }
    }
    public class DoctorDetailsModel
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string UploadDock { get; set; }
        public List<DoctorDetailsModel> DoctorList { get; set; }
        public string SpecilltyName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string Quantity { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public string LaborRoom { get; set; }
        public string OperationTheater { get; set; }
        public string DiagnosticFacilities { get; set; }
    }
    public class StaffDetailsModelOld
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string UploadDock { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string StaffSpecilltyName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int UserID { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public string Quantity { get; set; }
        public int ServiceOfferedByID { get; set; }

    }

    public class EquipmentFurnitureDetailsModel
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public string LaborRoom { get; set; }
        public string OperationTheater { get; set; }
        public string DiagnosticFacilities { get; set; }
        public string Yes { get; set; }
        public string No { get; set; }
        public string OtherInfo { get; set; }
        public string Declaretion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int UserID { get; set; }
    }
    public class OtherInformationDetailsModel
    {
        public string Name { get; set; }
        public int DistrictID { get; set; }
        public int MandalID { get; set; }
        public int VillageID { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public int Pincode { get; set; }
        public string Yes { get; set; }
        public string No { get; set; }
    }

    public class APMCECertificate
    {
        public string ApplicationNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicantNameAddress { get; set; }
        public string IssuingAuthority { get; set; }
        public string AppropriateAuthority { get; set; }
        public DateTime IssuedDate { get; set; }
        public List<string> ServiceDetails { get; set; }
        public string District { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ApplicantAddress { get; set; }
        public string Remarks { get; set; }
        public string InspectionReportNo { get; set; }
        public DateTime InspectionDate { get; set; }
        public APMCECertificate()
        {
            this.ServiceDetails = new List<string>();
        }
    }

    #endregion

    public class ThirdPartyVerification
    {
        public int TransactionId { get; set; }
        public int ServiceId { get; set; }
        public string Heading { get; set; }
        public string Name { get; set; }
        public string Diagnostic { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseIssuedDate { get; set; }
        public string LicenseExpiryDate { get; set; }
        public string DistrictId { get; set; }
        public string MandalId { get; set; }
        public string VillageId { get; set; }
        public string Cluster { get; set; }
    }

    //#region InceptionForm
    //public class InspectionModel
    //{
    //    public string HospitalName { get; set; }
    //    public string OwnerName { get; set; }
    //    public string QulificationCertificates { get; set; }
    //    public string PlaceAseptic { get; set; }
    //    public string EquipmentGynecologist { get; set; }
    //    public string EquipmentFacilities { get; set; }
    //    public string EquipmentUltraMachine { get; set; }
    //    public string EquipmentChrionic { get; set; }
    //    public string EquipmentCordocentesis { get; set; }
    //    public string EquipmentFoetoscope { get; set; }
    //    public string EquipmentSterilizaton { get; set; }
    //    public string EquipmentEmergency { get; set; }
    //    public string EmployeesGynaecologist { get; set; }
    //    public string EmployeesRadiologist { get; set; }
    //    public string EmployeesInception { get; set; }
    //    public string EmployeesReason { get; set; }

    //}
    //#endregion

    #region BioCapstone 
    public class BioCapstoneApplicantModel
    {
        public int Id { get; set; }
        //  [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        // [Required(ErrorMessage = "Institute Name is required")]
        public string InstitutionName { get; set; }
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        [StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        public string Aadhar { get; set; }

        [RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        [StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PAN { get; set; }
        // [Required(ErrorMessage = "Street Name is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        // [Required(ErrorMessage = "House Number is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        //  [Required(ErrorMessage = "PIN Code is required")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PINCode")]
        public string PINCode { get; set; }
        public string Fax { get; set; }
        public string Telegraph { get; set; }
        public string Telex { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        [Display(Name = "Role")]
        public string ApplicantRole { get; set; }
        [Display(Name = "Role (Other)")]
        public string ApplicantRoleOther { get; set; }
        public int ExistingApplicationId { get; set; }
    }
    public class AuthorisationModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Authorasation { get; set; }
        public string Studies { get; set; }
        public string Remarks { get; set; }
        public string Others { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
    public class BioCapstoneAddressofTreatmentFacility
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        [StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        public string Aadhar { get; set; }

        [RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        [StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PAN { get; set; }
        // [Required(ErrorMessage = "House Number is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        // [Required(ErrorMessage = "Street Name is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        //[Required(ErrorMessage = "PIN Code is required")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public string Fax { get; set; }
        public string Telegraph { get; set; }
        public string Telex { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }

    public class BioCapstoneAddressofDisposalWaste
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        //[Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        //[Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        [StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        public string Aadhar { get; set; }

        [RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        [StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PAN { get; set; }
        //  [Required(ErrorMessage = "House Number is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        // [Required(ErrorMessage = "Street Name is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        // [Required(ErrorMessage = "PIN Code is required")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PIN Code")]
        public string PINCode { get; set; }
        public string Fax { get; set; }
        public string Telegraph { get; set; }
        public string Telex { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
    public class TreatmentModle
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Attachment is required")]
        public string Attachment { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }

    }
    public class TreatmentDisposalModle
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Attachment is required")]
        public string Attachment { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }

    }
    public class QuantityWasteModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Select Category is required")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public string Quantity { get; set; }
        [Required(ErrorMessage = "Select Units is required")]
        public int UnitsId { get; set; }
        public string CategoryName { get; set; }
        public string QuantityOthers { get; set; }
        public string UnitName { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }

    }
    public class BioCapstoneDeclarationModel
    {

        public DateTime Date { get; set; }
        public string Place { get; set; }
        [Required(ErrorMessage = "Signature is required")]
        public string Signature { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }
        public int CreatedUserId { get; set; }
    }
    #endregion

    #region Eodb
    public class ProcedureCheckList
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public string SubDeptName { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceType { get; set; }
        public string Service { get; set; }
    }
    #endregion

    public class SMSModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string ApplicantMobileNumber { get; set; }
        public string ApplicantName { get; set; }
        public string DeptUserName { get; set; }
        public string DeptMobile { get; set; }
        public string PrevDeptName { get; set; }
        public string PrevDeptMobile { get; set; }
    }
    public class GraphModel
    {

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int MandalId { get; set; }
        public string MandalName { get; set; }
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public int value { get; set; }
        public string label { get; set; }
        public string Type { get; set; }
        public int StatusId { get; set; }
        public int ServiceId { get; set; }
    }
    public class MapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int ServiceId { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }

    }
    public class SeriesModel
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string SeriesName { get; set; }
        public List<string> Data { get; set; }
        public int value { get; set; }
        public string label { get; set; }
        public string Reject { get; set; }
        public string Approved { get; set; }
        public string Pending { get; set; }
        public string Appeal { get; set; }
    }

    public class ExistingLicense
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; }
        public string Address { get; set; }
        public string Aadhar { get; set; }
        public DateTime LicenseIssueDate { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }


        // Newly added For Add Existiing License Details
        public string ApplicationNumber{ get; set; }
        public DateTime SubmittedOn  { get; set; }

        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public int CurrentDesignationId { get; set; }
        
        public string TransactionType { get; set; }
        public string ExistingLicenseNumber { get; set; }
        public bool IsExisting { get; set; }

        public int TransactionId { get; set; }        
        public string ApplicantRole { get; set; }
        public string PAN { get; set; }

        [Required(ErrorMessage = "Please Enter NAME")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Select District")]
        public int DistrictID { get; set; }
        [Required(ErrorMessage = "Please Select Mandal")]
        public int MandalID { get; set; }
        [Required(ErrorMessage = "Please Select Village")]
        public int VillageID { get; set; }
        [Required(ErrorMessage = "Please Enter StreetName")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Please Enter HouseNo")]
        public string HouseNo { get; set; }
        [Required(ErrorMessage = "Please Enter Pincode")]
        public string Pincode { get; set; }

        public FormStatus Status { get; set; }
        public int StatusId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedUserId { get; set; }
        public DateTime UpdatedOn { get; set; }
    }    

    public class EquipmentMasterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Equipment Name")]
        public string Name { get; set; }
        public string Type { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class TAMCEFacilityModel
    {
        public int Id { get; set; }
        public string FacilityName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public bool IsAvailability { get; set; }
        public bool IsPartialInfoMismatch { get; set; }
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
