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
        public List<DistrictModel> GetDistricts()
        {
            List<DistrictModel> DistrictList = new List<DistrictModel>();
            DataTable dt = objDAL.GetDistricts();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DistrictModel objDistrict = new DistrictModel();
                    objDistrict.Id = Convert.ToInt32(row["DistrictId"]);
                    objDistrict.Name = row["DistrictName"].ToString();
                    DistrictList.Add(objDistrict);
                }
            }
            return DistrictList;
        }
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

        //public List<ApplicationModel> ApplicationTrack(string ApplicationNumber,Status status)
        //{
        //    objDAL = new UserDAL();
        //    DataTable dtAppTrack = objDAL.ApplicationTrack(ApplicationNumber,status);
        //    if (dtAppTrack == null)
        //        return null;
        //    List<ApplicationModel> objList = new List<ApplicationModel>();
        //    ApplicationModel model = new ApplicationModel();
        //    model.usermodel = new UserModel();
        //    foreach (DataRow row in dtAppTrack.Rows)
        //    {
        //        model.ApplicationNumber = Convert.ToString(row["ApplicationNumber"]);
        //        model.usermodel.UserName = Convert.ToString(row["UserName"]);
        //        model.Status = (Status)Convert.ToInt32(row["StatusId"]);
        //        objList.Add(model);
        //    }
        //    return objList;
        //}

        //public List<ThirdPartyVerification> ThirdPartyVerification(int DistrictId, int MandalId, int VillageId, string LicenseNumber)
        //{
        //    objDAL = new UserDAL();
        //    DataTable dtLicense = objDAL.ThirdPartyVerification(DistrictId, MandalId, VillageId, LicenseNumber);
        //    List<ThirdPartyVerification> objThirdPartyList = new List<ThirdPartyVerification>();
        //    if(dtLicense!=null && dtLicense.Rows.Count > 0)
        //    {
                
        //        foreach(DataRow dr in dtLicense.Rows)
        //        {
        //            ThirdPartyVerification objThirdParty = new ThirdPartyVerification();
        //            objThirdParty.Heading = dr["Heading"].ToString();
        //            objThirdParty.Name = dr["Name"].ToString();
        //            objThirdParty.LicenseNumber = dr["LicenseNumber"].ToString();
        //            DateTime LicenseIssuedDate = DateTime.Parse(dr["LicenseIssuedDate"].ToString());
        //            objThirdParty.LicenseIssuedDate = LicenseIssuedDate.ToString("dd-MM-yyyy");
        //            DateTime LicenseExpiry = DateTime.Parse(dr["LicenseExpiryDate"].ToString());
        //            objThirdParty.LicenseExpiryDate= LicenseExpiry.ToString("dd-MM-yyyy");
        //            objThirdParty.DistrictId = dr["District"].ToString();
        //            objThirdParty.MandalId = dr["Mandal"].ToString();
        //            objThirdParty.VillageId = dr["Village"].ToString();
        //            objThirdPartyList.Add(objThirdParty);
        //        }
                  
        //    }
        //    return objThirdPartyList;



        //    //if (dtLicense == null)
        //    //    return null;
        //    //ThirdPartyVerification model = new ThirdPartyVerification();
        //    //if (dtLicense.Rows.Count > 0)
        //    //{
        //    //    DataRow dr = dtLicense.Rows[0];
        //    //    model.Heading = dr["Heading"].ToString();
        //    //    if (model.Heading == "Hospital Name(APMCE Act)")
        //    //    {
        //    //        model.Name = dr["Name"].ToString();
        //    //    }
        //    //    else if (model.Heading == "Diagnostic(PCPNDT Act)")
        //    //    {
        //    //        model.Name = dr["Name"].ToString();
        //    //    }
        //    //    model.LicenseNumber = dr["LicenseNumber"].ToString();
        //    //    model.LicenseIssuedDate = string.IsNullOrEmpty(dr["LicenseIssuedDate"].ToString()) ? Convert.ToDateTime("01-01-0001") : Convert.ToDateTime(dr["LicenseIssuedDate"]);
        //    //    model.LicenseExpiryDate = string.IsNullOrEmpty(dr["LicenseExpiryDate"].ToString()) ? Convert.ToDateTime("01-01-0001") : Convert.ToDateTime(dr["LicenseExpiryDate"]);
        //    //    model.DistrictId = dr["District"].ToString();
        //    //    model.MandalId = dr["Mandal"].ToString();
        //    //    model.VillageId = dr["Village"].ToString();
        //    }
          
       

        #region Bio Capstone Application 
        //public int SaveBioCapstoneDetails(BioCapstoneViewModel model )
        //{
        //    return 1;
        //}
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
         
        //public bool saveExistingLicense(ExistingPCPNDTViewModel model)
        //{
        //    objDAL = new UserDAL();
        //    return objDAL.SaveExistingLicense(model);
        //}
        #endregion
    }
}
