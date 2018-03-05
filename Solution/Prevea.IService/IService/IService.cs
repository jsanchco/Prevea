namespace Prevea.IService.IService
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Web;
    using Model.CustomModel;
    using System;

    #endregion

    public interface IService : IDisposable
    {
        #region Document

        Result SaveDocument(int userCreatorId, int? userOwnerId, Document document);
        Result SaveDocumentWithParent(int userCreatorId, Document document);
        Result UpdateDocument(Document document, bool updateFile);
        Result UnsubscribeDocument(int documentId);
        Result SubscribeDocument(int documentId);
        Result DeleteDocument(int documentId);
        List<Document> GetDocuments(int documentStateId = 0);
        List<Document> GetDocumentsByParent(int id, int? parentId);
        Document GetDocument(int id);

        void RestoreFile(int userId, string urlRelative);
        void SaveFileTmp(int userId, HttpPostedFileBase files);

        #endregion

        #region Area

        List<Area> GetAreasByEntity(int entityId);

        #endregion

        #region User
            
        List<User> GetUsersInRoles(List<string> roles);
        User ValidateUser(string user, string password);
        List<string> GetRolesByUser(int userId);
        User GetUser(int userId);
        List<User> GetUsers();        
        ContactPerson GetContactPersonById(int contactPersonId);
        ContactPerson GetContactPersonByUserId(int userId);
        List<User> GetContactPersonsByCompany(int companyId);
        List<Employee> GetEmployeesByCompany(int companyId);
        List<User> GetUsersByUser(int id);
        Result SaveUser(int? roleId, User user);
        Result SaveContactPersonCompany(int roleId, int companyId, User user);
        Result SaveEmployeeCompany(int roleId, int companyId, User user);
        Result DeleteContactPersonCompany(int companyId, int userId);
        Result DeleteEmployeeCompany(int companyId, int userId);
        Result SubscribeContactPersonCompany(int companyId, int userId, bool subscribe);
        Result SubscribeEmployeeCompany(int companyId, int userId, bool subscribe);
        Result DeleteUser(int userId);
        Result SubscribeUser(int userId, bool subscribe);
        List<CustomRole> GetCustomRoles(List<int> listRoles);

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
        Company GetCompany(int companyId);
        List<Company> GetCompanies();
        Result SaveCompany(Company company);
        Result UpdateCompany(int companyId, Company company);
        bool DeleteCompany(int companyId);
        #endregion

        Result SubscribeCompany(int companyId, bool subscribe);
        List<Company> GetCompaniesByUser(int userId);

        #endregion

        #region Cnae

        List<Cnae> GetCnaes();
        Cnae GetCnae(int cnaeId);

        #endregion

        #region Agency

        #region Generic
        Agency GetAgency(int agencyId);
        List<Agency> GetAgencies();
        Result SaveAgency(Agency agency, int companyId);
        Result UpdateAgency(Agency agency, int agencyId);
        Result DeleteAgency(int agencyId);
        #endregion

        #endregion

        #region Payment Method

        PaymentMethod GetPaymentMethod(int paymentMethodId);
        Result SavePaymentMethod(PaymentMethod paymentMethod);
        Result UpdatePaymentMethod(int paymentMethodId, PaymentMethod paymentMethod);
        Result DeletePaymentMethod(int paymentMethodId);

        #endregion

        #region StretchEmployee

        StretchCalculate GetStretchCalculateByNumberEmployees(int numberEmployees);

        #endregion

        #region Mode Payment

        List<ModePayment> GetModesPayment();
        ModePayment GetModePayment(int modePaymentId);

        #endregion

        #region Mode Payment Medical Examination

        List<ModePaymentMedicalExamination> GetModesPaymentMedicalExamination();
        ModePaymentMedicalExamination GetModePaymentMedicalExamination(int id);

        #endregion

        #region Contractual Document Company

        ContractualDocumentCompany GetContractualDocument(int contractualDocumentId);
        List<ContractualDocumentCompany> GetContractualsDocuments(int? companyId = null);        
        List<ContractualDocumentCompany> GetChildrenContractualsDocuments(int contractualDocumentParentId);
        Result SaveContractualDocument(ContractualDocumentCompany contractualDocument);
        Result UpdateContractualDocument(int contractualDocumentId, ContractualDocumentCompany contractualDocument);
        bool DeleteContractualDocument(int contractualDocumentId);
        string CanAddContractualDocument(int companyId, int contractualDocumentTypeId);
        string VerifyNewContractualDocument(int companyId, int contractualDocumentTypeId);
        Result SaveContractualDocumentFirmed(HttpPostedFileBase fileDocumentFirmed, int companyId, int contractualDocumentId);
        Result SaveOtherDocument(HttpPostedFileBase fileOtherDocument, int contractualDocumentId);
        Result DeleteContractualDocumentCompanyFirmed(int contractualDocumentCompanyFirmedId);

        #endregion

        #region Simulation

        #region Generic
        Simulation GetSimulation(int simulationId);
        List<Simulation> GetSimulations();
        Result SaveSimulation(Simulation simulation, int? companyId = null);
        Result UpdateSimulation(int simulationId, Simulation simulation);
        Result DeleteSimulation(int simulationId);
        Result SubscribeSimulation(int simulationId, bool subscribe);
        #endregion

        List<Simulation> GetSimulationsByUser(int userId);
        List<Simulation> GetSimulationsOriginalsByUser(int userId);
        List<Simulation> GetSimulationsChildrenByUser(int userId, int simulationParentId);
        List<Simulation> GetSimulationsByCompany(int companyId);
        Result SendToCompanies(int simulationId);
        decimal GetTotalSimulation(int simulationId);
        Simulation GetSimulationActive(int companyId);

        #endregion

        #region Foreign Prevention Service

        ForeignPreventionService GetForeignPreventionService(int foreignPreventionServiceId);
        Result SaveForeignPreventionService(ForeignPreventionService foreignPreventionService);

        #endregion

        #region Agency Service

        AgencyService GetAgencyService(int agencyServiceId);
        Result SaveAgencyService(AgencyService agencyService);

        #endregion

        #region Training Service

        TrainingService GetTrainingService(int trainingServiceId);
        Result SaveTrainingService(TrainingService trainingService);

        #endregion

        #region Notification

        List<Notification> GetNotifications();
        Notification GetNotification(int notificationId);
        Result SaveNotification(Notification notification);
        Result UpdateNotification(int notificationId, Notification notification);
        Result DeleteNotification(int notificationId);

        List<Notification> GetNotificationsByUserId(int userId);
        List<Notification> GetNotificationsByRoleId(int roleId);
        int GetNumberNotificationsByUserId(int userId);

        #endregion

        #region Notification Type

        List<NotificationType> GetNotificationTypes();
        NotificationType GetNotificationType(int notificationTypeId);

        #endregion

        #region TrainingCourse

        List<TrainingCourse> GetAllTrainingCourses();
        TrainingCourse GetTrainingCourse(int id);
        Result SaveTrainingCourse(TrainingCourse trainingCourse);
        Result UpdateTrainingCourse(int id, TrainingCourse trainingCourse);
        Result DeleteTrainingCourse(int id);

        List<TrainingCourse> GetTrainingCourses(int? trainingCourse);
        #endregion

        #region TrainingCourseTrainingService

        TrainingCourseTrainingService GetTrainingCourseTrainingService(int trainingCourseTrainingService);
        List<TrainingCourseTrainingService> GetTrainingCoursesTrainingServiceByTrainingService(int trainingServiceId);
        List<TrainingCourseTrainingService> GetTrainingCourseTrainingServicesByTrainingCourse(int trainingCourseId);
        List<TrainingCourse> GetTrainingCoursesByTrainingService(int trainingServiceId);
        List<TrainingService> GetTrainingServicesByTrainingCourse(int trainingCourseId);
        Result SaveTrainingCourseTrainingService(TrainingCourseTrainingService trainingCourseTrainingService);
        Result DeleteTrainingCourseTrainingService(int id);
        TrainingCourse FindTrainingCourse(string text);

        #endregion

        #region WorkCenter

        List<WorkCenter> GetWorkCenters();
        WorkCenter GetWorkCenter(int id);
        Result SaveWorkCenter(WorkCenter workCenter);
        Result DeleteWorkCenter(int id);
        Result SubscribeWorkCenter(int id, bool subscribe);

        #endregion

        #region WorkCenterCompany

        List<WorkCenter> GetWorkCentersByCompany(int companyId);
        Result SaveWorkCenterCompany(int companyId, WorkCenter workCenter);
        Result DeleteWorkCenterCompany(int workCenterId);

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
        List<ContractualDocumentType> GetContractualDocumentTypesByParent(int contractualParentId);
        ContractualDocumentType GetContractualDocumentType(int id);
        List<ContractualDocumentType> GetContractualDocumentTypes(int companyId);
        List<ContractualDocumentType> GetContractualDocumentTypesBySimulation(int companyId, int simulationId);

        #endregion

        #region RequestMedicalExamination State

        List<RequestMedicalExaminationState> GetRequestMedicalExaminationStates();
        RequestMedicalExaminationState GetRequestMedicalExaminationState(int id);

        #endregion

        #region RequestMedicalExamination Employee

        RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeById(int id);
        List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployees();
        List<Employee> GetEmployeesByRequestMedicalExamination(int requestMedicalExaminationId);        
        Result SaveRequestMedicalExaminationEmployee(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee);
        Result DeleteRequestMedicalExaminationEmployee(int id);

        #endregion

        #region RequestMedicalExaminations

        RequestMedicalExaminations GetRequestMedicalExaminationById(int id);
        List<RequestMedicalExaminations> GetRequestMedicalExaminations();
        List<RequestMedicalExaminations> GetRequestMedicalExaminationsByCompany(int companyId);
        Result SaveRequestMedicalExaminations(RequestMedicalExaminations requestMedicalExamination);
        Result DeleteRequestMedicalExamination(int id);

        #endregion

        #region RequestMedicalExaminationEmployee State

        List<RequestMedicalExaminationEmployeeState> GetRequestMedicalExaminationEmployeeStates();
        RequestMedicalExaminationEmployeeState GetRequestMedicalExaminationEmployeeState(int id);

        #endregion
    }
}
