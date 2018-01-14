using System.IO;

namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Linq;
    using Model.ViewModel;
    using Model.Model;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using IService.IService;

    #endregion

    public class ProfileController : BaseController
    {
        #region Members

        public string TmpUpload => "~/Images/";

        #endregion

        #region Construtor

        public ProfileController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult ProfileUser()
        {
            var user = Service.GetUser(User.Id);

            var userRole = user?.UserRoles.FirstOrDefault();

            var simulationsAssigned = 0;
            var usersAssigned = 0;
            if (userRole != null)
            {
                switch (userRole.RoleId)
                {
                    case (int)EnRole.Super:
                        simulationsAssigned = Service.GetSimulations().Count(x => x.UserAssignedId == User.Id);
                        usersAssigned = Service.GetUsers().Count(x => 
                            x.UserRoles.FirstOrDefault().RoleId == (int)EnRole.PreveaPersonal ||
                            x.UserRoles.FirstOrDefault().RoleId == (int)EnRole.PreveaCommercial);
                        break;
                    case (int)EnRole.PreveaPersonal:
                        simulationsAssigned = Service.GetSimulations().Count(x => x.UserAssignedId == User.Id);
                        usersAssigned = Service.GetUsers().Count(x => x.UserParentId == user.Id);
                        break;

                    case (int)EnRole.PreveaCommercial:
                        simulationsAssigned = Service.GetSimulations().Count(x => x.UserId == User.Id);
                        usersAssigned = Service.GetUsers().Count(x => x.UserParentId == user.Id);
                        break;

                    case (int)EnRole.ContactPerson:
                        var companies = Service.GetCompanies();
                        foreach (var company in companies)
                        {
                            var contactPerson = company.ContactPersons.FirstOrDefault(x => x.UserId == user.Id);
                            if (contactPerson != null)
                            {
                                usersAssigned = company.Employees.Count;
                                break;
                            }
                        }                         
                        break;
                }
            }

            ViewBag.SimulationsAssigned = simulationsAssigned;
            ViewBag.CompaniesAssigned = user.Companies.Count(x => x.CompanyStateId == (int) EnCompanyState.Alta);
            ViewBag.UsersAssigned = usersAssigned;

            return PartialView(AutoMapper.Mapper.Map<UserViewModel>(user));
        }

        public ActionResult ChoosePhoto()
        {
            var user = Service.GetUser(User.Id);

            return PartialView("~/Views/Profile/ChoosePhoto.cshtml", user);
        }

        [HttpPost]
        public ActionResult SavePhoto(IEnumerable<HttpPostedFileBase> filePhoto)
        {
            try
            {
                var photo = filePhoto.First();
                var extension = Path.GetExtension(Path.GetFileName(photo.FileName));
                var url = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(TmpUpload), $"user_{User.Id}{extension}");

                photo.SaveAs(url);

                return Json(new Result { Status = Status.Ok}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return Json(new Result { Status = Status.Error }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}