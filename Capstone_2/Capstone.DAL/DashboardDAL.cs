using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
    public class DashboardDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlCommand command;
        SqlParameter param;
        List<SqlParameter> paramList;

        public object Utilities { get; private set; }
        #endregion
        public DashboardDAL()
        {
            dbManager = new SqlServerDBManager();
        }

        public DataSet GetUserDashboard(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetUserDashboardInfo", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 22-05-2017
                return null;
            }
        }

        public DataTable GetDraftApplications(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDraftApplications", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 22-05-2017
                return null;
            }
        }

        public DataTable GetSubmittedApplications(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetSubmittedApplications", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 22-05-2017
                return null;
            }
        }

        public DataTable GetLicenses(int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", userId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetLicenses", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 22-05-2017
                return null;
            }
        }
        public DataTable GetCommissionerDashoboard(string type,int districtId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Type", type);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCommissionerData", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 03-11-2017
                return null;
            }
        }
        #region Department User Dashboard
        public DataSet GetDeptUserDashboadCounts(UserModel user)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserId", user.Id);
                paramList.Add(param);
                param = new SqlParameter("@CurrentDesignationId", user.DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", user.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", user.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", user.VillageId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet ("GetDeptUserDashboadCounts", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Mounika, 31-05-2017
                return null;
            }
        }

        #endregion

        #region Admin Dashboard
        public DataSet GetAdminDashboadCounts(int DistrictId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetAdminDashboadCounts", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public DataSet TransactionTrack(int TransactionId, string TransactionType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", TransactionType);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetTransactionTrack", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - pj, 04-07-2017
                return null;
            }
        }
    }
}
