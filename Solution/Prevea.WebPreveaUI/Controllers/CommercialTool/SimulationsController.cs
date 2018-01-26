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
    using System.Diagnostics;

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

                var user = Service.GetUser(User.Id);
                simulation.Id = data.Id;
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromSimulation,
                    NotificationStateId = (int)EnNotificationState.Assigned,
                    SimulationId = simulation.Id,
                    ToUserId = user.UserParentId,
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
                Debug.WriteLine(e.Message);

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

                if (UpdateSimulation(foreignPreventionService.Id, EnUpdateSimultationFrom.ForeignPreventionService) == Status.Error)
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
                Debug.WriteLine(e.Message);

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
                var resultService = Service.SaveAgencyService(agencyService);
                if (resultService.Status == Status.Error)
                    return Json(resultService);

                if (UpdateSimulation(agencyService.Id, EnUpdateSimultationFrom.AgencyService) == Status.Error)
                {
                    var resultUpdateSimulation = new Result
                    {
                        Status = Status.Error,
                        Object = null,
                        Message = "Error en la actualización de la Simulación"
                    };
                    return Json(resultUpdateSimulation);
                }

                resultService.Object = AutoMapper.Mapper.Map<AgencyServiceViewModel>(resultService.Object);
                return Json(resultService);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

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
                var resultService = Service.SaveTrainingService(trainingService);
                if (resultService.Status == Status.Error)
                    return Json(resultService);

                if (UpdateSimulation(trainingService.Id, EnUpdateSimultationFrom.TrainngService) == Status.Error)
                {
                    var resultUpdateSimulation = new Result
                    {
                        Status = Status.Error,
                        Object = null,
                        Message = "Error en la actualización de la Simulación"
                    };
                    return Json(resultUpdateSimulation);
                }

                return Json(resultService);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(new Result
                {
                    Status = Status.Error,
                    Message = "Ha ocurrido un error en la Grabación de la Simulación"
                });
            }
        }

        public JsonResult Simulations_Destroy()
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
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Borrada la Simulación [{simulation.CompanyName}]"
                };
                var resultNotification = Service.SaveNotification(notification);

                if (resultNotification.Status == Status.Error)
                    return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);

                resultSimulation.Object = AutoMapper.Mapper.Map<SimulationViewModel>(resultSimulation.Object);

                return this.Jsonp(simulation);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

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
        public JsonResult GetStretchAgencyByNumberEmployees(int numberEmployees)
        {
            var stretchAgency = Service.GetStretchAgencyByNumberEmployees(numberEmployees);

            return Json(new { stretchAgency }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStretchAgencyByType(int type)
        {
            var amountAgencyByType = 0.0m;
            var percentageAgencyByType = Convert.ToDecimal(Service.GetTagValue("PercentegeStretchAgencies"));

            switch (type)
            {
                case (int)EnEngagementType.Society:
                    amountAgencyByType = Convert.ToDecimal(Service.GetTagValue("AmountAgencyBySociety"));
                    break;
                case (int)EnEngagementType.Autonomous:
                    amountAgencyByType = Convert.ToDecimal(Service.GetTagValue("AmountAgencyByAutonomous"));
                    break;
            }

            return Json(new { amountAgencyByType, percentageAgencyByType }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetEngagementTypes()
        //{
        //    var engagementTypes = Service.GetEngagmentTypes();
        //    var items = new List<SelectListItem>();
        //    items.AddRange(engagementTypes.Select(engagementType => new SelectListItem
        //    {
        //        Text = engagementType.Description,
        //        Value = engagementType.Id.ToString(CultureInfo.InvariantCulture)
        //    }));

        //    return Json(items, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetEngagementTypes()
        {
            var data = Service.GetEngagmentTypes();

            return this.Jsonp(AutoMapper.Mapper.Map<List<EngagementTypeViewModel>>(data));
        }

        [HttpPost]
        public JsonResult SendToCompanies(int simulationId)
        {
            var user = Service.GetUser(User.Id);

            var resultSimulation = Service.SendToCompanies(simulationId);
            if (resultSimulation.Status == Status.Error)
                return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);

            var simulation = Service.GetSimulation(simulationId);
            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromUser,
                NotificationStateId = (int)EnNotificationState.Issued,
                SimulationId = simulationId,
                ToUserId = user.UserParentId,
                Observations =
                    $"{user.Initials} - Enviada a Empresa la Simulación [{simulation.CompanyName}]"
            };
            var resultNotification = Service.SaveNotification(notification);

            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);

            return Json(new { resultSimulation }, JsonRequestBehavior.AllowGet);
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
                NotificationTypeId = (int) EnNotificationType.FromSede,
                NotificationStateId = (int) EnNotificationState.Issued,
                SimulationId = simulationId,
                ToUserId = simulation.UserId,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Asignada la Simulación [{simulation.CompanyName}]"
            };
            var resultNotification = Service.SaveNotification(notification);

            if (resultNotification.Status == Status.Error)
                return Json(new {result = resultSimulation}, JsonRequestBehavior.AllowGet);

            resultSimulation.Object = AutoMapper.Mapper.Map<SimulationViewModel>(resultSimulation.Object);

            return Json(new {result = resultSimulation}, JsonRequestBehavior.AllowGet);
        }

        private Status UpdateSimulation(int simulationId, EnUpdateSimultationFrom updateSimultationFrom)
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
                        Observations = GetObservation(simulation, updateSimultationFrom)                         
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
                        ToUserId = user.UserParentId,
                        Observations = GetObservation(simulation, updateSimultationFrom)
                    };

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

        private string GetObservation(Simulation simulation, EnUpdateSimultationFrom updateSimultationFrom)
        {
            var user = Service.GetUser(User.Id);

            string observations;
            switch (updateSimultationFrom)
            {
                case EnUpdateSimultationFrom.ForeignPreventionService:
                    observations =
                        $"{user.Initials} - Modificada la Simulación (SPA) [{simulation.CompanyName}] -> TEC: {simulation.ForeignPreventionService.AmountTecniques}€ VS: {simulation.ForeignPreventionService.AmountHealthVigilance}€ RM: {simulation.ForeignPreventionService.AmountMedicalExamination}€ Total: {Service.GetTotalSimulation(simulation.Id)}€";

                    break;
                case EnUpdateSimultationFrom.AgencyService:
                    observations =
                        $"{user.Initials} - Modificada la Simulación (Gestoría) [{simulation.CompanyName}]";

                    break;
                case EnUpdateSimultationFrom.TrainngService:
                    observations =
                        $"{user.Initials} - Modificada la Simulación (Formación) [{simulation.CompanyName}]";

                    break;
                default:
                    observations = String.Empty;
                    break;
            }

            return observations;
        }

        [HttpPost]
        public JsonResult SendToSEDE(int simulationId)
        {
            var simulation = Service.GetSimulation(simulationId);
            var user = Service.GetUser(User.Id);
            var notification = new Model.Model.Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromSimulation,
                NotificationStateId = (int)EnNotificationState.Issued,
                SimulationId = simulationId,
                ToUserId = user.UserParentId,
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
            var user = Service.GetUser(User.Id);

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
                    $"{user.Initials} - Validada la Simulación [{simulation.CompanyName}]"
            };
            var resultNotification = Service.SaveNotification(notification);
            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

            resultSimulation.Object = AutoMapper.Mapper.Map<SimulationViewModel>(resultSimulation.Object);

            return Json(new { result = resultSimulation }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public JsonResult Courses_Read([DataSourceRequest] DataSourceRequest request, int trainingServiceId)
        {
            var data = AutoMapper.Mapper.Map<List<TrainingCourseTrainingServiceViewModel>>(Service.GetTrainingCoursesTrainingServiceByTrainingService(trainingServiceId));
            foreach (var trainingCourseTrainingService in data)
            {
                var course = Service.GetTrainingCourse(trainingCourseTrainingService.TrainingCourseId);
                trainingCourseTrainingService.OriginalPrice = course.Price;
                trainingCourseTrainingService.Desviation = (trainingCourseTrainingService.Price / course.Price) - 1;
            }

            return this.Jsonp(data);
        }

        public JsonResult Courses_Update()
        {
            try
            {
                var user = Service.GetUser(User.Id);

                var trainingCourseTrainingService = this.DeserializeObject<TrainingCourseTrainingService>("course");
                if (trainingCourseTrainingService == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Curso" });
                }  

                var result = Service.SaveTrainingCourseTrainingService(trainingCourseTrainingService);
                if (result.Status == Status.Error)
                    return Json(new { result }, JsonRequestBehavior.AllowGet);

                var trainingCourseTrainingServiceFind =
                    Service.GetTrainingCourseTrainingService(trainingCourseTrainingService.Id);
                var observations =
                    $"{user.Initials} - Actualizado el Curso {trainingCourseTrainingServiceFind.TrainingCourse.Name} en la Simulación [{trainingCourseTrainingServiceFind.TrainingService.Simulation.CompanyName}]";

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromUser,
                    NotificationStateId = (int)EnNotificationState.Validated,
                    SimulationId = trainingCourseTrainingServiceFind.TrainingServiceId,
                    ToUserId = user.UserParentId,
                    Observations = observations
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

                var data = AutoMapper.Mapper.Map<TrainingCourseTrainingServiceViewModel>(trainingCourseTrainingServiceFind);
                var course = Service.GetTrainingCourse(trainingCourseTrainingService.TrainingCourseId);
                data.OriginalPrice = course.Price;
                data.Desviation = (trainingCourseTrainingService.Price / course.Price) - 1;

                return this.Jsonp(data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Curso" });
            }
        }

        public JsonResult Courses_Destroy()
        {
            try
            {
                var user = Service.GetUser(User.Id);

                var trainingCourseTrainingService = this.DeserializeObject<TrainingCourseTrainingService>("course");
                if (trainingCourseTrainingService == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Curso" });
                }

                var trainingCourseTrainingServiceFind =
                    Service.GetTrainingCourseTrainingService(trainingCourseTrainingService.Id);
                var observations =
                    $"{user.Initials} - Borrado el Curso {trainingCourseTrainingServiceFind.TrainingCourse.Name} a la Simulación [{trainingCourseTrainingServiceFind.TrainingService.Simulation.CompanyName}]";

                var result = Service.DeleteTrainingCourseTrainingService(trainingCourseTrainingService.Id);
                if (result.Status == Status.Error)
                    return Json(new { result }, JsonRequestBehavior.AllowGet);

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromUser,
                    NotificationStateId = (int)EnNotificationState.Validated,
                    SimulationId = trainingCourseTrainingServiceFind.TrainingServiceId,
                    ToUserId = user.UserParentId,
                    Observations = observations                       
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);
                
                return this.Jsonp(trainingCourseTrainingService);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Curso" });
            }
        }

        public JsonResult Courses_Create()
        {
            try
            {
                var user = Service.GetUser(User.Id);

                var trainingCourseTrainingService = this.DeserializeObject<TrainingCourseTrainingService>("course");
                if (trainingCourseTrainingService == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Curso" });
                }

                var result = Service.SaveTrainingCourseTrainingService(trainingCourseTrainingService);
                if (result.Status == Status.Error)
                    return Json(new { result }, JsonRequestBehavior.AllowGet);

                var trainingCourseTrainingServiceFind =
                    Service.GetTrainingCourseTrainingService(trainingCourseTrainingService.Id);
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromSede,
                    NotificationStateId = (int)EnNotificationState.Validated,
                    SimulationId = trainingCourseTrainingService.TrainingServiceId,
                    ToUserId = user.UserParentId,
                    Observations =
                        $"{user.Initials} - Agregado el Curso {trainingCourseTrainingServiceFind.TrainingCourse.Name } a la Simulación [{trainingCourseTrainingServiceFind.TrainingService.Simulation.CompanyName}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

                var data = AutoMapper.Mapper.Map<TrainingCourseTrainingServiceViewModel>(trainingCourseTrainingService);
                var course = Service.GetTrainingCourse(trainingCourseTrainingService.TrainingCourseId);
                data.OriginalPrice = course.Price;
                data.Desviation = (trainingCourseTrainingService.Price / course.Price) - 1;

                return this.Jsonp(data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Curso" });
            }
        }

        [HttpPost]
        public JsonResult FindNode(string text)
        {
            var resultSearch = new Result();

            var trainingCourse = Service.FindTrainingCourse(text);
            if (trainingCourse == null)
            {
                resultSearch.Status = Status.Error;
                resultSearch.Object = null;
                resultSearch.Message = "Texto no encontrado";
                return Json(new { result = resultSearch }, JsonRequestBehavior.AllowGet);
            }

            resultSearch.Status = Status.Ok;
            resultSearch.Object = AutoMapper.Mapper.Map<TrainingCourseViewModel>(trainingCourse);
            resultSearch.Message = "Texto encontrado";
            return Json(new { result = resultSearch }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChooseCourse()
        {
            return PartialView("~/Views/CommercialTool/Simulations/ChooseCourse.cshtml");
        }
    }

    public enum EnUpdateSimultationFrom { ForeignPreventionService, AgencyService, TrainngService }
}