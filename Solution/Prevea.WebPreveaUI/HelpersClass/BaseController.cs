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
    using System;
    using System.Diagnostics;
    using Rotativa.MVC;

    #endregion

    public class BaseController : Controller
    {
        protected readonly IService Service;

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

        public bool CreatePdf(Document document, string footer)
        {
            try
            {
                var filePath = Server.MapPath(document.UrlRelative);
                var directoryPath = Path.GetDirectoryName(filePath);
                if (directoryPath != null)
                    Directory.CreateDirectory(directoryPath);

                var actionPdf = new ActionAsPdf(
                    GetActionResultForReport(document.AreaId),
                    new { documentId = document.Id });
                if (!string.IsNullOrEmpty(footer))
                    actionPdf.RotativaOptions.CustomSwitches = footer;

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


        public ActionResult DownloadMedicalExamination(int id)
        {
            var medicalExamination = Service.GetMedicalExaminationById(id);
            var url = Server.MapPath(medicalExamination.Url);
            if (!System.IO.File.Exists(url))
                return null;

            var filedata = System.IO.File.ReadAllBytes(url);
            var contentType = MimeMapping.GetMimeMapping(url);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(medicalExamination.Url),
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        public ActionResult DownloadPdfByUrl(string urlRelative)
        {
            var url = Server.MapPath(urlRelative);
            if (!System.IO.File.Exists(url))
                return null;

            var filedata = System.IO.File.ReadAllBytes(url);
            var contentType = MimeMapping.GetMimeMapping(url);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(urlRelative),
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        protected static string GetActionResultForReport(int areaId)
        {
            string actionResult;
            switch (areaId)
            {
                case 6:
                    actionResult = "OfferSPAReport";
                    break;
                case 7:
                    actionResult = "OfferTrainingReport";
                    break;
                case 8:
                    actionResult = "OfferAgencyReport";
                    break;
                case 9:
                    actionResult = "ContractSPAReport";
                    break;
                case 10:
                    actionResult = "ContractTrainingReport";
                    break;
                case 11:
                    actionResult = "ContractAgencyReport";
                    break;
                case 15:
                    actionResult = "TemplateEmployeeCitationReport";
                    break;

                default:
                    actionResult = "DefaultReport";
                    break;
            }

            return actionResult;
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