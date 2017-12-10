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
        public JsonResult Simulations_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<SimulationViewModel>>(Service.GetSimulationsByUser(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult Simulations_Create()
        {
            const string errorSimulation = "Se ha producido un error en la Grabación de la Simulación";

            try
            {                
                var simulation = this.DeserializeObject<SimulationViewModel>("simulation");
                if (simulation == null)
                {
                    return this.Jsonp(new { Errors = errorSimulation });
                }

                var data = AutoMapper.Mapper.Map<Simulation>(simulation);

                data.UserId = User.Id;
                data.SimulationStateId = (int) EnSimulationState.ValidationPending;
                var result = Service.SaveSimulation(data);

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = errorSimulation });

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromSimulation,
                    NotificationStateId = (int)EnNotificationState.Issued,
                    SimulationId = data.Id,
                    ToRoleId = (int)EnRole.PreveaPersonal,
                    Observations = $"{Service.GetUser(User.Id).Initials} - Creación de la Simulación"
                };
                var resultNotification = Service.SaveNotification(notification);

                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = errorSimulation });

                simulation.Id = data.Id;
                return this.Jsonp(simulation);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = errorSimulation });
            }
        }

        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult DetailSimulation(int simulationId, int selectTabId)
        {
            ViewBag.SelectTabId = selectTabId;

            return PartialView("~/Views/CommercialTool/Simulations/DetailSimulation.cshtml",
                Service.GetSimulation(simulationId));
        }

        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult ForeignPreventionService(int simulationId)
        {
            var foreignPreventionService = Service.GetForeignPreventionService(simulationId) ?? new ForeignPreventionService
            {
                Id = simulationId,
                Simulation = Service.GetSimulation(simulationId)
            };

            return PartialView("~/Views/CommercialTool/Simulations/ForeignPreventionService.cshtml",
                foreignPreventionService);
        }

        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult AgencyService(int simulationId)
        {
            var agencyService = Service.GetAgencyService(simulationId) ?? new AgencyService
            {
                Id = simulationId,
                Simulation = Service.GetSimulation(simulationId)
            };

            return PartialView("~/Views/CommercialTool/Simulations/AgencyService.cshtml",
                agencyService);
        }

        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult TrainingService(int simulationId)
        {
            var trainingService = Service.GetTrainingService(simulationId) ?? new TrainingService
            {
                Id = simulationId,
                Simulation = Service.GetSimulation(simulationId)
            };

            return PartialView("~/Views/CommercialTool/Simulations/TrainingService.cshtml",
                trainingService);
        }

        [HttpPost]
        public ActionResult ForeignPreventionService(ForeignPreventionService foreignPreventionService)
        {
            try
            {
                var result = Service.SaveForeignPreventionService(foreignPreventionService);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(new Result { Status = Status.Error, Message = "Ha ocurrido un error en la Grabación de la Simulación" });
            }
        }

        [HttpPost]
        public ActionResult AgencyService(AgencyService agencyService)
        {
            try
            {
                var result = Service.SaveAgencyService(agencyService);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(new Result { Status = Status.Error, Message = "Ha ocurrido un error en la Grabación de la Simulación" });
            }
        }

        [HttpPost]
        public ActionResult TrainingService(TrainingService trainingService)
        {
            try
            {
                var result = Service.SaveTrainingService(trainingService);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(new Result { Status = Status.Error, Message = "Ha ocurrido un error en la Grabación de la Simulación" });
            }
        }

        public ActionResult Simulations_Destroy()
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
        public JsonResult SendNotificationFromSimulation(int simulationId)
        {
            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromSimulation,
                NotificationStateId = (int)EnNotificationState.Issued,
                Observations = $"Notificación {Service.GetNotifications().Count + 1}"
            };
            var result = Service.SaveNotification(notification);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendToCompanies(int simulationId)
        {
            var result = Service.SendToCompanies(simulationId);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}