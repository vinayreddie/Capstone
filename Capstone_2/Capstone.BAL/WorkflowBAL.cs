using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Data;
namespace Capstone.BAL
{
    public class WorkflowBAL
    {
        WorkflowDAL objDAL;
        public void GetMasterdata(int DepartmentId,out List<MasterServiceModel> ServicesList,out List<DocumentModel> DocumentList,out List<DesignationModel> DesignationList)
        {
            objDAL = new WorkflowDAL();
            DataSet ds = objDAL.GetMasterdata(DepartmentId);
            ServicesList = new List<MasterServiceModel>();DocumentList = new List<DocumentModel>();DesignationList = new List<DesignationModel>();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    ServicesList = ConvertServicestoList(ds.Tables[0]);
                if (ds.Tables[1].Rows.Count > 0)
                    DocumentList = ConvertDocumenttoList(ds.Tables[1]);
                if (ds.Tables[2].Rows.Count > 0)
                    DesignationList = ConvertDesignationtoList(ds.Tables[2]);
            }
                
        }
        private List<MasterServiceModel> ConvertServicestoList(DataTable dt)
        {
            List<MasterServiceModel> ServiceList = new List<MasterServiceModel>();
            foreach (DataRow row in dt.Rows)
            {
                MasterServiceModel Service = new MasterServiceModel();
                Service.Id = Convert.ToInt32(row["Id"]);
                Service.Name = row["Name"].ToString();
                Service.ActType = row["ActType"].ToString();
                ServiceList.Add(Service);
            }
            return ServiceList;
        }
        private List<DocumentModel> ConvertDocumenttoList(DataTable dt)
        {
            List<DocumentModel> DocumentList = new List<DocumentModel>();
            foreach (DataRow row in dt.Rows)
            {
                DocumentModel Document = new DocumentModel();
                Document.Id= Convert.ToInt32(row["Id"]);
                Document.Name= row["Name"].ToString();
                Document.DocumentTypeId = (DocumentType)(row["DocumentTypeId"]);
                Document.DocumentPath= row["DocumentPath"].ToString();
                DocumentList.Add(Document);
            }
            return DocumentList;
        }
        private List<DesignationModel> ConvertDesignationtoList(DataTable dt)
        {
            List<DesignationModel> DesignationList = new List<DesignationModel>();
            foreach (DataRow row in dt.Rows)
            {
                DesignationModel Designation = new DesignationModel();
                Designation.Id = Convert.ToInt32(row["Id"]);
                Designation.Name = row["Name"].ToString();
                Designation.DepartmentId = Convert.ToInt32(row["DepartmentId"]);
                DesignationList.Add(Designation);
            }
            return DesignationList;
        }
        public bool SaveWorkFlow(DataTable dtWorkFlow, DataTable dtRevWorkflow, ServiceModel service)//string requiredDocs, string approvalDocs,
        {
            objDAL = new WorkflowDAL();
            return objDAL.SaveWorkFlow(dtWorkFlow, dtRevWorkflow, service);
        }

        public DataTable GetWorkflowIndex()
        {
            objDAL = new WorkflowDAL();
            return objDAL.GetWorkflowIndex();
        }
    }
}
