namespace Prevea.WebPreveaUI
{
    #region Using

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Simulations",
                url: "CommercialTool/{controller}/{action}/{id}",
                defaults: new { controller = "Simulations", action = "Simulations", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Companies",
                url: "CommercialTool/{controller}/{action}/{id}",
                defaults: new { controller = "Companies", action = "Companies", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ManagementCourses",
                url: "Courses/{controller}/{action}/{id}",
                defaults: new { controller = "ManagementCourses", action = "ManagementCourses", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                   name: "Default",                   
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
               );
        }
    }
}
