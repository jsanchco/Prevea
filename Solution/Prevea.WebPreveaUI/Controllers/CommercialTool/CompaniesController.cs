namespace Prevea.WebPreveaUI.Controllers.CommercialTool
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using System.Diagnostics;
    using System.Web;
    using Model.CustomModel;
    using IRepository.IRepository;
    using Prevea.Service.Service;

    #endregion

    public class CompaniesController : BaseController
    {
        #region Constructor
        public CompaniesController(IRepository repository) : base(repository)
        {
        }
        #endregion

        #region Companies

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public ActionResult Companies()
        {
            return PartialView("~/Views/CommercialTool/Companies/Companies.cshtml");
        }

        [HttpGet]
        public ActionResult DetailCompany(int id, int selectTabId)
        {
            var company = Service.GetCompany(id);

            ViewBag.SelectTabId = selectTabId;

            var simulationActive = Service.GetSimulationActive(id);
            var employeesActives = company.Employees.Where(x => x.User.UserStateId == (int) EnUserState.Alta).ToList();
            if (simulationActive.NumberEmployees != employeesActives.Count)
                ViewBag.Error = new[] { "El número de Trabajadores no coincide con los Datos de la Simulación" };

            return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
        }

        [HttpGet]
        public ActionResult DetailCompanyByContactPerson()
        {
            var companies = Service.GetCompanies();
            foreach (var company in companies)
            {
                var contactPerson = company.ContactPersons.FirstOrDefault(x => x.UserId == User.Id);
                if (contactPerson != null)
                {
                    return RedirectToAction("DetailCompany", new { id = company.Id, selectTabId = 0 });
                }
            }

            return PartialView("~/Views/Error/AccessDenied.cshtml");
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public JsonResult Companies_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<CompanyViewModel>>(Service.GetCompaniesByUser(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult Companies_Create()
        {
            try
            {
                var company = this.DeserializeObject<Company>("company");
                if (company == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Empresa" });
                }

                company.FromSimulation = false;
                company.GestorId = User.Id;
                var result = Service.SaveCompany(company);

                return result.Status != Status.Error ? this.Jsonp(company) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Empresa" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Empresa" });
            }
        }

        [HttpPost]
        public JsonResult Company_Subscribe(int companyId, bool subscribe)
        {
            var result = Service.SubscribeCompany(companyId, subscribe);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Simulations

        [HttpGet]
        public ActionResult SimulationsCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/SimulationsCompany.cshtml", Service.GetCompany(companyId));
        }

        #endregion

        #region GeneralData

        [HttpGet]
        public ActionResult GeneralDataCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/GeneralDataCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpPost]
        public ActionResult UpdateGeneralDataCompany(Company company)
        {
            try
            {
                var result = Service.UpdateCompany(company.Id, company);

                ViewBag.SelectTabId = 1;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = "La Empresa se ha actualizado correctamente";

                    return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", result.Object as Company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
            }
        }

        [HttpGet]
        public JsonResult GetCnaes()
        {
            var cnaes = Service.GetCnaes();
            var items = new List<SelectListItem>();
            items.AddRange(cnaes.Select(cnae => new SelectListItem
            {
                Text = $"{cnae.CustomKey} {cnae.Name}",
                Value = cnae.Id.ToString(CultureInfo.InvariantCulture)
            }));

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Agency

        [HttpGet]
        public ActionResult AgencyCompany(int companyId)
        {
            var company = Service.GetCompany(companyId);

            var agency = company.Agency == null ? new AgencyViewModel { CompanyId = companyId } : AutoMapper.Mapper.Map<AgencyViewModel>(company.Agency);

            return PartialView("~/Views/CommercialTool/Companies/AgencyCompany.cshtml", agency);
        }

        [HttpPost]
        public ActionResult SaveAgencyCompany(AgencyViewModel agency)
        {
            try
            {
                var agencySave = AutoMapper.Mapper.Map<Agency>(agency);

                var result = agency.Id != 0 ? Service.UpdateAgency(agencySave, agency.Id) : Service.SaveAgency(agencySave, agency.CompanyId);

                var company = Service.GetCompany(agency.CompanyId);

                ViewBag.SelectTabId = 3;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(agency.CompanyId);

                ViewBag.SelectTabId = 3;

                ViewBag.Error = new List<string> { e.Message };

                return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
            }
        }

        #endregion

        #region Economic Data

        [HttpGet]
        public ActionResult EconomicDataCompany(int companyId)
        {
            var company = Service.GetCompany(companyId);

            ViewBag.Total = Math.Round(Service.GetTotalSimulation(company.SimulationCompanyActive.SimulationId), 2);

            var simulationCompany = new SimulationCompany
            {
                CompanyId = companyId,
                Company = company,
                SimulationId = company.SimulationCompanyActive.SimulationId,
                Simulation = company.SimulationCompanyActive.Simulation
            };
            return PartialView("~/Views/CommercialTool/Companies/EconomicDataCompany.cshtml", simulationCompany);
        }

        [HttpGet]
        public ActionResult EconomicDataSimulation(int simulationId)
        {
            var simulation = Service.GetSimulation(simulationId);

            ViewBag.Total = Math.Round(Service.GetTotalSimulation(simulationId), 2);

            var company = simulation.SimulationCompanies.FirstOrDefault().Company;
            var simulationCompany = new SimulationCompany
            {
                CompanyId = company.Id,
                Company = company,
                SimulationId = simulationId,
                Simulation = simulation
            };
            return PartialView("~/Views/CommercialTool/Companies/EconomicDataCompany.cshtml", simulationCompany);
        }

        [HttpPost]
        public JsonResult UpdateIncludeInContractualDocument(int simulationId, string contractualDocument, bool check)
        {
            try
            {
                var simulation = Service.GetSimulation(simulationId);

                if (simulation == null)
                    return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

                Result result;
                switch (contractualDocument)
                {
                    case "switchForeignPreventionService":
                        simulation.ForeignPreventionService.IncludeInContractualDocument = check;
                        result = Service.SaveForeignPreventionService(simulation.ForeignPreventionService);
                        if (result.Status == Status.Error)
                            return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
                        break;
                    case "switchAgencyService":
                        simulation.AgencyService.IncludeInContractualDocument = check;
                        result = Service.SaveAgencyService(simulation.AgencyService);
                        if (result.Status == Status.Error)
                            return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
                        break;
                    case "switchTrainingService":
                        simulation.TrainingService.IncludeInContractualDocument = check;
                        result = Service.SaveTrainingService(simulation.TrainingService);
                        if (result.Status == Status.Error)
                            return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
                        break;
                }

                return Json(new { resultStatus = Status.Ok }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Payment Method

        [HttpGet]
        public ActionResult PaymentMethodCompany(int companyId)
        {
            var company = Service.GetCompany(companyId);

            if (company.PaymentMethod == null)
            {
                return PartialView("~/Views/CommercialTool/Companies/PaymentMethodCompany.cshtml", new PaymentMethodViewModel
                {
                    Id = companyId
                });
            }

            var data = AutoMapper.Mapper.Map<PaymentMethodViewModel>(company.PaymentMethod);
            return PartialView("~/Views/CommercialTool/Companies/PaymentMethodCompany.cshtml", data);
        }

        [HttpPost]
        public ActionResult UpdatePaymentMethodCompany(PaymentMethodViewModel paymentMethod)
        {
            try
            {
                var paymentMethodSave = AutoMapper.Mapper.Map<PaymentMethod>(paymentMethod);
                var result = Service.SavePaymentMethod(paymentMethodSave);

                var company = Service.GetCompany(paymentMethod.Id);

                ViewBag.SelectTabId = 6;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(paymentMethod.Id);

                ViewBag.SelectTabId = 6;

                ViewBag.Error = new List<string> { e.Message };

                return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
            }
        }

        [HttpGet]
        public JsonResult GetSplitPayment()
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Enero", Value = 1.ToString() },
                new SelectListItem { Text = "Febrero", Value = 2.ToString() },
                new SelectListItem { Text = "Marzo", Value = 3.ToString() },
                new SelectListItem { Text = "Abril", Value = 4.ToString() },
                new SelectListItem { Text = "Mayo", Value = 5.ToString() },
                new SelectListItem { Text = "Junio", Value = 6.ToString() },
                new SelectListItem { Text = "Julio", Value = 7.ToString() },
                new SelectListItem { Text = "Agosto", Value = 8.ToString() },
                new SelectListItem { Text = "Septiembre", Value = 9.ToString() },
                new SelectListItem { Text = "Octubre", Value = 10.ToString() },
                new SelectListItem { Text = "Noviembre", Value = 11.ToString() },
                new SelectListItem { Text = "Diciembre", Value = 12.ToString() }
            };

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetModesPayment()
        {
            var modesPayment = Service.GetModesPayment();
            var items = new List<SelectListItem>();
            items.AddRange(modesPayment.Select(modePayment => new SelectListItem
            {
                Text = modePayment.Description,
                Value = modePayment.Id.ToString(CultureInfo.InvariantCulture)
            }));

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetModesPaymentMedicalExamination()
        {
            var modesPaymentMedicalExamination = Service.GetModesPaymentMedicalExamination();
            var items = new List<SelectListItem>();
            items.AddRange(modesPaymentMedicalExamination.Select(modePaymentMedicalExamination => new SelectListItem
            {
                Text = modePaymentMedicalExamination.Description,
                Value = modePaymentMedicalExamination.Id.ToString(CultureInfo.InvariantCulture)
            }));

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employees

        [HttpGet]
        public ActionResult EmployeesCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/EmployeesCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial,ContactPerson")]
        public JsonResult EmployeesCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetEmployeesByCompany(companyId).Select(x => x.User));
            foreach (var employee in data)
            {
                employee.CompanyId = companyId;
                if (employee.WorkStationId == null)
                    employee.WorkStationId = 0;
            }
 
            return this.Jsonp(data);
        }

        public JsonResult EmployeesCompany_Update()
        {
            try
            {
                var employee = this.DeserializeObject<UserViewModel>("employee");
                if (employee == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
                }

                if (employee.WorkStationId == 0)
                {
                    employee.WorkStationId = null;
                    employee.WorkStationName = null;
                }

                var data = AutoMapper.Mapper.Map<User>(employee);
                var result = Service.SaveEmployeeCompany((int)EnRole.Employee, (int)employee.CompanyId, data);

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                    {
                        employee.Id = user.Id;

                        var notification = new Model.Model.Notification
                        {
                            DateCreation = DateTime.Now,
                            NotificationTypeId = (int)EnNotificationType.FromUser,
                            NotificationStateId = (int)EnNotificationState.Assigned,
                            Observations =
                                $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}]"
                        };

                        var userNotification = Service.GetUser(User.Id);
                        var company = Service.GetCompany((int)employee.CompanyId);
                        if (userNotification.UserRoles.FirstOrDefault().RoleId == (int)EnRole.ContactPerson)
                        {
                            notification.ToUserId = company.GestorId;
                            var resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new { Errors = resultNotification });

                            notification = new Model.Model.Notification
                            {
                                DateCreation = DateTime.Now,
                                NotificationTypeId = (int)EnNotificationType.FromUser,
                                NotificationStateId = (int)EnNotificationState.Assigned,
                                Observations =
                                    $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}]"
                            };
                            var userGestor = Service.GetUser(company.GestorId);
                            notification.ToUserId = userGestor.UserParentId;
                            resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new { Errors = resultNotification });
                        }
                        else
                        {
                            foreach (var contactPerson in company.ContactPersons)
                            {
                                notification = new Model.Model.Notification
                                {
                                    DateCreation = DateTime.Now,
                                    NotificationTypeId = (int)EnNotificationType.FromUser,
                                    NotificationStateId = (int)EnNotificationState.Assigned,
                                    Observations =
                                        $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}]"
                                };
                                notification.ToUserId = contactPerson.UserId;
                                var resultNotification = Service.SaveNotification(notification);
                                if (resultNotification.Status == Status.Error)
                                    return this.Jsonp(new { Errors = resultNotification });
                            }
                        }
                    }

                    if (employee.WorkStationId != null)
                    {
                        var workStation = Service.GetWorkStationById((int)employee.WorkStationId);
                        employee.WorkStationName = string.IsNullOrEmpty(workStation.ProfessionalCategory)
                            ? workStation.Name
                            : $"{workStation.Name} ({workStation.ProfessionalCategory})";
                    }

                    return this.Jsonp(employee);
                }

                return result.Status != Status.Error ? this.Jsonp(employee) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
        }

        public ActionResult EmployeesCompany_Destroy()
        {
            try
            {
                var employee = this.DeserializeObject<UserViewModel>("employee");
                if (employee == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Trabajador" });
                }

                var result = Service.DeleteEmployeeCompany((int)employee.CompanyId, (int)employee.Id);

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                    {
                        employee.Id = user.Id;

                        var company = Service.GetCompany((int)employee.CompanyId);
                        var notification = new Model.Model.Notification
                        {
                            DateCreation = DateTime.Now,
                            NotificationTypeId = (int)EnNotificationType.FromUser,
                            NotificationStateId = (int)EnNotificationState.Assigned,
                            Observations =
                                $"{Service.GetUser(User.Id).Initials} - Eliminado el Trabajador [{user.Initials}] de la Empresa [{company.Name}]"
                        };

                        var userNotification = Service.GetUser(User.Id);                        
                        if (userNotification.UserRoles.FirstOrDefault().RoleId == (int)EnRole.ContactPerson)
                        {
                            notification.ToUserId = company.GestorId;
                            var resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new { Errors = resultNotification });

                            notification = new Model.Model.Notification
                            {
                                DateCreation = DateTime.Now,
                                NotificationTypeId = (int)EnNotificationType.FromUser,
                                NotificationStateId = (int)EnNotificationState.Assigned,
                                Observations =
                                    $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                            };
                            var userGestor = Service.GetUser(company.GestorId);
                            notification.ToUserId = userGestor.UserParentId;
                            resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new { Errors = resultNotification });
                        }
                        else
                        {
                            foreach (var contactPerson in company.ContactPersons)
                            {
                                notification = new Model.Model.Notification
                                {
                                    DateCreation = DateTime.Now,
                                    NotificationTypeId = (int)EnNotificationType.FromUser,
                                    NotificationStateId = (int)EnNotificationState.Assigned,
                                    Observations =
                                        $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                                };
                                notification.ToUserId = contactPerson.UserId;
                                var resultNotification = Service.SaveNotification(notification);
                                if (resultNotification.Status == Status.Error)
                                    return this.Jsonp(new { Errors = resultNotification });
                            }
                        }
                    }

                    if (employee.WorkStationId != null)
                    {
                        var workStation = Service.GetWorkStationById((int)employee.WorkStationId);
                        employee.WorkStationName = string.IsNullOrEmpty(workStation.ProfessionalCategory)
                            ? workStation.Name
                            : $"{workStation.Name} ({workStation.ProfessionalCategory})";
                    }

                    return this.Jsonp(employee);
                }

                return result.Status != Status.Error ? this.Jsonp(employee) : this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Trabajador" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Trabajador" });
            }
        }

        public ActionResult EmployeesCompany_Create()
        {
            try
            {
                var employee = this.DeserializeObject<UserViewModel>("employee");
                if (employee == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
                }

                if (employee.WorkStationId == 0)
                {
                    employee.WorkStationId = null;
                    employee.WorkStationName = null;
                }

                var result = Service.SaveEmployeeCompany((int)EnRole.Employee, (int)employee.CompanyId, AutoMapper.Mapper.Map<User>(employee));

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                    { 
                        employee.Id = user.Id;

                        var company = Service.GetCompany((int)employee.CompanyId);
                        var notification = new Model.Model.Notification
                        {
                            DateCreation = DateTime.Now,
                            NotificationTypeId = (int)EnNotificationType.FromUser,
                            NotificationStateId = (int)EnNotificationState.Assigned,
                            Observations =
                                $"{Service.GetUser(User.Id).Initials} - Alta del Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                        };

                        var userNotification = Service.GetUser(User.Id);                        
                        if (userNotification.UserRoles.FirstOrDefault().RoleId == (int) EnRole.ContactPerson)
                        {
                            notification.ToUserId = company.GestorId;
                            var resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new { Errors = resultNotification });

                            notification = new Model.Model.Notification
                            {
                                DateCreation = DateTime.Now,
                                NotificationTypeId = (int)EnNotificationType.FromUser,
                                NotificationStateId = (int)EnNotificationState.Assigned,
                                Observations =
                                    $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                            };
                            var userGestor = Service.GetUser(company.GestorId);
                            notification.ToUserId = userGestor.UserParentId;
                            resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new {Errors = resultNotification});
                        }
                        else
                        {                            
                            foreach (var contactPerson in company.ContactPersons)
                            {
                                notification = new Model.Model.Notification
                                {
                                    DateCreation = DateTime.Now,
                                    NotificationTypeId = (int)EnNotificationType.FromUser,
                                    NotificationStateId = (int)EnNotificationState.Assigned,
                                    Observations =
                                        $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                                };
                                notification.ToUserId = contactPerson.UserId;
                                var resultNotification = Service.SaveNotification(notification);
                                if (resultNotification.Status == Status.Error)
                                    return this.Jsonp(new { Errors = resultNotification });
                            }                            
                        }   
                    }

                    if (employee.WorkStationId != null)
                    {
                        var workStation = Service.GetWorkStationById((int)employee.WorkStationId);
                        employee.WorkStationName = string.IsNullOrEmpty(workStation.ProfessionalCategory)
                            ? workStation.Name
                            : $"{workStation.Name} ({workStation.ProfessionalCategory})";
                    }
                    
                    return this.Jsonp(employee);
                }

                return result.Status != Status.Error ? this.Jsonp(employee) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
        }

        [HttpPost]
        public JsonResult EmployeesCompany_Subscribe(int companyId, int userId, bool subscribe)
        {
            try
            {
                var result = Service.SubscribeEmployeeCompany(companyId, userId, subscribe);
                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                    {
                        var company = Service.GetCompany(companyId);
                        var notification = new Model.Model.Notification
                        {
                            DateCreation = DateTime.Now,
                            NotificationTypeId = (int) EnNotificationType.FromUser,
                            NotificationStateId = (int) EnNotificationState.Assigned,
                            Observations =
                                $"{Service.GetUser(User.Id).Initials} - Baja del Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                        };

                        var userNotification = Service.GetUser(User.Id);                        
                        if (userNotification.UserRoles.FirstOrDefault().RoleId == (int) EnRole.ContactPerson)
                        {
                            notification.ToUserId = company.GestorId;
                            var resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new {Errors = resultNotification});

                            notification = new Model.Model.Notification
                            {
                                DateCreation = DateTime.Now,
                                NotificationTypeId = (int)EnNotificationType.FromUser,
                                NotificationStateId = (int)EnNotificationState.Assigned,
                                Observations =
                                    $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                            };
                            var userGestor = Service.GetUser(company.GestorId);
                            notification.ToUserId = userGestor.UserParentId;
                            resultNotification = Service.SaveNotification(notification);
                            if (resultNotification.Status == Status.Error)
                                return this.Jsonp(new { Errors = resultNotification });
                        }
                        else
                        {                            
                            foreach (var contactPerson in company.ContactPersons)
                            {
                                notification = new Model.Model.Notification
                                {
                                    DateCreation = DateTime.Now,
                                    NotificationTypeId = (int)EnNotificationType.FromUser,
                                    NotificationStateId = (int)EnNotificationState.Assigned,
                                    Observations =
                                        $"{Service.GetUser(User.Id).Initials} - Actualizado el Trabajador [{user.Initials}] de la Empresa [{company.Name}"
                                };
                                notification.ToUserId = contactPerson.UserId;
                                var resultNotification = Service.SaveNotification(notification);
                                if (resultNotification.Status == Status.Error)
                                    return this.Jsonp(new {Errors = resultNotification});
                            }
                        }
                    }
                }

                return Json(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta del Trabajador" } : new { Errors = "Se ha producido un error al Dar de Baja del Trabajador" });
            }
        }

        [HttpGet]
        public JsonResult GetWorkStations([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var company = Service.GetCompany(companyId);
            var workStations = company.CnaeId != null ? Service.GetWorkStationsByCnaeId((int) company.CnaeId) : Service.GetWorkStations();
            //var data = new List<GenericModelDropDown> { new GenericModelDropDown { Id = 0, Name = "Selecciona ..." } };
            var data = new List<GenericModelDropDown>();
            foreach (var workStation in workStations)
            {
                data.Add(new GenericModelDropDown
                {
                    Id = workStation.Id,
                    Name = string.IsNullOrEmpty(workStation.ProfessionalCategory) ? workStation.Name : $"{workStation.Name} ({workStation.ProfessionalCategory})"
                });
            }

            return this.Jsonp(data);
        }

        #endregion

        #region ContactPersons

        [HttpGet]
        public ActionResult ContactPersonsCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/ContactPersonsCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial,ContactPerson")]
        public JsonResult ContactPersonsCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetContactPersonsByCompany(companyId));
            foreach (var contactPerson in data)
            {
                contactPerson.CompanyId = companyId;
                if (contactPerson.Id != null)
                {
                    var cp = Service.GetContactPersonByUserId((int)contactPerson.Id);
                    contactPerson.ContactPersonTypeId = cp.ContactPersonTypeId;
                    contactPerson.ContactPersonTypeDescription = cp.ContactPersonType.Description;
                }
            }

            return this.Jsonp(data);
        }

        public JsonResult ContactPersonsCompany_Update()
        {
            try
            {
                var contactPerson = this.DeserializeObject<UserViewModel>("contactPerson");
                if (contactPerson == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
                }

                var data = AutoMapper.Mapper.Map<User>(contactPerson);
                var result = Service.SaveContactPersonCompany((int)EnRole.ContactPerson, (int)contactPerson.CompanyId, contactPerson.ContactPersonTypeId, data);

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                    {
                        contactPerson.Id = user.Id;

                        var cp = Service.GetContactPersonByUserId((int)contactPerson.Id);
                        contactPerson.ContactPersonTypeId = cp.ContactPersonTypeId;
                        contactPerson.ContactPersonTypeDescription = cp.ContactPersonType.Description;
                    }

                    return this.Jsonp(contactPerson);
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
        }

        public ActionResult ContactPersonsCompany_Destroy()
        {
            try
            {
                var contactPerson = this.DeserializeObject<UserViewModel>("contactPerson");
                if (contactPerson == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Persona de Contacto" });
                }

                var result = Service.DeleteContactPersonCompany((int)contactPerson.CompanyId, (int)contactPerson.Id);

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(contactPerson);
                }

                return result.Status != Status.Error ? this.Jsonp(contactPerson) : this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Persona de Contacto" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Persona de Contacto" });
            }
        }

        public ActionResult ContactPersonsCompany_Create()
        {
            try
            {
                var contactPerson = this.DeserializeObject<UserViewModel>("contactPerson");
                if (contactPerson == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
                }

                var result = Service.SaveContactPersonCompany((int)EnRole.ContactPerson, (int)contactPerson.CompanyId, contactPerson.ContactPersonTypeId, AutoMapper.Mapper.Map<User>(contactPerson));

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                    {
                        contactPerson.Id = user.Id;

                        var cp = Service.GetContactPersonByUserId((int)contactPerson.Id);
                        contactPerson.ContactPersonTypeId = cp.ContactPersonTypeId;
                        contactPerson.ContactPersonTypeDescription = cp.ContactPersonType.Description;
                    }

                    return this.Jsonp(contactPerson);
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
        }

        [HttpPost]
        public JsonResult ContactPersonsCompany_Subscribe(int companyId, int userId, bool subscribe)
        {
            try
            {
                var result = Service.SubscribeContactPersonCompany(companyId, userId, subscribe);

                return Json(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta a la Persona de Contacto" } : new { Errors = "Se ha producido un error al Dar de Baja a la Persona de Contacto" });
            }
        }

        public JsonResult GetContactPersonTypesRemainingByCompany([DataSourceRequest] DataSourceRequest request, int companyId, int contactPersonTypeSelected)
        {
            var contactPersonTypesRemaining = Service.GetContactPersonTypesRemainingByCompany(companyId);
            if (contactPersonTypeSelected != 0 && contactPersonTypeSelected != (int)EnContactPersonType.Invited)
            {
                contactPersonTypesRemaining.Add(Service.GetContactPersonTypeById(contactPersonTypeSelected));
            }
            var data = AutoMapper.Mapper.Map<List<ContactPersonTypeViewModel>>(contactPersonTypesRemaining);

            return this.Jsonp(data);
        }

        [HttpPost]
        public JsonResult IsContactPersonInvited(int userId)
        {
            var contactPerson = Service.GetContactPersonByUserId(userId);
            if (contactPerson != null && contactPerson.ContactPersonTypeId == (int)EnContactPersonType.Invited)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Contractual Documents

        #region CRUD
        [HttpGet]
        public ActionResult ContractualsDocumentsCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/ContractualsDocumentsCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial,ContactPerson")]
        public JsonResult ContractualsDocumentsCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<DocumentViewModel>>(Service.GetDocumentsContractualsByCompany(companyId));

            return this.Jsonp(data);
        }

        public ActionResult ContractualsDocumentsCompany_Create()
        {
            try
            {
                var document = this.DeserializeObject<Document>("document");
                if (document?.CompanyId == null)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });

                var company = Service.GetCompany((int)document.CompanyId);
                document.Company = company;
                var error = Service.VerifyNewContractualDocument(document);
                if (!string.IsNullOrEmpty(error))  
                    return this.Jsonp(new { Errors = error });

                var documentUserOwners = new List<DocumentUserOwner>();                
                if (company?.ContactPersons != null)
                    documentUserOwners.AddRange(company.ContactPersons.Select(contactPerson => new DocumentUserOwner {UserId = contactPerson.UserId}));

                string extension = null;
                var area = Service.GetArea(document.AreaId);
                if (area.EntityId != 1)
                {
                    extension = area.Id == 13 ? string.Empty : ".pdf";
                }
                document.Area = area;
                                    
                var result = Service.SaveDocument(
                    document, 
                    false, 
                    new List<DocumentUserCreator> { new DocumentUserCreator { UserId = User.Id} },
                    documentUserOwners,
                    extension);

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });

                //document = Service.GetDocument(((Document)result.Object).Id);
                //if (document.AreaId == 9) // Otros documentos
                //    return this.Jsonp(AutoMapper.Mapper.Map<DocumentViewModel>(document));

                //if (!CreatePdf(document, Constants.FooterPdf))  
                //    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });

                if (((Document) result.Object).SimulationId != null)
                    ((Document) result.Object).Simulation =
                        Service.GetSimulation((int)((Document) result.Object).SimulationId);

                var data = AutoMapper.Mapper.Map<DocumentViewModel>(document);

                return this.Jsonp(data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
            }
        }

        public ActionResult ContractualsDocumentsCompany_Destroy()
        {
            try
            {
                var contractualDocument = this.DeserializeObject<DocumentViewModel>("document");
                if (contractualDocument == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
                }

                var result = Service.DeleteDocument(contractualDocument.Id);
                if (result.Status == Status.Ok)
                    return this.Jsonp(contractualDocument);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GenerateReport(int documentId)
        {
            try
            {
                var document = Service.GetDocument(documentId);
                if (document == null)
                    return Json(new { result = false, message = "Documento no encontrado" }, JsonRequestBehavior.AllowGet);

                switch (document.AreaId)
                {
                    case 6: // OFE_SPA
                        return Json(Service.GenerateOfferSPAReport(document, Server.MapPath("~")), JsonRequestBehavior.AllowGet);

                    default:
                        return Json(new Result { Status = Status.Error, Message = "Imposible generar el Documento" }, JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(new Result { Status = Status.Error, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ContractualDocumentReport(int documentId)
        {
            var document = Service.GetDocument(documentId);
            ViewBag.fileName = $"{document.Name}{document.Extension}";

            switch (document.AreaId)
            {
                #region OFE_SPA
                case 6: // OFE_SPA
                    var result = Service.GenerateOfferSPAReport(document, Server.MapPath("~"));
                    if (result.Status != Status.Error)
                    {
                        var fileName = $"~\\App_Data\\PDF\\{document.Name}{document.Extension}";
                        return DownloadPdfByUrl(fileName);
                        //return File(fileName, "application/pdf", Server.UrlEncode(fileName));
                    }        

                    if (result.Status == Status.Error)
                        return View("~/Views/CommercialTool/Companies/Reports/ContractSPAReport.cshtml", document);

                    ViewBag.ContractualDocumentId = documentId;
                    ViewBag.ContractualDocumentEnrollment = document.Name;
                    ViewBag.IVA = Service.GetTagValue("IVA");

                    var workCenters = Service.GetWorkCentersByCompany((int)document.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
                    ViewBag.NumberWorkCenters = workCenters.Count;

                    var provincesWorkCenters = string.Empty;
                    if (workCenters.Count > 0)
                    {
                        var distinctWorkCenters = workCenters
                            .GroupBy(x => x.Province.Trim())
                            .Select(g => new
                            {
                                Field = g.Key,
                                Count = g.Count()
                            }).ToList();


                        if (distinctWorkCenters.Count == 1)
                        {
                            provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                                $"{distinctWorkCenters[0].Field.Trim()}." :
                                $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                        }
                        else
                        {
                            for (var i = 0; i < distinctWorkCenters.Count; i++)
                            {
                                var workCenter = distinctWorkCenters[i];
                                var newWorkCenter = workCenter.Field.Trim();
                                if (newWorkCenter != string.Empty)
                                {
                                    if (i == distinctWorkCenters.Count - 1)
                                    {
                                        provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                            $"{newWorkCenter}." :
                                            $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                                    }
                                    else
                                    {
                                        provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                            $"{newWorkCenter}, " :
                                            $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                                    }
                                }
                            }
                        }
                    }
                    ViewBag.ProvincesWorkCenters = provincesWorkCenters;
                    
                    return View("~/Views/CommercialTool/Companies/Reports/OfferSPAReport.cshtml", document);
                #endregion

                #region OFE_FOR
                case 7: // OFE_FOR
                    return View("~/Views/CommercialTool/Companies/Reports/OfferTrainingReport.cshtml", document.Company);
                #endregion

                #region OFE_GES
                case 8: // OFE_GES
                    return View("~/Views/CommercialTool/Companies/Reports/OfferAgencyReport.cshtml", document.Company);
                #endregion

                #region CON_SPA
                case 9: // CON_SPA
                    ViewBag.ContractualDocumentId = documentId;
                    ViewBag.ContractualDocumentEnrollment = document.Name;
                    ViewBag.IVA = Service.GetTagValue("IVA");

                    workCenters = Service.GetWorkCentersByCompany((int)document.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
                    ViewBag.NumberWorkCenters = workCenters.Count;

                    provincesWorkCenters = string.Empty;
                    if (workCenters.Count > 0)
                    {
                        var distinctWorkCenters = workCenters
                            .GroupBy(x => x.Province.Trim())
                            .Select(g => new
                            {
                                Field = g.Key,
                                Count = g.Count()
                            }).ToList();


                        if (distinctWorkCenters.Count == 1)
                        {
                            provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                                $"{distinctWorkCenters[0].Field.Trim()}." :
                                $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                        }
                        else
                        {
                            for (var i = 0; i < distinctWorkCenters.Count; i++)
                            {
                                var workCenter = distinctWorkCenters[i];
                                var newWorkCenter = workCenter.Field.Trim();
                                if (newWorkCenter != string.Empty)
                                {
                                    if (i == distinctWorkCenters.Count - 1)
                                    {
                                        provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                            $"{newWorkCenter}." :
                                            $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                                    }
                                    else
                                    {
                                        provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                            $"{newWorkCenter}, " :
                                            $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                                    }
                                }
                            }
                        }
                    }
                    ViewBag.ProvincesWorkCenters = provincesWorkCenters;

                    return View("~/Views/CommercialTool/Companies/Reports/ContractSPAReport.cshtml", document);
                #endregion

                #region CON_FOR
                case 10: // CON_FOR
                    return View("~/Views/CommercialTool/Companies/Reports/ContractTrainingReport.cshtml", document.Company);
                #endregion

                #region CON_GES
                case 11: // CON_GES
                    return View("~/Views/CommercialTool/Companies/Reports/ContractAgencyReport.cshtml", document.Company);
                #endregion

                default:
                    return View("~/Views/Error/AccessDenied.cshtml");                                 
            }
        }

        [HttpGet]
        public ActionResult AddDocumentFirmed(int companyId, int contractualDocumentId)
        {
            var contractualDocument = Service.GetDocument(contractualDocumentId);

            ViewBag.CompanyId = companyId;
            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.Enrollment = contractualDocument.Name;

            return PartialView("~/Views/CommercialTool/Companies/AddDocumentFirmed.cshtml");
        }

        [HttpGet]
        public ActionResult AddOtherDocument(int contractualDocumentId)
        {
            var contractualDocument = Service.GetDocument(contractualDocumentId);

            return PartialView("~/Views/CommercialTool/Companies/AddOtherDocument.cshtml", contractualDocument);
        }

        [HttpGet]
        public ActionResult AddSignature(int contractualDocumentId)
        {
            var document = Service.GetDocument(contractualDocumentId);
            var signature = new Signature
            {
                DocumentId = contractualDocumentId,
                MySignature = document.Signature
            };

            return PartialView("~/Views/Shared/AddSignature.cshtml", signature);
        }

        [HttpPost]
        public ActionResult AddSignature(Signature signature)
        {
            return PartialView(signature);
        }

        [HttpPost]
        public ActionResult SaveDocumentFirmed(IEnumerable<HttpPostedFileBase> fileDocumentFirmed, int companyId, int documentId)
        {
            if (fileDocumentFirmed == null || !fileDocumentFirmed.Any())
                return Json(new Result { Status = Status.Error }, JsonRequestBehavior.AllowGet);

            var result =
                Service.SaveContractualDocumentFirmed(fileDocumentFirmed.FirstOrDefault(), companyId, documentId, User.Id);

            if (result.Status == Status.Error)
            {
                return Json(new Result
                {
                    Status = Status.Error
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Result
            {
                Status = Status.Ok,
                Object = AutoMapper.Mapper.Map<DocumentViewModel>(result.Object)
            }, JsonRequestBehavior.AllowGet);    
        }

        [HttpPost]
        public ActionResult SaveOtherDocument(IEnumerable<HttpPostedFileBase> fileOtherDocument, int documentId)
        {
            if (fileOtherDocument == null || !fileOtherDocument.Any())
                return Json(new Result { Status = Status.Error }, JsonRequestBehavior.AllowGet);

            var result =
                Service.SaveOtherDocument(fileOtherDocument.FirstOrDefault(), documentId);

            if (result.Status == Status.Error)
            {
                return Json(new Result
                {
                    Status = Status.Error
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Result
            {
                Status = Status.Ok,
                Object = AutoMapper.Mapper.Map<DocumentViewModel>(result.Object)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OfferView(int contractualDocumentId, bool isPartialView)
        {
            var contractualDocument = Service.GetDocument(contractualDocumentId);
            var actionResult = GetActionResultForReport(contractualDocument.AreaId);

            return RedirectToAction(
                actionResult, 
                "Companies", 
                new { contractualDocumentId = contractualDocument.Id, isPartialView } );
        }

        [HttpGet]
        public ActionResult OfferSPAReport(int documentId, bool isPartialView = false)
        {
            ViewBag.IsPartialView = isPartialView;

            var contractualDocument = Service.GetDocument(documentId);

            ViewBag.ContractualDocumentId = documentId;
            ViewBag.ContractualDocumentEnrollment = contractualDocument.Name;
            ViewBag.IVA = Service.GetTagValue("IVA");

            var workCenters = Service.GetWorkCentersByCompany((int)contractualDocument.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
            ViewBag.NumberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }
            ViewBag.ProvincesWorkCenters = provincesWorkCenters;
            
            if (isPartialView)
                return PartialView("~/Views/CommercialTool/Companies/Reports/OfferSPAReport.cshtml", contractualDocument);

            return View("~/Views/CommercialTool/Companies/Reports/OfferSPAReport.cshtml", contractualDocument);
        }

        [HttpGet]
        public ActionResult ContractSPAReport(int contractualDocumentId, bool isPartialView = false)
        {
            ViewBag.IsPartialView = isPartialView;

            var contractualDocument = Service.GetDocument(contractualDocumentId);

            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.ContractualDocumentEnrollment = contractualDocument.Name;
            ViewBag.IVA = Service.GetTagValue("IVA");

            var workCenters = Service.GetWorkCentersByCompany((int)contractualDocument.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
            ViewBag.NumberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }
            ViewBag.ProvincesWorkCenters = provincesWorkCenters;

            if (isPartialView)
                return PartialView("~/Views/CommercialTool/Companies/Reports/ContractSPAReport.cshtml", contractualDocument);

            return View("~/Views/CommercialTool/Companies/Reports/ContractSPAReport.cshtml", contractualDocument);
        }

        [HttpGet]
        public ActionResult OfferAgencyReport(int contractualDocumentId, bool isPartialView = false)
        {
            ViewBag.IsPartialView = isPartialView;

            var contractualDocument = Service.GetDocument(contractualDocumentId);

            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.ContractualDocumentEnrollment = contractualDocument.Name;
            ViewBag.IVA = Service.GetTagValue("IVA");

            var workCenters = Service.GetWorkCentersByCompany((int)contractualDocument.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
            ViewBag.NumberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }
            ViewBag.ProvincesWorkCenters = provincesWorkCenters;

            if (isPartialView)
                return PartialView("~/Views/CommercialTool/Companies/Reports/OfferAgencyReport.cshtml", contractualDocument.Company);

            return View("~/Views/CommercialTool/Companies/Reports/OfferAgencyReport.cshtml", contractualDocument.Company);
        }

        [HttpGet]
        public ActionResult ContractAgencyReport(int contractualDocumentId, bool isPartialView = false)
        {
            ViewBag.IsPartialView = isPartialView;

            var contractualDocument = Service.GetDocument(contractualDocumentId);

            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.ContractualDocumentEnrollment = contractualDocument.Name;
            ViewBag.IVA = Service.GetTagValue("IVA");

            var workCenters = Service.GetWorkCentersByCompany((int)contractualDocument.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
            ViewBag.NumberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }
            ViewBag.ProvincesWorkCenters = provincesWorkCenters;

            if (isPartialView)
                return PartialView("~/Views/CommercialTool/Companies/Reports/ContractAgencyReport.cshtml", contractualDocument.Company);

            return View("~/Views/CommercialTool/Companies/Reports/ContractAgencyReport.cshtml", contractualDocument.Company);
        }

        [HttpGet]
        public ActionResult OfferTrainingReport(int contractualDocumentId, bool isPartialView = false)
        {
            ViewBag.IsPartialView = isPartialView;

            var contractualDocument = Service.GetDocument(contractualDocumentId);

            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.ContractualDocumentEnrollment = contractualDocument.Name;
            ViewBag.IVA = Service.GetTagValue("IVA");

            var workCenters = Service.GetWorkCentersByCompany((int)contractualDocument.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
            ViewBag.NumberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }
            ViewBag.ProvincesWorkCenters = provincesWorkCenters;

            if (isPartialView)
                return PartialView("~/Views/CommercialTool/Companies/Reports/OfferTrainingReport.cshtml", contractualDocument.Company);

            return View("~/Views/CommercialTool/Companies/Reports/OfferTrainingReport.cshtml", contractualDocument.Company);
        }

        [HttpGet]
        public ActionResult ContractTrainingReport(int contractualDocumentId, bool isPartialView = false)
        {
            ViewBag.IsPartialView = isPartialView;

            var contractualDocument = Service.GetDocument(contractualDocumentId);

            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.ContractualDocumentEnrollment = contractualDocument.Name;
            ViewBag.IVA = Service.GetTagValue("IVA");

            var workCenters = Service.GetWorkCentersByCompany((int)contractualDocument.CompanyId).Where(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta).ToList();
            ViewBag.NumberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }
            ViewBag.ProvincesWorkCenters = provincesWorkCenters;

            if (isPartialView)
                return PartialView("~/Views/CommercialTool/Companies/Reports/ContractTrainingReport.cshtml", contractualDocument.Company);

            return View("~/Views/CommercialTool/Companies/Reports/ContractTrainingReport.cshtml", contractualDocument.Company);
        }

        [HttpGet]
        public ActionResult DefaultReport(int contractualDocumentId)
        {
            var contractualDocument = Service.GetDocument(contractualDocumentId);

            return View("~/Views/CommercialTool/Companies/Reports/DefaultReport.cshtml", contractualDocument.Company);
        }

        [HttpPost]
        public JsonResult AddContractualDocumentFirmed(int contractualDocumentId)
        {
            return Json(new { result = Status.Ok }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteContractualDocumentCompanyFirmed(int documentFirmedId)
        {
            Document documentParent = null;
            var documentParentId = Service.GetDocument(documentFirmedId).DocumentParentId;
            if (documentParentId != null)
            {
                documentParent = Service.GetDocument((int) documentParentId);
            }

            var result =
                Service.DeleteDocument(documentFirmedId);

            if (result.Status == Status.Error)
            {
                return Json(new Result
                {
                    Status = Status.Error
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Result
            {
                Status = Status.Ok,
                Object = AutoMapper.Mapper.Map<DocumentViewModel>(documentParent)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllContractualDocumentTypes([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<AreaViewModel>>(Service.GetContractualDocumentTypes());

            return this.Jsonp(data);
        }

        public JsonResult GetContractualDocumentTypes([DataSourceRequest] DataSourceRequest request, int companyId, int simulationId)
        {
            var data = AutoMapper.Mapper.Map<List<AreaViewModel>>(Service.GetContractualDocumentTypesBySimulation(companyId, simulationId));

            return this.Jsonp(data);
        }

        #endregion

        #region WorksCenter

        [HttpGet]
        public ActionResult WorkCentersCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/WorkCentersCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial,ContactPerson")]
        public JsonResult WorkCentersCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<WorkCenterViewModel>>(Service.GetWorkCentersByCompany(companyId));

            return this.Jsonp(data);
        }

        public JsonResult WorkCentersCompany_Update()
        {
            try
            {
                var workCenter = this.DeserializeObject<WorkCenter>("workCenter");
                if (workCenter == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Centro de Trabajo" });
                }

                var result = Service.SaveWorkCenter(workCenter);

                return result.Status != Status.Error ? this.Jsonp(workCenter) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Centro de Trabajo" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Centro de Trabajo" });
            }
        }

        public ActionResult WorkCentersCompany_Destroy()
        {
            try
            {
                var workCenter = this.DeserializeObject<WorkCenter>("workCenter");
                if (workCenter == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Centro de Trabajo" });
                }

                var result = Service.DeleteWorkCenterCompany(workCenter.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Centro de Trabajo" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<WorkCenterViewModel>(workCenter));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Persona de Contacto" });
            }
        }

        public ActionResult WorkCentersCompany_Create()
        {
            try
            {
                var workCenter = this.DeserializeObject<WorkCenterViewModel>("workCenter");
                if (workCenter == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Centro de Trabajo" });
                }

                var result = Service.SaveWorkCenterCompany(workCenter.CompanyId, AutoMapper.Mapper.Map<WorkCenter>(workCenter));

                if (result.Status != Status.Error)
                {
                    var workCenterCompany = result.Object as WorkCenterCompany;
                    if (workCenterCompany != null)
                        workCenter.Id = workCenterCompany.WorkCenterId;

                    return this.Jsonp(workCenter);
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Centro de Trabajo" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Centro de Trabajo" });
            }
        }

        [HttpPost]
        public JsonResult WorkCentersCompany_Subscribe(int workCenterId, bool subscribe)
        {
            try
            {
                var result = Service.SubscribeWorkCenter(workCenterId, subscribe);

                return Json(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta al Centro de Trabajo" } : new { Errors = "Se ha producido un error al Dar de Baja al Centro de Trabajo" });
            }
        }

        public JsonResult GetEstablishmentTypes([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<EstablishmentTypeViewModel>>(Service.GetEstablishmentTypes());

            return this.Jsonp(data);
        }
        #endregion
    }
}