namespace Prevea.WebPreveaUI.HelpersClass
{
    #region Using

    using System.Web;
    using System.Web.Mvc;

    #endregion

    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpContext ctx = HttpContext.Current;
            //// check  sessions here
            //if (HttpContext.Current.Session["User"] == null)
            //{
            //    filterContext.Result = new RedirectResult("~/Login/Index");
            //    return;
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}