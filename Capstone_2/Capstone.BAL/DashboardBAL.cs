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

        public DataTable GetDraftApplications(int userId)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetDraftApplications(userId);
        }

        public DataTable GetSubmittedApplications(int userId)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetSubmittedApplications(userId);
        }

        public DataTable GetLicenses(int userId)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetLicenses(userId);
        }
        #region Commissioner Dashboard
        public DataTable GetCommissionerDashoboard(string type,int districtId)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetCommissionerDashoboard(type, districtId);
        }
        #endregion
        #region Department User Dashboard
        public DataSet GetDeptUserDashboadCounts(UserModel user)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetDeptUserDashboadCounts(user);
        }
        #endregion

        #region Admin Dashboard
        public DataSet GetAdminDashboadCounts(int DistrictId)
        {
            objDAL = new DashboardDAL();
            return objDAL.GetAdminDashboadCounts(DistrictId);
        }
        #endregion

        public ApplicationTrackModel GetApplicationTrack(int transactionId, string transactionType)
        {
            objDAL = new DashboardDAL();
            LicenseDAL objLicenseDAL = new LicenseDAL();
            DataTable dtTransactions = objLicenseDAL.GetTransactions(transactionId, Status.All, transactionType);

            ApplicationTrackModel appTrackModel = new ApplicationTrackModel();
            foreach (DataRow tranrow in dtTransactions.Rows)
            {
                int _serviceId = Convert.ToInt32(tranrow["ServiceId"]);
                int _transactionId = Convert.ToInt32(tranrow["Id"]);
                string TransactionType = tranrow["TableName"].ToString();
               
           appTrackModel.ApplicationId = Convert.ToInt32(tranrow["ApplicationId"]);
                switch (_serviceId)
                {
                    case 1:
                        // APMCE
                        #region APMCE
                        appTrackModel.HasTrackforAPMCE = true;
                        DataSet dsAPMCEItems = objDAL.TransactionTrack(_transactionId, TransactionType);
                        appTrackModel.APMCETrackModel = GetTransactionModel(dsAPMCEItems);
                        
                        #endregion
                        break;
                    case 2: //PCPNDT 
                    #region PCPNDT
                    //case 27: // PCPNDT Appeal
                    case 26:
                        appTrackModel.HasTrackforPCPNDT = true;
                        DataSet dsPCPNDTItems = objDAL.TransactionTrack(_transactionId, TransactionType);
                        appTrackModel.PCPNDTTrackModel = GetTransactionModel(dsPCPNDTItems);
                        #endregion
                        break;
                    case 31: // BloodBank From 27C
                        #region Blood Bank Form 27C
                        appTrackModel.HasTrackForBloodBank27C = true;
                        DataSet dsBBForm27CItems = objDAL.TransactionTrack(_transactionId, TransactionType);
                        appTrackModel.BloodBankForm27CModel = GetTransactionModel(dsBBForm27CItems);
                        #endregion
                        break;
                    case 32: // BloodBank From 27E
                        #region Blood Bank Form 27E
                        appTrackModel.HasTrackForBloodBank27E = true;
                        DataSet dsBBForm27EItems = objDAL.TransactionTrack(_transactionId, TransactionType);
                        appTrackModel.bbForm27EModel = GetTransactionModel(dsBBForm27EItems);
                        #endregion
                        break;
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 28:
                    case 38:
                        appTrackModel.HasTrackforPCPNDT = true;
                         dsPCPNDTItems = objDAL.TransactionTrack(_transactionId, TransactionType);
                        appTrackModel.PCPNDTTrackModel = GetTransactionModel(dsPCPNDTItems);
                        break;
                    default:
                        break;
                }
            } 
            return appTrackModel;
        }

        public TransactionTrackModel GetTransactionModel(DataSet dsItems)
        {
            if (dsItems != null)
            {
                TransactionTrackModel tranTrackModel = new TransactionTrackModel();
                if (dsItems.Tables[0] != null)
                    tranTrackModel.GraphicalListModel = ConvertDesignationtoList(dsItems.Tables[0]);
                if (dsItems.Tables[1].Rows.Count > 0)
                {
                    //List<TransactionHistoryModel> TransactionList = new List<TransactionHistoryModel>();
                    tranTrackModel.TransactionHistory = new List<TransactionHistoryModel>();
                    DataTable dt = dsItems.Tables[1];
                    foreach (DataRow row in dt.Rows)
                    {
                        TransactionHistoryModel Transaction = new TransactionHistoryModel();
                        Transaction.UserName = row["Username"].ToString();
                        Transaction.Remarks = row["Remarks"].ToString();
                        Transaction.Status = row["Status"].ToString();
                        Transaction.CreatedOn = row["Date"].ToString();  //Convert.ToDateTime(dt.Rows[0]["Date"]);
                        tranTrackModel.TransactionHistory.Add(Transaction);
                    }
                }
                if (dsItems.Tables[2].Rows.Count > 0)
                {
                    tranTrackModel.CurrentDesignationId = Convert.ToInt32(dsItems.Tables[2].Rows[0]["CurrentDesignationId"]);
                    tranTrackModel.ServiceName = Convert.ToString(dsItems.Tables[2].Rows[0]["ServiceName"]);
                }

                return tranTrackModel;
            }
            else
            {
                return null;
            }

        }

        //public ApplicationTrackModel TransactionTrackList(int TransactionId, int DesignationId, int ApplicationId)
        //{
        //    objDAL = new DashboardDAL();
        //    ApplicationTrackModel appTrackModel = new ApplicationTrackModel();

        //    DataSet ds = objDAL.TransactionTrack(TransactionId, DesignationId, ApplicationId);
        //    appTrackModel.PCPNDTTrackModel = new TransactionTrackModel();
        //    appTrackModel.PCPNDTTrackModel.GraphicalListModel = new List<DesignationModel>();

            
        //    if (ds != null)
        //    {
        //        if (ds.Tables[0] != null)
        //            appTrackModel.PCPNDTTrackModel.GraphicalListModel = ConvertDesignationtoList(ds.Tables[0]);
        //        if (ds.Tables[1].Rows.Count > 0)
        //        {
        //            //List<TransactionHistoryModel> TransactionList = new List<TransactionHistoryModel>();
        //            appTrackModel.PCPNDTTrackModel.TransactionHistory = new List<TransactionHistoryModel>();
        //            DataTable dt = ds.Tables[1];
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                TransactionHistoryModel Transaction = new TransactionHistoryModel();
        //                Transaction.UserName = row["Username"].ToString();
        //                Transaction.Remarks = row["Remarks"].ToString();
        //                Transaction.Status = row["Status"].ToString();
        //                Transaction.CreatedOn = row["Date"].ToString();  //Convert.ToDateTime(dt.Rows[0]["Date"]);
        //                appTrackModel.PCPNDTTrackModel.TransactionHistory.Add(Transaction);

        //            }

        //        }
        //    }          
        //    return appTrackModel;
        //}
        

        private List<DesignationModel> ConvertDesignationtoList(DataTable dt)
        {
            List<DesignationModel> DesignationList = new List<DesignationModel>();
            foreach (DataRow row in dt.Rows)
            {
                DesignationModel Designation = new DesignationModel();
                Designation.Id = Convert.ToInt32(row["DesignationId"]);
                Designation.Name = row["Name"].ToString();
                DesignationList.Add(Designation);
            }
            return DesignationList;
        }
    }

 

    
}


