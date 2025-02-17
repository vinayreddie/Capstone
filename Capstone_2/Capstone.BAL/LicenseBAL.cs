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
        ApplicationDAL objApplicationDAL;

        public List<ApplicationModel> GetApplicationList(int userId)
        {
            objDAL = new LicenseDAL();
            DataTable dtItems = objDAL.GetApplicationList(userId);
            if (dtItems == null)
                return null;

            List<ApplicationModel> objList = new List<ApplicationModel>();
            ApplicationModel application = new ApplicationModel();
            foreach (DataRow row in dtItems.Rows)
            {
                application = new ApplicationModel();
                application.Id = Convert.ToInt32(row["Id"]);
                application.ApplicationNumber = row["ApplicationNumber"].ToString();
                application.SubmittedOn = Convert.ToDateTime(row["SubmittedOn"]);
                application.Status = (Status)Convert.ToInt32(row["Status"]);
                objList.Add(application);
            }

            return objList;
        }

        public DataTable GetApplicantDetailsForPayment(int applicationId)
        {
            objDAL = new LicenseDAL();
            return objDAL.GetApplicantDetailsForPayment(applicationId);
        }

        public ApplicationModel GetApplication(int TransactionId, Status status,string TransactionType)
        {
            objDAL = new LicenseDAL();
            DepartmentUserBAL objBAL = new DepartmentUserBAL();
            ApplicationBAL objApplicationBAL;
            DataTable dtItems = objDAL.GetTransactions(TransactionId, status, TransactionType);
            if (dtItems == null)
                return null;

            ApplicationModel applicationModel = new ApplicationModel();
            applicationModel.Id = Convert.ToInt32(dtItems.Rows[0]["Applicationid"]);
            applicationModel.ApplicationNumber = dtItems.Rows[0]["ApplicationNumber"].ToString();
            applicationModel.APMCEModel = null;
            applicationModel.PCPNDTModel = null;
            applicationModel.BloodBankModel = null;
            applicationModel.BloodBankForm27EModel = null;
            applicationModel.OrganTransplantModel = null;
            applicationModel.BioCapstoneModel = null;
            applicationModel.HomeopathyDrugStore = null;
            applicationModel.AllopathicDrugModel = null;
            foreach (DataRow row in dtItems.Rows)
            {
                
                int _serviceId = Convert.ToInt32(row["ServiceId"]);
                int _transactionId = Convert.ToInt32(row["Id"]);
                string _type = row["TableName"].ToString();
                switch (_serviceId)
                {
                    case 1:

                        applicationModel.APMCEModel = GetAPMCEData(_transactionId, _type);
                        break;
                    case 2: //PCPNDT Grant
                    case 26:
                    case 27: // PCPNDT Appeal
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId,_type);
                        break;
                    case 31:
                        applicationModel.BloodBankModel = GetBloodBankData(_transactionId);
                        break;
                    case 32:
                        applicationModel.BloodBankForm27EModel = GetForm27EBloodBankData(_transactionId);
                        break;
                    case 33:
                        //FileHandling.ReadWrite2Files fh = new FileHandling.ReadWrite2Files();
                        //applicationModel.OrganTransplantModel = fh.DeSerializeObject<OrganTransplantViewModel>("D:\\BioMedial_Test.OT");
                        applicationModel.OrganTransplantModel = GetOrganTransplantationData(_transactionId);
                        break;
                    case 34:
                        applicationModel.BioCapstoneModel = GetBioCapstoneDetails(_transactionId);
                        //FileHandling.ReadWrite2Files fh1 = new FileHandling.ReadWrite2Files();
                        //applicationModel.BioCapstoneModel = fh1.DeSerializeObject<BioCapstoneViewModel>("D:\\BioMedial_Test.BM");
                        break;
                    case 35:
                        applicationModel.HomeopathyDrugStore = GetHomeopathyData(_transactionId);
                        //FileHandling.ReadWrite2Files fhHomeopathy = new FileHandling.ReadWrite2Files();
                        //applicationModel.HomeopathyDrugStore = fhHomeopathy.DeSerializeObject<HomeopathyDrugStoreViewModel>("D:\\Homeopathy_TestDetails.HD");
                        break;
                    case 36:
                        objApplicationBAL = new ApplicationBAL();
                        applicationModel.AllopathicDrugModel = objApplicationBAL.GetAllopathicDetails(_transactionId);
                        applicationModel.AllopathicDrugModel.ServiceId = 36;

                        break;
                    case 37:
                      objApplicationBAL = new ApplicationBAL();
                        applicationModel.AllopathicDrugModel = objApplicationBAL.GetAllopathicDetails(_transactionId);
                        applicationModel.AllopathicDrugModel.ServiceId = 37;

                        break;
                    case 18:
                       
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.FacilityLogModel = objBAL.GetFacility(_transactionId);
                        applicationModel.PCPNDTModel.TestsModelLog = objBAL.GetPCPNDTTests(_transactionId, "InDirect");
                        applicationModel.PCPNDTModel.FacilitiesModellog = objBAL.GetFacilitiesforTests(_transactionId, "InDirect");
                        break;
                    case 19:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.InstitutionModelLog = objBAL.GetOwnership(_transactionId);
                        break;
                    case 20:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.InstitutionModelLog = objBAL.GetInstitution(_transactionId);
                        break;

                    case 21:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.TestsModelLog = objBAL.GetPCPNDTTests(_transactionId, "Direct");
                        break;
                
                    case 22:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.EquipmentListLog = objBAL.GetEquipments(_transactionId);
                        break;
                    case 23:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.FacilitiesModellog = objBAL.GetFacilitiesforTests(_transactionId, "Direct");
                        break;
                    case 24:
                        {
                            applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                            applicationModel.PCPNDTModel.ServiceType = "Amendment";
                            applicationModel.PCPNDTModel.ServiceId = _serviceId;
                            applicationModel.PCPNDTModel.EmployeeListLog = objBAL.GetEmployees(_transactionId);
                            
                        }
                        break;
                    case 28:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.cancelLiceseModel = objBAL.GetCancelLicenseDetails(_transactionId);
                        break;
                    case 38:
                        applicationModel.PCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        applicationModel.PCPNDTModel.ServiceType = "Amendment";
                        applicationModel.PCPNDTModel.ServiceId = _serviceId;
                        applicationModel.PCPNDTModel.NocforEquipmentModel = objBAL.GetNOCofEquipment(_transactionId);
                     

                        break;
                    default:
                        break;
                }
            }

            return applicationModel;

        }

        public APMCEViewModel GetAPMCEFilesData(int transactionId, string type)
        {
            objDAL = new LicenseDAL();
            DataSet dsItems = objDAL.GetAPMCEData(transactionId, type);
            if (dsItems == null)
                return null;

            APMCEViewModel apmceModel = new APMCEViewModel();

            apmceModel.APMCECertificateModel.ApplicationNumber = dsItems.Tables[13].Rows[0][0].ToString();
            if (!string.IsNullOrEmpty(dsItems.Tables[13].Rows[0][2].ToString()))
                apmceModel.APMCECertificateModel.ApplicationDate = Convert.ToDateTime(dsItems.Tables[13].Rows[0][2].ToString());

            #region Registration details
            RegistrationViewModel registrationModel = new RegistrationViewModel();
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[0].Rows[0];
                registrationModel.Id = (int)row["Id"];
                registrationModel.ApplicantPhoto = row["ApplicantPhotoPath"].ToString();
                registrationModel.AadharCardPath = row["AadharCardPath"].ToString();
                registrationModel.PANCardPath = row["PANCardPath"].ToString();
                registrationModel.HospitalPhoto = row["HospitalPhoto"].ToString();
                registrationModel.BioCapstoneWaste = row["BioCapstoneWaste"].ToString();
                registrationModel.TariffBoardPhoto = row["TariffBoardPhoto"].ToString();
                registrationModel.ConsultantsListPhoto = row["ConsultantsListPhoto"].ToString();
                registrationModel.FireExtinguisherPhoto = row["FireExtinguisherPhoto"].ToString();
                registrationModel.FireNOC = row["FireNOC"].ToString();
            }
            apmceModel.RegistrationModel = registrationModel;
            #endregion 

            #region Corresponding Address Details
            CorrespondingAddressViewModel correspondingAddressModel = new CorrespondingAddressViewModel();
            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[1].Rows[0];
                correspondingAddressModel.Id = Convert.ToInt32(row["Id"]);
                
                correspondingAddressModel.AadharCardPath = row["AadharCardPath"].ToString();
                correspondingAddressModel.PANCardPath = row["PANCardPath"].ToString();
                correspondingAddressModel.EducationCertificate = row["EducationCertificate"].ToString();
                correspondingAddressModel.IMAMembership = row["IMAMembership"].ToString();
                correspondingAddressModel.TAXReceipt = row["TAXReceipt"].ToString();
                correspondingAddressModel.THANA_APNA_Registration = row["THANA_APNA_Registration"].ToString();
                correspondingAddressModel.APMCR_TSMCR_MCI = row["APMCR_TSMCR_MCI"].ToString();
                correspondingAddressModel.CorrespondentPhoto = row["CorrespondentPhoto"].ToString();
                correspondingAddressModel.CorrespondentSignature = row["CorrespondentSignature"].ToString();
            }
            apmceModel.CorrespondingAddress = correspondingAddressModel;
            #endregion

            #region Trust Details
            TrustViewModel trustModel = new TrustViewModel();
            if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[2].Rows[0];
                trustModel.Id = Convert.ToInt32(row["Id"]);
                trustModel.Name = row["Name"].ToString();
                trustModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                trustModel.DistrictName = row["DistrictName"].ToString();
                trustModel.MandalId = Convert.ToInt32(row["MandalId"]);
                trustModel.MandalName = row["MandalName"].ToString();
                trustModel.VillageId = Convert.ToInt32(row["VillageId"]);
                trustModel.VillageName = row["VillageName"].ToString();
                trustModel.HouseNumber = row["HouseNumber"].ToString();
                trustModel.StreetName = row["StreetName"].ToString();
                trustModel.PINCode = row["PINCode"].ToString();
                trustModel.EstablishedDate = Convert.ToDateTime(row["EstablishedDate"]);
                trustModel.Id = (int)row["Id"];
                trustModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
            }
            apmceModel.TrustModel = trustModel;
            #endregion

            #region Accommodation details
            AccommadationViewModel AccommadationModel = new AccommadationViewModel();
            if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[3].Rows[0];
                AccommadationModel.EstablishmentTypeName = row["EstablishmentOwnedBy"].ToString();
                AccommadationModel.UploadedFilePath = row["UploadedDoc"].ToString();
                AccommadationModel.Id = (int)row["Id"];
            }
            apmceModel.Accommadation = AccommadationModel;
            #endregion

            #region Establishment Details
            EstablishmentModel establishmentModel = new EstablishmentModel();
            if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[4].Rows[0];
                establishmentModel.EstablishmentDate = Convert.ToDateTime(row["EstablishmentDate"]);
                establishmentModel.OpenArea = Convert.ToDecimal(row["OpenArea"]);
                establishmentModel.ConstructionArea = Convert.ToDecimal(row["ConstructionArea"]);
                establishmentModel.OpenAreaFilePath = row["OpenAreaDocPath"].ToString();
                establishmentModel.ConstructionAreaFilePath = row["ConstructionAreaDocPath"].ToString();
                establishmentModel.Id = (int)row["Id"];
            }
            apmceModel.EstablishmentModel = establishmentModel;
            #endregion

            #region Services Offered Details
            OfferedServicesModel offeredsserviceModel = new OfferedServicesModel();
            if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[5].Rows[0];
                offeredsserviceModel.BedStrength = Convert.ToInt32(row["BedStrength"]);
                offeredsserviceModel.OfferedServices = row["OfferedService"].ToString();
                offeredsserviceModel.OfferedServiceIDs = row["OfferedServicesIDs"].ToString();  // added by Chandu fro DDL bind 24-07-2017
                offeredsserviceModel.Id = (int)(row["Id"]);
                offeredsserviceModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
            }
            apmceModel.OfferedServices = offeredsserviceModel;
            #endregion

            #region Staff Details
            List<StaffDetailsModel> staffList = new List<StaffDetailsModel>();

            if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
            {
                StaffDetailsModel staffdetailsModel;
                foreach (DataRow row in dsItems.Tables[6].Rows)
                {
                    staffdetailsModel = new StaffDetailsModel();
                    staffdetailsModel.Id = Convert.ToInt32(row["Id"]);
                    staffdetailsModel.Name = Convert.ToString(row["Name"]);
                    staffdetailsModel.UploadedFilePath = Convert.ToString(row["StaffDetailsDocPath"]);
                    staffList.Add(staffdetailsModel);
                }
            }


            //StaffDetailsViewModel staffdetailsModel = new StaffDetailsViewModel();
            //if(dsItems.Tables[6] !=null && dsItems.Tables[6].Rows.Count > 0)
            //{
            //    DataRow row = dsItems.Tables[6].Rows[0];
            //    staffdetailsModel.StaffDesignation = row["Designation"].ToString();
            //    staffdetailsModel.Name = row["DoctorName"].ToString();
            //    staffdetailsModel.RegistrationNumber = row["RegistrationNo"].ToString();
            //    staffdetailsModel.UploadedFilePath = row["UploadPath"].ToString();
            //    staffdetailsModel.PhoneNumber = row["PhoneNumber"].ToString();
            //    staffdetailsModel.Email = row["EmailId"].ToString();

            //    staffdetailsModel.Id = Convert.ToInt32(row["Id"]);
            //    staffdetailsModel.FormStatus = (FormStatus)row["FormStatus"];
            //}

            #endregion

            #region Equipment Furniture Details
            List<InfraStructureModel> furnitureList = new List<InfraStructureModel>();

            if (dsItems.Tables[7] != null && dsItems.Tables[7].Rows.Count > 0)
            {

                foreach (DataRow row in dsItems.Tables[7].Rows)
                {
                    InfraStructureModel infraFurnitureModel = new InfraStructureModel();
                    infraFurnitureModel.EquipmentId = Convert.ToInt32(row["EquipmentId"]);
                    infraFurnitureModel.Name = row["EquipmentName"].ToString();
                    infraFurnitureModel.ItemModel = row["Model"].ToString();
                    infraFurnitureModel.UploadedFilePath = row["UploadDoc"].ToString();
                    infraFurnitureModel.Id = Convert.ToInt32(row["Id"]);
                    furnitureList.Add(infraFurnitureModel);
                }
            }

            #endregion

            #region Facilities Avaliable
            FacilitiesAvailableModel facilityModel = new FacilitiesAvailableModel();
            if (dsItems.Tables[8] != null && dsItems.Tables[8].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[8].Rows[0];
                facilityModel.HasLaborRoom = Convert.ToBoolean(row["LaborFacility"]);
                facilityModel.HasOperationTheater = Convert.ToBoolean(row["OperationTheater"]);
                facilityModel.OperationTheatreCount = Convert.ToInt32(row["OperationTheatreCount"]);
                facilityModel.HasDiagnosticFacility = Convert.ToBoolean(row["DiagnosticksFacility"]);
                facilityModel.HasDeclarationStamp = (bool)row["HasDeclarationStamp"];
                facilityModel.DeclarationStampFilePath = row["NonJudicialStamp"].ToString();
                if (row["NonJudicialStamp"].ToString() == "")
                {
                    facilityModel.DeclarationStampFilePath = null;
                }
                facilityModel.OtherInformationDescription = row["OtherInformation"].ToString();
                facilityModel.OtherInformationDocumentPath = row["OtherDocs"].ToString();
                facilityModel.Id = (int)row["Id"];
            }
            apmceModel.FacilitiesAvailableModel = facilityModel;
            #endregion

            #region Rejection Details
            if (dsItems.Tables[10] != null && dsItems.Tables[10].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[10].Rows[0];
                apmceModel.RejectionRemarks = row["RejectionRemarks"].ToString();
            }
            #endregion

            #region Additional Documents Details
            AdditionalDocumentsModel additionalDocumentsModel = new AdditionalDocumentsModel();
            if (dsItems.Tables[14] != null && dsItems.Tables[14].Rows.Count > 0)
            {
                foreach (DataRow row in dsItems.Tables[14].Rows)
                {
                    if (row["UploadType"].ToString() == "AD_BioCapstoneWastageClearanceFromTSGovt")
                        additionalDocumentsModel.BioCapstoneWastageClearanceFromFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_PollutionAuthorityLetterbyPCB")
                        additionalDocumentsModel.PollutionAuthorityLetterByPCBFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_ConsentForOperation")
                        additionalDocumentsModel.CFOFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_STP_File")
                        additionalDocumentsModel.STPFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_Fire_NOC_Copy")
                        additionalDocumentsModel.FEReportFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_TariffList")
                        additionalDocumentsModel.TarifListFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_EstablishmentBuildPlan")
                        additionalDocumentsModel.Establishment_BuildingPlanFilepath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalNamePlate")
                        additionalDocumentsModel.HospitalOutSideNamePlateBuildingFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalTariffBoard")
                        additionalDocumentsModel.TariffBoardFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalFireExhaustive")
                        additionalDocumentsModel.FireExhaustiveFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalLabOperationTheater")
                        additionalDocumentsModel.HospitalLabOperationTheaterFilePath = Convert.ToString(row["DocumentPath"]);
                }                             
            }
            apmceModel.AdditionalDocumentsModel = additionalDocumentsModel;
            #endregion

            apmceModel.TransactionId = transactionId;
            apmceModel.StaffDetailsList = staffList;
            apmceModel.InfraStructureList = furnitureList;            
            return apmceModel;
        }
        public APMCEViewModel GetAPMCEData(int transactionId ,string type)
        {
            objDAL = new LicenseDAL();
            DataSet dsItems = objDAL.GetAPMCEData(transactionId, type);
            if (dsItems == null)
                return null;

            APMCEViewModel apmceModel = new APMCEViewModel();

            apmceModel.APMCECertificateModel.ApplicationNumber = dsItems.Tables[13].Rows[0][0].ToString();
            if(!string.IsNullOrEmpty(dsItems.Tables[13].Rows[0][2].ToString()))
                apmceModel.APMCECertificateModel.ApplicationDate = Convert.ToDateTime(dsItems.Tables[13].Rows[0][2].ToString());
            
            #region Registration details
            RegistrationViewModel registrationModel = new RegistrationViewModel();
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[0].Rows[0];
                registrationModel.FacilityType = row["FacilityType"].ToString();
                registrationModel.Name = row["RegistrationName"].ToString();
                registrationModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                registrationModel.DistrictName = row["DistrictName"].ToString();
                registrationModel.MandalId = Convert.ToInt32(row["MandalId"]);
                registrationModel.MandalName = row["MandalName"].ToString();
                registrationModel.VillageId = Convert.ToInt32(row["VillageId"]);
                registrationModel.VillageName = row["VillageName"].ToString();
                registrationModel.HouseNumber = row["HouseNumber"].ToString();
                registrationModel.StreetName = row["StreetName"].ToString();
                registrationModel.PINCode = row["PINCode"].ToString();
                registrationModel.Id = (int)row["Id"];
                registrationModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
                registrationModel.HospitalTypeId = Convert.ToInt32(row["HospitalTypeId"]);
                registrationModel.HospitalType= row["HospitalType"].ToString();
                registrationModel.ClinicType= row["ClinicType"].ToString();
                registrationModel.BedStrength = row["BedStrength"].ToString();
                registrationModel.Speciality = row["Speciality"].ToString();

                registrationModel.BuildingHeight = Convert.ToInt32(row["BuildingHeight"]);

                registrationModel.ApplicantPhoto = row["ApplicantPhotoPath"].ToString();
                registrationModel.AadharCardPath = row["AadharCardPath"].ToString();
                registrationModel.PANCardPath = row["PANCardPath"].ToString();
            }
            apmceModel.RegistrationModel = registrationModel;
            #endregion 

            #region Corresponding Address Details
            CorrespondingAddressViewModel correspondingAddressModel = new CorrespondingAddressViewModel();
            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[1].Rows[0];
                correspondingAddressModel.Id = Convert.ToInt32(row["Id"]);
                correspondingAddressModel.Name = row["Name"].ToString();
                correspondingAddressModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                correspondingAddressModel.DistrictName = row["DistrictName"].ToString();
                correspondingAddressModel.MandalId = Convert.ToInt32(row["MandalId"]);
                correspondingAddressModel.MandalName = row["MandalName"].ToString();
                correspondingAddressModel.VillageId = Convert.ToInt32(row["VillageId"]);
                correspondingAddressModel.VillageName = row["VillageName"].ToString();
                correspondingAddressModel.HouseNumber = row["HouseNumber"].ToString();
                correspondingAddressModel.StreetName = row["StreetName"].ToString();
                correspondingAddressModel.PINCode = row["PINCode"].ToString();
                correspondingAddressModel.Id = (int)row["Id"];
                correspondingAddressModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]); ;
            }
            apmceModel.CorrespondingAddress = correspondingAddressModel;
            #endregion

            #region Trust Details
            TrustViewModel trustModel = new TrustViewModel();
            if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[2].Rows[0];
                trustModel.Id = Convert.ToInt32(row["Id"]);
                trustModel.Name = row["Name"].ToString();
                trustModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                trustModel.DistrictName = row["DistrictName"].ToString();
                trustModel.MandalId = Convert.ToInt32(row["MandalId"]);
                trustModel.MandalName = row["MandalName"].ToString();
                trustModel.VillageId = Convert.ToInt32(row["VillageId"]);
                trustModel.VillageName = row["VillageName"].ToString();
                trustModel.HouseNumber = row["HouseNumber"].ToString();
                trustModel.StreetName = row["StreetName"].ToString();
                trustModel.PINCode = row["PINCode"].ToString();
                trustModel.EstablishedDate = Convert.ToDateTime(row["EstablishedDate"]);
                trustModel.Id = (int)row["Id"];
                trustModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
            }
            apmceModel.TrustModel = trustModel;
            #endregion

            #region Accommodation details
            AccommadationViewModel AccommadationModel = new AccommadationViewModel();
            if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[3].Rows[0];
                AccommadationModel.EstablishmentTypeName = row["EstablishmentOwnedBy"].ToString();
                AccommadationModel.FromDate = Convert.ToDateTime(row["FromDate"]);
                AccommadationModel.ToDate = Convert.ToDateTime(row["ToDate"]);
                AccommadationModel.UploadedFilePath = row["UploadedDoc"].ToString();
                AccommadationModel.Id = (int)row["Id"];
                AccommadationModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
            }
            apmceModel.Accommadation = AccommadationModel;
            #endregion

            #region Establishment Details
            EstablishmentModel establishmentModel = new EstablishmentModel();
            if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[4].Rows[0];
                establishmentModel.EstablishmentDate = Convert.ToDateTime(row["EstablishmentDate"]);
                establishmentModel.OpenArea = Convert.ToDecimal(row["OpenArea"]);
                establishmentModel.ConstructionArea = Convert.ToDecimal(row["ConstructionArea"]);
                establishmentModel.OpenAreaFilePath = row["OpenAreaDocPath"].ToString();
                establishmentModel.ConstructionAreaFilePath = row["ConstructionAreaDocPath"].ToString();
                establishmentModel.Id = (int)row["Id"];
                establishmentModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
            }
            apmceModel.EstablishmentModel = establishmentModel;
            #endregion

            #region Services Offered Details
            OfferedServicesModel offeredsserviceModel = new OfferedServicesModel();
            if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[5].Rows[0];
                offeredsserviceModel.BedStrength = Convert.ToInt32(row["BedStrength"]);
                offeredsserviceModel.OfferedServices = row["OfferedService"].ToString();
                offeredsserviceModel.OfferedServiceIDs = row["OfferedServicesIDs"].ToString();  // added by Chandu fro DDL bind 24-07-2017
                offeredsserviceModel.Id = (int)(row["Id"]);
                offeredsserviceModel.FormStatus = (FormStatus)(row["FormStatus"] == null ? 0 : row["FormStatus"]);
            }
            apmceModel.OfferedServices = offeredsserviceModel;
            #endregion

            #region Staff Details
            List<StaffDetailsModel> staffList = new List<StaffDetailsModel>();

            if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
            {
                StaffDetailsModel staffdetailsModel;
                foreach (DataRow row in dsItems.Tables[6].Rows)
                {
                    staffdetailsModel = new StaffDetailsModel();
                    staffdetailsModel.Id = Convert.ToInt32(row["Id"]);
                    staffdetailsModel.Name = Convert.ToString(row["Name"]);
                    staffdetailsModel.StaffDesignation = Convert.ToString(row["Designation"]);
                    staffdetailsModel.RegistrationNumber = Convert.ToString(row["RegistrationNo"]);
                    staffdetailsModel.Speciality = Convert.ToString(row["Speciality"]);
                    staffdetailsModel.UploadedFilePath = Convert.ToString(row["StaffDetailsDocPath"]);
                    staffdetailsModel.PhoneNumber = Convert.ToString(row["PhoneNumber"]);
                    staffdetailsModel.Email = Convert.ToString(row["EmailId"]);
                    staffList.Add(staffdetailsModel);
                }
            }
           

            //StaffDetailsViewModel staffdetailsModel = new StaffDetailsViewModel();
            //if(dsItems.Tables[6] !=null && dsItems.Tables[6].Rows.Count > 0)
            //{
            //    DataRow row = dsItems.Tables[6].Rows[0];
            //    staffdetailsModel.StaffDesignation = row["Designation"].ToString();
            //    staffdetailsModel.Name = row["DoctorName"].ToString();
            //    staffdetailsModel.RegistrationNumber = row["RegistrationNo"].ToString();
            //    staffdetailsModel.UploadedFilePath = row["UploadPath"].ToString();
            //    staffdetailsModel.PhoneNumber = row["PhoneNumber"].ToString();
            //    staffdetailsModel.Email = row["EmailId"].ToString();

            //    staffdetailsModel.Id = Convert.ToInt32(row["Id"]);
            //    staffdetailsModel.FormStatus = (FormStatus)row["FormStatus"];
            //}

            #endregion

            #region Equipment Furniture Details
            List<InfraStructureModel> furnitureList = new List<InfraStructureModel>();

            if (dsItems.Tables[7] != null && dsItems.Tables[7].Rows.Count > 0)
            {
               
                foreach (DataRow row in dsItems.Tables[7].Rows)
                {
                    InfraStructureModel infraFurnitureModel = new InfraStructureModel();
                    infraFurnitureModel.EquipmentId = Convert.ToInt32(row["EquipmentId"]);
                    infraFurnitureModel.Name = row["EquipmentName"].ToString();
                    infraFurnitureModel.Quantity = Convert.ToInt32(row["Quantity"]);
                    infraFurnitureModel.ItemModel = row["Model"].ToString();
                    infraFurnitureModel.Remarks = row["Remarks"].ToString();
                    infraFurnitureModel.UploadedFilePath = row["UploadDoc"].ToString();
                    infraFurnitureModel.Id = Convert.ToInt32(row["Id"]);
                    furnitureList.Add(infraFurnitureModel);
                }
            }
          
            #endregion

            #region Facilities Avaliable
            FacilitiesAvailableModel facilityModel = new FacilitiesAvailableModel();
            if (dsItems.Tables[8] != null && dsItems.Tables[8].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[8].Rows[0];
                facilityModel.HasLaborRoom = Convert.ToBoolean(row["LaborFacility"]);
                facilityModel.HasOperationTheater = Convert.ToBoolean(row["OperationTheater"]);
                facilityModel.OperationTheatreCount = Convert.ToInt32(row["OperationTheatreCount"]);
                facilityModel.HasDiagnosticFacility = Convert.ToBoolean(row["DiagnosticksFacility"]);
                facilityModel.HasDeclarationStamp = (bool)row["HasDeclarationStamp"];
                facilityModel.DeclarationStampFilePath = row["NonJudicialStamp"].ToString();
                if (row["NonJudicialStamp"].ToString() == "")
                {
                    facilityModel.DeclarationStampFilePath = null;
                }
                facilityModel.OtherInformationDescription = row["OtherInformation"].ToString();
                facilityModel.OtherInformationDocumentPath = row["OtherDocs"].ToString();
                facilityModel.Id = (int)row["Id"];
                facilityModel.FormStatus = (FormStatus)row["FormStatus"];
            }
            apmceModel.FacilitiesAvailableModel = facilityModel;
            #endregion

            #region Rejection Details
            if (dsItems.Tables[10] != null && dsItems.Tables[10].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[10].Rows[0];
                apmceModel.RejectionRemarks = row["RejectionRemarks"].ToString();
            }
            #endregion

            #region Additional Documents Details
            AdditionalDocumentsModel additionalDocumentsModel = new AdditionalDocumentsModel();
            if (dsItems.Tables[14] != null && dsItems.Tables[14].Rows.Count > 0)
            {                
                foreach (DataRow row in dsItems.Tables[14].Rows)
                {
                    if (row["UploadType"].ToString() == "AD_BioCapstoneWastageClearanceFromTSGovt")
                    {
                        additionalDocumentsModel.BioCapstoneWastageClearanceFromFilePath = Convert.ToString(row["DocumentPath"]);
                        additionalDocumentsModel.BioCapstoneValidupto = Convert.ToDateTime(row["Validupto"]);
                    }
                    if (row["UploadType"].ToString() == "AD_PollutionAuthorityLetterbyPCB")
                    {
                        additionalDocumentsModel.PollutionAuthorityLetterByPCBFilePath = Convert.ToString(row["DocumentPath"]);
                        additionalDocumentsModel.PollutionAuthorityValidupto = Convert.ToDateTime(row["Validupto"]);
                    }
                    if (row["UploadType"].ToString() == "AD_ConsentForOperation")
                        additionalDocumentsModel.CFOFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_STP_File")
                        additionalDocumentsModel.STPFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_Fire_NOC_Copy")
                    {
                        additionalDocumentsModel.FEReportFilePath = Convert.ToString(row["DocumentPath"]);
                        additionalDocumentsModel.FireNOCValidupto = Convert.ToDateTime(row["Validupto"]);
                    }
                    if (row["UploadType"].ToString() == "AD_TariffList")
                        additionalDocumentsModel.TarifListFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_EstablishmentBuildPlan")
                        additionalDocumentsModel.Establishment_BuildingPlanFilepath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalNamePlate")
                        additionalDocumentsModel.HospitalOutSideNamePlateBuildingFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalTariffBoard")
                        additionalDocumentsModel.TariffBoardFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalFireExhaustive")
                        additionalDocumentsModel.FireExhaustiveFilePath = Convert.ToString(row["DocumentPath"]);
                    if (row["UploadType"].ToString() == "AD_HospitalLabOperationTheater")
                        additionalDocumentsModel.HospitalLabOperationTheaterFilePath = Convert.ToString(row["DocumentPath"]);
                }

                if(additionalDocumentsModel.BioCapstoneWastageClearanceFromFilePath != null && additionalDocumentsModel.PollutionAuthorityLetterByPCBFilePath != null &&
                   additionalDocumentsModel.CFOFilePath != null && additionalDocumentsModel.STPFilePath != null && additionalDocumentsModel.FEReportFilePath != null &&
                   additionalDocumentsModel.TarifListFilePath != null && additionalDocumentsModel.Establishment_BuildingPlanFilepath != null &&
                   additionalDocumentsModel.HospitalOutSideNamePlateBuildingFilePath != null && additionalDocumentsModel.TariffBoardFilePath != null &&
                   additionalDocumentsModel.FireExhaustiveFilePath != null && additionalDocumentsModel.HospitalLabOperationTheaterFilePath != null)
                {
                    additionalDocumentsModel.FormStatus = (FormStatus)(2); // completed
                }
                else
                    additionalDocumentsModel.FormStatus = (FormStatus)(1);  // partially Saved               
            }
            apmceModel.AdditionalDocumentsModel = additionalDocumentsModel;
            #endregion

            //apmceModel.APMCECertificateModel.ApplicationNumber=
            apmceModel.TransactionId = transactionId;
            apmceModel.StaffDetailsList = staffList;
            //if (apmceModel.StaffDetailsList.Count > 0)
            //    apmceModel.StaffDetailsmodel.FormStatus = FormStatus.Completed;
            apmceModel.InfraStructureList = furnitureList;
            //if (apmceModel.InfraStructureList.Count > 0)
            //    apmceModel.InfraStructure.FormStatus = FormStatus.Completed;
            

            return apmceModel;
        }
        public PCPNDTViewModel GetPCPNDTData(int transactionId,string type)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetPCPNDTData(transactionId, type);
                if (dsItems == null)
                    return null;
                PCPNDTViewModel pcpndtModel = new PCPNDTViewModel();

                #region Applicant Details
                ApplicantViewModel applicantModel = new ApplicantViewModel();
                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[0].Rows[0];
                    applicantModel.Id = Convert.ToInt32(row["Id"]);
                    applicantModel.Name = row["Name"].ToString();
                    applicantModel.ApplicantRole = row["ApplicantRole"].ToString();
                    applicantModel.ApplicantRoleOther = row["ApplicantRoleOther"].ToString();
                    applicantModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    applicantModel.DistrictName = row["DistrictName"].ToString();
                    applicantModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    applicantModel.MandalName = row["MandalName"].ToString();
                    applicantModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    applicantModel.VillageName = row["VillageName"].ToString();
                    applicantModel.Aadhar = row["Aadhar"].ToString();
                    applicantModel.PAN = row["PAN"].ToString();
                    applicantModel.HouseNumber = row["HouseNumber"].ToString();
                    applicantModel.StreetName = row["StreetName"].ToString();
                    applicantModel.PINCode = row["PINCode"].ToString();
                    applicantModel.ApplicantPhoto = row["ApplicantPhotoPath"].ToString();
                    applicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);

                    applicantModel.AadharCardPath = row["AadharCardPath"].ToString();
                    applicantModel.PANCardPath = row["PANCardPath"].ToString();
                }
                #endregion

                #region Establishment Details - Not in Use
                //EstablishmentViewModel establishmentModel = new EstablishmentViewModel();
                //if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                //{
                //    DataRow row = dsItems.Tables[1].Rows[0];
                //    establishmentModel.Id = Convert.ToInt32(row["Id"]);
                //    establishmentModel.Name = row["Name"].ToString();
                //    establishmentModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                //    establishmentModel.DistrictName = row["DistrictName"].ToString();
                //    establishmentModel.MandalId = Convert.ToInt32(row["MandalId"]);
                //    establishmentModel.MandalName = row["MandalName"].ToString();
                //    establishmentModel.VillageId = Convert.ToInt32(row["VillageId"]);
                //    establishmentModel.VillageName = row["VillageName"].ToString();
                //    establishmentModel.HouseNumber = row["HouseNumber"].ToString();
                //    establishmentModel.StreetName = row["StreetName"].ToString();
                //    establishmentModel.PINCode = row["PINCode"].ToString();
                //    establishmentModel.AddressProofPath = row["AddressProofDocPath"].ToString();
                //    establishmentModel.BuildingLayoutPath = row["BuildingLayoutDocPath"].ToString();
                //    establishmentModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                //}
                //pcpndtModel.EstablishmentModel = establishmentModel;
                #endregion

                #region Facility Details
                FacilityViewModel facilityModel = new FacilityViewModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    facilityModel.Id = Convert.ToInt32(row["Id"]);
                    facilityModel.Name = Convert.ToString(row["Name"]);
                    facilityModel.Faclities = Convert.ToString(row["Facilities"]);
                    facilityModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    facilityModel.DistrictName = Convert.ToString(row["DistrictName"]);
                    facilityModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    facilityModel.MandalName = Convert.ToString(row["MandalName"]);
                    facilityModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    facilityModel.VillageName = Convert.ToString(row["VillageName"]);
                    facilityModel.HouseNumber = Convert.ToString(row["HouseNumber"]);
                    facilityModel.StreetName = Convert.ToString(row["StreetName"]);
                    facilityModel.Phone = Convert.ToString(row["Phone"]);
                    facilityModel.Email = Convert.ToString(row["Email"]);
                    facilityModel.Fax = Convert.ToString(row["Fax"]);
                    facilityModel.Telegraph = Convert.ToString(row["Telegraph"]);
                    facilityModel.Telex = Convert.ToString(row["Telex"]);
                    facilityModel.PINCode = Convert.ToString(row["PINCode"]);
                    facilityModel.AddressProofType = Convert.ToString(row["AddressProofType"]);
                    facilityModel.AddressProofPath = Convert.ToString(row["AddressProofDocPath"]);
                    facilityModel.BuildingLayoutPath = Convert.ToString(row["BuildingLayoutDocPath"]);
                    facilityModel.OwnershipType = Convert.ToString(row["OwnershipType"]);
                    facilityModel.OwnerShipPath = Convert.ToString(row["OwnershipDocPath"]);
                    facilityModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                pcpndtModel.FacilityModel = facilityModel;
                #endregion

                #region Tests Details
                TestsModel testsModel = new TestsModel();
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[2].Rows[0];
                    testsModel.Id = Convert.ToInt32(row["Id"]);
                    testsModel.InvasiveTests = row["InvasiveTests"].ToString();
                    testsModel.NonInvasiveTests = row["NonInvasiveTests"].ToString();
                    testsModel.Remarks = row["Remarks"].ToString();
                    testsModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                #endregion

                #region Equipment Details
                List<EquipmentModel> equipmentList = new List<EquipmentModel>();
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    EquipmentModel equipment;
                    foreach (DataRow row in dsItems.Tables[3].Rows)
                    {
                        equipment = new EquipmentModel();
                        equipment.Id = Convert.ToInt32(row["Id"]);
                        equipment.Name = row["Name"].ToString();
                        equipment.SerialNumber = row["SerialNumber"].ToString();
                        equipment.MachineModel = row["MachineModel"].ToString();
                        equipment.Make = row["Make"].ToString();
                        equipment.Type = row["Type"].ToString();
                        equipment.UploadedFilePath = row["UploadedFilePath"].ToString();
                        equipment.NocFilePath = row["NOCFliePath"].ToString();
                        equipment.TransferCertificatePath = row["TransferCertificateFilePath"].ToString();
                        equipment.InstallationCerticatePath = row["InstallationCertificatePath"].ToString();
                        equipment.InvoicePath = row["InvoicePath"].ToString();
                        equipment.ImagePath = row["ImagePath"].ToString();
                        equipmentList.Add(equipment);
                    }
                }
                #endregion

                #region Facilities Details
                FacilitesModel facilitiesModel = new FacilitesModel();
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[4].Rows[0];
                    facilitiesModel.Id = Convert.ToInt32(row["Id"]);
                    facilitiesModel.Tests = row["Tests"].ToString();
                    facilitiesModel.Studies = row["Studies"].ToString();
                    facilitiesModel.Remarks = row["Remarks"].ToString();
                    facilitiesModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                #endregion

                #region Employee Details
                List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
                if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
                {
                    EmployeeViewModel employee;
                    foreach (DataRow row in dsItems.Tables[5].Rows)
                    {
                        employee = new EmployeeViewModel();
                        employee.Id = Convert.ToInt32(row["Id"]);
                        employee.Name = row["Name"].ToString();
                        employee.DesignationId = Convert.ToInt32(row["DesignationId"]);
                        employee.DesignationName = Convert.ToString(row["DesignationName"]);
                        employee.SubDesignation = row["SubDesignation"].ToString();
                        employee.Experience = row["Experience"].ToString();
                        employee.ExpYears = Convert.ToInt32(row["ExpYears"]);
                        employee.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                        employee.ExpDays = Convert.ToInt32(row["ExpDays"]);
                        employee.RegistrationNumber = row["RegistrationNumber"].ToString();
                        employee.UploadedFilePath = row["UploadedFilePath"].ToString();
                        employeeList.Add(employee);
                    }
                }
                #endregion

                #region Institution Details
                InstitutionViewModel institutionModel = new InstitutionViewModel();
                if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[6].Rows[0];
                    institutionModel.Id = Convert.ToInt32(row["Id"]);
                    institutionModel.OwnershipTypeId = Convert.ToInt32(row["OwnershipTypeId"]);
                    institutionModel.OwnershipTypeName = row["OwnershipTypeName"].ToString();
                    institutionModel.InstitutionTypeId = Convert.ToInt32(row["InstitutionTypeId"]);
                    institutionModel.InstitutionTypeName = row["InstitutionTypeName"].ToString();
                    institutionModel.TotalWorkArea = row["TotalWorkArea"] != DBNull.Value ?
                        Convert.ToDecimal(row["TotalWorkArea"]) : 0;
                    institutionModel.OwnershipOthers = row["OwnershipOther"].ToString();
                    institutionModel.InstitutionOthers = row["InstitutionOther"].ToString();
                    institutionModel.AffidavitDocPath = Convert.ToString(row["AffidavitDocPath"]);
                    institutionModel.ArticleDocPath = Convert.ToString(row["ArticleDocPath"]);
                    institutionModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);

                    if (dsItems.Tables[8] != null && dsItems.Tables[8].Rows.Count > 0)
                    {
                        institutionModel.StudyCertificateDocPaths = new List<DocumentUploadModel>();
                        DocumentUploadModel documentModel;
                        foreach (DataRow docRow in dsItems.Tables[8].Rows)
                        {
                            documentModel = new DocumentUploadModel();
                            documentModel.Id = Convert.ToInt32(docRow["Id"]);
                            documentModel.DocumentPath = Convert.ToString(docRow["DocumentPath"]);
                            institutionModel.StudyCertificateDocPaths.Add(documentModel);
                        }
                    }


                }
                #endregion

                #region Declaration Details
                DeclarationModel declarationModel = new DeclarationModel();
                if (dsItems.Tables[7] != null && dsItems.Tables[7].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[7].Rows[0];
                    declarationModel.Id = Convert.ToInt32(row["Id"]);
                    declarationModel.Name = row["Name"].ToString();
                    declarationModel.SonOf = row["SonOf"].ToString();
                    declarationModel.Age = row["Age"] != DBNull.Value ? Convert.ToInt32(row["Age"]) : 0;
                    declarationModel.ResidentOf = row["ResidentOf"].ToString();
                    declarationModel.Designation = row["Designation"].ToString();
                    declarationModel.Organization = row["Organization"].ToString();
                    declarationModel.Date = row["Date"] != DBNull.Value ? Convert.ToDateTime(row["Date"]) : default(DateTime);
                    declarationModel.Place = row["Place"].ToString();
                    declarationModel.Signature = row["Signature"].ToString();
                    declarationModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                    declarationModel.SignatureDocPath = row["SignatureDocPath"].ToString();
                }
                #endregion

                #region Rejection Details
                if (dsItems.Tables[9] != null && dsItems.Tables[9].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[9].Rows[0];
                    pcpndtModel.RejectionRemarks = row["RejectionRemarks"].ToString();
                }
                #endregion

                #region Other upload Details
                List<DocumentUploadModel> otheruploadList = new List<DocumentUploadModel>();
              //  pcpndtModel.DeclarationModel.OtherUploadsList = new List<DocumentUploadModel>();
                if (dsItems.Tables[11] != null && dsItems.Tables[11].Rows.Count > 0)
                {
                    DocumentUploadModel others;
                    foreach (DataRow row in dsItems.Tables[11].Rows)
                    {
                        others = new DocumentUploadModel();
                        others.Id = Convert.ToInt32(row["Id"]);
                        others.UploadType = row["UploadType"].ToString();
                        others.DocumentPath = row["DocumentPath"].ToString();
                        otheruploadList.Add(others);
                       
                    }
                   
                }
                #endregion

                #region Employee Uploads
                List<DocumentUploadModel> EmployeeUploads = new List<DocumentUploadModel>();
                if (dsItems.Tables[12] != null && dsItems.Tables[12].Rows.Count > 0)
                {
                    DocumentUploadModel employeeUpload;
                    foreach (DataRow row in dsItems.Tables[12].Rows)
                    {
                        employeeUpload = new DocumentUploadModel();
                        employeeUpload.Id = Convert.ToInt32(row["Id"]);
                        employeeUpload.ReferenceTable = row["ReferenceTable"].ToString();
                        employeeUpload.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                        employeeUpload.DocumentPath = row["DocumentPath"].ToString();
                        employeeUpload.UploadType = row["UploadType"].ToString();                        
                        EmployeeUploads.Add(employeeUpload);
                    }
                }
                if (dsItems.Tables[13] != null && dsItems.Tables[13].Rows.Count > 0)
                {
                    DataTable dt = dsItems.Tables[13];
                    pcpndtModel.ApplicationNumber = dt.Rows[0]["ApplicationNumber"].ToString();
                    pcpndtModel.AppliedSince = Convert.ToInt32(dt.Rows[0]["AppliedSince"]);
                }
                #endregion

                pcpndtModel.ApplicantModel = applicantModel;
                //pcpndtModel.EstablishmentModel = establishmentModel;
                pcpndtModel.FacilityModel = facilityModel;
                pcpndtModel.TestsModel = testsModel;
                pcpndtModel.EquipmentList = equipmentList;
                //pcpndtModel.DeclarationModel.OtherUploadsList = new List<DocumentUploadModel>();
              //  pcpndtModel.DeclarationModel.OtherUploadsList = otheruploadList;
                if (pcpndtModel.EquipmentList.Count > 0)
                    pcpndtModel.EquipmentModel.FormStatus = FormStatus.Completed;
                pcpndtModel.FacilitiesModel = facilitiesModel;
                pcpndtModel.EmployeeList = employeeList;
                if (pcpndtModel.EmployeeList.Count > 0)
                    pcpndtModel.EmployeeModel.FormStatus = FormStatus.Completed;
                pcpndtModel.InstitutionModel = institutionModel;
                pcpndtModel.DeclarationModel = declarationModel;
                pcpndtModel.DeclarationModel.OtherUploadsList = otheruploadList;
                pcpndtModel.TransactionId = transactionId;

                foreach (var employee in pcpndtModel.EmployeeList)
                {
                    employee.UploadDocuments = EmployeeUploads.Where(item => item.ReferenceId == employee.Id).ToList();
                }

                return pcpndtModel;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 22-05-2017
                return null;
            }
        }

        public List<DocumentUploadModel> GetOthersUploadData(int TransactionId, int UserId)
        {
            objDAL = new LicenseDAL();
            DataTable dt = objDAL.GetOtherUploads(TransactionId, UserId);
            List<DocumentUploadModel> otheruploadList = new List<DocumentUploadModel>();
            if (dt!=null && dt.Rows.Count > 0)
            {
                
                DocumentUploadModel others;
                foreach (DataRow row in dt.Rows)
                {
                    others = new DocumentUploadModel();
                    others.Id = Convert.ToInt32(row["Id"]);
                    others.UploadType = row["UploadType"].ToString();
                    others.DocumentPath = row["DocumentPath"].ToString();
                    otheruploadList.Add(others);
                }
            }
            return otheruploadList;
        }
        public BloodBankViewModel GetBloodBankData(int transactionId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetBloodBankData(transactionId);
                if (dsItems == null)
                    return null;
                BloodBankViewModel bloodbankModel = new BloodBankViewModel();
                #region Blood Bank Applicant Details
                BloodBankApplicantViewModel applicantModel = new BloodBankApplicantViewModel();
                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[0].Rows[0];
                    applicantModel.Id = Convert.ToInt32(row["Id"]);
                    applicantModel.Name = row["Name"].ToString();
                    applicantModel.OwnershipType = row["OwnershipType"].ToString();
                    applicantModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    applicantModel.DistrictName = row["DistrictName"].ToString();
                    applicantModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    applicantModel.MandalName = row["MandalName"].ToString();
                    applicantModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    applicantModel.VillageName = row["VillageName"].ToString();
                    applicantModel.Aadhar = row["Aadhar"].ToString();
                    applicantModel.PAN = row["PAN"].ToString();
                    applicantModel.HouseNumber = row["HouseNumber"].ToString();
                    applicantModel.StreetName = row["StreetName"].ToString();
                    applicantModel.UploadDocument = row["UploadedFile"].ToString();
                    applicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                bloodbankModel.BloodBankApplicantModel = applicantModel;
                #endregion
                #region Blood Bank Establishment Details
                BloodBankEstablishmentViewModel establishmentModel = new BloodBankEstablishmentViewModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    establishmentModel.Id = Convert.ToInt32(row["Id"]);
                    establishmentModel.Name = row["Name"].ToString();
                    establishmentModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    establishmentModel.DistrictName = row["DistrictName"].ToString();
                    establishmentModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    establishmentModel.MandalName = row["MandalName"].ToString();
                    establishmentModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    establishmentModel.VillageName = row["VillageName"].ToString();
                    establishmentModel.HouseNumber = row["HouseNumber"].ToString();
                    establishmentModel.StreetName = row["StreetName"].ToString();
                    establishmentModel.AddressProofPath = row["AddressProof"].ToString();
                    establishmentModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                bloodbankModel.BloodBankEstablishmentModel = establishmentModel;
                #endregion
                #region Blood Bank List Items
                List<BloodBankListModel> listItems = new List<BloodBankListModel>();
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    BloodBankListModel listModel;
                    foreach (DataRow row in dsItems.Tables[2].Rows)
                    {
                        listModel = new BloodBankListModel();
                        listModel.Id = Convert.ToInt32(row["Id"]);
                        listModel.Name = row["ItemName"].ToString();
                        listItems.Add(listModel);
                    }
                }
                bloodbankModel.BloodBankList = listItems;
                if (bloodbankModel.BloodBankList.Count > 0)
                    bloodbankModel.BloodBankListModel.FormStatus = FormStatus.Completed;
                #endregion
                #region Blood Bank Employee details
                List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    EmployeeViewModel employeeModel;
                    foreach (DataRow row in dsItems.Tables[3].Rows)
                    {
                        employeeModel = new EmployeeViewModel();
                        employeeModel.Id = Convert.ToInt32(row["Id"]);
                        employeeModel.Name = row["Name"].ToString();
                        employeeModel.QualificationId = Convert.ToInt32(row["QualificationId"]);
                        employeeModel.QualificationName = row["QualificationName"].ToString();
                        employeeModel.ExpYears = Convert.ToInt32(row["ExpYears"]);
                        employeeModel.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                        employeeModel.ExpDays = Convert.ToInt32(row["ExpDays"]);
                        employeeList.Add(employeeModel);
                    }
                }
                bloodbankModel.EmployeeList = employeeList;
                if (bloodbankModel.EmployeeList.Count > 0)
                    bloodbankModel.EmployeeModel.FormStatus = FormStatus.Completed;
                List<DocumentUploadModel> uploadList = new List<DocumentUploadModel>();
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    DocumentUploadModel uploadModel = new DocumentUploadModel();
                    foreach (DataRow row in dsItems.Tables[4].Rows)
                    {
                        uploadModel = new DocumentUploadModel();
                        uploadModel.Id = Convert.ToInt32(row["Id"]);
                        uploadModel.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                        uploadModel.ReferenceTable = row["ReferenceTable"].ToString();
                        uploadModel.DocumentPath = Convert.ToString(row["DocumentPath"]);
                        uploadModel.UploadType = Convert.ToString(row["UploadType"]);
                        uploadList.Add(uploadModel);
                    }
                }
                bloodbankModel.EmployeeModel.UploadDocuments = uploadList;

                for (int i = 0; i < bloodbankModel.EmployeeList.Count; i++)
                {
                    bloodbankModel.EmployeeList[i].UploadDocuments
                        .AddRange(uploadList.Where(item => item.ReferenceId == bloodbankModel.EmployeeList[i].Id).ToList());
                }

                #endregion
                #region Blood Bank Equipment details
                List<EquipmentModel> equipmentList = new List<EquipmentModel>();
                if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
                {
                    EquipmentModel equipmentModel;
                    foreach (DataRow row in dsItems.Tables[5].Rows)
                    {
                        equipmentModel = new EquipmentModel();
                        equipmentModel.Id = Convert.ToInt32(row["Id"]);
                        equipmentModel.Name = row["Name"].ToString();
                        equipmentModel.Type = row["Type"].ToString();
                        equipmentModel.MachineModel = row["MachineModel"].ToString();
                        equipmentModel.SerialNumber = row["SerialNumber"].ToString();
                        equipmentList.Add(equipmentModel);
                    }
                }
                bloodbankModel.EquipmentList = equipmentList;
                if (bloodbankModel.EquipmentList.Count > 0)
                    bloodbankModel.EquipmentModel.FormStatus = FormStatus.Completed;
                #endregion
                #region Blood Bank Inspection & Declaration Details
                BloodBankAttachments declarationModel = new BloodBankAttachments();
                if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[6].Rows[0];
                    declarationModel.Id = Convert.ToInt32(row["Id"]);
                    declarationModel.Name = row["Name"].ToString();
                    declarationModel.InspectionDate = Convert.ToDateTime(row["InspectionDate"]);
                    declarationModel.planPremisesPath = row["PremisesPlanDocPath"].ToString();
                    declarationModel.OwnerPremisesPath = row["PremisesOwnershipDocPath"].ToString();
                    declarationModel.IdProffPath = row["IdProofDocPath"].ToString();
                    declarationModel.DeclareDate = Convert.ToDateTime(row["Date"]);
                    declarationModel.Place = row["Place"].ToString();
                    declarationModel.Designation = row["Designation"].ToString();
                    declarationModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                bloodbankModel.BloodBankAttachments = declarationModel;
                #endregion

                bloodbankModel.TransactionId = transactionId;

                return bloodbankModel;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 16-08-2017
                throw;
            }
        }

        public BloodBankViewModel GetForm27EBloodBankData(int transactionId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetForm27EBloodBankData(transactionId);
                if (dsItems == null)
                    return null;
                BloodBankViewModel bloodbankModel = new BloodBankViewModel();
                #region Blood Bank Applicant Details
                BloodBankApplicantViewModel applicantModel = new BloodBankApplicantViewModel();
                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[0].Rows[0];
                    applicantModel.Id = Convert.ToInt32(row["Id"]);
                    applicantModel.Name = row["Name"].ToString();
                    applicantModel.OwnershipType = row["OwnershipType"].ToString();
                    applicantModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    applicantModel.DistrictName = row["DistrictName"].ToString();
                    applicantModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    applicantModel.MandalName = row["MandalName"].ToString();
                    applicantModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    applicantModel.VillageName = row["VillageName"].ToString();
                    applicantModel.Aadhar = row["Aadhar"].ToString();
                    applicantModel.PAN = row["PAN"].ToString();
                    applicantModel.HouseNumber = row["HouseNumber"].ToString();
                    applicantModel.StreetName = row["StreetName"].ToString();
                    applicantModel.UploadDocument = row["UploadedFile"].ToString();
                    applicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                bloodbankModel.BloodBankApplicantModel = applicantModel;
                #endregion
                #region Blood Bank Establishment Details
                BloodBankEstablishmentViewModel establishmentModel = new BloodBankEstablishmentViewModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    establishmentModel.Id = Convert.ToInt32(row["Id"]);
                    establishmentModel.Name = row["Name"].ToString();
                    establishmentModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    establishmentModel.DistrictName = row["DistrictName"].ToString();
                    establishmentModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    establishmentModel.MandalName = row["MandalName"].ToString();
                    establishmentModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    establishmentModel.VillageName = row["VillageName"].ToString();
                    establishmentModel.HouseNumber = row["HouseNumber"].ToString();
                    establishmentModel.StreetName = row["StreetName"].ToString();
                    establishmentModel.AddressProofPath = row["AddressProof"].ToString();
                    establishmentModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                bloodbankModel.BloodBankEstablishmentModel = establishmentModel;
                #endregion
                #region Blood Bank List Items
                List<BloodBankListModel> listItems = new List<BloodBankListModel>();
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    BloodBankListModel listModel;
                    foreach (DataRow row in dsItems.Tables[2].Rows)
                    {
                        listModel = new BloodBankListModel();
                        listModel.Id = Convert.ToInt32(row["Id"]);
                        listModel.Name = row["ItemName"].ToString();
                        listItems.Add(listModel);
                    }
                }
                bloodbankModel.BloodBankList = listItems;
                if (bloodbankModel.BloodBankList.Count > 0)
                    bloodbankModel.BloodBankListModel.FormStatus = FormStatus.Completed;
                #endregion
                #region Blood Bank Technical details
                List<TechnicalModel> technicalList = new List<TechnicalModel>();
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    TechnicalModel technicalModel;
                    foreach (DataRow row in dsItems.Tables[3].Rows)
                    {
                        technicalModel = new TechnicalModel();
                        technicalModel.Id = Convert.ToInt32(row["Id"]);
                        technicalModel.Name = row["Name"].ToString();
                        technicalModel.Responsibility = row["Responsibility"].ToString();
                        //technicalModel.QualificationId = Convert.ToInt32(row["QualificationId"]);
                        technicalModel.Qualification = row["QualificationName"].ToString();
                        technicalModel.ExpYears = Convert.ToInt32(row["ExpYears"]);
                        technicalModel.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                        technicalModel.ExpDays = Convert.ToInt32(row["ExpDays"]);
                        technicalList.Add(technicalModel);
                    }
                }
                bloodbankModel.TechnicalList = technicalList;
                if (bloodbankModel.TechnicalList.Count > 0)
                    bloodbankModel.TechnicalModel.FormStatus = FormStatus.Completed;
                List<DocumentUploadModel> uploadList = new List<DocumentUploadModel>();
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    DocumentUploadModel uploadModel = new DocumentUploadModel();
                    foreach (DataRow row in dsItems.Tables[4].Rows)
                    {
                        uploadModel = new DocumentUploadModel();
                        uploadModel.Id = Convert.ToInt32(row["Id"]);
                        uploadModel.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                        uploadModel.ReferenceTable = row["ReferenceTable"].ToString();
                        uploadModel.DocumentPath = Convert.ToString(row["DocumentPath"]);
                        uploadModel.UploadType = Convert.ToString(row["UploadType"]);
                        uploadList.Add(uploadModel);
                    }
                }
                //bloodbankModel.TechnicalModel = new TechnicalModel();
                //bloodbankModel.TechnicalModel.UploadDocuments = new List<DocumentUploadModel>();
                bloodbankModel.TechnicalModel.UploadDocuments = uploadList;

                for (int i = 0; i < bloodbankModel.TechnicalList.Count; i++)
                {
                    bloodbankModel.TechnicalList[i].UploadDocuments
                        .AddRange(uploadList.Where(item => item.ReferenceId == bloodbankModel.TechnicalList[i].Id).ToList());
                }

                #endregion
                #region Blood Bank Equipment details
                List<EquipmentModel> equipmentList = new List<EquipmentModel>();
                if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
                {
                    EquipmentModel equipmentModel;
                    foreach (DataRow row in dsItems.Tables[5].Rows)
                    {
                        equipmentModel = new EquipmentModel();
                        equipmentModel.Id = Convert.ToInt32(row["Id"]);
                        equipmentModel.Name = row["Name"].ToString();
                        equipmentModel.Type = row["Type"].ToString();
                        equipmentModel.MachineModel = row["MachineModel"].ToString();
                        equipmentModel.SerialNumber = row["SerialNumber"].ToString();
                        equipmentList.Add(equipmentModel);
                    }
                }
                bloodbankModel.EquipmentList = equipmentList;
                if (bloodbankModel.EquipmentList.Count > 0)
                    bloodbankModel.EquipmentModel.FormStatus = FormStatus.Completed;
                #endregion
                #region Blood Bank Inspection & Declaration Details
                BloodBankAttachments declarationModel = new BloodBankAttachments();
                if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[6].Rows[0];
                    declarationModel.Id = Convert.ToInt32(row["Id"]);
                    declarationModel.Name = row["Name"].ToString();
                    declarationModel.InspectionDate = Convert.ToDateTime(row["InspectionDate"]);
                    declarationModel.planPremisesPath = row["PremisesPlanDocPath"].ToString();
                    declarationModel.OwnerPremisesPath = row["PremisesOwnershipDocPath"].ToString();
                    declarationModel.IdProffPath = row["IdProofDocPath"].ToString();
                    declarationModel.DeclareDate = Convert.ToDateTime(row["Date"]);
                    declarationModel.Place = row["Place"].ToString();
                    declarationModel.Designation = row["Designation"].ToString();
                    declarationModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                bloodbankModel.BloodBankAttachments = declarationModel;
                #endregion

                bloodbankModel.TransactionId = transactionId;

                return bloodbankModel;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 21-08-2017
                throw;
            }
        }

        public HomeopathyDrugStoreViewModel GetHomeopathyData(int transactionId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetHomeopathyData(transactionId);
                if (dsItems == null)
                    return null;
                HomeopathyDrugStoreViewModel homeopathyModel = new HomeopathyDrugStoreViewModel();
                #region Homeopathy Applicant Details
                ApplicantViewModel HDapplicantModel = new ApplicantViewModel();
                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[0].Rows[0];
                    HDapplicantModel.Id = Convert.ToInt32(row["Id"]);
                    HDapplicantModel.Name = row["Name"].ToString();
                    HDapplicantModel.OwnershipType = row["OwnershipType"].ToString();
                    HDapplicantModel.Aadhar = row["Aadhar"].ToString();
                    HDapplicantModel.PAN = row["PAN"].ToString();
                    HDapplicantModel.MobileNo = row["Mobile"].ToString();
                    HDapplicantModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    HDapplicantModel.DistrictName = row["District"].ToString();
                    HDapplicantModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    HDapplicantModel.MandalName = row["Mandal"].ToString();
                    HDapplicantModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    HDapplicantModel.VillageName = row["Village"].ToString();
                    HDapplicantModel.HouseNumber = row["HouseNumber"].ToString();
                    HDapplicantModel.StreetName = row["StreetName"].ToString();
                    HDapplicantModel.PINCode = row["PINCode"].ToString();
                    HDapplicantModel.UploadDocument = row["UploadedFile"].ToString();
                    HDapplicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                homeopathyModel.HDApplicantModel = HDapplicantModel;
                #endregion
                #region Homeopathy Establishment Details
                HomeopathyEstablishmentViewModel establishmentModel = new HomeopathyEstablishmentViewModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    establishmentModel.Id = Convert.ToInt32(row["Id"]);
                    establishmentModel.Name = row["StoreName"].ToString();
                    establishmentModel.OwnedBy = row["OwnedBy"].ToString();
                    establishmentModel.Fromdate = Convert.ToDateTime(row["FromDate"]);
                    establishmentModel.ToDate = Convert.ToDateTime(row["ToDate"]);
                    establishmentModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    establishmentModel.DistrictName = row["District"].ToString();
                    establishmentModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    establishmentModel.MandalName = row["Mandal"].ToString();
                    establishmentModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    establishmentModel.VillageName = row["Village"].ToString();
                    establishmentModel.HouseNumber = row["HouseNo"].ToString();
                    establishmentModel.StreetName = row["StreetName"].ToString();
                    establishmentModel.PINCode = row["PINCode"].ToString();
                    establishmentModel.RentalDocument = row["RentDeedDocPath"].ToString();
                    establishmentModel.PlanPremisesDocument = row["PremisesDocPath"].ToString();
                    establishmentModel.AddressProff = row["AdressProofDocPath"].ToString();
                    establishmentModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                homeopathyModel.HDEstablishment = establishmentModel;
                #endregion
                #region Homeopathy Competent Person Incharge Details
                ApplicantViewModel hdCompetent = new ApplicantViewModel();
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[2].Rows[0];
                    hdCompetent.Id = Convert.ToInt32(row["Id"]);
                    hdCompetent.Name = row["Name"].ToString();
                    hdCompetent.Aadhar = row["AadharId"].ToString();
                    hdCompetent.MobileNo = row["MobileNo"].ToString();
                    hdCompetent.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    hdCompetent.DistrictName = row["District"].ToString();
                    hdCompetent.MandalId = Convert.ToInt32(row["MandalId"]);
                    hdCompetent.MandalName = row["Mandal"].ToString();
                    hdCompetent.VillageId = Convert.ToInt32(row["VillageId"]);
                    hdCompetent.VillageName = row["Village"].ToString();
                    hdCompetent.HouseNumber = row["HouseNo"].ToString();
                    hdCompetent.StreetName = row["StreetName"].ToString();
                    hdCompetent.PINCode = row["PINCode"].ToString();
                    hdCompetent.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                homeopathyModel.HDCompetentModel = hdCompetent;
                #endregion
                #region Homeopathy Declaration Details
                HomeopathyDeclaration declarationModel = new HomeopathyDeclaration();
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[3].Rows[0];
                    declarationModel.Id = Convert.ToInt32(row["Id"]);
                    declarationModel.CoveringLetter = row["CoveringDocPath"].ToString();
                    declarationModel.Date = Convert.ToDateTime(row["Date"]);
                    declarationModel.Signature = row["Signature"].ToString();
                    declarationModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                homeopathyModel.HDDeclaration = declarationModel;
                #endregion
                homeopathyModel.TransactionId = transactionId;
                return homeopathyModel;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 06-09-2017
                throw;
            }
        }   

        #region BioCapstone view Model
        public BioCapstoneViewModel GetBioCapstoneView()
        {
            BioCapstoneViewModel model = new BioCapstoneViewModel();

            FileHandling.ReadWrite2Files readBioCapstone = new FileHandling.ReadWrite2Files();
            model = readBioCapstone.DeSerializeObject<BioCapstoneViewModel>("D:\\BioMedial_Test.BM");

            BioCapstoneApplicantViewModel applicantmodel = new BioCapstoneApplicantViewModel();
            applicantmodel.Name = model.BioCapstoneApplicantModel.Name;
            applicantmodel.InstitutionName = model.BioCapstoneApplicantModel.InstitutionName;
            applicantmodel.DistrictId = model.BioCapstoneApplicantModel.DistrictId;
            applicantmodel.MandalId = model.BioCapstoneApplicantModel.MandalId;
            applicantmodel.VillageId = model.BioCapstoneApplicantModel.VillageId;
            applicantmodel.HouseNumber = model.BioCapstoneApplicantModel.HouseNumber;
            applicantmodel.StreetName = model.BioCapstoneApplicantModel.StreetName;
            applicantmodel.Fax = model.BioCapstoneApplicantModel.Fax;
            applicantmodel.Telegraph = model.BioCapstoneApplicantModel.Telegraph;
            applicantmodel.Telex = model.BioCapstoneApplicantModel.Telex;
            applicantmodel.PINCode = model.BioCapstoneApplicantModel.PINCode;
            model.BioCapstoneApplicantModel = applicantmodel;

            AuthorisationViewModel authorazation = new AuthorisationViewModel();

            BioCapstoneAddressTreatmentFacilityViewModel treatmentmodel = new BioCapstoneAddressTreatmentFacilityViewModel();

            treatmentmodel.DistrictId = model.BioCapstoneAddressTreatementFacilityModel.DistrictId;
            treatmentmodel.MandalId = model.BioCapstoneAddressTreatementFacilityModel.MandalId;
            treatmentmodel.VillageId = model.BioCapstoneAddressTreatementFacilityModel.VillageId;
            treatmentmodel.HouseNumber = model.BioCapstoneAddressTreatementFacilityModel.HouseNumber;
            treatmentmodel.StreetName = model.BioCapstoneAddressTreatementFacilityModel.StreetName;
            treatmentmodel.Fax = model.BioCapstoneAddressTreatementFacilityModel.Fax;
            treatmentmodel.Telegraph = model.BioCapstoneAddressTreatementFacilityModel.Telegraph;
            treatmentmodel.Telex = model.BioCapstoneAddressTreatementFacilityModel.Telex;
            treatmentmodel.PINCode = model.BioCapstoneAddressTreatementFacilityModel.PINCode;
            model.BioCapstoneAddressTreatementFacilityModel = treatmentmodel;

            BioCapstoneAddressofDisposalWasteViewModel wastemodel = new BioCapstoneAddressofDisposalWasteViewModel();
            wastemodel.DistrictId = model.BioCapstoneAddressDisposalWasteModel.DistrictId;
            wastemodel.MandalId = model.BioCapstoneAddressDisposalWasteModel.MandalId;
            wastemodel.VillageId = model.BioCapstoneAddressDisposalWasteModel.VillageId;
            wastemodel.HouseNumber = model.BioCapstoneAddressDisposalWasteModel.HouseNumber;
            wastemodel.StreetName = model.BioCapstoneAddressDisposalWasteModel.StreetName;
            wastemodel.Fax = model.BioCapstoneAddressDisposalWasteModel.Fax;
            wastemodel.Telegraph = model.BioCapstoneAddressDisposalWasteModel.Telegraph;
            wastemodel.Telex = model.BioCapstoneAddressDisposalWasteModel.Telex;
            wastemodel.PINCode = model.BioCapstoneAddressDisposalWasteModel.PINCode;
            model.BioCapstoneAddressDisposalWasteModel = wastemodel;

            List<TreatmentModle> treatmentList = new List<TreatmentModle>();

            //foreach ( model)
            //{
            //    TreatmentViewModle treatment = new TreatmentViewModle();
            //    treatment.Description = model.TreatmentModle.Description;
            //    treatment.Attachment = model.TreatmentModle.Description;
            //    treatmentList.Add(treatment);
            //}
            //model.TreatmentList = treatmentList;

            return model;
        }
        #endregion
        #region Organ Transplantation
        public List<OTStaffDetailsModel> ConvertToListOTStaffList(DataTable dt)
        {
                List<OTStaffDetailsModel> OTStaffList = new List<OTStaffDetailsModel>();
                OTStaffDetailsModel otStaffModel = new OTStaffDetailsModel();
                foreach (DataRow row in dt.Rows)
                {
                    otStaffModel.Id = Convert.ToInt32(row["Id"]);
                    otStaffModel.StaffType = row["StaffType"].ToString();
                    otStaffModel.Designation = row["Designation"].ToString();
                    otStaffModel.NoOfMembers = row["NoofMembers"].ToString()=="" ?0 : Convert.ToInt32(row["NoofMembers"]);
                    otStaffModel.Organ = row["Organ"].ToString();
                    otStaffModel.SectionName = row["SectionName"].ToString();
                    OTStaffList.Add(otStaffModel);
                }            
            return OTStaffList;
        }
        public List<OTOperationModel> ConvertToListOToperation(DataTable dt)
        {
            List<OTOperationModel> OToperationList = new List<OTOperationModel>();
            OTOperationModel OToperationModel = new OTOperationModel();
            foreach (DataRow row in dt.Rows)
            {
                OToperationModel.Id = Convert.ToInt32(row["Id"]);
                OToperationModel.Name = row["OperationName"].ToString();
                OToperationModel.OperationsPerformed = row["NoofOperations"].ToString()=="" ? 0 : Convert.ToInt32(row["NoofOperations"]);
                OToperationModel.SectionName = row["SectionName"].ToString();
                OToperationList.Add(OToperationModel);
            }
            return OToperationList;
        }
        public List<OTEquipmentModel> ConvertToListEquipment(DataTable dt)
        {
            List<OTEquipmentModel> OTequipmentList = new List<OTEquipmentModel>();
            OTEquipmentModel otEquipmentModel = new OTEquipmentModel();
            foreach (DataRow row in dt.Rows)
            {
                otEquipmentModel.Id = Convert.ToInt32(row["Id"]);
                otEquipmentModel.Name = row["EquipmentName"].ToString();
                otEquipmentModel.NoofEquipments = row["NoofEquipments"].ToString()=="" ? 0 : Convert.ToInt32(row["NoofEquipments"]);
                otEquipmentModel.SectionName = row["SectionName"].ToString();
                OTequipmentList.Add(otEquipmentModel);
            }
            return OTequipmentList;
        }
        public OrganTransplantViewModel GetOrganTransplantationData(int transactionId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetOrganTransplantationData(transactionId);
                if (dsItems == null)
                    return null;
                OrganTransplantViewModel organTransplantModel = new OrganTransplantViewModel();
                #region Hospital Details
                HospitalViewModel hosipitalModel = new HospitalViewModel();
                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count >0)
                {
                    DataRow row = dsItems.Tables[0].Rows[0];
                    hosipitalModel.Id = Convert.ToInt32(row["Id"]);
                    hosipitalModel.HospitalName = row["HospitalName"].ToString();
                    hosipitalModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    hosipitalModel.DistrictName = row["DistrictName"].ToString();
                    hosipitalModel.MandalName = row["MandalName"].ToString();
                    hosipitalModel.VillageName = row["VillageName"].ToString();
                    hosipitalModel.StreetName = row["StreetName"].ToString();
                    hosipitalModel.HouseNo = row["HouseNumber"].ToString();
                    hosipitalModel.Pincode = row["PINCode"].ToString();
                    hosipitalModel.PhoneNo = row["PhoneNo"].ToString();
                    hosipitalModel.TeacherNonTeach = row["TeachingType"].ToString();
                    hosipitalModel.Govtorpvt = row["Government"].ToString();
                    hosipitalModel.AnnualBudjet = Convert.ToInt32(row["AnnualBudjet"]);
                    hosipitalModel.TotalBedStrength = Convert.ToInt32(row["BedStrength"]);
                    hosipitalModel.Nameofdisciplines = row["DisciplinesName"].ToString();
                    hosipitalModel.PatientTurnoverPerYear = Convert.ToInt32(row["PatientTurnOver"]);
                    hosipitalModel.ByRoad = row["Road"].ToString();
                    hosipitalModel.ByRail = row["Rail"].ToString();
                    hosipitalModel.ByAir = row["Air"].ToString();
                }
                organTransplantModel.HospitalModel = hosipitalModel;
                #endregion
                #region Surgical Details
                SurgicalTeamModel surgicalModel = new SurgicalTeamModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow commonRow = dsItems.Tables[1].Rows[0];
                    surgicalModel.Id = Convert.ToInt32(commonRow["Id"]);
                    surgicalModel.NumberofBeds = commonRow["NoofSurgicalBeds"].ToString()=="" ? 0: Convert.ToInt32(commonRow["NoofSurgicalBeds"]);
                    surgicalModel.NoofOperationsPerYear = commonRow["NoofOperationsperYear"].ToString()=="" ? 0 : Convert.ToInt32(commonRow["NoofOperationsperYear"]);
                }                
                organTransplantModel.Surgical = surgicalModel;
                #endregion                
                #region Capstone Details
                CapstoneTeamModel CapstoneModel = new CapstoneTeamModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow Capstonerow = dsItems.Tables[1].Rows[0];
                    CapstoneModel.Id = Convert.ToInt32(Capstonerow["Id"]);
                    CapstoneModel.NoofBeds = Capstonerow["NoofCapstoneBeds"].ToString() == "" ? 0 : Convert.ToInt32(Capstonerow["NoofCapstoneBeds"]);
                    CapstoneModel.PatientTurnover = Capstonerow["PatientTurnOver"].ToString() == "" ? 0 : Convert.ToInt32(Capstonerow["PatientTurnOver"]);
                    CapstoneModel.TransplantPatients = Capstonerow["TransplantCandidates"].ToString() == "" ? 0 : Convert.ToInt32(Capstonerow["TransplantCandidates"]);
                }
                organTransplantModel.CapstoneTeam = CapstoneModel;
                #endregion
                #region Anaesthesiology
                AnaesthesiologyModel anaesthesiology = new AnaesthesiologyModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count >0)
                {
                    DataRow row= dsItems.Tables[1].Rows[0];
                    anaesthesiology.Id = Convert.ToInt32(row["Id"]);
                    anaesthesiology.OperationTheatres = row["NoofOperationTheatres"].ToString() == "" ? 0 : Convert.ToInt32(row["NoofOperationTheatres"]);
                    anaesthesiology.EmergencyOperationTheatres = row["EmergencyOperationTheatres"].ToString() == "" ? 0 : Convert.ToInt32(row["EmergencyOperationTheatres"]);
                    anaesthesiology.TransplantOperationTheatres = row["TransplantOperationTheatres"].ToString() == "" ? 0 : Convert.ToInt32(row["TransplantOperationTheatres"]);
                }
                organTransplantModel.Anaesthesiology = anaesthesiology;
                #endregion
                #region ICU
                ICUHDUFacilities icuModel = new ICUHDUFacilities();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    icuModel.Id = Convert.ToInt32(row["Id"]);
                    icuModel.ICUBeds = row["NoofICUBeds"].ToString() == "" ? 0 : Convert.ToInt32(row["NoofICUBeds"]);
                    icuModel.Trained = row["Trained"].ToString() == "" ? 0 : Convert.ToInt32(row["Trained"]);
                    icuModel.Nurses = row["Nurses"].ToString() == "" ? 0 : Convert.ToInt32(row["Nurses"]);
                    icuModel.Technicians = row["Technicians"].ToString() == "" ? 0 : Convert.ToInt32(row["Technicians"]);
                    icuModel.OtherSupportiveFacilities = row["OtherFacilities"].ToString();
                }
                organTransplantModel.ICUHDUFacilities = icuModel;
                #endregion
                #region Laboratory
                LaboratoryFacilitiesModel laboratorymodel = new LaboratoryFacilitiesModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    laboratorymodel.Id = Convert.ToInt32(row["Id"]);
                    laboratorymodel.Investigations = row["LaboratoryInvestigations"].ToString();
                }
                organTransplantModel.LaboratoryFacilities = laboratorymodel;
                #endregion
                #region Facilities
                OrganTransplantFacilitiesModel OTfacilityModel = new OrganTransplantFacilitiesModel();
                if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[5].Rows[0];
                    OTfacilityModel.Id = Convert.ToInt32(row["Id"]);
                    OTfacilityModel.BloodBankFacilities =Convert.ToBoolean(row["BloodBankFacilities"]);
                    OTfacilityModel.DialysisFacilities = Convert.ToBoolean(row["DialysisFacilities"].ToString());
                    OTfacilityModel.Nephrologist = Convert.ToBoolean(row["Nephrologist"].ToString());
                    OTfacilityModel.Neurologist = Convert.ToBoolean(row["Neurologist"].ToString());
                    OTfacilityModel.NeuroSurgeon = Convert.ToBoolean(row["NeuroSurgeon"].ToString());
                    OTfacilityModel.Urologist = Convert.ToBoolean(row["Urologist"].ToString());
                    OTfacilityModel.GISurgeon = Convert.ToBoolean(row["GISurgeon"].ToString());
                    OTfacilityModel.Paediatrician = Convert.ToBoolean(row["Paediatrician"].ToString());
                    OTfacilityModel.Physicotherapist = Convert.ToBoolean(row["Physicotherapist"].ToString());
                    OTfacilityModel.SocialWorker = Convert.ToBoolean(row["SocialWorker"].ToString());
                    OTfacilityModel.Immunologist = Convert.ToBoolean(row["Immunologist"].ToString());
                    OTfacilityModel.Cardiologist = Convert.ToBoolean(row["Cardiologist"].ToString());
                }
                organTransplantModel.Facilities = OTfacilityModel;
                #endregion
                #region Declaration
                DeclarationModel declarationModel = new DeclarationModel();
                if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[6].Rows[0];
                    declarationModel.Id = Convert.ToInt32(row["Id"]);
                    declarationModel.Signature = row["Signature"].ToString();
                }
                organTransplantModel.Declaration = declarationModel;
                #endregion
                #region Operations
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    List<OTOperationModel> OToperationlist = ConvertToListOToperation(dsItems.Tables[3]);
                    if (OToperationlist.Where(item => item.SectionName == "Anaesthesiology").Count() > 0)
                    {
                        anaesthesiology.OperationsList = OToperationlist.Where(item => item.SectionName == "Anaesthesiology").ToList();
                    }

                    //OTOperationModel otOperationModel = new OTOperationModel();
                    //foreach (DataRow row in dsItems.Tables[3].Rows)
                    //{
                    //    otOperationModel.Id = Convert.ToInt32(row[""]);
                    //    otOperationModel.Name = row[""].ToString();
                    //    otOperationModel.OperationsPerformed = Convert.ToInt32(row[""]);
                    //}
                }
                #endregion
                #region Equipment
                if(dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    List<OTEquipmentModel> OTequipmentList = ConvertToListEquipment(dsItems.Tables[4]);
                    if (OTequipmentList.Where(item => item.SectionName== "Anaesthesiology").Count() > 0 )
                    {
                        anaesthesiology.EquipmentsList = OTequipmentList.Where(item => item.SectionName == "Anaesthesiology").ToList();
                    }
                }
                #endregion
                #region Staff Details
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    List<OTStaffDetailsModel> OTstafflist = ConvertToListOTStaffList(dsItems.Tables[2]);
                    if (OTstafflist.Where(item => item.SectionName == "Surgical").Count() > 0)
                    {
                        surgicalModel.StaffDetailsList = OTstafflist.Where(item => item.SectionName == "Surgical").ToList();
                    }
                    if (OTstafflist.Where(item => item.SectionName == "Capstone").Count() > 0)
                    {
                        CapstoneModel.StaffDetailsList = OTstafflist.Where(item => item.SectionName == "Capstone").ToList();
                    }
                    if (OTstafflist.Where(item => item.SectionName == "").Count() > 0)
                    {
                        anaesthesiology.StaffDetailsList = OTstafflist.Where(item => item.SectionName == "").ToList();
                    }
                }
                #endregion
                organTransplantModel.TransactionId = transactionId;           
                return organTransplantModel;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 13-09-2017
                throw;
            }
        }
        #endregion

        public QueryResponseViewModel GetQueryResponseData(int userId)
        {
            objDAL = new LicenseDAL();
            DataSet dsItems = objDAL.GetQueryResponseData(userId);
            if (dsItems == null)
                return null;

            QueryResponseViewModel model = new QueryResponseViewModel();
            #region Raised Queries
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                model.RaisedQueryList = new List<QueryModel>();
                foreach (DataRow row in dsItems.Tables[0].Rows)
                {
                    QueryModel query = new QueryModel();
                    query.Id = Convert.ToInt32(row["Id"]);
                    query.TransactionId = Convert.ToInt32(row["TransactionId"]);
                    query.ApplicationType = row["ApplicationType"].ToString();
                    query.DepartmentName = row["DepartmentName"].ToString();
                    query.Description = row["Description"].ToString();
                    query.UploadedFilePath = row["UploadedDocPath"].ToString();
                    query.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                    model.RaisedQueryList.Add(query);
                }
            }
            #endregion

            #region Query Logs
            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
            {
                model.QueryLogList = new List<QueryModel>();
                foreach (DataRow row in dsItems.Tables[1].Rows)
                {
                    QueryModel query = new QueryModel();
                    query.Type = row["Type"].ToString();
                    query.DepartmentName = row["DepartmentName"].ToString();
                    query.Description = row["Description"].ToString();
                    query.UploadedFilePath = row["UploadedDocPath"].ToString();
                    query.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                    model.QueryLogList.Add(query);
                }
            }
            #endregion

            return model;
        }
        public QueryModel GetRaisedQueryData(int id)
        {
            objDAL = new LicenseDAL();
            DataTable dtItems = objDAL.GetRaisedQueryData(id);
            if (dtItems == null)
                return null;

            QueryModel model = new QueryModel();
            if (dtItems.Rows.Count > 0)
            {
                model.DepartmentName = dtItems.Rows[0]["DepartmentName"].ToString();
                model.Description = dtItems.Rows[0]["Description"].ToString();
                model.UploadedFilePath = dtItems.Rows[0]["UploadedDocPath"].ToString();
                model.CreatedOn = Convert.ToDateTime(dtItems.Rows[0]["CreatedOn"]);
                model.ApplicationType = dtItems.Rows[0]["ApplicationType"].ToString();
            }
            return model;

        }
        public bool SubmitResponse(QueryModel model)
        {
            objDAL = new LicenseDAL();
            return objDAL.SubmitResponse(model);
        }
        public PCPNDTLicenseInfoModel GetPCPNDTLicenseDetails(int transactionId,string TableName)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetPCPNDTLicenseDetails(transactionId, TableName);
                if (dsItems == null)
                    return null;
                PCPNDTLicenseInfoModel model = new PCPNDTLicenseInfoModel();
                DataRow row = dsItems.Tables[0].Rows[0];
                model.IssuingAuthority = Convert.ToString(row["IssuingAuthority"]);
                model.Facilities = Convert.ToString(row["Facilities"]);
                model.ApplicantNameAddress = Convert.ToString(row["NameAddress"]);
                model.TestRemarks = Convert.ToString(row["TestsRemarks"]);
                model.FacilitiesRemarks = Convert.ToString(row["FacilitiesRemarks"]);
                model.LicenseNumber = Convert.ToString(row["LicenseNumber"]);
                model.LicenseIssuedDate = Convert.ToDateTime(row["LicenseIssuedDate"]);
                model.LicenseExpiryDate = Convert.ToDateTime(row["LicenseExpiryDate"]);
                model.AppropriateAuthority = Convert.ToString(row["AppropriateAuthority"]);
                model.ServiceId= Convert.ToInt32(row["ServiceId"]);
                // Binding Invasive Tests
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    model.InvasiveTests = new List<string>();
                    foreach (DataRow test in dsItems.Tables[1].Rows)
                        model.InvasiveTests.Add(Convert.ToString(test["InvasiveTests"]));
                }

                // Binding Non-Invasive Tests
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    model.NonInvasiveTests = new List<string>();
                    foreach (DataRow test in dsItems.Tables[2].Rows)
                        model.NonInvasiveTests.Add(Convert.ToString(test["NonInvasiveTests"]));
                }

                // Bind Tests
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    model.Tests = new List<string>();
                    foreach (DataRow test in dsItems.Tables[3].Rows)
                        model.Tests.Add(Convert.ToString(test["Tests"]));
                }

                // Bind Studies
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    model.Studies = new List<string>();
                    foreach (DataRow test in dsItems.Tables[4].Rows)
                        model.Studies.Add(Convert.ToString(test["Studies"]));
                }

                // Binding Equipments
                if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
                {
                    model.EquipmentList = new List<EquipmentModel>();
                    EquipmentModel equipment;
                    foreach (DataRow dataRow in dsItems.Tables[5].Rows)
                    {
                        equipment = new EquipmentModel();
                        equipment.Id = Convert.ToInt32(dataRow["Id"]);
                        equipment.Name = Convert.ToString(dataRow["Name"]);
                        equipment.SerialNumber = Convert.ToString(dataRow["SerialNumber"]);
                        equipment.MachineModel = Convert.ToString(dataRow["MachineModel"]);
                        equipment.Make = Convert.ToString(dataRow["Make"]);
                        equipment.Type = Convert.ToString(dataRow["Type"]);
                        equipment.UploadedFilePath = Convert.ToString(dataRow["UploadedFilePath"]);
                        model.EquipmentList.Add(equipment);
                    }
                }

                // Binding Employees
                if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
                {
                    model.EmployeeList = new List<EmployeeViewModel>();
                    EmployeeViewModel employee;
                    foreach (DataRow datarow in dsItems.Tables[6].Rows)
                    {
                        employee = new EmployeeViewModel();
                        employee.Id = Convert.ToInt32(datarow["Id"]);
                        employee.Name = Convert.ToString(datarow["Name"]);
                        employee.DesignationId = Convert.ToInt32(datarow["DesignationId"]);
                        employee.DesignationName = Convert.ToString(datarow["DesignationName"]);
                        employee.SubDesignation = Convert.ToString(datarow["SubDesignation"]);
                        employee.ExpYears = Convert.ToInt32(datarow["ExpYears"]);
                        employee.ExpMonths = Convert.ToInt32(datarow["ExpMonths"]);
                        employee.ExpDays = Convert.ToInt32(datarow["ExpDays"]);
                       // employee.Experience = Convert.ToString(datarow["Experience"]);
                        employee.RegistrationNumber = Convert.ToString(datarow["RegistrationNumber"]);
                        employee.UploadedFilePath = Convert.ToString(datarow["UploadedFilePath"]);
                        model.EmployeeList.Add(employee);
                    }
                }

                return model;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 05-06-2017
                return null;
            }
        }

        public APMCERejection GetAPMCERejectionDetails(int ApplicationId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetAPMCERejectionApplications(ApplicationId);
                if (dsItems == null)
                    return null;
                APMCERejection model = new APMCERejection();
                DataRow row = dsItems.Tables[0].Rows[0];
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    model.ApplicationType = Convert.ToString(dsItems.Tables[1].Rows[0]["ApplicationType"]);
                    model.FacilityNameAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["NameAddress"]);
                    model.ReasonsOfRejection = Convert.ToString(dsItems.Tables[1].Rows[0]["Reasons"]);
                    model.AppropriateAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["AppropriateAuthority"]);
                }
                return model;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 21-07-2017
                return null;
            }
        }

        public BloodBankNOCModel GetBloodBankNOC(int TransectionId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataTable dt = objDAL.GetBloodBankNOC(TransectionId);
                if (dt == null)
                    return null;
                BloodBankNOCModel model = new BloodBankNOCModel();
                DataRow row = dt.Rows[0];
                if (dt.Rows.Count > 0)
                {
                    model.Name = dt.Rows[0]["ApplicantName"].ToString();                   
                    model.HospitalNameAndAddress = dt.Rows[0]["HospitalNameAddress"].ToString();
                    model.BloodbankFrom = Convert.ToString(dt.Rows[0]["BloodBankForm"]);
                    model.ApplicationDate = Convert.ToDateTime(row["ApplicationDate"]);
                    model.Date = Convert.ToDateTime(row["LicenseIssuedDate"]);
                    model.ApplicationNo= dt.Rows[0]["ApplicationNumber"].ToString();

                }
                return model;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 15-09-2017
                return null;
            }
        }
        public PCPNDTRejection GetPCPNDTRejectionDetails(int ApplicationId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetPCPNDTRejectionDetails(ApplicationId);
                if (dsItems == null)
                    return null;
                PCPNDTRejection model = new PCPNDTRejection();
                DataRow row = dsItems.Tables[0].Rows[0];
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {

                    model.ApplicationType = Convert.ToString(dsItems.Tables[1].Rows[0]["ApplicationType"]);
                    model.IssuingAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["IssuingAuthority"]);
                    model.Facilities = Convert.ToString(dsItems.Tables[1].Rows[0]["Facilities"]);
                    model.FacilityNameAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["NameAddress"]);
                    model.ReasonsOfRemarks = Convert.ToString(dsItems.Tables[1].Rows[0]["Reasons"]);
                    model.AppropriateAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["AppropriateAuthority"]);
                    model.ReceivedPlace = Convert.ToString(dsItems.Tables[1].Rows[0]["DistrictName"]);
                    //model.ReceivedDate = dsItems.Tables[1].Rows[0]["SubmittedOn"] != DBNull.Value ?
                    //    Convert.ToDateTime(dsItems.Tables[1].Rows[0]["SubmittedOn"]) : default(DateTime);
                }
                return model;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log            -Pj, 10-07-2017
                return null;
            }
        }
        public LicenseQuestionnaireModel GetRejectedServices(int transactionId, string TransactionType)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetRejectedServices(transactionId, TransactionType);
                if (dsItems == null)
                    return null;
                LicenseQuestionnaireModel model = new LicenseQuestionnaireModel();
                DataRow row = dsItems.Tables[0].Rows[0];
                model.ApplicationModel.ApplicationNumber = Convert.ToString(row["ApplicationNumber"]);
                foreach (DataRow servicerow in dsItems.Tables[1].Rows)
                {
                    if (Convert.ToInt32(servicerow["ServiceId"]) == 1)
                        model.HasAppliedforAPMCE = true;
                    //else if (Convert.ToInt32(servicerow["ServiceId"]) == 2)
                    //{
                        model.HasAppliedforPCPNDT = true;
                   // }
                }
                return model;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public DataTable GetTAMCEFeeDetails(int applicationId)
        {
            objDAL = new LicenseDAL();
            return objDAL.GetTAMCEFeeDetails(applicationId);
        }

        public DataSet GetServicesByApplicationID(int applicationId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetServicesByApplicationID(applicationId);
                return dsItems;
                //if (dsItems == null)
                //    return null;
                //PaymentModel model = new PaymentModel();
                //model.ApplicationId = applicationId;
                //// DataRow row = dsItems.Rows[0];
                ////model.ApplicationNumber = Convert.ToInt32(dsItems.Rows[0]["ApplicationNumber"]);
                //if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                //{
                //    DataRow row = dsItems.Tables[0].Rows[0];
                //    model.HasAppliedforAPMCE = true;
                //    model.PhysiotherapyCenters = row["APMCE"].ToString();
                //}
                //if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                //{
                //    DataRow row = dsItems.Tables[1].Rows[0];
                //    if(Convert.ToInt32(row["ServiceId"]) == 2)
                //    {
                //        model.HasAppliedforPCPNDT = true;
                //        model.MultipleFacilities = row["PCPNDT"].ToString();
                //    }

                //}

                //    foreach (DataRow row in dsItems.Rows)
                //{
                //    if (Convert.ToInt32(row["SID"]) == 1)
                //    {
                //        model.HasAppliedforAPMCE = true;
                //        model.PhysiotherapyCenters = row["FEE"].ToString();
                //    }

                //    if (Convert.ToInt32(row["SID"]) == 2)
                //    {
                //        model.HasAppliedforPCPNDT = true;
                //        if (Convert.ToInt32(row["FEE"]) == 25000)
                //        {
                //            model.SingleFacilities = row["FEE"].ToString();
                //        }
                //        else if (Convert.ToInt32(row["FEE"]) == 35000)
                //        {
                //            model.MultipleFacilities = row["FEE"].ToString();
                //        }

                //    }
                //}
                //DataRow servicesrow = dsItems.Tables[1].Rows[0];
                //model.PCPNDT = Convert.ToInt32(servicesrow["FEE"]);
                //model.SubTotal = Convert.ToInt32(servicesrow["FEE"]);


            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public APMCECertificate GetAPMCECertificate(int TransactionId,string TableName)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItems = objDAL.GetAPMCECertificateDetails(TransactionId, TableName);
                if (dsItems == null)
                    return null;
                APMCECertificate model = new APMCECertificate();
                DataRow row = dsItems.Tables[0].Rows[0];
                model.ApplicationNumber = Convert.ToString(row["LicenseNumber"]);
                model.ApplicationDate = Convert.ToDateTime(row["CreatedOn"]);
                model.IssuedDate = Convert.ToDateTime(row["LicenseIssuedDate"]);
                model.ExpiryDate = Convert.ToDateTime(row["LicenseExpiryDate"]);
                // Added On 01-07-2021 by Chandu
                model.InspectionReportNo = Convert.ToString(row["InspectionReportNo"]);
                model.InspectionDate = Convert.ToDateTime(row["InspectionDate"]);

                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    model.ApplicantNameAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["NameAddress"]);
                    model.District = Convert.ToString(dsItems.Tables[1].Rows[0]["DistrictName"]);
                    model.IssuingAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["IssuingAuthority"]);
                    model.AppropriateAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["AppropriateAuthority"]);
                }
                // Services         -- PJ, 19-07-2017
                foreach (DataRow serivcerow in dsItems.Tables[2].Rows)
                {
                    model.ServiceDetails.Add(serivcerow["Services"].ToString());
                }
                return model;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            -Pj, 12-07-2017
                return null;
            }
        }
        public DataTable GetLicenseType(int TransactionId,string Type)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataTable dtItems = objDAL.GetLicenseType(TransactionId, Type);
                return dtItems;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public DataTable GetLicenseTypeForLicense(int TransactionId, string TableName)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataTable dtItems = objDAL.GetLicenseTypeForLicense(TransactionId, TableName);
                return dtItems;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public DataSet GetRejectLicense(int transactionId, string TransactionType)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataSet dsItem = objDAL.GetRejectedServices(transactionId, TransactionType);
                return dsItem;
            }
            catch (Exception ex)
            {
                // TODO :Write exception log            -Jai, 21-07-2017
                return null;
            }
        }

        
        #region Bio Capstone Saving 
        public int SaveBioCapstoneApplicantDetails(BioCapstoneViewModel model, ref int _applicationId, ref int transactionId, ref string applicationNumber)
        {
            LicenseDAL objDAL = new LicenseDAL();
            return objDAL.SaveBioCapstoneApplicantDetails(model, ref _applicationId, ref transactionId, ref applicationNumber);
        }
        public BioCapstoneViewModel GetBioCapstoneDetails(int transactionId) 
        {
            objDAL = new LicenseDAL();
            DataSet dsItems = objDAL.GetBioCapstoneDetails(transactionId);
            if (dsItems == null)
                return null;
            BioCapstoneViewModel BioCapstoneModel = new BioCapstoneViewModel();

            #region Particulars of applicant
           
           // AuthorisationViewModel authorisationModel = new AuthorisationViewModel();
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                BioCapstoneApplicantViewModel applicantModel = new BioCapstoneApplicantViewModel();
                DataRow row = dsItems.Tables[0].Rows[0];
                applicantModel.Id = Convert.ToInt32(row["Id"]);
                applicantModel.Name = row["Name"].ToString();
                applicantModel.InstitutionName = row["InstitutionName"].ToString();
                applicantModel.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                applicantModel.DistrictName = row["DistrictName"].ToString();
                applicantModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                applicantModel.MandalName= row["MandalName"].ToString();
                applicantModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                applicantModel.VillageName=row["VillageName"].ToString();
                applicantModel.HouseNumber= row["HouseNumber"].ToString();
                applicantModel.StreetName= row["StreetName"].ToString();
                applicantModel.Fax= row["Fax"].ToString();
                applicantModel.Telegraph= row["Telegraph"].ToString();
                applicantModel.Telex= row["Telex"].ToString();
                applicantModel.PINCode = row["PINCode"].ToString();
                applicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                BioCapstoneModel.BioCapstoneApplicantModel = applicantModel;
               

                //authorisationModel.Authorasation = row["Authorisation"].ToString();
                //BioCapstoneModel.AuthorisationModel = authorisationModel;
            }
            #endregion

            #region Address of Treatment Facility
            BioCapstoneAddressTreatmentFacilityViewModel TreatmentModel = new BioCapstoneAddressTreatmentFacilityViewModel();
            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[1].Rows[0];
                TreatmentModel.Id= Convert.ToInt32(row["Id"]);
                TreatmentModel.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                TreatmentModel.DistrictName = row["DistrictName"].ToString();
                TreatmentModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                TreatmentModel.MandalName = row["MandalName"].ToString();
                TreatmentModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                TreatmentModel.VillageName = row["VillageName"].ToString();
                TreatmentModel.HouseNumber = row["HouserNo"].ToString();
                TreatmentModel.StreetName = row["StreetName"].ToString();
                TreatmentModel.Fax = row["Fax"].ToString();
                TreatmentModel.Telegraph = row["Telegraph"].ToString();
                TreatmentModel.Telex = row["Telex"].ToString();
                TreatmentModel.PINCode = row["PINCode"].ToString();
                TreatmentModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                BioCapstoneModel.BioCapstoneAddressTreatementFacilityModel = TreatmentModel;
            }
            #endregion

            #region Address of Disposal waste
            BioCapstoneAddressofDisposalWasteViewModel WasteModel = new BioCapstoneAddressofDisposalWasteViewModel();
            if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[2].Rows[0];
                WasteModel.Id = Convert.ToInt32(row["Id"]);
                WasteModel.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                WasteModel.DistrictName = row["DistrictName"].ToString();
                WasteModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                WasteModel.MandalName = row["MandalName"].ToString();
                WasteModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                WasteModel.VillageName = row["VillageName"].ToString();
                WasteModel.HouseNumber = row["HouserNo"].ToString();
                WasteModel.StreetName = row["StreetName"].ToString();
                WasteModel.Fax = row["Fax"].ToString();
                WasteModel.Telegraph = row["Telegraph"].ToString();
                WasteModel.Telex = row["Telex"].ToString();
                WasteModel.PINCode = row["PINCode"].ToString();
                WasteModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                BioCapstoneModel.BioCapstoneAddressDisposalWasteModel = WasteModel;
            }
            #endregion

            #region Mode of treatment
            List<TreatmentModle> treatmentList = new List<TreatmentModle>();
            if(dsItems.Tables[3] !=null && dsItems.Tables[3].Rows.Count>0)
            {
                foreach (DataRow row in dsItems.Tables[3].Rows)
                {
                    TreatmentModle objTreatment = new TreatmentModle();
                   // DataRow row = dsItems.Tables[3].Rows[0];
                    objTreatment.Id = Convert.ToInt32(row["Id"]);
                    objTreatment.Description = row["Description"].ToString();
                    objTreatment.Attachment = row["FilePath"].ToString();
                    treatmentList.Add(objTreatment);
                }
                
            }
          //  BioCapstoneModel.TreatmentList = treatmentList;
            #endregion

            #region Mode of treatment and disposal
            List<TreatmentDisposalModle> treatmentDisposalList = new List<TreatmentDisposalModle>();
            if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow row in dsItems.Tables[4].Rows)
                {
                    TreatmentDisposalModle objTreatmentDisposal = new TreatmentDisposalModle();
                    //DataRow row = dsItems.Tables[4].Rows[0];
                    objTreatmentDisposal.Id = Convert.ToInt32(row["Id"]);
                    objTreatmentDisposal.Description = row["Description"].ToString();
                    objTreatmentDisposal.Attachment = row["FilePath"].ToString();
                    treatmentDisposalList.Add(objTreatmentDisposal);
                }
               
            }
                BioCapstoneModel.TreatmentDisposalList = treatmentDisposalList;
                #endregion

                #region Category an Quantity of Waste
                List<QuantityWasteModel> quantityWasteList = new List<QuantityWasteModel>();
            if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
            {
                foreach (DataRow row in dsItems.Tables[5].Rows)
                {
                    QuantityWasteModel objquantityWaste = new QuantityWasteModel();
                   // DataRow row = dsItems.Tables[5].Rows[0];
                    objquantityWaste.Id = Convert.ToInt32(row["Id"]);
                    objquantityWaste.CategoryName = row["Category"].ToString();
                    objquantityWaste.Quantity = row["Qunatity"].ToString();
                    objquantityWaste.UnitName = row["Units"].ToString();
                    objquantityWaste.QuantityOthers = row["Others"].ToString();
                    quantityWasteList.Add(objquantityWaste);
                }
                
            }
                BioCapstoneModel.QuantityWasteList = quantityWasteList;
                #endregion

                #region Declaration
              
            if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
            {
                DeclarationViewModel Declaration = new DeclarationViewModel();
                DataRow row = dsItems.Tables[6].Rows[0];
                Declaration.Id = Convert.ToInt32(row["Id"]);
                Declaration.Date =Convert.ToDateTime(row["Date"].ToString());
                Declaration.Place = row["Place"].ToString();
                Declaration.Signature = row["Signature"].ToString();
                Declaration.Designation = row["Designation"].ToString();
                Declaration.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                BioCapstoneModel.DeclarationModel = Declaration;
            }
            #endregion

            #region Authorisation Details
           
            if (dsItems.Tables[7] != null && dsItems.Tables[7].Rows.Count > 0)
            {
                AuthorisationViewModel authorisationActivityModel = new AuthorisationViewModel();
                DataRow row = dsItems.Tables[7].Rows[0];
                authorisationActivityModel.Id = Convert.ToInt32(row["Id"]);
                authorisationActivityModel.Authorasation = row["Authorisation"].ToString();
                authorisationActivityModel.Others = row["AuthorisationOther"].ToString();
                authorisationActivityModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                BioCapstoneModel.AuthorisationModel = authorisationActivityModel;
            }
            #endregion
           
            BioCapstoneModel.TransactionId = transactionId;
            BioCapstoneModel.TreatmentList = treatmentList;
            if (BioCapstoneModel.TreatmentList!=null && BioCapstoneModel.TreatmentList.Count > 0)
            BioCapstoneModel.TreatmentModle.FormStatus = FormStatus.Completed;
            if (BioCapstoneModel.TreatmentDisposalList !=null && BioCapstoneModel.TreatmentDisposalList.Count > 0)
                BioCapstoneModel.TreatmentDisposalModle.FormStatus = FormStatus.Completed;
            if (BioCapstoneModel.QuantityWasteList !=null && BioCapstoneModel.QuantityWasteList.Count > 0)
                BioCapstoneModel.QuantityWasteModel.FormStatus = FormStatus.Completed;
            return BioCapstoneModel;
        }
        #endregion
        #region Organ Transplantation
        public bool SaveOrganTransplantation(OrganTransplantViewModel model, ref int applicationId, ref int transactionId)
        {
            LicenseDAL objDAL = new LicenseDAL();
            return objDAL.SaveOrganTransplantation(model, ref applicationId, ref transactionId);
        }

        #endregion
        #region Homeopathy
        public int SaveHomeopathyDetails(HomeopathyDrugStoreViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new LicenseDAL();
            return objDAL.SaveHomeopathyDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveHomeopathyApplicant(ApplicantViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new LicenseDAL();
            return objDAL.SaveHomeopathyApplicant(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveHomeopathyEstablishment(HomeopathyEstablishmentViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new LicenseDAL();
            return objDAL.SaveHomeopathyEstablishment(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveHomeopathyCompetent(ApplicantViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new LicenseDAL();
            return objDAL.SaveHomeopathyCompetent(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveHomeopathyDeclaration(HomeopathyDeclaration model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new LicenseDAL();
            return objDAL.SaveHomeopathyDeclaration(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        #endregion

        #region NOC for Equipment amendment
        public DataTable GetEquipmentList(int TransactionId)
        {
            objDAL = new LicenseDAL();
            return objDAL.GetEquipmentList(TransactionId);
        }
        public bool SaveEquipmentNOC(List<NOCforquipmentModel> objModelList)
        {
            objDAL = new LicenseDAL();
            int Count=0;
            foreach (NOCforquipmentModel item in objModelList)
            {
                bool Result = objDAL.SaveEquipmentNOC(item);
                if (Result)
                      Count++;               
            }
            if (Count == objModelList.Count )
                return true;
            else
                return false;
            
        }
        public DataTable GetAmmendments(int userId)
        {
            objDAL = new LicenseDAL();
            return objDAL.GetAmmendments(userId);
        }
        public NOCforquipmentModel GetNOCCertificateData(int AmendmentId)
        {
            objDAL = new LicenseDAL();
            NOCforquipmentModel objNOC = new NOCforquipmentModel();
            objNOC.EquipmentDetails = new EquipmentModel();
            DataTable dt= objDAL.GetNOCCertificateData(AmendmentId);
            if(dt!=null && dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    objNOC.LicenseNumber = dr["LicenseNumber"].ToString();
                    objNOC.EquipmentDetails.Name = dr["EquipmentName"].ToString();
                    objNOC.EquipmentDetails.Make = dr["Make"].ToString();
                    objNOC.EquipmentDetails.MachineModel = dr["Model"].ToString();
                    objNOC.EquipmentDetails.SerialNumber = dr["SerialNumber"].ToString();
                    objNOC.FacilityName = dr["FacilityName"].ToString();
                    objNOC.DistrictName = dr["DistrictName"].ToString().ToUpper();
                    objNOC.Date = dr["Date"].ToString();
                    objNOC.Address = dr["Address"].ToString();
                    objNOC.DistrictState = dr["DistrictState"].ToString();
                    
                }
            }
            return objNOC;
        }
        #endregion

        public bool UpdateUserIdForExistingData(int UserId, int TransactionId)
        {
            objDAL = new LicenseDAL();
            return objDAL.UpdateUserIdForExistingData(UserId, TransactionId);
        }
        public bool AppealApproval(ApprovalsModel approval)
        {
            objDAL = new LicenseDAL();
            return objDAL.AppealApproval(approval);
        }

        public bool SaveUpdateCertificatePath(int transId,string certificatePath,int userId)
        {
            objDAL = new LicenseDAL();
            return objDAL.SaveUpdateCertificatePath(transId,certificatePath, userId);
        }
        public int CheckIsCertificateSavedInFolder(int transId)
        {
            objDAL = new LicenseDAL();
            return objDAL.CheckIsCertificateSavedInFolder(transId);
        }
        public DataTable GetUserMailId(int transId)
        {
            objDAL = new LicenseDAL();
            return objDAL.GetUserMailId(transId);
        }

        public DataTable GetTAMCEUploadedDocsData(int transactionId,int docsRoleTypeId)
        {
            try
            {
                objDAL = new LicenseDAL();
                DataTable dsItems = objDAL.GetTAMCEUploadedDocsData(transactionId,docsRoleTypeId);
                return dsItems;                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
