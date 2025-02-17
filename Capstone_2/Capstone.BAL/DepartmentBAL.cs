using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.BAL
{
    public class DepartmentBAL : MasterBAL
    {
        DepartmentDAL departmentDAL;

        public bool SaveDepartmentandAdmin(DepartmentAdminViewModel model)
        {
            departmentDAL = new DepartmentDAL();
            bool result = departmentDAL.SaveDepartmentandAdmin(model);
            //if (dtItems == null)
            //    return null;

            // return ConverttoList(dtItems);
            return result;
        }

        public List<DepartmentAdminViewModel> GetDepartmentandAdmins()
        {
            departmentDAL = new DepartmentDAL();
            DataTable dtItems = departmentDAL.GetDepartmentandAdmins();
            if (dtItems == null)
                return null;

            return ConverttoList(dtItems);
        }

        private List<DepartmentAdminViewModel> ConverttoList(DataTable dtItems)
        {
            try
            {
                List<DepartmentAdminViewModel> objList = new List<DepartmentAdminViewModel>();
                DepartmentAdminViewModel department = new DepartmentAdminViewModel();
                foreach (DataRow row in dtItems.Rows)
                {
                    department = new DepartmentAdminViewModel();
                    department.Department.Id = Convert.ToInt32(row["DepartmentId"]);
                    department.Department.Name = row["DepartmentName"].ToString();
                    department.Department.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    department.Department.MandalId = Convert.ToInt32(row["MandalId"]);
                    department.Department.VillageId = Convert.ToInt32(row["VillageId"]);
                    department.Department.StreetName = row["StreetName"].ToString();
                    department.Department.HouseNumber = row["HouseNo"].ToString();
                    department.Department.PinCode = row["Pincode"].ToString();
                    department.Department.Logo = row["LogoPath"].ToString();
                    department.Department.DistrictName = row["DistrictName"].ToString();
                    department.Department.MandalName = row["MandalName"].ToString();
                    department.Department.VillageName = row["VillageName"].ToString();
                    department.DepartmentAdmin.FirstName = row["Admin"].ToString();
                    department.DepartmentAdmin.UserName = row["UserName"].ToString();
                    department.DepartmentAdmin.Id = Convert.ToInt32(row["DeptAdminId"]);
                    department.DepartmentAdmin.AadharNumber = row["Aadhar"].ToString();
                    department.DepartmentAdmin.MobileNumber = row["MobileNumber"].ToString();
                    department.DepartmentAdmin.EmailId = row["EmailId"].ToString();
                    department.DepartmentAdmin.SecurityQuestion = row["SecurityQuestion"].ToString();
                    department.DepartmentAdmin.SecurityAnswer = row["SecurityAnswer"].ToString();
                    //department.DepartmentAdmin.DesginationName = row["Designation"].ToString();
                    objList.Add(department);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       - Raj, 07-05-2017
                return null;
            }
        }

        #region TAMCE Actions
        public List<GraphModel> GetCommissionerTAMCEData(DateTime? FromDate, DateTime? ToDate)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerTAMCEData(FromDate,ToDate);
            return ConvertToList(dt);
        }

        public DataTable CommissionerDashboardServicewiseTAMCE(int statusId, int DistrictId, int MandalId, int VillageId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerDashboardForTAMCE(statusId, DistrictId, MandalId, VillageId);
            return dt; //ConvertToDistrictList(dt);

        }
        public DataTable GetCommissionerTAMCEApplications(int StatusId, int VillageId, DateTime? FromDate, DateTime? ToDate)
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.GetCommissionerTAMCEApplications(StatusId, VillageId, FromDate, ToDate);

        }

        public List<GraphModel> GetCommissionerLicensedApplicationsForTAMCE(string Type)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerLicensedApplications(Type);
            return ConvertToList(dt);
        }

        #endregion

        public List<GraphModel> GetCommissionerPCPNDTData()
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt= departmentDAL.GetCommissionerPCPNDTData();
            return ConvertToList(dt);
        }
        public List<GraphModel> ConvertToList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.label = row["label"].ToString();
                    objGraph.value =Convert.ToInt32(row["value"].ToString());
                    objGraph.StatusId= Convert.ToInt32(row["StatusId"].ToString());
                    //objGraph.Type = row["Type"].ToString();
                    //objGraph.ServiceId= Convert.ToInt32(row["ServiceId"].ToString());
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }
        public DataTable CommissionerDashboardServicewise(int statusId, int DistrictId, int MandalId, int VillageId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerDashboard(statusId, DistrictId, MandalId, VillageId,FromDate,ToDate);
            return dt; //ConvertToDistrictList(dt);

        }
        public DataTable GetDistrictWiseLicensedApplications(string Type,string ApplicationType)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetDistrictWiseLicensedApplications(Type,ApplicationType);
            return dt; //ConvertToDistrictList(dt);

        }
        public DataTable GetCommissionerPCPNDTApplications(int StatusId, int VillageId)
        {
            departmentDAL = new DepartmentDAL();
           return departmentDAL.GetCommissionerPCPNDTApplications(StatusId, VillageId);
          
        }
        #region Licensed Applications
        public DataTable GetCommissionerLicensedApplications(string Type)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerLicensedApplications(Type);
            return dt; //ConvertToDistrictList(dt);
        }
        public DataTable GetCumulativeLicensedApplications(int serviceId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCumulativeLicensedApplications(serviceId);
            return dt; //ConvertToDistrictList(dt);
        }
        public DataTable GetCumulativeLicensedApplicationsView(int serviceId, int districtId, int mandalId, int villageId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCumulativeLicensedApplicationsView( serviceId, districtId, mandalId,villageId);
            return dt; //ConvertToDistrictList(dt);
        }

        public DataTable GetCommissionerApprovedAmendments(string Type, int DistrictId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerApprovedAmendments(Type,DistrictId);
            return dt; //ConvertToDistrictList(dt);
        }
        public DataTable GetCommissionerApprovedRegistrations(string Type, int DistrictId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerApprovedRegistrations(Type,DistrictId);
            return dt; //ConvertToDistrictList(dt);
        }
        #endregion
        #region Pending Applications
        public DataTable GetPendingApplications(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetPendingApplications(FromDate,ToDate);
            return dt; 
        }
        public DataTable GetDistrictWisePendingApplicationsByDeptUser(int DeptUserId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetDistrictWisePendingApplicationsByDeptUser(DeptUserId,FromDate,ToDate);
            return dt;
        }
        public DataTable GetCommissionerPCPNDTPendingApplications(int deptUserId, int DistrictId, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.GetCommissionerPCPNDTPendingApplications(deptUserId, DistrictId, FromDate, ToDate);

        }
        public List<GraphModel> GetPendingApplicationsDistrictwise(int districtId, int serviceId, int mandalId, int villageId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetPendingApplicationsDistrictwise(districtId, serviceId, mandalId, villageId);
            if (dt.Rows[0]["Type1"].ToString() == "District")
            {
                return DistrictWisePendingList(dt);
            }
            else if (dt.Rows[0]["Type1"].ToString() == "Mandal")
            {
                return MandalWisePendingList(dt);
            }
            else if (dt.Rows[0]["Type1"].ToString() == "Village")
            {
                return VillageWisePendingList(dt);
            }
            return null;
        }
        public List<GraphModel> DistrictWisePendingList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    objGraph.DistrictName = row["DistrictName"].ToString();
                    objGraph.value = Convert.ToInt32(row["Value"].ToString());
                    objGraph.Type = row["Type1"].ToString();
                    objGraph.ServiceId = Convert.ToInt32(row["ServiceId"].ToString());
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }

        public List<GraphModel> MandalWisePendingList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    objGraph.DistrictName = row["DistrictName"].ToString();
                    objGraph.MandalId = Convert.ToInt32(row["MandalId"]);
                    objGraph.MandalName = row["MandalName"].ToString();
                    objGraph.value = Convert.ToInt32(row["Value"].ToString());
                    objGraph.Type = row["Type1"].ToString();
                    objGraph.ServiceId = Convert.ToInt32(row["ServiceId"].ToString());
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }

        public List<GraphModel> VillageWisePendingList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    objGraph.DistrictName = row["DistrictName"].ToString();
                    objGraph.MandalId = Convert.ToInt32(row["MandalId"]);
                    objGraph.MandalName = row["MandalName"].ToString();
                    objGraph.VillageId = Convert.ToInt32(row["VillageId"]);
                    objGraph.VillageName = row["VillageName"].ToString();
                    objGraph.value = Convert.ToInt32(row["Value"].ToString());
                    objGraph.ServiceId = Convert.ToInt32(row["ServiceId"].ToString());
                    // objGraph.Type = row["Type"].ToString();
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }
        public DataTable GetPendingApplications(int serviceId, int districtId, int mandalId, int villageId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetPendingApplicationsView(serviceId, districtId,mandalId,villageId);
            return dt; //ConvertToDistrictList(dt);
        }
        #endregion 

        //public List<GraphModel> ConvertToDistrictList(DataSet dt)
        //{
        //    try
        //    {
        //        List<GraphModel> objList = new List<GraphModel>();
        //        GraphModel objGraph = new GraphModel();
        //        foreach (DataRow row in dt.Rows.Count>0)
        //        {
        //            objGraph = new GraphModel();
        //            objGraph.DistrictId =Convert.ToInt32(row["DistrictId"].ToString());
        //            objGraph.label = row["DistrictName"].ToString();
        //            objGraph.value = Convert.ToInt32(row["Value"].ToString());
        //            objGraph.Type = row["Type1"].ToString();
        //            //objGraph.Type = row["Type"].ToString();
        //           // objGraph.ServiceId = Convert.ToInt32(row["ServiceId"].ToString());
        //            objList.Add(objGraph);
        //        }

        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log.       -- kishore  25-10-2017
        //        return null;
        //    }
        //} 


        public List<GraphModel> GetCommissionerDashboardDetails(int districtId, int serviceId, int mandalId, int villageId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetCommissionerDashboardDetails(districtId, serviceId, mandalId,villageId);
            if (dt.Rows[0]["Type1"].ToString() == "District" )
            {
                return DistrictWiseList(dt);
            }
            else if(dt.Rows[0]["Type1"].ToString()== "Mandal")
            {
                return MandalWiseList(dt);
            }
            else if(dt.Rows[0]["Type1"].ToString()=="Village")
                    {
                return VillageWiseList(dt);
            }

            return null;
        }

      
        public List<GraphModel> DistrictWiseList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    objGraph.DistrictName = row["DistrictName"].ToString();
                    objGraph.value = Convert.ToInt32(row["Value"].ToString());
                    objGraph.Type = row["Type1"].ToString();
                    objGraph.ServiceId =Convert.ToInt32( row["ServiceId"].ToString());
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }

        public List<GraphModel> MandalWiseList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    objGraph.DistrictName = row["DistrictName"].ToString();
                    objGraph.MandalId = Convert.ToInt32(row["MandalId"]);
                    objGraph.MandalName = row["MandalName"].ToString();
                    objGraph.value = Convert.ToInt32(row["Value"].ToString());
                    objGraph.Type = row["Type1"].ToString();
                    objGraph.ServiceId = Convert.ToInt32(row["ServiceId"].ToString());
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }

        public List<GraphModel> VillageWiseList(DataTable dt)
        {
            try
            {
                List<GraphModel> objList = new List<GraphModel>();
                GraphModel objGraph = new GraphModel();
                foreach (DataRow row in dt.Rows)
                {
                    objGraph = new GraphModel();
                    objGraph.DistrictId = Convert.ToInt32(row["DistrictId"]);
                    objGraph.DistrictName = row["DistrictName"].ToString();
                    objGraph.MandalId = Convert.ToInt32(row["MandalId"]);
                    objGraph.MandalName = row["MandalName"].ToString();
                    objGraph.VillageId = Convert.ToInt32(row["VillageId"]);
                    objGraph.VillageName = row["VillageName"].ToString();
                    objGraph.value = Convert.ToInt32(row["Value"].ToString());
                    objGraph.ServiceId = Convert.ToInt32(row["ServiceId"].ToString());
                    // objGraph.Type = row["Type"].ToString();
                    objList.Add(objGraph);
                }

                return objList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log.       -- kishore  25-10-2017
                return null;
            }
        }

        #region Bind Maps
        public DataTable BindMapsCount()
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.BindMapsCount();
        }
        public DataTable BindMaps(int serviceId)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.BindMaps(serviceId);
            return dt; 
        }
        #endregion

        #region License Expire Appliactions
        public DataTable GetLicenseExpireApplicationsCount()
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetLicenseExpireApplicationsCount();
            return dt;
        }
        public DataSet GetExpiredLicenses()
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.GetExpiredLicenses();
        }
        public DataTable GetCommissionerExpiredApplications(string Type, int DistrictId)
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.GetCommissionerExpiredApplications(Type, DistrictId);
        }
        #endregion

        #region AppealApplications
        public DataTable GetAppealApplications(int DistrictId, int TransactionId, string TransactionType)
        {
            departmentDAL = new DepartmentDAL();
            DataTable dt = departmentDAL.GetAppealApplications(DistrictId, TransactionId, TransactionType);
            return dt;
        }
        #endregion

        public DataTable GetDashboard1TAMCEApplicationsList(int StatusId, int DistrictId, int MandalId, int VillageId, DateTime? FromDate, DateTime? ToDate)
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.GetDashboard1TAMCEApplicationsList(StatusId, DistrictId,MandalId,VillageId, FromDate, ToDate);
        }
        public DataTable GetReceivedTodayTAMCEApplications(string StatusId, int DistrictId, int MandalId, int VillageId)
        {
            departmentDAL = new DepartmentDAL();
            return departmentDAL.GetReceivedTodayTAMCEApplications(StatusId, DistrictId, MandalId, VillageId);
        }
    }
}
