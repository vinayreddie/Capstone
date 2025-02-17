using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Capstone.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.BAL
{
    public class UserBAL : MasterBAL 
    {
        #region Global
        UserDAL objDAL;
        #endregion

        public string GetSaltCode(string UserName)
        {
            objDAL = new UserDAL();
            return objDAL.GetSaltCode(UserName);
        }
        public UserViewModel  ValidateUser(LoginModel login)
        {
            try
            {
                objDAL = new UserDAL();
                DataSet dsItems = objDAL.ValidateUser(login);
                if (dsItems == null || dsItems.Tables.Count == 0)
                    return null;

                #region Preparing User Model
                DataTable dtUser = dsItems.Tables[0];
               UserViewModel user = new UserViewModel();
                user.Id = Convert.ToInt32(dtUser.Rows[0]["ID"]);
                user.FirstName = dtUser.Rows[0]["UserName"].ToString();
                user.EmailId = dtUser.Rows[0]["Email"].ToString();
                // user.LastName = dtUser.Rows[0]["LastName"].ToString();
                // user.RoleId = Convert.ToInt32(dtUser.Rows[0]["RoleId"]);
                // user.DesignationId =string.IsNullOrEmpty(dtUser.Rows[0]["DesignationId"].ToString())?0: Convert.ToInt32(dtUser.Rows[0]["DesignationId"]);
                // user.DepartmentId =string.IsNullOrEmpty(dtUser.Rows[0]["DepartmentId"].ToString())?0: Convert.ToInt32(dtUser.Rows[0]["DepartmentId"]);
                // user.DistrictId = string.IsNullOrEmpty(dtUser.Rows[0]["DistrictId"].ToString())?0: Convert.ToInt32(dtUser.Rows[0]["DistrictId"]);
                // user.MandalId= string.IsNullOrEmpty(dtUser.Rows[0]["MandalId"].ToString())?0: Convert.ToInt32(dtUser.Rows[0]["MandalId"]);
                // user.VillageId= string.IsNullOrEmpty(dtUser.Rows[0]["VillageId"].ToString()) ? 0 : Convert.ToInt32(dtUser.Rows[0]["VillageId"]);
                // user.UserPhoto= string.IsNullOrEmpty(dtUser.Rows[0]["ImagePath"].ToString()) ?null : dtUser.Rows[0]["ImagePath"].ToString();
                //user.AadharNumber= dtUser.Rows[0]["Aadhar"].ToString();
                // user.PANNumber = dtUser.Rows[0]["PanNumber"].ToString();
                // user.EmailId = dtUser.Rows[0]["EmailId"].ToString();
                // user.StreetName = dtUser.Rows[0]["StreetName"].ToString();
                // user.MobileNumber = dtUser.Rows[0]["MobileNumber"].ToString();
                // user.HouseNo = dtUser.Rows[0]["HouseNo"].ToString();
                // user.UserName = dtUser.Rows[0]["UserName"].ToString();

                // user.IsMobileNoVerified = Convert.ToBoolean(dtUser.Rows[0]["IsMobileVerified"]);
                #endregion

                #region Preparing Role Model
                RoleModel role = new RoleModel();
                DataTable dtRole = dsItems.Tables[1];
                role.Id = Convert.ToInt32(dtRole.Rows[0]["Id"]);
                role.Name = dtRole.Rows[0]["Name"].ToString();
                role.DefaultAction = String.IsNullOrEmpty(dtRole.Rows[0]["DefaultAction"].ToString()) ?
                    null : dtRole.Rows[0]["DefaultAction"].ToString();
                role.DefaultController = String.IsNullOrEmpty(dtRole.Rows[0]["DefaultController"].ToString()) ?
                    null : dtRole.Rows[0]["DefaultController"].ToString();
                role.DefaultArea = String.IsNullOrEmpty(dtRole.Rows[0]["DefaultArea"].ToString()) ?
                    null : dtRole.Rows[0]["DefaultArea"].ToString();
                user.Role = role;
                #endregion



                #region Preparing Linklist
                List<RoleLinkModel> linkList = new List<RoleLinkModel>(); RoleLinkModel linkItem;
                DataTable dtLink = dsItems.Tables[2];
                foreach (DataRow row in dtLink.Rows)
                {
                    linkItem = new RoleLinkModel();
                    linkItem.Id = Convert.ToInt32(row["Id"]);
                    linkItem.RoleId = Convert.ToInt32(row["RoleId"]);
                    linkItem.MenuId = Convert.ToInt32(row["MenuId"]);
                    linkItem.MenuName = row["MenuName"].ToString();
                    linkItem.LinkName = row["LinkName"].ToString();
                    if (row["ParentLinkId"] != System.DBNull.Value)
                        linkItem.ParentLinkId = Convert.ToInt32(row["ParentLinkId"]);
                    linkItem.AreaName = row["AreaName"].ToString();
                    linkItem.ControllerName = row["ControllerName"].ToString();
                    linkItem.ActionName = row["ActionName"].ToString();
                    linkItem.IconClass = row["IconClass"].ToString();
                    linkItem.OrderNumber = Convert.ToInt32(row["OrderNumber"]);
                    linkList.Add(linkItem);
                }
                user.Role.LinkList = linkList;
                #endregion

                return user;
            }
            catch (Exception ex)
            {
                 return null;
            }
        }
        public int SaveUserRegistratinDetails(UserModel objusermodel)
        {
            return objDAL.SaveUserRegistratinDetails(objusermodel);
            
        }
        //public List<DistrictModel> GetDistricts()
        //{
        //    List<DistrictModel> DistrictList = new List<DistrictModel>();
        //    DataTable dt = objDAL.GetDistricts();
        //    if (dt != null)
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            DistrictModel objDistrict = new DistrictModel();
        //            objDistrict.Id = Convert.ToInt32(row["DistrictId"]);
        //            objDistrict.Name = row["DistrictName"].ToString();
        //            DistrictList.Add(objDistrict);
        //        }
        //    }
        //    return DistrictList;
        //}
        //public List<MandalModel> GetMandalsByDistrictId(int DistrictId)
        //{
        //    List<MandalModel> MandalList = new List<MandalModel>();
        //    DataTable dt = objDAL.GetMandalsByDistrictId(DistrictId);
        //    if (dt != null)
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            MandalModel objMandal = new MandalModel();
        //            objMandal.Id = Convert.ToInt32(row["MandalId"]);
        //            objMandal.Name = row["MandalName"].ToString();
        //            MandalList.Add(objMandal);
        //        }
        //    }
        //    return MandalList;
        //}

        //public List<VillageModel> GetVillgesByMandalId(int MandalId)
        //{
        //    List<VillageModel> VillageList = new List<VillageModel>();
        //    DataTable dt = objDAL.GetVillagesByMandalId(MandalId);
        //    if (dt != null)
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            VillageModel objVillage = new VillageModel();
        //            objVillage.Id = Convert.ToInt32(row["VillageId"]);
        //            objVillage.Name = row["VillageName"].ToString();
        //            VillageList.Add(objVillage);
        //        }
        //    }
        //    return VillageList;
        //}

        #region DeptAdminAddUser
        //public DataTable CheckUserExistOrNot(UserModel User)
        //{
        //    return objDAL.CheckUserExistOrNot(User);
        //}
        public bool AddUser(UserModel user)
        {
            objDAL=new UserDAL();
            return objDAL.AddUser(user);
        }
        #endregion

        public int SaveUserRegistratin(UserModel objuser)
        {
            objDAL = new UserDAL();
            return objDAL.SaveUserRegistratin(objuser);
           
        }
        public int SaveContact(ContactModel objuser)
        {
            objDAL = new UserDAL();
            return objDAL.SaveContact(objuser);

        }
        public bool updateUserToTAMCEapplication(int existingApplicationTransId,int newUserId)
        {
            objDAL = new UserDAL();
            return objDAL.updateUserToTAMCEapplication(existingApplicationTransId, newUserId);

        }
        public string ValidateUser(UserModel objuser)
        {
            objDAL = new UserDAL();
            return objDAL.ValidateUser(objuser);

        }
        public UserModel ForgetPassword(string EmailId)  //int UserId,
        {
            objDAL = new UserDAL();
            UserModel model = new UserModel();
            DataTable dtUser = objDAL.ForgetPassword(EmailId);  //UserId,
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                DataRow dr = dtUser.Rows[0];
               // model.Id = UserId;
                model.UserName = Convert.ToString(dr["UserName"]);
                model.Password = Convert.ToString(dr["Password"]);
            }
            return model;
        }

        public UserViewModel ProfileDetails(int UserId)
        {
            objDAL = new UserDAL();
            UserViewModel model = new UserViewModel();
            model.Role = new RoleModel();

            DataTable dtProfile = objDAL.ProfileDetails(UserId);
            if(dtProfile!=null && dtProfile.Rows.Count > 0)
            {
                DataRow dr = dtProfile.Rows[0];
                model.Id = UserId;
                model.FirstName = Convert.ToString(dtProfile.Rows[0]["firstName"]);
                model.LastName = Convert.ToString(dtProfile.Rows[0]["lastName"]);
                model.UserName = Convert.ToString(dtProfile.Rows[0]["username"]);
                model.MobileNumber = Convert.ToString(dtProfile.Rows[0]["PhoneNumber"]);
                model.EmailId = Convert.ToString(dtProfile.Rows[0]["EmailId"]);
                model.DateofJoin = string.IsNullOrEmpty(dtProfile.Rows[0]["intakeId"].ToString()) ?Convert.ToDateTime("01-01-0001") : Convert.ToDateTime(dtProfile.Rows[0]["intakeId"]);
                //Convert.ToDateTime(dr["DateOfJoin"]);  //Convert.ToDateTime(dtProfile.Rows[0]["DateOfJoin"].ToString());
                //model.Role.Name = Convert.ToString(dtProfile.Rows[0]["RoleName"]);
                //model.AadharNumber = Convert.ToString(dtProfile.Rows[0]["Aadhar"]);
                //model.PANNumber = Convert.ToString(dtProfile.Rows[0]["PanNumber"]);
                //model.JurisdictionLevel = Convert.ToString(dtProfile.Rows[0]["Jurisdiction"]);
                //model.DistrictId = string.IsNullOrEmpty(dtProfile.Rows[0]["District"].ToString()) ? 0 : Convert.ToInt32(dtProfile.Rows[0]["District"]);
                model.CountryId = string.IsNullOrEmpty(dtProfile.Rows[0]["countryId"].ToString()) ? 0 : Convert.ToInt32(dtProfile.Rows[0]["countryId"]);
                model.CollegeId = string.IsNullOrEmpty(dtProfile.Rows[0]["collegeId"].ToString()) ? 0 : Convert.ToInt32(dtProfile.Rows[0]["collegeId"]);
                //model.HouseNo = Convert.ToString(dtProfile.Rows[0]["HouseNo"]);
                //model.StreetName = Convert.ToString(dtProfile.Rows[0]["StreetName"]);
                ////model.SecurityQuestion = Convert.ToString(dtProfile.Rows[0]["SecurityQuestion"]);
                //model.SecurityAnswer = Convert.ToString(dtProfile.Rows[0]["SecurityAnswer"]);
                model.UserPhoto= string.IsNullOrEmpty(dtProfile.Rows[0]["ImagePath"].ToString()) ? "" : dtProfile.Rows[0]["ImagePath"].ToString();
               
            }           
            return model;
        }

        public UserModel CheckForUserName(string UserName)
        {
            objDAL = new UserDAL();
            UserModel user = new UserModel();
            DataTable dt = objDAL.CheckForUserName(UserName);
            if (dt != null && dt.Rows.Count > 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                user.UserName = dt.Rows[0]["UserName"].ToString();
                user.OTP = dt.Rows[0]["OTP"].ToString();
                user.EmailId = dt.Rows[0]["EmailId"].ToString();
            }
            return user;
              
        }

        public bool ResetPassword(int Id, string Password)
        {
            objDAL = new UserDAL();
            string OTP = Utitlities.GenerateNumber();
            return objDAL.ResetPassword(Id, Password, OTP);
        }
        public bool VerifyUpdateMobileNumber(int Id, string MobileNumber,string username)
        {
            objDAL = new UserDAL();
            return objDAL.VerifyUpdateMobileNumber(Id, MobileNumber, username);
        }

        public int SaveProfileDetails(UserViewModel model, ref int userId)
        {
            objDAL = new UserDAL();
            return objDAL.SaveProfileDetails(model, ref userId);
        }

        public List<ApplicationModel> ApplicationTrack(string ApplicationNumber,Status status)
        {
            objDAL = new UserDAL();
            DataTable dtAppTrack = objDAL.ApplicationTrack(ApplicationNumber,status);
            if (dtAppTrack == null)
                return null;
            List<ApplicationModel> objList = new List<ApplicationModel>();
            ApplicationModel model = new ApplicationModel();
            model.usermodel = new UserModel();
            foreach (DataRow row in dtAppTrack.Rows)
            {
                model.ApplicationNumber = Convert.ToString(row["ApplicationNumber"]);
                model.usermodel.UserName = Convert.ToString(row["UserName"]);
                model.Status = (Status)Convert.ToInt32(row["StatusId"]);
                objList.Add(model);
            }
            return objList;
        }

        public List<ThirdPartyVerification> ThirdPartyVerification(int DistrictId, int MandalId, int VillageId, string LicenseNumber)
        {
            objDAL = new UserDAL();
            DataTable dtLicense = objDAL.ThirdPartyVerification(DistrictId, MandalId, VillageId, LicenseNumber);
            List<ThirdPartyVerification> objThirdPartyList = new List<ThirdPartyVerification>();
            if(dtLicense!=null && dtLicense.Rows.Count > 0)
            {
                
                foreach(DataRow dr in dtLicense.Rows)
                {
                    ThirdPartyVerification objThirdParty = new ThirdPartyVerification();
                    objThirdParty.Heading = dr["Heading"].ToString();
                    objThirdParty.Name = dr["Name"].ToString();
                    objThirdParty.LicenseNumber = dr["LicenseNumber"].ToString();
                    DateTime LicenseIssuedDate = DateTime.Parse(dr["LicenseIssuedDate"].ToString());
                    objThirdParty.LicenseIssuedDate = LicenseIssuedDate.ToString("dd-MM-yyyy");
                    DateTime LicenseExpiry = DateTime.Parse(dr["LicenseExpiryDate"].ToString());
                    objThirdParty.LicenseExpiryDate= LicenseExpiry.ToString("dd-MM-yyyy");
                    objThirdParty.DistrictId = dr["District"].ToString();
                    objThirdParty.MandalId = dr["Mandal"].ToString();
                    objThirdParty.VillageId = dr["Village"].ToString();
                    objThirdPartyList.Add(objThirdParty);
                }
                  
            }
            return objThirdPartyList;



            //if (dtLicense == null)
            //    return null;
            //ThirdPartyVerification model = new ThirdPartyVerification();
            //if (dtLicense.Rows.Count > 0)
            //{
            //    DataRow dr = dtLicense.Rows[0];
            //    model.Heading = dr["Heading"].ToString();
            //    if (model.Heading == "Hospital Name(APMCE Act)")
            //    {
            //        model.Name = dr["Name"].ToString();
            //    }
            //    else if (model.Heading == "Diagnostic(PCPNDT Act)")
            //    {
            //        model.Name = dr["Name"].ToString();
            //    }
            //    model.LicenseNumber = dr["LicenseNumber"].ToString();
            //    model.LicenseIssuedDate = string.IsNullOrEmpty(dr["LicenseIssuedDate"].ToString()) ? Convert.ToDateTime("01-01-0001") : Convert.ToDateTime(dr["LicenseIssuedDate"]);
            //    model.LicenseExpiryDate = string.IsNullOrEmpty(dr["LicenseExpiryDate"].ToString()) ? Convert.ToDateTime("01-01-0001") : Convert.ToDateTime(dr["LicenseExpiryDate"]);
            //    model.DistrictId = dr["District"].ToString();
            //    model.MandalId = dr["Mandal"].ToString();
            //    model.VillageId = dr["Village"].ToString();
            }
          
       

        #region Bio Capstone Application 
        public int SaveBioCapstoneDetails(BioCapstoneViewModel model )
        {
            return 1;
        }
        #endregion
        public string SendSMS(string message, string phoneNo, string userId, string smsType)
        {
            objDAL = new UserDAL();
            return objDAL.SendSMS(message, phoneNo, userId, smsType);
        }

        #region ExistingLicense
        public DataTable GetExistingLicenseDetails(string LicenseNumber)
        {
            objDAL = new UserDAL();
            DataTable dtExistingLicense = objDAL.GetExistingLicenseDetails(LicenseNumber);
           //ExistingLicense objExistingLicense = new ExistingLicense();
           // if (dtExistingLicense != null && dtExistingLicense.Rows.Count > 0)
           // {
           //     objExistingLicense.Name = dtExistingLicense.Rows[0]["Name"].ToString();
           //     objExistingLicense.LicenseNumber = dtExistingLicense.Rows[0]["ExistingLicenseNumber"].ToString();
           //     objExistingLicense.Address = dtExistingLicense.Rows[0]["Address"].ToString();
           //     objExistingLicense.Aadhar = dtExistingLicense.Rows[0]["Aadhar"].ToString();
           //     if (dtExistingLicense.Rows[0]["LicenseIssuedDate"] == DBNull.Value || dtExistingLicense.Rows[0]["LicenseIssuedDate"]=="")
           //         objExistingLicense.LicenseIssueDate = default(DateTime);
           //     else
           //     objExistingLicense.LicenseIssueDate = Convert.ToDateTime(dtExistingLicense.Rows[0]["LicenseIssuedDate"]);
           //     if (dtExistingLicense.Rows[0]["LicenseExpiryDate"] == DBNull.Value || dtExistingLicense.Rows[0]["LicenseIssuedDate"] == "")
           //         objExistingLicense.LicenseExpiryDate = default(DateTime);
           //     else
           //         objExistingLicense.LicenseExpiryDate = Convert.ToDateTime(dtExistingLicense.Rows[0]["LicenseExpiryDate"]);
           //     objExistingLicense.MobileNo = dtExistingLicense.Rows[0]["Mobile"].ToString();
           //     objExistingLicense.Email = dtExistingLicense.Rows[0]["Email"].ToString();
                

           // }
         return dtExistingLicense;



        }

        public ApplicationModel GetApplication(int TransactionId, Status status)
        {
            LicenseDAL objLicenseDAL = new LicenseDAL();
            ApplicationBAL objApplicationBAL;
            DataTable dtItems = objLicenseDAL.GetTransactions(TransactionId, status, "Transaction");
            if (dtItems == null)
                return null;

            ApplicationModel applicationModel = new ApplicationModel();
            //applicationModel.Id = applicationId;
            applicationModel.APMCEModel = null;
            applicationModel.PCPNDTModel = null;
            applicationModel.BloodBankModel = null;
            applicationModel.BloodBankForm27EModel = null;
            applicationModel.OrganTransplantModel = null;
            applicationModel.BioCapstoneModel = null;
            applicationModel.HomeopathyDrugStore = null;
            applicationModel.AllopathicDrugModel = null;
            applicationModel.ExistingPCPNDTModel = null;
            
            foreach (DataRow row in dtItems.Rows)
            {

                int _serviceId = Convert.ToInt32(row["ServiceId"]);
                int _transactionId = Convert.ToInt32(row["Id"]);
                string _type = row["TableName"].ToString();
                switch (_serviceId)
                {
                   
                    case 2: //PCPNDT Grant
                    
                        applicationModel.ExistingPCPNDTModel = GetPCPNDTData(_transactionId, _type);
                        break;
                     
                    default:
                        applicationModel.ExistingPCPNDTModel = null; // newly added by chandu
                        break;
                }
            }

            return applicationModel;

        }

        public ExistingPCPNDTViewModel GetPCPNDTData(int transactionId, string type)
        {
            try
            {
                LicenseDAL objLicenseDAL = new LicenseDAL();
                DataSet dsItems = objLicenseDAL.GetPCPNDTData(transactionId, type);
                if (dsItems == null)
                    return null;
                ExistingPCPNDTViewModel pcpndtModel = new ExistingPCPNDTViewModel();

                #region Applicant Details
                ExistingApplicantModel applicantModel = new ExistingApplicantModel();
                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[0].Rows[0];
                    applicantModel.Id = Convert.ToInt32(row["Id"]);
                    applicantModel.Name = row["Name"].ToString();
                    applicantModel.ApplicantRole = row["ApplicantRole"].ToString();
                    applicantModel.ApplicantRoleOther = row["ApplicantRoleOther"].ToString();
                    applicantModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    applicantModel.DistrictName = row["DistrictName"].ToString();
                    applicantModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    applicantModel.MandalName = row["MandalName"].ToString();
                    applicantModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    applicantModel.VillageName = row["VillageName"].ToString();
                    applicantModel.Aadhar = row["Aadhar"].ToString();
                    applicantModel.PAN = row["PAN"].ToString();
                    applicantModel.HouseNumber = row["HouseNumber"].ToString();
                    applicantModel.StreetName = row["StreetName"].ToString();
                    applicantModel.PINCode = row["PINCode"].ToString();
                    applicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);

                }
                #endregion

                #region Establishment Details - Not in Use
                //EstablishmentViewModel establishmentModel = new EstablishmentViewModel();
                //if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                //{
                //    DataRow row = dsItems.Tables[1].Rows[0];
                //    establishmentModel.Id = Convert.ToInt32(row["Id"]);
                //    establishmentModel.Name = row["Name"].ToString();
                //    establishmentModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                //    establishmentModel.DistrictName = row["DistrictName"].ToString();
                //    establishmentModel.MandalId = Convert.ToInt32(row["MandalId"]);
                //    establishmentModel.MandalName = row["MandalName"].ToString();
                //    establishmentModel.VillageId = Convert.ToInt32(row["VillageId"]);
                //    establishmentModel.VillageName = row["VillageName"].ToString();
                //    establishmentModel.HouseNumber = row["HouseNumber"].ToString();
                //    establishmentModel.StreetName = row["StreetName"].ToString();
                //    establishmentModel.PINCode = row["PINCode"].ToString();
                //    establishmentModel.AddressProofPath = row["AddressProofDocPath"].ToString();
                //    establishmentModel.BuildingLayoutPath = row["BuildingLayoutDocPath"].ToString();
                //    establishmentModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                //}
                //pcpndtModel.EstablishmentModel = establishmentModel;
                #endregion

                #region Facility Details
                ExistingFacilityModel facilityModel = new ExistingFacilityModel();
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[1].Rows[0];
                    facilityModel.Id = Convert.ToInt32(row["Id"]);
                    facilityModel.Name = Convert.ToString(row["Name"]);
                    facilityModel.Faclities = Convert.ToString(row["Facilities"]);
                    facilityModel.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    facilityModel.DistrictName = Convert.ToString(row["DistrictName"]);
                    facilityModel.MandalId = Convert.ToInt32(row["MandalId"]);
                    facilityModel.MandalName = Convert.ToString(row["MandalName"]);
                    facilityModel.VillageId = Convert.ToInt32(row["VillageId"]);
                    facilityModel.VillageName = Convert.ToString(row["VillageName"]);
                    facilityModel.HouseNumber = Convert.ToString(row["HouseNumber"]);
                    facilityModel.StreetName = Convert.ToString(row["StreetName"]);
                    facilityModel.Phone = Convert.ToString(row["Phone"]);
                    facilityModel.Email = Convert.ToString(row["Email"]);
                    facilityModel.Fax = Convert.ToString(row["Fax"]);
                    facilityModel.Telegraph = Convert.ToString(row["Telegraph"]);
                    facilityModel.Telex = Convert.ToString(row["Telex"]);
                    facilityModel.PINCode = Convert.ToString(row["PINCode"]);
                    facilityModel.AddressProofPath = Convert.ToString(row["AddressProofDocPath"]);
                    facilityModel.BuildingLayoutPath = Convert.ToString(row["BuildingLayoutDocPath"]);
                    facilityModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                pcpndtModel.FacilityModel = facilityModel;
                #endregion

                #region Tests Details
                ExistingTestsModel testsModel = new ExistingTestsModel();
                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[2].Rows[0];
                    testsModel.Id = Convert.ToInt32(row["Id"]);
                    testsModel.InvasiveTests = row["InvasiveTests"].ToString();
                    testsModel.NonInvasiveTests = row["NonInvasiveTests"].ToString();
                    testsModel.Remarks = row["Remarks"].ToString();
                    testsModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                #endregion

                #region Equipment Details
                List<ExistingEquipmentModel> equipmentList = new List<ExistingEquipmentModel>();
                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    ExistingEquipmentModel equipment;
                    foreach (DataRow row in dsItems.Tables[3].Rows)
                    {
                        equipment = new ExistingEquipmentModel();
                        equipment.Id = Convert.ToInt32(row["Id"]);
                        equipment.Name = row["Name"].ToString();
                        equipment.SerialNumber = row["SerialNumber"].ToString();
                        equipment.MachineModel = row["MachineModel"].ToString();
                        equipment.Make = row["Make"].ToString();
                        equipment.Type = row["Type"].ToString();
                        equipment.UploadedFilePath = row["UploadedFilePath"].ToString();
                        equipmentList.Add(equipment);
                    }
                }
                #endregion

                #region Facilities Details
                ExistingFacilitesModel facilitiesModel = new ExistingFacilitesModel();
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[4].Rows[0];
                    facilitiesModel.Id = Convert.ToInt32(row["Id"]);
                    facilitiesModel.Tests = row["Tests"].ToString();
                    facilitiesModel.Studies = row["Studies"].ToString();
                    facilitiesModel.Remarks = row["Remarks"].ToString();
                    facilitiesModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                #endregion

                #region Employee Details
                List<ExistingEmployeeModel> employeeList = new List<ExistingEmployeeModel>();
                if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
                {
                    ExistingEmployeeModel employee;
                    foreach (DataRow row in dsItems.Tables[5].Rows)
                    {
                        employee = new ExistingEmployeeModel();
                        employee.Id = Convert.ToInt32(row["Id"]);
                        employee.Name = row["Name"].ToString();
                        employee.DesignationId = Convert.ToInt32(row["DesignationId"]);
                        employee.DesignationName = Convert.ToString(row["DesignationName"]);
                        employee.Experience = row["Experience"].ToString();
                        employee.ExpYears = Convert.ToInt32(row["ExpYears"]);
                        employee.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                        employee.ExpDays = Convert.ToInt32(row["ExpDays"]);
                        employee.RegistrationNumber = row["RegistrationNumber"].ToString();
                        employee.UploadedFilePath = row["UploadedFilePath"].ToString();
                        employeeList.Add(employee);
                    }
                }
                #endregion

                #region Institution Details
                ExistingInstitutionModel institutionModel = new ExistingInstitutionModel();
                if (dsItems.Tables[6] != null && dsItems.Tables[6].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[6].Rows[0];
                    institutionModel.Id = Convert.ToInt32(row["Id"]);
                    institutionModel.OwnershipTypeId = Convert.ToInt32(row["OwnershipTypeId"]);
                    institutionModel.OwnershipTypeName = row["OwnershipTypeName"].ToString();
                    institutionModel.InstitutionTypeId = Convert.ToInt32(row["InstitutionTypeId"]);
                    institutionModel.InstitutionTypeName = row["InstitutionTypeName"].ToString();
                    institutionModel.TotalWorkArea = row["TotalWorkArea"] != DBNull.Value ?
                        Convert.ToDecimal(row["TotalWorkArea"]) : 0;
                    institutionModel.OwnershipOthers = row["OwnershipOther"].ToString();
                    institutionModel.InstitutionOthers = row["InstitutionOther"].ToString();
                    institutionModel.AffidavitDocPath = Convert.ToString(row["AffidavitDocPath"]);
                    institutionModel.ArticleDocPath = Convert.ToString(row["ArticleDocPath"]);
                    institutionModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);

                    if (dsItems.Tables[8] != null && dsItems.Tables[8].Rows.Count > 0)
                    {
                        institutionModel.StudyCertificateDocPaths = new List<DocumentUploadModel>();
                        DocumentUploadModel documentModel;
                        foreach (DataRow docRow in dsItems.Tables[8].Rows)
                        {
                            documentModel = new DocumentUploadModel();
                            documentModel.Id = Convert.ToInt32(docRow["Id"]);
                            documentModel.DocumentPath = Convert.ToString(docRow["DocumentPath"]);
                            institutionModel.StudyCertificateDocPaths.Add(documentModel);
                        }
                    }


                }
                #endregion

                #region Declaration Details
                ExistingDeclarationModel declarationModel = new ExistingDeclarationModel();
                if (dsItems.Tables[7] != null && dsItems.Tables[7].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[7].Rows[0];
                    declarationModel.Id = Convert.ToInt32(row["Id"]);
                    declarationModel.Name = row["Name"].ToString();
                    declarationModel.SonOf = row["SonOf"].ToString();
                    declarationModel.Age = row["Age"] != DBNull.Value ? Convert.ToInt32(row["Age"]) : 0;
                    declarationModel.ResidentOf = row["ResidentOf"].ToString();
                    declarationModel.Designation = row["Designation"].ToString();
                    declarationModel.Organization = row["Organization"].ToString();
                    declarationModel.Date = row["Date"] != DBNull.Value ? Convert.ToDateTime(row["Date"]) : default(DateTime);
                    declarationModel.Place = row["Place"].ToString();
                    declarationModel.Signature = row["Signature"].ToString();
                    declarationModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                }
                #endregion

                #region Other upload Details
                List<DocumentUploadModel> otheruploadList = new List<DocumentUploadModel>();
                //  pcpndtModel.DeclarationModel.OtherUploadsList = new List<DocumentUploadModel>();
                if (dsItems.Tables[11] != null && dsItems.Tables[11].Rows.Count > 0)
                {
                    DocumentUploadModel others;
                    foreach (DataRow row in dsItems.Tables[11].Rows)
                    {
                        others = new DocumentUploadModel();
                        others.Id = Convert.ToInt32(row["Id"]);
                        others.UploadType = row["UploadType"].ToString();
                        others.DocumentPath = row["DocumentPath"].ToString();
                        otheruploadList.Add(others);

                    }

                }
                #endregion

                #region Rejection Details
                if (dsItems.Tables[9] != null && dsItems.Tables[9].Rows.Count > 0)
                {
                    DataRow row = dsItems.Tables[9].Rows[0];
                    pcpndtModel.RejectionRemarks = row["RejectionRemarks"].ToString();
                }
                #endregion

                pcpndtModel.ApplicantModel = applicantModel;
                //pcpndtModel.EstablishmentModel = establishmentModel;
                pcpndtModel.FacilityModel = facilityModel;
                pcpndtModel.TestsModel = testsModel;
                pcpndtModel.EquipmentList = equipmentList;
                //pcpndtModel.DeclarationModel.OtherUploadsList = new List<DocumentUploadModel>();
                //  pcpndtModel.DeclarationModel.OtherUploadsList = otheruploadList;
                if (pcpndtModel.EquipmentList.Count > 0)
                    pcpndtModel.EquipmentModel.FormStatus = FormStatus.Completed;
                pcpndtModel.FacilitiesModel = facilitiesModel;
                pcpndtModel.EmployeeList = employeeList;
                if (pcpndtModel.EmployeeList.Count > 0)
                    pcpndtModel.EmployeeModel.FormStatus = FormStatus.Completed;
                pcpndtModel.InstitutionModel = institutionModel;
                pcpndtModel.DeclarationModel = declarationModel;
                pcpndtModel.DeclarationModel.OtherUploadsList = otheruploadList;
                pcpndtModel.TransactionId = transactionId;

                return pcpndtModel;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Siva, 12-12-2017
                return null;
            }
        }

        public bool saveExistingLicense(ExistingPCPNDTViewModel model)
        {
            objDAL = new UserDAL();
            return objDAL.SaveExistingLicense(model);
        }
        #endregion
    }
}
