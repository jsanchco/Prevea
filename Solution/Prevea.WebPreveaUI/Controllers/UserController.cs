﻿namespace Prevea.WebPreveaUI.Controllers
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
    using Model.CustomModel;
    using IRepository.IRepository;

    #endregion

    public class UserController : BaseController
    {
        #region Constructor

        public UserController(IRepository repository) : base(repository)
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

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromUser,
                    NotificationStateId = (int)EnNotificationState.Issued,
                    ToRoleId = (int)EnRole.Super,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Modificado usuario [{((User)result.Object).Initials}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                user = AutoMapper.Mapper.Map<UserViewModel>(result.Object);
                return this.Jsonp(user);
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

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromUser,
                    NotificationStateId = (int)EnNotificationState.Issued,
                    ToRoleId = (int)EnRole.Super,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Borrado usuario [{user.Initials}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                return this.Jsonp(user);
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

                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Usuario" });

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromUser,
                    NotificationStateId = (int)EnNotificationState.Issued,
                    ToRoleId = (int)EnRole.Super,
                    Observations =
                        $"{Service.GetUser(User.Id).Initials} - Creado un nuevo usuario [{((User)result.Object).Initials}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                user = AutoMapper.Mapper.Map<UserViewModel>(result.Object);
                return this.Jsonp(user);
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
                if (result.Status == Status.Error)
                    return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta del Usuario" } : new { Errors = "Se ha producido un error al Dar de Baja del Usuario" });

                var notification = new Model.Model.Notification
                {
                    DateCreation = DateTime.Now,
                    NotificationTypeId = (int)EnNotificationType.FromUser,
                    NotificationStateId = (int)EnNotificationState.Issued,
                    ToRoleId = (int)EnRole.Super,
                    Observations = subscribe ?
                        $"{Service.GetUser(User.Id).Initials} - Alta usuario [{((User)result.Object).Initials}]" :
                        $"{Service.GetUser(User.Id).Initials} - Baja usuario [{((User)result.Object).Initials}]"
                };
                var resultNotification = Service.SaveNotification(notification);
                if (resultNotification.Status == Status.Error)
                    return this.Jsonp(new { Errors = resultNotification });

                return Json(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                return Json(subscribe ? new { Errors = "Se ha producido un error al Dar de Alta del Usuario" } : new { Errors = "Se ha producido un error al Dar de Baja del Usuario" });
            }
        }

        public JsonResult GetCustomRoles([DataSourceRequest] DataSourceRequest request)
        {
            var user = Service.GetUser(User.Id);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return null;

            var roles = new List<CustomRole>();
            switch (userRole.RoleId)
            {
                case (int)EnRole.Super:
                    roles = Service.GetCustomRoles(new List<int>
                    {
                        (int)EnRole.Super,
                        (int)EnRole.Library,
                        (int)EnRole.PreveaPersonal,
                        (int)EnRole.PreveaCommercial,
                        (int)EnRole.ExternalPersonal
                    });

                    break;
                case (int)EnRole.PreveaPersonal:
                    roles = Service.GetCustomRoles(new List<int>
                    {
                        (int)EnRole.Library,
                        (int)EnRole.PreveaCommercial,
                        (int)EnRole.ExternalPersonal
                    });

                    break;
                case (int)EnRole.Library:
                    roles = Service.GetCustomRoles(new List<int>
                    {
                        (int)EnRole.Library
                    });

                    break;
                case (int)EnRole.PreveaCommercial:
                    roles = Service.GetCustomRoles(new List<int>
                    {
                        (int)EnRole.ExternalPersonal
                    });

                    break;
            }

            return this.Jsonp(roles);
        }

        [HttpGet]
        public ActionResult ContactAs()
        {
            return PartialView();
        }

        public JsonResult ContactAs_Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = Service.GetUsersByUserFromContactAs(User.Id);
            var usersFromContactAs = new List<UserFromContactAsViewModel>();
            foreach (var user in users)
            {
                var userFromContactAsViewModel = new UserFromContactAsViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = $"{user.FirstName} {user.LastName}",
                    RoleDescription = user.UserRoles.FirstOrDefault()?.Role.Description,
                    Initials = user.Initials,
                    Nick = user.Nick,
                    Password = user.Password
                };
                var role = user.UserRoles.FirstOrDefault();
                if (role?.Role.Id == (int) EnRole.ContactPerson)
                {
                    var contactPerson = Service.GetContactPersons().FirstOrDefault(x => x.UserId == user.Id);
                    if (contactPerson != null)
                    {
                        userFromContactAsViewModel.CompanyName = contactPerson.Company.Name;
                        userFromContactAsViewModel.CompanyEnrollment = contactPerson.Company.Enrollment;
                    }
                }
                if (role?.Role.Id == (int)EnRole.Employee)
                {
                    var employee = Service.GetEmployees().FirstOrDefault(x => x.UserId == user.Id);
                    if (employee != null)
                    {
                        userFromContactAsViewModel.CompanyName = employee.Company.Name;
                        userFromContactAsViewModel.CompanyEnrollment = employee.Company.Enrollment;
                    }
                }
                usersFromContactAs.Add(userFromContactAsViewModel);
            }

            return this.Jsonp(usersFromContactAs);
        }
    }
}