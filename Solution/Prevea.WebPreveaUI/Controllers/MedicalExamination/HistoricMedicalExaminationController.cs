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
    using IRepository.IRepository;

    #endregion

    public class HistoricMedicalExaminationController : BaseController
    {
        #region Constructor
        public HistoricMedicalExaminationController(IRepository repository) : base(repository)
        {
        }
        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Super,ContactPerson,PreveaPersonal")]
        public ActionResult HistoricMedicalExamination()
        {
            var contactPerson = Service.GetContactPersonByUserId(User.Id);
            if (contactPerson != null)
            {
                ViewBag.ContactPersonId = contactPerson.Id;
                ViewBag.CompanyId = contactPerson.CompanyId;
            }
            else
            {
                ViewBag.ContactPersonId = 0;
                ViewBag.CompanyId = 0;
            }

            return PartialView("~/Views/MedicalExamination/HistoricMedicalExamination.cshtml");
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminations_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            if (companyId != 0)
            {
                var data = AutoMapper.Mapper.Map<List<RequestMedicalExaminationsViewModel>>(Service.GetRequestMedicalExaminationsByCompany(companyId));
                return this.Jsonp(data);
            }
            else
            {
                var data = AutoMapper.Mapper.Map<List<RequestMedicalExaminationsViewModel>>(Service.GetRequestMedicalExaminations());
                return this.Jsonp(data);
            }
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
                
                return this.Jsonp(AutoMapper.Mapper.Map<RequestMedicalExaminationsViewModel>(Service.GetRequestMedicalExaminationById(((RequestMedicalExaminations)result.Object).Id)));
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

                
                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromRequestMedicalExamination,
                    NotificationStateId = (int)EnNotificationState.Assigned  
                };

                var contactPerson = Service.GetContactPersonByUserId(User.Id);
                if (contactPerson != null)
                {
                    notification.ToUserId = contactPerson.Company.SimulationCompanyActive.Simulation.UserAssignedId;
                    notification.Observations =
                        $"{Service.GetUser(User.Id).Initials} - Actualizada la Petición de Reconocimiento Médico [{contactPerson.Company.Name}]";
                }
                    
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
                if (employee.User.WorkStationId == null)
                    continue;

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
                        RequestMedicalExaminationsId = requestMedicalExaminationId,
                        ChangeDate = false,
                        ClinicId = null                                         
                    });
                }
                else
                {
                    var requestMedicalExaminationEmployee = Service.GetRequestMedicalExaminationEmployeeById(findEmployeesByRequestMedicalExamination.Id);
                    if (requestMedicalExaminationEmployee.ClinicId == null)
                        requestMedicalExaminationEmployee.ClinicId = 0;

                    requestMedicalExaminationEmployee.SplitDoctors =
                        requestMedicalExaminationEmployee.DoctorsMedicalExaminationEmployee.Select(x => x.DoctorId).ToArray();
                    requestMedicalExaminationEmployee.Doctors = string.Join(",", requestMedicalExaminationEmployee.SplitDoctors);

                    listEmployees.Add(new RequestMedicalExaminationEmployeeViewModel
                    {
                        Id = requestMedicalExaminationEmployee.Id,
                        Date = requestMedicalExaminationEmployee.Date,
                        EmployeeId = employee.Id,
                        EmployeeName = name,
                        EmployeeDNI = employee.User.DNI,
                        Included = true,
                        RequestMedicalExaminationsId = requestMedicalExaminationId,
                        ChangeDate = requestMedicalExaminationEmployee.ChangeDate,
                        ClinicId = requestMedicalExaminationEmployee.ClinicId,
                        SplitDoctors = requestMedicalExaminationEmployee.SplitDoctors,
                        Doctors = requestMedicalExaminationEmployee.Doctors,
                        Observations = requestMedicalExaminationEmployee.Observations,
                        SamplerNumber = requestMedicalExaminationEmployee.SamplerNumber
                    });
                }
            }            

            return this.Jsonp(listEmployees);
        }
        
        public JsonResult UpdateRequestHistoricMedicalExaminationEmployees(List<RequestMedicalExaminationEmployeeViewModel> listEmployees)
        {
            var employee = listEmployees.FirstOrDefault();
            if (employee != null)
            {
                var requestMedicalExaminationEmployee = Service.GetRequestMedicalExaminationEmployeeById(employee.Id);
                if (requestMedicalExaminationEmployee?.RequestMedicalExaminations.RequestMedicalExaminationStateId ==
                    (int) EnRequestMedicalExaminationState.Blocked)
                {
                    return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
                }
            }

            var updateRequestHistoricMedicalExaminationEmployees = Service.UpdateRequestHistoricMedicalExaminationEmployees(AutoMapper.Mapper.Map<List<RequestMedicalExaminationEmployee>>(listEmployees), User.Id);
            
            if (updateRequestHistoricMedicalExaminationEmployees.Status == Status.Error)
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

            var data = AutoMapper.Mapper.Map<RequestMedicalExaminationsViewModel>(updateRequestHistoricMedicalExaminationEmployees.Object);
            return Json(new { resultStatus = Status.Ok, requestMedicalExamination = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClinics([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<ClinicViewModel>>(Service.GetClincs());

            return this.Jsonp(data);
        }

        public JsonResult GetDoctors([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<DoctorViewModel>>(Service.GetUsersInRoles(new List<string> { "Doctor" }));

            return this.Jsonp(data);
        }
    }
}