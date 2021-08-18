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
                 return null;
            }
        }

         
 

     }
}
