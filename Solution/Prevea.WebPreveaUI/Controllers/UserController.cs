namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using System.Linq;

    #endregion

    public class UserController : BaseController
    {
        #region Constructor

        public UserController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult Users()
        {
            return PartialView();
        }

        public JsonResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetUsersByUser(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult Users_Update()
        {
            try
            {
                var user = this.DeserializeObject<UserViewModel>("user");
                if (user == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });
                }

                var data = AutoMapper.Mapper.Map<User>(user);
                var result = Service.SaveUser(user.RoleId, data);

                if (result.Status != Status.Error)
                {
                    user = AutoMapper.Mapper.Map<UserViewModel>(result.Object);
                    return this.Jsonp(user);
                }
                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });
            }
        }

        public ActionResult Users_Destroy()
        {
            try
            {
                var user = this.DeserializeObject<UserViewModel>("user");
                if (user == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Usuario" });
                }

                var result = Service.DeleteUser((int)user.Id);

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(user);
                }

                return result.Status != Status.Error ? this.Jsonp(user) : this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Usuario" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Trabajador" });
            }
        }

        public ActionResult Users_Create()
        {
            try
            {
                var user = this.DeserializeObject<UserViewModel>("user");
                if (user == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });
                }

                var result = Service.SaveUser(user.RoleId, AutoMapper.Mapper.Map<User>(user));

                if (result.Status != Status.Error)
                {
                    user = AutoMapper.Mapper.Map<UserViewModel>(result.Object);
                    return this.Jsonp(user);
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });
            }
        }

        [HttpPost]
        public JsonResult Users_Subscribe(int userId, bool subscribe)
        {
            try
            {
                var result = Service.SubscribeUser(userId, subscribe);

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta del Usuario" } : new { Errors = "Se ha producido un error al Dar de Baja del Usuario" });
            }
        }

        private List<string> GetRolesFiltered()
        {
            var user = Service.GetUser(User.Id);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return null;

            switch (userRole.RoleId)
            {
                case (int)EnRole.Super:
                    return new List<string>
                    {
                        Enum.GetName(typeof(EnRole), (int)EnRole.Super),
                        Enum.GetName(typeof(EnRole), (int)EnRole.Library),
                        Enum.GetName(typeof(EnRole), (int)EnRole.PreveaPersonal),
                        Enum.GetName(typeof(EnRole), (int)EnRole.PreveaCommercial),
                        Enum.GetName(typeof(EnRole), (int)EnRole.ExternalPersonal)
                    };
                case (int)EnRole.PreveaPersonal:
                    return new List<string>
                    {
                        Enum.GetName(typeof(EnRole), (int)EnRole.Library),
                        Enum.GetName(typeof(EnRole), (int)EnRole.PreveaPersonal),
                        Enum.GetName(typeof(EnRole), (int)EnRole.PreveaCommercial),
                        Enum.GetName(typeof(EnRole), (int)EnRole.ExternalPersonal)
                    };
                case (int)EnRole.Library:
                    return new List<string>
                    {
                        Enum.GetName(typeof(EnRole), (int)EnRole.Library)
                    };
                case (int)EnRole.PreveaCommercial:
                    return new List<string>
                    {
                        Enum.GetName(typeof(EnRole), (int)EnRole.ExternalPersonal)
                    };

                default:
                    return null;
            }
        }

        public JsonResult GetRoles([DataSourceRequest] DataSourceRequest request)
        {
            var user = Service.GetUser(User.Id);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return null;

            var roles = new List<CustomRole>();
            switch (userRole.RoleId)
            {
                case (int)EnRole.Super:
                    roles.Add(new CustomRole
                    {
                        RoleId = (int) EnRole.Super,
                        RoleName = Enum.GetName(typeof(EnRole), (int) EnRole.Super),
                        RoleDescription = "Super"
                    });
                    roles.Add(new CustomRole
                    {
                        RoleId = (int) EnRole.Library,
                        RoleName = Enum.GetName(typeof(EnRole), (int) EnRole.Library),
                        RoleDescription = "Biblioteca"
                    });
                    roles.Add(new CustomRole
                    {
                        RoleId = (int) EnRole.PreveaPersonal,
                        RoleName = Enum.GetName(typeof(EnRole), (int) EnRole.PreveaPersonal),
                        RoleDescription = "Personal de Prevea"
                    });
                    roles.Add(new CustomRole
                    {
                        RoleId = (int) EnRole.PreveaCommercial,
                        RoleName = Enum.GetName(typeof(EnRole), (int) EnRole.PreveaCommercial),
                        RoleDescription = "Comercial de Prevea"
                    });
                    roles.Add(new CustomRole
                    {
                        RoleId = (int) EnRole.ExternalPersonal,
                        RoleName = Enum.GetName(typeof(EnRole), (int) EnRole.ExternalPersonal),
                        RoleDescription = "Personal Externo"
                    });

                    break;
                case (int)EnRole.PreveaPersonal:   
                    roles.Add(new CustomRole
                    {
                        RoleId = (int)EnRole.Library,
                        RoleName = Enum.GetName(typeof(EnRole), (int)EnRole.Library),
                        RoleDescription = "Biblioteca"
                    });
                    roles.Add(new CustomRole
                    {
                        RoleId = (int)EnRole.PreveaCommercial,
                        RoleName = Enum.GetName(typeof(EnRole), (int)EnRole.PreveaCommercial),
                        RoleDescription = "Comercial de Prevea"
                    });
                    roles.Add(new CustomRole
                    {
                        RoleId = (int)EnRole.ExternalPersonal,
                        RoleName = Enum.GetName(typeof(EnRole), (int)EnRole.ExternalPersonal),
                        RoleDescription = "Personal Externo"
                    });

                    break;
                case (int)EnRole.Library:
                    roles.Add(new CustomRole
                    {
                        RoleId = (int)EnRole.Library,
                        RoleName = Enum.GetName(typeof(EnRole), (int)EnRole.Library),
                        RoleDescription = "Biblioteca"
                    });

                    break;
                case (int)EnRole.PreveaCommercial:
                    roles.Add(new CustomRole
                    {
                        RoleId = (int)EnRole.ExternalPersonal,
                        RoleName = Enum.GetName(typeof(EnRole), (int)EnRole.ExternalPersonal),
                        RoleDescription = "Personal Externo"
                    });

                    break;
            }

            return this.Jsonp(roles);
        }
    }

    public class CustomRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}