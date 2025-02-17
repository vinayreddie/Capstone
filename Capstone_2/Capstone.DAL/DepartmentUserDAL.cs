using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data;
using Capstone.Framework;                        

namespace Capstone.DAL
{
    public class DepartmentUserDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        #endregion
        public DepartmentUserDAL()
        {
            dbManager = new SqlServerDBManager();
        }
        public DataTable GetListofApplications(int DesignationId,int DistrictId,int MandalId,int VillageId,string Type,int UserId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DesignationId", DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetListofApplications", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 10-05-2017
                return null;
            }
        }
        #region New/Grant 
        public DataSet ApprovalSceenOnloadData(int TransactionId,int DesignationId,int ServiceId, int DeptUserId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@DesignationId", DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@DeptUserId", DeptUserId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet ("ApprovalSceenOnloadData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 10-05-2017
                return null;
            }
        }
        public bool SaveApprovals(ApprovalsModel Approval ,int DesignationId,DataTable InspectionReports,DataTable UploadList, int ReferenceId, string DocumentPath)
        {
            try
             {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", Approval.TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@UserId", Approval.UserId);
                paramList.Add(param);
                param = new SqlParameter("@Status", Approval.status);
                paramList.Add(param);
                param = new SqlParameter("@Remarks",Approval.Remarks );
                paramList.Add(param);
                param = new SqlParameter("@DesignationId", DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@InspectionReports", InspectionReports);
                param.SqlDbType = System.Data.SqlDbType.Structured;
                paramList.Add(param);
                param = new SqlParameter("@UploadList", UploadList);
                param.SqlDbType = System.Data.SqlDbType.Structured;
                paramList.Add(param);
                param = new SqlParameter("@ReferenceId", ReferenceId);
                paramList.Add(param);
                param = new SqlParameter("@DocumentPath", DocumentPath);
                paramList.Add(param);
                param = new SqlParameter("@InspectionReportDate", Approval.InspectionDate);
                paramList.Add(param);
                SqlCommand cmd= dbManager.ExecuteProcedure ("SaveApproval", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 10-05-2017
                return false;
            }
        }

        public DataTable GetApprovalSMSData(int TransactionId, int WorkFlowType,string Type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@WorkFlowType", WorkFlowType);
                paramList.Add(param);
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDeptSMSData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - siva, 18-10-2017
                return null;
            }
        }
        public DataTable GetQureyResponsebyTransactionId(int TransactionId,string TransactionType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId",TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", TransactionType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetQureyResponsebyTransactionId", paramList);
             
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika,23-05-2017
                return null;
            }
        }
        public bool SaveInspectionFacilitiesQuestions(DataTable InspectionModel,int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@InspectionModel", InspectionModel);
                param.SqlDbType = System.Data.SqlDbType.Structured; 
                paramList.Add(param);
               
                SqlCommand cmd = dbManager.ExecuteProcedure("SaveInspectionFacilityQuestions", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 02-06-2017
                return false;
            }
        }

        public DataSet GetInspectionQuestionsList(int TransactionId, int DepartmentUserId,string Type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);  
                paramList.Add(param);
                param = new SqlParameter("@DeptUserId", DepartmentUserId);
                paramList.Add(param);
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);


                return dbManager.ExecuteSPMultipleResultSet("SP_GetInspectionDetails", paramList);
               
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 03-06-2017
                return null;
            }

        }
        #endregion

        #region Amendment
        public DataTable GetListofAmendments(int DesignationId, int DistrictId, int MandalId, int VillageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DesignationId", DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);              
                return dbManager.ExecuteStoredProc("GetListofAmendments", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 10-05-2017
                return null;
            }
        }
        public DataSet AmendmentApprovalOnloadData(int AmendmentId, int DesignationId, int ServiceId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                param = new SqlParameter("@DesignationId", DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", ServiceId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("AmendmentApprovalOnloadData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 10-05-2017
                return null;
            }
        }
        public bool SaveAmedmentApprovals(ApprovalsModel Approval, int DesignationId, DataTable InspectionReports, DataTable UploadList, int ReferenceId, string DocumentPath)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", Approval.AmendmentId);// TrasactionId =Ammendment Id
                paramList.Add(param);
                param = new SqlParameter("@UserId", Approval.UserId);
                paramList.Add(param);
                param = new SqlParameter("@Status", Approval.status);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", Approval.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@DesignationId", DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@InspectionReports", InspectionReports);
                param.SqlDbType = System.Data.SqlDbType.Structured;
                paramList.Add(param);
                param = new SqlParameter("@UploadList", UploadList);
                param.SqlDbType = System.Data.SqlDbType.Structured;
                paramList.Add(param);
                param = new SqlParameter("@ReferenceId", ReferenceId);
                paramList.Add(param);
                param = new SqlParameter("@DocumentPath", DocumentPath);
                paramList.Add(param);

                SqlCommand cmd = dbManager.ExecuteProcedure("SaveAmendmentApproval", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 10-05-2017
                return false;
            }
        }
        #endregion

        #region PCPNDT Individual tabs data For Amendments
        public DataTable GetFacility(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return  dbManager.ExecuteStoredProc("GetFacilityDetails", paramList);
                
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 12-06-2017
                return null;
            }
        }
        public DataTable GetPCPNDTTests(int AmendmentId,string AmendmentType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                param = new SqlParameter("@AmendmentType", AmendmentType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetPCPNDTTests", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataTable GetFacilitiesforTests(int AmendmentId, string AmendmentType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                param = new SqlParameter("@AmendmentType", AmendmentType);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetFacilitiesDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataTable GetEquipments(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEquipmentDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataSet GetEmployees(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetEmployeeDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataSet GetOwnership(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet ("GetOwnershipDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataTable GetInstitution(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetInstitutionDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataTable GetCancelLicenseDetails(int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);// TrasactionId =Ammendment Id  
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCancelDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }
        public DataTable GetPCPNDTAppealReason(int AmendmentId,int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetPCPNDTAppealReason", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 13-06-2017
                return null;
            }
        }

        #endregion

        #region APMCEAmendmentDetails

        public DataTable GetAPMCERegistrationDetails(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCERegistrationDetails", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAPMCECorrespondentDetails(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCECorrespondingAddress", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 21-07-2017
                return null;
            }
        }

        public DataTable GetAPMCEAccomodationDetails(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCEAccomodationDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 21-07-2017
                return null;
            }
        }

        public DataTable GetAPMCEEquipmentAndFurniture(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCEEquipmentAndFurniture", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 21-07-2017
                return null;
            }
        }
        public DataTable GetAPMCETypeOfServices(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCETypeOfServices", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 21-07-2017
                return null;
            }
        }
        public DataTable GetAPMCEStaffDetails(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCEStaffDetails", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 21-07-2017
                return null;
            }
        }

        public DataTable GetAPMCEFacilitiesAvailable(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCEFacilitiesAvailable", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - siva, 21-07-2017
                return null;
            }
        }

        public DataTable GetAPMCEAppealReason(int AmendmentId, int TransactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetAPMCEAppealReason", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - kishore, 09-10-2017
                return null;
            }
        }

        public DataTable GetNOCofEquipment(int AmendmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@AmendmentId", AmendmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetNOCofEquipment", paramList);

            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetEquipmentbyTransactionId";
                exception.CustomMessage = "AmendmentId :" + AmendmentId;
                Logger.LogError(exception);
                return null;
            }
        }
        #endregion

        public DataTable GetStatuswiseApplicationDetailsIndex(int UserId,int RoleId,string FromDate=null, string ToDate=null, int DistrictId = 0)
        {
            var sp = "GetStatuswiseApplicationDetailsIndex";
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                param = new SqlParameter("@RoleId", RoleId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
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

        public DataTable GetDistrictwiseNewCentresReport(int UserId, int RoleId, string FromDate = null, string ToDate = null, int DistrictId = 0)
        {
            var sp = "GetDistrictwiseNewCentresReport";
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                param = new SqlParameter("@RoleId", RoleId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
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

        public DataTable GetDistrictwiseCentresReport(int DistrictId = 0, int CentreId = 0)
        {
            var sp = "GetDistrictwiseCentresReport";
            try
            {
                paramList = new List<SqlParameter>();                
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@CentreId", CentreId);
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

        public DataTable GetDistrictwiseCentresLicensesReport(int DistrictId = 0, int CentreId = 0)
        {
            var sp = "GetDistrictwiseCentresLicensesReport";
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@CentreId", CentreId);
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
    }
}

