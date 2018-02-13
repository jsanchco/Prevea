namespace Prevea.WebPreveaUI.HelpersClass
{
    #region Using

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Security;
    using IService.IService;
    using Model.Model;

    #endregion

    public class BaseController : Controller
    {
        protected IService Service;

        #region Construtor

        public BaseController(IService service)
        {
            Service = service;
        }

        #endregion

        protected new virtual AppPrincipal User => HttpContext.User as AppPrincipal;

        protected List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                from error in state.Errors
                select error.ErrorMessage;

            return query.ToList();
        }

        public ActionResult DownloadFile(int id)
        {            
            var document = Service.GetDocument(id);
            var url = Server.MapPath(document.UrlRelative);
            if (!System.IO.File.Exists(url))
                return null;

            if (User != null)
            {
                var historicDownloadDocument = new HistoricDownloadDocument
                {
                    UserId = User.Id,
                    DocumentId = id
                };

                Service.SaveHistoricDownloadDocument(historicDownloadDocument);
            }

            var filedata = System.IO.File.ReadAllBytes(url);
            var contentType = MimeMapping.GetMimeMapping(url);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(document.UrlRelative),
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        public ActionResult DownloadContractualDocument(int id)
        {
            var document = Service.GetContractualDocument(id);
            var url = Server.MapPath(document.UrlRelative);
            if (!System.IO.File.Exists(url))
                return null;

            var filedata = System.IO.File.ReadAllBytes(url);
            var contentType = MimeMapping.GetMimeMapping(url);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(document.UrlRelative),
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}