namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;

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
            var user = Service.GetUser(User.Id);

            return PartialView(user);
        }  
    }
}