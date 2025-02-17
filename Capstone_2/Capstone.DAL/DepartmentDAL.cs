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
    public class DepartmentDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        #endregion
        public DepartmentDAL()
        {
            dbManager = new SqlServerDBManager();
        }
        public bool SaveDepartmentandAdmin(DepartmentAdminViewModel model)
        {
            try
            {
                paramList = new List<SqlParameter>();
                
                // Department parameters
                param = new SqlParameter("@DepartmentId", model.Department.Id);
                paramList.Add(param);
                param = new SqlParameter("@DepartmentName", model.Department.Name);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.Department.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.Department.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.Department.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@LogoPath", model.Department.Logo);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.Department.HouseNumber==null ? "" : model.Department.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.Department.StreetName == null ? "" : model.Department.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.Department.PinCode == null ? "" : model.Department.PinCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.Department.usermodel.Id);
                paramList.Add(param);
                // Department Admin parameters
                param = new SqlParameter("@AdminUserId", model.DepartmentAdmin.Id);
                paramList.Add(param);
                param = new SqlParameter("@FirstName", model.DepartmentAdmin.FirstName);
                paramList.Add(param);
                param = new SqlParameter("@AadharNumber", model.DepartmentAdmin.AadharNumber);
                paramList.Add(param);
                param = new SqlParameter("@MobileNumber", model.DepartmentAdmin.MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@EmailId", model.DepartmentAdmin.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@SecurityQuestion", model.DepartmentAdmin.SecurityQuestion);
                paramList.Add(param);
                param = new SqlParameter("@SecurityAnswer", model.DepartmentAdmin.SecurityAnswer);
                paramList.Add(param); 
               SqlCommand cmd= dbManager.ExecuteProcedure("CreateDepartmentandAdmin", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 06-05-2017
                return false;
            }
        }

        public DataTable GetDepartmentandAdmins()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetDepartmentandAdmins");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 07-05-2017
                throw;
            }
        }
        public DataTable GetCommissionerTAMCEData(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                if (FromDate == null || ToDate == null)
                {
                    return dbManager.ExecuteStoredProc("GetCommissionerDataTAMCE");
                }
                else
                {
                    paramList = new List<SqlParameter>();
                    param = new SqlParameter("@FromDate", FromDate);
                    paramList.Add(param);
                    param = new SqlParameter("@ToDate", ToDate);
                    paramList.Add(param);
                    return dbManager.ExecuteStoredProc("GetCommissionerDataOfTAMCE", paramList);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCommissionerDashboardForTAMCE(int statusId, int DistrictId, int MandalId, int VillageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@StatusId", statusId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetTAMCEDistrictWiseApplicationsByStatusId", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCommissionerTAMCEApplications(int StatusId, int VillageId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@StatusId", StatusId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerDashboardApplicationsForTAMCE", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCommissionerPCPNDTData()
        {
            try
            {
               
                return dbManager.ExecuteStoredProc("GetCommissionerDataByServisewise");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Servicewise Applications
        public DataTable GetCommissionerDashboard(int statusId, int DistrictId, int MandalId, int VillageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@StatusId", statusId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDistrictWiseApplicationsByStatusId", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        public DataTable GetDistrictWiseLicensedApplications(string Type,string ApplicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationType", ApplicationType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDistrictWiseLicensedApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCommissionerPCPNDTApplications(int StatusId, int VillageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@StatusId", StatusId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
               // param = new SqlParameter("@FromDate", FromDate);
                //paramList.Add(param);
               // param = new SqlParameter("@ToDate", ToDate);
               // paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerDashboardApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region BindMaps
        public DataTable BindMapsCount()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetMapsCountByServicewise");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable BindMaps(int serviceId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", serviceId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLatLang", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Licensed  Applications
        public DataTable GetCommissionerLicensedApplications(string Type)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLicensedApplications",paramList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetCumulativeLicensedApplications(int serviceId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", serviceId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLicensedCumulativeApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCumulativeLicensedApplicationsView(int serviceId, int districtId, int mandalId, int villageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", serviceId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", mandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", villageId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLicensedApplicationsView", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public DataTable GetCommissionerApprovedAmendments(string Type,int DistrictId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerApprovedAmendments", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetCommissionerApprovedRegistrations(string Type,int DistrictId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerApprovedRegistrations", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Pending Applications
        public DataTable GetPendingApplications(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerPendingApplications",paramList);//GetPendingApplications
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDistrictWisePendingApplicationsByDeptUser(int DeptUserId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DeptUserId", DeptUserId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDistrictWisePendingApplicationsByDeptUser", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable GetCommissionerPCPNDTPendingApplications(int DeptUserId, int DistrictId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DeptUserId", DeptUserId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerDashboardPendingApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetPendingApplicationsDistrictwise(int districtId, int serviceId, int mandalId, int villageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", serviceId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", mandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", villageId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetPendingApplicationsDistrictwise", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable GetPendingApplicationsView(int serviceId, int districtId,  int mandalId, int villageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", serviceId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", mandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", villageId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetPendingApplicationsView", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region License Expire Applications
        public DataTable GetLicenseExpireApplicationsCount()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetLicenseExpiryApplicationsCount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable GetCommissionerDashboardDetails(int districtId, int serviceId, int mandalId, int villageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", serviceId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", mandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", villageId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLicensedCumulativeApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public DataSet GetExpiredLicenses()
        {
            try
            {
                
                return dbManager.ExecuteSPMultipleResultSet("GetExpiredLicenses");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public DataTable GetCommissionerExpiredApplications(string Type, int DistrictId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Type", Type);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerExpiredApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region AppealApplications
        public DataTable GetAppealApplications(int DistrictId, int TransactionId, string TransactionType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", TransactionType);
                paramList.Add(param);
               
                return dbManager.ExecuteStoredProc("GetAppealApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region Dashboard1 Methods
        public DataTable GetDashboard1TAMCEApplicationsList(int StatusId, int DistrictId, int MandalId, int VillageId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@StatusId", StatusId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
                param = new SqlParameter("@FromDate", FromDate);
                paramList.Add(param);
                param = new SqlParameter("@ToDate", ToDate);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDashboard1ApplicationsList", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable GetReceivedTodayTAMCEApplications(string StatusId, int DistrictId, int MandalId, int VillageId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@StatusId", StatusId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", VillageId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetReceivedTodayTAMCEApplications", paramList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
