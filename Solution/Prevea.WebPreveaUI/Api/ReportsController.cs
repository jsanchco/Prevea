namespace Prevea.WebPreveaUI.Api
{
    #region Using

    using System.Web.Http;

    #endregion

    public class ReportsController : ApiController
    {
        protected IService.IService.IService Service;

        [Route("Users")]
        public IHttpActionResult Users()
        {
            var users = Service.GetUsers();

            return Json(users);
        }
    }
}
