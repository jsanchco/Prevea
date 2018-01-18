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
    using System.IO;
    using Rotativa.MVC;

    #endregion

    public class CompaniesController : BaseController
    {
        #region Constructor
        public CompaniesController(IService service) : base(service)
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

            return PartialView("~/Views/CommercialTool/Companies/DetailCompany.cshtml", company);
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

                ViewBag.SelectTabId = 0;

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

                ViewBag.SelectTabId = 2;

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

                ViewBag.SelectTabId = 2;

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
            const decimal iva = 1.21m;

            ViewBag.Total = Math.Round(Service.GetTotalSimulation(company.SimulationCompany.SimulationId) * iva, 2);

            return PartialView("~/Views/CommercialTool/Companies/EconomicDataCompany.cshtml", company);
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

                ViewBag.SelectTabId = 5;

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

                ViewBag.SelectTabId = 5;

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
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public JsonResult EmployeesCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
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

        public JsonResult EmployeesCompany_Update()
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

                return Json(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta del Trabajador" } : new { Errors = "Se ha producido un error al Dar de Baja del Trabajador" });
            }
        }

        #endregion

        #region ContactPersons

        [HttpGet]
        public ActionResult ContactPersonsCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/ContactPersonsCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public JsonResult ContactPersonsCompany_Read([DataSourceRequest] DataSourceRequest request, int companyId)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetContactPersonsByCompany(companyId));
            foreach (var contactPerson in data)
            {
                contactPerson.CompanyId = companyId;
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
                var result = Service.SaveContactPersonCompany((int)EnRole.ContactPerson, (int)contactPerson.CompanyId, data);

                return result.Status != Status.Error ? this.Jsonp(contactPerson) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Persona de Contacto" });
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

        #endregion

        #region Documents

        [HttpGet]
        public ActionResult ContractualsDocumentsCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/ContractualsDocumentsCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
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
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });

                var result = Service.SaveContractualDocument(AutoMapper.Mapper.Map<ContractualDocumentCompany>(contractualDocument));
                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });

                if (!CreatePdf(result.Object as ContractualDocumentCompany))  
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });

                return this.Jsonp(AutoMapper.Mapper.Map<ContractualDocumentCompanyViewModel>(result.Object));
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
                var contractualDocument = this.DeserializeObject<ContractualDocumentCompanyViewModel>("contractualDocument");
                if (contractualDocument == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
                }

                if (Service.DeleteContractualDocument(contractualDocument.Id))
                    return this.Jsonp(contractualDocument);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
            }
        }

        [HttpGet]
        public ActionResult OfferView(int contractualDocumentId)
        {
            var contractualDocument = Service.GetContractualDocument(contractualDocumentId);
            ViewBag.ContractualDocumentId = contractualDocumentId;

            return PartialView("~/Views/CommercialTool/Companies/Reports/OfferReport.cshtml", contractualDocument.Company);
        }

        [HttpGet]
        public ActionResult OfferReport(int contractualDocumentId)
        {
            var contractualDocument = Service.GetContractualDocument(contractualDocumentId);

            ViewBag.ContractualDocumentId = contractualDocumentId;
            ViewBag.NumberWorkCenters = Service.GetWorkCentersByCompany(contractualDocument.CompanyId).Count(x => x.WorkCenterStateId == (int)EnWorkCenterState.Alta);

            return View("~/Views/CommercialTool/Companies/Reports/OfferReport.cshtml", contractualDocument.Company);
        }

        [HttpPost]
        public JsonResult CanAddContractualDocument(int companyId)
        {
            var message = Service.VerifyNewContractualDocument(companyId);
            if (string.IsNullOrEmpty(message))
                return Json(new { result = Status.Ok }, JsonRequestBehavior.AllowGet);

            return Json(new { result = Status.Error, message }, JsonRequestBehavior.AllowGet);
        }

        private bool CreatePdf(ContractualDocumentCompany contractualDocument)
        {
            try
            {
                var filePath = Server.MapPath(contractualDocument.UrlRelative);
                var actionPdf = new ActionAsPdf("OfferReport", new { contractualDocumentId = contractualDocument.Id });
                var applicationPdfData = actionPdf.BuildPdf(ControllerContext);
                var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fileStream.Write(applicationPdfData, 0, applicationPdfData.Length);
                fileStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }      
        }

        #endregion

        #region WorksCenter

        [HttpGet]
        public ActionResult WorkCentersCompany(int companyId)
        {
            return PartialView("~/Views/CommercialTool/Companies/WorkCentersCompany.cshtml", Service.GetCompany(companyId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
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