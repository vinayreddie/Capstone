using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Framework;

namespace Capstone.DAL
{
    public class ApplicationDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        SqlCommand command;
        #endregion
        public ApplicationDAL()
        {
            dbManager = new SqlServerDBManager();
        }

        public DataTable GetDistrictPaymentAccountId(int districtId)
        {
            string sp = "GetDistrictPaymentAccountId";
            try
            {
                paramList = new List<SqlParameter>()
                {
                    new SqlParameter("@DistrictId", districtId)
                };

                return dbManager.ExecuteStoredProc(sp, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SavePayment(int apcmeTransId, string orderId, string paymentId, string signature, decimal amount, int userId)
        {
            var sp = "SavePayment";
            try
            {
                #region Preparing Parameters
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", SqlDbType.Int);
                param.Value = 0;
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);

                param = new SqlParameter("@APMCETransactionId", apcmeTransId);
                paramList.Add(param);

                param = new SqlParameter("@OrderId", orderId);
                paramList.Add(param);

                param = new SqlParameter("@PaymentId", paymentId);
                paramList.Add(param);

                param = new SqlParameter("@Signature", signature);
                paramList.Add(param);

                param = new SqlParameter("@Amount", amount);
                paramList.Add(param);

                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                #endregion

                command = dbManager.ExecuteProcedure(sp, paramList);
                if (command == null)
                    return -1;

                var id = Convert.ToInt32(command.Parameters["@Id"].Value);
                return id;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 2020-10-26
                return -1;
            }
        }

        public DataTable GetPaymentDetails(int tamceTransactionId)
        {
            string sp = "GetPaymentDetails";
            try
            {
                paramList = new List<SqlParameter>()
                {
                    new SqlParameter("@TAMCETransactionId", tamceTransactionId)
                };
                return dbManager.ExecuteStoredProc(sp, paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = sp;
                exception.CustomMessage = "TAMCE Transaction Id : " + tamceTransactionId;
                Logger.LogError(exception);
                return null;
            }
        }

        public int SubmitApplication(int applicationId, int userId, ref string applicationNumber) 
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SubmitApplication", paramList);
                if(command != null)
                {
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 26-05-2017
                return -1;
            }            
        }
        public DataTable GetRejectedApplications(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetRejectedApplications", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 30-06-2017
                return null;
            }
        }
        public DataTable GetSMSDetails(int ApplicationId, int transactionId, int WorkFlowType, string ApplicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", ApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@WorkFlowType", WorkFlowType);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationType", ApplicationType);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("SP_GetSMSData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - siva, 18-10-2017
                return null;
            }
        }

        public DataTable GetAppealSubmitSMSData(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetAppealSubmitSMSData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - siva, 18-11-2017
                return null;
            }
        }
        public DataSet GetAcknowledgeDetails(int applicationId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAcknowledges", paramList);
            }
            catch (Exception ex)
            { 
                // TODO: Write exception log        - Raj, 03-06-2017
                return null;
            }
        }
        // Not using    - Raj, 07-06-2017
        public DataTable DeleteandGetFiles(int id, int userId, string referenceTable)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", id);
                paramList.Add(param);
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                param = new SqlParameter("@ReferenceTable", referenceTable);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("DeleteandGetFiles", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 07-06-2017
                return null;
            }
        }

        #region AllothapicDrug
        public int SaveAllopathicDrugDetails(AllopathicDrugStoreViewModel model, int UserId, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.ADApplicantModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantId", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.ADApplicantModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipType", model.ADApplicantModel.OwnershipType);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.ADApplicantModel.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.ADApplicantModel.PAN);
                paramList.Add(param);
                param = new SqlParameter("@Mobile", model.ADApplicantModel.MobileNo);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.ADApplicantModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.ADApplicantModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.ADApplicantModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.ADApplicantModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.ADApplicantModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@UploadType", model.ADApplicantModel.FileType);
                paramList.Add(param);
                param = new SqlParameter("@UploadFile", model.ADApplicantModel.UploadDocument);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.ADApplicantModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", "Grant");
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", model.ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", FormStatus.Completed);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", 5);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyId", model.ADPharmacyModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyName", model.ADPharmacyModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentOwnedBy", model.ADPharmacyModel.OwnedBy);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", model.ADPharmacyModel.Fromdate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", model.ADPharmacyModel.ToDate);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyDistrictId", model.ADPharmacyModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyMandalId", model.ADPharmacyModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyVillageId", model.ADPharmacyModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyHouseNo", model.ADPharmacyModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyStreetName", model.ADPharmacyModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PharmacyPincode", model.ADPharmacyModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@CompetentId", model.ADCompetentPersonModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@CompetentName", model.ADCompetentPersonModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@CompetentAadhar", model.ADCompetentPersonModel.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@CompetentMobile", model.ADCompetentPersonModel.MobileNo);
                paramList.Add(param);
                param = new SqlParameter("@CompetentDistrictId", model.ADCompetentPersonModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@CompetentMandalId", model.ADCompetentPersonModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@CompetentVillageId", model.ADCompetentPersonModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@CompetentHouseNo", model.ADCompetentPersonModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@CompetentStreet", model.ADCompetentPersonModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@CompetentPin", model.ADCompetentPersonModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@CompetentDocuments", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.ADCompetentPersonModel.uploadedDocuments);
                paramList.Add(param);
                param = new SqlParameter("@Drugs", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.AllopathicDrugList);
                paramList.Add(param);
                param = new SqlParameter("@DeclarationId", model.ADDeclaration.Id);
                paramList.Add(param);
                param = new SqlParameter("@Description", model.ADDeclaration.TextArea);
                paramList.Add(param);
                param = new SqlParameter("@DeclarationDate", model.ADDeclaration.Date);
                paramList.Add(param);
                param = new SqlParameter("@Sign", model.ADDeclaration.Sign);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAllopathicDrugStoreDetails", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    // formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - siva, 01-09-2017
                return -1;
            }
        }

        public int SaveADApplicantDetails(ApplicantViewModel model, ref int applicationId, ref int transactionId,  ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", model.ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRole", model.OwnershipType);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.PAN);
                paramList.Add(param);
                param = new SqlParameter("@Mobile", model.MobileNo);
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
                param = new SqlParameter("@TransactionType", "Grant");
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UploadFile", model.UploadDocument);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAllopathicApplicant", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    //applicantId= Convert.ToInt32(command.Parameters["@Id"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public int SaveADPharmacyDetails(AllopathicPharmacyViewModel model, ref int applicationId, ref int transactionId, ref int PharmacyId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", model.ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentOwnedBy", model.OwnedBy);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", model.Fromdate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", model.ToDate);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNo", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", "Grant");
                paramList.Add(param);
                param = new SqlParameter("@PinCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAllopathicPharmacy", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    PharmacyId = Convert.ToInt32(command.Parameters["@Id"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public int SaveADCompetentDetails(ApplicantViewModel model, ref int applicationId, ref int transactionId, ref int CompetentId,
            ref FormStatus formStatus, ref string applicationNumber, int DocumentsCount)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", model.ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@Mobile", model.MobileNo);
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
                param = new SqlParameter("@TransactionType", "Grant");
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@CompetentDocuments", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.uploadedDocuments);
                paramList.Add(param);
                param = new SqlParameter("@DocumentsCount", DocumentsCount);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveAllopathicCompetent", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    CompetentId = Convert.ToInt32(command.Parameters["@Id"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public int SaveADDrugDetails(List<AllopathicDrugName> DrugsList, ref int applicationId, ref int transactionId, int ServiceId, int UserId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@Drugs", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(DrugsList);
                paramList.Add(param);
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", "Grant");
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveAllopathicDrugsList", paramList);
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

                return -1;
            }

        }

        public int SaveADDeclaration(AllopathicDeclaration model, ref int applicationId, ref int transactionId, 
            ref int DeclarationId, int ServiceId, ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                paramList.Add(param);
                param = new SqlParameter("@Description", model.TextArea);
                paramList.Add(param);
                param = new SqlParameter("@DeclarationDate", model.Date);
                paramList.Add(param);
                param = new SqlParameter("@Sign", model.Sign);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", "Grant");
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveAllopathicDeclaration", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    DeclarationId= Convert.ToInt32(command.Parameters["@Id"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);  //
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public DataSet GetAllopathicDetails(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@transactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAllopathicDetails", paramList);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        public DataSet GetAllTAMCEformsCertificateDetails(int transactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAllTAMCEformsCertificateDetails", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 03-06-2017
                return null;
            }
        }

        public DataSet GetTemparoryCertificateDetails(int transactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetTemparoryCertificateDetails", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetRejectCertificateDetails(int transactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetRejectCertificateDetails", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAcknowledgeCertificateDetails(int transactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAcknowledgeCertificateDetails", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
