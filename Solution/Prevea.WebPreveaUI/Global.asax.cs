namespace Prevea.WebPreveaUI
{
    #region Using

    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using IRepository.IRepository;
    using Newtonsoft.Json;
    using Security;
    using App_Start;
    using System.Web.Http;

    #endregion

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<AppPrincipalSerializeModel>(authTicket.UserData);
                var newUser = new AppPrincipal(authTicket.Name)
                {
                    Id = serializeModel.UserId,
                    FirstName = serializeModel.FirstName,
                    LastName = serializeModel.LastName,
                    Roles = serializeModel.Roles
                };

                HttpContext.Current.User = newUser;
            }
        }

        protected void Application_PostAuthenticateRequest_old(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                        if (formsAuthenticationTicket != null)
                        {
                            var userId = formsAuthenticationTicket.Name;
                            var roles = string.Empty;

                            var repository = DependencyResolver.Current.GetService<IRepository>();
                            if (repository != null)
                            {
                                var user = repository.GetUserByGuid(userId);
                                user = repository.GetUser(1);
                                if (user != null)
                                {
                                    var listRoles = repository.GetRolesByUser(user.Id);
                                    if (listRoles.Count > 0)
                                        roles = String.Join(";", listRoles);
                                }
                            }

                            ////Let us set the Pricipal with our user specific details
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(userId, "Forms"), roles.Split(';'));
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: {0}",
                                   ex.Message);
                    }
                }
            }
        }
    }
}
