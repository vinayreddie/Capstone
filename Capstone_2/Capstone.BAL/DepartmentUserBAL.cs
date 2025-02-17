using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using System.Data;
using Capstone.Models;
using Capstone.Framework;

namespace Capstone.BAL
{
    public class DepartmentUserBAL
    {
        DepartmentUserDAL objDAL;
        public List<TransactionViewModel> GetListofApplications(int DesignationId, int DistrictId, int MandalId, int VillageId, string Type, int UserId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetListofApplications(DesignationId, DistrictId, MandalId, VillageId, Type, UserId);
            if (dt == null)
                return null;
            List<TransactionViewModel> Transactionlist = new List<TransactionViewModel>();
            if(Type== "ForwardAppToday" || Type== "RejectedToday")
            {
                foreach (DataRow row in dt.Rows)
                {
                    TransactionViewModel Transaction = new TransactionViewModel();
                    Transaction.Id = Convert.ToInt32(row["TransactionId"]);
                    Transaction.ServiceId = Convert.ToInt32(row["ServiceId"]);
                    Transaction.ServiceName = row["ServiceName"].ToString();
                    Transaction.ApplicantName = row["ApplicantName"].ToString();
                    Transaction.AmendmentId = Convert.ToInt32(row["AmendmentId"]);
                    Transaction.TranServiceId = Convert.ToInt32(row["TranServiceId"]);
                    Transaction.ServiceType = row["ServiceType"].ToString();
                    Transaction.CurrentDesignationName = row["CurrentDesignation"].ToString();
                    Transaction.StatusName = row["status"].ToString();
                    //Transaction.LicenseExpiryDate = row["LicenseExpiryDate"].ToString();
                    Transactionlist.Add(Transaction);
                }
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    TransactionViewModel Transaction = new TransactionViewModel();
                    Transaction.Id = Convert.ToInt32(row["TransactionId"]);
                    Transaction.ServiceId = Convert.ToInt32(row["ServiceId"]);
                    Transaction.ServiceName = row["ServiceName"].ToString();
                    Transaction.ApplicantName = row["ApplicantName"].ToString();
                    Transaction.AmendmentId = Convert.ToInt32(row["AmendmentId"]);
                    Transaction.TranServiceId = Convert.ToInt32(row["TranServiceId"]);
                    Transaction.ServiceType = row["ServiceType"].ToString();
                    Transaction.Type = Type == "QueryProcessed" ? row["Type"].ToString() : null;
                    Transactionlist.Add(Transaction);
                }
            }
            
            return Transactionlist;
        }
        public ApprovalComplexViewModel ApprovalSceenOnloadData(int TransactionId, int DesignationId, int ServiceId, int DeptUserId)
        {
            objDAL = new DepartmentUserDAL();
            ApprovalComplexViewModel Approval = new ApprovalComplexViewModel();
            DataSet ds = objDAL.ApprovalSceenOnloadData(TransactionId, DesignationId, ServiceId, DeptUserId);
            List<DesignationModel> DesignationList = new List<DesignationModel>(); WorkFlowModel Workflow = new WorkFlowModel();
            List<ApprovalsViewModel> ApprovalList = new List<ApprovalsViewModel>();
            if (ds != null)
            {
                if (ds.Tables[0] != null)
                    DesignationList = ConvertDesignationtoList(ds.Tables[0]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    Workflow.HasInspectionPrevilege = ds.Tables[1].Rows[0]["HasInspectionPrivilege"].ToString() == "" ? false : Convert.ToBoolean(ds.Tables[1].Rows[0]["HasInspectionPrivilege"]);
                    Workflow.HasReturnPrevilege = ds.Tables[1].Rows[0]["HasReturnPrivilege"].ToString() == "" ? false : Convert.ToBoolean(ds.Tables[1].Rows[0]["HasReturnPrivilege"]);
                    Workflow.HasRaisedQueryPrevilege = ds.Tables[1].Rows[0]["HasRaiseQueryPrivilege"].ToString() == "" ? false : Convert.ToBoolean(ds.Tables[1].Rows[0]["HasRaiseQueryPrivilege"]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                    Workflow.HasApprovalPrevilege = Convert.ToBoolean(ds.Tables[2].Rows[0]["HasApprovalPrivilege"]);
                if (ds.Tables[3].Rows.Count > 0)
                    ApprovalList = ConvertApprovaltoList(ds.Tables[3]);
                if (ds.Tables[4].Rows.Count > 0)
                {

                    Approval.QueryCount = Convert.ToInt32(ds.Tables[4].Rows[0]["QueryCount"]);
                    Approval.QueryResponseCount = Convert.ToInt32(ds.Tables[4].Rows[0]["QueryResponseCount"]);
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    Approval.FacilityList = new List<FacilityMasterModel>();
                    DataTable dt = ds.Tables[5];
                    foreach (DataRow row in dt.Rows)
                    {
                        FacilityMasterModel Facility = new Models.FacilityMasterModel();
                        Facility.Id = Convert.ToInt32(row["Id"]);
                        Facility.Name = row["Name"].ToString();
                        Facility.InspectionPartialViewName = row["inspectionpartialviewname"].ToString();
                        Approval.FacilityList.Add(Facility);
                    }

                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    Approval.QuestionModelList = new List<QuestionModel>();
                    DataTable dt = ds.Tables[6];
                    foreach (DataRow row in dt.Rows)
                    {
                        QuestionModel Question = new QuestionModel();
                        Question.Id = Convert.ToInt32(row["QuestionId"]);
                        Question.Question = row["Question"].ToString();
                        Question.Answer = row["Answer"].ToString();
                        Approval.QuestionModelList.Add(Question);

                    }

                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    Approval.InspectionReportCount = Convert.ToInt32(ds.Tables[7].Rows[0]["InspectionCount"]);
                }
                if (ds.Tables[8].Rows.Count > 0)
                {
                    Approval.UploadList = new List<DocumentUploadModel>();
                    DataTable dt = ds.Tables[8];
                    foreach (DataRow row in dt.Rows)
                    {
                        DocumentUploadModel Upload = new DocumentUploadModel();
                        Upload.Id = Convert.ToInt32(row["Id"]);
                        Upload.UploadType = row["UploadType"].ToString();
                        Upload.UploadedUserId = Convert.ToInt32(row["UploadedUserId"]);
                        Upload.UploadedUserName = row["UploadedUserName"].ToString();
                        Upload.DocumentPath = row["DocumentPath"].ToString();

                        Approval.UploadList.Add(Upload);
                    }
                }
                if (ds.Tables[9].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[9];
                    Approval.ServiceType = dt.Rows[0]["ServiceType"].ToString();

                }
                if (ds.Tables[10].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[10];
                    Approval.InspectionPDFpath = dt.Rows[0]["DocumentPath"].ToString();
                }
                if (ds.Tables[11].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[11];
                    Approval.CurrentDestignationId = Convert.ToInt32(dt.Rows[0]["CurrentDesignationId"]);
                    Approval.CurrentStatus = (Status)dt.Rows[0]["CurrentStatus"];
                    Approval.ReturnedSource = dt.Rows[0]["ReturnedSource"]==DBNull.Value?0: Convert.ToInt32(dt.Rows[0]["ReturnedSource"]);
                    
                }
               
                if (ds.Tables[12] != null && ds.Tables[12].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[12];
                    Approval.ApplicationNumber = dt.Rows[0]["ApplcationNumber"].ToString();
                    Approval.ApplicationId= Convert.ToInt32(dt.Rows[0]["ApplicationId"]); 
                }
            }
            Approval.DesginationList = DesignationList; Approval.Workflow = Workflow; Approval.ApprovalList = ApprovalList;
            return Approval;
        }
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
        private List<ApprovalsViewModel> ConvertApprovaltoList(DataTable dt)
        {
            List<ApprovalsViewModel> ApprovalList = new List<ApprovalsViewModel>();
            foreach (DataRow row in dt.Rows)
            {
                ApprovalsViewModel Approval = new ApprovalsViewModel();
                Approval.DesignationId = Convert.ToInt32(row["DesignationId"]);
                Approval.DesignationName = row["DesignationName"].ToString();
                Approval.TransactionId = Convert.ToInt32(row["TransactionId"]);
                Approval.Remarks = row["Remarks"].ToString();
                Approval.status = (Status)row["Status"];
                ApprovalList.Add(Approval);
            }
            return ApprovalList;
        }
        public bool SaveApprovals(ApprovalsModel Approval, int DesignationId, List<InspectionModel> InspectionList, List<DocumentUploadModel> UploadList, int ReferenceId, string DocumentPath)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = Utitlities.ConvertToDataTable(InspectionList);
            DataTable dtUploads = null;
            if (UploadList != null && UploadList.Count > 0)
            {
                dtUploads = Utitlities.ConvertToDataTable(UploadList);
                Approval.InspectionDate = Convert.ToDateTime(dtUploads.Rows[0]["UploadedDate"]);
                Approval.InspectionFile = dtUploads.Rows[0]["DocumentPath"].ToString();
                DocumentPath = dtUploads.Rows[0]["DocumentPath"].ToString();
                dtUploads.Columns.Remove("UploadedDate");
            }
            else
                Approval.InspectionDate = Convert.ToDateTime(DateTime.Now);


            return objDAL.SaveApprovals(Approval, DesignationId, dt, dtUploads, ReferenceId, DocumentPath);
        }
        public List<QueryModel> GetQureyResponsebyTransactionId(int TransactionId, string TransactionType)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetQureyResponsebyTransactionId(TransactionId,TransactionType);
            if (dt == null)
                return null;
            return ConvertQuerytoList(dt);

        }
        public SMSModel GetApprovalSMSData(int transactionId, int WorkflowType, string Type)
        {
            objDAL = new DepartmentUserDAL();
            SMSModel SmsData = new SMSModel();
            DataTable dt = objDAL.GetApprovalSMSData(transactionId, WorkflowType, Type);
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

        private List<QueryModel> ConvertQuerytoList(DataTable dt)
        {
            List<QueryModel> QueryList = new List<QueryModel>();
            foreach (DataRow row in dt.Rows)
            {
                QueryModel Query = new QueryModel();
                Query.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                Query.Description = row["Description"].ToString();
                Query.TransactionId = Convert.ToInt32(row["TransactionId"]);
                Query.Type = row["Type"].ToString();
                Query.UploadedFilePath = row["UploadedDocPath"].ToString();
                Query.UserId = Convert.ToInt32(row["UserId"]);
                QueryList.Add(Query);
            }
            return QueryList;
        }

        public DataSet GetDeptUserInspectionQuestions(int TransactionId, int DepartmentUserId,string Type)
        {
            objDAL = new DepartmentUserDAL();
            return objDAL.GetInspectionQuestionsList(TransactionId, DepartmentUserId,Type);
        }
        public bool SaveInspectionFacilitiesQuestions(List<InspectionModel> InspectionQuestions, int TransactionId)
        {
            DataTable InspectionFacilitiesModel = Utitlities.ConvertToDataTable(InspectionQuestions);
            objDAL = new DepartmentUserDAL();
            return objDAL.SaveInspectionFacilitiesQuestions(InspectionFacilitiesModel, TransactionId);
        }

        #region Amendment
        public List<TransactionViewModel> GetListofAmendments(int DesignationId, int DistrictId, int MandalId, int VillageId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetListofAmendments(DesignationId, DistrictId, MandalId, VillageId);
            if (dt == null)
                return null;
            List<TransactionViewModel> Transactionlist = new List<TransactionViewModel>();
            foreach (DataRow row in dt.Rows)
            {
                TransactionViewModel Transaction = new TransactionViewModel();
                Transaction.Id = Convert.ToInt32(row["TransactionId"]);
                Transaction.ServiceId = Convert.ToInt32(row["ServiceId"]);
                Transaction.ServiceName = row["ServiceName"].ToString();
                Transaction.ApplicantName = row["ApplicantName"].ToString();
                Transaction.AmendmentId = Convert.ToInt32(row["AmendmentId"]);
                Transaction.TranServiceId = Convert.ToInt32(row["TranServiceId"]);
                Transactionlist.Add(Transaction);
            }
            return Transactionlist;
        }
        public ApprovalComplexViewModel AmendmentApprovalOnloadData(int AmendmentId, int DesignationId, int ServiceId)
        {
            objDAL = new DepartmentUserDAL();
            ApprovalComplexViewModel Approval = new ApprovalComplexViewModel();
            DataSet ds = objDAL.AmendmentApprovalOnloadData(AmendmentId, DesignationId, ServiceId);
            List<DesignationModel> DesignationList = new List<DesignationModel>(); WorkFlowModel Workflow = new WorkFlowModel();
            List<ApprovalsViewModel> ApprovalList = new List<ApprovalsViewModel>();
            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    DesignationList = ConvertDesignationtoList(ds.Tables[0]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    Workflow.HasInspectionPrevilege = ds.Tables[1].Rows[0]["HasInspectionPrivilege"].ToString() == "" ? false : Convert.ToBoolean(ds.Tables[1].Rows[0]["HasInspectionPrivilege"]);
                    Workflow.HasReturnPrevilege = ds.Tables[1].Rows[0]["HasReturnPrivilege"].ToString() == "" ? false : Convert.ToBoolean(ds.Tables[1].Rows[0]["HasReturnPrivilege"]);
                    Workflow.HasRaisedQueryPrevilege = ds.Tables[1].Rows[0]["HasRaiseQueryPrivilege"].ToString() == "" ? false : Convert.ToBoolean(ds.Tables[1].Rows[0]["HasRaiseQueryPrivilege"]);
                }
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    Workflow.HasApprovalPrevilege = Convert.ToBoolean(ds.Tables[2].Rows[0]["HasApprovalPrivilege"]);
                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    ApprovalList = ConvertApprovaltoList(ds.Tables[3]);
                if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    Approval.QueryCount = Convert.ToInt32(ds.Tables[4].Rows[0]["QueryCount"]);
                    Approval.QueryResponseCount = Convert.ToInt32(ds.Tables[4].Rows[0]["QueryResponseCount"]);
                }
                if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {
                    Approval.FacilityList = new List<FacilityMasterModel>();
                    DataTable dt = ds.Tables[5];
                    foreach (DataRow row in dt.Rows)
                    {
                        FacilityMasterModel Facility = new Models.FacilityMasterModel();
                        Facility.Id = Convert.ToInt32(row["Id"]);
                        Facility.Name = row["Name"].ToString();
                        Facility.InspectionPartialViewName = row["inspectionpartialviewname"].ToString();
                        Approval.FacilityList.Add(Facility);
                    }

                }
                if (ds.Tables[8].Rows.Count > 0)
                {
                    Approval.UploadList = new List<DocumentUploadModel>();
                    DataTable dt = ds.Tables[8];
                    foreach (DataRow row in dt.Rows)
                    {
                        DocumentUploadModel Upload = new DocumentUploadModel();
                        Upload.Id = Convert.ToInt32(row["Id"]);
                        Upload.UploadType = row["UploadType"].ToString();
                        Upload.UploadedUserId = Convert.ToInt32(row["UploadedUserId"]);
                        Upload.UploadedUserName = row["UploadedUserName"].ToString();
                        Upload.DocumentPath = row["DocumentPath"].ToString();

                        Approval.UploadList.Add(Upload);
                    }
                }
                if (ds.Tables[9].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[9];
                    Approval.ServiceType = dt.Rows[0]["ServiceType"].ToString();

                }
                if (ds.Tables[10].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[10];
                    Approval.InspectionPDFpath = dt.Rows[0]["DocumentPath"].ToString();
                }
                if (ds.Tables[11].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[11];
                    Approval.CurrentDestignationId = Convert.ToInt32(dt.Rows[0]["CurrentDesignationId"]);
                    Approval.CurrentStatus = (Status)(Convert.ToInt32(dt.Rows[0]["CurrentStatus"]));
                }

                if (ds.Tables[12] != null && ds.Tables[12].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[12];
                    Approval.ApplicationNumber = dt.Rows[0]["ApplcationNumber"].ToString();
                }
            }
            Approval.DesginationList = DesignationList; Approval.Workflow = Workflow; Approval.ApprovalList = ApprovalList;
            return Approval;
        }
        public bool SaveAmedmentApprovals(ApprovalsModel Approval, int DesignationId, List<InspectionModel> InspectionList, List<DocumentUploadModel> UploadList, int ReferenceId, string DocumentPath)
        {            
            objDAL = new DepartmentUserDAL();
            DataTable dt = Utitlities.ConvertToDataTable(InspectionList);
            DataTable dtUploads = null;
            if (UploadList != null)
            {
                dtUploads = Utitlities.ConvertToDataTable(UploadList);
                dtUploads.Columns.Remove("UploadedDate");
            }
            return objDAL.SaveAmedmentApprovals(Approval, DesignationId, dt, dtUploads, ReferenceId, DocumentPath);
        }
        #endregion

        #region PCPNDT Individual tabs data for Amendments
        public FacilityViewModel GetFacility(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            FacilityViewModel facilityModel = new FacilityViewModel();
            DataTable dt = objDAL.GetFacility(AmendmentId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                facilityModel.Id = Convert.ToInt32(row["Id"]);
                facilityModel.Faclities = Convert.ToString(row["Facilities"]);
            }
            return facilityModel;
        }
        public TestsModel GetPCPNDTTests(int AmendmentId, string AmendmentType)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetPCPNDTTests(AmendmentId, AmendmentType);
            TestsModel testsModel = new TestsModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                testsModel.Id = Convert.ToInt32(row["Id"]);
                testsModel.InvasiveTests = row["InvasiveTests"].ToString();
                testsModel.NonInvasiveTests = row["NonInvasiveTests"].ToString();
                testsModel.Remarks = row["Remarks"].ToString();

                // Commented by Raj, 19-08-2017
                // FormStatus will not be t_testslog table
                //testsModel.FormStatus = (FormStatus)Convert.ToInt32(row["FormStatus"]); 
            }
            return testsModel;
        }
        public FacilitesModel GetFacilitiesforTests(int AmendmentId, string AmendmentType)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetFacilitiesforTests(AmendmentId, AmendmentType);
            FacilitesModel facilitiesModel = new FacilitesModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                facilitiesModel.Id = Convert.ToInt32(row["Id"]);
                facilitiesModel.Tests = row["Tests"].ToString();
                facilitiesModel.Studies = row["Studies"].ToString();
                facilitiesModel.Remarks = row["Remarks"].ToString();
            }
            return facilitiesModel;
        }
        public List<EquipmentModel> GetEquipments(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetEquipments(AmendmentId);
            List<EquipmentModel> equipmentList = new List<EquipmentModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                EquipmentModel equipment;
                foreach (DataRow row in dt.Rows)
                {
                    equipment = new EquipmentModel();
                    equipment.Id = row["EquipmentId"].ToString() == "" ? 0 : Convert.ToInt32(row["EquipmentId"]);
                    equipment.EquipmentLogId = Convert.ToInt32(row["Id"]);
                    equipment.Name = row["Name"].ToString();
                    equipment.SerialNumber = row["SerialNumber"].ToString();
                    equipment.MachineModel = row["MachineModel"].ToString();
                    equipment.Make = row["Make"].ToString();
                    equipment.Type = row["Type"].ToString();
                    equipment.UploadedFilePath = row["UploadedFilePath"].ToString();
                    equipment.NocFilePath = row["NOCFliePath"].ToString();
                    equipment.TransferCertificatePath = row["TransferCertificateFilePath"].ToString();
                    equipment.InstallationCerticatePath = row["InstallationCertificatePath"].ToString();
                    equipment.InvoicePath = row["InvoicePath"].ToString();
                    equipment.ImagePath = row["ImagePath"].ToString();
                    equipment.IsDeleted = (bool)row["IsDeleted"];
                    equipmentList.Add(equipment);
                }
            }
            return equipmentList;
        }
        public List<EmployeeViewModel> GetEmployees(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataSet ds = objDAL.GetEmployees(AmendmentId);
            if (ds == null)
                return null;

            #region Preparing Employee Details
            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            EmployeeViewModel employee;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                employee = new EmployeeViewModel();
                employee.Id = row["employeeId"].ToString() == "" ? 0 : Convert.ToInt32(row["employeeId"]);
                employee.EmployeeLogId = Convert.ToInt32(row["Id"]);
                employee.Name = row["Name"].ToString();
                employee.DesignationId = Convert.ToInt32(row["DesignationId"]);
                employee.DesignationName = Convert.ToString(row["DesignationName"]);
                employee.SubDesignation = Convert.ToString(row["SubDesignation"]);
                employee.ExpYears = Convert.ToInt32(row["ExpYears"]);
                employee.ExpMonths = Convert.ToInt32(row["ExpMonths"]);
                employee.ExpDays = Convert.ToInt32(row["ExpDays"]);
                employee.RegistrationNumber = row["RegistrationNumber"].ToString();
                employee.UploadedFilePath = row["UploadedFilePath"].ToString();
                employee.IsDeleted = (bool)row["IsDeleted"];
                employeeList.Add(employee);
            }
            #endregion

            #region Preparing Employee Documents
            List<DocumentUploadModel> employeeDocuments = new List<DocumentUploadModel>();
            DocumentUploadModel employeeDocument;
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                employeeDocument = new DocumentUploadModel();
                employeeDocument.Id = Convert.ToInt32(row["Id"]);
                employeeDocument.ReferenceTable = Convert.ToString(row["ReferenceTable"]);
                employeeDocument.ReferenceId = Convert.ToInt32(row["ReferenceId"]);
                employeeDocument.DocumentPath = Convert.ToString(row["DocumentPath"]);
                employeeDocument.UploadType = Convert.ToString(row["UploadType"]);
                employeeDocuments.Add(employeeDocument);
            }
            #endregion

            #region Merge Employee & Documents
            foreach (var emp in employeeList)
            {
                emp.UploadDocuments = employeeDocuments.Where(item => item.ReferenceId == emp.EmployeeLogId).ToList();
            }
            #endregion

            return employeeList;
        }
        public InstitutionViewModel GetOwnership(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataSet dsItems = objDAL.GetOwnership(AmendmentId);
            InstitutionViewModel institutionModel = new InstitutionViewModel();
            if (dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsItems.Tables[0].Rows[0];
                institutionModel.Id = Convert.ToInt32(row["Id"]);
                institutionModel.OwnershipTypeId = Convert.ToInt32(row["OwnershipTypeId"]);
                institutionModel.OwnershipTypeName = row["OwnershipTypeName"].ToString();
                institutionModel.AffidavitDocPath = Convert.ToString(row["AffidavitDocPath"]);
                institutionModel.ArticleDocPath = Convert.ToString(row["ArticleDocPath"]);

                if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
                {
                    institutionModel.StudyCertificateDocPaths = new List<DocumentUploadModel>();
                    DocumentUploadModel documentModel;
                    foreach (DataRow docRow in dsItems.Tables[8].Rows)
                    {
                        documentModel = new DocumentUploadModel();
                        documentModel.Id = Convert.ToInt32(docRow["Id"]);
                        documentModel.DocumentPath = Convert.ToString(docRow["DocumentPath"]);
                        institutionModel.StudyCertificateDocPaths.Add(documentModel);
                    }
                }
            }
            return institutionModel;
        }
        public InstitutionViewModel GetInstitution(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetInstitution(AmendmentId);
            InstitutionViewModel institutionModel = new InstitutionViewModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                institutionModel.Id = Convert.ToInt32(row["Id"]);
                institutionModel.InstitutionTypeId = Convert.ToInt32(row["InstitutionTypeId"]);
                institutionModel.InstitutionTypeName = row["InstitutionTypeName"].ToString();
            }
            return institutionModel;
        }
        public CancelLicenseModel GetCancelLicenseDetails(int TransactionId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetCancelLicenseDetails(TransactionId);
            CancelLicenseModel model = new CancelLicenseModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.ApplicantName = dr["ApplicantName"].ToString();
                model.CenterName = dr["CenterName"].ToString();
            }
            return model;
        }
        public PCPNDTViewModel GetPCPNDTAppealReason(int AmendmentId, int TransactionId)
        {
            objDAL = new DepartmentUserDAL();
            PCPNDTViewModel PCPNDTView = new PCPNDTViewModel();
            DataTable dt = objDAL.GetPCPNDTAppealReason(AmendmentId, TransactionId);
            if (dt != null && dt.Rows.Count > 0)
            {
                PCPNDTView.RejectionRemarks = dt.Rows[0]["Reasons"].ToString();
                PCPNDTView.ReasonforAppeal = dt.Rows[1]["Reasons"].ToString();
            }
            return PCPNDTView;
        }
        #endregion

        #region APMCEAmmendmentDetails

        public RegistrationViewModel GetAPMCERegistrationDetails(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetAPMCERegistrationDetails(AmendmentId);
            RegistrationViewModel model = new RegistrationViewModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.Id = Convert.ToInt32(dr["Id"]);
                model.FacilityType = dr["FacilityType"].ToString();
                model.Name = dr["RegistrationName"].ToString();
                model.DistrictId = Convert.ToInt32(dr["DistrictId"]);
                model.DistrictName = dr["DistrictName"].ToString();
                model.MandalId = Convert.ToInt32(dr["MandalId"]);
                model.MandalName = dr["MandalName"].ToString();
                model.VillageId = Convert.ToInt32(dr["VillageId"]);
                model.VillageName = dr["VillageName"].ToString();
                model.HouseNumber = dr["HouseNumber"].ToString();
                model.StreetName = dr["StreetName"].ToString();
                model.PINCode = dr["PINCode"].ToString();
                model.Id = (int)dr["Id"];
                model.FormStatus = (FormStatus)(dr["FormStatus"] == null ? 0 : dr["FormStatus"]);
                model.HospitalTypeId = Convert.ToInt32(dr["HospitalTypeId"]);
                model.ClinicType = dr["ClinicType"].ToString();
                model.BedStrength = dr["BedStrength"].ToString();
                model.Speciality = dr["Speciality"].ToString();

                model.BuildingHeight = Convert.ToInt32(dr["BuildingHeight"]);

                model.ApplicantPhoto = dr["ApplicantPhotoPath"].ToString();
                model.AadharCardPath = dr["AadharCardPath"].ToString();
                model.PANCardPath = dr["PANCardPath"].ToString();

            }
            return model;
        }

        public CorrespondingAddressViewModel GetAPMCECorrespondentDetails(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetAPMCECorrespondentDetails(AmendmentId);
            CorrespondingAddressViewModel model = new CorrespondingAddressViewModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.Id = Convert.ToInt32(dr["Id"]);
                model.Name = dr["Name"].ToString();
                model.DistrictName = dr["DistrictName"].ToString();
                model.MandalName = dr["MandalName"].ToString();
                model.VillageName = dr["VillageName"].ToString();
                model.HouseNumber = dr["HouseNumber"].ToString();
                model.StreetName = dr["StreetName"].ToString();
                model.PINCode = dr["PINCode"].ToString();

            }
            return model;
        }

        public AccommadationViewModel GetAPMCEAccomodationDetails(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetAPMCEAccomodationDetails(AmendmentId);
            AccommadationViewModel model = new AccommadationViewModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.Id = Convert.ToInt32(dr["Id"]);
                model.EstablishmentTypeName = dr["EstablishmentTypeName"].ToString();
                model.FromDate = Convert.ToDateTime(dr["FromDate"]);
                model.ToDate = Convert.ToDateTime(dr["ToDate"]);
                model.UploadedFilePath = dr["UploadedDoc"].ToString();


            }
            return model;
        }

        public List<InfraStructureModel> GetEquipmentAndFurniture(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            List<InfraStructureModel> EquipmentAndFurnitureList = new List<InfraStructureModel>();
            DataTable dt = objDAL.GetAPMCEEquipmentAndFurniture(AmendmentId);

            if (dt != null && dt.Rows.Count > 0)
            {
                InfraStructureModel model = new InfraStructureModel();
                DataRow dr = dt.Rows[0];
                model.Id = Convert.ToInt32(dr["Id"]);
                model.Name = dr["EquipmentName"].ToString();
                model.Quantity = Convert.ToInt32(dr["Quantity"]);
                model.ItemModel = dr["Model"].ToString();
                model.Remarks = dr["Remarks"].ToString();
                model.UploadedFilePath = dr["UploadDoc"].ToString();
                EquipmentAndFurnitureList.Add(model);
            }
            return EquipmentAndFurnitureList;
        }

        public List<StaffDetailsViewModel> GetAPMCEStaffDetails(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetAPMCEStaffDetails(AmendmentId);
            List<StaffDetailsViewModel> staffDetailsList = new List<StaffDetailsViewModel>();

            if (dt != null && dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    StaffDetailsViewModel model = new StaffDetailsViewModel();
                    model.Id = Convert.ToInt32(dr["Id"]);
                    model.StaffDesignation = dr["Designation"].ToString();
                    model.Name = dr["DoctorName"].ToString();
                    model.RegistrationNumber = dr["RegistrationNo"].ToString();
                    model.PhoneNumber = dr["PhoneNumber"].ToString();
                    model.Email = dr["EmailId"].ToString();
                    model.UploadedFilePath = dr["UploadPath"].ToString();
                    staffDetailsList.Add(model);

                }

            }
            return staffDetailsList;
        }

        public OfferedServicesModel GetAPMCEOfferedServices(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetAPMCETypeOfServices(AmendmentId);
            OfferedServicesModel model = new OfferedServicesModel();
            if (dt != null && dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                model.Id = Convert.ToInt32(dr["Id"]);
                model.BedStrength = Convert.ToInt32(dr["BedStrength"]);
                model.OfferedServices = dr["OfferedService"].ToString();


            }
            return model;
        }

        public FacilitiesAvailableModel GetAPMCEFacilitiesAvailable(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetAPMCEFacilitiesAvailable(AmendmentId);

            FacilitiesAvailableModel facilityModel = new FacilitiesAvailableModel();
            if (dt != null && dt.Rows.Count > 0)
            {

                DataRow row = dt.Rows[0];
                facilityModel.HasLaborRoom = Convert.ToBoolean(row["LaborFacility"]);
                facilityModel.HasOperationTheater = Convert.ToBoolean(row["OperationTheater"]);
                facilityModel.HasDiagnosticFacility = Convert.ToBoolean(row["DiagnosticksFacility"]);
                facilityModel.HasDeclarationStamp = (bool)row["HasDeclarationStamp"];
                facilityModel.DeclarationStampFilePath = row["NonJudicialStamp"].ToString();
                if (row["NonJudicialStamp"].ToString() == "")
                {
                    facilityModel.DeclarationStampFilePath = null;
                }
                facilityModel.OtherInformationDescription = row["OtherInformation"].ToString();
                facilityModel.OtherInformationDocumentPath = row["OtherDocs"].ToString();
                facilityModel.Id = (int)row["Id"];
                facilityModel.FormStatus = (FormStatus)row["FormStatus"];
            }
            return facilityModel;


        }

        public APMCEViewModel GetAPMCEAppealReason(int AmendmentId, int TransactionId)
        {
            objDAL = new DepartmentUserDAL();
            APMCEViewModel APMCEView = new APMCEViewModel();
            DataTable dt = objDAL.GetAPMCEAppealReason(AmendmentId, TransactionId);
            if (dt != null && dt.Rows.Count > 0)
            {
                APMCEView.RejectionRemarks = dt.Rows[0]["Reasons"].ToString();
                APMCEView.ReasonforAppeal = dt.Rows[1]["Reasons"].ToString();
            }
            return APMCEView;
        }

        public NOCforquipmentModel GetNOCofEquipment(int AmendmentId)
        {
            objDAL = new DepartmentUserDAL();
            DataTable dt = objDAL.GetNOCofEquipment(AmendmentId);
            NOCforquipmentModel objModel = new NOCforquipmentModel();
            if(dt!=null && dt.Rows.Count > 0)
            {
                objModel.EquipmentDetails = new EquipmentModel();
                objModel.TransactionId = Convert.ToInt32(dt.Rows[0]["TransactionId"]);
                objModel.AmendmentId= Convert.ToInt32(dt.Rows[0]["AmendmentId"]);
                objModel.EquipmentDetails.Name = dt.Rows[0]["Name"].ToString();
                objModel.EquipmentDetails.SerialNumber= dt.Rows[0]["SerialNumber"].ToString();
                objModel.EquipmentDetails.MachineModel= dt.Rows[0]["MachineModel"].ToString();
                objModel.EquipmentDetails.Make= dt.Rows[0]["Make"].ToString();
                objModel.EquipmentDetails.Type= dt.Rows[0]["Type"].ToString();
                objModel.EquipmentDetails.InvoicePath= dt.Rows[0]["InvoicePath"].ToString();
                objModel.EquipmentDetails.NocFilePath= dt.Rows[0]["NOCFliePath"].ToString();
                objModel.EquipmentDetails.TransferCertificatePath= dt.Rows[0]["TransferCertificateFilePath"].ToString();
                objModel.EquipmentDetails.InstallationCerticatePath= dt.Rows[0]["InstallationCertificatePath"].ToString();
                objModel.EquipmentDetails.ImagePath= dt.Rows[0]["ImagePath"].ToString();
                objModel.DistrictName= dt.Rows[0]["DistrictName"].ToString();
                objModel.OtherState= dt.Rows[0]["OtherState"].ToString();
                objModel.Remarks= dt.Rows[0]["Remarks"].ToString();
                objModel.Type = dt.Rows[0]["OtherState"].ToString() == "" ? "DistrictId" : "OtherState";                
            }
            return objModel;
        }


        #endregion

        public DataTable GetStatuswiseApplicationDetailsIndex(int UserId,int RoleId,string FromDate=null, string ToDate=null,int DistrictId =0)
        {
            objDAL = new DepartmentUserDAL();
            return objDAL.GetStatuswiseApplicationDetailsIndex(UserId,RoleId, FromDate, ToDate,DistrictId);
        }
        public DataTable GetDistrictwiseNewCentresReport(int UserId, int RoleId, string FromDate = null, string ToDate = null, int DistrictId = 0)
        {
            objDAL = new DepartmentUserDAL();
            return objDAL.GetDistrictwiseNewCentresReport(UserId, RoleId, FromDate, ToDate, DistrictId);
        }
        public DataTable GetDistrictwiseCentresReport(int DistrictId = 0, int CentreId = 0)
        {
            objDAL = new DepartmentUserDAL();
            return objDAL.GetDistrictwiseCentresReport(DistrictId,CentreId);
        }
        public DataTable GetDistrictwiseCentresLicensesReport(int DistrictId = 0, int CentreId = 0)
        {
            objDAL = new DepartmentUserDAL();
            return objDAL.GetDistrictwiseCentresLicensesReport(DistrictId, CentreId);
        }
    }
}
