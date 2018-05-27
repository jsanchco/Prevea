namespace Prevea.Repository.Repository
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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationState> NotificationStates { get; set; }
        public DbSet<TrainingCourseModality> TrainingCourseModalities { get; set; }
        public DbSet<TrainingCourse> TrainingCourses { get; set; }
        public DbSet<TrainingCourseTrainingService> TrainingCoursesTrainingServices { get; set; }
        public DbSet<EstablishmentType> EstablishmentTypes { get; set; }
        public DbSet<WorkCenter> WorkCenters { get; set; }
        public DbSet<WorkCenterState> WorkCenterStates { get; set; }
        public DbSet<WorkCenterCompany> WorkCentersCompany { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<StretchAgency> StretchesAgencies { get; set; }
        public DbSet<EngagementType> EngagementTypes { get; set; }
        public DbSet<RequestMedicalExaminations> RequestMedicalExaminations { get; set; }
        public DbSet<RequestMedicalExaminationState> RequestMedicalExaminationStates { get; set; }
        public DbSet<RequestMedicalExaminationEmployee> RequestMedicalExaminationsEmployees { get; set; }
        public DbSet<RequestMedicalExaminationEmployeeState> RequestMedicalExaminationEmployeeStates { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<DoctorMedicalExaminationEmployee> DoctorMedicalExaminationEmployees { get; set; }
        public DbSet<MedicalExaminationDocuments> MedicalExaminationDocuments { get; set; }
        public DbSet<WorkStation> WorkStations { get; set; }
        public DbSet<DeltaCode> DeltaCodes { get; set; }
        public DbSet<RiskEvaluation> RiskEvaluations { get; set; }
        public DbSet<PreventivePlan> PreventivesPlans { get; set; }
        public DbSet<TemplatePreventivePlan> TemplatePreventivePlans { get; set; }
        public DbSet<PreventivePlanTemplatePreventivePlan> PreventivePlanTemplatePreventivePlans { get; set; }
        public DbSet<Mailing> Mailings { get; set; }
        public DbSet<DataMail> DataMails { get; set; }
        public DbSet<DataMailState> DataMailStates { get; set; }

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
            modelBuilder.Entity<Simulation>().HasOptional(x => x.UserAssigned).WithMany(y => y.SimulationsAssigned);
            modelBuilder.Entity<Employee>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestMedicalExaminations>().HasRequired(x => x.Company).WithMany(y => y.RequestMedicalExaminations).WillCascadeOnDelete(false);
            modelBuilder.Entity<Document>().HasOptional(x => x.DocumentFirmed).WithMany().HasForeignKey(x => x.DocumentFirmedId);
        }
    }
}
