using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Capstone.BAL
{
    public class AmendmentBAL
    {
        #region Global
        AmendmentDAL objDAL;
        #endregion
        public AmendmentModel GetCorrespondentDetails(int TransectionId, AmendmentModel model)
        {

            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            // ApmceAmendmentModel model = new ApmceAmendmentModel(); 
            dt = objDAL.GetCorrespodentDetails(TransectionId);

            if (dt.Rows.Count > 0)
            {

                model.CorrespondingAddressViewModel.Name = dt.Rows[0]["Name"].ToString();
                model.CorrespondingAddressViewModel.HouseNumber = dt.Rows[0]["HouseNumber"].ToString();
                model.CorrespondingAddressViewModel.StreetName = dt.Rows[0]["StreetName"].ToString();
                model.CorrespondingAddressViewModel.PINCode = dt.Rows[0]["PINCode"].ToString();
                model.CorrespondingAddressViewModel.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"].ToString());
                model.CorrespondingAddressViewModel.MandalId = Convert.ToInt32(dt.Rows[0]["MandalId"].ToString());
                model.CorrespondingAddressViewModel.VillageId = Convert.ToInt32(dt.Rows[0]["VillageId"].ToString());
                model.CorrespondingAddressViewModel.DistrictName = dt.Rows[0]["DistrictName"].ToString();
                model.CorrespondingAddressViewModel.MandalName = dt.Rows[0]["MandalName"].ToString();
                model.CorrespondingAddressViewModel.VillageName = dt.Rows[0]["VillageName"].ToString();


            }
            return (model);
        }
        public AmendmentModel GetTrustDetails(int TransectionId, AmendmentModel model)
        {

            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();

            dt = objDAL.GetTrustDetails(TransectionId);

            if (dt.Rows.Count > 0)
            {

                model.CorrespondingAddressViewModel.Name = dt.Rows[0]["Name"].ToString();
                model.CorrespondingAddressViewModel.HouseNumber = dt.Rows[0]["HouseNumber"].ToString();
                model.CorrespondingAddressViewModel.StreetName = dt.Rows[0]["StreetName"].ToString();
                model.CorrespondingAddressViewModel.PINCode = dt.Rows[0]["PINCode"].ToString();
                model.CorrespondingAddressViewModel.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"].ToString());
                model.CorrespondingAddressViewModel.MandalId = Convert.ToInt32(dt.Rows[0]["MandalId"].ToString());
                model.CorrespondingAddressViewModel.VillageId = Convert.ToInt32(dt.Rows[0]["VillageId"].ToString());
                model.CorrespondingAddressViewModel.DistrictName = dt.Rows[0]["DistrictName"].ToString();
                model.CorrespondingAddressViewModel.MandalName = dt.Rows[0]["MandalName"].ToString();
                model.CorrespondingAddressViewModel.VillageName = dt.Rows[0]["VillageName"].ToString();


            }
            return (model);
        }
        public AmendmentModel GetEquipmentDetails(int TransectionId, AmendmentModel model)
        {

            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();

            dt = objDAL.GetEquipmentDetails(TransectionId);

            if (dt.Rows.Count > 0)
            {


                model.InfraStructureModel.Name = dt.Rows[0]["Name"].ToString();
                model.InfraStructureModel.Quantity = Convert.ToInt32(dt.Rows[0]["Quantity"].ToString());
                model.InfraStructureModel.ItemModel = dt.Rows[0]["ItemModel"].ToString();
                model.InfraStructureModel.Remarks = dt.Rows[0]["Remarks"].ToString();
                //  model.CorrespondingAddressViewModel.DistrictId = Convert.ToInt32(dt.Rows[0]["UploadedFilePath"].ToString());



            }
            return (model);
        }
        public PCPNDTViewModel GetFacilityDetails(int TransectionId, PCPNDTViewModel model)
        {

            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();

            dt = objDAL.GetFacilityDetails(TransectionId, model);

            if (dt.Rows.Count > 0)
            {
                model.FacilityModel.Name = dt.Rows[0]["Name"].ToString();
                model.FacilityModel.Faclities = dt.Rows[0]["Facilities"].ToString();
                model.FacilityModel.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"].ToString());
                model.FacilityModel.MandalId = Convert.ToInt32(dt.Rows[0]["MandalId"].ToString());
                model.FacilityModel.VillageId = Convert.ToInt32(dt.Rows[0]["VillageId"].ToString());
                model.FacilityModel.DistrictName = dt.Rows[0]["DistrictName"].ToString();
                model.FacilityModel.MandalName = dt.Rows[0]["MandalName"].ToString();
                model.FacilityModel.VillageName = dt.Rows[0]["VillageName"].ToString();
                model.FacilityModel.Phone = dt.Rows[0]["Phone"].ToString();
                model.FacilityModel.HouseNumber = dt.Rows[0]["HouseNumber"].ToString();
                model.FacilityModel.StreetName = dt.Rows[0]["StreetName"].ToString();
                model.FacilityModel.PINCode = dt.Rows[0]["PINCode"].ToString();
                model.FacilityModel.Email = dt.Rows[0]["Email"].ToString();
                model.FacilityModel.Fax = dt.Rows[0]["Fax"].ToString();
                model.FacilityModel.Telegraph = dt.Rows[0]["Telegraph"].ToString();
                model.FacilityModel.Telex = dt.Rows[0]["Telex"].ToString();
                model.FacilityModel.AddressProofPath = dt.Rows[0]["AddressProofDocPath"].ToString();
                model.FacilityModel.BuildingLayoutPath = dt.Rows[0]["BuildingLayoutDocPath"].ToString();



            }
            return (model);
        }
        public AmendmentModel GetFacilitiesDetails(int TransectionId, AmendmentModel model)
        {

            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();

            dt = objDAL.GetFacilitiesDetails(TransectionId);

            if (dt.Rows.Count > 0)
            {
                model.FacilityModel.Name = dt.Rows[0]["Name"].ToString();
                model.FacilityModel.Faclities = dt.Rows[0]["Facilities"].ToString();
                model.FacilityModel.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"].ToString());
                model.FacilityModel.MandalId = Convert.ToInt32(dt.Rows[0]["MandalId"].ToString());
                model.FacilityModel.VillageId = Convert.ToInt32(dt.Rows[0]["VillageId"].ToString());
                model.FacilityModel.DistrictName = dt.Rows[0]["DistrictName"].ToString();
                model.FacilityModel.MandalName = dt.Rows[0]["MandalName"].ToString();
                model.FacilityModel.VillageName = dt.Rows[0]["VillageName"].ToString();

                model.FacilityModel.Phone = dt.Rows[0]["Phone"].ToString();
                model.FacilityModel.HouseNumber = dt.Rows[0]["HouseNumber"].ToString();
                model.FacilityModel.StreetName = dt.Rows[0]["StreetName"].ToString();
                model.FacilityModel.PINCode = dt.Rows[0]["PINCode"].ToString();

                model.FacilityModel.Email = dt.Rows[0]["Email"].ToString();
                model.FacilityModel.Fax = dt.Rows[0]["Fax"].ToString();
                model.FacilityModel.Telegraph = dt.Rows[0]["Telegraph"].ToString();
                model.FacilityModel.Telex = dt.Rows[0]["Telex"].ToString();
                model.FacilityModel.AddressProofPath = dt.Rows[0]["AddressProofDocPath"].ToString();
                model.FacilityModel.BuildingLayoutPath = dt.Rows[0]["BuildingLayoutDocPath"].ToString();



            }
            return (model);
        }
        public PCPNDTViewModel GetEmployeeDetails(int TransectionId, PCPNDTViewModel model)
        {
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetEmployeeDetails(TransectionId);
            if (dt.Rows.Count > 0)
            {
                model.EmployeeModel.Name = dt.Rows[0]["Name"].ToString();
                model.EmployeeModel.Experience = dt.Rows[0]["Experience"].ToString();
                model.EmployeeModel.DesignationId = Convert.ToInt32(dt.Rows[0]["DesignationId"].ToString());
                model.EmployeeModel.DesignationName = dt.Rows[0]["DesignationName"].ToString();
                model.EmployeeModel.RegistrationNumber = dt.Rows[0]["RegistrationNumber"].ToString();
                model.EmployeeModel.UploadedFilePath = dt.Rows[0]["UploadedFilePath"].ToString();
            }
            return (model);
        }
        public APMCEViewModel GetAPMCEData(int transactionId,string Type) 
        {
            objDAL = new AmendmentDAL();     
            DataSet dsItems = objDAL.GetAPMCEData(transactionId, Type);  
            if (dsItems == null)
                return null;

            APMCEViewModel apmceModel = new APMCEViewModel();
            #region Get Registration details
            RegistrationViewModel registrationModel = new RegistrationViewModel();
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[0].Rows[0];
                registrationModel.Id = Convert.ToInt32(row["Id"]);
                registrationModel.FacilityType = row["FacilityType"].ToString();
                registrationModel.HospitalType = row["HospitalType"].ToString();
                registrationModel.ClinicType = row["ClinicType"].ToString();
                if(string.IsNullOrEmpty(row["BedStrength"].ToString()))
                    registrationModel.BedStrength = "-";
                else
                    registrationModel.BedStrength = row["BedStrength"].ToString();
                if (string.IsNullOrEmpty(row["BedStrength"].ToString()))
                    registrationModel.Speciality = "-";
                else
                    registrationModel.Speciality = row["Speciality"].ToString();

                registrationModel.Name = row["RegistrationName"].ToString();
                registrationModel.DistrictId =Convert.ToInt32(row["DistrictId"].ToString());
                registrationModel.DistrictName = row["DistrictName"].ToString();
                registrationModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                registrationModel.MandalName = row["MandalName"].ToString();
                registrationModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                registrationModel.VillageName = row["VillageName"].ToString();
                registrationModel.HouseNumber = row["HouseNumber"].ToString();
                registrationModel.StreetName = row["StreetName"].ToString();
                registrationModel.PINCode = row["PINCode"].ToString();
                registrationModel.BuildingHeight = Convert.ToInt32(row["BuildingHeight"]);
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
                correspondingAddressModel.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                correspondingAddressModel.DistrictName = row["DistrictName"].ToString();
                correspondingAddressModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                correspondingAddressModel.MandalName = row["MandalName"].ToString();
                correspondingAddressModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                correspondingAddressModel.VillageName = row["VillageName"].ToString();
                correspondingAddressModel.HouseNumber = row["HouseNumber"].ToString();
                correspondingAddressModel.StreetName = row["StreetName"].ToString();
                correspondingAddressModel.PINCode = row["PINCode"].ToString();
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
                trustModel.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                trustModel.DistrictName = row["DistrictName"].ToString();
                trustModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                trustModel.MandalName = row["MandalName"].ToString();
                trustModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                trustModel.VillageName = row["VillageName"].ToString();
                trustModel.HouseNumber = row["HouseNumber"].ToString();
                trustModel.StreetName = row["StreetName"].ToString();
                trustModel.PINCode = row["PINCode"].ToString();
                trustModel.EstablishedDate = Convert.ToDateTime(row["EstablishedDate"]);
            }
            apmceModel.TrustModel = trustModel;
            #endregion

            #region Accommodation details
            AccommadationViewModel AccommadationModel = new AccommadationViewModel();
            if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[3].Rows[0];
                AccommadationModel.Id =Convert.ToInt32(row["Id"].ToString());
                AccommadationModel.EstablishmentTypeName = row["EstablishmentOwnedBy"].ToString();
                AccommadationModel.FromDate = Convert.ToDateTime(row["FromDate"]);
                AccommadationModel.ToDate = Convert.ToDateTime(row["ToDate"]);
                AccommadationModel.UploadedFilePath = row["UploadedDoc"].ToString();
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
            }
            apmceModel.EstablishmentModel = establishmentModel;
            #endregion 

            #region Services Offered Details
            OfferedServicesModel offeredsserviceModel = new OfferedServicesModel();
            if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[5].Rows[0];
                offeredsserviceModel.Id = Convert.ToInt32(row["Id"]);
                offeredsserviceModel.BedStrength = Convert.ToInt32(row["BedStrength"]);
                offeredsserviceModel.OfferedServices = row["OfferedService"].ToString();
                offeredsserviceModel.OfferedServiceIDs = row["OfferedServicesIDs"].ToString();
            }
            apmceModel.OfferedServices = offeredsserviceModel;
            #endregion 

            #region Staff Details
            //StaffDetailsViewModel staffdetailsModel = new StaffDetailsViewModel();
            List<StaffDetailsModel> staffdetailsModel = new List<StaffDetailsModel>();
            if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
            {
               // DataRow row = dsItems.Tables[6].Rows[0];
                StaffDetailsModel staffdetailsList;
                foreach(DataRow row in dsItems.Tables[6].Rows)
                {
                    staffdetailsList = new StaffDetailsModel();
                    staffdetailsList.Id = Convert.ToInt32(row["Id"].ToString());
                    staffdetailsList.StaffDesignation = row["Designation"].ToString();
                    staffdetailsList.Name = row["Name"].ToString();
                    staffdetailsList.RegistrationNumber = row["RegistrationNo"].ToString();
                    staffdetailsList.UploadedFilePath = row["StaffDetailsDocPath"].ToString();
                    staffdetailsList.PhoneNumber = row["PhoneNumber"].ToString();
                    staffdetailsList.Email = row["EmailId"].ToString();
                    staffdetailsModel.Add(staffdetailsList);
                }
                
            }
           
            #endregion 

            #region Equipment Furniture Details
            List<InfraStructureModel> infraFurnitureModel = new List<InfraStructureModel>();
           // InfraStructureModel infraFurnitureModel = new InfraStructureModel();
            if (dsItems.Tables[7] != null && dsItems.Tables[7].Rows.Count > 0)
            {
                InfraStructureModel infrastructurelist;

                foreach(DataRow row in dsItems.Tables[7].Rows)
                {
                    infrastructurelist = new InfraStructureModel();
                    infrastructurelist.Id =Convert.ToInt32(row["Id"].ToString());
                    infrastructurelist.Name = row["EquipmentName"].ToString();
                    infrastructurelist.Quantity = Convert.ToInt32(row["Quantity"]);
                    infrastructurelist.ItemModel = row["Model"].ToString();
                    infrastructurelist.Remarks = row["Remarks"].ToString();
                    infrastructurelist.UploadedFilePath = row["UploadDoc"].ToString();
                    infraFurnitureModel.Add(infrastructurelist);
                }
               
            }
           
            #endregion

            #region Facilities Available
            FacilitiesAvailableModel facilitiesmodel = new FacilitiesAvailableModel();
            if (dsItems.Tables[8] != null && dsItems.Tables[8].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[8].Rows[0];
               facilitiesmodel.Id = Convert.ToInt32(row["Id"]);
               facilitiesmodel.HasLaborRoom =Convert.ToBoolean(row["LaborFacility"].ToString());
               facilitiesmodel.HasOperationTheater = Convert.ToBoolean(row["OperationTheater"].ToString());
               facilitiesmodel.HasDiagnosticFacility = Convert.ToBoolean(row["DiagnosticksFacility"].ToString());
               facilitiesmodel.HasDeclarationStamp = Convert.ToBoolean(row["HasDeclarationStamp"].ToString());
                facilitiesmodel.DeclarationStampFilePath = row["NonJudicialStamp"].ToString();
               facilitiesmodel.OtherInformationDescription = row["OtherInformation"].ToString();
               facilitiesmodel.OtherInformationDocumentPath = row["OtherDocs"].ToString();
              // facilitiesmodel.FormStatus = row["FormStatus"].ToString();
               facilitiesmodel.Id = Convert.ToInt32(row["Id"].ToString());
               
            }
            apmceModel.FacilitiesAvailableModel = facilitiesmodel;
            #endregion

            #region License cancel
            CancelLicenseModel LicenseModel = new CancelLicenseModel();
            if (dsItems.Tables[9] != null && dsItems.Tables[9].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[9].Rows[0];
                LicenseModel.LicenseNo = row["LicenseNumber"].ToString();
                LicenseModel.IsseuDate = Convert.ToDateTime(row["LicenseIssuedDate"].ToString());
                LicenseModel.ExpireDate = Convert.ToDateTime(row["LicenseExpiryDate"].ToString());
            }
            #endregion

            #region APMCE Certificate
            APMCECertificate apmceCertificateModel = GetAPMCELicense(transactionId,Type);
            #endregion
            apmceModel.APMCECertificateModel = apmceCertificateModel;
            apmceModel.cancelLiceseModel = LicenseModel;
            apmceModel.TransactionId = transactionId;
            apmceModel.StaffDetailsList = staffdetailsModel;
            if (apmceModel.StaffDetailsList.Count > 0)
                apmceModel.StaffDetails.FormStatus = FormStatus.Completed;
            apmceModel.InfraStructureList = infraFurnitureModel;
            if (apmceModel.InfraStructureList.Count > 0)
                apmceModel.InfraStructure.FormStatus = FormStatus.Completed;

            return apmceModel;
        }
        public PCPNDTViewModel GetPCPNDTData(int transactionId,string Type)
        {
            try
            {
                objDAL = new AmendmentDAL();
                DataSet dsItems = objDAL.GetPCPNDTData(transactionId, Type);
                if (dsItems == null)
                    return null;
                 PCPNDTViewModel model = new PCPNDTViewModel();

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

                #region Facility Details
                FacilityViewModel facilityModel = new FacilityViewModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    facilityModel.Id = Convert.ToInt32(row["Id"]);
                    facilityModel.Name = row["Name"].ToString();
                    facilityModel.Faclities = row["Facilities"].ToString();
                    facilityModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    facilityModel.DistrictName = row["DistrictName"].ToString();
                    facilityModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    facilityModel.MandalName = row["MandalName"].ToString();
                    facilityModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    facilityModel.VillageName = row["VillageName"].ToString();
                    facilityModel.HouseNumber = row["HouseNumber"].ToString();
                    facilityModel.StreetName = row["StreetName"].ToString();
                    facilityModel.Phone = row["Phone"].ToString();
                    facilityModel.Email = row["Email"].ToString();
                    facilityModel.Fax = row["Fax"].ToString();
                    facilityModel.Telegraph = row["Telegraph"].ToString();
                    facilityModel.Telex = row["Telex"].ToString();
                    facilityModel.PINCode = row["PINCode"].ToString();
                    facilityModel.AddressProofPath = row["AddressProofDocPath"].ToString();
                    facilityModel.BuildingLayoutPath = row["BuildingLayoutDocPath"].ToString();
                    facilityModel.OwnershipType = Convert.ToString(row["OwnershipType"]);
                    facilityModel.OwnerShipPath = Convert.ToString(row["OwnershipDocPath"]);
                    facilityModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
               // model.FacilityModel = facilityModel;
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
                        equipment.InvoicePath = row["InvoicePath"].ToString();
                        equipment.NocFilePath = row["NOCFliePath"].ToString();
                        equipment.TransferCertificatePath= row["TransferCertificateFilePath"].ToString();
                        equipment.InstallationCerticatePath= row["InstallationCertificatePath"].ToString();
                        equipment.ImagePath= row["ImagePath"].ToString();
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
                        employee.SubDesignation = Convert.ToString(row["SubDesignation"]);
                        employee.ExpYears = Convert.ToInt32(row["ExpYears"]);
                        employee.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                        employee.ExpDays = Convert.ToInt32(row["ExpDays"]);
                        employee.RegistrationNumber = row["RegistrationNumber"].ToString();                        
                        employeeList.Add(employee);
                    }
                }

                List<DocumentUploadModel> employeeUploads = new List<DocumentUploadModel>();
                DocumentUploadModel employeeUpload;
                foreach (DataRow row in dsItems.Tables[12].Rows)
                {
                    employeeUpload = new DocumentUploadModel();
                    employeeUpload.Id = Convert.ToInt32(row["Id"]);
                    employeeUpload.ReferenceTable = Convert.ToString(row["ReferenceTable"]);
                    employeeUpload.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                    employeeUpload.DocumentPath = Convert.ToString(row["DocumentPath"]);
                    employeeUpload.UploadType = Convert.ToString(row["UploadType"]);
                    employeeUploads.Add(employeeUpload);
                }

                foreach (var emp in employeeList)
                {
                    emp.UploadDocuments = employeeUploads.Where(item => item.ReferenceId == emp.Id).ToList();
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

                    institutionModel.AffidavitDocPath = row["AffidavitDocPath"].ToString();
                    institutionModel.ArticleDocPath = row["ArticleDocPath"].ToString();
                    institutionModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                    List<DocumentUploadModel> studycertificateList = new List<DocumentUploadModel>();
                    if (dsItems.Tables[8] != null && dsItems.Tables[8].Rows.Count > 0)
                    {
                        DocumentUploadModel studycertificate;
                        foreach (DataRow row1 in dsItems.Tables[8].Rows)
                        {
                            studycertificate = new DocumentUploadModel();
                            studycertificate.Id = Convert.ToInt32(row1["Id"]);
                            studycertificate.DocumentPath = row1["DocumentPath"].ToString();
                            studycertificateList.Add(studycertificate);
                        }
                    }
                    institutionModel.StudyCertificateDocPaths = studycertificateList;
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

                #region License cancel
                CancelLicenseModel LicenseModel = new CancelLicenseModel();
                if (dsItems.Tables[10] != null && dsItems.Tables[10].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[10].Rows[0];
                    LicenseModel.LicenseNo = row["LicenseNumber"].ToString();
                    LicenseModel.IsseuDate =Convert.ToDateTime(row["LicenseIssuedDate"].ToString());
                    LicenseModel.ExpireDate =Convert.ToDateTime(row["LicenseExpiryDate"].ToString());
                   

                }
                #endregion

                #region PCPNDT Certificate
                PCPNDTLicenseInfoModel PcpndtModel = GetLicenses(transactionId, Type);
                
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

                model.PCPNDTLicenseModel = PcpndtModel;
               // PcpndtModel.GetLicense( transactionId);

                model.ApplicantModel = applicantModel;
                //pcpndtModel.EstablishmentModel = establishmentModel;
                model.FacilityModel = facilityModel;
                model.TestsModel = testsModel;
                model.EquipmentList = equipmentList;
                if (model.EquipmentList.Count > 0)
                    model.EquipmentModel.FormStatus = FormStatus.Completed;
                model.FacilitiesModel = facilitiesModel;
                model.EmployeeList = employeeList;
                if (model.EmployeeList.Count > 0)
                    model.EmployeeModel.FormStatus = FormStatus.Completed;
                model.InstitutionModel = institutionModel;
                model.DeclarationModel = declarationModel;
                model.TransactionId = transactionId;
                model.cancelLiceseModel = LicenseModel;

                return model;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log      
                return null;
            }
        }
        public APMCECertificate GetAPMCELicense(int transactionId,string Type)
        {
           
            LicenseBAL objBAL = new LicenseBAL();
            DataTable dtItems = objBAL.GetLicenseType(transactionId, Type);
            APMCECertificate model = new APMCECertificate();
            if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
            {
                model = objBAL.GetAPMCECertificate(transactionId, Type);
                return model;
            }
            else
            {
                return null;
            }
        }
        public PCPNDTLicenseInfoModel GetLicenses(int transactionId,string Type)
        {

          
            LicenseBAL objBAL = new LicenseBAL();
            DataTable dtItems = objBAL.GetLicenseType(transactionId, Type);
            PCPNDTLicenseInfoModel model = new PCPNDTLicenseInfoModel();
            //if (dtItems.Rows[0]["ActType"].ToString() == "APMCE")
            //{
            //    model = objBAL.GetAPMCECertificate(transactionId);
            //    return PartialView("_APMCETemporaryCertificate", model.APMCECertificate);
            //}
             if (dtItems.Rows[0]["ActType"].ToString() == "PCPNDT")
            {
                
                model = objBAL.GetPCPNDTLicenseDetails(transactionId, Type);
                //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
                //PCPNDTmodel = model.PCPNDTCertificate;
                return  model;
            }
            //else if (dtItems.Rows[0]["ActType"].ToString() == "BloodBank")
            //{
            //    // model.BloodBankNOCModel = objBAL.GetPCPNDTLicenseDetails(TransactionId);
            //    //PCPNDTLicenseInfoModel PCPNDTmodel= new PCPNDTLicenseInfoModel();
            //    //PCPNDTmodel = model.PCPNDTCertificate;
            //    return PartialView("_BloodBankNOC");  //,   model.PCPNDTCertificate);
            //}
            else
            {
                return null;
            }

        }
        public int SaveEmployeeDetails(List<EmployeeViewModel> model, int TransectionId, int UserId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveEmployeeDetails(model, TransectionId, UserId);

        }
        public PCPNDTViewModel GetInstitutionDetails(int TransectionId, PCPNDTViewModel model)
        {
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetInstitutionDetails(TransectionId, model);
            if (dt.Rows.Count > 0)
            {
                model.InstitutionModel.OwnershipTypeName = dt.Rows[0]["OwnershipTypeName"].ToString();
                model.InstitutionModel.InstitutionTypeName = dt.Rows[0]["InstitutionTypeName"].ToString();
                model.InstitutionModel.TotalWorkArea = Convert.ToDecimal(dt.Rows[0]["TotalWorkArea"].ToString());

            }
            return (model);
        }
        public PCPNDTViewModel GetEquipmentDetails(int TransectionId, PCPNDTViewModel model)
        {
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetEquipmentDetails(TransectionId, model);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow roe in dt.Rows)
                {


                    model.EquipmentModel.Name = dt.Rows[0]["Name"].ToString();
                    model.EquipmentModel.SerialNumber = dt.Rows[0]["SerialNumber"].ToString();
                    model.EquipmentModel.Make = dt.Rows[0]["Make"].ToString();
                    model.EquipmentModel.Type = dt.Rows[0]["Type"].ToString();
                    model.EquipmentModel.UploadedFilePath = dt.Rows[0]["UploadedFilePath"].ToString();
                }
            }
            return (model);
        }
        public PCPNDTViewModel GetFacilitiesDetails(int TransectionId, PCPNDTViewModel model)
        {
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetFacilitiesDetails(TransectionId, model);
            if (dt.Rows.Count > 0)
            {
                model.FacilitiesModel.Tests = dt.Rows[0]["Tests"].ToString();
                model.FacilitiesModel.Studies = dt.Rows[0]["Studies"].ToString();
                model.FacilitiesModel.Remarks = dt.Rows[0]["Remarks"].ToString();


            }
            return (model);
        }
        public List<EmployeeViewModel> GetEmployees(int transactionId)
        {
            objDAL = new AmendmentDAL();
            DataTable dtItems = objDAL.GetEmployees(transactionId);
            if (dtItems == null)
                return null;

            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            EmployeeViewModel employee;
            foreach (DataRow row in dtItems.Rows)
            {
                employee = new EmployeeViewModel();
                employee.Id = Convert.ToInt32(row["Id"]);
                employee.Name = Convert.ToString(row["Name"]);
                employee.DesignationId = Convert.ToInt32(row["DesignationId"]);
                employee.DesignationName = Convert.ToString(row["DesignationName"]);
                employee.Experience = Convert.ToString(row["Experience"]);
                employee.RegistrationNumber = Convert.ToString(row["RegistrationNumber"]);
                employee.UploadedFilePath = Convert.ToString(row["UploadedFilePath"]);
                employee.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
                employeeList.Add(employee);
            }
            return employeeList;
        }

        #region Renewal for APMCE
        public APMCERenewalModel GetAPMCERenewalDetails(int TransectionId)
        {
            try
            {
                objDAL = new AmendmentDAL();
                DataSet dsItems = objDAL.GetAPMCERenewalDetails(TransectionId);
                if (dsItems == null)
                    return null;
                APMCERenewalModel model = new APMCERenewalModel();
                DataRow row = dsItems.Tables[0].Rows[0];
                model.ApplicationNumber = Convert.ToString(row["ApplicationNumber"]);
                model.ApplicationDate = Convert.ToDateTime(row["ApplicationDate"]);
                model.IssuedDate = Convert.ToDateTime(row["LicenseIssuedDate"]);
                model.ExpiryDate = Convert.ToDateTime(row["LicenseExpiryDate"]);
                model.RenewalDate = Convert.ToDateTime(row["RenewalDate"]);
                model.RenewalValidDate = Convert.ToDateTime(row["RenewalValidDate"]);
                // Services List
                foreach (DataRow seriverow in dsItems.Tables[1].Rows)
                {
                    model.ServiceDetails.Add(seriverow["Services"].ToString());
                }
                return model;
            }
            catch(Exception ex)
            {
                // TODO: Write exception log        - Jai, 26-07-2017
                return null;
            }
        }
        #endregion

        #region APMCE Amendments

        public int SaveRegistrationDetails(RegistrationModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveRegistrationDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveCorrespondingAddressDetails(CorrespondingAddressModel model, 
          ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveCorrespondingAddressDetails(model, ref transactionId);
               
        }

        public int SaveAccommodationDetails(AccommodationModel model,
          ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveAccommodationDetails(model, ref transactionId);  

        }

        public int SaveTrustDetails(TrustModel model, ref int transactionId)

        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveTrustDetails(model, ref transactionId
               );
        }

        public int SaveInfraStructure(List<InfraStructureModel> objList,  ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveInfraStructure(objList,  ref transactionId);
        }

        public int SaveServicesOfferedDetails(OfferedServicesModel model, 
           ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveServicesOfferedDetails(model,ref transactionId
               );
        }
        public int SaveFacilitiesAvailable(FacilitiesAvailableModel model, 
           ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveFacilitiesAvailable(model,ref transactionId
               );
        }

        public int SaveStaffDetailsOLD(StaffDetailsModel model,
           ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveStaffDetailsOLD(model,  ref transactionId
                );
        }

        public int SaveStaffDetails(List<StaffDetailsModel> objList, ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveStaffDetails(objList, ref transactionId);
        }

        #endregion   

        #region PCPNDT Amendments

        #region Facility Details Saving 
        public int SaveFacilityAmendment(FacilityViewModel model, FacilitesModel facilities, TestsModel tests, 
            ref int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveFacilityAmendment(model, facilities, tests, ref transactionId);


        }
        #endregion

        #region PCPNDT Equipment Details
        public int SaveEquipments(List<EquipmentModel> objList, int TransectionId, int UserId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveEquipments(objList, TransectionId, UserId);
        }
        public List<EquipmentModel> GetEquipments(int transactionId)
        {
            objDAL = new AmendmentDAL();
            DataTable dtItems = objDAL.GetEquipments(transactionId);
            if (dtItems == null)
                return null;
            List<EquipmentModel> equipmentList = new List<EquipmentModel>();
            EquipmentModel equipment;
            foreach (DataRow row in dtItems.Rows)
            {
                equipment = new EquipmentModel();
                equipment.Id = Convert.ToInt32(row["Id"]);
                equipment.Name = Convert.ToString(row["Name"]);
                equipment.SerialNumber = Convert.ToString(row["SerialNumber"]);
                equipment.MachineModel = Convert.ToString(row["MachineModel"]);
                equipment.Make = Convert.ToString(row["Make"]);
                equipment.Type = Convert.ToString(row["Type"]);
                equipment.UploadedFilePath = Convert.ToString(row["UploadedFilePath"]);
                equipmentList.Add(equipment);
            }
            return equipmentList;
        }
        #endregion

        #region  Institution Details Saving for PCPNDT Amendment
        public int SaveInstitutionDetails(InstitutionModel model, ref int transactionId)

        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveInstitutionDetails(model, ref transactionId);

        }
        #endregion

        #region  Ownership Details Saving for PCPNDT Amendment
        public int SaveOwnershipDetails(InstitutionModel model, ref int transactionId)

        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveOwnershipDetails(model, ref transactionId);

        }
        #endregion

        #region Tests Details Saving for PCPNDT Amendment
        public int SaveTestsAmendment(TestsModel model, ref int transactionId)     //TODO:  kishore
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveTestsAmendment(model, ref transactionId);
        }
        #endregion  

        #region Facilities Details Saving for PCPNDT Amendment 
        public int SaveFacilitiesAmendment(FacilitesModel model, ref int transactionId)      //TODO:  kishore

        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveFacilitiesAmendment(model, ref transactionId);
        }
        #endregion  

        #region License Cancel  for PCPNDT
        public CancelLicenseModel GetLicenseSearch(CancelLicenseModel model)
        {
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            //  CancelLicenseModel licensecancel = new CancelLicenseModel();
            dt = objDAL.GetLicenseSearch(model);
            if (dt.Rows.Count > 0)
            {
                model.LicenseNo = dt.Rows[0]["LicenseNumber"].ToString();
                model.IsseuDate = Convert.ToDateTime(dt.Rows[0]["LicenseIssuedDate"].ToString());
                model.ExpireDate = Convert.ToDateTime(dt.Rows[0]["LicenseExpiryDate"].ToString());
                return (model);
            }
            else
            {
                return null;
            }
        }
        public int PCPNDTLicenseCancel(CancelLicenseModel model, int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.PCPNDTLicenseCancel(model, transactionId);
        }
        #endregion

       

        #region Appeal  for PCPNDT
        public AppealModel GetRemarks(AppealModel model, int transactionId)
        {
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetRemarks(model, transactionId);
            if (dt.Rows.Count > 0)
            {
                //model.Remarks =Convert.ToString(dt.Rows[0]["Remarks"].ToString());
            }
            return (model);
        }

        // Following method is not using    - Raj, 07-09-17
        //public int SaveAppealForPCPNDT(ApplicationModel model)
        //{
        //    objDAL = new AmendmentDAL();
        //    return objDAL.SaveAppealForPCPNDT(model);
        //}

        public int SaveAppeal(int transactionId, string remarks, int userId ,string type)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveAppeal(transactionId, remarks, userId,type);
        }
        #endregion

        #region Approved   and Rejected List
        public List<TransactionViewModel> GetApprovedList(UserModel model)
        {
            List<TransactionViewModel> ApprovedList = new List<TransactionViewModel>();
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetApprovedList(model);
            if (dt.Rows.Count > 0)
            {
                TransactionViewModel approved;
                foreach (DataRow row in dt.Rows)
                {
                    approved = new TransactionViewModel();
                    // approved.Id = Convert.ToInt32(row["Id"]);
                    approved.ApplicantName = row["ApplicantName"].ToString();
                    approved.ServiceName = row["ServiceName"].ToString();

                    ApprovedList.Add(approved);
                }
            }
            return ApprovedList;
        }
        public List<TransactionViewModel> GetRejectedList(UserModel model)
        {
            List<TransactionViewModel> RejectedList = new List<TransactionViewModel>();
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.GetRejectedList(model);
            if (dt.Rows.Count > 0)
            {
                TransactionViewModel approved;
                foreach (DataRow row in dt.Rows)
                {
                    approved = new TransactionViewModel();
                    // approved.Id = Convert.ToInt32(row["Id"]);
                    approved.ApplicantName = row["ApplicantName"].ToString();
                    approved.ServiceName = row["ServiceName"].ToString();

                    RejectedList.Add(approved);
                }
            }
            return RejectedList;
        }
        #endregion

        public List<string> CheckForAmendment(int transactionId)
        {
            List<string> serviceList = new List<string>();
            objDAL = new AmendmentDAL();
            DataTable dt = new DataTable();
            dt = objDAL.CheckForAmendment(transactionId);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    string serviceId;
                    serviceId = Convert.ToString(dr["ServiceId"]);
                    serviceList.Add(serviceId);
                }
            }
            return serviceList;
        }
        #endregion 

        #region Rennual  
        public int SaveRenewal(UserModel model, int transactionId)
        {
            objDAL = new AmendmentDAL();
            return objDAL.SaveRenewal(model, transactionId);
        }
        public PCPNDTLicenseInfoModel GetPCPNDTLicenseDetails(int transactionId)
        {
            try
            {
                objDAL = new AmendmentDAL();
                DataSet dsItems = objDAL.GetPCPNDTLicenseDetails(transactionId);
                if (dsItems == null)
                    return null;
                PCPNDTLicenseInfoModel model = new PCPNDTLicenseInfoModel();
                DataRow row = dsItems.Tables[0].Rows[0];
                model.IssuingAuthority = Convert.ToString(row["IssuingAuthority"]);
                model.AppropriateAuthority = Convert.ToString(row["AppropriateAuthority"]);
                model.Facilities = Convert.ToString(row["Facilities"]);
                model.ApplicantNameAddress = Convert.ToString(row["NameAddress"]);
                model.TestRemarks = Convert.ToString(row["TestsRemarks"]);
                model.FacilitiesRemarks = Convert.ToString(row["FacilitiesRemarks"]);
                model.LicenseNumber = Convert.ToString(row["LicenseNumber"]);
                model.LicenseIssuedDate = Convert.ToDateTime(row["LicenseIssuedDate"]);
                model.LicenseExpiryDate = Convert.ToDateTime(row["LicenseExpiryDate"]);

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
                    model.NonInvasiveTests = new List<string>();
                    foreach (DataRow test in dsItems.Tables[3].Rows)
                        model.NonInvasiveTests.Add(Convert.ToString(test["Tests"]));
                }

                // Bind Studies
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    model.NonInvasiveTests = new List<string>();
                    foreach (DataRow test in dsItems.Tables[4].Rows)
                        model.NonInvasiveTests.Add(Convert.ToString(test["Studies"]));
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
                        employee.Experience = Convert.ToString(datarow["Experience"]);
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
        #endregion

       
    }
}

