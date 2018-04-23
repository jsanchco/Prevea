namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using Kendo.Mvc.UI;
    using System.Collections.Generic;
    using System.Globalization;
    using Model.Model;
    using System.Linq;
    using System.Web;
    using System;
    using Common;
    using IService.IService;
    using Model.ViewModel;

    #endregion

    public class DocumentController : BaseController
    {
        #region Constructor

        public DocumentController(IService service) : base(service)
        {
        }

        #endregion

        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public ActionResult Documents()
        {
            ViewBag.IsRoleLibrary = User.IsInRole("Super") || User.IsInRole("Library");

            return PartialView();
        }

        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial")]
        public ActionResult HistoricDownloadDocuments()
        {
            return PartialView();
        }

        [AppAuthorize(Roles = "Super,Admin,Library,Basic")]
        public JsonResult Documents_Read([DataSourceRequest] DataSourceRequest request, int documentStateId)
        {
            var documents = Service.GetDocuments(1);
            if (documentStateId == 3)
                documents.AddRange(Service.GetDocuments(3));

            var data = AutoMapper.Mapper.Map<List<DocumentViewModel>>(documents);

            return this.Jsonp(data);
        }

        [AppAuthorize(Roles = "Super,Library")]
        public JsonResult HistoricDownloadDocument_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<HistoricDownloadDocumentViewModel>>(Service.GetHistoricDownloadDocuments());

            return this.Jsonp(data);
        }        

        public ActionResult DocumentsByParent_Read([DataSourceRequest] DataSourceRequest request, int id, int? documentParentId)
        {
            var data = AutoMapper.Mapper.Map<List<DocumentViewModel>>(Service.GetDocumentsByParent(id, documentParentId));

            return this.Jsonp(data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult AddDocument(Document document)
        {
            return PartialView(document);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult SubscribeDocument(int id)
        {
            try
            {
                var result = Service.SubscribeDocument(id);

                if (result.Status != Status.Error)
                {
                    return PartialView("Documents");
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("Documents");
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("Documents");
            }
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult DetailDocument(int id)
        {
            var document = Service.GetDocument(id);

            return PartialView(document);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult EditDocument(int id)
        {
            var data = AutoMapper.Mapper.Map<DocumentViewModel>(Service.GetDocument(id));

            return PartialView(data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult DeleteDocument(int id)
        {
            var document = Service.GetDocument(id);

            return PartialView(document);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult AddDocumentWithParent(int id)
        {
            var document = Service.GetDocument(id);
            document.Observations = string.Empty;
            if (document.DocumentParentId == null)
            {
                document.DocumentParentId = id;
            }

            return PartialView(document);
        }

        [HttpPost]
        public ActionResult SaveDocument(Document document)
        {
            try
            {
                var result = Service.SaveDocument(
                    document, 
                    true,
                    new List<DocumentUserCreator> { new DocumentUserCreator { UserId = User.Id } },
                    null);

                if (result.Status != Status.Error)
                    return PartialView("Documents");

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("Documents");
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("Documents");
            }
        }

        [HttpPost]
        public ActionResult SaveDocumentWithParent(Document document)
        {
            try
            {
                document.DocumentUserCreators = new List<DocumentUserCreator> { new DocumentUserCreator { UserId = User.Id } };
                var result = Service.SaveDocumentWithParent(document);

                if (result.Status != Status.Error)
                    return PartialView("Documents");

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("Documents");
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("Documents");
            }
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                Service.SaveFileTmp(User.Id, files.First());

                return Json(new { status = "Ok", from = "SaveFile" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", from = "SaveFile", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditDocument(DocumentViewModel document)
        {
            try
            {
                ModelState.Remove("UpdateFile");

                var documentEdit = AutoMapper.Mapper.Map<Document>(document);

                var result = Service.UpdateDocument(documentEdit, document.UpdateFile);

                var documentResult = AutoMapper.Mapper.Map<DocumentViewModel>(Service.GetDocument(document.Id));
                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = "El Documento se ha actualizado correctamente";
                    return PartialView("EditDocument", documentResult);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("EditDocument", documentResult);
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("Documents");
            }
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult UnsubscribeDocument(int id)
        {
            try
            {
                var result = Service.UnsubscribeDocument(id);

                if (result.Status != Status.Error)
                { 
                    return PartialView("Documents");
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("DeleteDocument", Service.GetDocument(id));
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("DeleteDocument", Service.GetDocument(id));
            }
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Library")]
        public ActionResult DeleteTotalDocument(int id)
        {
            try
            {
                var result = Service.DeleteDocument(id);

                if (result.Status != Status.Error)
                {
                    return PartialView("Documents");
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("DeleteDocument", Service.GetDocument(id));
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };

                return PartialView("DeleteDocument", Service.GetDocument(id));
            }
        }

        public JsonResult GetAreas()
        {
            var areas = Service.GetAreasByEntity(1);
            var items = new List<SelectListItem>();
            items.AddRange(areas.Select(area => new SelectListItem
            {
                Text = area.Name,
                Value = area.Id.ToString(CultureInfo.InvariantCulture)
            }));

            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}