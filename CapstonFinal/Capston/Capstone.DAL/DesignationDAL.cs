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
    public class DesignationDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        #endregion
        public DesignationDAL()
        {
            dbManager = new SqlServerDBManager();
        }
    
        public DataTable SaveCabsdervice(CabserviceModel model)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                paramList.Add(param);
                param = new SqlParameter("@PNR", model.PNR);
                paramList.Add(param);
                param = new SqlParameter("@AirportId", model.AirportId);
                paramList.Add(param);
                param = new SqlParameter("@CountryId", model.CountryId);
                paramList.Add(param);
                param = new SqlParameter("@DepartureDate",Convert.ToDateTime( model.DepartureDate));
                paramList.Add(param);
                param = new SqlParameter("@ArrivalDate", Convert.ToDateTime(model.ArrivalDate));
                paramList.Add(param);
                param = new SqlParameter("@DropingAddress", model.DropingAddress);
                paramList.Add(param);
                param = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.UserId);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("CreateCabservice", paramList);
            }
            catch (Exception ex)
            {
                 return null;
            }

        }
        

        public DataTable GetCabservice(int userid)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", userid);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCabserviceData", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetOtherservice(int userid,int serviceid)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@studentId", userid);
                paramList.Add(param);
                param = new SqlParameter("@serviceId", serviceid);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetOtherserviceData", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable SaveOthersdervice(OtherServiceModel model)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                paramList.Add(param);
                param = new SqlParameter("@serviceId", model.ServiceId);
                paramList.Add(param);
               
                param = new SqlParameter("@studentId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@PickupDate",Convert.ToDateTime( model.PickupDate));
                paramList.Add(param);
                param = new SqlParameter("@PostalCode", model.PostalCode);
                paramList.Add(param);
                param = new SqlParameter("@droppingAddress", model.DropingAddress);
                paramList.Add(param);
                param = new SqlParameter("@PNR", model.PNR);
                paramList.Add(param);
                param = new SqlParameter("@TimeSlot", model.Timeslot);
                paramList.Add(param);

                return dbManager.ExecuteStoredProc("SaveOtherserivices", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }

        }






        public DataTable GetDesignationbyDeptId(int DepartmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();              
                param = new SqlParameter("@DepartmentId", DepartmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDesinationByDeptId", paramList);
            }
            catch (Exception ex)
            {
                 return null;
            }
        }
    }
}
