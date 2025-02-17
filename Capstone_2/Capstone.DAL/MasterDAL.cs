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
        
        public DataTable GetMandals(int districtId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetMandals", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 05-05-2017
                return null;
            }            
        }
        public DataTable GetVillages(int mandalId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@MandalId", mandalId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetVillages", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 06-05-2017
                return null;
            }
        }
        public DataSet GetOwnershipMasterData()
        {
            try
            {
                return dbManager.ExecuteSPMultipleResultSet("GetOwnershipMasterData");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.           - Raj, 20-05-2017
                return null;
            }
        }
        public DataTable GetDoctorSpecialityMaster()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetDoctorSpecialities");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 30-05-2017
                throw;
            }
        }
        public DataTable GetFacilityMaster()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetFacilityMaster");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 30-05-2017
                throw;
            }
        }
        public DataTable GetHospitalTypes()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetHospitalTypes");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 05-05-2017
                return null;
            }
        }

        public DataTable GetEquipmentTypesList(int HospitalTypeId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@HospitalTypeId", HospitalTypeId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEquipmentTypesList", paramList);
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - Raj, 13-06-2017
                return null;
            }
        }
        public DataTable GetOfferedServices()
        {
            try
            {
                return dbManager.ExecuteStoredProc("[dbo].[GetOfferedServices]");
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - Raj, 13-06-2017
                return null;
            }
        }

        public DataTable GetOfferedServicesByHospitalTypeId(int hospitalTypeId)
        {
            string sp = "GetOfferedServicesByHospitalTypeId";
            try
            {
                paramList = new List<SqlParameter>()
                {
                    new SqlParameter("@HospitalTypeId", hospitalTypeId)
                };

                return dbManager.ExecuteStoredProc(sp, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetEquipmentBasedOnOfferedServices(string offeredServiceIds)
        {
            string sp = "GetEquipmentBasedOfferedServices";
            try
            {
                paramList = new List<SqlParameter>()
                {
                    new SqlParameter("@OfferedServiceIds", offeredServiceIds)
                };

                return dbManager.ExecuteStoredProc(sp, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetQualifications(string actType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ActType", actType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetQualifications", paramList);
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - Raj, 13-06-2017
                return null;
            }
        }

        #region   Designation List
        public DataTable GetDesignationList(int departmentId)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@departmentId", departmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDesinationByDeptId", paramList);

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DataTable GetDepartmentUsersList(int RoleType, int DepartmentId)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@RoleId", RoleType);
                paramList.Add(param);
                param = new SqlParameter("@DepartmentId", DepartmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetDepartmentUsersData", paramList);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Equipment Master Details
        public int SaveEquipmentDetails(EquipmentMasterModel equipmentModel)
        {
            int result = 0;
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", equipmentModel.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", equipmentModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentType", equipmentModel.Type);
                paramList.Add(param);
                param = new SqlParameter("@IsActive", equipmentModel.IsActive);
                paramList.Add(param);
                param = new SqlParameter("@UserId", equipmentModel.UserId);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveEquipmentMaster", paramList);
                if (command != null)
                {
                    result = Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public DataTable GetEquipmentsList()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetEquipmentsList");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 06-05-2017
                return null;
            }
        }
        public DataTable GetEquipmentDetails(int EquipmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@EquipmentId", EquipmentId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEquipmentDetails", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetEquipmentDetails on Id : " + EquipmentId;
                Logger.LogError(exception);
                return null;
            }
        }

        //public DataTable CheckforEquipmentExistance(int id, string name)
        //{
        //    try
        //    {
        //        paramList = new List<SqlParameter>();
        //        param = new SqlParameter("@FPOId", fpoId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@Id", id);
        //        paramList.Add(param);
        //        param = new SqlParameter("@Name", name);
        //        paramList.Add(param);
        //        return dbManager.ExecuteStoredProc("CheckforEquipmentExistance", paramList);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
        //        exception.DbObject = "CheckforEquipmentExistance";
        //        exception.CustomMessage = "FPOId : " + fpoId + "; CropName : " + name;
        //        Logger.LogError(exception);
        //        return null;
        //    }
        //}
        #endregion


        public int SaveOfferedServicesEquipments(OfferedServiceEquipmentsMasterModel model)
        {
            int result = 0;
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param); 
                param = new SqlParameter("@ServiceId", model.ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@HospitalTypeId", model.HospitalTypeId);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentIds", model.EquipmentIds);
                paramList.Add(param);
                param = new SqlParameter("@IsActive", model.IsActive);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveOfferedServicesEquipments", paramList);
                if (command != null)
                {
                    result = Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public DataTable GetOfferedServicesEquipmentsList()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetOfferedServicesEquipmentsList");
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 06-05-2017
                return null;
            }
        }

        public DataTable GetOfferedServiceEquipmentsDetails(int ServiceId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", ServiceId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetOfferedServiceEquipmentsbyId", paramList);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = "GetOfferedServiceEquipmentsbyId on Id : " + ServiceId;
                Logger.LogError(exception);
                return null;
            }
        }

        public DataTable GetCentresList(int districtId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DistrictId", districtId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetCentresList", paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
