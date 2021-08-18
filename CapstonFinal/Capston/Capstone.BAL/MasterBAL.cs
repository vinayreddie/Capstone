using Capstone.DAL;
using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.BAL
{
    public class MasterBAL
    {
        MasterDAL masterDAL;

        public List<DistrictModel> GetCountries()
        {
            masterDAL = new MasterDAL();
            DataTable dtItems = masterDAL.GetCountries();
            if (dtItems == null)
                return null;

            List<DistrictModel> districtsList = new List<DistrictModel>();
            DistrictModel district;
            foreach (DataRow row in dtItems.Rows)
            {
                district = new DistrictModel();
                district.Id = Convert.ToInt32(row["Id"]);
                district.Name = row["Name"].ToString();
                districtsList.Add(district);
            }

            return districtsList;
        }
        public List<AirportModel> GetAirports()
        {
            masterDAL = new MasterDAL();
            DataTable dtItems = masterDAL.GetAirport();
            if (dtItems == null)
                return null;

            List<AirportModel> districtsList = new List<AirportModel>();
            AirportModel district;
            foreach (DataRow row in dtItems.Rows)
            {
                district = new AirportModel();
                district.Id = Convert.ToInt32(row["Id"]);
                district.Name = row["airportname"].ToString();
                districtsList.Add(district);
            }

            return districtsList;
        }

        public List<CollegeModel> GetCollege()
        {
            masterDAL = new MasterDAL();
            DataTable dtItems = masterDAL.GetCollege();
            if (dtItems == null)
                return null;

            List<CollegeModel> CollegeList = new List<CollegeModel>();
            CollegeModel College;
            foreach (DataRow row in dtItems.Rows)
            {
                College = new CollegeModel();
                College.Id = Convert.ToInt32(row["Id"]);
                College.Name = row["collegeName"].ToString();
                CollegeList.Add(College);
            }

            return CollegeList;
        }
        //public List<MandalModel> GetMandalList(int districtId)
        //{
        //    masterDAL = new MasterDAL();
        //    DataTable dtItems = masterDAL.GetMandals(districtId);
        //    if (dtItems == null)
        //        return null;

        //    List<MandalModel> mandalList = new List<MandalModel>();
        //    MandalModel mandal;
        //    foreach (DataRow row in dtItems.Rows)
        //    {
        //        mandal = new MandalModel();
        //        mandal.Id = Convert.ToInt32(row["Id"]);
        //        mandal.Name = row["Name"].ToString();
        //        mandalList.Add(mandal);
        //    }
        //    return mandalList;
        //}
        //public List<VillageModel> GetVillageList(int mandalId)
        //{
        //    masterDAL = new MasterDAL();
        //    DataTable dtItems = masterDAL.GetVillages(mandalId);
        //    if (dtItems == null)
        //        return null;

        //    List<VillageModel> villageList = new List<VillageModel>();
        //    VillageModel village;
        //    foreach (DataRow row in dtItems.Rows)
        //    {
        //        village = new VillageModel();
        //        village.Id = Convert.ToInt32(row["Id"]);
        //        village.Name = row["Name"].ToString();
        //        villageList.Add(village);
        //    }
        //    return villageList;
        //}
        //public void GetOwnershipMasterData(ref List<OwnershipTypeModel> ownershipTypeList, ref List<InstitutionTypeModel> institutionTypeList)
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataSet dsItems = masterDAL.GetOwnershipMasterData();
        //        if(dsItems != null)
        //        {
        //            if(dsItems.Tables[0] != null && dsItems.Tables[0].Rows.Count > 0)
        //            {
        //                OwnershipTypeModel ownershipType;
        //                foreach (DataRow row in dsItems.Tables[0].Rows)
        //                {
        //                    ownershipType = new OwnershipTypeModel();
        //                    ownershipType.Id = Convert.ToInt32(row["Id"]);
        //                    ownershipType.Name = row["Name"].ToString();
        //                    ownershipTypeList.Add(ownershipType);
        //                }
        //            }

        //            if (dsItems.Tables[1] != null && dsItems.Tables[1].Rows.Count > 0)
        //            {
        //                InstitutionTypeModel institutionType;
        //                foreach (DataRow row in dsItems.Tables[1].Rows)
        //                {
        //                    institutionType = new InstitutionTypeModel();
        //                    institutionType.Id = Convert.ToInt32(row["Id"]);
        //                    institutionType.Name = row["Name"].ToString();
        //                    institutionTypeList.Add(institutionType);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        - vinay, 20-05-2017
        //    }
        //}
        //public List<DoctorSpecialityModel> GetDoctorSpecialityList()
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataTable dtItems = masterDAL.GetDoctorSpecialityMaster();
        //        if (dtItems == null)
        //            return null;

        //        List<DoctorSpecialityModel> objList = new List<DoctorSpecialityModel>();
        //        DoctorSpecialityModel speciality;
        //        foreach (DataRow row in dtItems.Rows)
        //        {
        //            speciality = new DoctorSpecialityModel();
        //            speciality.Id = Convert.ToInt32(row["Id"]);
        //            speciality.Name = row["Name"].ToString();
        //            objList.Add(speciality);
        //        }
        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        - vinay, 30-05-2017
        //        return null;
        //    }
        //}
        //public List<FacilityMasterModel> GetFacilityList()
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataTable dtItems = masterDAL.GetFacilityMaster();
        //        if (dtItems == null)
        //            return null;

        //        List<FacilityMasterModel> objList = new List<FacilityMasterModel>();
        //        FacilityMasterModel facility;
        //        foreach (DataRow row in dtItems.Rows)
        //        {
        //            facility = new FacilityMasterModel();
        //            facility.Id = Convert.ToInt32(row["Id"]);
        //            facility.Name = row["Name"].ToString();
        //            objList.Add(facility);
        //        }
        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        - vinay, 30-05-2017
        //        return null;
        //    }
        //}
        //public List<OfferedServiceMasterModel> GetOfferedServices()
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataTable dtItems = masterDAL.GetOfferedServices();
        //        if (dtItems == null)
        //            return null;
        //        List<OfferedServiceMasterModel> objList = new List<OfferedServiceMasterModel>();
        //        OfferedServiceMasterModel model;
        //        foreach (DataRow row in dtItems.Rows)
        //        {
        //            model = new OfferedServiceMasterModel();
        //            model.Id = Convert.ToInt32(row["Id"]);
        //            model.Name = row["Name"].ToString();
        //            objList.Add(model);
        //        }
        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        - vinay, 13-06-2017
        //        return null;
        //    }
        //}

        //public List<OfferedServiceMasterModel> GetOfferedServicesByHospitalTypeId(int hospitalTypeId)
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataTable dtItems = masterDAL.GetOfferedServicesByHospitalTypeId(hospitalTypeId);
        //        if (dtItems == null)
        //            return null;
        //        List<OfferedServiceMasterModel> objList = new List<OfferedServiceMasterModel>();
        //        OfferedServiceMasterModel model;
        //        foreach (DataRow row in dtItems.Rows)
        //        {
        //            model = new OfferedServiceMasterModel();
        //            model.Id = Convert.ToInt32(row["Id"]);
        //            model.Name = row["Name"].ToString();
        //            objList.Add(model);
        //        }
        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        - vinay, 28-12-2020
        //        return null;
        //    }
        //}

        //public List<EquipmentMasterModel> GetEquipmentBasedOnOfferedServices(string offeredServiceIds)
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataTable dtItems = masterDAL.GetEquipmentBasedOnOfferedServices(offeredServiceIds);
        //        if (dtItems == null)
        //            return null;
        //        List<EquipmentMasterModel> objList = new List<EquipmentMasterModel>();
        //        EquipmentMasterModel model;
        //        foreach (DataRow row in dtItems.Rows)
        //        {
        //            model = new EquipmentMasterModel();
        //            model.Id = Convert.ToInt32(row["Id"]);
        //            model.Name = row["Name"].ToString();
        //            model.Type = row["Type"].ToString();
        //            objList.Add(model);
        //        }
        //        return objList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Write exception log        - vinay, 28-12-2020
        //        return null;
        //    }
        //}

       

        #region DesignationList by Dept
        //public List<RoleModel> GetDesignationList(int departmentId)
        //{
        //    masterDAL = new MasterDAL();
        //    DataTable dt = masterDAL.GetDesignationList(departmentId);
        //    List<RoleModel> roleList = new List<RoleModel>();
        //    if (dt != null)
        //    {

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            RoleModel role = new RoleModel();
        //            role.Id = Convert.ToInt32(row["Id"]);
        //            role.Name = row["Name"].ToString();
        //            role.DepartmentId = Convert.ToInt32(row["DepartmentId"]);
        //            roleList.Add(role);
        //        }
        //    }
        //    return roleList;
        //}

        //public List<DepartmentViewModel> GetDepartmentUsers(int RoleType, int DepartmentId)
        //{
        //    masterDAL = new MasterDAL();
        //    List<DepartmentViewModel> DeptUsersList = new List<DepartmentViewModel>();              
        //    DataTable dt = masterDAL.GetDepartmentUsersList(RoleType, DepartmentId);
        //    if (dt != null)
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            DepartmentViewModel objDeptUser = new DepartmentViewModel();
        //            objDeptUser.usermodel = new UserModel();
        //            objDeptUser.usermodel.Id = Convert.ToInt32(row["UserId"]);
        //            objDeptUser.usermodel.JurisdictionId = Convert.ToInt32(row["JurisdictionId"]);
        //            objDeptUser.usermodel.JurisdictionLevel = row["Jurisdiction"].ToString();
        //            objDeptUser.usermodel.DesignationName = row["DesignationName"].ToString();
        //            objDeptUser.usermodel.DistrictId = Convert.ToInt32(row["DistrictId"]);
        //            objDeptUser.DistrictName = row["DistrictName"].ToString();
        //            objDeptUser.usermodel.MandalId = row["MandalId"].ToString() == "" ? 0 : Convert.ToInt32(row["MandalId"]);
        //            objDeptUser.MandalName = row["MandalName"].ToString();
        //            objDeptUser.usermodel.VillageId = row["VillageId"].ToString() == "" ? 0 : Convert.ToInt32(row["VillageId"]);
        //            objDeptUser.VillageName = row["VillageName"].ToString();
        //            objDeptUser.usermodel.Address = row["Address"].ToString();
        //            objDeptUser.usermodel.FirstName = row["FirstName"].ToString();
        //            objDeptUser.usermodel.LastName = row["LastName"].ToString();
        //            objDeptUser.usermodel.UserName = row["UserName"].ToString();
        //            objDeptUser.usermodel.DesignationId = Convert.ToInt32(row["DesignationId"]);
        //            objDeptUser.usermodel.DesignationName = row["designationName"].ToString(); 
        //            //objDeptUser.usermodel.Role.Name = row["Name"].ToString();
        //            objDeptUser.usermodel.EmailId = row["EmailId"].ToString();
        //            objDeptUser.usermodel.MobileNumber = row["MobileNumber"].ToString();
        //            DeptUsersList.Add(objDeptUser);
        //        }
        //    }
        //    return DeptUsersList;
        //}
        #endregion

        //#region Equipment Details
        //public int SaveEquipmentDetails(EquipmentMasterModel equipmentModel)
        //{
        //    masterDAL = new MasterDAL();
        //    return masterDAL.SaveEquipmentDetails(equipmentModel);
        //}
        //public EquipmentMasterModel GetEquipment(int id)
        //{
        //    try
        //    {
        //        masterDAL = new MasterDAL();
        //        DataTable dtItems = masterDAL.GetEquipmentDetails(id);
        //        if (dtItems == null)
        //            return null;

        //        EquipmentMasterModel equipment = new EquipmentMasterModel();
        //        equipment.Id = Convert.ToInt32(dtItems.Rows[0]["Id"]);
        //        equipment.Name = dtItems.Rows[0]["Name"].ToString();
        //        equipment.Type = dtItems.Rows[0]["Type"].ToString();
        //        equipment.IsActive = Convert.ToBoolean(dtItems.Rows[0]["IsActive"]);

        //        return equipment;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
        //        Logger.LogError(exception);
        //        return null;
        //    }
        //}
        //public List<EquipmentMasterModel> GetEquipmentsList()
        //{
        //    masterDAL = new MasterDAL();
        //    DataTable dt = masterDAL.GetEquipmentsList();
        //    if (dt == null)
        //        return null;
        //    return ConvertToList(dt);
        //}
        //private List<EquipmentMasterModel> ConvertToList(DataTable dt)
        //{
        //    List<EquipmentMasterModel> EquipmentList = new List<EquipmentMasterModel>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        EquipmentMasterModel Equipment = new EquipmentMasterModel();
        //        Equipment.Id = Convert.ToInt32(row["Id"]);
        //        Equipment.Name = row["Name"].ToString();
        //        Equipment.Type = row["Type"].ToString();
        //        Equipment.IsActive = Convert.ToBoolean(row["IsActive"]);
        //        EquipmentList.Add(Equipment);
        //    }
        //    return EquipmentList;
        //}
        ////public bool CheckforEquipmentExistance(int id, string name)
        ////{
        ////    try
        ////    {
        ////        masterDAL = new MasterDAL();
        ////        DataTable dtItems = null;// masterDAL.CheckforEquipmentExistance(fpoId, id, name);
        ////        if (dtItems == null)
        ////            return true;

        ////        return Convert.ToBoolean(dtItems.Rows[0]["EquipmentCount"]);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
        ////        exception.CustomMessage = "Equipment Name : " + name;
        ////        Logger.LogError(exception);
        ////        return true;
        ////    }
        ////}
        //#endregion
 
    }
}
