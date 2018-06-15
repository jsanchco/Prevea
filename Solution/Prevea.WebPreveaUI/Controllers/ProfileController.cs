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
    using System.IO;
    using Kendo.Mvc.UI;
    using Common;
    using System.Diagnostics;
    using IRepository.IRepository;

    #endregion

    public class ProfileController : BaseController
    {
        #region Members

        public string TmpUpload => "~/Images/";

        #endregion

        #region Construtor

        public ProfileController(IRepository repository) : base(repository)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult ProfileUser()
        {
            var user = Service.GetUser(User.Id);

            var simulationsAssigned = 0;
            var companiesAssigned = 0;
            var usersAssigned = 0;

            GetDataHeader(ref simulationsAssigned, ref companiesAssigned, ref usersAssigned);

            ViewBag.SimulationsAssigned = simulationsAssigned;
            ViewBag.CompaniesAssigned = companiesAssigned;
            ViewBag.UsersAssigned = usersAssigned;

            ViewBag.SelectTabId = 0;

            if (user.UserRoles.FirstOrDefault()?.RoleId == (int) EnRole.Employee)
            {
                user.WorkStationCustom = user.WorkStation.Name;
                user.ProfessionalCategoryCustom = user.WorkStation.ProfessionalCategory;
            }

            return PartialView(AutoMapper.Mapper.Map<UserViewModel>(user));
        }

        public ActionResult ChoosePhoto()
        {
            var user = Service.GetUser(User.Id);

            return PartialView("~/Views/Profile/ChoosePhoto.cshtml", user);
        }

        public ActionResult ChangePassword()
        {
            return PartialView("~/Views/Profile/ChangePassword.cshtml");
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
                Debug.WriteLine(ex.Message);

                return Json(new Result { Status = Status.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Personal Data

        [HttpGet]
        public ActionResult PersonalDataProfile()
        {
            var user = Service.GetUser(User.Id);

            if (user.UserRoles.FirstOrDefault()?.RoleId == (int)EnRole.Employee)
            {
                user.WorkStationCustom = user.WorkStation.Name;
                user.ProfessionalCategoryCustom = user.WorkStation.ProfessionalCategory;
            }

            var data = AutoMapper.Mapper.Map<UserViewModel>(user);

            return PartialView("~/Views/Profile/PersonalDataProfile.cshtml", data);
        }

        [HttpPost]
        public ActionResult UpdatePersonalDataProfile(UserViewModel user)
        {
            try
            {
                var simulationsAssigned = 0;
                var companiesAssigned = 0;
                var usersAssigned = 0;

                GetDataHeader(ref simulationsAssigned, ref companiesAssigned, ref usersAssigned);

                ViewBag.SimulationsAssigned = simulationsAssigned;
                ViewBag.CompaniesAssigned = companiesAssigned;
                ViewBag.UsersAssigned = usersAssigned;

                var result = Service.SaveUser(user.RoleId, AutoMapper.Mapper.Map<User>(user));

                ViewBag.SelectTabId = 0;

                if (result.Status != Status.Error)
                {
                    ViewBag.Notification = "Tu Perfil se ha actualizado correctamente";

                    var userFind = Service.GetUser(User.Id);
                    if (userFind.UserRoles.FirstOrDefault()?.RoleId == (int)EnRole.Employee)
                    {
                        user.WorkStationCustom = userFind.WorkStation.Name;
                        user.ProfessionalCategoryCustom = userFind.WorkStation.ProfessionalCategory;
                    }

                    return PartialView("~/Views/Profile/ProfileUser.cshtml", user);
                }

                ViewBag.Error = new List<string> { result.Message };

                return PartialView("~/Views/Profile/ProfileUser.cshtml", user);
            }
            catch (Exception e)
            {
                ViewBag.SelectTabId = 0;

                ViewBag.Error = new List<string> { e.Message };

                return PartialView("~/Views/Profile/ProfileUser.cshtml", user);
            }
        }

        [HttpPost]
        public JsonResult UpdatePassword(string oldPassword, string newPassword)
        {
            try
            {
                var user = Service.GetUser(User.Id);
                if (user.Password != oldPassword)
                    return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
                
                var role = user.UserRoles.FirstOrDefault()?.Role;
                if (role == null)
                    return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

                user.Password = newPassword;
                Service.SaveUser(role.Id, user);

                return Json(new Result { Status = Status.Ok }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Economic Tracking

        [HttpGet]
        public ActionResult EconomicTrackingProfile()
        {
            var user = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(User.Id));

            return PartialView("~/Views/Profile/EconomicTrackingProfile.cshtml", user);
        }

        [HttpGet]
        public JsonResult Simulations_Read([DataSourceRequest] DataSourceRequest request, int userId)
        {
            var simulationsByUser = Service.GetSimulationsByUser(userId).Where(x => x.SimulationStateId == (int)EnSimulationState.SendToCompany);
            foreach (var simulation in simulationsByUser)
            {
                simulation.Total = Service.GetTotalSimulation(simulation.Id);
            }

            var data = AutoMapper.Mapper.Map<List<SimulationViewModel>>(simulationsByUser);

            return this.Jsonp(data);
        }

        #endregion

        #region Documents

        [HttpGet]
        public ActionResult DocumentsProfile()
        {
            var user = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(User.Id));

            return PartialView("~/Views/Profile/DocumentsProfile.cshtml", user);
        }

        #endregion

        private void GetDataHeader(ref int simulationsAssigned, ref int companiesAssigned, ref int usersAssigned)
        {
            var user = Service.GetUser(User.Id);

            var userRole = user?.UserRoles.FirstOrDefault();

            simulationsAssigned = 0;
            usersAssigned = 0;
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

            companiesAssigned = user.Companies.Count(x => x.CompanyStateId == (int)EnCompanyState.Alta);
        }
    }
}