namespace Prevea.WebPreveaUI.Controllers.MedicalExamination
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using IService.IService;
    using Model.Model;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Model.ViewModel;
    using Common;
    using System;
    using System.Diagnostics;
    using System.Linq;

    #endregion

    public class HistoricMedicalExaminationController : BaseController
    {
        #region Constructor
        public HistoricMedicalExaminationController(IService service) : base(service)
        {
        }
        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "ContactPerson")]
        public ActionResult HistoricMedicalExamination()
        {
            var contactPerson = Service.GetContactPersonByUserId(User.Id);
            if (contactPerson != null)
            {
                ViewBag.ContactPersonId = contactPerson.Id;
                ViewBag.CompanyId = contactPerson.CompanyId;

                return PartialView("~/Views/MedicalExamination/Historic/HistoricMedicalExamination.cshtml");
            }

            return PartialView("~/Views/Error/AccessDenied.cshtml");
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminations_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<RequestMedicalExaminationsViewModel>>(Service.GetRequestMedicalExaminationsByCompany(companyId));

            return this.Jsonp(data);
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminations_Create()
        {
            const string errorRequestMedicalExamination = "Se ha producido un error en la Grabación de RequestMedicalExamination";

            try
            {
                var requestMedicalExamination = this.DeserializeObject<RequestMedicalExaminationsViewModel>("requestMedicalExamination");
                if (requestMedicalExamination == null)
                {
                    return this.Jsonp(new { Errors = errorRequestMedicalExamination });
                }

                var data = AutoMapper.Mapper.Map<RequestMedicalExaminations>(requestMedicalExamination);
                var result = Service.SaveRequestMedicalExaminations(data);

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = errorRequestMedicalExamination });

                var contactPerson = Service.GetContactPersonByUserId(User.Id);
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromRequestMedicalExamination,
                    NotificationStateId = (int)EnNotificationState.Assigned,
                    ToUserId = contactPerson.Company.SimulationCompanyActive.Simulation.UserAssignedId,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Alta de la Petición de Reconocimiento Médico [{contactPerson.Company.Name}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                return this.Jsonp(requestMedicalExamination);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = errorRequestMedicalExamination });
            }
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminations_Update()
        {
            const string errorRequestMedicalExamination = "Se ha producido un error en la Actualización de RequestMedicalExamination";

            try
            {
                var requestMedicalExamination = this.DeserializeObject<RequestMedicalExaminationsViewModel>("requestMedicalExamination");
                if (requestMedicalExamination == null)
                {
                    return this.Jsonp(new { Errors = errorRequestMedicalExamination });
                }

                var data = AutoMapper.Mapper.Map<RequestMedicalExaminations>(requestMedicalExamination);
                var result = Service.SaveRequestMedicalExaminations(data);

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = errorRequestMedicalExamination });

                var contactPerson = Service.GetContactPersonByUserId(User.Id);
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromRequestMedicalExamination,
                    NotificationStateId = (int)EnNotificationState.Assigned,
                    ToUserId = contactPerson.Company.SimulationCompanyActive.Simulation.UserAssignedId,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Actualizada la Petición de Reconocimiento Médico [{contactPerson.Company.Name}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                var requestMedicalExaminationsViewModel = result.Object as RequestMedicalExaminations;
                if (requestMedicalExaminationsViewModel != null)
                    requestMedicalExamination.Id = requestMedicalExaminationsViewModel.Id;
                else
                    return this.Jsonp(new { Errors = "RequestMedicalExamination in NULL" });

                return this.Jsonp(requestMedicalExamination);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = errorRequestMedicalExamination });
            }
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminations_Destroy()
        {
            const string errorRequestMedicalExamination = "Se ha producido un error en el Borrado de RequestMedicalExamination";

            try
            {
                var requestMedicalExamination = this.DeserializeObject<RequestMedicalExaminationsViewModel>("requestMedicalExamination");
                if (requestMedicalExamination == null)
                {
                    return this.Jsonp(new { Errors = errorRequestMedicalExamination });
                }
                
                var result = Service.DeleteRequestMedicalExamination(requestMedicalExamination.Id);

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = errorRequestMedicalExamination });

                var contactPerson = Service.GetContactPersonByUserId(User.Id);
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromRequestMedicalExamination,
                    NotificationStateId = (int)EnNotificationState.Assigned,
                    ToUserId = contactPerson.Company.SimulationCompanyActive.Simulation.UserAssignedId,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Eliminada la Petición de Reconocimiento Médico [{contactPerson.Company.Name}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                return this.Jsonp(requestMedicalExamination);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = errorRequestMedicalExamination });
            }
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminationEmployees_Read([DataSourceRequest] DataSourceRequest request, int requestMedicalExaminationId, int companyId)
        {
            var requestMedicalExamination = Service.GetRequestMedicalExaminationById(requestMedicalExaminationId);
            var requestMedicalExaminationEmployeesByRequestMedicalExamination =
                    Service.GetRequestMedicalExaminationEmployees()
                    .Where(x => x.RequestMedicalExaminationsId == requestMedicalExaminationId)
                    .ToList();

            var employeesByCompany = Service.GetEmployeesByCompany(companyId)
                .Where(x => x.User.UserStateId == (int)EnUserState.Alta).ToList();

            var listEmployees = new List<RequestMedicalExaminationEmployeeViewModel>();            
            foreach(var employee in employeesByCompany)
            {
                var name = string.Empty;
                if (employee.User.FirstName != null)
                    name += $"{employee.User.FirstName} ";
                if (employee.User.LastName != null)
                    name += employee.User.LastName;

                var findEmployeesByRequestMedicalExamination = requestMedicalExaminationEmployeesByRequestMedicalExamination.Find(x => x.EmployeeId == employee.Id);
                if (findEmployeesByRequestMedicalExamination == null)
                {
                    listEmployees.Add(new RequestMedicalExaminationEmployeeViewModel
                    {
                        Date = new DateTime(requestMedicalExamination.Date.Year, requestMedicalExamination.Date.Month, requestMedicalExamination.Date.Day, 0, 0, 0),
                        EmployeeId = employee.Id,
                        EmployeeName = name,
                        EmployeeDNI = employee.User.DNI,
                        Included = false,
                        RequestMedicalExaminationsId = requestMedicalExaminationId
                    });
                }
                else
                {
                    var requestMedicalExaminationEmployee = Service.GetRequestMedicalExaminationEmployeeById(findEmployeesByRequestMedicalExamination.Id);
                    listEmployees.Add(new RequestMedicalExaminationEmployeeViewModel
                    {
                        Date = requestMedicalExaminationEmployee.Date,
                        EmployeeId = employee.Id,
                        EmployeeName = name,
                        EmployeeDNI = employee.User.DNI,
                        Included = true,
                        RequestMedicalExaminationsId = requestMedicalExaminationId
                    });
                }
            }            

            return this.Jsonp(listEmployees);
        }
        
        public JsonResult UpdateRequestHistoricMedicalExaminationEmployees(List<RequestMedicalExaminationEmployeeViewModel> listEmployees)
        {
            if (listEmployees == null || listEmployees.Count == 0)
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

            var requestMedicalExaminationsId = listEmployees[0].RequestMedicalExaminationsId;
            var requestMedicalExaminationEmployeesByRequestMedicalExamination =
                    Service.GetRequestMedicalExaminationEmployees()
                    .Where(x => x.RequestMedicalExaminationsId == requestMedicalExaminationsId)
                    .ToList();
            foreach (var requestMedicalExaminationEmployee in requestMedicalExaminationEmployeesByRequestMedicalExamination)
            {
                var removeEmployee =
                    Service.DeleteRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee.Id);
                if (removeEmployee.Status == Status.Error)
                    return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }
            foreach (var employee in listEmployees)
            {
                if (!employee.Included)
                    continue;

                var requestMedicalExaminationEmployee = new RequestMedicalExaminationEmployee
                {
                    Date = employee.Date,
                    EmployeeId = employee.EmployeeId,
                    RequestMedicalExaminationEmployeeStateId = (int) EnRequestMedicalExaminationEmployeeState.Pending,
                    RequestMedicalExaminationsId = requestMedicalExaminationsId
                };
                var saveEmployee = Service.SaveRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);
                if (saveEmployee.Status == Status.Error)
                    return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { resultStatus = Status.Ok }, JsonRequestBehavior.AllowGet);
        }
    }
}