namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Linq;
    using Model.ViewModel;

    #endregion

    public class ProfileController : BaseController
    {
        #region Construtor

        public ProfileController(IService.IService.IService service) : base(service)
        {
        }

        #endregion

        // GET: Profile
        public ActionResult ProfileUser()
        {
            var user = AutoMapper.Mapper.Map<UserViewModel>(Service.GetUser(User.Id));

            ViewBag.SimulationsAssigned = Service.GetSimulations().Count(x => x.UserAssignedId == User.Id);

            return PartialView(user);
        }  
    }
}