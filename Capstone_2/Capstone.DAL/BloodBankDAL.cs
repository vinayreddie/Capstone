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
   public class BloodBankDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlCommand command;
        SqlParameter param;
        List<SqlParameter> paramList;
        #endregion
        public BloodBankDAL()
        {
            dbManager = new SqlServerDBManager();
        }

        public int SaveBloodBankApplicantDetails(BloodBankApplicantModel model, ref int applicationId, ref int transactionId,
    ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 31); // BloodBank Form 27C
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipType", model.OwnershipType);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.PAN);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UploadedFile", model.UploadDocument);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBloodBankApplicantDetails", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - Jai, 12-08-2017
                return -1;
            }
        }

        public int SaveBloodBankEstablishment(BloodBankEstablishmentModel model, ref int applicationId,
    ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 31); // BloodBank Form 27C
                paramList.Add(param);
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
                param = new SqlParameter("@UploadedFilePath", model.AddressProofPath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBloodBankEstablishmentDetails", paramList);
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

        public int SaveBloodBankEquipments(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
    ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 31); // Blood bank Grant Service Id : 31
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertEquipmentBulk", paramList);
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
                // TODO: Write exception log        - Raj, 16-05-2017
                return -1;
            }
        }

        #region Bloodbank Saving Listof items 
        public int SaveListofItems(List<BloodBankListModel> objList, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 31); // BloodBank Form 27C
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@ListItems", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertBloodBankListItems", paramList);
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
                // TODO: Write exception log        --kishore 12-08-17
                return -1;
            }
        }

        public DataTable GetBloodBankListItems(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetBloodBankListItems", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }
        #endregion

        #region BloodBank Saving Equipment Details
        public int SaveEquipment(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
     ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                //param = new SqlParameter("@Id", model.Id);
                //param.Direction = System.Data.ParameterDirection.InputOutput;
                //paramList.Add(param);
                param = new SqlParameter("@ServiceId", 31); // BloodBank Form 27C
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertEquipmentBulk", paramList);
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
                // TODO: Write exception log        --Jai  21-08-17
                return -1;
            }
        }

        public DataTable GetBloodBankEquipments(int transactionId)
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
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }
        #endregion

        public int SaveBloodBankEmployee(List<EmployeeViewModel> objList, ref int applicationId, ref int transactionId,
     ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            try
            {
                // Set FormStatus
                if (objList.Count(item => item.IsDeleted == false) > 0)
                    formStatus = FormStatus.Completed;

                int result = 0;

                foreach (var employee in objList)
                {
                    paramList = new List<SqlParameter>();
                    param = new SqlParameter("@Id", employee.Id);
                    param.Direction = System.Data.ParameterDirection.InputOutput;
                    paramList.Add(param);
                    param = new SqlParameter("@ApplicationId", applicationId);
                    param.Direction = System.Data.ParameterDirection.InputOutput;
                    paramList.Add(param);
                    param = new SqlParameter("@TransactionId", transactionId);
                    param.Direction = System.Data.ParameterDirection.InputOutput;
                    paramList.Add(param);
                    param = new SqlParameter("@Name", employee.Name);
                    paramList.Add(param);
                    param = new SqlParameter("@QualificationId", employee.QualificationId);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpYears", employee.ExpYears);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpMonths", employee.ExpMonths);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpDays", employee.ExpDays);
                    paramList.Add(param);
                    param = new SqlParameter("@IsDeleted", employee.IsDeleted);
                    paramList.Add(param);
                    param = new SqlParameter("@UserId", employee.CreatedUserId);
                    paramList.Add(param);
                    param = new SqlParameter("@DocumentsList", System.Data.SqlDbType.Structured);
                    param.Value = Utitlities.ConvertToDataTable(employee.UploadDocuments);
                    paramList.Add(param);
                    param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                    param.Direction = ParameterDirection.Output;
                    paramList.Add(param);
                    param = new SqlParameter("@TransactionType", applicationType);
                    paramList.Add(param);

                    command = dbManager.ExecuteProcedure("SaveUpdateBloodBankEmployee", paramList);
                    if (command != null)
                    {
                        applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                        transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                        applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                        
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        --kishore 13-08-17
                return -1;
            }
        }

        public DataSet GetBloodBankEmployees(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetBloodBankEmployees", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }

        #region BloodBank Saving Declaration Details
        public int SaveDeclarationDetails(BloodBankAttachments model, ref int applicationId, ref int transactionId,
   ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 31); // BloodBank Form 27C
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);

                param = new SqlParameter("@InspectionDate", model.InspectionDate);
                paramList.Add(param);
                param = new SqlParameter("@DeclarationDate", model.DeclareDate);
                paramList.Add(param);
                param = new SqlParameter("@Place", model.Place);
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@Designation", model.Designation);
                paramList.Add(param);
                param = new SqlParameter("@PremisesPlaDocPath", model.planPremisesPath);
                paramList.Add(param);
                param = new SqlParameter("@PremisesOwnershipDocPath", model.OwnerPremisesPath);
                paramList.Add(param);
                param = new SqlParameter("@IdProofDocPath", model.IdProffPath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBloodBankDeclarationDetails", paramList);
                if (command != null)
                {
                    
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        --kishore 13-08-17
                return -1;
            }
        }
        #endregion


        #region Blood Bank Form 27E
        public int SaveBloodBankApplicantForm27E(BloodBankApplicantModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 32); // BloodBank Form 27 E
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipType", model.OwnershipType);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.PAN);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UploadedFile", model.UploadDocument);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBloodBankApplicantDetails", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - Jai, 12-08-2017
                return -1;
            }
        }

        public int SaveBloodBankEstablishmentForm27E(BloodBankEstablishmentModel model, ref int applicationId,
    ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 32); // BloodBank Form 27E
                paramList.Add(param);
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
                param = new SqlParameter("@UploadedFilePath", model.AddressProofPath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBloodBankEstablishmentDetails", paramList);
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
        #region Bloodbank Saving Listof items 
        public int SaveListofItemsForm27E(List<BloodBankListModel> objList, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 32); // BloodBank Form 27E
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@ListItems", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertBloodBankListItems", paramList);
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
                // TODO: Write exception log        --kishore 12-08-17
                return -1;
            }
        }

        public DataTable GetBloodBankListItemsForm27E(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetBloodBankListItems", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }
        #endregion
        #region Technical Staff
        public int SaveBloodBankTechnicalDetails(List<TechnicalModel> objList, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            try
            {
                // Set FormStatus
                if (objList.Count(item => item.IsDeleted == false) > 0)
                    formStatus = FormStatus.Completed;

                int result = 0;

                foreach (var technical in objList)
                {
                    paramList = new List<SqlParameter>();
                    param = new SqlParameter("@Id", technical.Id);
                    param.Direction = System.Data.ParameterDirection.InputOutput;
                    paramList.Add(param);
                    param = new SqlParameter("@ApplicationId", applicationId);
                    param.Direction = System.Data.ParameterDirection.InputOutput;
                    paramList.Add(param);
                    param = new SqlParameter("@TransactionId", transactionId);
                    param.Direction = System.Data.ParameterDirection.InputOutput;
                    paramList.Add(param);
                    param = new SqlParameter("@Name", technical.Name);
                    paramList.Add(param);
                    param = new SqlParameter("@Responsibility", technical.Responsibility);
                    paramList.Add(param);
                    param = new SqlParameter("@Qualification", technical.Qualification);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpYears", technical.ExpYears);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpMonths", technical.ExpMonths);
                    paramList.Add(param);
                    param = new SqlParameter("@ExpDays", technical.ExpDays);
                    paramList.Add(param);
                    param = new SqlParameter("@IsDeleted", technical.IsDeleted);
                    paramList.Add(param);
                    param = new SqlParameter("@UserId", technical.CreatedUserId);
                    paramList.Add(param);
                    param = new SqlParameter("@DocumentsList", System.Data.SqlDbType.Structured);
                    param.Value = Utitlities.ConvertToDataTable(technical.UploadDocuments);
                    paramList.Add(param);
                    param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                    param.Direction = ParameterDirection.Output;
                    paramList.Add(param);
                    param = new SqlParameter("@TransactionType", applicationType);
                    paramList.Add(param);

                    command = dbManager.ExecuteProcedure("SaveUpdateBloodBankTechnicalStaff", paramList);
                    if (command != null)
                    {
                        applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                        transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                        applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();

                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        --Jai  19-08-17
                return -1;
            }
        }

        public DataSet GetBloodBankTechnicalStaff(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetBloodBankTechnicalStaff", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 21-08-2017
                return null;
            }
        }
        #endregion
        #region BloodBank Saving Equipment Details
        public int SaveEquipmentForm27E(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
     ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                //param = new SqlParameter("@Id", model.Id);
                //param.Direction = System.Data.ParameterDirection.InputOutput;
                //paramList.Add(param);
                param = new SqlParameter("@ServiceId", 32); // BloodBank Form 27E
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", applicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertEquipmentBulk", paramList);
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
                // TODO: Write exception log        --Jai  21-08-17
                return -1;
            }
        }

        public DataTable GetBloodBankEquipmentForm27E(int transactionId)
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
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }
        #endregion
        #region BloodBank Saving Declaration Details
        public int SaveDeclarationForm27E(BloodBankAttachments model, ref int applicationId, ref int transactionId,
   ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 32); // BloodBank Form 27E
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);

                param = new SqlParameter("@InspectionDate", model.InspectionDate);
                paramList.Add(param);
                param = new SqlParameter("@DeclarationDate", model.DeclareDate);
                paramList.Add(param);
                param = new SqlParameter("@Place", model.Place);
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@Designation", model.Designation);
                paramList.Add(param);
                param = new SqlParameter("@PremisesPlaDocPath", model.planPremisesPath);
                paramList.Add(param);
                param = new SqlParameter("@PremisesOwnershipDocPath", model.OwnerPremisesPath);
                paramList.Add(param);
                param = new SqlParameter("@IdProofDocPath", model.IdProffPath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBloodBankDeclarationDetails", paramList);
                if (command != null)
                {

                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        --kishore 13-08-17
                return -1;
            }
        }
        #endregion

        #endregion



    }
}
