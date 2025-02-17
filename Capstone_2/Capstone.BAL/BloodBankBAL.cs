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
    public class BloodBankBAL : MasterBAL
    {
        BloodBankDAL objDAL;

        public int SaveBloodBankApplicantDetails(BloodBankApplicantModel model, ref int applicationId, ref int transactionId,
    ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankApplicantDetails(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveBloodBankEstablishmentDetails(BloodBankEstablishmentModel model, ref int applicationId,
    ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankEstablishment(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveBloodBankEquipments(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
    ref FormStatus formStatus, ref string applicationNumber, string ApplicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankEquipments(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, ApplicationType);
        }

       

        public int SaveListofItems(List<BloodBankListModel> objList, ref int applicationId, ref int transactionId,
          ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveListofItems(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        public List<BloodBankListModel> GetBloodBankListItems(int transactionId)
        {
            try
            {
                objDAL = new BloodBankDAL();
                DataTable dtItems = objDAL.GetBloodBankListItems(transactionId);
                if (dtItems == null)
                    return null;

                List<BloodBankListModel> list = new List<BloodBankListModel>();
                BloodBankListModel listItem;
                foreach (DataRow row in dtItems.Rows)
                {
                    listItem = new BloodBankListModel();
                    listItem.Id = Convert.ToInt32(row["Id"]);
                    listItem.Name = row["ItemName"].ToString();
                    list.Add(listItem);

                }

                return list;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }

        public List<EquipmentModel> GetBloodBankEquipments(int transactionId)
        {
            try
            {
                objDAL = new BloodBankDAL();
                DataTable dtItems = objDAL.GetBloodBankEquipments(transactionId);
                if (dtItems == null)
                    return null;

                List<EquipmentModel> list = new List<EquipmentModel>();
                EquipmentModel listItem;
                foreach (DataRow row in dtItems.Rows)
                {
                    listItem = new EquipmentModel();
                    listItem.Id = Convert.ToInt32(row["Id"]);
                    listItem.Name = row["Name"].ToString();
                    listItem.SerialNumber = row["SerialNumber"].ToString();
                    listItem.MachineModel = row["MachineModel"].ToString();
                    listItem.Make = row["Make"].ToString();
                    listItem.Type = row["Type"].ToString();
                    listItem.UploadedFilePath = row["UploadedFilePath"].ToString();
                    list.Add(listItem);
                }

                return list;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }

        public int SaveEquipment(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
         ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveEquipment(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        public int SaveBloodBankEmployee(List<EmployeeViewModel> objList, ref int applicationId, ref int transactionId,
         ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankEmployee(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        public List<EmployeeViewModel> GetBloodBankEmployees(int transactionId)
        {
            try
            {
                objDAL = new BloodBankDAL();
                DataSet dsItems = objDAL.GetBloodBankEmployees(transactionId);
                if (dsItems == null)
                    return null;

                List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
                EmployeeViewModel employee;
                foreach (DataRow row in dsItems.Tables[0].Rows)
                {
                    employee = new EmployeeViewModel();
                    employee.Id = Convert.ToInt32(row["Id"]);
                    employee.Name = row["Name"].ToString();
                    employee.QualificationId = Convert.ToInt32(row["QualificationId"]);
                    employee.QualificationName = row["QualificationName"].ToString();
                    employee.ExpYears = Convert.ToInt32(row["ExpYears"]);
                    employee.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                    employee.ExpDays = Convert.ToInt32(row["ExpDays"]);
                    employeeList.Add(employee);
                }


                // Employee Uploads
                List<DocumentUploadModel> documentsList = new List<DocumentUploadModel>();
                DocumentUploadModel document;
                foreach (DataRow row in dsItems.Tables[1].Rows)
                {
                    document = new DocumentUploadModel();
                    document.Id = Convert.ToInt32(row["Id"]);
                    document.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                    document.DocumentPath = Convert.ToString(row["DocumentPath"]);
                    document.UploadType = Convert.ToString(row["UploadType"]);
                    documentsList.Add(document);
                }

                // Group Doucments to Employees
                for (int i = 0; i < employeeList.Count; i++)
                {
                    employeeList[i].UploadDocuments = documentsList
                        .Where(item => item.ReferenceId == employeeList[i].Id).ToList();
                }

                return employeeList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }

        public int SaveDeclarationDetails(BloodBankAttachments objList, ref int applicationId, ref int transactionId,
        ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveDeclarationDetails(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveBloodBankTechnicalDetails(List<TechnicalModel> objList, ref int applicationId, ref int transactionId,
 ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankTechnicalDetails(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        public List<TechnicalModel> GetBloodBankTechnicalStaff(int transactionId)
        {
            try
            {
                objDAL = new BloodBankDAL();
                DataSet dsItems = objDAL.GetBloodBankTechnicalStaff(transactionId);
                if (dsItems == null)
                    return null;

                List<TechnicalModel> technicalList = new List<TechnicalModel>();
                TechnicalModel technical;
                foreach (DataRow row in dsItems.Tables[0].Rows)
                {
                    technical = new TechnicalModel();
                    technical.Id = Convert.ToInt32(row["Id"]);
                    technical.Name = row["Name"].ToString();
                    technical.QualificationId = Convert.ToInt32(row["QualificationId"]);
                    technical.Qualification = row["QualificationName"].ToString();
                    technical.ExpYears = Convert.ToInt32(row["ExpYears"]);
                    technical.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                    technical.ExpDays = Convert.ToInt32(row["ExpDays"]);
                    technicalList.Add(technical);
                }


                // Technical Staff Uploads
                List<DocumentUploadModel> documentsList = new List<DocumentUploadModel>();
                DocumentUploadModel document;
                foreach (DataRow row in dsItems.Tables[1].Rows)
                {
                    document = new DocumentUploadModel();
                    document.Id = Convert.ToInt32(row["Id"]);
                    document.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                    document.DocumentPath = Convert.ToString(row["DocumentPath"]);
                    document.UploadType = Convert.ToString(row["UploadType"]);
                    documentsList.Add(document);
                }

                // Group Doucments to Employees
                for (int i = 0; i < technicalList.Count; i++)
                {
                    technicalList[i].UploadDocuments = documentsList
                        .Where(item => item.ReferenceId == technicalList[i].Id).ToList();
                }

                return technicalList;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Jai, 21-08-2017
                return null;
            }
        }

        #region BloodBank Form 27E
        public int SaveBloodBankApplicantForm27E(BloodBankApplicantModel model, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankApplicantForm27E(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        public int SaveBloodBankEstablishmentForm27E(BloodBankEstablishmentModel model, ref int applicationId,
    ref int transactionId, ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveBloodBankEstablishmentForm27E(model, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        #region List Items Form 27 E
        public int SaveListofItemsForm27E(List<BloodBankListModel> objList, ref int applicationId, ref int transactionId,
  ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveListofItemsForm27E(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        public List<BloodBankListModel> GetBloodBankListItemsFomr27E(int transactionId)
        {
            try
            {
                objDAL = new BloodBankDAL();
                DataTable dtItems = objDAL.GetBloodBankListItemsForm27E(transactionId);
                if (dtItems == null)
                    return null;

                List<BloodBankListModel> list = new List<BloodBankListModel>();
                BloodBankListModel listItem;
                foreach (DataRow row in dtItems.Rows)
                {
                    listItem = new BloodBankListModel();
                    listItem.Id = Convert.ToInt32(row["Id"]);
                    listItem.Name = row["ItemName"].ToString();
                    list.Add(listItem);

                }

                return list;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }
        #endregion
        #region Equipment Form 27 E
        public int SaveEquipmentForm27E(List<EquipmentModel> objList, ref int applicationId, ref int transactionId,
 ref FormStatus formStatus, ref string applicationNumber, ApplicationType applicationType)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveEquipmentForm27E(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber, applicationType);
        }

        public List<EquipmentModel> GetBloodBankEquipmentForm27E(int transactionId)
        {
            try
            {
                objDAL = new BloodBankDAL();
                DataTable dtItems = objDAL.GetBloodBankEquipmentForm27E(transactionId);
                if (dtItems == null)
                    return null;

                List<EquipmentModel> list = new List<EquipmentModel>();
                EquipmentModel listItem;
                foreach (DataRow row in dtItems.Rows)
                {
                    listItem = new EquipmentModel();
                    listItem.Id = Convert.ToInt32(row["Id"]);
                    listItem.Name = row["Name"].ToString();
                    listItem.SerialNumber = row["SerialNumber"].ToString();
                    listItem.MachineModel = row["MachineModel"].ToString();
                    listItem.Make = row["Make"].ToString();
                    listItem.Type = row["Type"].ToString();
                    listItem.UploadedFilePath = row["UploadedFilePath"].ToString();
                    list.Add(listItem);
                }

                return list;
            }
            catch (Exception ex)
            {
                // TODO: Write exception log        - Raj, 16-08-2017
                return null;
            }
        }
        #endregion
        public int SaveDeclarationForm27E(BloodBankAttachments objList, ref int applicationId, ref int transactionId,
ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new BloodBankDAL();
            return objDAL.SaveDeclarationForm27E(objList, ref applicationId, ref transactionId,
                ref formStatus, ref applicationNumber);
        }

        #endregion

    }
}
