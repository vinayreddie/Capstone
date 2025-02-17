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
    public class APMCEDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlCommand command;
        SqlParameter param;
        List<SqlParameter> paramList;
        #endregion
        public APMCEDAL()
        {
            dbManager = new SqlServerDBManager();
        }

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
                param = new SqlParameter("@BuildingHeight", model.BuildingHeight);
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

                command = dbManager.ExecuteProcedure("SaveRegistrationDetails", paramList);
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
        public int SaveCorrespondingAddressDetails(CorrespondingAddressModel model, ref int applicationId, 
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
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
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveCorrespondingAddress", paramList);
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
                // TODO: Write exception log        - Raj, 13-05-2017
                return -1;
            }
        }
        public int SaveTrustDetails(TrustModel model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
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
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveTrustDetails", paramList);
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
                // TODO: Write exception log.       - Raj, 13-05-2017
                return -1;
            }
        }
        public int SaveAccommodationDetails(AccommodationModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentOwnedBy", model.EstablishementType);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", model.FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", model.ToDate);
                paramList.Add(param);
                param = new SqlParameter("@UploadedDoc", model.UploadedFilePath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAccommodationDetails", paramList);
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
                // TODO: Write exception log        - Raj, 22-06-2017
                return -1;
            }
        }
        public int SaveInfraStructure(List<InfraStructureModel> objList, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType, int existingApplicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", existingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@InfraStructureList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertInfraStructureBulk", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    // Set FormStatus
                    if (objList.Count(item => item.IsDeleted == false) > 0)
                        formStatus = FormStatus.Completed;
                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log      - Raj, 01-06-2017
                return -1;
            }
        }

        public int SaveStaffDetails(List<StaffDetailsModel> objList, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType, int existingApplicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                //param = new SqlParameter("@Id", model.Id);
                //param.Direction = System.Data.ParameterDirection.InputOutput;
                //paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", existingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@StaffDetailsList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertStaffDetailsBulk", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    // Set FormStatus
                    if (objList.Count(item => item.IsDeleted == false) > 0)
                        formStatus = FormStatus.Completed;
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
        public DataTable GetInfraStructures(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetInfraStructures", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Wirte exception log        - Raj, 01-06-2017
                return null;
            }
        }

        public int SaveEstablishmentDetails(EstablishmentModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentDate", model.EstablishmentDate);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@OpenArea", model.OpenArea);
                paramList.Add(param);
                param = new SqlParameter("@ConstructionArea", model.ConstructionArea);
                paramList.Add(param);
                param = new SqlParameter("@ConstructionAreaFilePath", model.ConstructionAreaFilePath);
                paramList.Add(param);
                param = new SqlParameter("@OpenAreaFilePath", model.OpenAreaFilePath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveEstablishmentDetails", paramList);
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
                // TODO: Write exception log        
                return -1;
            }
        }

        public int SaveServicesOfferedDetails(OfferedServicesModel model, ref int applicationId,ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
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
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
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
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveOfferedServicesDetails", paramList);
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
                // TODO: Write exception log        
                return -1;
            }
        }
        
        //public int SaveStaffDetails(StaffDetailsModel model, ref int applicationId, ref int transactionId, 
        //    ref FormStatus formStatus, ref string applicationNumber, string applicationType)
        //{
        //    try
        //    {
        //        paramList = new List<SqlParameter>();
        //        param = new SqlParameter("@Id", model.Id);
        //        param.Direction = System.Data.ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@ApplicationId", applicationId);
        //        param.Direction = System.Data.ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@TransactionId", transactionId);
        //        param.Direction = System.Data.ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@Designation", model.StaffDesignation);
        //        paramList.Add(param);
        //        param = new SqlParameter("@StaffName", model.Name);
        //        paramList.Add(param);
        //        param = new SqlParameter("@RegistrationNo", model.RegistrationNumber);
        //        paramList.Add(param);
        //        //param = new SqlParameter("@SpecialtyId", model.SpecialtyId);
        //        //paramList.Add(param);
        //        param = new SqlParameter("@Speciality", model.Speciality);
        //        paramList.Add(param);
        //        param = new SqlParameter("@StaffDocPath", model.UploadedFilePath);
        //        paramList.Add(param);
        //        param = new SqlParameter("@PhoneNumber", Convert.ToInt32(model.PhoneNumber));
        //        paramList.Add(param);
        //        param = new SqlParameter("@EmailId", model.Email);
        //        paramList.Add(param);
        //        param = new SqlParameter("@UserId", model.CreatedUserId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@FormStatus", 0);
        //        param.Direction = ParameterDirection.Output;
        //        paramList.Add(param);
        //        param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
        //        param.Direction = ParameterDirection.Output;
        //        paramList.Add(param);

        //        command = dbManager.ExecuteProcedure("SaveStaffDetails", paramList);
        //        if (command != null)
        //        {
        //            applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
        //            transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
        //            formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
        //            applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
        //            return Convert.ToInt32(command.Parameters["@Id"].Value);
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        
        //        return -1;
        //    }
        //}

        public DataTable GetStaffDetails(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetStaffDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Wirte exception log        - Raj, 01-06-2017
                return null;
            }
        }

        public int SaveFacilitiesAvailable(FacilitiesAvailableModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@HaspediatriccareFacility", model.HasLaborRoom);
                paramList.Add(param);
                param = new SqlParameter("@HasOperationTheater", model.HasOperationTheater);
                paramList.Add(param);
                param = new SqlParameter("@OperationTheaterCount", model.OperationTheatreCount);
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
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveFacilitiesAvailable", paramList);
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
                // TODO: Write exception log        
                return -1;
            }
        }

        public int SaveAdditionalDocuments(AdditionalDocumentsModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@BioCapstoneWastageClearanceFilePath", model.BioCapstoneWastageClearanceFromFilePath);
                paramList.Add(param);
                param = new SqlParameter("@BioCapstoneValidupto", model.BioCapstoneValidupto);
                paramList.Add(param);
                param = new SqlParameter("@PollutionAuthorityFilePath", model.PollutionAuthorityLetterByPCBFilePath);
                paramList.Add(param);
                param = new SqlParameter("@PollutionAuthorityValidupto", model.PollutionAuthorityValidupto);
                paramList.Add(param);
                param = new SqlParameter("@CFOFilePath", model.CFOFilePath);
                paramList.Add(param);
                param = new SqlParameter("@STPFilePath", model.STPFilePath);
                paramList.Add(param);
                param = new SqlParameter("@FEReportFilePath", model.FEReportFilePath);
                paramList.Add(param);
                param = new SqlParameter("@FireNOCValidupto", model.FireNOCValidupto);
                paramList.Add(param);
                param = new SqlParameter("@TarifListFilePath", model.TarifListFilePath);
                paramList.Add(param);
                param = new SqlParameter("@BuildingPlanFilepath", model.Establishment_BuildingPlanFilepath);
                paramList.Add(param);
                param = new SqlParameter("@HospitalNamePlateFilePath", model.HospitalOutSideNamePlateBuildingFilePath);
                paramList.Add(param);
                param = new SqlParameter("@TariffBoardFilePath", model.TariffBoardFilePath);
                paramList.Add(param);
                param = new SqlParameter("@FireExhaustiveFilePath", model.FireExhaustiveFilePath);
                paramList.Add(param);
                param = new SqlParameter("@HospitalLabOperationTheaterFilePath", model.HospitalLabOperationTheaterFilePath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAdditionalDocuments", paramList);
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
                // TODO: Write exception log        
                return -1;
            }
        }

        public int SaveExistingLicenseDetails(ExistingLicense model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name.ToString());
                paramList.Add(param);
                param = new SqlParameter("@SubmittedOn", model.SubmittedOn);
                paramList.Add(param);
                param = new SqlParameter("@LicenseNumber", model.ExistingLicenseNumber);
                paramList.Add(param);
                param = new SqlParameter("@ExistingLicenseNumber", model.ExistingLicenseNumber);
                paramList.Add(param);
                param = new SqlParameter("@LicenseIssueDate", model.LicenseIssueDate);
                paramList.Add(param);
                param = new SqlParameter("@LicenseExpiryDate", model.LicenseExpiryDate);
                paramList.Add(param);
                param = new SqlParameter("@CurrentDesignationId", model.CurrentDesignationId);
                paramList.Add(param);
                param = new SqlParameter("@MobileNo", model.MobileNo);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.PAN);
                paramList.Add(param);
                param = new SqlParameter("@Email", model.Email);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictID);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalID);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageID);
                paramList.Add(param);
                param = new SqlParameter("@HouseNo", model.HouseNo);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@Pincode", model.Pincode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", model.ApplicationNumber);
                //param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveExistingLicenseDetails", paramList);
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
                //TODO: Write exception log 
                return -1;
            }
        }

        public bool IsExistingLicenseNumberExists(string existingLicenseNumber,string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ExistingLicenseNumber", existingLicenseNumber);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", applicationNumber);
                paramList.Add(param);
                param = new SqlParameter("@Result", false);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("IsExistingLicenseNumberExists", paramList);
                if (cmd != null)
                {
                    return Convert.ToBoolean(cmd.Parameters["@Result"].Value);
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "IsExistingLicenseNumberExists";
                Logger.LogError(exception);
                return false;
            }
        }
        public DataTable GetExistingLicensesIndex(int UserId)
        {
            var sp = "GetExistingLicensesIndex";
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                DataTable dt = dbManager.ExecuteStoredProc(sp, paramList);
                if (dt != null)
                {
                    return dt;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = sp;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataTable BindExistingLicenseTrans(int TransId)
        {
            var sp = "BindExistingLicenseTrans";
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransId", TransId);
                paramList.Add(param);
                DataTable dt = dbManager.ExecuteStoredProc(sp, paramList);
                if (dt != null)
                {
                    return dt;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = sp;
                Logger.LogError(exception);
                return null;
            }
        }
        public int DeleteExistingLicense(int Id, int UserId)
        {
            int result = 0;
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", Id);
                paramList.Add(param);
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);

                SqlCommand cmd = dbManager.ExecuteProcedure("DeleteExistingLicense", paramList);
                if (cmd != null)
                {
                    result = 1;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "DeleteExistingLicense";
                Logger.LogError(exception);
                return -1;
            }
        }
    }
}
