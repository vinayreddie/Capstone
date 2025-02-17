using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Capstone.Models
{
    public class DepartmentAdminViewModel
    {
        public DepartmentViewModel Department { get; set; }
        public UserViewModel DepartmentAdmin { get; set; }

        public DepartmentAdminViewModel()
        {
            this.Department = new DepartmentViewModel();
            this.DepartmentAdmin = new UserViewModel();
        }
    }

    public class DepartmentViewModel : DepartmentModel
    {
        public UserModel usermodel { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
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
    public class ApprovalsViewModel : ApprovalsModel
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
    }
    public class LicenseViewModel
    {
        public APMCECertificate APMCECertificate { get; set; }
        public PCPNDTLicenseInfoModel PCPNDTCertificate { get; set; }
        public BloodBankNOCModel BloodBankNOCModel { get; set; }
    }
    public class RejectViewModel
    {
        public APMCERejection APMCERejection { get; set; }
        public PCPNDTRejection PCPNDTRejection { get; set; }
    }
    public class RegistrationViewModel : RegistrationModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
        public string HospitalType { get; set; }

    }
    #region Workflow complex ViewModel
    public class ComplexWorkflowViewModel
    {
        public ServiceModel ServiceModel { get; set; }
        public WorkFlowModel WorkflowModel { get; set; }

    }
    public class WorkFlowViewModel : WorkFlowModel
    {
        public string DepartmentName { get; set; }
        public string FromOfficerName { get; set; }
        public string ToOfficerName { get; set; }

    }
    #endregion

    #region Application Form complex Models

    public class AcknowledgeModel
    {
        public bool HasAppliedforAPMCE { get; set; }
        public bool HasAppliedforPCPNDT { get; set; }
        public bool HasAppliedforBloodBank { get; set; }
        public APMCEAckModel APMCEAckModel { get; set; }
        public PCPNDTAckModel PCPNDTAckModel { get; set; }
        public BloodBankAckModel BloodBankAckModel { get; set; }
        public bool HasMissingDocuments { get; set; }
        public List<string> ServiceDetails { get; set; }
    }

    #region APMCE Models
    public class APMCEViewModel
    {
        public RegistrationViewModel RegistrationModel { get; set; }
        public RegistrationViewModel RegistrationModelLog { get; set; }
        public TrustViewModel TrustModel { get; set; }
        public CorrespondingAddressViewModel CorrespondingAddress { get; set; }
        public CorrespondingAddressViewModel CorrespondingAddressLog { get; set; }
        public AccommadationViewModel Accommadation { get; set; }
        public AccommadationViewModel AccommadationLog { get; set; }
        public EstablishmentModel EstablishmentModel { get; set; }
        public InfraStructureModel InfraStructure { get; set; }
        public List<InfraStructureModel> InfraStructureList { get; set; }
        public List<InfraStructureModel> InfraStructureListLog { get; set; }

        public StaffDetailsModel StaffDetailsmodel { get; set; }
        public List<StaffDetailsModel> StaffDetailsList { get; set; }
        public FacilitiesAvailableModel FacilitiesAvailableModel { get; set; }
        public FacilitiesAvailableModel FacilitiesAvailableLogModel { get; set; }
        public OfferedServicesModel OfferedServices { get; set; }

        public OfferedServicesModel OfferedServicesLog { get; set; }
        public StaffDetailsViewModel StaffDetails { get; set; }
        public List<StaffDetailsViewModel> StaffDetailsLog { get; set; }
        public CancelLicenseModel cancelLiceseModel { get; set; }
        public APMCECertificate APMCECertificateModel { get; set; }
        public AdditionalDocumentsModel AdditionalDocumentsModel { get; set; }
        public int TransactionId { get; set; }
        public string RejectionRemarks { get; set; }
        public string ReasonforAppeal { get; set; }
        public int AmendmentSno { get; set; }
        public APMCEViewModel()
        {
            this.RegistrationModel = new RegistrationViewModel();
            this.EstablishmentModel = new EstablishmentViewModel();
            this.CorrespondingAddress = new CorrespondingAddressViewModel();
            this.TrustModel = new TrustViewModel();
            this.Accommadation = new AccommadationViewModel();
            this.InfraStructure = new InfraStructureModel();
            this.FacilitiesAvailableModel = new FacilitiesAvailableModel();
            this.OfferedServices = new OfferedServicesModel();
            this.StaffDetails = new StaffDetailsViewModel();
            this.APMCECertificateModel = new APMCECertificate();
            this.AdditionalDocumentsModel = new AdditionalDocumentsModel();
        }
        public bool HasAppliedforCorrespondent { get; set; }
        public bool HasAppliedforAccomodation { get; set; }
        public bool HasAppliedforServices { get; set; }
        public bool HasAppliedforEmployee { get; set; }
        public bool HasAppliedforSpecialtiesAvailable { get; set; }
        public bool HasAppliedforEquipmentFurniture { get; set; }
        public bool HasAppliedforLabor { get; set; }
        public bool HasAppliedforOperationTheatre { get; set; }
        public bool HasAppliedforDiagnosticCenter { get; set; }
    }
    public class CorrespondingAddressViewModel : CorrespondingAddressModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class TrustViewModel : TrustModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class AccommadationViewModel : AccommodationModel
    {
        public string EstablishmentTypeName { get; set; }
    }
    public class StaffDetailsViewModel : StaffDetailsModel
    {
        public string SpecialtyName { get; set; }
        public int AmendmentSNo { get; set; }
    }
    #endregion

    #region PCPNDT View Models
    public class PCPNDTViewModel
    {
        public ApplicantViewModel ApplicantModel { get; set; }
        public EstablishmentViewModel EstablishmentModel { get; set; }
        public FacilityViewModel FacilityModel { get; set; }
        public FacilityViewModel FacilityLogModel { get; set; }
        public TestsModel TestsModel { get; set; }
        public TestsModel TestsModelLog { get; set; }
        public EquipmentModel EquipmentModel { get; set; }
        public FacilitesModel FacilitiesModel { get; set; }
        public FacilitesModel FacilitiesModellog { get; set; }
        public EmployeeViewModel EmployeeModel { get; set; }
        public InstitutionViewModel InstitutionModel { get; set; }
        public InstitutionViewModel InstitutionModelLog { get; set; }
        public DeclarationModel DeclarationModel { get; set; }
        public CancelLicenseModel cancelLiceseModel { get; set; }
        public List<EquipmentModel> EquipmentList { get; set; }
        public List<EquipmentModel> EquipmentListLog { get; set; }
        public List<EmployeeViewModel> EmployeeList { get; set; }
        public List<EmployeeViewModel> EmployeeListLog { get; set; }
        public PCPNDTLicenseInfoModel PCPNDTLicenseModel { get; set; }
        public NOCforquipmentModel NocforEquipmentModel { get; set; }
        public string ServiceType { get; set; }
        public int ServiceId { get; set; }
        public string ApplicationNumber { get; set; }
        public int AppliedSince { get; set; }
        public int TransactionId { get; set; }
        //public string ExistingApplicationNo { get; set; }
        public PCPNDTViewModel()
        {
            this.ApplicantModel = new ApplicantViewModel();
            this.EstablishmentModel = new EstablishmentViewModel();
            this.FacilityModel = new FacilityViewModel();
            this.TestsModel = new TestsModel();
            this.EquipmentModel = new EquipmentModel();
            this.FacilitiesModel = new FacilitesModel();
            this.EmployeeModel = new EmployeeViewModel();
            this.InstitutionModel = new InstitutionViewModel();
            this.DeclarationModel = new DeclarationModel();
            this.PCPNDTLicenseModel = new PCPNDTLicenseInfoModel();
            this.NocforEquipmentModel = new NOCforquipmentModel();
        }
        public bool HasAppliedforFacilityAddress { get; set; }
        public bool HasAppliedforInstitution { get; set; }
        public bool HasAppliedforEquipment { get; set; }
        public bool HasAppliedforEmployeeDetails { get; set; }
        public bool HasAppliedforFacilitiesavailable { get; set; }
        public string RejectionRemarks { get; set; }
        [Required(ErrorMessage = "Feild is required")]
        public string ReasonforAppeal { get; set; }
    }

    public class ApplicantViewModel : ApplicantModel
    {
        public int ServiceId { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class EstablishmentViewModel : EstablishmentModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }

    public class FacilityViewModel : FacilityModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class EmployeeViewModel : EmployeeModel
    {
        public string DesignationName { get; set; }

    }
    public class InstitutionViewModel : InstitutionModel
    {
        public string OwnershipTypeName { get; set; }
        public string InstitutionTypeName { get; set; }
    }
    #endregion

    #region BloodBank View Models
    public class BloodBankViewModel
    {
        public BloodBankApplicantViewModel BloodBankApplicantModel { get; set; }
        public BloodBankEstablishmentViewModel BloodBankEstablishmentModel { get; set; }
        public BloodBankListModel BloodBankListModel { get; set; }
        public EmployeeViewModel EmployeeModel { get; set; }
        public TechnicalModel TechnicalModel { get; set; }
        public EquipmentModel EquipmentModel { get; set; }
        public BloodBankAttachments BloodBankAttachments { get; set; }
        public List<BloodBankListModel> BloodBankList { get; set; }
        public List<EmployeeViewModel> EmployeeList { get; set; }
        public List<EquipmentModel> EquipmentList { get; set; }
        public List<TechnicalModel> TechnicalList { get; set; }

        public int TransactionId { get; set; }
        public BloodBankViewModel()
        {
            this.BloodBankApplicantModel = new BloodBankApplicantViewModel();
            this.BloodBankEstablishmentModel = new BloodBankEstablishmentViewModel();
            this.BloodBankListModel = new BloodBankListModel();
            this.EmployeeModel = new EmployeeViewModel();
            this.EquipmentModel = new EquipmentModel();
            this.BloodBankAttachments = new BloodBankAttachments();
            this.TechnicalModel = new TechnicalModel();
        }

    }

    public class BloodBankApplicantViewModel : BloodBankApplicantModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class BloodBankEstablishmentViewModel : BloodBankEstablishmentModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    #endregion

    #region Homeopathy Drug Store
    public class HomeopathyDrugStoreViewModel
    {
        public ApplicantViewModel HDApplicantModel { get; set; }
        public HomeopathyEstablishmentViewModel HDEstablishment { get; set; }
        public ApplicantViewModel HDCompetentModel { get; set; }
        public HomeopathyDeclaration HDDeclaration { get; set; }
        public int TransactionId { get; set; }
        public int CreatedUserId { get; set; }
        public HomeopathyDrugStoreViewModel()
        {
            this.HDApplicantModel = new ApplicantViewModel();
            this.HDEstablishment = new HomeopathyEstablishmentViewModel();
            this.HDCompetentModel = new ApplicantViewModel();
            this.HDDeclaration = new HomeopathyDeclaration();
        }
    }
    public class HomeopathyEstablishmentViewModel : HomeopathyEstablishment
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    #endregion
    #region Allopathic Drug Store
    public class AllopathicDrugStoreViewModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public ApplicantViewModel ADApplicantModel { get; set; }
        public AllopathicPharmacyViewModel ADPharmacyModel { get; set; }
        public ApplicantViewModel ADCompetentPersonModel { get; set; }
        public AllopathicDrugName ADDrugNameModel { get; set; }
        public List<AllopathicDrugName> AllopathicDrugList { get; set; }
        public AllopathicDeclaration ADDeclaration { get; set; }
        public string uploadForm { get; set; }
        public int TransactionId { get; set; }
        public AllopathicDrugStoreViewModel()
        {
            this.ADApplicantModel = new ApplicantViewModel();
            this.ADPharmacyModel = new AllopathicPharmacyViewModel();
            this.ADCompetentPersonModel = new ApplicantViewModel();
            this.ADDrugNameModel = new AllopathicDrugName();
            this.ADDeclaration = new AllopathicDeclaration();

        }
    }
    public class AllopathicPharmacyViewModel : AllopathicPharmacy
    { public int ServiceId { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    #endregion
    #endregion

    #region TransactionViewModel
    public class TransactionViewModel : TransactionModel
    {
        public string ServiceName { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
        public string ApplicantName { get; set; }
        public int AmendmentId { get; set; }
        public int TranServiceId { get; set; }
        public string Type { get; set; }
        public string CurrentDesignationName { get; set; }
        public string StatusName { get; set; }
        public string LicenseExpiryDate { get; set; }
    }
    #endregion

    #region ApprovalScreen Complex view model 
    public class ApprovalComplexViewModel
    {
        public List<DesignationModel> DesginationList;
        public WorkFlowModel Workflow { get; set; }
        public ApprovalsModel Approval { get; set; }
        public List<ApprovalsViewModel> ApprovalList { get; set; }
        public List<FacilityMasterModel> FacilityList { get; set; }
        public APMCEViewModel APMCEModel { get; set; }
        public PCPNDTViewModel PCPNDTModel { get; set; }
        public BloodBankViewModel BloodBankModel { get; set; }
        public BloodBankViewModel BloodBankForm27EModel { get; set; }
        public List<QuestionModel> QuestionModelList { get; set; }
        public List<DocumentUploadModel> UploadList { get; set; }
        public NOCforquipmentModel NOCforEquipment { get; set; }
        public int QueryCount { get; set; }
        public int QueryResponseCount { get; set; }
        public int ServiceId { get; set; }
        public int TranServiceId { get; set; }
        public int InspectionReportCount { get; set; }
        public string InspectionPDF { get; set; }
        public string InspectionPDFpath { get; set; }
        public string ServiceType { get; set; }
        public int CurrentDestignationId { get; set; }
        public string ApplicationNumber { get; set; }
        public int ApplicationId { get; set; }
        public Status CurrentStatus { get; set; }
        public int ReturnedSource { get; set; }
        public TAMCEFacilityModel tamceFacilityModel { get; set; }

    }
    #endregion

    #region Query Response View Model
    public class QueryResponseViewModel
    {
        public List<QueryModel> RaisedQueryList { get; set; }
        public List<QueryModel> QueryLogList { get; set; }
    }
    #endregion

    #region Designation View Model
    public class DesignationComplexViewModel
    {
        public List<DesignationModel> DesignationList { get; set; }
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

    #region OrganTransplantation
    public class HospitalViewModel : HospitalModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    #endregion
    
    // Old

    #region Compolex view models
    public class DeptAdminComplexViewModel
    {
        public DocumentModel DocumentModel { get; set; }
        public RoleModel RoleModel { get; set; }
        public WorkFlowModel WorkFlowModel { get; set; }
        public ServiceModel ServiceModel { get; set; }

    }



    public class PCPNDTComplexViewModel
    {
        public int ApplicantUserId { get; set; }
        public int ApplicantServiceId { get; set; }
        public PCPNDTUserRegDetails PcpndtUserRegModel { get; set; }
        public ApplicantdetailsModel ApplicantModel { get; set; }
        public OwnershipdetailsModel OwnerShipModel { get; set; }
        //public FacilityDetailsModel FacilityInfoModel { get; set; }
        public RequiredDetailsForApprovalsModel RequireddetailsforApprovalsModel { get; set; }
        public EquipmentDetailsModel EquipmentDetailsModel { get; set; }
        public EmployeeDetailsModel EmployeeDetailsModel { get; set; }
        public FacilitiesAvailableDetailsModel FacilitiesAvailableModel { get; set; }

        public ApplicationFormModel ApplicationFormModel { get; set; }
        public DocumentModel Attachments { get; set; }

        public string InstitutionType { get; set; }
        public string TotalWorkArea { get; set; }
        public string Invasive { get; set; }
        public string NonInvasive { get; set; }

        public string Tests { get; set; }
        public string Studies { get; set; }
        public string Reason { get; set; }


        public RegistrationTypeDetailsModel RegistrationTypeDetailsModel { get; set; }
        public CorrespondantDetailsModel CorrespondantDetailsModel { get; set; }
        public SocietyDetailsModel SocietyDetailsModel { get; set; }
        public AccomodationDetailsModel AccomodationDetailsModel { get; set; }
        //public EstablishmentDetailsModel EstablishmentDetailsModel { get; set; }
        public ServiceOfferedByModel ServiceOfferedByModel { get; set; }
        public DoctorDetailsModel DoctorDetailsModel { get; set; }
        public StaffDetailsModel StaffDetailsModel { get; set; }


        public EquipmentFurnitureDetailsModel EquipmentFurnitureDetailsModel { get; set; }
        public OtherInformationDetailsModel OtherInformationDetailsModel { get; set; }
    }

    public class APMCEComplexViewModel
    {
        public int ApplicantUserId { get; set; }
        public int ApplicantServiceId { get; set; }
        public RegistrationTypeDetailsModel RegistrationTypeDetailsModel { get; set; }
        public CorrespondantDetailsModel CorrespondantDetailsModel { get; set; }
        public SocietyDetailsModel SocietyDetailsModel { get; set; }
        public AccomodationDetailsModel AccomodationDetailsModel { get; set; }
        //public EstablishmentDetailsModel EstablishmentDetailsModel { get; set; }
        public ServiceOfferedByModel ServiceOfferedByModel { get; set; }
        public DoctorDetailsModel DoctorDetailsModel { get; set; }
        public StaffDetailsModel StaffDetailsModel { get; set; }
        //  public ParaCapstoneStaffDetailsModel ParaCapstoneStaffDetailsModel { get; set; }
        //    public SupportingStaffDetailsModel SupportingStaffDetailsModel { get; set; }
        //  public AvailableSpelistDtailsModel AvailableSpelistDtailsModel { get; set; }

        public EquipmentFurnitureDetailsModel EquipmentFurnitureDetailsModel { get; set; }
        public OtherInformationDetailsModel OtherInformationDetailsModel { get; set; }

    }

    #region Tempmodels for demo
    public class tempuserview
    {
        public UserModel UserModel { get; set; }
        public UserRegisterInfoModel UserRegisterInfoModel { get; set; }
    }
    public class tempDepartmentuserview
    {
        public UserModel UserModel { get; set; }
        public DeptUser DeptUser { get; set; }
    }
    public class DepartmentAndUserViewModel
    {
        public DepartmentModel DepartmentModel { get; set; }
        public UserModel UserModel { get; set; }

        [Required(ErrorMessage = "Please Upload Logo")]
        public HttpPostedFileBase UploadLogo { get; set; }

        public string LogoPath { get; set; }

    }
    public class DetpUserApproval
    {

        public deptUserApplicationView deptUserApplicationView { get; set; }
        public List<ApprovolStatusModel> approvolstatusmodel { get; set; }
        public UserModel userModel { get; set; }
        public HttpPostedFileBase attachment { get; set; }
        public int TransactionId { get; set; }
        public int RoleId { get; set; }
        public bool IsInspectionReport { get; set; }
        public bool IsIsApprovalAuthority { get; set; }
        public Status PreviousRoleStatus { get; set; }
        public Status CurrentRolestatus { get; set; }
        public int ApplicantUserId { get; set; }

        public int ApplicantServiceId { get; set; }
        public InspectionModel InspectionModel { get; set; }
    }

    #endregion
    #endregion

    #region Transaction view models

    #endregion

    #region Custom view Models
    public class ApprovalStatusViewModel : ApprovalStatusModel
    {
        public string RoleName { get; set; }
        public string StatusDescription { get; set; }
    }
    public class ApplicantTransactionViewModel : ApplicantTransactionModel
    {

        public string ServiceName { get; set; }
        public string StageDescription { get; set; }

    }
    #endregion

    #region Bio Capstone View Model

    public class BioCapstoneApplicantViewModel : BioCapstoneApplicantModel
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class AuthorisationViewModel : AuthorisationModel
    {

    }
    public class BioCapstoneAddressTreatmentFacilityViewModel : BioCapstoneAddressofTreatmentFacility
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class BioCapstoneAddressofDisposalWasteViewModel : BioCapstoneAddressofDisposalWaste
    {
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class TreatmentViewModle : TreatmentModle
    {

    }
    public class TreatmentDIsposalViewModle : TreatmentDisposalModle
    {

    }
    public class QuantityWasteViewModel : QuantityWasteModel
    {

    }
    public class DeclarationViewModel : DeclarationModel
    {

    }
    public class BioCapstoneViewModel
    {
        public BioCapstoneApplicantViewModel BioCapstoneApplicantModel { get; set; }
        public AuthorisationViewModel AuthorisationModel { get; set; }
        public BioCapstoneAddressTreatmentFacilityViewModel BioCapstoneAddressTreatementFacilityModel { get; set; }
        public BioCapstoneAddressofDisposalWasteViewModel BioCapstoneAddressDisposalWasteModel { get; set; }
        public TreatmentViewModle TreatmentModle { get; set; }
        public List<TreatmentModle> TreatmentList { get; set; }
        public List<TreatmentDisposalModle> TreatmentDisposalList { get; set; }
        public List<QuantityWasteModel> QuantityWasteList { get; set; }
        public TreatmentDIsposalViewModle TreatmentDisposalModle { get; set; }
        public QuantityWasteViewModel QuantityWasteModel { get; set; }
        public DeclarationViewModel DeclarationModel { get; set; }
        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        [Display(Name = "Role")]
        public string ApplicantRole { get; set; }
        [Display(Name = "Role (Other)")]
        public string ApplicantRoleOther { get; set; }
        public int ExistingApplicationId { get; set; }
        public int TransactionId { get; set; }
        public BioCapstoneViewModel()
        {
            this.BioCapstoneApplicantModel = new BioCapstoneApplicantViewModel();
            this.AuthorisationModel = new AuthorisationViewModel();
            this.BioCapstoneAddressTreatementFacilityModel = new BioCapstoneAddressTreatmentFacilityViewModel();
            this.BioCapstoneAddressDisposalWasteModel = new BioCapstoneAddressofDisposalWasteViewModel();
            this.TreatmentModle = new TreatmentViewModle();
            this.TreatmentDisposalModle = new TreatmentDIsposalViewModle();
            this.QuantityWasteModel = new QuantityWasteViewModel();
            this.DeclarationModel = new DeclarationViewModel();
        }
    }
    #endregion

    #region Organ Transplant View model
    public class OrganTransplantViewModel
    {
        public HospitalViewModel HospitalModel { get; set; }

        public SurgicalTeamModel Surgical { get; set; }
        public CapstoneTeamModel CapstoneTeam { get; set; }
        public AnaesthesiologyModel Anaesthesiology { get; set; }
        public ICUHDUFacilities ICUHDUFacilities { get; set; }
        public LaboratoryFacilitiesModel LaboratoryFacilities { get; set; }
        public ImagingServicesModel ImagingServices { get; set; }
        public HaematologyServiesModel HaematologyServices { get; set; }
        public OrganTransplantFacilitiesModel Facilities { get; set; }
        public DeclarationModel Declaration { get; set; }
        public int TransactionId { get; set; }
        public int CreatedUserID { get; set; }


    }
    #endregion

    #region ExistingLicense
    public class ExistingPCPNDTViewModel
    {
        public ExistingApplicantModel ApplicantModel { get; set; }
        public ExistingEstablishmentModel EstablishmentModel { get; set; }
        public ExistingFacilityModel FacilityModel { get; set; }
        public ExistingFacilityModel FacilityLogModel { get; set; }
        public ExistingTestsModel TestsModel { get; set; }
        public ExistingTestsModel TestsModelLog { get; set; }
        public ExistingEquipmentModel EquipmentModel { get; set; }
        public ExistingFacilitesModel FacilitiesModel { get; set; }
        public ExistingFacilitesModel FacilitiesModellog { get; set; }
        public ExistingEmployeeModel EmployeeModel { get; set; }
        public ExistingInstitutionModel InstitutionModel { get; set; }
        public ExistingInstitutionModel InstitutionModelLog { get; set; }
        public ExistingDeclarationModel DeclarationModel { get; set; }
        public ExistingCancelLicenseModel cancelLiceseModel { get; set; }
        public List<ExistingEquipmentModel> EquipmentList { get; set; }
        public List<ExistingEquipmentModel> EquipmentListLog { get; set; }
        public List<ExistingEmployeeModel> EmployeeList { get; set; }
        public List<ExistingEmployeeModel> EmployeeListLog { get; set; }
        public ExistingPCPNDTLicenseInfoModel PCPNDTLicenseModel { get; set; }

        public int ApplicationId { get; set; }
        public int TransactionId { get; set; }
        public string ExistingApplicationNo { get; set; }
        public ExistingPCPNDTViewModel()
        {
            this.ApplicantModel = new ExistingApplicantModel();
            this.EstablishmentModel = new ExistingEstablishmentModel();
            this.FacilityModel = new ExistingFacilityModel();
            this.TestsModel = new ExistingTestsModel();
            this.EquipmentModel = new ExistingEquipmentModel();
            this.FacilitiesModel = new ExistingFacilitesModel();
            this.EmployeeModel = new ExistingEmployeeModel();
            this.InstitutionModel = new ExistingInstitutionModel();
            this.DeclarationModel = new ExistingDeclarationModel();
            this.PCPNDTLicenseModel = new ExistingPCPNDTLicenseInfoModel();
            this.EmployeeList = new List<ExistingEmployeeModel>();
        }
        public bool HasAppliedforFacilityAddress { get; set; }
        public bool HasAppliedforInstitution { get; set; }
        public bool HasAppliedforEquipment { get; set; }
        public bool HasAppliedforEmployeeDetails { get; set; }
        public bool HasAppliedforFacilitiesavailable { get; set; }
        public string RejectionRemarks { get; set; }
        [Required(ErrorMessage = "Feild is required")]
        public string ReasonforAppeal { get; set; }
    }
    public class ExistingApplicantModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        //@"^[a-zA-Z]+$"
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabets only")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please select Role")]
        [Display(Name = "Role")]

        public string ApplicantRole { get; set; }
        [Display(Name = "Role (Other)")]
        [Required(ErrorMessage = "Please enter Role (Other)")]
        public string ApplicantRoleOther { get; set; }
        [Display(Name = "District")]
        [Required(ErrorMessage = "Please select District")]
        public int DistrictId { get; set; }
        [Display(Name = "Mandal")]
        [Required(ErrorMessage = "Please select Mandal")]
        public int MandalId { get; set; }
        [Display(Name = "Village")]
        [Required(ErrorMessage = "Please select Village")]
        public int VillageId { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Mobilenumber must be numeric")]
        [StringLength(10, ErrorMessage = "Mobile Number should be 10 digits", MinimumLength = 10)]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Enter Aadhar Number")]
        [StringLength(12, ErrorMessage = "Aadhar should be 12 digits", MinimumLength = 12)]
        public string Aadhar { get; set; }
        [Required(ErrorMessage = "Please Enter PAN Number")]
        [RegularExpression("[A-Za-z]{5}\\d{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN Number")]
        [StringLength(10, ErrorMessage = "PAN Number should be 10 digits", MinimumLength = 10)]
        public string PAN { get; set; }
        [Required(ErrorMessage = "Please enter H. No.")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        [Required(ErrorMessage = "Please enter Street")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Please enter PIN Code")]
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
        public List<DocumentUploadModel> uploadedDocuments { get; set; }
        public int ServiceId { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }

        public ExistingApplicantModel()
        {
            this.uploadedDocuments = new List<DocumentUploadModel>();
        }

        public string AadharPath { get; set; }
        public string PANPath { get; set; }
    }
    public class ExistingEstablishmentModel
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
        [Required(ErrorMessage = "OpenAreaFilePath is required")]
        public string OpenAreaFilePath { get; set; }
        [Required(ErrorMessage = "ConstructionAreaFilePath is required")]
        public string ConstructionAreaFilePath { get; set; }
        public FormStatus FormStatus { get; set; }
        public int CreatedUserId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class ExistingFacilityModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select District")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Please select Mandal")]
        [Display(Name = "Mandal")]
        public int MandalId { get; set; }
        [Required(ErrorMessage = "Please select Village")]
        [Display(Name = "Village")]
        public int VillageId { get; set; }
        [Required(ErrorMessage = "Street Name is required")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "House Number is required")]
        [Display(Name = "H. No.")]
        public string HouseNumber { get; set; }
        [Required(ErrorMessage = "PIN Code")]
        [StringLength(6, ErrorMessage = "PIN Code should be 6 digits", MinimumLength = 6)]
        [Display(Name = "PINCode")]
        public string PINCode { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, ErrorMessage = "Phone number should be 10 digits", MinimumLength = 10)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Telegraph { get; set; }
        public string Telex { get; set; }
        [Required(ErrorMessage = "Please select at least one Facility")]
        public string Faclities { get; set; }
        [Required(ErrorMessage = "Please Upload Address Proof")]
        [Display(Name = "Address Proof")]
        public string AddressProofPath { get; set; }
        [Required(ErrorMessage = "Please Upload Building Layout")]
        [Display(Name = "Building Layout")]
        public string BuildingLayoutPath { get; set; }

        public int CreatedUserId { get; set; }
        public FormStatus FormStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int ExistingApplicationId { get; set; }
        public string DistrictName { get; set; }
        public string MandalName { get; set; }
        public string VillageName { get; set; }
    }
    public class ExistingTestsModel
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
    public class ExistingEquipmentModel
    {
        public int Id { get; set; }
        public int EquipmentLogId { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Make")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Please enter Model")]
        [Display(Name = "Model")]
        public string MachineModel { get; set; }
        [Required(ErrorMessage = "Please enter Serial No.")]
        [Display(Name = "Serial No.")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Please enter Type")]
        public string Type { get; set; }
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please upload a file")]
        public string UploadedFilePath { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public FormStatus FormStatus { get; set; }

    }
    public class ExistingFacilitesModel
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
    public class ExistingEmployeeModel
    {
        public string DesignationName { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public int EmployeeLogId { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "please enter Alphabet only")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Select Designation")]
        public int DesignationId { get; set; }
        public int QualificationId { get; set; }
        public string QualificationName { get; set; }
        //[Required(ErrorMessage = "Please enter Experience")]
        public string Experience { get; set; }
        [Required(ErrorMessage = "Enter Years")]
        [Range(0, 100, ErrorMessage = "Value can't be more than 100")]
        public int ExpYears { get; set; }
        [Required(ErrorMessage = "Enter Months")]
        [Range(0, 12, ErrorMessage = "Value can't be more than 12")]
        public int ExpMonths { get; set; }
        [Required(ErrorMessage = "Enter Days")]
        [Range(0, 31, ErrorMessage = "Enter number between 0 to 31")]
        public int ExpDays { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
        [Display(Name = "Reg. No.")]
        [Required(ErrorMessage = "Please enter Reg. No.")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please upload file")]
        public string UploadedFilePath { get; set; }
        [Required(ErrorMessage = "Please upload files")]
        public List<DocumentUploadModel> UploadDocuments { get; set; }
        public int CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public FormStatus FormStatus { get; set; }
       

        public ExistingEmployeeModel()
        {
            this.UploadDocuments = new List<DocumentUploadModel>();
        }

    }
    public class ExistingInstitutionModel
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
        public string OwnershipTypeName { get; set; }
        public string InstitutionTypeName { get; set; }
    }
    public class ExistingDeclarationModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "SonOf is required")]
        public string SonOf { get; set; }
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "ResidentOf is required")]
        public string ResidentOf { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Organisation is required")]
        public string Organization { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Place is required")]
        public string Place { get; set; }
        [Required(ErrorMessage = "Signature is required")]
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
        public DocumentUploadModel DocumentUploadModel { get; set; }
        public List<DocumentUploadModel> OtherUploadsList { get; set; }
    }
    public class ExistingCancelLicenseModel
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
    public class ExistingPCPNDTLicenseInfoModel
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
    }


    #endregion

    public class EquipmentComplexViewModel
    {
        public List<EquipmentMasterModel> EquipmentList { get; set; }
        public NotificationModel Notification { get; set; }
    }

    public class OfferedServiceEquipmentsComplexViewModel : OfferedServiceEquipmentsMasterModel
    {
        public string HospitalType { get; set; }
        public string OfferedService { get; set; }
        public string Equipments { get; set; }
        public NotificationModel Notification { get; set; }
    }

    public class TAMCEMultiFormsViewModel
    {
        public AcknowledgeModel acknowledgeModel { get; set; }
        public APMCECertificate tamceCertificate { get; set; }
        public APMCEAckModel tamceAckModel { get; set; }        
    }   
}
