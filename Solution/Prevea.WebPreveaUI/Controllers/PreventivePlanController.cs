using Prevea.Model.CustomModel;

namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using System.Linq;

    #endregion

    public class PreventivePlanController : BaseController
    {
        #region Constructor

        public PreventivePlanController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult PreventivePlans()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult DetailPreventivePlan(int id, int selectTabId)
        {
            var preventivePlan = Service.GetPreventivePlanById(id);           

            ViewBag.ListTemplates = preventivePlan.PreventivePlanTemplatePreventivePlans.Select(preventivePlanPreventivePlan => preventivePlanPreventivePlan.TemplatePreventivePlanId).ToList();
            ViewBag.SelectTabId = selectTabId;

            var data = AutoMapper.Mapper.Map<PreventivePlanViewModel>(Service.GetPreventivePlanById(id));

            return PartialView(data);
        }

        [HttpGet]
        public ActionResult EditorTemplatePreventivePlan(int preventivePlan, int templateId)
        {
            ViewBag.TemplateId = templateId;
            ViewBag.Snippets = Service.GetEditorSnippets(preventivePlan, templateId);


            return PartialView(AutoMapper.Mapper.Map<PreventivePlanViewModel>(Service.GetPreventivePlanById(preventivePlan)));
        }

        [HttpGet]
        public JsonResult PreventivePlans_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<PreventivePlanViewModel>>(Service.GetPreventivePlans(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult PreventivePlans_Update()
        {
            try
            {
                var preventivePlan = this.DeserializeObject<PreventivePlan>("preventivePlan");
                if (preventivePlan == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del PreventivePlan" });
                }

                var result = Service.SavePreventivePlan(preventivePlan);

                return result.Status != Status.Error ? this.Jsonp(preventivePlan) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del PreventivePlan" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del PreventivePlan" });
            }
        }

        public ActionResult PreventivePlans_Destroy()
        {
            try
            {
                var preventivePlan = this.DeserializeObject<PreventivePlan>("preventivePlan");
                if (preventivePlan == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del PreventivePlan" });
                }

                var result = Service.DeletePreventivePlan(preventivePlan.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del PreventivePlan" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<PreventivePlanViewModel>(preventivePlan));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del PreventivePlan" });
            }
        }

        public ActionResult PreventivePlans_Create()
        {
            try
            {
                var preventivePlan = this.DeserializeObject<PreventivePlanViewModel>("preventivePlan");
                if (preventivePlan == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del PreventivePlan" });
                }

                var result = Service.SavePreventivePlan(AutoMapper.Mapper.Map<PreventivePlan>(preventivePlan));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<PreventivePlanViewModel>(Service.GetPreventivePlanById(((PreventivePlan)result.Object).Id)));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del PreventivePlan" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del PreventivePlan" });
            }
        }

        [HttpGet]
        public JsonResult Companies_Read([DataSourceRequest] DataSourceRequest request)
        {
            var companies = Service.GetCompaniesByUser(User.Id).Where(x =>
                x.SimulationCompanyActive.Simulation.StateForeignPreventionService);

            var data = AutoMapper.Mapper.Map<List<CompanyViewModel>>(companies);

            return this.Jsonp(data);
        }

        public JsonResult GetContract(int companyId)
        {
            var contract = Service.GetDocumentsContractualsByCompany(companyId).FirstOrDefault(x => x.AreaId == 9 && x.DocumentStateId == (int)EnDocumentState.Activo);
            if (contract == null)
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

            return Json(new { resultStatus = Status.Ok, contract = AutoMapper.Mapper.Map<DocumentViewModel>(contract) }, 
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTemplatesPreventivePlans()
        {
            var templatePreventivePlans = Service.GetTemplatePreventivePlans();

            var templates = new List<GenericModelDropDown>();
            foreach (var templatePreventivePlan in templatePreventivePlans)
            {
                templates.Add(new GenericModelDropDown { Id = templatePreventivePlan.Id, Name = templatePreventivePlan.Name });
            }

            return Json(templates, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CanSaveTemplatePreventivePlan(int preventivePlanId, int templateId)
        {
            if (Service.ExistPreventivePlanTemplatePreventivePlan(preventivePlanId, templateId) != null)
                return Json(new { exist = true });

            var template = Service.GetTemplatePreventivePlanById(templateId);

            return Json(new { exist = false, text = template.Template });
        }

        public JsonResult GetEditorSnippets(int preventivePlanId, int templateId)
        {
            return Json(new { snippets = Service.GetEditorSnippets(preventivePlanId, templateId) });
        }

        public JsonResult SaveTemplatePreventivePlan(int preventivePlanId, int templateId, string text)
        {
            var preventivePlanTemplatePreventivePlan =
                Service.ExistPreventivePlanTemplatePreventivePlan(preventivePlanId, templateId);

            if (preventivePlanTemplatePreventivePlan == null)
            {
                preventivePlanTemplatePreventivePlan = new PreventivePlanTemplatePreventivePlan
                {
                    PreventivePlanId = preventivePlanId,
                    TemplatePreventivePlanId = templateId,
                    Text = text
                };
            }
            else
            {
                preventivePlanTemplatePreventivePlan.Text = text;
            }

            var result = Service.SavePreventivePlanTemplatePreventivePlan(preventivePlanTemplatePreventivePlan);

            return result.Status != Status.Error ? Json(new { resultStatus = Status.Ok }) : Json(new { Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan" });
        }

        public JsonResult GetDataFromPreventivePlanTemplatePreventivePlan(int preventivePlanId, int templateId)
        {
            var preventivePlanTemplatePreventivePlan =
                Service.ExistPreventivePlanTemplatePreventivePlan(preventivePlanId, templateId);

            return Json(new
            {
                title = preventivePlanTemplatePreventivePlan.TemplatePreventivePlan.Name,
                text = preventivePlanTemplatePreventivePlan.Text
            });
        }

        public JsonResult DeleteTemplatePreventivePlan(int preventivePlanId, int templateId)
        {
            var preventivePlanTemplatePreventivePlan =
                Service.ExistPreventivePlanTemplatePreventivePlan(preventivePlanId, templateId);
            if (preventivePlanTemplatePreventivePlan != null)
            {
                var result =
                    Service.DeletePreventivePlanTemplatePreventivePlan(preventivePlanTemplatePreventivePlan.Id);

                return Json(new { resultStatus = result.Status });
            }

            return Json(new { resultStatus = Status.Ok });
        }
    }
}