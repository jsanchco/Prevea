namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using IService.IService;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Kendo.Mvc.UI;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using HelpersClass;

    #endregion

    public partial class CompanyController
    {
        #region Companies

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult Companies()
        {
            return PartialView();
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult Companies_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<CompanyViewModel>>(Service.GetCompanies());

            return this.Jsonp(data);
        }

        [HttpGet]
        public ActionResult DetailCompany(int id, int selectTabId)
        {
            var company = Service.GetCompany(id);

            ViewBag.SelectTabId = selectTabId;

            return PartialView(company);
        }

        [HttpGet]
        public ActionResult AddCompany()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult SaveCompany(Company company)
        {
            try
            {
                company.GestorId = User.Id;

                var result = Service.SaveCompany(company);

                if (result.Status != Status.Error)
                    return PartialView("Companies");

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("Companies");
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("Companies");
            }
        }

        #endregion

        #region GeneralData

        [HttpGet]
        public ActionResult GeneralDataCompany(int companyId)
        {
            return PartialView(Service.GetCompany(companyId));
        }

        [HttpPost]
        public ActionResult UpdateGeneralDataCompany(Company company)
        {
            try
            {
                var result = Service.UpdateCompany(company.Id, company);

                ViewBag.SelectTabId = 0;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = "La Empresa se ha actualizado correctamente";

                    return PartialView("DetailCompany", result.Object as Company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("DetailCompany", company);
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("DetailCompany", company);
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

        #region ContactPersons

        [HttpGet]
        public ActionResult ContactPersonsCompany(int companyId)
        {
            return PartialView(Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult ContactPersonsCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetContactPersonsByCompany(companyId));

            return this.Jsonp(data);
        }

        [HttpGet]
        public ActionResult AddContactPersonCompany(int companyId, int userId)
        {
            var company = Service.GetCompany(companyId);

            if (userId != 0)
            {
                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView(data);
            }

            return PartialView(new UserViewModel
            {
                CompanyId = companyId,
                CompanyName = company.Name,
                CompanyEnrollment = company.Enrollment,
                UserStateId = (int)EnUserState.Alta
            });
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult DeleteContactPersonCompany(int companyId, int userId)
        {
            var company = Service.GetCompany(companyId);

            if (userId != 0)
            {
                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView(data);
            }

            return PartialView(new UserViewModel
            {
                CompanyId = companyId,
                CompanyName = company.Name,
                CompanyEnrollment = company.Enrollment,
                UserStateId = (int)EnUserState.Alta
            });
        }

        public ActionResult DeleteContactPerson(int companyId, int userId)
        {
            try
            {
                var company = Service.GetCompany(companyId);

                var result = Service.DeleteContactPersonCompany(companyId, userId);

                ViewBag.SelectTabId = 1;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteContactPersonCompany", data);

            }
            catch (Exception e)
            {
                var company = Service.GetCompany(companyId);

                ViewBag.Error = new List<string> { e.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteContactPersonCompany", data);
            }
        }

        public ActionResult SubscribeContactPersonCompany(int companyId, int userId, bool subscribe)
        {
            try
            {
                var company = Service.GetCompany(companyId);

                var result = Service.SubscribeContactPersonCompany(companyId, userId, subscribe);

                ViewBag.SelectTabId = 1;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteContactPersonCompany", data);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(companyId);

                ViewBag.Error = new List<string> { e.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteContactPersonCompany", data);
            }
        }

        [HttpPost]
        public ActionResult SaveContactPersonCompany(UserViewModel userViewModel)
        {
            try
            {
                var data = AutoMapper.Mapper.Map<User>(userViewModel);

                if (userViewModel.CompanyId == null)
                {
                    ViewBag.Notification = "Se ha producido un error en la Grabación de la Persona de Contacto";

                    return PartialView("Companies");
                }

                var result = Service.SaveContactPersonCompany((int)EnRole.ContactPerson, (int)userViewModel.CompanyId, data);
                if (result.Status != Status.Error)
                {
                    var company = Service.GetCompany((int)userViewModel.CompanyId);

                    ViewBag.SelectTabId = 1;
                    ViewBag.Notification = "La Persona de Contacto se ha guardado correctamente";

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("AddContactPersonCompany", new UserViewModel { CompanyId = userViewModel.CompanyId });
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("Companies");
            }
        }

        #endregion

        #region Agency

        [HttpGet]
        public ActionResult AgencyCompany(int companyId)
        {
            var company = Service.GetCompany(companyId);

            var agency = company.Agency == null ? new AgencyViewModel { CompanyId = companyId } : AutoMapper.Mapper.Map<AgencyViewModel>(company.Agency);

            return PartialView(agency);
        }

        [HttpPost]
        public ActionResult SaveAgencyCompany(AgencyViewModel agency)
        {
            try
            {
                var agencySave = AutoMapper.Mapper.Map<Agency>(agency);

                var result = agency.Id != 0 ? Service.UpdateAgency(agencySave, agency.Id) : Service.SaveAgency(agencySave, agency.CompanyId);

                var company = Service.GetCompany(agency.CompanyId);

                ViewBag.SelectTabId = 2;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("DetailCompany", company);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(agency.CompanyId);

                ViewBag.SelectTabId = 2;

                ViewBag.Error = new List<string> { e.Message };

                return PartialView("DetailCompany", company);
            }
        }

        #endregion

        #region Employees

        [HttpGet]
        public ActionResult EmployeesCompany(int companyId)
        {
            return PartialView(Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult EmployeesCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetEmployeesByCompany(companyId));

            return this.Jsonp(data);
        }

        [HttpGet]
        public ActionResult AddEmployeeCompany(int companyId, int userId)
        {
            var company = Service.GetCompany(companyId);

            if (userId != 0)
            {
                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView(data);
            }

            return PartialView(new UserViewModel
            {
                CompanyId = companyId,
                CompanyName = company.Name,
                CompanyEnrollment = company.Enrollment,
                UserStateId = (int)EnUserState.Alta
            });
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult DeleteEmployeeCompany(int companyId, int userId)
        {
            var company = Service.GetCompany(companyId);

            if (userId != 0)
            {
                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView(data);
            }

            return PartialView(new UserViewModel
            {
                CompanyId = companyId,
                CompanyName = company.Name,
                CompanyEnrollment = company.Enrollment,
                UserStateId = (int)EnUserState.Alta
            });
        }

        public ActionResult DeleteEmployee(int companyId, int userId)
        {
            try
            {
                var company = Service.GetCompany(companyId);

                var result = Service.DeleteEmployeeCompany(companyId, userId);

                ViewBag.SelectTabId = 3;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteEmployeeCompany", data);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(companyId);

                ViewBag.Error = new List<string> { e.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteEmployeeCompany", data);
            }
        }

        public ActionResult SubscribeEmployeeCompany(int companyId, int userId, bool subscribe)
        {
            try
            {
                var company = Service.GetCompany(companyId);

                var result = Service.SubscribeEmployeeCompany(companyId, userId, subscribe);

                ViewBag.SelectTabId = 3;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteEmployeeCompany", data);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(companyId);

                ViewBag.Error = new List<string> { e.Message };

                var data = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(userId));

                data.CompanyId = companyId;
                data.CompanyName = company.Name;
                data.CompanyEnrollment = company.Enrollment;

                return PartialView("DeleteEmployeeCompany", data);
            }
        }

        [HttpPost]
        public ActionResult SaveEmployeeCompany(UserViewModel userViewModel)
        {
            try
            {
                var data = AutoMapper.Mapper.Map<User>(userViewModel);

                if (userViewModel.CompanyId == null)
                {
                    ViewBag.Notification = "Se ha producido un error en la Grabación del Trabajador";

                    return PartialView("Companies");
                }

                var result = Service.SaveEmployeeCompany((int)EnRole.Employee, (int)userViewModel.CompanyId, data);
                if (result.Status != Status.Error)
                {
                    var company = Service.GetCompany((int)userViewModel.CompanyId);

                    ViewBag.SelectTabId = 3;
                    ViewBag.Notification = "El Trabajador se ha guardado correctamente";

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("AddEmployeeCompany", new UserViewModel { CompanyId = userViewModel.CompanyId });
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("AddEmployeeCompany", new UserViewModel { CompanyId = userViewModel.CompanyId });
            }
        }

        #endregion

        #region Economic Data

        [HttpGet]
        public ActionResult EconomicDataCompany(int companyId)
        {
            var company = Service.GetCompany(companyId);

            var subscribeNumberEmployees = company.Employees.Count(e => e.User.UserStateId == (int)EnUserState.Alta);

            if (company.EconomicData == null)
            {
                return PartialView(new EconomicDataViewModel
                {
                    Id = companyId,
                    SubscribeNumberEmployees = subscribeNumberEmployees,
                    StretchCalculate = Service.GetStretchCalculateByNumberEmployees(subscribeNumberEmployees)
                });
            }

            var data = AutoMapper.Mapper.Map<EconomicDataViewModel>(company.EconomicData);
            data.SubscribeNumberEmployees = subscribeNumberEmployees;
            data.StretchCalculate = Service.GetStretchCalculateByNumberEmployees(subscribeNumberEmployees);

            return PartialView(data);
        }

        [HttpPost]
        public ActionResult UpdateEconomicDataCompany(EconomicDataViewModel economicData)
        {
            try
            {
                var economicDataSave = AutoMapper.Mapper.Map<EconomicData>(economicData);
                if (economicDataSave.AmountTecniques == decimal.Zero)
                    economicDataSave.AmountTecniques = null;
                if (economicDataSave.AmountHealthVigilance == decimal.Zero)
                    economicDataSave.AmountHealthVigilance = null;
                if (economicDataSave.AmountMedicalExamination == decimal.Zero)
                    economicDataSave.AmountMedicalExamination = null;

                var result = Service.SaveEconomicData(economicDataSave);

                var company = Service.GetCompany(economicData.Id);

                ViewBag.SelectTabId = 4;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("DetailCompany", company);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(economicData.Id);

                ViewBag.SelectTabId = 4;

                ViewBag.Error = new List<string> { e.Message };

                return PartialView("DetailCompany", company);
            }
        }

        [HttpPost]
        public JsonResult GetStretchCalculate(int companyId)
        {
            var company = Service.GetCompany(companyId);

            var subscribeNumberEmployees = company.Employees.Count(e => e.User.UserStateId == (int)EnUserState.Alta);
            var stretchCalculate = Service.GetStretchCalculateByNumberEmployees(subscribeNumberEmployees);

            return Json(new { stretchCalculate, subscribeNumberEmployees }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Payment Method

        [HttpGet]
        public ActionResult PaymentMethodCompany(int companyId)
        {
            var company = Service.GetCompany(companyId);

            if (company.PaymentMethod == null)
            {
                return PartialView(new PaymentMethodViewModel
                {
                    Id = companyId
                });
            }

            var data = AutoMapper.Mapper.Map<PaymentMethodViewModel>(company.PaymentMethod);
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult UpdatePaymentMethodCompany(PaymentMethodViewModel paymentMethod)
        {
            try
            {
                var paymentMethodSave = AutoMapper.Mapper.Map<PaymentMethod>(paymentMethod);
                var result = Service.SavePaymentMethod(paymentMethodSave);

                var company = Service.GetCompany(paymentMethod.Id);

                ViewBag.SelectTabId = 5;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = result.Message;

                    return PartialView("DetailCompany", company);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("DetailCompany", company);
            }
            catch (Exception e)
            {
                var company = Service.GetCompany(paymentMethod.Id);

                ViewBag.SelectTabId = 5;

                ViewBag.Error = new List<string> { e.Message };

                return PartialView("DetailCompany", company);
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
                Text = modePayment.Name,
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
                Text = modePaymentMedicalExamination.Name,
                Value = modePaymentMedicalExamination.Id.ToString(CultureInfo.InvariantCulture)
            }));

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region _Employees

        [HttpGet]
        public ActionResult _EmployeesCompany(int companyId)
        {
            return PartialView(Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult _EmployeesCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetEmployeesByCompany(companyId));
            foreach (var employee in data)
            {
                employee.CompanyId = companyId;
            }
            //var employeesByCompany = Service.GetEmployeesByCompany(companyId);
            //var data = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(employeesByCompany, opt => opt.AfterMap((src, dest) => 
            //{
            //    foreach (var item in dest)
            //    {
            //        item.CompanyId = companyId;
            //    }
            //} ));

            return this.Jsonp(data);
        }

        public JsonResult _EmployeesCompany_Update()
        {
            try
            {
                var employee = this.DeserializeObject<UserViewModel>("employee");
                if (employee == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
                }

                var data = AutoMapper.Mapper.Map<User>(employee);
                var result = Service.SaveEmployeeCompany((int)EnRole.Employee, (int)employee.CompanyId, data);

                return result.Status != Status.Error ? this.Jsonp(employee) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
        }

        public ActionResult _EmployeesCompany_Destroy()
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
                    return this.Jsonp(employee);
                }

                return result.Status != Status.Error ? this.Jsonp(employee) : this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Trabajador" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Trabajador" });
            }
        }

        public ActionResult _EmployeesCompany_Create()
        {
            try
            {
                var employee = this.DeserializeObject<UserViewModel>("employee");
                if (employee == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
                }

                var result = Service.SaveEmployeeCompany((int)EnRole.Employee, (int)employee.CompanyId, AutoMapper.Mapper.Map<User>(employee));

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                        employee.Id = user.Id;

                    return this.Jsonp(employee);
                }

                return result.Status != Status.Error ? this.Jsonp(employee) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Trabajador" });
            }
        }

        [HttpPost]
        public JsonResult _EmployeesCompany_Subscribe(int companyId, int userId, bool subscribe)
        {
            try
            {
                var result = Service.SubscribeEmployeeCompany(companyId, userId, subscribe);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta del Trabajador" } : new { Errors = "Se ha producido un error al Dar de Baja del Trabajador" });
            }
        }

        #endregion

        #region _ContactPersons

        [HttpGet]
        public ActionResult _ContactPersonsCompany(int companyId)
        {
            return PartialView(Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult _ContactPersonsCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetContactPersonsByCompany(companyId));
            foreach (var contactPerson in data)
            {
                contactPerson.CompanyId = companyId;
            }

            return this.Jsonp(data);
        }

        public JsonResult _ContactPersonsCompany_Update()
        {
            try
            {
                var contactPerson = this.DeserializeObject<UserViewModel>("contactPerson");
                if (contactPerson == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
                }

                var data = AutoMapper.Mapper.Map<User>(contactPerson);
                var result = Service.SaveContactPersonCompany((int)EnRole.ContactPerson, (int)contactPerson.CompanyId, data);

                return result.Status != Status.Error ? this.Jsonp(contactPerson) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
        }

        public ActionResult _ContactPersonsCompany_Destroy()
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
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Persona de Contacto" });
            }
        }

        public ActionResult _ContactPersonsCompany_Create()
        {
            try
            {
                var contactPerson = this.DeserializeObject<UserViewModel>("contactPerson");
                if (contactPerson == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
                }

                var result = Service.SaveContactPersonCompany((int)EnRole.ContactPerson, (int)contactPerson.CompanyId, AutoMapper.Mapper.Map<User>(contactPerson));

                if (result.Status != Status.Error)
                {
                    var user = result.Object as User;
                    if (user != null)
                        contactPerson.Id = user.Id;

                    return this.Jsonp(contactPerson);
                }

                return result.Status != Status.Error ? this.Jsonp(contactPerson) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
            }
        }

        [HttpPost]
        public JsonResult _ContactPersonsCompany_Subscribe(int companyId, int userId, bool subscribe)
        {
            try
            {
                var result = Service.SubscribeContactPersonCompany(companyId, userId, subscribe);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta a la Persona de Contacto" } : new { Errors = "Se ha producido un error al Dar de Baja a la Persona de Contacto" });
            }
        }

        #endregion

        #region _Companies

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public ActionResult _Companies()
        {
            return PartialView();
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult _Companies_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<CompanyViewModel>>(Service.GetCompanies());

            return this.Jsonp(data);
        }

        public JsonResult _Companies_Create()
        {
            try
            {
                var company = this.DeserializeObject<Company>("company");
                if (company == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Empresa" });
                }

                company.FromSimulator = false;
                company.GestorId = User.Id;
                var result = Service.SaveCompany(company);

                return result.Status != Status.Error ? this.Jsonp(company) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Empresa" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Empresa" });
            }
        }

        [HttpPost]
        public JsonResult _Company_Subscribe(int companyId, bool subscribe)
        {
            var result = Service.SubscribeCompany(companyId, subscribe);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Documentos

        [HttpGet]
        public ActionResult ContractualsDocumentsCompany(int companyId)
        {
            return PartialView(Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin")]
        public JsonResult ContractualsDocumentsCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<ContractualDocumentCompanyViewModel>>(Service.GetContractualsDocuments(companyId));

            return this.Jsonp(data);
        }

        public ActionResult ContractualsDocumentsCompany_Create()
        {
            try
            {
                var contractualDocument = this.DeserializeObject<ContractualDocumentCompanyViewModel>("contractualDocument");
                if (contractualDocument == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
                }

                var result = Service.SaveContractualDocument(AutoMapper.Mapper.Map<ContractualDocumentCompany>(contractualDocument));
    
                return result.Status != Status.Error ? this.Jsonp(AutoMapper.Mapper.Map<ContractualDocumentCompanyViewModel>(result.Object)) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
            }
        }

        public ActionResult ContractualsDocumentsCompany_Destroy()
        {
            try
            {
                var contractualDocument = this.DeserializeObject<ContractualDocumentCompanyViewModel>("contractualDocument");
                if (contractualDocument == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
                }

                if (Service.DeleteContractualDocument(contractualDocument.Id))
                    return this.Jsonp(contractualDocument);

                return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del Documento"});
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
            }
        }

        #endregion
    }
}