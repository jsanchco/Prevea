﻿namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Data.Entity;

    #endregion

    public class Context : DbContext
    {
        #region Members

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<DocumentState> DocumentStates { get; set; }
        public DbSet<UserState> UserStates { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentUserCreator> DocumentUserCreators { get; set; }
        public DbSet<DocumentUserOwner> DocumentUserOwners { get; set; }
        public DbSet<HistoricDownloadDocument> HistoricDownloadDocuments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Cnae> Cnaes { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EconomicData> EconomicsDatas { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ModePayment> ModesPayment { get; set; }
        public DbSet<ModePaymentMedicalExamination> ModesPaymentMedicalExamination { get; set; }
        public DbSet<StretchEmployee> StretchesEmployees { get; set; }
        public DbSet<Simulation> Simulations { get; set; }
        public DbSet<ForeignPreventionService> ForeignPreventionServices { get; set; }
        public DbSet<AgencyService> AgencyServices { get; set; }
        public DbSet<TrainingService> TrainingServices { get; set; }
        public DbSet<SimulationCompany> SimulationCompanies { get; set; }
        public DbSet<SimulationState> SimulationStates { get; set; }
        public DbSet<CompanyState> CompanyStates { get; set; }
        public DbSet<ContractualDocumentCompany> ContractualsDocumentsCompany { get; set; }
        public DbSet<ContractualDocumentType> ContractualDocumentTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationState> NotificationStates { get; set; }

        #endregion

        #region Constructor

        public Context()
        {

        }

        public Context(string connectionString) : base(connectionString)
        {

        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistoricDownloadDocument>().HasRequired(x => x.User).WithMany(y => y.HistoricDownloadDocuments).WillCascadeOnDelete(false);
            modelBuilder.Entity<ContactPerson>().HasRequired(x => x.Company).WithMany(y => y.ContactPersons).WillCascadeOnDelete(false);
            modelBuilder.Entity<Employee>().HasRequired(x => x.Company).WithMany(y => y.Employees).WillCascadeOnDelete(false);
        }
    }
}
