namespace Prevea.WebPreveaUI.Controllers.Courses
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Collections.Generic;
    using Model.ViewModel;
    using Common;
    using System;
    using IService.IService;
    using Model.Model;

    #endregion

    public class ManagementCoursesController : BaseController
    {
        #region Constructor
        public ManagementCoursesController(IService service) : base(service)
        {
        }
        #endregion

        [HttpGet]
        public ActionResult ManagementCourses()
        {
            return PartialView("~/Views/Courses/ManagementCourses/ManagementCourses.cshtml");
        }

        [HttpGet]
        public ActionResult TrainingCourses_Read(int? Id)
        {
            var data = AutoMapper.Mapper.Map<List<TrainingCourseViewModel>>(Service.GetTrainingCourses(Id));

            return this.Jsonp(data);
        }

        [HttpPost]
        public JsonResult AddFamily(string name)
        {
            var trainingCourse = new TrainingCourse
            {
                Name = name,
                IsFamily = true,
                ReportsTo = 1
            };

            var resultTrainingCourse = Service.SaveTrainingCourse(trainingCourse);
            if (resultTrainingCourse.Status == Status.Error)
                return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);

            var notification = new Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromUser,
                NotificationStateId = (int)EnNotificationState.Issued,                
                ToRoleId = (int)EnRole.PreveaPersonal,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Cursos -> Alta Familia [{name}]"
            };
            var resultNotification = Service.SaveNotification(notification);
            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

            return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTitle(int familyId, string name)
        {
            var trainingCourse = new TrainingCourse
            {
                Name = name,
                IsTitle = true,
                ReportsTo = familyId
            };

            var resultTrainingCourse = Service.SaveTrainingCourse(trainingCourse);
            if (resultTrainingCourse.Status == Status.Error)
                return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);

            var notification = new Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromUser,
                NotificationStateId = (int)EnNotificationState.Issued,
                ToRoleId = (int)EnRole.PreveaPersonal,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Cursos -> Alta Título [{name}]"
            };
            var resultNotification = Service.SaveNotification(notification);
            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

            return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCourse(int titleId, string name, int hours, decimal price, int modality)
        {
            var trainingCourse = new TrainingCourse
            {
                Name = name,
                IsCourse = true,
                Hours = hours,
                Price = price,
                TrainingCourseModalityId = modality,
                ReportsTo = titleId
            };

            var resultTrainingCourse = Service.SaveTrainingCourse(trainingCourse);
            if (resultTrainingCourse.Status == Status.Error)
                return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);

            var notification = new Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromUser,
                NotificationStateId = (int)EnNotificationState.Issued,
                ToRoleId = (int)EnRole.PreveaPersonal,
                Observations =
                    $"{Service.GetUser(User.Id).Initials} - Cursos -> Alta Curso [{name} - {hours} horas - {price:#.##}€ - {modality}]"
            };
            var resultNotification = Service.SaveNotification(notification);
            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

            return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CanDeleteCourse(int courseId)
        {
            //// TODO -> Can I remove this node???
            //// TODO

            return Json(new { result = Status.Ok  }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteNode(int nodeId)
        {
            var trainingCourse = Service.GetTrainingCourse(nodeId);

            var resultTrainingCourse = Service.DeleteTrainingCourse(nodeId);
            if (resultTrainingCourse.Status == Status.Error)
                return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);

            var observations = string.Empty;
            if (trainingCourse.IsFamily)
            {
                observations = $"{Service.GetUser(User.Id).Initials} - Cursos -> Borrada Familia [{trainingCourse.Name}]";
            }

            if (trainingCourse.IsTitle)
            {
                observations = $"{Service.GetUser(User.Id).Initials} - Cursos -> Borrado Título [{trainingCourse.Name}]";
            }

            if (trainingCourse.IsCourse)
            {
                observations = $"{Service.GetUser(User.Id).Initials} - Cursos -> Borrado Curso [{trainingCourse.Name}]";
            }

            var notification = new Notification
            {
                DateCreation = DateTime.Now,
                NotificationTypeId = (int)EnNotificationType.FromUser,
                NotificationStateId = (int)EnNotificationState.Issued,
                ToRoleId = (int)EnRole.PreveaPersonal,
                Observations = observations                    
            };
            var resultNotification = Service.SaveNotification(notification);
            if (resultNotification.Status == Status.Error)
                return Json(new { result = resultNotification }, JsonRequestBehavior.AllowGet);

            return Json(new { result = resultTrainingCourse }, JsonRequestBehavior.AllowGet);
        }
    }
}