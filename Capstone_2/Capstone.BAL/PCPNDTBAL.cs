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
    public class PCPNDTBAL
    {
        PCPNDTDAL objDAL;       

        public int SaveApplicantDetails(ApplicantModel model, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveApplicantDetails(model, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber);
        }
        //public int SaveEstablishmentDetails(EstablishmentModel model, ref int applicationId, ref int transactionId, 
        //    ref FormStatus formStatus, ref string applicationNumber)
        //{
        //    objDAL = new PCPNDTDAL();
        //    return objDAL.SaveEstablishmentDetails(model, ref applicationId, ref transactionId, 
        //        ref formStatus, ref applicationNumber);
        //}

        public int SaveFacilityDetails(FacilityModel model, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveFacilityDetails(model, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber);
        }
        public int SaveTests(TestsModel model, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveTests(model, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber);
        }
        public int SaveEquipments(List<EquipmentModel> objList, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber,string ApplicationType, int existingApplicationId)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveEquipments(objList, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber, ApplicationType, existingApplicationId);
        }
        public List<EquipmentModel> GetEquipments(int transactionId)
        {
            objDAL = new PCPNDTDAL();
            DataTable dtItems = objDAL.GetEquipments(transactionId);
            if (dtItems == null)
                return null;
            List<EquipmentModel> equipmentList = new List<EquipmentModel>();
            EquipmentModel equipment;
            foreach (DataRow row in dtItems.Rows)
            {
                equipment = new EquipmentModel();
                equipment.Id = Convert.ToInt32(row["Id"]);
                equipment.Name = Convert.ToString(row["Name"]);
                equipment.SerialNumber = Convert.ToString(row["SerialNumber"]);
                equipment.MachineModel = Convert.ToString(row["MachineModel"]);
                equipment.Make = Convert.ToString(row["Make"]);
                equipment.Type = Convert.ToString(row["Type"]);
                equipment.UploadedFilePath = Convert.ToString(row["UploadedFilePath"]);
                equipment.NocFilePath = row["NOCFliePath"].ToString();
                equipment.TransferCertificatePath = row["TransferCertificateFilePath"].ToString();
                equipment.InstallationCerticatePath = row["InstallationCertificatePath"].ToString();
                equipment.InvoicePath = row["InvoicePath"].ToString();
                equipment.ImagePath = row["ImagePath"].ToString();
                equipmentList.Add(equipment);
            }
            return equipmentList;
        }
        public int SaveFacilities(FacilitesModel model, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveFacilities(model, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber);
        }
        public int CheckforEmployeeRegistration(string registrationNumber)
        {
            objDAL = new PCPNDTDAL();
            DataTable dtItems = objDAL.CheckforEmployeeRegistration(registrationNumber);
            if (dtItems == null || dtItems.Rows.Count == 0)
                return 0;

            int employeeCount = Convert.ToInt32(dtItems.Rows[0]["EmployeeCount"]);
            return employeeCount;
        }
        public int SaveEmployees(List<EmployeeViewModel> objList, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber,string ApplicationType, int existingApplicationId)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveEmployees(objList, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber, ApplicationType, existingApplicationId);
        }
        public List<EmployeeViewModel> GetEmployees(int transactionId)
        {
            objDAL = new PCPNDTDAL();
            DataSet ds = objDAL.GetEmployees(transactionId);
            if (ds == null)
                return null;

            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            EmployeeViewModel employee;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                employee = new EmployeeViewModel();
                employee.Id = Convert.ToInt32(row["Id"]);
                employee.Name = Convert.ToString(row["Name"]);
                employee.DesignationId = Convert.ToInt32(row["DesignationId"]);
                employee.DesignationName = Convert.ToString(row["DesignationName"]);
                employee.SubDesignation = Convert.ToString(row["SubDesignation"]);
                employee.Experience = Convert.ToString(row["Experience"]);
                employee.ExpYears = Convert.ToInt32(row["ExpYears"]);
                employee.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                employee.ExpDays = Convert.ToInt32(row["ExpDays"]);
                employee.RegistrationNumber = Convert.ToString(row["RegistrationNumber"]);
                employeeList.Add(employee);
            }

            List<DocumentUploadModel> uploadsList = new List<DocumentUploadModel>();
            DocumentUploadModel uploadDoc;
            foreach (DataRow uploadrow in ds.Tables[1].Rows)
            {
                uploadDoc = new DocumentUploadModel();
                uploadDoc.Id = Convert.ToInt32(uploadrow["Id"]);
                uploadDoc.ReferenceId = Convert.ToInt32(uploadrow["ReferenceId"]);
                uploadDoc.DocumentPath = uploadrow["DocumentPath"].ToString();
                uploadDoc.UploadType = uploadrow["UploadType"].ToString();
                uploadsList.Add(uploadDoc);
            }

            foreach (var emp in employeeList)
            {
                emp.UploadDocuments = uploadsList.Where(item => item.ReferenceId == emp.Id).ToList();
            }

            return employeeList;
        }
        public int SaveInstitutionDetails(InstitutionModel model, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveInstitutionDetails(model, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber);
        }
        public int SaveDeclarationDetails(DeclarationModel model, ref int applicationId, ref int transactionId, 
            ref FormStatus formStatus, ref string applicationNumber)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.SaveDeclarationDetails(model, ref applicationId, ref transactionId, 
                ref formStatus, ref applicationNumber);
        }
        public int GetEnclosuresCnt(int transactionId)
        {
            objDAL = new PCPNDTDAL();
            return objDAL.GetEnclosuresCnt(transactionId);
           
        }
    }
}
