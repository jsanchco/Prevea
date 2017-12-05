namespace Prevea.WebPreveaUI.HelpersClass
{
    #region Using

    using System.Web.Mvc;
    using Kendo.Mvc.Extensions;

    #endregion

    public class PreveaViewEngine : WebFormViewEngine
    {
        public PreveaViewEngine()
        {
            var viewLocations = new[] {
                //"~/Views/{1}/{0}.aspx",
                //"~/Views/{1}/{0}.ascx",
                //"~/Views/Shared/{0}.aspx",
                //"~/Views/Shared/{0}.ascx",
                //"~/Views/{0}/{1}.cshtml",
                "~/Views/CommercialTool/{1}/{0}.cshtml"
            };

            PartialViewLocationFormats = viewLocations;
            ViewLocationFormats = viewLocations;
        }
    }
}