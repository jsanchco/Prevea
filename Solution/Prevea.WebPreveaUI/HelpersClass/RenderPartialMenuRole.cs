namespace Prevea.WebPreveaUI.HelpersClass
{
    #region Using

    using System.Web.Mvc;
    using Model.Model;
    using System.Linq;
    using System.Web.Mvc.Html;

    #endregion

    public static class RenderPartialMenuRole
    {
        public static MvcHtmlString MenuRole(this HtmlHelper html, User user)
        {
            var userRole = user.UserRoles.FirstOrDefault();
            if (userRole != null)
            {
                var roleName = userRole.Role.Name;
                switch (roleName)
                {
                    case "Super":
                        html.RenderPartial("MenuSuper");
                        break;

                    case "Admin":
                        html.RenderPartial("MenuAdmin");
                        break;

                    default:
                        return null;                     
                }
            }

            return null;
        }
    }
}