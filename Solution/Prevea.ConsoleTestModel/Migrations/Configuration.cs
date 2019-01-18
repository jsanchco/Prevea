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
                new User { FirstName = "Jes�s", LastName = "S�nchez Corzo", Email = "jsanchco@gmail.com", DNI = "50963841G", Nick = "SU-50963841G"},
                new User { FirstName = "Juan Manuel", LastName = "Carrasco Mart�nez", Email = "jmcarrasco@preveaspa.com", DNI = "1828925B", Nick = "SU-1828925B"},
                new User { FirstName = "Virgilio", LastName = "Carrasco Mart�nez", Email = "vcarrasco@preveaspa.com", DNI = "51919038B", Nick = "SU-51919038B"},
                new User { FirstName = "Virgilio", LastName = "Carrasco Mart�nez", Email = "vcarrasco@preveaspa.com", DNI = "51919038B", Nick = "PP-51919038B", UserParentId = 3},
                new User { FirstName = "Virgilio", LastName = "Carrasco Mart�nez", Email = "vcarrasco@preveaspa.com", DNI = "51919038B", Nick = "CP-51919038B", UserParentId = 4},
                new User { FirstName = "Rafael", LastName = "Fern�ndez S�nchez", Email = "rfernandez@prevespa.com", DNI = "51642026B", Nick = "PP-51642026B", UserParentId = 3},
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
                new UserRole { UserId = 3, RoleId = (int) EnRole.Super },
                new UserRole { UserId = 4, RoleId = (int) EnRole.PreveaPersonal },
                new UserRole { UserId = 5, RoleId = (int) EnRole.PreveaCommercial },
                new UserRole { UserId = 6, RoleId = (int) EnRole.PreveaPersonal },
            };
            userroles.ForEach(p => context.UserRoles.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var entities = new List<Entity>
            {
                new Entity { Name = "Library", Description = "Biblioteca" },
                new Entity { Name = "Company", Description = "Empresas" },
                new Entity { Name = "MedicalExamination", Description = "Reconocimientos M�dicos" }
            };
            entities.ForEach(p => context.Entities.AddOrUpdate(s => s.Id, p));
            context.SaveChanges();

            var areas = new List<Area>
            {
                new Area { Name = "ADM", Description = "Administraci�n", EntityId = 1, Url = "~/App_Data/Library/ADM/", StoreInServer = true }, // 1
                new Area { Name = "FOR", Description = "Formaci�n", EntityId = 1, Url = "~/App_Data/Library/FOR/", StoreInServer = true }, // 2
                new Area { Name = "TEC", Description = "T�cnicas", EntityId = 1, Url = "~/App_Data/Library/TEC/", StoreInServer = true }, // 3
                new Area { Name = "MET", Description = "Medicina trabajo", EntityId = 1, Url = "~/App_Data/Library/MET/", StoreInServer = true }, // 4
                new Area { Name = "COM", Description = "Comercial", EntityId = 1, Url = "~/App_Data/Library/COM/", StoreInServer = true   }, // 5
                new Area { Name = "OFE_SPA", Description = "Oferta SPA", EntityId = 2, Url = "~/App_Data/Company/OFE/SPA/", StoreInServer = false }, // 6
                new Area { Name = "OFE_FOR", Description = "Oferta Formaci�n", EntityId = 2, Url = "~/App_Data/Company/OFE/FOR/", StoreInServer = false }, // 7
                new Area { Name = "OFE_GES", Description = "Oferta Gestor�a", EntityId = 2, Url = "~/App_Data/Company/OFE/GES/", StoreInServer = false  }, // 8
                new Area { Name = "CON_SPA", Description = "Contrato SPA", EntityId = 2, Url = "~/App_Data/Company/CON/SPA/", StoreInServer = false }, // 9
                new Area { Name = "CON_FOR", Description = "Contrato Formaci�n", EntityId = 2, Url = "~/App_Data/Company/CON/FOR/", StoreInServer = false }, // 10
                new Area { Name = "CON_GES", Description = "Contrato Gestor�a", EntityId = 2, Url = "~/App_Data/Company/CON/GES/", StoreInServer = false }, // 11
                new Area { Name = "ANX", Description = "Anexo", EntityId = 2, Url = "~/App_Data/Company/ANX/", StoreInServer = true }, // 12
                new Area { Name = "OTR", Description = "Otros Documentos", EntityId = 2, Url = "~/App_Data/Company/OTR/", StoreInServer = true }, // 13
                new Area { Name = "UNS", Description = "Baja Documento", EntityId = 2, Url = "~/App_Data/Company/UNS/", StoreInServer = false }, // 14
                new Area { Name = "CIT", Description = "Citaci�n", EntityId = 3, Url = "~/App_Data/MedicalExamination/CIT/", StoreInServer = false }, // 15
                new Area { Name = "RCM", Description = "Reconocimiento M�dico", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = false }, // 16
                new Area { Name = "BLT", Description = "An�litica de Sangre", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = true }, // 17
                new Area { Name = "ELT", Description = "Electrocardiograma", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = true }, // 18
                new Area { Name = "AUD", Description = "Informe Audiom�trico", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = true }, // 19
                new Area { Name = "SPR", Description = "Espirometr�a", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = true }, // 20
                new Area { Name = "URA", Description = "An�litica de Orina", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = true }, // 21
                new Area { Name = "OTR", Description = "Otros", EntityId = 3, Url = "~/App_Data/MedicalExamination/RCM/", StoreInServer = true } // 22
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
            //        Description = "Primer Documento de ADMinistraci�n",
            //        Observations = "*** Documento Original ***",
            //        DocumentNumber = 1,
            //        Date = DateTime.Now,
            //        DateModification = DateTime.Now,
            //        AreaId = 1,
            //        Edition = 1,
            //        DocumentStateId = 1,
            //        UrlRelative = "~/App_Data/Library/ADM/ADM_001_1.docx"},

            //    new Document {
            //        Description = "Segundo Documento de ADMinistraci�n",
            //        Observations = "*** Documento Original ***",
            //        DocumentNumber = 2,
            //        Date = DateTime.Now,
            //        DateModification = DateTime.Now,
            //        AreaId = 1,
            //        Edition = 1,
            //        DocumentStateId = 1,
            //        UrlRelative = "~/App_Data/Library/ADM/ADM_002_1.docx"},

            //    new Document {
            //        Description = "Tercer Documento de ADMinistraci�n",
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

            var dataMailStates = (EnDataMailState[])Enum.GetValues(typeof(EnDataMailState));
            foreach (var type in dataMailStates)
            {
                context.DataMailStates.AddOrUpdate(new DataMailState { Name = type.ToString() });
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

            context.DeltaCodes.Add(new DeltaCode { Id = 1, Name = "Ca�da de personas a distinto nivel" });
            context.DeltaCodes.Add(new DeltaCode { Id = 2, Name = "Ca�da de personas al mismo nivel" });
            context.DeltaCodes.Add(new DeltaCode { Id = 3, Name = "Ca�das de objetos por desplome o derrumbamiento" });
            context.DeltaCodes.Add(new DeltaCode { Id = 4, Name = "Ca�das de objetos por manipulaci�n" });
            context.DeltaCodes.Add(new DeltaCode { Id = 5, Name = "Ca�das por objetos desprendidos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 6, Name = "Pisadas sobre objetos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 7, Name = "Choques contra objetos inm�viles" });
            context.DeltaCodes.Add(new DeltaCode { Id = 8, Name = "Choques contra objetos m�viles" });            
            context.DeltaCodes.Add(new DeltaCode { Id = 9, Name = "Golpes por objetos o herramientas" });
            context.DeltaCodes.Add(new DeltaCode { Id = 10, Name = "Proyecci�n de fragmentos o part�culas" });
            context.DeltaCodes.Add(new DeltaCode { Id = 11, Name = "Atrapamiento por o entre objetos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 12, Name = "Atrapamiento por vuelco de m�quinas, tractores o veh�culos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 13, Name = "Sobreesfuerzos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 14, Name = "Exposici�n a temperaturas ambientales extremas" });
            context.DeltaCodes.Add(new DeltaCode { Id = 15, Name = "Contactos t�rmicos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 16, Name = "Exposici�n a contactos el�ctricos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 17, Name = "Exposici�n a sustancias nocivas" });
            context.DeltaCodes.Add(new DeltaCode { Id = 18, Name = "Contactos sustancias c�usticas y/o corrosivas" });
            context.DeltaCodes.Add(new DeltaCode { Id = 19, Name = "Exposici�n a radiaciones" });
            context.DeltaCodes.Add(new DeltaCode { Id = 20, Name = "Explosiones" });
            context.DeltaCodes.Add(new DeltaCode { Id = 21, Name = "Incendios" });
            context.DeltaCodes.Add(new DeltaCode { Id = 22, Name = "Accidentes causados por seres vivos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 23, Name = "Atropellos o golpes con veh�culos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 24, Name = "Accidentes de tr�fico" });
            context.DeltaCodes.Add(new DeltaCode { Id = 25, Name = "Causas naturales" });
            context.DeltaCodes.Add(new DeltaCode { Id = 26, Name = "Otras" });
            context.DeltaCodes.Add(new DeltaCode { Id = 27, Name = "Agentes qu�micos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 28, Name = "Agentes f�sicos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 29, Name = "Agentes biol�gicos" });
            context.DeltaCodes.Add(new DeltaCode { Id = 30, Name = "Otras circunstancias" });
            context.DeltaCodes.Add(new DeltaCode { Id = 31, Name = "Condiciones ergon�micas" });
            context.DeltaCodes.Add(new DeltaCode { Id = 32, Name = "Factores psicosociales" });
            context.SaveChanges();

            var incidencesStates = (EnIncidenceState[])Enum.GetValues(typeof(EnIncidenceState));
            foreach (var type in incidencesStates)
            {
                context.IncidenceStates.AddOrUpdate(new IncidenceState { Name = type.ToString() });
            }
            context.SaveChanges();

            var criticalNivelStates = (EnCriticalNivel[])Enum.GetValues(typeof(EnCriticalNivel));
            foreach (var type in criticalNivelStates)
            {
                context.CriticalNivels.AddOrUpdate(new CriticalNivel { Name = type.ToString() });
            }
            context.SaveChanges();

            var priorityCorrectiveActions = (EnPriorityCorrectiveAction[])Enum.GetValues(typeof(EnPriorityCorrectiveAction));
            foreach (var type in priorityCorrectiveActions)
            {
                context.PriorityCorrectiveActions.AddOrUpdate(new PriorityCorrectiveAction { Name = type.ToString() });
            }
            context.SaveChanges();

            var contactPersonTypes = (EnContactPersonType[])Enum.GetValues(typeof(EnContactPersonType));
            foreach (var type in contactPersonTypes)
            {
                context.ContactPersonTypes.AddOrUpdate(new ContactPersonType { Name = type.ToString() });
            }
            context.SaveChanges();
        }
    }
}
