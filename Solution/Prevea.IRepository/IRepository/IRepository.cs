namespace Prevea.IRepository.IRepository
{
    #region Using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.Model;
    using Model.CustomModel;
    using System;

    #endregion

    public interface IRepository : IDisposable
    {
        #region User

        #region Generic
        List<User> GetUsers();
        User GetUser(int id);
        User CreateUser();
        User SaveUser(User user);
        User SaveUser(int? roleId, User user);
        User UpdateUser(int id, User user, int? roleId = null);
        User SubscribeUser(int id, bool subscribe);
        bool DeleteUser(int id);
        #endregion

        User GetUserByGuid(string guid);
        List<string> GetRolesByUser(int id);
        User ValidateUser(string user, string password);
        Task<User> ValidateUserAsync(string user, string password);
        List<User> GetUsersInRoles(List<string> roles);
        List<User> GetContatcPersonsByCompany(int companyId);        
        List<User> GetUsersByUser(int id);
        List<CustomRole> GetCustomRoles(List<int> listRoles);

        #endregion

        #region Document

        List<Document> GetDocuments();
        Document GetDocument(int id);
        Document SaveDocument(int userCreatorId, int? userOwnerId, Document document);
        Document SaveDocumentWithParent(int userCreatorId, Document document);
        Document UpdateDocument(int id, Document document);        
        Document UnsubscribeDocument(int id);
        Document SubscribeDocument(int id);
        bool DeleteDocument(int id);

        List<Document> GetDocumentsByState(int documentStateId);
        List<Document> GetDocumentsByParent(int id, int? parentId);
        List<Document> GetChildrenDocument(int parentId);
        int GetNumberDocumentsByArea(int areaId);

        #endregion

        #region Entity

        List<Entity> GetEntities();
        Entity GetEntity(int id);
        Entity SaveEntity(Entity entity);
        Entity UpdateEntity(int id, Entity entity);
        void DeleteEntity(int id);

        #endregion

        #region Area

        List<Area> GetAreas();
        Area GetArea(int id);
        Area SaveArea(Area area);
        Area UpdateArea(int id, Area area);
        void DeleteArea(int id);

        List<Area> GetAreasByEntity(int entityId);

        #endregion

        #region HistoricDownloadDocument

        #region Generic
        List<HistoricDownloadDocument> GetHistoricDownloadDocuments();
        HistoricDownloadDocument SaveHistoricDownloadDocument(HistoricDownloadDocument historicDownloadDocument);
        #endregion

        List<HistoricDownloadDocument> GetHistoricDownloadDocumentsByDocument(int documentId);
        List<HistoricDownloadDocument> GetHistoricDownloadDocumentsByUser(int userId);

        #endregion

        #region Company

        #region Generic
        Company GetCompany(int id);
        List<Company> GetCompanies();
        Company SaveCompany(Company company);
        Company UpdateCompany(int id, Company company);
        bool DeleteCompany(int id);
        #endregion

        List<Company> GetCompaniesByAgency(int companyId);
        List<Company> GetCompaniesByUser(int userId);

        #endregion

        #region Cnae

        List<Cnae> GetCnaes();
        Cnae GetCnae(int id);

        #endregion

        #region Agency

        List<Agency> GetAgencies();        
        Agency GetAgency(int id);
        Agency SaveAgency(Agency agency, int companyId);
        Agency UpdateAgency(int id, Agency agency);
        bool DeleteAgency(int id);

        #endregion

        #region ContactPerson
        List<ContactPerson> GetContactPersons();
        ContactPerson GetContactPersonById(int contactPersonId);
        ContactPerson SaveContactPerson(ContactPerson contactPerson);
        bool DeleteContactPerson(ContactPerson contactPerson);
        #endregion

        #region Employee
        List<Employee> GetEmployeesByCompany(int companyId);
        Employee SaveEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        #endregion

        #region Payment Method

        PaymentMethod GetPaymentMethod(int paymentMethodId);
        PaymentMethod SavePaymentMethod(PaymentMethod paymentMethod);
        PaymentMethod UpdatePaymentMethod(int paymentMethodId, PaymentMethod paymentMethod);
        bool DeletePaymentMethod(int paymentMethodId);

        #endregion

        #region StretchEmployee

        StretchEmployee GetStretchEmployeeByNumberEmployees(int numberEmployees);
        StretchCalculate GetStretchCalculateByNumberEmployees(int numberEmployees);

        #endregion

        #region Mode Payment

        List<ModePayment> GetModesPayment();
        ModePayment GetModePayment(int id);

        #endregion

        #region Mode Payment Medical Examination

        List<ModePaymentMedicalExamination> GetModesPaymentMedicalExamination();
        ModePaymentMedicalExamination GetModePaymentMedicalExamination(int id);

        #endregion

        #region Contractual Document Company

        ContractualDocumentCompany GetContractualDocument(int contractualDocumentId);
        List<ContractualDocumentCompany> GetContractualsDocuments(int? companyId = null);
        ContractualDocumentCompany SaveContractualDocument(ContractualDocumentCompany contractualDocument);
        ContractualDocumentCompany UpdateContractualDocument(int contractualDocumentId, ContractualDocumentCompany contractualDocument);
        bool DeleteContractualDocument(int contractualDocumentId);

        #endregion

        #region Simulation

        #region Generic
        Simulation GetSimulation(int id);
        List<Simulation> GetSimulations();
        Simulation SaveSimulation(Simulation simulation);
        Simulation UpdateSimulation(int id, Simulation simulation);
        bool DeleteSimulation(int id);
        bool SubscribeSimulation(int id, bool subscribe);
        #endregion

        List<Simulation> GetSimulationByUser(int userId);

        #endregion

        #region Foreign Prevention Service

        ForeignPreventionService GetForeignPreventionService(int id);
        ForeignPreventionService SaveForeignPreventionService(ForeignPreventionService foreignPreventionService);

        #endregion

        #region Agency Service

        AgencyService GetAgencyService(int id);
        AgencyService SaveAgencyService(AgencyService agencyService);

        #endregion

        #region Training Service

        TrainingService GetTrainingService(int id);
        TrainingService SaveTrainingService(TrainingService trainingService);

        #endregion

        #region Simulation Company
        SimulationCompany GetSimulationCompany(int simulationId, int? companyId = null);
        SimulationCompany SaveSimulationCompany(int simulationId, int? companyId = null);
        SimulationCompany UpdateSimulationCompany(int simulationId, int companyId);
        bool DeleteSimulationCompany(int simulationId, int companyId);
        List<SimulationCompany> GetSimulationsCompanyByCompany(int companyId);
        #endregion

        #region Notification

        List<Notification> GetNotifications();
        Notification GetNotification(int id);
        Notification SaveNotification(Notification notification);
        Notification UpdateNotification(int id, Notification notification);
        bool DeleteNotification(int id);

        List<Notification> GetNotificationsByUserId(int userId);
        List<Notification> GetNotificationsByRoleId(int roleId);

        int GetNumberNotificationsByUserId(int userId);

        #endregion

        #region Notification Type

        List<NotificationType> GetNotificationTypes();
        NotificationType GetNotificationType(int id);

        #endregion

        #region TrainingCourse

        List<TrainingCourse> GetAllTrainingCourses();
        TrainingCourse GetTrainingCourse(int id);
        TrainingCourse SaveTrainingCourse(TrainingCourse trainingCourse);
        TrainingCourse UpdateTrainingCourse(int id, TrainingCourse trainingCourse);
        bool DeleteTrainingCourse(int id);

        List<TrainingCourse> GetTrainingCourses(int? trainingCourse);
        #endregion

        #region TrainingCourseTrainingService

        TrainingCourseTrainingService GetTrainingCourseTrainingService(int trainingCourseTrainingService);
        List<TrainingCourseTrainingService> GetTrainingCoursesTrainingServiceByTrainingService(int trainingServiceId);
        List<TrainingCourseTrainingService> GetTrainingCourseTrainingServicesByTrainingCourse(int trainingCourseId);
        List<TrainingCourse> GetTrainingCoursesByTrainingService(int trainingServiceId);
        List<TrainingService> GetTrainingServicesByTrainingCourse(int trainingCourseId);
        TrainingCourseTrainingService SaveTrainingCourseTrainingService(TrainingCourseTrainingService trainingCourseTrainingService);
        bool DeleteTrainingCourseTrainingService(int id);
        TrainingCourse FindTrainingCourse(string text);

        #endregion

        #region WorkCenter

        List<WorkCenter> GetWorkCenters();
        WorkCenter GetWorkCenter(int id);
        WorkCenter SaveWorkCenter(WorkCenter workCenter);
        bool DeleteWorkCenter(int id);
        WorkCenter SubscribeWorkCenter(int id, bool subscribe);

        #endregion

        #region WorkCenterCompany

        List<WorkCenter> GetWorkCentersByCompany(int companyId);
        WorkCenterCompany GetWorkCenterCompanyByWorkCenter(int workCenterId);
        WorkCenterCompany SaveWorkCenterCompany(WorkCenterCompany workCenterCompany);
        bool DeleteWorkCenterCompany(int workCenterId);

        #endregion

        #region EstablishmentType

        List<EstablishmentType> GetEstablishmentTypes();

        #endregion

        #region Configuration

        string GetTagValue(string tag);

        #endregion

        #region StretchAgency

        StretchAgency GetStretchAgencyByNumberEmployees(int numberEmployees);

        #endregion

        #region EngagementType

        List<EngagementType> GetEngagmentTypes();
        EngagementType GetEngagementType(int id);

        #endregion

        #region ContractualDocumentType

        List<ContractualDocumentType> GetContractualDocumentTypes();
        ContractualDocumentType GetContractualDocumentType(int id);

        #endregion

        #region RequestMedicalExamination State

        List<RequestMedicalExaminationState> GetRequestMedicalExaminationStates();
        RequestMedicalExaminationState GetRequestMedicalExaminationState(int id);

        #endregion

        #region RequestMedicalExamination Employee

        RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeById(int id);
        List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployees();
        List<Employee> GetEmployeesByRequestMedicalExamination(int requestMedicalExaminationId);        
        RequestMedicalExaminationEmployee SaveRequestMedicalExaminationEmployee(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee);
        bool DeleteRequestMedicalExaminationEmployee(int id);

        #endregion

        #region RequestMedicalExaminations

        RequestMedicalExaminations GetRequestMedicalExaminationById(int id);
        List<RequestMedicalExaminations> GetRequestMedicalExaminations();
        List<RequestMedicalExaminations> GetRequestMedicalExaminationsByCompany(int companyId);
        RequestMedicalExaminations SaveRequestMedicalExaminations(RequestMedicalExaminations requestMedicalExamination);
        bool DeleteRequestMedicalExamination(int id);

        #endregion

        #region RequestMedicalExaminationEmployee State

        List<RequestMedicalExaminationEmployeeState> GetRequestMedicalExaminationEmployeeStates();
        RequestMedicalExaminationEmployeeState GetRequestMedicalExaminationEmployeeState(int id);

        #endregion
    }
}
