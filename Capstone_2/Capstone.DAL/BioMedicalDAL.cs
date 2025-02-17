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
  public  class BioCapstoneDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlCommand command;
        SqlParameter param;
        List<SqlParameter> paramList;

      
        #endregion
        public BioCapstoneDAL()
        {
            dbManager = new SqlServerDBManager();
        }
        public int SaveBioCapstoneApplicationDetails(BioCapstoneViewModel model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@Id", model.BioCapstoneApplicantModel.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);

                //Particulars of applicant
                param = new SqlParameter("@Name", model.BioCapstoneApplicantModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionName", model.BioCapstoneApplicantModel.InstitutionName);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRole", model.ApplicantRole);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRoleOther", model.BioCapstoneApplicantModel.ApplicantRoleOther);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", model.BioCapstoneApplicantModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", model.BioCapstoneApplicantModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", model.BioCapstoneApplicantModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@HouseNumber", model.BioCapstoneApplicantModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@StreetName", model.BioCapstoneApplicantModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@Fax", model.BioCapstoneApplicantModel.Fax);
                paramList.Add(param);
                param = new SqlParameter("@Telegraph", model.BioCapstoneApplicantModel.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@Telex", model.BioCapstoneApplicantModel.Telex);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.BioCapstoneApplicantModel.PINCode); 
                paramList.Add(param);

                //Authorisation of activity 
                param = new SqlParameter("@BioCapstoneAuthorisation", model.AuthorisationModel.Authorasation);
                paramList.Add(param);
                param = new SqlParameter("@BioCapstoneOthers", model.AuthorisationModel.Others);
                paramList.Add(param);
                //Address of Treatment Facility
                param = new SqlParameter("@TreatmentDistrictId", model.BioCapstoneAddressTreatementFacilityModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentMandalId", model.BioCapstoneAddressTreatementFacilityModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentVillageId", model.BioCapstoneAddressTreatementFacilityModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentHouseNumber", model.BioCapstoneAddressTreatementFacilityModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentStreetName", model.BioCapstoneAddressTreatementFacilityModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentFax", model.BioCapstoneAddressTreatementFacilityModel.Fax);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentTelegraph", model.BioCapstoneAddressTreatementFacilityModel.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentTelex", model.BioCapstoneAddressTreatementFacilityModel.Telex);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentPINCode", model.BioCapstoneAddressTreatementFacilityModel.PINCode);
                paramList.Add(param);

                //BioCapstone Address of Disposal waste
                param = new SqlParameter("@DisposalDistrictId", model.BioCapstoneAddressDisposalWasteModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@DisposalMandalId", model.BioCapstoneAddressDisposalWasteModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@DisposalVillageId", model.BioCapstoneAddressDisposalWasteModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@DisposalHouseNumber", model.BioCapstoneAddressDisposalWasteModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@DisposalStreetName", model.BioCapstoneAddressDisposalWasteModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@DisposalFax", model.BioCapstoneAddressDisposalWasteModel.Fax);
                paramList.Add(param);
                param = new SqlParameter("@DisposalTelegraph", model.BioCapstoneAddressDisposalWasteModel.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@DisposalTelex", model.BioCapstoneAddressDisposalWasteModel.Telex);
                paramList.Add(param);
                param = new SqlParameter("@DisposalPINCode", model.BioCapstoneAddressDisposalWasteModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                //Mode of treatment
                param = new SqlParameter("@TreatmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.TreatmentList);
                paramList.Add(param);
                //Mode of treatment and disposal
                param = new SqlParameter("@TreatmentDisposalList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.TreatmentDisposalList);
                paramList.Add(param);
                //Category an Quantity of Waste
                param = new SqlParameter("@QuantityWasteList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.QuantityWasteList);
                paramList.Add(param);
                //Declaration
                param = new SqlParameter("@Designation", model.DeclarationModel.Designation);
                paramList.Add(param);
                param = new SqlParameter("@Place", model.DeclarationModel.Place);
                paramList.Add(param);
                param = new SqlParameter("@Signature", model.DeclarationModel.Signature);
                paramList.Add(param);
                param = new SqlParameter("@Date", model.DeclarationModel.Date);
                paramList.Add(param);

                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstone", paramList);
                if (command != null)
                {
                    applicationId = Convert.ToInt32(command.Parameters["@ApplicationId"].Value);
                    transactionId = Convert.ToInt32(command.Parameters["@TransactionId"].Value);
                    formStatus = (FormStatus)Convert.ToInt32(command.Parameters["@FormStatus"].Value);
                    applicationNumber = command.Parameters["@ApplicationNumber"].Value.ToString();
                    return 1; //Convert.ToInt32(command.Parameters["@Id"].Value);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log            - kishore, 01-09-2017
                return -1;
            }
        }
        public int SaveBioApplicantDetails(BioCapstoneApplicantViewModel model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Name", model.Name);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionName", model.InstitutionName);
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
                param = new SqlParameter("@Fax", model.Fax);
                paramList.Add(param);
                param = new SqlParameter("@Telegraph", model.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@Telex", model.Telex);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstoneApplicant", paramList);
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
                // TODO: Write exception log            - kishore, 06-09-2017
                return -1;
            }
        }

        public int SaveBioCapstoneTreatmentDetails(BioCapstoneAddressTreatmentFacilityViewModel model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
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
                param = new SqlParameter("@Fax", model.Fax);
                paramList.Add(param);
                param = new SqlParameter("@Telegraph", model.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@Telex", model.Telex);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstoneTreatment", paramList);
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
                // TODO: Write exception log            - kishore, 06-09-2017
                return -1;
            }
        }

        public int SaveBioCapstoneDisposalwaste(BioCapstoneAddressofDisposalWaste model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
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
                param = new SqlParameter("@Fax", model.Fax);
                paramList.Add(param);
                param = new SqlParameter("@Telegraph", model.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@Telex", model.Telex);
                paramList.Add(param);
                param = new SqlParameter("@PINCode", model.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstoneDisposalwaste", paramList);
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
                // TODO: Write exception log            - kishore, 06-09-2017
                return -1;
            }
        }

        public int SaveModeoftreatment(List<TreatmentModle> objList, ref int applicationId, ref int transactionId,
          ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();              
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertTreatmentModeBulk", paramList);
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
                // TODO: Write exception log        - kishore, 06-09-2017
                return -1;
            }
        }
        public DataTable GetTreatment(int transactionId,string modeType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@ModeType", modeType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetTreatment", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 06-09-2017
                return null;
            }
        }
        public int SaveModeofTreatmentDisposal(List<TreatmentDisposalModle> objList, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@TreatmentDisposalList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertTreatmentDisposalModelBulk", paramList);
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
                // TODO: Write exception log        - kishore, 06-09-2017
                return -1;
            }
        }
        public DataTable GetTreatmentDisposal(int transactionId,string modeType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                param = new SqlParameter("@ModeType", modeType);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetTreatment", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 06-09-2017
                return null;
            }
        }

        public int SaveQuantityofWaste(List<QuantityWasteModel> objList, ref int applicationId, ref int transactionId,
       ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@ApplicationId", applicationId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@TransactionType", ApplicationType);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@CreatedUserId", objList[0].CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@QuantityWasteList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(objList);
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("InsertQuantityWasteModelBulk", paramList);
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
                // TODO: Write exception log        - kishore, 07-09-2017
                return -1;
            }
        }
        public DataTable GetQuantityWaste(int transactionId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@TransactionId", transactionId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetQuantityWaste", paramList);
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - kishore, 07-09-2017
                return null;
            }
        }

        public int SaveBioDeclarationDetails(DeclarationViewModel model, ref int applicationId, ref int transactionId,
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
                param = new SqlParameter("@TransactionId", transactionId);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Designation", model.Designation);
                paramList.Add(param);
                param = new SqlParameter("@Place", model.Place);
                paramList.Add(param);
                param = new SqlParameter("@Signature", model.Signature);
                paramList.Add(param);
                param = new SqlParameter("@Date", model.Date);
                paramList.Add(param);
               
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", 0);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstoneDeclaration", paramList);
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
                // TODO: Write exception log            - kishore, 06-09-2017
                return -1;
            }
        }

        public int SaveAuthorisationActivity(AuthorisationViewModel model, ref int applicationId, ref int transactionId,
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
               
                param = new SqlParameter("@Id", model.Id);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@Authorisation", model.Authorasation);
                paramList.Add(param);
                param = new SqlParameter("@Others", model.Others);
                paramList.Add(param);
                param = new SqlParameter("@UserId", model.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FormStatus", model.FormStatus);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@ApplicationNumber", SqlDbType.VarChar, 50);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);

                command = dbManager.ExecuteProcedure("SaveBioCapstoneAuthorisation", paramList);
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
                // TODO: Write exception log            - kishore, 08--09-2017
                return -1;
            }
        }
    }
}
