namespace Prevea.ConsoleTestModel.Migrations
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    internal sealed class Configuration : DbMigrationsConfiguration<Repository.Repository.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(Repository.Repository.Context context)
        {
            var userState = (EnUserState[])Enum.GetValues(typeof(EnUserState));
            foreach (var uState in userState)
            {
                context.UserStates.AddOrUpdate(new UserState { Name = uState.ToString() });
            }
            context.SaveChanges();

            var users = new List<User>
            {
                new User { FirstName = "Jesús", LastName = "Sánchez Corzo", Email = "jsanchco@gmail.com", Dni = "50963841G", Nick = "SU-50963841G"},
                new User { FirstName = "Juan Manuel", LastName = "Carrasco Martínez", Email = "jmcarrasco@preveaspa.com", Dni = "1899945P", Nick = "SU-1899945P"},
                new User { FirstName = "Daniela", LastName = "Sánchez Aceituno", Email = "dsanchez@gmail.com", Dni = "11111111A", Nick = "PP-11111111A", UserParentId = 1},
                new User { FirstName = "Virginia", LastName = "Pérez Prusiel", Email = "vprusiel@gmail.com", Dni = "22222222B", Nick = "CP-22222222B", UserParentId = 3},
                new User { FirstName = "Rafael", LastName = "Fernández Sánchez", Email = "rfernandez@preveaspa.com", Dni = "33333333C", Nick = "PP-33333333C", UserParentId = 2},
                new User { FirstName = "Ignacio", LastName = "González Muñoz", Email = "igonzalez@preveaspa.com", Dni = "44444444D", Nick = "CP-44444444D", UserParentId = 5}
            };
            users.ForEach(p => context.Users.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var roles = (EnRole[]) Enum.GetValues(typeof(EnRole));
            foreach (var rol in roles)
            {
                context.Roles.AddOrUpdate(new Role { Name = rol.ToString() });
            }
            context.SaveChanges();

            var userroles = new List<UserRole>
            {
                new UserRole { UserId = 1, RoleId = (int) EnRole.Super },
                new UserRole { UserId = 2, RoleId = (int) EnRole.Super },
                new UserRole { UserId = 3, RoleId = (int) EnRole.PreveaPersonal },
                new UserRole { UserId = 4, RoleId = (int) EnRole.PreveaCommercial },
                new UserRole { UserId = 5, RoleId = (int) EnRole.PreveaPersonal },
                new UserRole { UserId = 6, RoleId = (int) EnRole.PreveaCommercial }
            };
            userroles.ForEach(p => context.UserRoles.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var entities = new List<Entity>
            {
                new Entity { Name = "Library", Description = "Biblioteca" },
                new Entity { Name = "Companies", Description = "Empresas" }
            };
            entities.ForEach(p => context.Entities.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var areas = new List<Area>
            {
                new Area { Name = "ADM", Description = "Administración", EntityId = 1},
                new Area { Name = "FOR", Description = "Formación", EntityId = 1 },
                new Area { Name = "TEC", Description = "Técnicas", EntityId = 1 },
                new Area { Name = "MET", Description = "Medicina trabajo", EntityId = 1 },
                new Area { Name = "COM", Description = "Comercial", EntityId = 1 }
            };
            areas.ForEach(p => context.Areas.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var dStates = (EnDocumentState[])Enum.GetValues(typeof(EnDocumentState));
            foreach (var dSate in dStates)
            {
                context.DocumentStates.AddOrUpdate(new DocumentState { Name = dSate.ToString() });
            }
            context.SaveChanges();

            var modesPayment = (EnModePayment[])Enum.GetValues(typeof(EnModePayment));
            foreach (var modePayment in modesPayment)
            {
                context.ModesPayment.AddOrUpdate(new ModePayment { Name = modePayment.ToString() });
            }
            context.SaveChanges();

            var modesPaymentMedicalExamination = (EnModePaymentMedicalExamination[])Enum.GetValues(typeof(EnModePaymentMedicalExamination));
            foreach (var modePaymentMedicalExamination in modesPaymentMedicalExamination)
            {
                context.ModesPaymentMedicalExamination.AddOrUpdate(new ModePaymentMedicalExamination { Name = modePaymentMedicalExamination.ToString() });
            }
            context.SaveChanges();

            //var documents = new List<Document>
            //{
            //    new Document {
            //        Description = "Primer Documento de ADMinistración",
            //        Observations = "*** Documento Original ***",
            //        DocumentNumber = 1,
            //        Date = DateTime.Now,
            //        DateModification = DateTime.Now,
            //        AreaId = 1,
            //        Edition = 1,
            //        DocumentStateId = 1,
            //        UrlRelative = "~/App_Data/Library/ADM/ADM_001_1.docx"},

            //    new Document {
            //        Description = "Segundo Documento de ADMinistración",
            //        Observations = "*** Documento Original ***",
            //        DocumentNumber = 2,
            //        Date = DateTime.Now,
            //        DateModification = DateTime.Now,
            //        AreaId = 1,
            //        Edition = 1,
            //        DocumentStateId = 1,
            //        UrlRelative = "~/App_Data/Library/ADM/ADM_002_1.docx"},

            //    new Document {
            //        Description = "Tercer Documento de ADMinistración",
            //        Observations = "*** Documento Original ***",
            //        DocumentNumber = 3,
            //        Date = DateTime.Now,
            //        DateModification = DateTime.Now,
            //        AreaId = 1,
            //        Edition = 1,
            //        DocumentStateId = 1,
            //        UrlRelative = "~/App_Data/Library/ADM/ADM_003_1.docx"}
            //};
            //documents.ForEach(p => context.Documents.AddOrUpdate(s => s.Id, p));
            //context.SaveChanges();

            var cnaes = Cnaes.GetCnaes();
            cnaes.ForEach(p => context.Cnaes.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var stretchEmployee = new StretchEmployee
            {
                Initial = 1,
                End = 3,
                AmountByEmployeeInTecniques = 150,
                AmountByEmployeeInHealthVigilance = 15,
                AmountByEmployeeInMedicalExamination = 40,
                IsComplete = true
            };
            context.StretchesEmployees.AddOrUpdate(stretchEmployee);           

            stretchEmployee = new StretchEmployee
            {
                Initial = 4,
                End = 15,
                AmountByEmployeeInTecniques = 60,
                AmountByEmployeeInHealthVigilance = 15,
                AmountByEmployeeInMedicalExamination = 40,
                IsComplete = false
            };
            context.StretchesEmployees.AddOrUpdate(stretchEmployee);

            stretchEmployee = new StretchEmployee
            {
                Initial = 16,
                End = 30,
                AmountByEmployeeInTecniques = 55,
                AmountByEmployeeInHealthVigilance = 10,
                AmountByEmployeeInMedicalExamination = 40,
                IsComplete = false
            };
            context.StretchesEmployees.AddOrUpdate(stretchEmployee);

            stretchEmployee = new StretchEmployee
            {
                Initial = 31,
                End = null,
                AmountByEmployeeInTecniques = 35,
                AmountByEmployeeInHealthVigilance = 10,
                AmountByEmployeeInMedicalExamination = 40,
                IsComplete = false
            };
            context.StretchesEmployees.AddOrUpdate(stretchEmployee);
            context.SaveChanges();

            var stretchAgency = new StretchAgency
            {
                Initial = 1,
                End = 3,
                AmountByRoster = 18,
                Percentege = 15
            };
            context.StretchesAgencies.AddOrUpdate(stretchAgency);

            stretchAgency = new StretchAgency
            {
                Initial = 4,
                End = 7,
                AmountByRoster = 14,
                Percentege = 10
            };
            context.StretchesAgencies.AddOrUpdate(stretchAgency);

            stretchAgency = new StretchAgency
            {
                Initial = 8,
                End = null,
                AmountByRoster = 12,
                Percentege = 10
            };
            context.StretchesAgencies.AddOrUpdate(stretchAgency);
            context.SaveChanges();

            var companyStates = (EnCompanyState[])Enum.GetValues(typeof(EnCompanyState));
            foreach (var dSate in companyStates)
            {
                context.CompanyStates.AddOrUpdate(new CompanyState { Name = dSate.ToString() });
            }
            context.SaveChanges();

            var contractualCompanyStates = (EnContractualDocumentType[])Enum.GetValues(typeof(EnContractualDocumentType));
            foreach (var dType in contractualCompanyStates)
            {
                context.ContractualDocumentTypes.AddOrUpdate(new ContractualDocumentType { Name = dType.ToString() });
            }
            context.SaveChanges();

            var notificationTypes = (EnNotificationType[])Enum.GetValues(typeof(EnNotificationType));
            foreach (var dType in notificationTypes)
            {
                context.NotificationTypes.AddOrUpdate(new NotificationType { Name = dType.ToString() });
            }
            context.SaveChanges();

            var notificationStates = (EnNotificationState[])Enum.GetValues(typeof(EnNotificationState));
            foreach (var dState in notificationStates)
            {
                context.NotificationStates.AddOrUpdate(new NotificationState { Name = dState.ToString() });
            }
            context.SaveChanges();

            var simulationStates = (EnSimulationState[])Enum.GetValues(typeof(EnSimulationState));
            foreach (var dState in simulationStates)
            {
                context.SimulationStates.AddOrUpdate(new SimulationState { Name = dState.ToString() });
            }
            context.SaveChanges();

            var trainingCourseModalities = (EnTrainingCourseModality[])Enum.GetValues(typeof(EnTrainingCourseModality));
            foreach (var dState in trainingCourseModalities)
            {
                context.TrainingCourseModalities.AddOrUpdate(new TrainingCourseModality { Name = dState.ToString() });
            }
            context.SaveChanges();

            var establishmentTypes = (EnEstablishmentType[])Enum.GetValues(typeof(EnEstablishmentType));
            foreach (var dState in establishmentTypes)
            {
                context.EstablishmentTypes.AddOrUpdate(new EstablishmentType { Name = dState.ToString() });
            }
            context.SaveChanges();

            var worksCenterStates = (EnWorkCenterState[])Enum.GetValues(typeof(EnWorkCenterState));
            foreach (var dState in worksCenterStates)
            {
                context.WorkCenterStates.AddOrUpdate(new WorkCenterState { Name = dState.ToString() });
            }
            context.SaveChanges();

            var typesEngagements = (EnEngagementType[])Enum.GetValues(typeof(EnEngagementType));
            foreach (var type in typesEngagements)
            {            
                context.EngagementTypes.AddOrUpdate(new EngagementType { Name = type.ToString() });
            }
            context.SaveChanges();

            var typesRequestMedicalExaminations = (EnRequestMedicalExaminationState[])Enum.GetValues(typeof(EnRequestMedicalExaminationState));
            foreach (var type in typesRequestMedicalExaminations)
            {
                context.RequestMedicalExaminationStates.AddOrUpdate(new RequestMedicalExaminationState { Name = type.ToString() });
            }
            context.SaveChanges();

            var typesRequestMedicalExaminationEmployees = (EnRequestMedicalExaminationEmployeeState[])Enum.GetValues(typeof(EnRequestMedicalExaminationEmployeeState));
            foreach (var type in typesRequestMedicalExaminationEmployees)
            {
                context.RequestMedicalExaminationEmployeeStates.AddOrUpdate(new RequestMedicalExaminationEmployeeState { Name = type.ToString() });
            }
            context.SaveChanges();

            context.TrainingCourses.Add(new TrainingCourse
            {
                IsRoot = true
            });
            context.SaveChanges();

            context.Configurations.Add(new Model.Model.Configuration { Tag = "AmountAgencyBySociety", Value = "250"});
            context.Configurations.Add(new Model.Model.Configuration { Tag = "AmountAgencyByAutonomous", Value = "100" });
            context.Configurations.Add(new Model.Model.Configuration { Tag = "PercentegeStretchEmployees", Value = "20" });
            context.Configurations.Add(new Model.Model.Configuration { Tag = "PercentegeStretchAgencies", Value = "20" });
            context.Configurations.Add(new Model.Model.Configuration { Tag = "IVA", Value = "1,21" });
            context.SaveChanges();
        }
    }
}
