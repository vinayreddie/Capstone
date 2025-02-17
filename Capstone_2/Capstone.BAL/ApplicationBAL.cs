using Capstone.BAL;
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
    public class ApplicationBAL
    {
        ApplicationDAL objDAL;

        public string GetDistrictPaymentAccountId(int districtId)
        {
            objDAL = new ApplicationDAL();
            var dt = objDAL.GetDistrictPaymentAccountId(districtId);
            return dt.Rows[0]["AccountId"].ToString();
        }

        public int SavePayment(int apmceTransId, string orderId, string paymentId, string signature, decimal amount, int userId)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SavePayment(apmceTransId, orderId, paymentId, signature, amount, userId);
        }

        public DataTable GetPaymentDetails(int tamceTransactionId)
        {
            objDAL = new ApplicationDAL();
            return objDAL.GetPaymentDetails(tamceTransactionId);
        }

        public int SubmitApplication(int applicationId, int userId, ref string applicationNumber)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SubmitApplication(applicationId, userId, ref applicationNumber);
        }
        public SMSModel GetSMSDetails(int applicationId, int transactionId, int WorkFlowType, string ApplicationType)
        {
            objDAL = new ApplicationDAL();
            SMSModel SmsData = new SMSModel();
            DataTable dt = objDAL.GetSMSDetails(applicationId, transactionId, WorkFlowType, ApplicationType);
            if (dt != null && dt.Rows.Count > 0)
            {


                SmsData.ApplicationNumber = dt.Rows[0]["ApplicationNumber"].ToString();
                SmsData.ApplicantName = dt.Rows[0]["Name"].ToString();
                SmsData.ApplicantMobileNumber = dt.Rows[0]["ApplicantMobile"].ToString();
                SmsData.DeptUserName = dt.Rows[0]["DeptUserName"].ToString();
                SmsData.DeptMobile = dt.Rows[0]["DeptMobile"].ToString();
                SmsData.PrevDeptName = dt.Rows[0]["PreviousDeptName"].ToString();
                SmsData.PrevDeptMobile = dt.Rows[0]["PreviousDeptMobile"].ToString();

            }
            return SmsData;
        }
        public SMSModel GetAppealSubmitSMSData(int TransactionId)
        {
            objDAL = new ApplicationDAL();
            SMSModel SmsData = new SMSModel();
            DataTable dt = objDAL.GetAppealSubmitSMSData(TransactionId);
            if (dt != null && dt.Rows.Count > 0)
            {


                SmsData.ApplicationNumber = dt.Rows[0]["ApplicationNumber"].ToString();
                SmsData.ApplicantName = dt.Rows[0]["Name"].ToString();
                SmsData.ApplicantMobileNumber = dt.Rows[0]["ApplicantMobile"].ToString();
                SmsData.DeptUserName = dt.Rows[0]["DeptUserName"].ToString();
                SmsData.DeptMobile = dt.Rows[0]["DeptMobile"].ToString();

            }
            return SmsData;
        }
        public AcknowledgeModel GetAcknowledgeDetails(int applicationId, string TableName)
        {
            try
            {
                objDAL = new ApplicationDAL();
                DataSet dsItems = objDAL.GetAcknowledgeDetails(applicationId, TableName);
                if (dsItems == null)
                    return null;

                AcknowledgeModel model = new AcknowledgeModel();

                foreach (DataRow row in dsItems.Tables[0].Rows)
                {
                    int serviceId = Convert.ToInt32(row["ServiceId"]);
                    switch (serviceId)
                    {
                        case 1:
                            {
                                model.HasAppliedforAPMCE = true;
                            }
                            break;
                        case 2:
                            {
                                model.HasAppliedforPCPNDT = true;
                            }
                            break;
                        case 31: // Blood Bank Form 27 C
                        case 32: // Blood Bank Form 27 E
                            {
                                model.HasAppliedforBloodBank = true;
                            }
                            break;
                    }
                }

                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    // TODO: Bind APMCE Ack         - Raj, 23-06-2017
                    model.APMCEAckModel = new APMCEAckModel();
                    model.APMCEAckModel.ApplicationType = Convert.ToString(dsItems.Tables[1].Rows[0]["ApplicationType"]);
                    model.APMCEAckModel.ApplicantNameAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["ApplicantNameAddress"]);
                    model.APMCEAckModel.IssuingAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["IssuingAuthority"]);
                    model.APMCEAckModel.AppropriateAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["AppropriateAuthority"]);
                    model.APMCEAckModel.ReceivedDate = dsItems.Tables[1].Rows[0]["SubmittedOn"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[1].Rows[0]["SubmittedOn"]) : default(DateTime);
                    model.APMCEAckModel.ReceivedPlace = Convert.ToString(dsItems.Tables[1].Rows[0]["Place"]);
                }

                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    model.PCPNDTAckModel = new PCPNDTAckModel();
                    model.PCPNDTAckModel.ApplicationType = Convert.ToString(dsItems.Tables[2].Rows[0]["ApplicationType"]);
                    model.PCPNDTAckModel.Facilities = Convert.ToString(dsItems.Tables[2].Rows[0]["Facilities"]);
                    model.PCPNDTAckModel.ApplicantNameAddress = Convert.ToString(dsItems.Tables[2].Rows[0]["ApplicantNameAddress"]);
                    model.PCPNDTAckModel.IssuingAuthority = Convert.ToString(dsItems.Tables[2].Rows[0]["IssuingAuthority"]);
                    model.PCPNDTAckModel.AppropriateAutority = Convert.ToString(dsItems.Tables[2].Rows[0]["AppropriateAuthority"]);
                    model.PCPNDTAckModel.ReceivedDate = dsItems.Tables[2].Rows[0]["SubmittedOn"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[2].Rows[0]["SubmittedOn"]) : default(DateTime);
                }

                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    model.BloodBankAckModel = new BloodBankAckModel();
                    model.BloodBankAckModel.FormName = Convert.ToString(dsItems.Tables[3].Rows[0]["FormName"]);
                    model.BloodBankAckModel.ApplicationType = Convert.ToString(dsItems.Tables[3].Rows[0]["ApplicationType"]);
                    model.BloodBankAckModel.NameAddress = Convert.ToString(dsItems.Tables[3].Rows[0]["NameAddress"]);
                    model.BloodBankAckModel.Date = dsItems.Tables[3].Rows[0]["DATE"] != DBNull.Value ?
                                    Convert.ToDateTime(dsItems.Tables[3].Rows[0]["DATE"]) : default(DateTime);
                }
                if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
                {
                    model.BloodBankAckModel = new BloodBankAckModel();
                    model.BloodBankAckModel.FormName = Convert.ToString(dsItems.Tables[4].Rows[0]["FormName"]);
                    model.BloodBankAckModel.BloodBankForm = Convert.ToString(dsItems.Tables[4].Rows[0]["BloodbankForm"]);
                    model.BloodBankAckModel.ApplicationType = Convert.ToString(dsItems.Tables[4].Rows[0]["ApplicationType"]);
                    model.BloodBankAckModel.NameAddress = Convert.ToString(dsItems.Tables[4].Rows[0]["NameAddress"]);
                    model.BloodBankAckModel.Date = dsItems.Tables[4].Rows[0]["DATE"] != DBNull.Value ?
                                    Convert.ToDateTime(dsItems.Tables[4].Rows[0]["DATE"]) : default(DateTime);
                }

                return model;

            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 15-09-2017
                return null;
            }
        }

        public DataTable GetRejectedApplications(int userId)
        {
            objDAL = new ApplicationDAL();
            return objDAL.GetRejectedApplications(userId);
        }

        // Not using - Raj, 07-06-2017
        public List<DocumentUploadModel> DeleteandGetFiles(int id, int userId, string referenceTable)
        {
            try
            {
                objDAL = new ApplicationDAL();
                DataTable dtItems = objDAL.DeleteandGetFiles(id, userId, referenceTable);
                if (dtItems == null)
                    return null;

                List<DocumentUploadModel> docsList = new List<DocumentUploadModel>();
                DocumentUploadModel document;
                foreach (DataRow row in dtItems.Rows)
                {
                    document = new DocumentUploadModel();
                    document.Id = Convert.ToInt32(row["Id"]);
                    document.DocumentPath = Convert.ToString(row["DocumentPath"]);
                    docsList.Add(document);
                }
                return docsList;
            }
            catch (Exception ex)
            {
                //TODO: Write exception log     - Raj, 07-06-2017
                return null;
            }
        }

        #region Allopathic
        public int SaveAllopathicDrugDetails(AllopathicDrugStoreViewModel model, int UserId, ref int applicationId, ref int transactionId,
           ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SaveAllopathicDrugDetails(model, UserId, ref applicationId, ref transactionId, ref formStatus, ref applicationNumber);
        }

        public int SaveADApplicantDetails(ApplicantViewModel model, ref int applicationId, ref int transactionId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SaveADApplicantDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }
        public int SaveADPharmacyDetails(AllopathicPharmacyViewModel model, ref int applicationId, ref int transactionId, ref int PharmacyId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SaveADPharmacyDetails(model, ref applicationId, ref transactionId, ref PharmacyId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveADCompetentDetails(ApplicantViewModel model, ref int applicationId, ref int transactionId, ref int CompetentId,
           ref FormStatus formStatus, ref string applicationNumber, int DocumentsCount)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SaveADCompetentDetails(model, ref applicationId, ref transactionId, ref CompetentId,
                ref formStatus, ref applicationNumber, DocumentsCount);
        }
        public int SaveADDrugDetails(List<AllopathicDrugName> DrugsList, ref int applicationId, ref int transactionId, int ServiceId, int UserId,
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SaveADDrugDetails(DrugsList, ref applicationId,
                ref transactionId, ServiceId, UserId, ref formStatus, ref applicationNumber);
        }
        public int SaveADDeclaration(AllopathicDeclaration model, ref int applicationId, ref int transactionId,
            ref int DeclarationId, int ServiceId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new ApplicationDAL();
            return objDAL.SaveADDeclaration(model, ref applicationId,
                ref transactionId, ref DeclarationId, ServiceId, ref formStatus, ref applicationNumber);

        }

        public AllopathicDrugStoreViewModel GetAllopathicDetails(int transactionId)
        {
            objDAL = new ApplicationDAL();
            DataSet dsItems = objDAL.GetAllopathicDetails(transactionId);
            if (dsItems == null)
                return null;
            AllopathicDrugStoreViewModel AllopathicModel = new AllopathicDrugStoreViewModel();

            #region Particulars of applicant
            ApplicantViewModel applicantModel = new ApplicantViewModel();
            AuthorisationViewModel authorisationModel = new AuthorisationViewModel();
            AllopathicModel.TransactionId = transactionId;
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[0].Rows[0];
                AllopathicModel.Id = Convert.ToInt32(row["ApplicationId"]);
                applicantModel.Id = Convert.ToInt32(row["Id"]);
                applicantModel.ServiceId = Convert.ToInt32(row["ServiceId"]);
                applicantModel.Name = row["Name"].ToString();
                applicantModel.OwnershipType = row["ApplicantRole"].ToString();
                applicantModel.Aadhar = row["Aadhar"].ToString();
                applicantModel.PAN = row["PAN"].ToString();
                applicantModel.MobileNo = row["Mobile"].ToString();
                applicantModel.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                applicantModel.DistrictName = row["DistrictName"].ToString();
                applicantModel.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                applicantModel.MandalName = row["MandalName"].ToString();
                applicantModel.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                applicantModel.VillageName = row["VillageName"].ToString();
                applicantModel.HouseNumber = row["HouseNumber"].ToString();
                applicantModel.StreetName = row["StreetName"].ToString();
                applicantModel.PINCode = row["PINCode"].ToString();
                applicantModel.ApplicantUpload = row["UploadedFile"].ToString();
                applicantModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                AllopathicModel.ADApplicantModel = applicantModel;
            }
            #endregion

            #region Allopathic Pharmacy
            AllopathicPharmacyViewModel AllopathicPharmacy = new AllopathicPharmacyViewModel();
            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[1].Rows[0];
                AllopathicPharmacy.Id = Convert.ToInt32(row["Id"]);
                AllopathicPharmacy.Name = row["Name"].ToString();
                AllopathicPharmacy.OwnedBy = row["OwnedBy"].ToString();
                AllopathicPharmacy.Fromdate = Convert.ToDateTime(row["FromDate"].ToString());
                AllopathicPharmacy.ToDate = Convert.ToDateTime(row["ToDate"].ToString());



                AllopathicPharmacy.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                AllopathicPharmacy.DistrictName = row["DistrictName"].ToString();
                AllopathicPharmacy.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                AllopathicPharmacy.MandalName = row["MandalName"].ToString();
                AllopathicPharmacy.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                AllopathicPharmacy.VillageName = row["VillageName"].ToString();
                AllopathicPharmacy.HouseNumber = row["HouseNo"].ToString();
                AllopathicPharmacy.StreetName = row["StreetName"].ToString();
                AllopathicPharmacy.PINCode = row["PINCode"].ToString();
                AllopathicPharmacy.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                AllopathicModel.ADPharmacyModel = AllopathicPharmacy;

            }
            #endregion

            #region Competent Incharge Details
            ApplicantViewModel Competent = new ApplicantViewModel();
            if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[2].Rows[0];
                Competent.Id = Convert.ToInt32(row["Id"]);
                Competent.Name = row["Name"].ToString();
                Competent.Aadhar = row["AadharId"].ToString();
                Competent.MobileNo = row["MobileNo"].ToString();
                Competent.DistrictId = Convert.ToInt32(row["DistrictId"].ToString());
                Competent.DistrictName = row["DistrictName"].ToString();
                Competent.MandalId = Convert.ToInt32(row["MandalId"].ToString());
                Competent.MandalName = row["MandalName"].ToString();
                Competent.VillageId = Convert.ToInt32(row["VillageId"].ToString());
                Competent.VillageName = row["VillageName"].ToString();
                Competent.HouseNumber = row["HouseNo"].ToString();
                Competent.StreetName = row["StreetName"].ToString();
                Competent.PINCode = row["PINCode"].ToString();
                Competent.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                AllopathicModel.ADCompetentPersonModel = Competent;
            }
            #endregion

            #region Competent
            List<DocumentUploadModel> CompetentDocuments = new List<DocumentUploadModel>();
            if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow row in dsItems.Tables[3].Rows)
                {
                    DocumentUploadModel Documet = new DocumentUploadModel();
                    Documet.Id = Convert.ToInt32(row["Id"]);
                    Documet.UploadType = row["UploadType"].ToString();
                    Documet.DocumentPath = row["DocumentPath"].ToString();
                    CompetentDocuments.Add(Documet);
                }
                AllopathicModel.ADCompetentPersonModel.uploadedDocuments = CompetentDocuments;

            }
            List<AllopathicDrugName> DrugsList = new List<AllopathicDrugName>();
            if (dsItems.Tables[4] != null && dsItems.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow row in dsItems.Tables[4].Rows)
                {
                    AllopathicDrugName Drug = new AllopathicDrugName();
                    Drug.Id = Convert.ToInt32(row["Id"]);
                    Drug.Name = row["Name"].ToString();
                    DrugsList.Add(Drug);
                }
                AllopathicModel.AllopathicDrugList = DrugsList;

            }

            #endregion

            #region Declaration

            if (dsItems.Tables[5] != null && dsItems.Tables[5].Rows.Count > 0)
            {
                AllopathicDeclaration Declaration = new AllopathicDeclaration();
                DataRow row = dsItems.Tables[5].Rows[0];
                Declaration.Id = Convert.ToInt32(row["Id"]);
                Declaration.TextArea = row["Description"].ToString();
                Declaration.Sign = row["Signature"].ToString();
                Declaration.Date = Convert.ToDateTime(row["Date"].ToString());
                Declaration.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]);
                AllopathicModel.ADDeclaration = Declaration;
            }

            return AllopathicModel;
        }
        #endregion

        #endregion

        #region Form 1 to Form 7 Certificates Methods
        public TAMCEMultiFormsViewModel GetAllTAMCEformsCertificateDetails(int transactionId, string TableName)
        {
            TAMCEMultiFormsViewModel model = new TAMCEMultiFormsViewModel();
            model.tamceCertificate = new APMCECertificate();
            try
            {
                objDAL = new ApplicationDAL();
                DataSet dsItems = objDAL.GetAllTAMCEformsCertificateDetails(transactionId, TableName);
                if (dsItems == null)
                    return null;

                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    model.tamceAckModel = new APMCEAckModel();
                    model.tamceAckModel.ApplicationType = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplicationType"]);
                    model.tamceAckModel.ApplicantNameAddress = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplicantNameAddress"]);
                    model.tamceAckModel.IssuingAuthority = Convert.ToString(dsItems.Tables[0].Rows[0]["IssuingAuthority"]);
                    model.tamceAckModel.AppropriateAuthority = Convert.ToString(dsItems.Tables[0].Rows[0]["AppropriateAuthority"]);
                    model.tamceAckModel.ReceivedDate = dsItems.Tables[0].Rows[0]["SubmittedOn"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[0].Rows[0]["SubmittedOn"]) : default(DateTime);
                    model.tamceAckModel.ReceivedPlace = Convert.ToString(dsItems.Tables[0].Rows[0]["Place"]);
                }
                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    model.tamceCertificate.ApplicationNumber = Convert.ToString(dsItems.Tables[1].Rows[0]["ApplicationNumber"]);
                    model.tamceCertificate.ApplicationDate = dsItems.Tables[1].Rows[0]["ApplicationDate"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[1].Rows[0]["ApplicationDate"]) : default(DateTime);
                    model.tamceCertificate.IssuedDate = dsItems.Tables[1].Rows[0]["IssuedDate"] != DBNull.Value ?
                       Convert.ToDateTime(dsItems.Tables[1].Rows[0]["IssuedDate"]) : default(DateTime);
                    model.tamceCertificate.ExpiryDate = dsItems.Tables[1].Rows[0]["ExpiryDate"] != DBNull.Value ?
                       Convert.ToDateTime(dsItems.Tables[1].Rows[0]["ExpiryDate"]) : default(DateTime);
                    model.tamceCertificate.ApplicantNameAddress = Convert.ToString(dsItems.Tables[2].Rows[0]["NameAddress"]); //["ApplicantName"]);
                    model.tamceCertificate.District = Convert.ToString(dsItems.Tables[1].Rows[0]["District"]);
                    model.tamceCertificate.ApplicantAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["ApplnAddress"]);
                }

                if (dsItems.Tables[2] != null && dsItems.Tables[2].Rows.Count > 0)
                {
                    model.tamceCertificate.District = Convert.ToString(dsItems.Tables[2].Rows[0]["DistrictName"]);
                    model.tamceCertificate.IssuingAuthority = Convert.ToString(dsItems.Tables[2].Rows[0]["IssuingAuthority"]);
                    model.tamceCertificate.AppropriateAuthority = Convert.ToString(dsItems.Tables[2].Rows[0]["AppropriateAuthority"]);
                }

                if (dsItems.Tables[3] != null && dsItems.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow serivcerow in dsItems.Tables[3].Rows)
                    {
                        model.tamceCertificate.ServiceDetails.Add(serivcerow["Services"].ToString());
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 15-09-2017
                return null;
            }
        }

        public APMCECertificate GetTemparoryCertificateDetails(int transactionId, string TableName)
        {
            APMCECertificate model = new APMCECertificate();
            try
            {
                objDAL = new ApplicationDAL();
                DataSet dsItems = objDAL.GetTemparoryCertificateDetails(transactionId, TableName);
                if (dsItems == null)
                    return null;

                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    model.ApplicationNumber = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplicationNumber"]);
                    model.ApplicationDate = dsItems.Tables[0].Rows[0]["ApplicationDate"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[0].Rows[0]["ApplicationDate"]) : default(DateTime);
                    model.IssuedDate = dsItems.Tables[0].Rows[0]["IssuedDate"] != DBNull.Value ?
                       Convert.ToDateTime(dsItems.Tables[0].Rows[0]["IssuedDate"]) : default(DateTime);
                    model.ExpiryDate = dsItems.Tables[0].Rows[0]["ExpiryDate"] != DBNull.Value ?
                       Convert.ToDateTime(dsItems.Tables[0].Rows[0]["ExpiryDate"]) : default(DateTime);

                    model.District = Convert.ToString(dsItems.Tables[0].Rows[0]["District"]);
                    model.ApplicantAddress = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplnAddress"]);
                    model.Remarks = Convert.ToString(dsItems.Tables[0].Rows[0]["RejectionRemarks"]);

                    if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                    {
                        model.District = Convert.ToString(dsItems.Tables[1].Rows[0]["DistrictName"]);
                        model.IssuingAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["IssuingAuthority"]);
                        model.AppropriateAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["AppropriateAuthority"]);
                        model.ApplicantNameAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["NameAddress"]); //["ApplicantName"]);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public APMCECertificate GetRejectCertificateDetails(int transactionId, string TableName)
        {
            APMCECertificate model = new APMCECertificate();
            try
            {
                objDAL = new ApplicationDAL();
                DataSet dsItems = objDAL.GetRejectCertificateDetails(transactionId, TableName);
                if (dsItems == null)
                    return null;

                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    model.ApplicationNumber = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplicationNumber"]);
                    model.ApplicationDate = dsItems.Tables[0].Rows[0]["ApplicationDate"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[0].Rows[0]["ApplicationDate"]) : default(DateTime);
                    model.IssuedDate = dsItems.Tables[0].Rows[0]["IssuedDate"] != DBNull.Value ?
                       Convert.ToDateTime(dsItems.Tables[0].Rows[0]["IssuedDate"]) : default(DateTime);
                    model.ExpiryDate = dsItems.Tables[0].Rows[0]["ExpiryDate"] != DBNull.Value ?
                       Convert.ToDateTime(dsItems.Tables[0].Rows[0]["ExpiryDate"]) : default(DateTime);
                    
                    model.District = Convert.ToString(dsItems.Tables[0].Rows[0]["District"]);
                    model.ApplicantAddress = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplnAddress"]);
                    model.Remarks = Convert.ToString(dsItems.Tables[0].Rows[0]["RejectionRemarks"]); 

                    if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                    {
                        model.District = Convert.ToString(dsItems.Tables[1].Rows[0]["DistrictName"]);
                        model.IssuingAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["IssuingAuthority"]);
                        model.AppropriateAuthority = Convert.ToString(dsItems.Tables[1].Rows[0]["AppropriateAuthority"]);
                        model.ApplicantNameAddress = Convert.ToString(dsItems.Tables[1].Rows[0]["NameAddress"]); //["ApplicantName"]);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AcknowledgeModel GetAcknowledgeCertificateDetails(int TransactionId, string TableName)
        {
            AcknowledgeModel model = new AcknowledgeModel();
            try
            {
                objDAL = new ApplicationDAL();
                DataSet dsItems = objDAL.GetAcknowledgeCertificateDetails(TransactionId, TableName);
                if (dsItems == null)
                    return null;

                if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    model.APMCEAckModel = new APMCEAckModel();
                    model.APMCEAckModel.ApplicationType = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplicationType"]);
                    model.APMCEAckModel.ApplicantNameAddress = Convert.ToString(dsItems.Tables[0].Rows[0]["ApplicantNameAddress"]);
                    model.APMCEAckModel.IssuingAuthority = Convert.ToString(dsItems.Tables[0].Rows[0]["IssuingAuthority"]);
                    model.APMCEAckModel.AppropriateAuthority = Convert.ToString(dsItems.Tables[0].Rows[0]["AppropriateAuthority"]);
                    model.APMCEAckModel.ReceivedDate = dsItems.Tables[0].Rows[0]["SubmittedOn"] != DBNull.Value ?
                        Convert.ToDateTime(dsItems.Tables[0].Rows[0]["SubmittedOn"]) : default(DateTime);
                    model.APMCEAckModel.ReceivedPlace = Convert.ToString(dsItems.Tables[0].Rows[0]["Place"]);
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}

