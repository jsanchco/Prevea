﻿using System.Runtime.CompilerServices;

namespace Prevea.WebPreveaUI.App_Start
{
    #region Using

    using Model.Model;
    using Model.ViewModel;
    using System.Linq;

    #endregion

    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<AreaViewModel, Area>();
            AutoMapper.Mapper.CreateMap<Area, AreaViewModel>();

            AutoMapper.Mapper.CreateMap<CnaeViewModel, Cnae>();
            AutoMapper.Mapper.CreateMap<Cnae, CnaeViewModel>();

            AutoMapper.Mapper.CreateMap<Document, DocumentViewModel>()
                .ForMember(x => x.AreaId, x => x.MapFrom(y => y.Area.Id))
                .ForMember(x => x.AreaName, x => x.MapFrom(y => y.Area.Name))
                .ForMember(x => x.AreaDescription, x => x.MapFrom(y => y.Area.Description))
                .ForMember(x => x.AreaUrl, x => x.MapFrom(y => y.Area.Url))
                .ForMember(x => x.AreaStoreInServer, x => x.MapFrom(y => y.Area.StoreInServer))
                .ForMember(x => x.CompanyName, x => x.MapFrom(y => y.Company.Name))
                .ForMember(x => x.SimulationName, x => x.MapFrom(y => y.Simulation.Name))
                .ForMember(x => x.DocumentUserCreatorName, x => x.MapFrom(y => y.DocumentUserCreators.FirstOrDefault().User.Initials));
            AutoMapper.Mapper.CreateMap<DocumentViewModel, Document>();

            AutoMapper.Mapper.CreateMap<HistoricDownloadDocument, HistoricDownloadDocumentViewModel>()
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.FirstName + " " + y.User.LastName))
                .ForMember(x => x.DocumentName, x => x.MapFrom(y => y.Document.Name))
                .ForMember(x => x.Icon, x => x.MapFrom(y => y.Document.Icon));

            AutoMapper.Mapper.CreateMap<Company, CompanyViewModel>()
                .ForMember(x => x.CnaeName, x => x.MapFrom(y => y.Cnae.Name))
                .ForMember(x => x.ContactPersonName,
                    x => x.MapFrom(y => y.ContactPersons.FirstOrDefault() != null
                        ? y.ContactPersons.FirstOrDefault().User.FirstName + " " + y.ContactPersons.FirstOrDefault().User.LastName
                        : string.Empty))
                .ForMember(x => x.ContactPersonPhoneNumber,
                    x => x.MapFrom(y => y.ContactPersons.FirstOrDefault() != null
                        ? y.ContactPersons.FirstOrDefault().User.PhoneNumber
                        : string.Empty))
                .ForMember(x => x.ContactPersonEmail,
                    x => x.MapFrom(y => y.ContactPersons.FirstOrDefault() != null
                        ? y.ContactPersons.FirstOrDefault().User.Email
                        : string.Empty))
                .ForMember(x => x.Address,
                    x => x.MapFrom(y => string.IsNullOrEmpty(y.Address) ? string.Empty : y.Address))
                .ForMember(x => x.Location,
                    x => x.MapFrom(y => string.IsNullOrEmpty(y.Location) ? string.Empty : y.Location))
                .ForMember(x => x.PostalCode,
                    x => x.MapFrom(y => string.IsNullOrEmpty(y.PostalCode) ? string.Empty : y.PostalCode));                

            AutoMapper.Mapper.CreateMap<User, UserViewModel>()
                .ForMember(x => x.RoleId, x => x.MapFrom(y => y.UserRoles.FirstOrDefault().Role.Id))
                .ForMember(x => x.RoleName, x => x.MapFrom(y => y.UserRoles.FirstOrDefault().Role.Name))
                .ForMember(x => x.RoleDescription, x => x.MapFrom(y => y.UserRoles.FirstOrDefault().Role.Description))
                .ForMember(x => x.UserStateName, x => x.MapFrom(y => y.UserState.Name))
                .ForMember(x => x.WorkStationName, x => x.MapFrom(y => string.IsNullOrEmpty(y.WorkStation.ProfessionalCategory) 
                    ? y.WorkStation.Name ?? string.Empty
                    : $"{y.WorkStation.Name} ({y.WorkStation.ProfessionalCategory})"))
                .ForMember(x => x.UserParentInitials, x => x.MapFrom(y => y.UserParent.Initials));
            AutoMapper.Mapper.CreateMap<UserViewModel, User>();

            AutoMapper.Mapper.CreateMap<AgencyViewModel, Agency>();
            AutoMapper.Mapper.CreateMap<Agency, AgencyViewModel>();

            AutoMapper.Mapper.CreateMap<Simulation, SimulationViewModel>()
                .ForMember(x => x.SimulationStateName, x => x.MapFrom(y => y.SimulationState.Name))
                .ForMember(x => x.SimulationStateName, x => x.MapFrom(y => y.SimulationState.Description))
                .ForMember(x => x.UserInitials, x => x.MapFrom(y => y.User.Initials))
                .ForMember(x => x.CompanyId, x => x.MapFrom(y => y.SimulationCompanyActive.CompanyId))
                .ForMember(x => x.CompanyEnrollment, x => x.MapFrom(y => y.SimulationCompanyActive.Company.Enrollment))
                .ForMember(x => x.TotalForeignPreventionService, x => x.MapFrom(y => y.SimulationCompanyActive.Simulation.ForeignPreventionService.Total))
                .ForMember(x => x.TotalAgencyService, x => x.MapFrom(y => y.SimulationCompanyActive.Simulation.AgencyService.Total))
                .ForMember(x => x.TotalTrainingService, x => x.MapFrom(y => y.SimulationCompanyActive.Simulation.TrainingService.Total))
                .ForMember(x => x.UserAssignedInitials, x => x.MapFrom(y => y.UserAssigned.Initials));
            AutoMapper.Mapper.CreateMap<SimulationViewModel, Simulation>();

            AutoMapper.Mapper.CreateMap<PaymentMethodViewModel, PaymentMethod>();
            AutoMapper.Mapper.CreateMap<PaymentMethod, PaymentMethodViewModel>();

            AutoMapper.Mapper.CreateMap<Notification, NotificationViewModel>()
                .ForMember(x => x.NotificationTypeName, x => x.MapFrom(y => y.NotificationType.Name))
                .ForMember(x => x.NotificationTypeDescription, x => x.MapFrom(y => y.NotificationType.Description))
                .ForMember(x => x.NotificationStateName, x => x.MapFrom(y => y.NotificationState.Name))
                .ForMember(x => x.NotificationStateDescription, x => x.MapFrom(y => y.NotificationState.Description))
                .ForMember(x => x.ToRolName, x => x.MapFrom(y => y.ToRole.Description))
                .ForMember(x => x.SimulationAssignedTo, x => x.MapFrom(y => y.Simulation.UserAssignedId))
                .ForMember(x => x.SimulationName, x => x.MapFrom(y => y.Simulation.CompanyName))
                .ForMember(x => x.ToUserInitials, x => x.MapFrom(y => y.ToUser.Initials));
            AutoMapper.Mapper.CreateMap<NotificationViewModel, Notification>();

            AutoMapper.Mapper.CreateMap<TrainingCourse, TrainingCourseViewModel>()
                .ForMember(x => x.TrainingCourseModalityName, x => x.MapFrom(y => y.TrainingCourseModality.Name));
            AutoMapper.Mapper.CreateMap<TrainingCourseViewModel, TrainingCourse>();

            AutoMapper.Mapper.CreateMap<TrainingCourseTrainingService, TrainingCourseTrainingServiceViewModel>()
                .ForMember(x => x.TrainingCourseName, x => x.MapFrom(y => y.TrainingCourse.Name));
            AutoMapper.Mapper.CreateMap<TrainingCourseViewModel, TrainingCourse>();

            AutoMapper.Mapper.CreateMap<WorkCenter, WorkCenterViewModel>()
                .ForMember(x => x.EstablishmentTypeName, x => x.MapFrom(y => y.EstablishmentType.Name))
                .ForMember(x => x.WorkCenterStateName, x => x.MapFrom(y => y.WorkCenterState.Name));
            AutoMapper.Mapper.CreateMap<WorkCenterViewModel, WorkCenter>();

            AutoMapper.Mapper.CreateMap<EstablishmentTypeViewModel, EstablishmentType>();
            AutoMapper.Mapper.CreateMap<EstablishmentType, EstablishmentTypeViewModel>();

            AutoMapper.Mapper.CreateMap<EngagementTypeViewModel, EngagementType>();
            AutoMapper.Mapper.CreateMap<EngagementType, EngagementTypeViewModel>();

            AutoMapper.Mapper.CreateMap<AgencyServiceViewModel, AgencyService>();
            AutoMapper.Mapper.CreateMap<AgencyService, AgencyServiceViewModel>();

            AutoMapper.Mapper.CreateMap<RequestMedicalExaminations, RequestMedicalExaminationsViewModel>()
                .ForMember(x => x.CompanyName, x => x.MapFrom(y => y.Company.Name))
                .ForMember(x => x.NumberEmployees, x => x.MapFrom(y => y.RequestMedicalExaminationEmployees.Count))
                .ForMember(x => x.RequestMedicalExaminationStateDescription, x => x.MapFrom(y => y.RequestMedicalExaminationState.Description));
            AutoMapper.Mapper.CreateMap<RequestMedicalExaminationsViewModel, RequestMedicalExaminations>();

            AutoMapper.Mapper.CreateMap<RequestMedicalExaminationEmployee, RequestMedicalExaminationEmployeeViewModel>()
                .ForMember(x => x.RequestMedicalExaminationEmployeeStateDescription, x => x.MapFrom(y => y.RequestMedicalExaminationEmployeeState.Description))
                .ForMember(x => x.EmployeeDNI, x => x.MapFrom(y => y.Employee.User.DNI))
                .ForMember(x => x.EmployeeName, x => x.MapFrom(y => $"{y.Employee.User.FirstName} {y.Employee.User.LastName}"))
                .ForMember(x => x.ClinicName, x => x.MapFrom(y => y.Clinic.Name))
                .ForMember(x => x.RequestMedicalExaminationsStateId, x => x.MapFrom(y => y.RequestMedicalExaminations.RequestMedicalExaminationStateId));
            AutoMapper.Mapper.CreateMap<RequestMedicalExaminationEmployeeViewModel, RequestMedicalExaminationEmployee>();

            AutoMapper.Mapper.CreateMap<ClinicViewModel, Clinic>();
            AutoMapper.Mapper.CreateMap<Clinic, ClinicViewModel>();

            AutoMapper.Mapper.CreateMap<User, DoctorViewModel>()
                .ForMember(x => x.Name, x => x.MapFrom(y => y.FirstName + " " + y.LastName));

            AutoMapper.Mapper.CreateMap<WorkStationViewModel, WorkStation>();
            AutoMapper.Mapper.CreateMap<WorkStation, WorkStationViewModel>()
                //.ForMember(x => x.Name, x => x.MapFrom(y => string.IsNullOrEmpty(y.ProfessionalCategory) ? y.Name : $"{y.Name} [{y.ProfessionalCategory}]"))
                .ForMember(x => x.CnaeDescription, x => x.MapFrom(y => $"[{y.Cnae.CustomKey}] -> {y.Cnae.Name}"));

            AutoMapper.Mapper.CreateMap<DeltaCodeViewModel, DeltaCode>();
            AutoMapper.Mapper.CreateMap<DeltaCode, DeltaCodeViewModel>();

            AutoMapper.Mapper.CreateMap<RiskEvaluationViewModel, RiskEvaluation>();
            AutoMapper.Mapper.CreateMap<RiskEvaluation, RiskEvaluationViewModel>()
                .ForMember(x => x.WorkStationName, x => x.MapFrom(y => y.WorkStation.Name))
                .ForMember(x => x.WorkStationDescription, x => x.MapFrom(y => y.WorkStation.Description))
                .ForMember(x => x.DuplicateDeltaCodeId, x => x.MapFrom(y => y.DeltaCode.Id.ToString()))
                .ForMember(x => x.DeltaCodeName, x => x.MapFrom(y => y.DeltaCode.Name))
                .ForMember(x => x.DeltaCodeDescription, x => x.MapFrom(y => y.DeltaCode.Description));

            AutoMapper.Mapper.CreateMap<PreventivePlanViewModel, PreventivePlan>();
            AutoMapper.Mapper.CreateMap<PreventivePlan, PreventivePlanViewModel>()
                .ForMember(x => x.CompanyName, x => x.MapFrom(y => y.Company.Name))
                .ForMember(x => x.CompanyEnrollment, x => x.MapFrom(y => y.Company.Enrollment))
                .ForMember(x => x.DocumentBeginDate, x => x.MapFrom(y => y.Document.BeginDate))
                .ForMember(x => x.DocumentEndDate, x => x.MapFrom(y => y.Document.EndDate));

            AutoMapper.Mapper.CreateMap<MailingViewModel, Mailing>();
            AutoMapper.Mapper.CreateMap<Mailing, MailingViewModel>();

            AutoMapper.Mapper.CreateMap<DataMailViewModel, DataMail>();
            AutoMapper.Mapper.CreateMap<DataMail, DataMailViewModel>()
                .ForMember(x => x.DataMailStateDescription, x => x.MapFrom(y => y.DataMailState.Description))
                .ForMember(x => x.CreatorInitials, x => x.MapFrom(y => y.Creator.Initials));

            AutoMapper.Mapper.CreateMap<IncidenceViewModel, Incidence>();
            AutoMapper.Mapper.CreateMap<Incidence, IncidenceViewModel>()
                .ForMember(x => x.UserInitials, x => x.MapFrom(y => y.User.Initials))
                .ForMember(x => x.IncidenceStateDescription, x => x.MapFrom(y => y.IncidenceState.Description))
                .ForMember(x => x.CriticalNivelDescription, x => x.MapFrom(y => y.CriticalNivel.Description));

            AutoMapper.Mapper.CreateMap<TemplateMedicalExaminationViewModel, TemplatePreventivePlan>();
            AutoMapper.Mapper.CreateMap<TemplatePreventivePlan, TemplatePreventivePlanViewModel>();

            AutoMapper.Mapper.CreateMap<CorrectiveActionViewModel, CorrectiveAction>();
            AutoMapper.Mapper.CreateMap<CorrectiveAction, CorrectiveActionViewModel>()
                .ForMember(x => x.PriorityCorrectiveActionDescription, x => x.MapFrom(y => y.PriorityCorrectiveAction.Description));

            AutoMapper.Mapper.CreateMap<PriorityCorrectiveActionViewModel, PriorityCorrectiveAction>();
            AutoMapper.Mapper.CreateMap<PriorityCorrectiveAction, PriorityCorrectiveActionViewModel>();

            AutoMapper.Mapper.CreateMap<ContactPersonTypeViewModel, ContactPersonType>();
            AutoMapper.Mapper.CreateMap<ContactPersonType, ContactPersonTypeViewModel>();
        }
    }
}