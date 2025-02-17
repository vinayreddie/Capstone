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
    public class OrganTransplantBAL
    {
        OrganTransplantDAL objDAL;
        public int SaveHospitalDetails(HospitalViewModel model, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.SaveHospitalDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveSurgicalDetails(SurgicalTeamModel model, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.SaveSurgicalDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public DataTable GetStaffDetails(int TransactionId, string SectionName)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.GetStaffDetails(TransactionId, SectionName);        
                        
        }
        public int SaveCapstoneDetails(CapstoneTeamModel model, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.SaveCapstoneDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveAnaesthesiologyDetails(AnaesthesiologyModel model, ref int applicationId, ref int transactionId,
       ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.SaveAnaesthesiologyDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public DataTable GetOperations(int TransactionId, string SectionName)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.GetOperations(TransactionId, SectionName);
        }
        public DataTable GetEquipments(int TransactionId, string SectionName)
        {
            objDAL = new OrganTransplantDAL();
            return objDAL.GetEquipments(TransactionId, SectionName);
        }
    }
}
