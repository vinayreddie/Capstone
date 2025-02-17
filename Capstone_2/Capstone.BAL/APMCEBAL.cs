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
    public class APMCEBAL
    {
        APMCEDAL objDAL;

        public int SaveRegistrationDetails(RegistrationModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveRegistrationDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveTrustDetails(TrustModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveTrustDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveAccommodationDetails(AccommodationModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveAccommodationDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveCorrespondingAddressDetails(CorrespondingAddressModel model, ref int applicationId, 
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveCorrespondingAddressDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveInfraStructure(List<InfraStructureModel> objList, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType, int existingApplicationId)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveInfraStructure(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType, existingApplicationId);
        }

        public int SaveStaffDetails(List<StaffDetailsModel> objList, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType, int existingApplicationId)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveStaffDetails(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType, existingApplicationId);
        }

        public List<InfraStructureModel> GetInfraStructures(int transactionId)
        {
            objDAL = new APMCEDAL();
            DataTable dtItems = objDAL.GetInfraStructures(transactionId);
            if (dtItems == null)
                return null;
            List<InfraStructureModel> objList = new List<InfraStructureModel>();
            InfraStructureModel model;
            foreach (DataRow row in dtItems.Rows)
            {
                model = new InfraStructureModel();
                model.Id = Convert.ToInt32(row["Id"]);
                model.Name = Convert.ToString(row["Name"]);
                model.Quantity = Convert.ToInt32(row["Quantity"]);
                model.ItemModel = Convert.ToString(row["ItemModel"]);
                model.Remarks = Convert.ToString(row["Remarks"]);
                model.UploadedFilePath = Convert.ToString(row["UploadedFilePath"]);
                objList.Add(model);
            }
            return objList;
        }

        public int SaveEstablishmentDetails(EstablishmentModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveEstablishmentDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveServicesOfferedDetails(OfferedServicesModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveServicesOfferedDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        //public int SaveStaffDetails(StaffDetailsModel model, ref int applicationId,
        //    ref int transactionId, ref FormStatus formStatus, ref string applicationNumber, string applicationType)
        //{
        //    objDAL = new APMCEDAL();
        //    return objDAL.SaveStaffDetails(model, ref applicationId, ref transactionId,
        //        ref formStatus, ref applicationNumber, applicationType);
        //}

        public List<StaffDetailsModel> GetStaffDetails(int transactionId)
        {
            objDAL = new APMCEDAL();
            DataTable dtItems = objDAL.GetStaffDetails(transactionId);
            if (dtItems == null)
                return null;
            List<StaffDetailsModel> objList = new List<StaffDetailsModel>();
            StaffDetailsModel model;
            foreach (DataRow row in dtItems.Rows)
            {
                model = new StaffDetailsModel();
                model.Id = Convert.ToInt32(row["Id"]);
                model.Name = Convert.ToString(row["Name"]);
                model.StaffDesignation = Convert.ToString(row["Designation"]);
                model.RegistrationNumber = Convert.ToString(row["RegistrationNo"]);
                model.Speciality = Convert.ToString(row["Speciality"]);
                model.UploadedFilePath = Convert.ToString(row["StaffDetailsDocPath"]);
                model.PhoneNumber = Convert.ToString(row["PhoneNumber"]);
                model.Email = Convert.ToString(row["EmailId"]);
                objList.Add(model);
            }
            return objList;
        }

        public int SaveFacilitiesAvailable(FacilitiesAvailableModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveFacilitiesAvailable(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveAdditionalDocuments(AdditionalDocumentsModel model, ref int applicationId,
            ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveAdditionalDocuments(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveExistingLicenseDetails(ExistingLicense model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.SaveExistingLicenseDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public bool IsExistingLicenseNumberExists(string existingLicenseNumber, string applicationNumber)
        {
            objDAL = new APMCEDAL();
            return objDAL.IsExistingLicenseNumberExists(existingLicenseNumber, applicationNumber);
        }
        public DataTable GetExistingLicensesIndex(int UserId)
        {
            objDAL = new APMCEDAL();
            return objDAL.GetExistingLicensesIndex(UserId);
        }
        public DataTable BindExistingLicenseTrans(int TransId)
        {
            objDAL = new APMCEDAL();
            return objDAL.BindExistingLicenseTrans(TransId);
        }
        public int DeleteExistingLicense(int Id, int UserId)
        {
            objDAL = new APMCEDAL();
            return objDAL.DeleteExistingLicense(Id, UserId);
        }
    }
}
