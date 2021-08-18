using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
namespace Capstone.BAL
{
    public class DashboardBAL
    {
        DashboardDAL objDAL;

        public DataSet GetUserDashboard(int userId)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetUserDashboard(userId);
        }

       
       
       
    }

 

    
}


