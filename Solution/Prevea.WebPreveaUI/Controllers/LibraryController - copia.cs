using Prevea.Model.ViewModel;

namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using IRepository.IRepository;
    using HelpersClass;
    using Models;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using System.Collections.Generic;
    using System.Globalization;
    using Model.Model;
    using System.Linq;
    using System.Web;
    using System.IO;
    using System;
    using Common;

    #endregion

    [SessionExpire]
    public class LibraryController : BaseController
    {
        #region Constructor

        public LibraryController(IRepository repository) : base(repository)
        {
        }

        #endregion

        // GET: Library
        [Authorize(Roles = "SuperAdmin,Library")]
        public ActionResult Index()
        {
            InitView();

            return PartialView();
        }

        public JsonResult Library_Read([DataSourceRequest] DataSourceRequest request)
        {
            return this.Jsonp(Repository.GetLibraries_map(false));
        }

        public ActionResult Library_Read_old([DataSourceRequest] DataSourceRequest request)
        {
            return Json(Repository.GetLibraries_map(false).ToDataSourceResult(request));
        }

        public ActionResult LibraryByParent_Read([DataSourceRequest] DataSourceRequest request, int id, int? parentId)
        {
            return this.Jsonp(Repository.GetLibrariesByParent_map(id, parentId));
        }

        public ActionResult LibraryByParent_Read_old([DataSourceRequest] DataSourceRequest request, int id, int? parentId)
        {
            return Json(Repository.GetLibrariesByParent_map(id, parentId).ToDataSourceResult(request));
        }

        [HttpGet]
        [Authorize(Roles = "Library")]
        public ActionResult AddFileDocument(Library library)
        {
            return PartialView(library);
        }

        [HttpGet]
        [Authorize(Roles = "Library")]
        public ActionResult AddFileDocumentWithParent(int id)
        {
            var library = Repository.GetLibraryById(id);
            library.Observations = String.Empty;

            return PartialView(library);
        }

        [HttpGet]
        [Authorize(Roles = "Library")]
        public ActionResult DetailDocument(int id)
        {
            var library = Repository.GetLibraryById(id);
        
            return PartialView(library);
        }

        [HttpGet]
        [Authorize(Roles = "Library")]
        public ActionResult EditDocument(int id, string typeEdit, string routeFileDelete)
        {
            var library = Repository.GetLibraryById(id);
            if (!String.IsNullOrEmpty(routeFileDelete))
            {
                library.Url = routeFileDelete;
            }
            var libraryArea = Repository.GetLibraryAreaById(library.LibraryAreaId);

            ViewBag.TypeEdit = typeEdit;
            ViewBag.LibraryArea = libraryArea.Name;

            return PartialView(library);
        }

        [HttpPost]
        public ActionResult SaveFileDocument(Library library)
        {
            try
            {
                library.LibraryDesignationId = null;

                var date = DateTime.Now;
                library.DateInitial = date;
                library.DateModication = date;
                library.StatusLibraryId = 1;

                library.Observations = "*** Documento Original ***";

                if (Repository.SaveLibrary(library) == null)
                {
                    ViewBag.Error = new List<string> {"Ha ocurrido un error al guardar este Documento"};
                    return PartialView("AddFileDocument", library);
                }

                return PartialView("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };
                return PartialView("AddFileDocument", library);
            }
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> files, int libraryAreaId, int? parentId, string routeFileDelete)
        {
            try
            {
                if (files == null || files.Count() > 1)
                {
                    return Json(new {status = "Error", oldFile = string.Empty, message = "files es nulo o mayor que uno"}, JsonRequestBehavior.AllowGet);
                }

                if (!String.IsNullOrEmpty(routeFileDelete))
                {
                    if (System.IO.File.Exists(routeFileDelete))
                    {
                        System.IO.File.Delete(routeFileDelete);
                    }
                }

                var libraryViewModel = new LibraryViewModel();

                var libraryArea = Repository.GetLibraryAreaById(libraryAreaId);
                CreateDirectory(libraryArea.Name);

                libraryViewModel.LibraryAreaId = libraryAreaId;
                var fileName = Path.GetFileName(files.First().FileName);
                libraryViewModel.Extension = Path.GetExtension(fileName);
                libraryViewModel.Icon = GetIconFromExtension(libraryViewModel.Extension);                

                if (parentId != null)
                {
                    var libraryParent = Repository.GetLibraryById((int)parentId);

                    libraryViewModel.DocumentNumber = libraryParent.DocumentNumber;
                    libraryViewModel.Name = libraryParent.Name;

                    libraryViewModel.Edition = libraryParent.Edition + 1;
                }
                else
                {
                    var documentNumber = Repository.GetNumberLibrariesByLibraryArea(libraryViewModel.LibraryAreaId) + 1;
                    libraryViewModel.DocumentNumber = documentNumber.ToString("000");
                    libraryViewModel.Name = String.Format("{0}_{1}", libraryArea.Name, libraryViewModel.DocumentNumber);

                    libraryViewModel.Edition = 1;
                }

                libraryViewModel.FileName = String.Format("{0}_{1}{2}", libraryViewModel.Name, libraryViewModel.Edition,
                    libraryViewModel.Extension);

                libraryViewModel.Url = Path.Combine(Server.MapPath(Constants.LibraryUpload), libraryArea.Name,
                    libraryViewModel.FileName);

                var file = files.First();
                file.SaveAs(libraryViewModel.Url);

                return Json(new {status = "Ok", from = "SaveFile", libraryViewModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", from = "SaveFile", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveFileTmp(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (Session["User"] != null)
                    User = Session["User"] as UserProfile;

                var saveFileOk = Util.SaveFileTmp(files.First(), User.Id);

                return String.IsNullOrEmpty(saveFileOk) ? Json(new {status = "Ok", from = "SaveFileTmp"}, JsonRequestBehavior.AllowGet) : Json(new { status = "Error", from = "SaveFileTmp", message = saveFileOk }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", from = "SaveFileTmp", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemoveFile(string[] fileNames, string routeFileDelete)
        {
            try
            {
                if (!String.IsNullOrEmpty(routeFileDelete))
                {
                    if (System.IO.File.Exists(routeFileDelete))
                    {
                        System.IO.File.Delete(routeFileDelete);
                    }
                }

                return Json(new {status = "Ok", from = "RemoveFile", oldFile = String.Empty}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", from = "RemoveFile", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveFileDocumentWithParent(Library library)
        {
            try
            {
                var libraryOriginal = Repository.GetLibraryById(library.Id);

                library.LibraryDesignationId = null;

                library.DateInitial = libraryOriginal.DateInitial;
                library.DateModication = DateTime.Now;

                library.StatusLibraryId = 1;
                if (library.ParentId == null)
                {
                    library.ParentId = library.Id;
                }

                library.Id = 0;
                if (Repository.SaveLibraryWithParent(library) == null)
                {
                    ViewBag.Error = new List<string> { "Ha ocurrido un error al guardar este Documento" };
                    
                    return PartialView("AddFileDocumentWithParent", libraryOriginal);
                }

                return PartialView("DetailDocument", library);
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };
                return PartialView("AddFileDocumentWithParent", library);
            }
        }

        [HttpPost]
        public ActionResult EditDocument(Library library)
        {
            try
            {
                var libraryOriginal = Repository.GetLibraryById(library.Id);

                library.LibraryDesignationId = null;

                library.DateInitial = libraryOriginal.DateInitial;
                library.DateModication = DateTime.Now;

                if (Repository.UpdateLibrary(library) == null)
                {
                    ViewBag.Error = new List<string> { "Ha ocurrido un error al actualizar este Documento" };

                    return PartialView("EditDocument", libraryOriginal);
                }

                ViewBag.Notification = "Actualización correcta";
                return PartialView("EditDocument", Repository.GetLibraryById(library.Id));
            }
            catch (Exception e)
            {
                ViewBag.Error = new List<string> { e.Message };
                return PartialView("Index");
            }
        }

        public ActionResult DownloadFile(string directory, string fileName)
        {
            var filepath = Path.Combine(Server.MapPath("~/App_Data/Library"), directory, fileName);
            var filedata = System.IO.File.ReadAllBytes(filepath);
            var contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        public JsonResult GetLibraryAreas()
        {
            var areas = Repository.GetLibraryAreas();
            var items = new List<SelectListItem>();
            items.AddRange(areas.Select(area => new SelectListItem
            {
                Text = area.Name,
                Value = area.Id.ToString(CultureInfo.InvariantCulture)
            }));

            //return this.Jsonp(items);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsersInRoles()
        {
            var users = Repository.GetUsersInRoles(new List<string> {"Library"});
            var items = new List<SelectListItem>();
            items.AddRange(users.Select(user => new SelectListItem
            {
                Text = user.Initials,
                Value = user.Id.ToString(CultureInfo.InvariantCulture)
            }));

            //return this.Jsonp(items);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        private string GetIconFromExtension(string extension)
        {
            switch (extension)
            {
                case ".doc":
                case ".docx":
                    return "doc_opt.png";
                case ".xls":
                case ".xlsx":
                    return "xls_opt.png";
                case ".pdf":
                    return "pdf_opt.png";
                default:
                    return "pdf_opt.png";
            }
        }

        private void CreateDirectory(string directory)
        {
            var physicalPath = Path.Combine(Server.MapPath(Constants.LibraryUpload), directory);
            var exits = Directory.Exists(physicalPath);

            if (!exits)
                Directory.CreateDirectory(physicalPath);
        }

        private void InitView()
        {
            if (Session["User"] != null)
                User = Session["User"] as UserProfile;

            ViewBag.HasKendo = true;
            ViewBag.HasForm = true;

            ViewBag.User = User;
            ViewBag.active = "Library";
            ViewBag.opened = "Library";
        }
    }
}