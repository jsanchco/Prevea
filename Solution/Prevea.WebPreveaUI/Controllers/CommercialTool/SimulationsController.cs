namespace Prevea.WebPreveaUI.Controllers.CommercialTool
{
    #region Using

    using System.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using System.Linq;
    using Common;
    using HelpersClass;
    using Model.ViewModel;

    #endregion

    public class SimulationsController : BaseController
    {
        #region Constructor

        public SimulationsController(IService service) : base(service)
        {
        }

        #endregion

        [AppAuthorize(Roles = "Super,PreveaPersonal,PreveaCommercial")]
        public ActionResult Simulations()
        {
            return PartialView("~/Views/CommercialTool/Simulations/Simulations.cshtml");
        }

        [HttpGet]
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
                    return this.Jsonp(new {Errors = errorSimulation});
                }

                var data = AutoMapper.Mapper.Map<Simulation>(simulation);

                data.UserId = User.Id;
                data.SimulationStateId = (int) EnSimulationState.ValidationPending;
                var result = Service.SaveSimulation(data);

                if (result.Status == Status.Error)
                    return this.Jsonp(new {Errors = errorSimulation});

                simulation.Id = data.Id;
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromSimulation,
                    NotificationStateId = (int)EnNotificationState.Assigned,
                    SimulationId = simulation.Id,
                    ToRoleId = (int)EnRole.PreveaPersonal,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Alta de la Simulación [{simulation.CompanyName}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                return this.Jsonp(simulation);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = errorSimulation});
            }
        }

        [AppAuthorize(Roles = "Super,PreveaPersonal,PreveaCommercial")]
        public ActionResult DetailSimulation(int simulationId, int selectTabId)
        {
            ViewBag.SelectTabId = selectTabId;

            return PartialView("~/Views/CommercialTool/Simulations/DetailSimulation.cshtml",
                Service.GetSimulation(simulationId));
        }

        public ActionResult ForeignPreventionService(int simulationId)
        {
            var foreignPreventionService = Service.GetForeignPreventionService(simulationId) ??
                                           new ForeignPreventionService
                                           {
                                               Id = simulationId,
                                               Simulation = Service.GetSimulation(simulationId)
                                           };

            return PartialView("~/Views/CommercialTool/Simulations/ForeignPreventionService.cshtml",
                foreignPreventionService);
        }

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
                var resultService = Service.SaveForeignPreventionService(foreignPreventionService);
                if (resultService.Status == Status.Error)
                    return Json(resultService);

                if (UpdateSimulation(foreignPreventionService.Id) == Status.Error)
                {
                    var resultUpdateSimulation = new Result
                    {
                        Status = Status.Error,
                        Object = null,
                        Message = "Error en la actualización de la Simulación"
                    };
                    return Json(resultUpdateSimulation);
                }

                return Json(new Result { Status = Status.Ok });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(new Result
                {
                    Status = Status.Error,
                    Message = "Ha ocurrido un error en la Grabación de la Simulación"
                });
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

                return Json(new Result
                {
                    Status = Status.Error,
                    Message = "Ha ocurrido un error en la Grabación de la Simulación"
                });
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

                return Json(new Result
                {
                    Status = Status.Error,
                    Message = "Ha ocurrido un error en la Grabación de la Simulación"
                });
            }
        }

        public ActionResult Simulations_Destroy()
        {
            try
            {
                var simulation = this.DeserializeObject<Simulation>("simulation");
                if (simulation == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado de la Simulación"});
                }

                var resultSimulation = Service.SubscribeSimulation(simulation.Id, false);
                if (resultSimulation.Status == Status.Error)
                    return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromSimulation,
                    NotificationStateId = (int)EnNotificationState.Issued,
                    SimulationId = simulation.Id,
                    ToUserId = simulation.UserAssignedId,
                    ToRoleId = (int)EnRole.PreveaPersonal,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Borrada la Simulación [{simulation.CompanyName}]"
                };
                var resultNotification = Service.SaveNotification(notification);

                if (resultNotification.Status == Status.Error)
                    return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);

                resultSimulation.Object = AutoMapper.Mapper.Map<SimulationViewModel>(resultSimulation.Object);

                return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado de la Simulación"});
            }
        }

        [HttpPost]
        public JsonResult GetStretchCalculateByNumberEmployees(int numberEmployees)
        {
            var stretchCalculate = Service.GetStretchCalculateByNumberEmployees(numberEmployees);

            return Json(new {stretchCalculate}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendToCompanies(int simulationId)
        {
            var result = Service.SendToCompanies(simulationId);

            return Json(new {result}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AssignSimulation(int simulationId)
        {
            var simulation = Service.GetSimulation(simulationId);
            simulation.UserAssignedId = User.Id;
            simulation.DateAssigned = DateTime.Now;

            var resultSimulation = Service.UpdateSimulation(simulationId, simulation);
            if (resultSimulation.Status == Status.Error)
                return Json(new {result = resultSimulation}, JsonRequestBehavior.AllowGet);

            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int) EnNotificationType.FromSimulation,
                NotificationStateId = (int) EnNotificationState.Issued,
                SimulationId = simulationId,
                ToUserId = simulation.UserId,
                ToRoleId = (int) EnRole.PreveaPersonal,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Asignada la Simulación [{simulation.CompanyName}]"
            };
            var resultNotification = Service.SaveNotification(notification);

            if (resultNotification.Status == Status.Error)
                return Json(new {result = resultSimulation}, JsonRequestBehavior.AllowGet);

            resultSimulation.Object = AutoMapper.Mapper.Map<SimulationViewModel>(resultSimulation.Object);

            return Json(new {result = resultSimulation}, JsonRequestBehavior.AllowGet);
        }

        private Status UpdateSimulation(int simulationId)
        {
            var user = Service.GetUser(User.Id);
            var simulation = Service.GetSimulation(simulationId);

            Result resultSimulation;
            Result resultNotification;
            Model.Model.Notification notification;
            
            switch (user.UserRoles.First().RoleId)
            {
                case (int) EnRole.Super:
                case (int) EnRole.PreveaPersonal:
                    simulation.SimulationStateId = (int) EnSimulationState.Modificated;
                    resultSimulation = Service.UpdateSimulation(simulation.Id, simulation);

                    if (resultSimulation.Status == Status.Error)
                        return Status.Error;

                    notification = new Model.Model.Notification
                    {
                        DateCreation = DateTime.Now,
                        NotificationTypeId = (int) EnNotificationType.FromSede,
                        NotificationStateId = (int) EnNotificationState.Issued,
                        SimulationId = simulationId,
                        ToUserId = simulation.UserId,
                        Observations =
                            $"{Service.GetUser(User.Id).Initials} - Modificada la Simulación [{simulation.CompanyName}] -> TEC: {simulation.ForeignPreventionService.AmountTecniques}€ VS: {simulation.ForeignPreventionService.AmountHealthVigilance}€ RM: {simulation.ForeignPreventionService.AmountMedicalExamination}€"
                    };
                    resultNotification = Service.SaveNotification(notification);

                    if (resultNotification.Status == Status.Error)
                        return Status.Error;

                    return Status.Ok;

                case (int)EnRole.PreveaCommercial:
                    simulation.SimulationStateId = (int)EnSimulationState.ValidationPending;
                    resultSimulation = Service.UpdateSimulation(simulation.Id, simulation);

                    notification = new Model.Model.Notification
                    {
                        DateCreation = DateTime.Now,
                        NotificationTypeId = (int)EnNotificationType.FromUser,
                        NotificationStateId = (int)EnNotificationState.Issued,
                        SimulationId = simulationId,
                        Observations =
                            $"{Service.GetUser(User.Id).Initials} - Modificada la Simulación (SPA) [{simulation.CompanyName}] -> TEC: {simulation.ForeignPreventionService.AmountTecniques}€ VS: {simulation.ForeignPreventionService.AmountHealthVigilance}€ RM: {simulation.ForeignPreventionService.AmountMedicalExamination}€"
                    };
                    if (simulation.UserAssignedId != null)
                    {
                        notification.ToUserId = simulation.UserAssignedId;
                    }
                    else
                    {
                        notification.ToRoleId = (int) EnRole.PreveaPersonal;
                    }

                    resultNotification = Service.SaveNotification(notification);

                    if (resultNotification.Status == Status.Error)
                        return Status.Error;

                    if (resultSimulation.Status == Status.Error)
                        return Status.Error;

                    return Status.Ok;

                default:
                    return Status.Ok;
            }
        }

        [HttpPost]
        public JsonResult SendToSEDE(int simulationId)
        {
            var simulation = Service.GetSimulation(simulationId);
            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromSimulation,
                NotificationStateId = (int)EnNotificationState.Issued,
                SimulationId = simulationId,
                ToRoleId = (int)EnRole.PreveaPersonal,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Modificación de la Simulación [{simulation.CompanyName}]"
            };
            var result = Service.SaveNotification(notification);

            result.Object = AutoMapper.Mapper.Map<NotificationViewModel>(result.Object);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendNotificationValidateToUser(int simulationId)
        {
            var simulation = Service.GetSimulation(simulationId);
            simulation.SimulationStateId = (int) EnSimulationState.Validated;

            var resultSimulation = Service.UpdateSimulation(simulationId, simulation);
            if (resultSimulation.Status == Status.Error)
                return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);

            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromSede,
                NotificationStateId = (int)EnNotificationState.Validated,
                SimulationId = simulationId,
                ToUserId = simulation.UserId,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Validada la Simulación [{simulation.CompanyName}]"
            };
            var resultNotification = Service.SaveNotification(notification);
            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

            resultSimulation.Object = AutoMapper.Mapper.Map<SimulationViewModel>(resultSimulation.Object);

            return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);
        }        
    }
}