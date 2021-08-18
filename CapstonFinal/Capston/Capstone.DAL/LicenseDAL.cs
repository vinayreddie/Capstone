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

        
    }
}
