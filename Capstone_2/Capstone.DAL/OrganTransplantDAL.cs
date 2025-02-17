using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Framework;

namespace Capstone.DAL
{
    public class OrganTransplantDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlCommand command;
        SqlParameter param;
        List<SqlParameter> paramList;

        public object Utilities { get; private set; }
        #endregion
        public int SaveHospitalDetails(HospitalViewModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);                                
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.HospitalName);
                paramList.Add(param);               
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@HouseNo", model.HouseNo);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.Pincode);
                paramList.Add(param);               
                param = new SqlParameter("@PhoneNo", model.PhoneNo);
                paramList.Add(param);
                param = new SqlParameter("@TeachingType", model.TeacherNonTeach);
                paramList.Add(param);
                param = new SqlParameter("@Government", model.Govtorpvt);
                paramList.Add(param);
                param = new SqlParameter("@Annualbudjet", model.AnnualBudjet);
                paramList.Add(param);
                param = new SqlParameter("@BedStrength", model.TotalBedStrength);
                paramList.Add(param);
                param = new SqlParameter("@DisciplinesName", model.Nameofdisciplines);
                paramList.Add(param);
                param = new SqlParameter("@PatientTurnOver", model.PatientTurnoverPerYear);
                paramList.Add(param);
                param = new SqlParameter("@Road", model.ByRoad);
                paramList.Add(param);
                param = new SqlParameter("@Rail", model.ByRail);
                paramList.Add(param);
                param = new SqlParameter("@Air", model.ByAir);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserID", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveOTHospital", paramList);
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
                // TODO: Write exception log            - Raj, 20-05-2017
                return -1;
            }
        }

        public int SaveSurgicalDetails(SurgicalTeamModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@NoofBeds", model.NumberofBeds);
                paramList.Add(param);
                param = new SqlParameter("@NoofOperationsperYear", model.NoofOperationsPerYear);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@Staffdetails", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.StaffDetailsList);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("SaveOTSurgicalTeam", paramList);
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
                // TODO: Write exception log            - Raj, 20-05-2017
                return -1;
            }
        }
        
        public DataTable GetStaffDetails(int TransactionId,string SectionName )
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();          
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@SectionName", SectionName);
                paramList.Add(param);               
                return  dbManager.ExecuteStoredProc ("GetOTStaffDetails", paramList);
               
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - Raj, 20-05-2017
                return null;
            }
        }
        public int SaveCapstoneDetails(CapstoneTeamModel model, ref int applicationId, ref int transactionId,
         ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@NoofBeds", model.NoofBeds);
                paramList.Add(param);
                param = new SqlParameter("@PatientTurnover", model.PatientTurnover);
                paramList.Add(param);
                param = new SqlParameter("@TransplantPatients", model.TransplantPatients);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@Staffdetails", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.StaffDetailsList);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("[SaveOTCapstoneTeam]", paramList);
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
                // TODO: Write exception log            - Raj, 20-05-2017
                return -1;
            }
        }
        public int SaveAnaesthesiologyDetails(AnaesthesiologyModel model, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@OperationTheatres", model.OperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@EmergencyOperationTheatres", model.EmergencyOperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@TransplantOperationTheatres", model.TransplantOperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@Staffdetails", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.StaffDetailsList);
                paramList.Add(param);
                param = new SqlParameter("@OperationList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.OperationsList);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.EquipmentsList);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveOTAnaesthesiology", paramList);
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
                // TODO: Write exception log            - Raj, 20-05-2017
                return -1;
            }
        }
        public DataTable GetOperations(int TransactionId,string SectionName)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@SectionName", SectionName);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetOTOperations", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - Raj, 20-05-2017
                return null;
            }
        }
        public DataTable GetEquipments(int TransactionId,string SectionName)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@SectionName", SectionName);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetOTEqupments", paramList);

            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - Raj, 20-05-2017
                return null;
            }
        }
        public int SaveICUHDUFacilities(AnaesthesiologyModel model, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                dbManager = new DAL.SqlServerDBManager();
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ICUBeds", model.OperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@Trained", model.EmergencyOperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@Nurses", model.TransplantOperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@Technicians", model.TransplantOperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@OtherFacilities", model.TransplantOperationTheatres);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.UserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);               
                param = new SqlParameter("@Equipments", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.EquipmentsList);
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveOTAnaesthesiology", paramList);
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
                // TODO: Write exception log            - Raj, 20-05-2017
                return -1;
            }
        }

    }
}
