using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Capstone.DAL
{
    public class LicenseDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        SqlCommand command;
        List<SqlParameter> paramList;
        static System.Net.HttpWebRequest request;
        static System.IO.Stream dataStream;
        #endregion
        public LicenseDAL()
        {
            dbManager = new SqlServerDBManager();
        }

        public DataTable GetApplicationList(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetApplicationList", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetApplicationList";
                exception.CustomMessage = "user Id : " + userId;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataTable GetApplicantDetailsForPayment(int applicationId)
        {
            try
            {
                paramList = new List<SqlParameter>()
                {
                    new SqlParameter("@ApplicationId", applicationId)
                };
                return dbManager.ExecuteStoredProc("GetApplicantDetailsForPayment", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTransactions(int transactionId, Status status, string TransactionType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@StatusId", Convert.ToInt32(status));
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", TransactionType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetTransactions", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetTransactions";
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Status Id: " + Convert.ToInt32(status);
                Logger.LogError(exception);
                return null;
            }
        }



        public DataSet GetAPMCEData(int transactionId, string type)
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
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetAPMCEData";
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Type :" + type;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataSet GetPCPNDTData(int transactionId, string type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@Type", type);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAPMCEData", paramList); //GetPCPNDTData
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetPCPNDTData";
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Type :" + type;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataTable GetOtherUploads(int TransactionId, int UserId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetOtherUploads", paramList);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public DataSet GetBloodBankData(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetBloodBankData", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetBloodBankData";
                exception.CustomMessage = "Transaction Id : " + transactionId;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataSet GetForm27EBloodBankData(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetBloodBankForm27EData", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetBloodBankForm27EData";
                exception.CustomMessage = "Transaction Id : " + transactionId;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataSet GetHomeopathyData(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetHomeopathyData", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetHomeopathyData";
                exception.CustomMessage = "Transaction Id : " + transactionId;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataSet GetOrganTransplantationData(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetOrganTransplantationData", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetOrganTransplantationData";
                exception.CustomMessage = "Transaction Id : " + transactionId;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataSet GetQueryResponseData(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetQueryResponseData", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetQueryResponseData";
                exception.CustomMessage = "user Id : " + userId;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataTable GetRaisedQueryData(int id)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", id);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetRaisedQueryData", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetRaisedQueryData";
                exception.CustomMessage = "Id : " + id;
                Logger.LogError(exception);
                return null;
            }
        }
        public bool SubmitResponse(QueryModel model)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Description", model.Description);
                paramList.Add(param);
                param = new SqlParameter("@UploadedDoctPath", model.UploadedFilePath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@QueryId", model.QueryId);
                paramList.Add(param);
                param = new SqlParameter("@Type", model.Type);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", model.TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@Applicationtype", model.ApplicationType);
                paramList.Add(param);
                dbManager.ExecuteProcedure("SubmitQueryReponse", paramList);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SubmitQueryReponse";
                exception.ParentParamField = model;
                Logger.LogError(exception);
                return false;
            }
        }
        public DataSet GetPCPNDTLicenseDetails(int transactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Tid", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@Type", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetPCPNDTLicense", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetPCPNDTLicense";
                exception.CustomMessage = "Trasaction Id : " + transactionId + " ,Table Name :" + TableName;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataSet GetAPMCERejectionApplications(int ApplicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("ApplicationId", ApplicationId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAPMCERejection", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetAPMCERejection";
                exception.CustomMessage = "Application Id : " + ApplicationId;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataSet GetPCPNDTRejectionDetails(int ApplicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", ApplicationId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetPCPNDTRejection", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetPCPNDTRejection";
                exception.CustomMessage = "Application Id : " + ApplicationId;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataSet GetRejectedServices(int transactionId, string TransactionType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", TransactionType);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetRejectedServices", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetRejectedServices";
                exception.CustomMessage = "Transaction Id : " + transactionId + "Transaction Table: " + TransactionType;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataTable GetTAMCEFeeDetails(int applicationId)
        {
            var sp = "GetTAMCEFeeDetails";
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc(sp, paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = sp;
                exception.CustomMessage = "Application Id : " + applicationId;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataSet GetServicesByApplicationID(int applicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                paramList.Add(param);

                return dbManager.ExecuteSPMultipleResultSet("GetServicesByApplicationId", paramList); // GetServicesByApplicationId
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetServicesByApplicationId";
                exception.CustomMessage = "Application Id : " + applicationId;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataSet GetAPMCECertificateDetails(int TransactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Tid", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@Type", TableName);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAPMCECertificate", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetAPMCECertificate";
                exception.CustomMessage = "Transaction Id : " + TransactionId + " ,Table Name :" + TableName;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataTable GetBloodBankNOC(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetBloodBankNOC", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetBloodBankNOC";
                exception.CustomMessage = "Transaction Id : " + TransactionId;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataTable GetLicenseType(int TransactionId, string Type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", Type);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetServiceType", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetServiceType";
                exception.CustomMessage = "Transaction Id : " + TransactionId + " ,Type :" + Type;
                Logger.LogError(exception);
                return null;
            }
        }
        public DataTable GetLicenseTypeForLicense(int TransactionId, string TableName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@TableName", TableName);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetServiceType", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetServiceType";
                exception.CustomMessage = "Transaction Id : " + TransactionId + " ,Table Name :" + TableName;
                Logger.LogError(exception);
                return null;
            }
        }
        #region Bio Capstone Saving
        public int SaveBioCapstoneApplicantDetails(BioCapstoneViewModel model, ref int applicationId, ref int transactionId, ref string applicationNumber)
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
                param = new SqlParameter("@Id", model.BioCapstoneApplicantModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@Name", model.BioCapstoneApplicantModel.Name);
                paramList.Add(param);
                //param = new SqlParameter("@ApplicantRole", model.ApplicantRole);
                //paramList.Add(param);
                //param = new SqlParameter("@ApplicantRoleOther", model.ApplicantRoleOther);
                //paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.BioCapstoneApplicantModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.BioCapstoneApplicantModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.BioCapstoneApplicantModel.VillageId);
                paramList.Add(param);
                //param = new SqlParameter("@Aadhar", model.BioCapstoneApplicantModel.Aadhar);
                //paramList.Add(param);
                //param = new SqlParameter("@PAN", model.BioCapstoneApplicantModel.PAN);
                //paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.BioCapstoneApplicantModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.BioCapstoneApplicantModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.BioCapstoneApplicantModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                //param = new SqlParameter("@FormStatus", 0);
                //param.Direction = ParameterDirection.Output;
                //paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstoneTrasaction", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    //formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return 1;  //Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveBioCapstoneTrasaction";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Application Id :" + applicationId;
                Logger.LogError(exception);
                return -1;
            }
        }
        public DataSet GetBioCapstoneDetails(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@transactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetBioCapstone", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetBioCapstone";
                exception.CustomMessage = "Transaction Id : " + transactionId;
                Logger.LogError(exception);
                return null;
            }
        }
        #endregion

        #region Organ Transplantation
        public bool SaveOrganTransplantation(OrganTransplantViewModel model, ref int applicationId, ref int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.HospitalModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@Name", model.HospitalModel.HospitalName);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.HospitalModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.HospitalModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.HospitalModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.HospitalModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@HouseNo", model.HospitalModel.HouseNo);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.HospitalModel.Pincode);
                paramList.Add(param);
                param = new SqlParameter("@PhoneNo", model.HospitalModel.PhoneNo);
                paramList.Add(param);
                param = new SqlParameter("@TeachingType", model.HospitalModel.TeacherNonTeach);
                paramList.Add(param);
                param = new SqlParameter("@Government", model.HospitalModel.Govtorpvt);
                paramList.Add(param);
                param = new SqlParameter("@Annualbudjet", model.HospitalModel.AnnualBudjet);
                paramList.Add(param);
                param = new SqlParameter("@BedStrength", model.HospitalModel.TotalBedStrength);
                paramList.Add(param);
                param = new SqlParameter("@DisciplinesName", model.HospitalModel.Nameofdisciplines);
                paramList.Add(param);
                param = new SqlParameter("@PatientTurnOver", model.HospitalModel.PatientTurnoverPerYear);
                paramList.Add(param);
                param = new SqlParameter("@Road", model.HospitalModel.ByRoad);
                paramList.Add(param);
                param = new SqlParameter("@Rail", model.HospitalModel.ByRail);
                paramList.Add(param);
                param = new SqlParameter("@Air", model.HospitalModel.ByAir);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserID", model.CreatedUserID);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveOTHospital", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveOTHospital";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id: " + transactionId + " ,Application Id :" + applicationId;
                Logger.LogError(exception);
                return false;
            }
        }
        #endregion

        #region Homeopathy
        public int SaveHomeopathyDetails(HomeopathyDrugStoreViewModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 35); // Homeopathy ServiceId=35
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.HDApplicantModel.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                //Applicant
                param = new SqlParameter("@Id", model.HDApplicantModel.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.HDApplicantModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipType", model.HDApplicantModel.OwnershipType);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.HDApplicantModel.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.HDApplicantModel.PAN);
                paramList.Add(param);
                param = new SqlParameter("@Mobile", model.HDApplicantModel.MobileNo);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.HDApplicantModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.HDApplicantModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.HDApplicantModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HDApplicantModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.HDApplicantModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.HDApplicantModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UploadedFile", model.HDApplicantModel.UploadDocument);
                paramList.Add(param);
                //Establishmnet
                param = new SqlParameter("@EstablishmentId", model.HDEstablishment.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentName", model.HDEstablishment.Name);
                paramList.Add(param);
                param = new SqlParameter("@OwnedBy", model.HDEstablishment.OwnedBy);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", model.HDEstablishment.Fromdate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", model.HDEstablishment.ToDate);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentDistrictId", model.HDEstablishment.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentMandalId", model.HDEstablishment.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentVillageId", model.HDEstablishment.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentHouseNumber", model.HDEstablishment.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentStreetName", model.HDEstablishment.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@EstablishmentPINCode", model.HDEstablishment.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@RentDeedDocPath", model.HDEstablishment.RentalDocument);
                paramList.Add(param);
                param = new SqlParameter("@PremisesDocPath", model.HDEstablishment.PlanPremisesDocument);
                paramList.Add(param);
                param = new SqlParameter("@AdressProofDocPath", model.HDEstablishment.AddressProff);
                paramList.Add(param);
                // Competent Person Incharge Details
                param = new SqlParameter("@CompetentId", model.HDCompetentModel.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CompetentName", model.HDCompetentModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@CompetentAadharId", model.HDCompetentModel.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@MobileNo", model.HDCompetentModel.MobileNo);
                paramList.Add(param);
                param = new SqlParameter("@CompetentDistrictId", model.HDCompetentModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@CompetentMandalId", model.HDCompetentModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@CompetentVillageId", model.HDCompetentModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@CompetentHouseNumber", model.HDCompetentModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@CompetentStreetName", model.HDCompetentModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@CompetentPINCode", model.HDCompetentModel.PINCode);
                paramList.Add(param);
                // Declaration
                param = new SqlParameter("@DeclarationId", model.HDDeclaration.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CoveringDocPath", model.HDDeclaration.CoveringLetter);
                paramList.Add(param);
                param = new SqlParameter("@Date", model.HDDeclaration.Date);
                paramList.Add(param);
                param = new SqlParameter("@Signature", model.HDDeclaration.Signature);
                paramList.Add(param);

                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveHomeopathy", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    //formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveHomeopathy";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Application Id :" + applicationId;
                Logger.LogError(exception);
                return -1;
            }
        }

        public int SaveHomeopathyApplicant(ApplicantViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 35); // Homeopathy
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

                command = dbManager.ExecuteProcedure("SaveHomeopathyApplicant", paramList);
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
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveHomeopathyApplicant";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Application Id :" + applicationId;
                Logger.LogError(exception);
                return -1;
            }
        }

        public int SaveHomeopathyEstablishment(HomeopathyEstablishmentViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 35); // Homeopathy
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
                param = new SqlParameter("@OwnedBy", model.OwnedBy);
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
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@RentDeedDocPath", model.RentalDocument);
                paramList.Add(param);
                param = new SqlParameter("@PremisesDocPath", model.PlanPremisesDocument);
                paramList.Add(param);
                param = new SqlParameter("@AdressProofDocPath", model.AddressProff);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveHomeopathyEstablishment", paramList);
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
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveHomeopathyEstablishment";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id: " + transactionId + "Application Id :" + applicationId;
                Logger.LogError(exception);
                return -1;
            }
        }

        public int SaveHomeopathyCompetent(ApplicantViewModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 35); // Homeopathy
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
                param = new SqlParameter("@AadharId", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@MobileNo", model.MobileNo);
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

                command = dbManager.ExecuteProcedure("SaveHomeopathyCompetent", paramList);
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
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveHomeopathyCompetent";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id : " + transactionId + " ,Application Id :" + applicationId;
                Logger.LogError(exception);
                return -1;
            }
        }

        public int SaveHomeopathyDeclaration(HomeopathyDeclaration model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 35); // Homeopathy
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
                param = new SqlParameter("@CoveringDocPath", model.CoveringLetter);
                paramList.Add(param);
                param = new SqlParameter("@Date", model.Date);
                paramList.Add(param);
                param = new SqlParameter("@Signature", model.Signature);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveHomeopathyDeclaration", paramList);
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
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveHomeopathyDeclaration";
                exception.ParentParamField = model;
                exception.CustomMessage = "Transaction Id :" + transactionId + ", Application Id :" + applicationId;
                Logger.LogError(exception);
                return -1;
            }
        }
        #endregion

        #region NOC for Equipment amendment
        public DataTable GetEquipmentList(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEquipmentbyTransactionId", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetEquipmentbyTransactionId";
                exception.CustomMessage = "Transaction Id : " + TransactionId;
                Logger.LogError(exception);
                return null;
            }
        }

        public bool SaveEquipmentNOC(NOCforquipmentModel objModel)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", objModel.TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentId", objModel.EquipmentId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", objModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@OtherState", objModel.OtherState);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", objModel.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@UserId", objModel.UserId);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("SaveNOCforEquipementAmendment", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetEquipmentbyTransactionId";
                exception.ParentParamField = objModel;
                Logger.LogError(exception);
                return false;
            }
        }
        public DataTable GetAmmendments(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAmmendments", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - siva katta, 07-02-2018
                return null;
            }
        }
        public DataTable GetNOCCertificateData(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetNOCCertificateData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - siva katta, 07-02-2018
                return null;
            }
        }
        #endregion

        public bool UpdateUserIdForExistingData(int UserId, int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("UpdateUserIdForExistingData", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "UpdateUserIdForExistingData";

                Logger.LogError(exception);
                return false;
            }
        }

        public bool AppealApproval(ApprovalsModel approval)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", approval.UserId);
                paramList.Add(param);
                param = new SqlParameter("@AmendmentId", approval.TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", approval.Remarks);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("ApproveAppeal", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "ApproveAppeal";

                Logger.LogError(exception);
                return false;
            }
        }

        public int CheckIsCertificateSavedInFolder(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@CertificateResult", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("CheckIsCertificateSavedInFolder", paramList);
                if (cmd != null)
                {
                    int res1 = Convert.ToInt32(cmd.Parameters["@CertificateResult"].Value);
                    return res1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "UpdateUserIdForExistingData";

                Logger.LogError(exception);
                return 0;
            }
        }
        public bool SaveUpdateCertificatePath(int TransactionId, string filePath, int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@FilePath", filePath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);

                SqlCommand cmd = dbManager.ExecuteProcedure("SaveUpdateCertificatePath", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "SaveUpdateCertificatePath";
                Logger.LogError(exception);
                return false;
            }
        }

        public DataTable GetUserMailId(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetUserMailId", paramList);
                //param = new SqlParameter("@UserEmailId", SqlDbType.VarChar);
                //param.Direction = ParameterDirection.Output;
                //paramList.Add(param);
                //SqlCommand cmd = dbManager.ExecuteProcedure("GetUserMailId", paramList);
                //if (cmd != null)
                //{
                //    string res1 = cmd.Parameters["@UserEmailId"].Value.ToString();
                //    return res1;
                //}
                //else
                //   return null;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetUserMailId";

                Logger.LogError(exception);
                return null;
            }
        }

        public DataTable GetTAMCEUploadedDocsData(int transactionId, int docsRoleTypeId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@DocsRoleTypeId", docsRoleTypeId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("GetTAMCEUploadedDocsData", paramList); // GetServicesByApplicationId
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetTAMCEUploadedDocsData";
                exception.CustomMessage = "transactionId Id : " + transactionId;
                Logger.LogError(exception);
                return null;
            }
        }
    }
}
