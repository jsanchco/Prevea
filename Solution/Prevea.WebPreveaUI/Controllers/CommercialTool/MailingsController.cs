namespace Prevea.WebPreveaUI.Controllers.CommercialTool
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using System.Diagnostics;

    #endregion

    public class MailingsController : BaseController
    {
        #region Constructor
        public MailingsController(IService service) : base(service)
        {
        }
        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Super,PreveaPersonal")]
        public ActionResult Mailings()
        {
            return PartialView("~/Views/CommercialTool/Mailings/Mailings.cshtml");
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,PreveaPersonal")]
        public ActionResult DetailMailing(int id)
        {
            var mailing = Service.GetMailingById(id);

            return PartialView("~/Views/CommercialTool/Mailings/DetailMailing.cshtml", mailing);
        }

        [AppAuthorize(Roles = "Super,PreveaPersonal")]
        public ActionResult DataMails(int mailingId)
        {
            ViewBag.MailingId = mailingId;

            return PartialView("~/Views/CommercialTool/Mailings/DataMails.cshtml");
        }

        [AppAuthorize(Roles = "Super,PreveaPersonal")]
        public ActionResult Mail(int mailingId)
        {
            ViewBag.MailingId = mailingId;

            return PartialView("~/Views/CommercialTool/Mailings/Mail.cshtml");
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,PreveaPersonal")]
        public JsonResult Mailings_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<MailingViewModel>>(Service.GetMailings());

            return this.Jsonp(data);
        }

        public JsonResult Mailings_Update()
        {
            try
            {
                var mailing = this.DeserializeObject<Mailing>("mailing");
                if (mailing == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
                }

                var result = Service.SaveMailing(mailing);

                return result.Status != Status.Error ? this.Jsonp(mailing) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
            }
        }

        public ActionResult Mailings_Destroy()
        {
            try
            {
                var mailing = this.DeserializeObject<Mailing>("mailing");
                if (mailing == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Mailing" });
                }

                var result = Service.DeleteMailing(mailing.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Mailing" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<MailingViewModel>(mailing));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Mailing" });
            }
        }

        public ActionResult Mailings_Create()
        {
            try
            {
                var mailing = this.DeserializeObject<MailingViewModel>("mailing");
                if (mailing == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
                }

                var result = Service.SaveMailing(AutoMapper.Mapper.Map<Mailing>(mailing));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<MailingViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
            }
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,PreveaPersonal")]
        public JsonResult DataMails_Read([DataSourceRequest] DataSourceRequest request, int mailingId)
        {
            var data = AutoMapper.Mapper.Map<List<DataMailViewModel>>(Service.GetDataMailsByMailing(mailingId));

            return this.Jsonp(data);
        }

        public JsonResult DataMails_Update()
        {
            try
            {
                var dataMail = this.DeserializeObject<DataMail>("dataMail");
                if (dataMail == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DataMail" });
                }

                var result = Service.SaveDataMail(dataMail);

                return result.Status != Status.Error ? this.Jsonp(AutoMapper.Mapper.Map<DataMailViewModel>(dataMail)) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Mailing" });
            }
        }

        public ActionResult DataMails_Destroy()
        {
            try
            {
                var dataMail = this.DeserializeObject<DataMail>("dataMail");
                if (dataMail == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del DataMail" });
                }

                var result = Service.DeleteDataMail(dataMail.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del DataMail" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<DataMailViewModel>(dataMail));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del DataMail" });
            }
        }

        public ActionResult DataMails_Create()
        {
            try
            {
                var dataMail = this.DeserializeObject<DataMail>("dataMail");
                if (dataMail == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DataMail" });
                }

                var result = Service.SaveDataMail(dataMail);                

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<DataMailViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DataMail" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DataMail" });
            }
        }

        public JsonResult SendMailing(int mailingId)
        {
            var mailing = Service.GetMailingById(mailingId);
            if (mailing == null)
            {
                return Json(new { resultStatus = Status.Error });
            }

            foreach (var dataMail in mailing.DataMails)
                Service.SendEmail(dataMail.Id, dataMail.EMail, mailing.Subject, mailing.Mail, dataMail.Data);

            var hasError = false;
            mailing.Sent = true;
            mailing.SendDate = DateTime.Now;
            Service.SaveMailing(mailing);

            mailing = Service.GetMailingById(mailingId);
            foreach (var dataMail in mailing.DataMails)
            {
                if (dataMail.DataMailStateId == (int) EnDataMailState.Error)
                {
                    hasError = true;
                    break;
                }
            }


            return hasError ? 
                Json(new {resultStatus = Status.Error, dataMails = AutoMapper.Mapper.Map<List<DataMailViewModel>>(mailing.DataMails), mailingState = mailing.Sent }, JsonRequestBehavior.AllowGet) : 
                Json(new {resultStatus = Status.Ok, dataMails = AutoMapper.Mapper.Map<List<DataMailViewModel>>(mailing.DataMails), mailingState = mailing.Sent }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataMails(int mailingId, List<DataMail> dataMails)
        {
            var result = Service.UpdateDataMails(mailingId, dataMails);

            return Json(new { resultStatus = result.Status });
        }

        public JsonResult HasDataMailsInDataBase(int mailingId)
        {
            var mailing = Service.GetMailingById(mailingId);

            return Json(mailing.DataMails?.Count > 0 ? new { resultStatus = true } : new { resultStatus = false });
        }

        public JsonResult GetBodyMail(int mailingId)
        {
            var mailing = Service.GetMailingById(mailingId);
            if (mailing == null)
                return Json(new { resultStatus = Status.Error });

            return Json(new { resultStatus = Status.Ok, bodyMail = mailing.Mail });
        }

        public JsonResult SaveBodyMail(int mailingId, string text)
        {
            var mailing = Service.GetMailingById(mailingId);
            mailing.Mail = text;

            var result = Service.SaveMailing(mailing);

            return Json(new { resultStatus = result.Status });
        }
    }
}