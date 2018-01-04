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
                if (uState == EnUserState.NotMapped)
                    continue;

                context.UserStates.AddOrUpdate(new UserState { Name = uState.ToString() });
            }
            context.SaveChanges();

            var users = new List<User>
            {
                new User { FirstName = "Jesús", LastName = "Sánchez Corzo", Email = "jsanchco@gmail.com", DNI = "50963841G", Nick = "SU-50963841G"},
                new User { FirstName = "Juan Manuel", LastName = "Carrasco Martínez", Email = "jmcarrasco@preveaspa.com", DNI = "1899945P", Nick = "SU-1899945P"},
            };
            users.ForEach(p => context.Users.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var roles = (EnRole[]) Enum.GetValues(typeof(EnRole));
            foreach (var rol in roles)
            {
                if (rol == EnRole.NotMapped)
                    continue;

                context.Roles.AddOrUpdate(new Role { Name = rol.ToString() });
            }
            context.SaveChanges();

            var userroles = new List<UserRole>
            {
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 2, RoleId = 1 }
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
                if (dSate == EnDocumentState.NotMapped)
                    continue;

                context.DocumentStates.AddOrUpdate(new DocumentState { Name = dSate.ToString() });
            }
            context.SaveChanges();

            var modesPayment = (EnModePayment[])Enum.GetValues(typeof(EnModePayment));
            foreach (var modePayment in modesPayment)
            {
                if (modePayment == EnModePayment.NotMapped)
                    continue;

                context.ModesPayment.AddOrUpdate(new ModePayment { Name = modePayment.ToString() });
            }
            context.SaveChanges();

            var modesPaymentMedicalExamination = (EnModePaymentMedicalExamination[])Enum.GetValues(typeof(EnModePaymentMedicalExamination));
            foreach (var modePaymentMedicalExamination in modesPaymentMedicalExamination)
            {
                if (modePaymentMedicalExamination == EnModePaymentMedicalExamination.NotMapped)
                    continue;

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

            var companyStates = (EnCompanyState[])Enum.GetValues(typeof(EnCompanyState));
            foreach (var dSate in companyStates)
            {
                if (dSate == EnCompanyState.NotMapped)
                    continue;

                context.CompanyStates.AddOrUpdate(new CompanyState { Name = dSate.ToString() });
            }
            context.SaveChanges();

            var contractualCompanyStates = (EnContractualDocumentType[])Enum.GetValues(typeof(EnContractualDocumentType));
            foreach (var dType in contractualCompanyStates)
            {
                if (dType == EnContractualDocumentType.NotMapped)
                    continue;

                context.ContractualDocumentTypes.AddOrUpdate(new ContractualDocumentType { Name = dType.ToString() });
            }
            context.SaveChanges();

            var notificationTypes = (EnNotificationType[])Enum.GetValues(typeof(EnNotificationType));
            foreach (var dType in notificationTypes)
            {
                if (dType == EnNotificationType.NotMapped)
                    continue;

                context.NotificationTypes.AddOrUpdate(new NotificationType { Name = dType.ToString() });
            }
            context.SaveChanges();

            var notificationStates = (EnNotificationState[])Enum.GetValues(typeof(EnNotificationState));
            foreach (var dState in notificationStates)
            {
                if (dState == EnNotificationState.NotMapped)
                    continue;

                context.NotificationStates.AddOrUpdate(new NotificationState { Name = dState.ToString() });
            }
            context.SaveChanges();

            var simulationStates = (EnSimulationState[])Enum.GetValues(typeof(EnSimulationState));
            foreach (var dState in simulationStates)
            {
                if (dState == EnSimulationState.NotMapped)
                    continue;

                context.SimulationStates.AddOrUpdate(new SimulationState { Name = dState.ToString() });
            }
            context.SaveChanges();

            var trainingCourseModalities = (EnTrainingCourseModality[])Enum.GetValues(typeof(EnTrainingCourseModality));
            foreach (var dState in trainingCourseModalities)
            {
                if (dState == EnTrainingCourseModality.NotMapped)
                    continue;

                context.TrainingCourseModalities.AddOrUpdate(new TrainingCourseModality { Name = dState.ToString() });
            }
            context.SaveChanges();

            context.TrainingCourses.Add(new TrainingCourse
            {
                IsRoot = true
            });
            context.SaveChanges();
        }
    }
}
