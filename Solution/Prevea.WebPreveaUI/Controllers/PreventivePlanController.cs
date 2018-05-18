using System.Linq;

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
            var preventivePlan = AutoMapper.Mapper.Map<PreventivePlanViewModel>(Service.GetPreventivePlanById(id));

            ViewBag.SelectTabId = selectTabId;

            return PartialView(preventivePlan);
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
            var data = AutoMapper.Mapper.Map<List<CompanyViewModel>>(Service.GetCompaniesByUser(User.Id));

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
    }
}