namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using IService.IService;
    using System;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Model.Model;
    using Model.ViewModel;
    using Common;

    #endregion

    public partial class CompanyController
    {
        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult Simulators()
        {
            return PartialView();
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult Simulators_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<SimulationViewModel>>(Service.GetSimulationsByUser(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult Simulators_Create()
        {
            try
            {
                var simulator = this.DeserializeObject<SimulationViewModel>("simulator");
                if (simulator == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Simulación" });
                }

                var data = AutoMapper.Mapper.Map<Simulation>(simulator);

                data.UserId = User.Id;
                var result = Service.SaveSimulation(data);

                if (result.Status == Status.Error)
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación de la Simulación"});

                simulator.Id = data.Id;
                return this.Jsonp(simulator);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Simulación" });
            }
        }

        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult EditSimulator(int simulatorId)
        {
            return PartialView(Service.GetSimulation(simulatorId));
        }

        [HttpPost]
        public ActionResult EditSimulator(Simulation simulator)
        {
            try
            {
                simulator.UserId = User.Id;
                var result = Service.UpdateSimulation(simulator.Id, simulator);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json( new Result { Status = Status.Error, Message = "Ha ocurrido un error en la Grabación de la Simulación"});
            }
        }

        public ActionResult Simulators_Destroy()
        {
            try
            {
                var simulator = this.DeserializeObject<Simulation>("simulator");
                if (simulator == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Simulación" });
                }

                var result = Service.DeleteSimulation(simulator.Id);

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(simulator);
                }

                return result.Status != Status.Error ? this.Jsonp(simulator) : this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Simulación" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Simulación" });
            }
        }

        [HttpPost]
        public JsonResult GetStretchCalculateByNumberEmployees(int numberEmployees)
        {
            var stretchCalculate = Service.GetStretchCalculateByNumberEmployees(numberEmployees);

            return Json(new { stretchCalculate }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendNotificationFromSimulator(int simulatorId)
        {
            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromSimulator,
                NotificationStateId = (int)EnNotificationState.Issued,
                Observations = $"Notificación {Service.GetNotifications().Count + 1}"
            };
            var result = Service.SaveNotification(notification);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendToCompanies(int simulatorId)
        {
            var result = Service.SendToCompanies(simulatorId);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult CompanyFromSimulator(int simulatorId)
        {
            var simulator = Service.GetSimulation(simulatorId);

            ViewBag.SelectTabId = 0;

            return PartialView("DetailCompany", simulator.SimulationCompany.Company);           
        }
    }
}