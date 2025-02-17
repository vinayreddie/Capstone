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
using Capstone.Framework;
using System.Security.Cryptography;

namespace Capstone.DAL
{
    public class UserDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        SqlCommand command;
        static System.Net.HttpWebRequest request;
        static System.IO.Stream dataStream;
        #endregion
        public UserDAL()
        {
            dbManager = new SqlServerDBManager();
        }

        public string GetSaltCode(string UserName)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserName", UserName);
                paramList.Add(param);
                param = new SqlParameter("@SaltCode",SqlDbType.NVarChar,10);
                param.Direction = ParameterDirection.Output;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("GetSaltCode", paramList);
                if (command != null)
                    return command.Parameters["@SaltCode"].Value.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 02-04-2017
                return null;
            }
        }
        public DataSet ValidateUser(LoginModel login)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@UserName", login.Username);
                paramList.Add(param);
                //login.Password = CalculateMD5Hash(login.Password);
                //var hashedPassword = Framework.Utitlities.GetHashedPassword(login.Password);
                param = new SqlParameter("@Password", login.Password);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet("ValidateUser", paramList);
            }
            catch (Exception ex)
            {
                 return null;
            }
        }

        public int SaveUserRegistratinDetails(UserModel regDetails)
        {
            try
            {
                //int tranID = 0;
                paramList = new List<SqlParameter>();
               // param = new SqlParameter("@User_ID", tranID);
               // param.Direction = parameterDirection.InputOutput;
               // paramList.Add(param);
                param = new SqlParameter("@FirstName", regDetails.FirstName);
                paramList.Add(param);
                param = new SqlParameter("@LastName", regDetails.LastName);
                paramList.Add(param);
                param = new SqlParameter("@Address", regDetails.HouseNo + "-" + regDetails.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@MobileNumber", regDetails.MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@AadhaarNumber", regDetails.AadharNumber);
                paramList.Add(param);
                param = new SqlParameter("@DistrictID", regDetails.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalID", regDetails.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageID", regDetails.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@EmailID", regDetails.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@PANNumber", regDetails.PANNumber);
                paramList.Add(param);
                param = new SqlParameter("@Password", regDetails.PANNumber);
                paramList.Add(param);
                param = new SqlParameter("@UserName", regDetails.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedBy", regDetails.CreatedUserId);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("Sp_Save_UserRegistrationDetails", paramList);

                if (cmd != null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError("Inserting  error", "Inserting Dupplicate ");
                // JScript.ShowAlert("Error in User registration");
                throw ex;
            }
        }
        public string CalculateMD5Hash(string input)
        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString());

            }

            return sb.ToString();

        }
        public DataTable GetDistricts()
        {
            try
            {
                return dbManager.ExecuteStoredProc("GetDistricts");
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public DataTable GetMandalsByDistrictId(int DistrictId)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetMandals", paramList);

            }
            catch (Exception ex)
            {
                //TODO -Write Exception Log siva, -10-04-2017
                return null;
            }
        }

        public DataTable GetVillagesByMandalId(int MandalId)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@MandalId", MandalId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetVillages", paramList);

            }
            catch (Exception ex)
            {
                //TODO -Write Exception Log siva, -10-04-2017
                return null;
            }
        }
        public int SaveUserRegistratin(UserModel regDetails)
        {
            paramList = new List<SqlParameter>();
            dbManager = new SqlServerDBManager();
            try
            {
               
               
                int tranID = 0;
                param = new SqlParameter("@UserID", tranID);
                param.Direction = ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@FirstName", regDetails.FirstName.ToUpper());
                paramList.Add(param);
                //param = new SqlParameter("@LastName", regDetails.LastName);
                //paramList.Add(param);
                param = new SqlParameter("@HouseNo", regDetails.HouseNo );
                paramList.Add(param);
                param = new SqlParameter("@StreetName", regDetails.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@MobileNumber", regDetails.MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", regDetails.AadharNumber);
                paramList.Add(param);
                param = new SqlParameter("@DistrictID", regDetails.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalID", regDetails.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageID", regDetails.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@EmailID", regDetails.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@PANNumber", regDetails.PANNumber.ToUpper());
                paramList.Add(param);
                //Cancatenate Unique Code
                // string SaltCode= Utitlities.GenerateUniqueCode();
                //regDetails.Password = regDetails.Password + SaltCode;
                //regDetails.Password = CalculateMD5Hash(regDetails.Password);
                //param = new SqlParameter("@Password", regDetails.Password);//"202cb962ac59075b964b07152d234b70"

                string SaltCode = Utitlities.GenerateUniqueCode();
                regDetails.Password = Framework.Utitlities.GetHashedPassword(regDetails.Password);
                param = new SqlParameter("@Password", regDetails.Password);
                paramList.Add(param);
                param = new SqlParameter("@SaltCode",SaltCode);
                paramList.Add(param);
                param = new SqlParameter("@UserName", regDetails.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@RoleId", 5);
                paramList.Add(param);
               
                SqlCommand cmd = dbManager.ExecuteProcedure("SP_SaveUserRegistration", paramList);

                if (cmd != null)
                {
                    //return true;

                    return Convert.ToInt32(cmd.Parameters["@UserID"].Value.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError("Inserting  error", "Inserting Dupplicate ");
                // JScript.ShowAlert("Error in User registration");
                throw ex;
            }
        }

        public int SaveContact(ContactModel objuser)
        {
            paramList = new List<SqlParameter>();
            dbManager = new SqlServerDBManager();
            try
            {


                int tranID = 0;
                //param = new SqlParameter("@UserID", tranID);
                //param.Direction = ParameterDirection.InputOutput;
                //paramList.Add(param);
                param = new SqlParameter("@FirstName", objuser.FirstName.ToUpper());
                paramList.Add(param);
                //param = new SqlParameter("@LastName", regDetails.LastName);
                //paramList.Add(param);
                param = new SqlParameter("@LastName", objuser.LastName);
                paramList.Add(param);
                param = new SqlParameter("@PhoneNumber", objuser.MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@EmailID", objuser.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@CollegeID", objuser.CollegeId);
                paramList.Add(param);
                param = new SqlParameter("@Query", objuser.Query);
                paramList.Add(param);
              
                

               

                SqlCommand cmd = dbManager.ExecuteProcedure("SaveUpdateContact", paramList);

                if (cmd != null)
                {
                    //return true;
                    return 1;
                    //return Convert.ToInt32(cmd.Parameters["@UserID"].Value.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError("Inserting  error", "Inserting Dupplicate ");
                // JScript.ShowAlert("Error in User registration");
                throw ex;
            }
        }



        public bool updateUserToTAMCEapplication(int existingApplicationTransId, int newUserId)
        {
            paramList = new List<SqlParameter>();
            dbManager = new SqlServerDBManager();
            try
            {
                param = new SqlParameter("@ExistingApplicationTransId", existingApplicationTransId);
                paramList.Add(param);
                param = new SqlParameter("@UserId", newUserId);
                paramList.Add(param);

                SqlCommand cmd = dbManager.ExecuteProcedure("UpdateUserToExistingTAMCEapplication", paramList);

                if (cmd != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError("Inserting  error", "Inserting Dupplicate ");
                // JScript.ShowAlert("Error in User registration");
                throw ex;
            }
        }
        public string  ValidateUser(UserModel objuser)
        {
            DataTable dt = new DataTable();
            
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@EmailID", objuser.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@Aadhar", objuser.AadharNumber);
                paramList.Add(param);
                param = new SqlParameter("@result", SqlDbType.VarChar,500);
                //param.IsNullable = true;
               // param.Direction = ParameterDirection.InputOutput;
                param.Direction = ParameterDirection.Output;
                //param.Size = 88;
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("ValidateUserId", paramList);
                if (command != null)
                {
                    //emailid = command.Parameters["@EmailID"].Value.ToString();
                    //aadhar = command.Parameters["@Aadhar"].Value.ToString();
                    return (string)command.Parameters["@result"].Value;
                }
                else
                    return "";
                        
            }
            catch(Exception ex)
            {
                // TODO: Write exception log            - kishore, 31-05-2017
                throw ex;
            }
        }

        public DataTable ForgetPassword(string EmailId)  //int UserId,
        {
            paramList = new List<SqlParameter>();
            try
            {
                //param = new SqlParameter("@UserId", UserId);
                ////param.Direction = System.Data.ParameterDirection.InputOutput;
                //paramList.Add(param);
                param = new SqlParameter("@EmailID", EmailId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("ForgetPassword", paramList);
            }
            catch (Exception ex)
            {
                // TODO Write exception log    -- Jai, 04-08-2017
                return null;
            }
        }
        #region AddUSer
        //public DataTable CheckUserExistOrNot(UserModel User)
        //{
        //    paramList = new List<SqlParameter>();
        //    try
        //    {
        //        param = new SqlParameter("@JurisdictionId", User.JurisdictionId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@DistrictId", User.DistrictId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@MandalId", User.MandalId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@VillageId", User.VillageId);
        //        paramList.Add(param);
        //        param = new SqlParameter("@DesignationId", User.Designation);
        //        paramList.Add(param);

        //        return dbManager.ExecuteStoredProc("CheckWhetherUserExistOrNot", paramList);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        public bool AddUser(UserModel user)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@UserId", user.Id);
                paramList.Add(param);
                param = new SqlParameter("@FirstName", user.FirstName);
                paramList.Add(param);
                param = new SqlParameter("@LastName", user.LastName);
                paramList.Add(param);               
                param = new SqlParameter("@DesignationId", user.DesignationId);
                paramList.Add(param);               
                param = new SqlParameter("@DepartmentId", user.DepartmentId);
                paramList.Add(param);
                param = new SqlParameter("@JurisdictionId", user.JurisdictionId);
                paramList.Add(param);
                param = new SqlParameter("@DistrictId", user.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@MandalId", user.MandalId == 0 ? (object)DBNull.Value : user.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@VillageId", user.VillageId == 0 ? (object)DBNull.Value : user.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@EmailId", user.EmailId);
                paramList.Add(param);
                param = new SqlParameter("@MobileNumber", user.MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@CreatedBy", user.CreatedUserId);
                paramList.Add(param);
                //Cancatenate Unique Code
                string SaltCode = Utitlities.GenerateUniqueCode();
                user.Password = "5750548dc79d8276ade369c7ed17b9f7" + SaltCode;
               // user.Password = CalculateMD5Hash(user.Password);
                param = new SqlParameter("@Password", user.Password);
                paramList.Add(param);
                param = new SqlParameter("@SaltCode", SaltCode);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("SaveDeptAdminUser", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Profile Details
        public DataTable ProfileDetails(int UserId)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@UserId", UserId);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetEditProfile", paramList);
            }
            catch (Exception ex)
            {
                // TODO Write exception log    -- Jai, 28-07-2017
                return null;
            }
        }

        public int SaveProfileDetails(UserViewModel model, ref int userId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                //param = new SqlParameter("@RoleId", model.Role.Id);
                //paramList.Add(param);               
                param = new SqlParameter("@UserId", model.Id);
                param.Direction = System.Data.ParameterDirection.InputOutput;
                paramList.Add(param);
                param = new SqlParameter("@FirstName", model.FirstName);
                paramList.Add(param);
                param = new SqlParameter("@LastName", model.LastName);
                paramList.Add(param);
                param = new SqlParameter("@MobileNumber", model.MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@EmailId", model.EmailId);
                paramList.Add(param);

                if (model.Role.Id != 5)
                {
                    param = new SqlParameter("@Intake",  model.DateofJoin.ToString("dd-MMM-yyyy"));
                    paramList.Add(param);
                }
                else
                {
                    param = new SqlParameter("@Intake", model.DateofJoin.ToString());
                    paramList.Add(param);

                }
                //param = new SqlParameter("@Aadhar", model.AadharNumber);
                //paramList.Add(param);
                //param = new SqlParameter("@PanNumber", model.PANNumber);
                //paramList.Add(param);
                param = new SqlParameter("@CountryId", model.CountryId);
                paramList.Add(param);
                param = new SqlParameter("@CollegeId", model.CollegeId);
                paramList.Add(param);
               
                //param = new SqlParameter("@SecurityQuestion", model.SecurityQuestion);
                //paramList.Add(param);
                //param = new SqlParameter("@SecurityAnswer", model.SecurityAnswer);
                //paramList.Add(param);
               
                param = new SqlParameter("@ImagePath", model.UserPhoto);
                paramList.Add(param);
                command = dbManager.ExecuteProcedure("UpdateProfile", paramList);
                if (command != null)
                {
                    userId = Convert.ToInt32(command.Parameters["@UserId"].Value);
                    return Convert.ToInt32(command.Parameters["@UserId"].Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - jai, 29-07-2017
                return -1;
            }
        }
        #endregion

        #region Application Track
        public DataTable ApplicationTrack(string ApplicationNumber, Status status)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@ApplicationNumber", ApplicationNumber);
                paramList.Add(param);
                param = new SqlParameter("@StatusId", Convert.ToInt32(status));
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetApplicationTrack", paramList);
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - jai, 09-08-2017
                return null;
            }
        }
        #endregion
        #region Third Party Verification
        public DataTable ThirdPartyVerification(int DistrictId, int MandalId, int VillageId, string LicenseNumber)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@DistrictId", DistrictId);
                paramList.Add(param);
                param = new SqlParameter("MandalId", MandalId);
                paramList.Add(param);
                param = new SqlParameter("VillageId", VillageId);
                paramList.Add(param);
                param = new SqlParameter("@LicenseNumber", LicenseNumber==""?(object)DBNull.Value:LicenseNumber);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetThirdPartyVerificationDetails", paramList);
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - jai, 10-08-2017
                return null;
            }
        }
        #endregion

        public DataTable CheckForUserName(string UserName)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@UserName", UserName);
                paramList.Add(param);
                
                return dbManager.ExecuteStoredProc("CheckForUserName", paramList);
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - siva katta, 07-03-2018
                return null;
            }
        }

        public bool ResetPassword(int Id, string Password, string OTP)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@Id", Id);
                paramList.Add(param);
                //var hashedPassword = Framework.Utitlities.GetHashedPassword(Password);
                param = new SqlParameter("@Password", Password);
                paramList.Add(param);
                //param = new SqlParameter("@OTP", OTP);
                //paramList.Add(param);
                //param = new SqlParameter("@SaltCode", hashedPassword);
                //paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("ResetPassword", paramList);

                if (cmd != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - siva katta, 07-03-2018
                return false;
            }
        }

        public bool VerifyUpdateMobileNumber(int Id, string MobileNumber, string userName)
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new SqlParameter("@Id", Id);
                paramList.Add(param);
                param = new SqlParameter("@MobileNumber", MobileNumber);
                paramList.Add(param);
                param = new SqlParameter("@UserName", userName);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("VerifyUpdateMobileNumber", paramList);

                if (cmd != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string SendSMS(string message, string phoneNo, string userId, string smsType)
        {
            string strresult = "false";
            //generate  Sequence_NO
            // long seqno = GetNext();
            //  strSecretCode = "MS" + seqno.Tostring();
            try
            {

                //Dear sir/madam, payment of rs<<.*>>received against bill no<<.*>>vide transaction number<<.*>>thank you for utilizing meeseva services.
                //insert
                //  if (InsertMsgLog(message, phoneNo, userId))
                // {
                request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");// http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                request.ProtocolVersion = HttpVersion.Version10;
                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                Console.WriteLine("Before Calling Method");

                string strResponse = "";
                if (smsType == "single")
                    strResponse = sendSingleSMS(phoneNo, message);
                //else if (smsType == "bulk")
                //    strResponse = sendBulkSMS(phoneNo, message);
                //else if (smsType == "unicode")
                //    strResponse = sendSingleUicodeSMS(phoneNo, message);


                if (strResponse == "OK")
                {
                    //lblMsg.Text = "SMS sent Succesfully";
                    //update to Y
                    //if (updateTransaction(strSecretCode, strSecretCode, strResponse, userId))
                    strresult = "Success";
                    // else
                    //    strresult = "Problem in updating";
                }
                else
                    strresult = strResponse;
                // }
                // else
                //  strresult = "Problem in inserting";
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return strresult;
        }
        public string sendSingleSMS(string mobileNo, string message)
        {
            string username = "MEESEVA";
            string password = "1qaz!QAZ";
            string senderid = "MESEVA";
            string smsservicetype = "singlemsg"; //For single message.
            string query = "username=" + HttpUtility.UrlEncode(username) +
                "&password=" + HttpUtility.UrlEncode(password) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(message) +
                "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
                "&senderid=" + HttpUtility.UrlEncode(senderid);

            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            string Status = ((HttpWebResponse)response).StatusDescription;
            //dataStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream);
            //string responseFromServer = reader.ReadToEnd();
            //reader.Close();
            dataStream.Close();
            response.Close();
            return Status;
        }

        #region ExistingLicense
        public DataTable GetExistingLicenseDetails(string LicenseNumber)
        {
            paramList = new List<SqlParameter>();
            try
            {
               
                param = new SqlParameter("@LicenseNumber", LicenseNumber);
                paramList.Add(param);
                return dbManager.ExecuteStoredProc("GetExistingLicenseDetails", paramList);
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - siva, 05-12-2017
                return null;
            }
        }

        public bool SaveExistingLicense(ExistingPCPNDTViewModel model)
        {
            paramList = new List<SqlParameter>();
            dbManager = new SqlServerDBManager();
            try
            {

                //Applicant
                param = new SqlParameter("@ApplicationId", model.ApplicationId);
                paramList.Add(param);
                param = new SqlParameter("@TransactionId", model.TransactionId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantId", model.ApplicantModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantName", model.ApplicantModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRole", model.ApplicantModel.ApplicantRole);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantRoleOther", model.ApplicantModel.ApplicantRoleOther);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantDistrictID", model.ApplicantModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantMandalID", model.ApplicantModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantVillageID", model.ApplicantModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantAadhar", model.ApplicantModel.Aadhar);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantPAN", model.ApplicantModel.PAN);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantHouseNumber", model.ApplicantModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantStreetName", model.ApplicantModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantPINCode", model.ApplicantModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantMobile", model.ApplicantModel.MobileNo);
                paramList.Add(param);
                param = new SqlParameter("@ApplicantEmail", model.ApplicantModel.Email);
                paramList.Add(param);

                //Facility Registration
                param = new SqlParameter("@FacilityId", model.FacilityModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@Facilities", model.FacilityModel.Faclities);
                paramList.Add(param);
                param = new SqlParameter("@FacilityName", model.FacilityModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@FacilityDistrictId", model.FacilityModel.DistrictId);
                paramList.Add(param);
                param = new SqlParameter("@FacilityMandalId", model.FacilityModel.MandalId);
                paramList.Add(param);
                param = new SqlParameter("@FacilityVillageId", model.FacilityModel.VillageId);
                paramList.Add(param);
                param = new SqlParameter("@FacilityHouseNumber", model.FacilityModel.HouseNumber);
                paramList.Add(param);
                param = new SqlParameter("@FacilityStreetName", model.FacilityModel.StreetName);
                paramList.Add(param);
                param = new SqlParameter("@FacilityPhone", model.FacilityModel.Phone);
                paramList.Add(param); 
                param = new SqlParameter("@FacilityEmail", model.FacilityModel.Email);
                paramList.Add(param);
                param = new SqlParameter("@FacilityFax", model.FacilityModel.Fax);
                paramList.Add(param);
                param = new SqlParameter("@FacilityTelegrah", model.FacilityModel.Telegraph);
                paramList.Add(param);
                param = new SqlParameter("@FacilityTelex", model.FacilityModel.Telex);
                paramList.Add(param);
                param = new SqlParameter("@FacilityPINCode", model.FacilityModel.PINCode);
                paramList.Add(param);
                param = new SqlParameter("@FacilityAddressProofDocPath", model.FacilityModel.AddressProofPath);
                paramList.Add(param);
                param = new SqlParameter("@FacilityBuildingLayoutDocPath", model.FacilityModel.BuildingLayoutPath);
                paramList.Add(param);

                //Tests
                param = new SqlParameter("@TestsId", model.TestsModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@InvasiveTests", model.TestsModel.InvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@NonInvasiveTests", model.TestsModel.NonInvasiveTests);
                paramList.Add(param);
                param = new SqlParameter("@TestRemarks", model.FacilityModel.DistrictId);
                paramList.Add(param);

                //Equipment List
                param = new SqlParameter("@EquipmentList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.EquipmentList);
                paramList.Add(param);

                //Facilities
                param = new SqlParameter("@FacilitiesId", model.FacilitiesModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@Tests", model.FacilitiesModel.Tests);
                paramList.Add(param);
                param = new SqlParameter("@Studies", model.FacilitiesModel.Studies);
                paramList.Add(param);
                param = new SqlParameter("@FacilitiesRemarks", model.FacilitiesModel.Remarks);
                paramList.Add(param);

                //Employee List
                param = new SqlParameter("@EmployeeList", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.EmployeeList);
                paramList.Add(param);

                //Institution
                param = new SqlParameter("@InstitutionId", model.InstitutionModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipTypeId", model.InstitutionModel.OwnershipTypeId);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionTypeId", model.InstitutionModel.InstitutionTypeId);
                paramList.Add(param);
                param = new SqlParameter("@TotalWorkArea", model.InstitutionModel.TotalWorkArea);
                paramList.Add(param);
                param = new SqlParameter("@AffidavitDocPath", model.InstitutionModel.AffidavitDocPath);
                paramList.Add(param);
                param = new SqlParameter("@ArticleDocPath", model.InstitutionModel.ArticleDocPath);
                paramList.Add(param);
                param = new SqlParameter("@StudyCertificates", System.Data.SqlDbType.Structured);
                param.Value = Utitlities.ConvertToDataTable(model.InstitutionModel.StudyCertificateDocPaths);
                paramList.Add(param);
                param = new SqlParameter("@OwnershipOthers", model.InstitutionModel.OwnershipOthers);
                paramList.Add(param);
                param = new SqlParameter("@InstitutionOthers", model.InstitutionModel.InstitutionOthers);
                paramList.Add(param);

                //Declaration
                param = new SqlParameter("@DeclarationId", model.DeclarationModel.Id);
                paramList.Add(param);
                param = new SqlParameter("@DeclarationName", model.DeclarationModel.Name);
                paramList.Add(param);
                param = new SqlParameter("@SonOf", model.DeclarationModel.SonOf);
                paramList.Add(param);
                param = new SqlParameter("@Age", model.DeclarationModel.Age);
                paramList.Add(param);
                param = new SqlParameter("@ResidentOf", model.DeclarationModel.ResidentOf);
                paramList.Add(param);
                param = new SqlParameter("@Designation", model.DeclarationModel.Designation);
                paramList.Add(param);
                param = new SqlParameter("@Organization", model.DeclarationModel.Organization);
                paramList.Add(param);
                param = new SqlParameter("@Date", model.DeclarationModel.Date);
                paramList.Add(param);
                param = new SqlParameter("@Place", model.DeclarationModel.Place);
                paramList.Add(param);
                param = new SqlParameter("@Signature", model.DeclarationModel.Signature);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("SaveExistingLicense", paramList);

                if (cmd != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError("Inserting  error", "Inserting Dupplicate ");
                // JScript.ShowAlert("Error in User registration");
                throw ex;
            }
        }



        #endregion
    }
}
