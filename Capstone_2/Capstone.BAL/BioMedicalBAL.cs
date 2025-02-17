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
   public class BioCapstoneBAL
    {
        BioCapstoneDAL objDAL;
        public int SaveBioCapstoneApplicationDetails(BioCapstoneViewModel model, ref int applicationId, ref int transactionId,
          ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveBioCapstoneApplicationDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveBioApplicantDetails(BioCapstoneApplicantViewModel model, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveBioApplicantDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveBioCapstoneTreatmentDetails(BioCapstoneAddressTreatmentFacilityViewModel model, ref int applicationId, ref int transactionId,
          ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveBioCapstoneTreatmentDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveBioCapstoneDisposalwaste(BioCapstoneAddressofDisposalWaste model, ref int applicationId, ref int transactionId,
         ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveBioCapstoneDisposalwaste(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveModeoftreatment(List<TreatmentModle> objList, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveModeoftreatment(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber,  ApplicationType);
        }
        public List<TreatmentModle> GetTreatment(int transactionId ,string modeType)
        {
            objDAL = new BioCapstoneDAL();
            DataTable dtItems = objDAL.GetTreatment(transactionId, modeType);
            if (dtItems == null)
                return null;
            List<TreatmentModle> treatmentList = new List<TreatmentModle>();
            TreatmentModle treatment;
            foreach (DataRow row in dtItems.Rows)
            {
               treatment = new TreatmentModle();
               treatment.Id = Convert.ToInt32(row["Id"]);
               treatment.Description = Convert.ToString(row["Description"]);
               treatment.Attachment = Convert.ToString(row["FilePath"]);
               treatmentList.Add(treatment);
            }
            return treatmentList;
        }
        public int SaveModeofTreatmentDisposal(List<TreatmentDisposalModle> objList, ref int applicationId, ref int transactionId,
       ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveModeofTreatmentDisposal(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, ApplicationType);
        }
        public List<TreatmentDisposalModle> GetTreatmentDisposal(int transactionId , string modeType)
        {
            objDAL = new BioCapstoneDAL();
            DataTable dtItems = objDAL.GetTreatmentDisposal(transactionId, modeType);
            if (dtItems == null)
                return null;
            List<TreatmentDisposalModle> treatmentDisposalList = new List<TreatmentDisposalModle>();
            TreatmentDisposalModle treatmentDisposal;
            foreach (DataRow row in dtItems.Rows)
            {
                treatmentDisposal = new TreatmentDisposalModle();
                treatmentDisposal.Id = Convert.ToInt32(row["Id"]);
                treatmentDisposal.Description = Convert.ToString(row["Description"]);
                treatmentDisposal.Attachment = Convert.ToString(row["FilePath"]);
                treatmentDisposalList.Add(treatmentDisposal);
            }
            return treatmentDisposalList;
        }
        public int SaveQuantityofWaste(List<QuantityWasteModel> objList, ref int applicationId, ref int transactionId,
     ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveQuantityofWaste(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, ApplicationType);
        }
        public List<QuantityWasteModel> GetQuantityWaste(int transactionId)
        {
            objDAL = new BioCapstoneDAL();
            DataTable dtItems = objDAL.GetQuantityWaste(transactionId);
            if (dtItems == null)
                return null;
            List<QuantityWasteModel> quantityWasteList = new List<QuantityWasteModel>();
            QuantityWasteModel quantityWaste;
            foreach (DataRow row in dtItems.Rows)
            {
                quantityWaste = new QuantityWasteModel();
                quantityWaste.Id = Convert.ToInt32(row["Id"]);
                quantityWaste.CategoryName = Convert.ToString(row["Category"]);
                quantityWaste.Quantity = Convert.ToString(row["Qunatity"]);
                quantityWaste.UnitName = Convert.ToString(row["Units"]);
                quantityWasteList.Add(quantityWaste);
            }
            return quantityWasteList;
        }
        public int SaveBioDeclarationDetails(DeclarationViewModel model, ref int applicationId, ref int transactionId,
          ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveBioDeclarationDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveAuthorisationActivity(AuthorisationViewModel model, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BioCapstoneDAL();
            return objDAL.SaveAuthorisationActivity(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
    }
   
}
