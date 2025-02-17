﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;
using System.Data;
using Capstone.Framework;

namespace Capstone.DAL
{
   
    public class WorkflowDAL
    {
        #region Global
        SqlServerDBManager dbManager;
        SqlParameter param;
        List<SqlParameter> paramList;
        #endregion
        public WorkflowDAL()
        {
            dbManager = new DAL.SqlServerDBManager();
        }
        public DataSet GetMasterdata(int DepartmentId)
        {
            try
            {
                paramList = new List<SqlParameter>();
                param = new SqlParameter("@DepartmentId", DepartmentId);
                paramList.Add(param);
                return dbManager.ExecuteSPMultipleResultSet ("GetMasterdataforWorkflow", paramList);
            }
            catch(Exception ex)
            {
                // TODO: Write exception log.       - Mounika, 09-05-2017
                return null;
            }

        }
         public bool SaveWorkFlow(DataTable dtWorkFlow,DataTable dtRevWorkflow, ServiceModel service) //string requiredDocs ,string approvalDocs
        {
            paramList = new List<SqlParameter>();
            try
            {
                param = new  SqlParameter("@Workflowlist", SqlDbType.Structured);
                param.Value = dtWorkFlow;                
                paramList.Add(param);
                param = new SqlParameter("@RevWorkflowlist", SqlDbType.Structured);
                param.Value = dtRevWorkflow;
                paramList.Add(param);
                param = new SqlParameter("@ServiceId", service.ServiceId);
                paramList.Add(param);
                param = new SqlParameter("@DepartmentId",service.DepartmentId);
                paramList.Add(param);
                param = new SqlParameter("@RequiredDocId", service.RequiredDocId);
                paramList.Add(param);
                param = new SqlParameter("@ApprovalDocId", service.ApprovalDocId);
                paramList.Add(param);
                param = new SqlParameter("@CreatedBy", service.CreatedUserId);
                paramList.Add(param);
                param = new SqlParameter("@FixedFee", service.HasFixedFee);
                paramList.Add(param);
                param = new SqlParameter("@DepartmentRaised", service.HasDepartmentRaisedFee);
                paramList.Add(param);
                param = new SqlParameter("@Autogenerated", service.HasAutogenerated);
                paramList.Add(param);
                param = new SqlParameter("@Fee", service.Fee);
                paramList.Add(param);
                param = new SqlParameter("@Formula", service.Formula);
                paramList.Add(param);
                SqlCommand cmd = dbManager.ExecuteProcedure("SaveWorkflow", paramList);
                if (cmd != null)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                //TODO : Write exception log -mounika 16-03-2017
                return false;
            }

        }

        public DataTable GetWorkflowIndex()
        {
            string sp = "GetWorkflowIndex";
            try
            {
                return dbManager.ExecuteStoredProc(sp);
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                exception.DbObject = sp;                
                Logger.LogError(exception);
                return null;
            }
        }
    }
}
