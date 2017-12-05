namespace Prevea.WebPreveaUI.Controllers.CommercialTool
{
    #region Using

    using System.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using HelpersClass;

    #endregion

    public class SimulationsController : BaseController
    {
        #region Constructor
        public SimulationsController(IService service) : base(service)
        {
        }

        #endregion

        // GET: Simulations
        public ActionResult Simulations()
        {
            return PartialView("~/Views/CommercialTool/Simulations/Simulations.cshtml");
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult Simulators_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<SimulatorViewModel>>(Service.GetSimulatorsByUser(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult Simulators_Create()
        {
            try
            {
                var simulation = this.DeserializeObject<SimulatorViewModel>("simulation");
                if (simulation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Simulación" });
                }

                var data = AutoMapper.Mapper.Map<Simulator>(simulation);

                data.UserId = User.Id;
                var result = Service.SaveSimulator(data);

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Simulación" });

                simulation.Id = data.Id;
                return this.Jsonp(simulation);
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
            return PartialView("~/Views/CommercialTool/Simulations/EditSimulation.cshtml",
                Service.GetSimulator(simulatorId));
        }

        [HttpPost]
        public ActionResult EditSimulator(Simulator simulator)
        {
            try
            {
                simulator.UserId = User.Id;
                var result = Service.UpdateSimulator(simulator.Id, simulator);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(new Result { Status = Status.Error, Message = "Ha ocurrido un error en la Grabación de la Simulación" });
            }
        }

        public ActionResult Simulators_Destroy()
        {
            try
            {
                var simulator = this.DeserializeObject<Simulator>("simulator");
                if (simulator == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Simulación" });
                }

                var result = Service.DeleteSimulator(simulator.Id);

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
    }
}