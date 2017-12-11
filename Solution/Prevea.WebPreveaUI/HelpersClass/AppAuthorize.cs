namespace Prevea.WebPreveaUI.HelpersClass
{
    #region Using

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class AppAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "UserNoRegistred" }));
            }
        }
    }
}