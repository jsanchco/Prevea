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
                var roleId = userRole.Role.Id;
                switch (roleId)
                {
                    case (int)EnRole.Super:
                        html.RenderPartial("MenuSuper");
                        break;

                    case (int)EnRole.Admin:
                        html.RenderPartial("MenuAdmin");
                        break;

                    case (int)EnRole.PreveaPersonal:
                        html.RenderPartial("MenuPreveaPersonal");
                        break;

                    case (int)EnRole.PreveaCommercial:
                        html.RenderPartial("MenuPreveaCommercial");
                        break;

                    case (int)EnRole.ContactPerson:
                        html.RenderPartial("MenuContactPerson");
                        break;

                    case (int)EnRole.Employee:
                        html.RenderPartial("MenuEmployee");
                        break;

                    default:
                        return null;                     
                }
            }

            return null;
        }
    }
}