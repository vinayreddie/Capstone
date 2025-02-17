using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class AmendmentDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        SqlCommand command;
        #endregion
        public AmendmentDAL()
        {
            dbManager = new SqlServerDBManager();
        }
        public DataTable GetCorrespodentDetails(int TransectionId)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetCorrespondingDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 02-06-17
                return null;
            }
        }
        public DataTable GetTrustDetails(int TransectionId)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetTrustDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 02-06-17
                return null;
            }
        }
        public DataTable GetEquipmentDetails(int TransectionId)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetEquipmentDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }
        public DataTable GetFacilityDetails(int TransectionId, PCPNDTViewModel model)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetFacilityDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }
        public DataTable GetFacilitiesDetails(int TransectionId)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetFacilitiesDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }
        public DataTable GetEmployeeDetails(int TransectionId)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetEmployeeDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }
        public DataSet GetAPMCEData(int transactionId,string type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@Type", type);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAPMCEData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 13-05-2017
                return null;
            }
        }
        public DataSet GetPCPNDTData(int TransactionId, string Type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetPCPNDTData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 04-06-2017
                return null;
            }
        }


        //param = new SqlParameter("@Id", objEmployee.Id);
        //paramList.Add(param);
        //param = new SqlParameter("@TransactionId", TransactionId);
        //paramList.Add(param);
        //param = new SqlParameter("@Name", objEmployee.Name);
        //paramList.Add(param);
        //param = new SqlParameter("@DistrictId", objEmployee.DesignationId);
        //paramList.Add(param);
        //param = new SqlParameter("@MandalId", objEmployee.Experience);
        //paramList.Add(param);
        //param = new SqlParameter("@VillageId", objEmployee.RegistrationNumber);
        //paramList.Add(param);
        //param = new SqlParameter("@HouseNumber", objEmployee.UploadedFilePath);
        //paramList.Add(param);
        //param = new SqlParameter("@StreetName", objEmployee.IsDeleted);
        //paramList.Add(param);
        //param = new SqlParameter("@Facilities", objEmployee.FormStatus);
        //paramList.Add(param);
        //param = new SqlParameter("@CreatedUserId", objEmployee.CreatedUserId);
        //paramList.Add(param);
        public DataTable GetInstitutionDetails(int TransectionId, PCPNDTViewModel model)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetInstitutionDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }
        public DataTable GetEquipmentDetails(int TransectionId, PCPNDTViewModel model)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetEquipmentDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }
        public DataTable GetFacilitiesDetails(int TransectionId, PCPNDTViewModel model)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetFacilitiesDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 05-06-17
                return null;
            }
        }

        #region APMCE Amendments

        public int SaveRegistrationDetails(RegistrationModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicantId", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@FacilityType", model.FacilityType);
                paramList.Add(param);
                param = new SqlParameter("@HospitalTypeId", model.HospitalTypeId);
                paramList.Add(param);
                param = new SqlParameter("@ClinicType", model.ClinicType);
                paramList.Add(param);
                param = new SqlParameter("@BedStrength", model.BedStrength);
                paramList.Add(param);
                param = new SqlParameter("@Speciality", model.Speciality);
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@LicenseNumber", model.LicenseNumber);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAmendmentRegistrationDetails", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - Raj, 21-06-2017
                return -1;
            }
        }

        public int SaveCorrespondingAddressDetails(CorrespondingAddressModel model, ref int transactionId)

        {
            try
            {
                paramList = new List<SqlParameter>();
           
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveCorrespondingAddressAmendment", paramList);
                if (command != null)
                {

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 18-07-2017
                return -1;
            }
        }
        public int SaveAccommodationDetails(AccommodationModel model,
          ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
               
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentOwnedBy", model.EstablishementType);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", model.FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", model.ToDate);
                paramList.Add(param);
                param = new SqlParameter("@UploadedDoc", model.UploadedFilePath);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.UserId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveAccomodationAmendment", paramList);
                if (command != null)
                {
                 
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    return 1;
                }
                else
                {
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        
                return -1;
            }
        }

        public int SaveTrustDetails(TrustModel model,  ref int transactionId)

        {
            try
            {
                paramList = new List<SqlParameter>();
              
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@EstablishedDate", model.EstablishedDate);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveTrustDetails", paramList);
                if (command != null)
                {
                
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                 
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 13-05-2017
                return -1;
            }
        }

        public int SaveInfraStructure(List<InfraStructureModel> objList,  ref int transactionId)
           
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@InfraStructureList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveInfrastructureAmendment", paramList);
                if (command != null)
                {
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log     
                return -1;
            }
        }

        public int SaveServicesOfferedDetails(OfferedServicesModel model,  ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ExistOfferedServiceId", model.Id);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@BedStrength", model.BedStrength);
                paramList.Add(param);
                param = new SqlParameter("@OfferedServices", model.OfferedServices);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveAmendmentOfferedServices", paramList);
                if (command != null)
                {
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        
                return -1;
            }
        }

        public int SaveFacilitiesAvailable(FacilitiesAvailableModel model,
         ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@HaspediatriccareFacility", model.HasLaborRoom);
                paramList.Add(param);
                param = new SqlParameter("@HasOperationTheater", model.HasOperationTheater);
                paramList.Add(param);
                param = new SqlParameter("@HasDiagnosticsfacilities", model.HasDiagnosticFacility);
                paramList.Add(param);
                param = new SqlParameter("@HasDeclarationStamp", model.HasDeclarationStamp);
                paramList.Add(param);
                param = new SqlParameter("@JudicialStampPaperDoc", model.DeclarationStampFilePath);
                paramList.Add(param);
                param = new SqlParameter("@OtherInformationDescription", model.OtherInformationDescription);
                paramList.Add(param);
                param = new SqlParameter("@OtherDocs", model.OtherInformationDocumentPath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
             

                command = dbManager.ExecuteProcedure("SaveFacilitiAvailableAmendment", paramList);
                if (command != null)
                {
                  
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        
                return -1;
            }
        }

        public int SaveStaffDetails(List<StaffDetailsModel> objList, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@UserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@StaffDetailsList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);                

                command = dbManager.ExecuteProcedure("SaveAmendmentStaffDetails", paramList);
                if (command != null)
                {                   
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);                    
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        
                return -1;
            }
        }
        public int SaveStaffDetailsOLD(StaffDetailsModel model, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Designation", model.StaffDesignation);
                paramList.Add(param);
                param = new SqlParameter("@StaffName", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@RegistrationNo", model.RegistrationNumber);
                paramList.Add(param);
                param = new SqlParameter("@Speciality", model.Speciality);
                paramList.Add(param);
                param = new SqlParameter("@StaffDocPath", model.UploadedFilePath);
                paramList.Add(param);
                param = new SqlParameter("@PhoneNumber", Convert.ToInt32(model.PhoneNumber));
                paramList.Add(param);
                param = new SqlParameter("@EmailId", model.Email);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveStaffDetailsAmendment", paramList);
                if (command != null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        
                return -1;
            }
        }
        #endregion

        #region APMCE Renewal Details
        public DataSet GetAPMCERenewalDetails(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetAPMCERenewalCertificate", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 
                return null;
            }
        }
        #endregion

        #region PCPNDT Amendments 
        #region Facility Details Saving for PCPNDT Amendment
        public int SaveFacilityAmendment(FacilityModel facilityModel, FacilitesModel facilities, TestsModel tests,
            ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Facilities", facilityModel.Faclities);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", facilityModel.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@InvasiveTests", tests.InvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@NonInvasiveTests", tests.NonInvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@TestRemarks", tests.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@FacilityTests", facilities.Tests);
                paramList.Add(param);
                param = new SqlParameter("@FacilityStudies", facilities.Studies);
                paramList.Add(param);
                param = new SqlParameter("@FacilityRemarks", facilities.Remarks);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveFacilityAmendment", paramList);
                if (command != null)
                {

                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 15-06-2017
                return -1;
            }
        }
        public int SaveFacilityAmendmentOld(FacilityModel model, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@Facilities", model.Faclities);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@Phone", model.Phone);
                paramList.Add(param);
                param = new SqlParameter("@Email", model.Email);
                paramList.Add(param);
                param = new SqlParameter("@Fax", model.Fax);
                paramList.Add(param);
                param = new SqlParameter("@Telegrah", model.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@Telex", model.Telex);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@AddressProofDocPath", model.AddressProofPath);
                paramList.Add(param);
                param = new SqlParameter("@BuildingLayoutDocPath", model.BuildingLayoutPath);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveFacilityAmendment", paramList);
                if (command != null)
                {

                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 15-06-2017
                return -1;
            }
        }
        #endregion

        #region  Employee Details Saving for PCPNDT Amendment
        public DataTable GetEmployees(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEmployees", paramList);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public int SaveEmployeeDetails(List<EmployeeViewModel> objEmployeeList, int TransectionId, int UserId)
        {
            try
            {
                foreach (var employee in objEmployeeList)
                {
                    paramList = new List<SqlParameter>();

                    param = new SqlParameter("@TransactionId", TransectionId);
                    paramList.Add(param);
                    param = new SqlParameter("@UserId", UserId);
                    paramList.Add(param);
                    param = new SqlParameter("@Id", employee.Id);
                    paramList.Add(param);
                    param = new SqlParameter("@Name", employee.Name);
                    paramList.Add(param);
                    param = new SqlParameter("@DesignationId", employee.DesignationId);
                    paramList.Add(param);
                    param = new SqlParameter("@SubDesignation", employee.SubDesignation);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpYears", employee.ExpYears);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpMonths", employee.ExpMonths);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpDays", employee.ExpDays);
                    paramList.Add(param);
                    param = new SqlParameter("@RegistrationNumber", employee.RegistrationNumber);
                    paramList.Add(param);
                    param = new SqlParameter("@IsDeleted", employee.IsDeleted);
                    paramList.Add(param);
                    param = new SqlParameter("@UploadDocuments", System.Data.SqlDbType.Structured);
                    param.Value = Utitlities.ConvertToDataTable(employee.UploadDocuments);
                    paramList.Add(param);
                    command = dbManager.ExecuteProcedure("SaveEmployeeAmendment", paramList);
                    if (command == null)
                        return 0;
                }

                return 1;

            }
            catch (Exception ex)
            {

                return -1;
            }
        }
        #endregion

        #region   Equipment Details Saving for PCPNDT Amendment
        public int SaveEquipments(List<EquipmentModel> objEquipmentList, int TransectionId, int UserId)
        {
            try
            {

                paramList = new List<SqlParameter>();

                param = new SqlParameter("@TransactionId", TransectionId);
                paramList.Add(param);
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);

                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objEquipmentList);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveEquipmentAmendment", paramList);
                if (command != null)
                {
                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public DataTable GetEquipments(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEquipments", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 8-05-2017
                return null;
            }
        }
        #endregion

        #region  Institution Details Saving for PCPNDT Amendment

        public int SaveInstitutionDetails(InstitutionModel model, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@OwnershipTypeId", model.OwnershipTypeId);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipOthers", model.OwnershipOthers);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionTypeId", model.InstitutionTypeId);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionOthers", model.InstitutionOthers);
                paramList.Add(param);
                param = new SqlParameter("@TotalWorkArea", model.TotalWorkArea);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@AffidavitDocPath", model.AffidavitDocPath);
                paramList.Add(param);
                param = new SqlParameter("@ArticleDocPath", model.ArticleDocPath);
                paramList.Add(param);
                //param = new SqlParameter("@StudyCertificates", SqlDbType.Structured);
                //param.Value = Utitlities.ConvertToDataTable<DocumentUploadModel>(model.StudyCertificateDocPaths);
                //paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveInstitutionAmendment", paramList);
                if (command != null)
                {

                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - kishore, 19-06-2017
                return -1;
            }
        }

        #endregion

        #region  Ownership Details Saving for PCPNDT Amendment

        public int SaveOwnershipDetails(InstitutionModel model, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);

                param = new SqlParameter("@OwnershipTypeId", model.OwnershipTypeId);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionTypeId", model.InstitutionTypeId);
                paramList.Add(param);
                param = new SqlParameter("@TotalWorkArea", model.TotalWorkArea);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@AffidavitDocPath", model.AffidavitDocPath);
                paramList.Add(param);
                param = new SqlParameter("@ArticleDocPath", model.ArticleDocPath);
                paramList.Add(param);
                param = new SqlParameter("@StudyCertificates", SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable<DocumentUploadModel>(model.StudyCertificateDocPaths);
                paramList.Add(param);


                command = dbManager.ExecuteProcedure("SaveOwnershipAmendment", paramList);
                if (command != null)
                {

                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - kishore, 19-06-2017
                return -1;
            }
        }

        #endregion

        #region Tests Details Saving for PCPNDT Amendment
        public int SaveTestsAmendment(TestsModel model, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();

                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@InvasiveTests", model.InvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@NonInvasiveTests", model.NonInvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", model.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveTestsAmendment", paramList);
                if (command != null)
                {
                    return 1;
                }

                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 20-06-2017
                return -1;
            }
        }
        #endregion

        #region Facilities Details Saving for PCPNDT Amendment 
        public int SaveFacilitiesAmendment(FacilitesModel model, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);

                param = new SqlParameter("@Tests", model.Tests);
                paramList.Add(param);
                param = new SqlParameter("@Studies", model.Studies);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", model.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveFacilitiesAmendment", paramList);
                if (command != null)
                {
                    return 1;
                }

                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - kishore, 20-06-2017
                return -1;
            }
        }
        #endregion

        #region PCPNDT License Cancel  
        public DataTable GetLicenseSearch(CancelLicenseModel model)
        {
            try
            {

                paramList = new List<SqlParameter>();
                param = new SqlParameter("@LicenseNo", model.LicenseNo);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLicenseCancel", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 23-06-17
                return null;
            }
        }
        public int PCPNDTLicenseCancel(CancelLicenseModel model, int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SavePCPNDTLicenseCancellationHistory", paramList);
                if (command != null)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 28-06-17
                return -1;
            }
        }
        #endregion

        #region Rennual for PCPNDT
        public DataSet GetPCPNDTLicenseDetails(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetPCPNDTLicense", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 
                return null;
            }
        }
        public int SaveRenewal(UserModel model, int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionID", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SubmitPCPNDTRenewal", paramList);
                if (command != null)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 28-06-17
                return -1;
            }
        }
        #endregion

        #region Appeal fro PCPNDT
        public DataTable GetRemarks(AppealModel model, int transactionId)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetRemarks", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        // Following method is not using    - Raj, 07-09-17
        //public int SaveAppealForPCPNDT(ApplicationModel model)
        //{
        //    try
        //    {
        //        paramList = new List<SqlParameter>();
        //        param = new SqlParameter("@TransactionID", model.PCPNDTModel.TransactionId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@CreatedUserId", model.Id);
        //        paramList.Add(param);
        //        param = new SqlParameter("@Remarks", model.PCPNDTModel.ReasonforAppeal);
        //        paramList.Add(param);
        //        command = dbManager.ExecuteProcedure("SaveAppealForPCPNDT", paramList);
        //        if (command != null)
        //        {
        //            return 1;
        //        }
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log.       - kishore, 28-06-17
        //        return -1;
        //    }
        //}

        public int SaveAppeal(int transactionId, string remarks, int userId, string type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionID", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", userId);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", remarks);
                paramList.Add(param);
                param = new SqlParameter("@Type", type);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveAppealForPCPNDT", paramList);
                if (command != null)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 28-06-17
                return -1;
            }
        }
        #endregion

        #region Get Approved and Rejected List 

        public DataTable GetApprovedList(UserModel model)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@CurrentDesignationId", model.DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetApprovedList", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 04-07-2017
                return null;
            }
        }
        public DataTable GetRejectedList(UserModel model)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@CurrentDesignationId", model.DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetRejectedList", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 04-07-2017
                return null;
            }
        }
        #endregion

        public DataTable CheckForAmendment(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId",transactionId);
                paramList.Add(param);
               
                return dbManager.ExecuteStoredProc("CheckForAmendment", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Siva Katta, 23-01-2018
                return null;
            }
        }
        #endregion


    }
}
