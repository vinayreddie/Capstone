using Capstone.Framework;
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
    public class PCPNDTDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlCommand command;
        SqlParameter param;
        List<SqlParameter> paramList;

        public object Utilities { get; private set; }
        #endregion
        public PCPNDTDAL()
        {
            dbManager = new SqlServerDBManager();
        }
        public int SaveApplicantDetails(ApplicantModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRole", model.ApplicantRole);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRoleOther", model.ApplicantRoleOther);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", model.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@PAN", model.PAN);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantPhotoPath", model.ApplicantPhoto == "undefined" ? (object)DBNull.Value : model.ApplicantPhoto);
                paramList.Add(param);

                param = new SqlParameter("@AadharCardPath", model.AadharCardPath == "undefined" ? (object)DBNull.Value : model.AadharCardPath);
                paramList.Add(param);
                param = new SqlParameter("@PANCardPath", model.PANCardPath == "undefined" ? (object)DBNull.Value : model.PANCardPath);
                paramList.Add(param);

                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveApplicantDetails", paramList);
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

        //public int SaveEstablishmentDetails(EstablishmentModel model, ref int applicationId, ref int transactionId, 
        //    ref FormStatus formStatus, ref string applicationNumber)
        //{
        //    try
        //    {
        //        paramList = new List<SqlParameter>();
        //        param = new SqlParameter("@Id", model.Id);
        //        param.Direction = System.Data.ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@ApplicationId", applicationId);
        //        param.Direction = System.Data.ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@TransactionId", transactionId);
        //        param.Direction = System.Data.ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@Name", model.Name);
        //        paramList.Add(param);
        //        param = new SqlParameter("@DistrictId", model.DistrictId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@MandalId", model.MandalId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@VillageId", model.VillageId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@HouseNumber", model.HouseNumber);
        //        paramList.Add(param);
        //        param = new SqlParameter("@StreetName", model.StreetName);
        //        paramList.Add(param);
        //        param = new SqlParameter("@PINCode", model.PINCode);
        //        paramList.Add(param);
        //        param = new SqlParameter("@AddressProofDocPath", model.AddressProofPath);
        //        paramList.Add(param);
        //        param = new SqlParameter("@BuildingLayoutDocPath", model.BuildingLayoutPath);
        //        paramList.Add(param);
        //        param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@FormStatus", model.FormStatus);
        //        param.Direction = ParameterDirection.InputOutput;
        //        paramList.Add(param);
        //        param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
        //        param.Direction = ParameterDirection.Output;
        //        paramList.Add(param);

        //        command = dbManager.ExecuteProcedure("[SaveEstablishmentDetails]", paramList);
        //        if(command != null)
        //        {
        //            applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
        //            transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
        //            formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
        //            applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
        //            return Convert.ToInt32(command.Parameters["@Id"].Value);
        //        }
        //        else
        //        {
        //            return 0;
        //        }                   

        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log.       - Raj, 10-05-2017
        //        return -1;
        //    }
        //}
        public int SaveFacilityDetails(FacilityModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@Id", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@Facilities", model.Faclities);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@Phone", model.Phone);
                paramList.Add(param);
                param = new SqlParameter("@Email", model.Email);
                paramList.Add(param);
                param = new SqlParameter("@Fax", model.Fax);
                paramList.Add(param);
                param = new SqlParameter("@Telegrah", model.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@Telex", model.Telex);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@AddressProofType", model.AddressProofType);
                paramList.Add(param);
                param = new SqlParameter("@AddressProofDocPath", model.AddressProofPath);
                paramList.Add(param);
                param = new SqlParameter("@BuildingLayoutDocPath", model.BuildingLayoutPath);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipType", model.OwnershipType);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipDocPath", model.OwnerShipPath);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveFacilityDetails", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 12-05-2017
                return -1;
            }
        }
        public int SaveTests(TestsModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@InvasiveTests", model.InvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@NonInvasiveTests", model.NonInvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", model.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveTests", paramList);
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
                // TODO: Write exception log        - Raj, 21-05-2017
                return -1;
            }
        }
        public int SaveEquipments(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber, string ApplicationType, int existingApplicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 2);  // PCPNDT Grant Service Id : 2
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", existingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertEquipmentBulk", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    // Set FormStatus
                    if (objList.Count(item => item.IsDeleted == false) > 0)
                        formStatus = FormStatus.Completed;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-05-2017
                return -1;
            }
        }
        public DataTable GetEquipments(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEquipments", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 8-05-2017
                return null;
            }
        }
        public int SaveFacilities(FacilitesModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Tests", model.Tests);
                paramList.Add(param);
                param = new SqlParameter("@Studies", model.Studies);
                paramList.Add(param);
                param = new SqlParameter("@Remarks", model.Remarks);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveFacilities", paramList);
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
                // TODO: Write exception log            - Raj, 19-05-2017
                return -1;
            }
        }
        public int SaveEmployeesOld(List<EmployeeViewModel> objList, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber, string ApplicationType, int existingApplicationId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 2);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", existingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@EmployeeList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertEmployeeBulk", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    // Set FormStatus
                    if (objList.Count(item => item.IsDeleted == false) > 0)
                        formStatus = FormStatus.Completed;
                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log      - Raj, 19-05-2017
                return -1;
            }
        }

        public int SaveEmployees(List<EmployeeViewModel> objList, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber, string ApplicationType, int existingApplicationId)
        {
            try
            {
                int result = 0;
                foreach (var employee in objList)
                {
                    result += SavePCPNDTEmployee(employee, ref applicationId, ref transactionId,
                        ref applicationNumber, ApplicationType, ref existingApplicationId);
                }

                if (result > 0)
                    return 1;
                else
                    return result;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log      - Raj, 19-05-2017
                return -1;
            }
        }

        public DataTable CheckforEmployeeRegistration(string registrationNumber)
        {
            paramList = new List<SqlParameter>();
            param = new SqlParameter("@EmployeeRegistrationNumber", registrationNumber);
            paramList.Add(param);

            return dbManager.ExecuteStoredProc("CheckforEmployeeRegistration", paramList);
        }

        private int SavePCPNDTEmployee(EmployeeViewModel employee, ref int applicationId, ref int transactionId,
            ref string applicationNumber, string ApplicationType, ref int existingApplicationId)
        {
            try
            {

                #region Prepare Parameters
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ServiceId", 2);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", existingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", employee.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@Id", employee.Id);
                paramList.Add(param);
                param = new SqlParameter("@Name", employee.Name);
                paramList.Add(param);
                param = new SqlParameter("@DesignationId", employee.DesignationId);
                paramList.Add(param);
                param = new SqlParameter("@SubDesignation", employee.SubDesignation);
                paramList.Add(param);
                param = new SqlParameter("@ExpYears", employee.ExpYears);
                paramList.Add(param);
                param = new SqlParameter("@ExpMonths", employee.ExpMonths);
                paramList.Add(param);
                param = new SqlParameter("@ExpDays", employee.ExpDays);
                paramList.Add(param);
                param = new SqlParameter("@RegistrationNumber", employee.RegistrationNumber);
                paramList.Add(param);
                param = new SqlParameter("@IsDeleted", employee.IsDeleted);
                paramList.Add(param);
                param = new SqlParameter("@DocumentsList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(employee.UploadDocuments);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                #endregion

                command = dbManager.ExecuteProcedure("SavePCPNDTEmployee", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return 1;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log      - Raj, 19-05-2017
                return -1;
            }
        }

        public DataSet GetEmployees(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("GetEmployees", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 8-05-2017
                return null;
            }
        }
        public int SaveInstitutionDetails(InstitutionModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@OwnershipTypeId", model.OwnershipTypeId);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipOthers", model.OwnershipOthers);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionTypeId", model.InstitutionTypeId);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionOthers", model.InstitutionOthers);
                paramList.Add(param);
                param = new SqlParameter("@TotalWorkArea", model.TotalWorkArea);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@AffidavitDocPath", model.AffidavitDocPath);
                paramList.Add(param);
                param = new SqlParameter("@ArticleDocPath", model.ArticleDocPath);
                paramList.Add(param);
                param = new SqlParameter("@StudyCertificates", SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable<DocumentUploadModel>(model.StudyCertificateDocPaths);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveInstitutionDetails", paramList);
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
        public int SaveDeclarationDetails(DeclarationModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", model.ApplicationType.ToString());
                paramList.Add(param);
                param = new SqlParameter("@ExistingApplicationId", model.ExistingApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@SonOf", model.SonOf);
                paramList.Add(param);
                param = new SqlParameter("@Age", model.Age);
                paramList.Add(param);
                param = new SqlParameter("@ResidentOf", model.ResidentOf);
                paramList.Add(param);
                param = new SqlParameter("@Designation", model.Designation);
                paramList.Add(param);
                param = new SqlParameter("@Organization", model.Organization);
                paramList.Add(param);
                param = new SqlParameter("@Date", model.Date);
                paramList.Add(param);
                param = new SqlParameter("@Place", model.Place);
                paramList.Add(param);
                param = new SqlParameter("@Signature", model.Signature);
                paramList.Add(param);
                param = new SqlParameter("@SignatureDocPath", model.SignatureDocPath);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@OtherUploads", SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable<DocumentUploadModel>(model.OtherUploadsList);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveDeclarationDetails", paramList);
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

                // Utitlities.SendSMS("9948111643",  model.Date.ToString());
                // TODO: Write exception log        - Raj, 21-05-2017
                return -1;
            }
        }
        public int GetEnclosuresCnt(int transactionId)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@UploadsCount", SqlDbType.Int);
                param.Value = 0;
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("GetEnclosersCnt", paramList);
                if (command == null)
                    return 0;

                var count = Convert.ToInt32(command.Parameters["@UploadsCount"].Value);
                return count;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 19-09-2017
                return 0;
            }
        }
    }
}
