using Capstone.BAL;
using Capstone.Framework;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    public class MasterController : Controller
    {
        MasterBAL masterBAL;
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult AddOfferedServices()
        {
            LicenseBAL objBAL = new LicenseBAL();
            //ViewBag.DistrictList = objBAL.GetCountries();
            ViewBag.HospitalTypesList = objBAL.GetHospitalTypes();
            ViewBag.OfferedServicesList = objBAL.GetOfferedServices();
            return View();
        }

        public JsonResult SaveOfferedServicesEquipments(OfferedServiceEquipmentsMasterModel objData)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                masterBAL = new MasterBAL();
                int result = 0;

                var user = Session.GetDataFromSession<UserModel>("User");
                objData.UserId = user.Id;


                //var x = masterBAL.CheckforServiceEquipmentExistance(objData.HospitalTypeId, objData.EquipmentIds,objData.ServiceId);
                //if (x)
                //{
                //    notification.Title = "Error";
                //    notification.NotificationType = NotificationType.Danger;
                //    notification.NotificationMessage = "EquipmentDetails details already existed";
                //    notification.ShowNonActionButton = true;
                //    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                //    notification.NonActionButtonText = "OK";

                //    return Json(new
                //    {
                //        notification = notification
                //    });
                //}

                result = masterBAL.SaveOfferedServicesEquipments(objData);

                if (result > 0)
                {
                    string msg = string.Empty;
                    if (objData.Id == 0)
                        msg = "Equipment details saved successfully.";
                    else
                        msg = "Equipment details updated successfully.";

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = msg;
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "OK";
                }
                else
                {
                    notification.Title = "Error";
                    notification.NotificationType = NotificationType.Danger;
                    notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    notification.NonActionButtonText = "OK";
                }
                //  return Json(notification);
                return Json(new
                {
                    notification = notification,
                    EquipmentsList = masterBAL.GetOfferedServicesEquipmentsList()
                });
            }
            else
            {
                #region Preparing Modal Errors
                var modelStateErrors = ModelStateErrorHandler.GetModelStateErrors(ModelState);

                // Error Notification
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Please clear validations and try again";
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.ShowListItems = true;
                notification.ListItems = modelStateErrors;
                notification.ShowNonActionButton = true;
                notification.NonActionButtonText = "Close";
                #endregion

                return Json(new
                {
                    notification = notification
                });
            }
        }

        public JsonResult GetOfferedServicesEquipmentsList()
        {
            masterBAL = new MasterBAL();
            List<OfferedServiceEquipmentsComplexViewModel> List = masterBAL.GetOfferedServicesEquipmentsList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditOfferedService(int id)
        {
            masterBAL = new MasterBAL();
            var equipment = masterBAL.GetOfferedServiceEquipmentsDetails(id);
            return Json(equipment);
        }

        #region Equipment Master Details
        public ActionResult AddEquipmentMaster()
        {
            return View();
        }
        
        public JsonResult SaveEquipment1(EquipmentMasterModel equipemntData)
        {
            NotificationModel notification = new NotificationModel();
            if (ModelState.IsValid)
            {
                masterBAL = new MasterBAL();
                int result = 0;

                var user = Session.GetDataFromSession<UserModel>("User");
                equipemntData.UserId = user.Id;

                //var x = masterBAL.CheckforEquipmentExistance(equipemntData.Id, equipemntData.Name);
                //if (x)
                //{
                //    notification.Title = "Error";
                //    notification.NotificationType = NotificationType.Danger;
                //    notification.NotificationMessage = "Crop details already existed";
                //    notification.ShowNonActionButton = true;
                //    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                //    notification.NonActionButtonText = "OK";

                //    return Json(new
                //    {
                //        notification = notification
                //    });
                //}

                result = masterBAL.SaveEquipmentDetails(equipemntData);

                if (result > 0)
                {
                    string msg = string.Empty;
                    if (equipemntData.Id == 0)
                        msg = "Equipemnt details saved successfully.";
                    else
                        msg = "Crop details updated successfully.";

                    notification.Title = "Success";
                    notification.NotificationType = NotificationType.Success;
                    notification.NotificationMessage = msg;
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Success;
                    notification.NonActionButtonText = "OK";
                }
                else
                {
                    notification.Title = "Error";
                    notification.NotificationType = NotificationType.Danger;
                    notification.NotificationMessage = "Something went wrong! Please contact Help desk";
                    notification.ShowNonActionButton = true;
                    notification.NonActionButtonClassType = PopupButtonClass.Danger;
                    notification.NonActionButtonText = "OK";
                }
                //  return Json(notification);
                return Json(new
                {
                    notification = notification,
                    EquipmentsList = ""//masterBAL.GetEquipmentsList()
                });
            }
            else
            {
                #region Preparing Modal Errors
                var modelStateErrors = ModelStateErrorHandler.GetModelStateErrors(ModelState);

                // Error Notification
                notification.Title = "Error";
                notification.NotificationType = NotificationType.Danger;
                notification.NotificationMessage = "Please clear validations and try again";
                notification.NonActionButtonClassType = PopupButtonClass.Danger;
                notification.ShowListItems = true;
                notification.ListItems = modelStateErrors;
                notification.ShowNonActionButton = true;
                notification.NonActionButtonText = "Close";
                #endregion

                return Json(new
                {
                    notification = notification
                });
            }
        }

        public JsonResult EditEquipment(int id)
        {
            masterBAL = new MasterBAL();
            var equipment = "";//masterBAL.GetEquipment(id);
            return Json(equipment);
        }

        [HttpPost]
        public ActionResult SaveEquipment(EquipmentMasterModel model)
        {
            int result = 0;
            if (ModelState.ContainsKey("Id"))
                ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                masterBAL = new MasterBAL();
                model.UserId = Session.GetDataFromSession<UserModel>("User").DepartmentId;
                if (model.Id == 0)
                    model.UserId = Session.GetDataFromSession<UserModel>("User").Id;
                else
                    model.UserId = Session.GetDataFromSession<UserModel>("User").CreatedUserId;
                EquipmentComplexViewModel equipment = new EquipmentComplexViewModel();

                if (model.Type == null || model.Type == "")
                    model.Type = "Equipment";
                result = masterBAL.SaveEquipmentDetails(model);
                NotificationModel Notification = new NotificationModel();
                if (result > 0)
                {
                    Notification.Title = "Sucess";
                    Notification.NotificationType = NotificationType.Success;
                        if(model.Id > 0)
                    Notification.NotificationMessage = model.Name + " Updated Successfully";
                        else
                        Notification.NotificationMessage = model.Name + " Saved Successfully";
                }
                else
                {
                    Notification.Title = "Error";
                    Notification.NotificationType = NotificationType.Danger;
                    Notification.NotificationMessage = "Oops! something went wrong. Please contact helpdesk";
                }
                //equipment.DesignationList = DesignationList;
                equipment.Notification = Notification;
                return Json(equipment);

            }
            else
            {
                return RedirectToAction("CreateEquipment");
            }
        }
        public JsonResult GetEquipments()
        {
            masterBAL = new MasterBAL();
            List<EquipmentMasterModel> List = masterBAL.GetEquipmentsList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}