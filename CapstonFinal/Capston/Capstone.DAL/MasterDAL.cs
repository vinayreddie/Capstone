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
    public class MasterDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        SqlCommand command;
        #endregion
        public MasterDAL()
        {
            dbManager = new SqlServerDBManager();
        }

        public DataTable GetCountries()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetCountries");
            }
            catch (Exception ex)
            {
                 return null;
            }            
        }
        public DataTable GetAirport()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetAirport");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCollege()
        {
            try
            {
                return dbManager.ExecuteStoredProc("Getcollege");
            }
            catch (Exception ex)
            {
                 return null;
            }
        }
        
        
    }
}
