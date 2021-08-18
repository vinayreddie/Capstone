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
        //private List<DocumentModel> ConvertDocumenttoList(DataTable dt)
        //{
        //    List<DocumentModel> DocumentList = new List<DocumentModel>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        DocumentModel Document = new DocumentModel();
        //        Document.Id= Convert.ToInt32(row["Id"]);
        //        Document.Name= row["Name"].ToString();
        //        Document.DocumentTypeId = (DocumentType)(row["DocumentTypeId"]);
        //        Document.DocumentPath= row["DocumentPath"].ToString();
        //        DocumentList.Add(Document);
        //    }
        //    return DocumentList;
        //}
       
        //public bool SaveWorkFlow(DataTable dtWorkFlow, DataTable dtRevWorkflow, ServiceModel service)//string requiredDocs, string approvalDocs,
        //{
        //    objDAL = new WorkflowDAL();
        //    return objDAL.SaveWorkFlow(dtWorkFlow, dtRevWorkflow, service);
        //}

        public DataTable GetWorkflowIndex()
        {
            objDAL = new WorkflowDAL();
            return objDAL.GetWorkflowIndex();
        }
    }
}
