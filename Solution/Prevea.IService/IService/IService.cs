namespace Prevea.IService.IService
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Web;
    using Model.CustomModel;
    using System;
    using Model.ViewModel;

    #endregion

    public interface IService : IDisposable
    {
        #region Document

        Result SaveDocument(Document document, bool restoreFile, List<DocumentUserCreator> usersCreators, List<DocumentUserOwner> usersOwners, string extension = null);
        Result SaveDocumentWithParent(Document document);
        Result UpdateDocument(Document document, bool updateFile);
        Result UnsubscribeDocument(int documentId);
        Result SubscribeDocument(int documentId);
        Result DeleteDocument(int documentId);
        List<Document> GetDocuments(int documentStateId = 0);
        List<Document> GetDocumentsByParent(int id, int? parentId);
        List<Document> GetChildrenDocument(int parentId);
        Document GetDocument(int id);
        Document GetDocument(string name);
        List<Document> GetDocumentsContractualsByCompany(int? companyId);
        string VerifyNewContractualDocument(Document document);        
        Result SaveOtherDocument(HttpPostedFileBase fileOtherDocument, int documentId);
        Result SaveContractualDocumentFirmed(HttpPostedFileBase fileDocumentFirmed, int companyId, int documentId, int userId);
        Document GetDocumentByArea(int areaId);

        void RestoreFile(int userId, string urlRelative);
        void SaveFileTmp(int userId, HttpPostedFileBase files);

        #endregion

        #region Area

        Area GetArea(int id);
        List<Area> GetAreasByEntity(int entityId);
        Area GetAreaByName(string name);
        List<Area> GetAreasByCompanyAndSimulation(int companyId, int simulationId);        

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
        List<ContactPerson> GetContactPersons();
        List<Employee> GetEmployees();
        Employee GetEmployeeById(int id);
        Employee GetEmployeeByUser(int userId);
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
        List<User> GetUsersByUserFromContactAs(int id);
        User GetUserByEmployee(int employeeId);

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
        List<WorkStation> GetWorkStationsByCnaeId(int cnaeId);
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

        #region RequestMedicalExamination State

        List<RequestMedicalExaminationState> GetRequestMedicalExaminationStates();
        RequestMedicalExaminationState GetRequestMedicalExaminationState(int id);

        #endregion

        #region RequestMedicalExamination Employee

        RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeById(int id);
        List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployeeByEmployeeId(int employeeId);
        RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeByEmployeeId(int requestMedicalExaminationsId, int employeeId);
        List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployees();
        List<Employee> GetEmployeesByRequestMedicalExamination(int requestMedicalExaminationId);        
        Result SaveRequestMedicalExaminationEmployee(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee);
        Result DeleteRequestMedicalExaminationEmployee(int id);
        Result UpdateRequestHistoricMedicalExaminationEmployees(List<RequestMedicalExaminationEmployee> listEmployees, int userId);
        List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployeesByDate(int doctorId, DateTime date);
        string GenerateMedicalExaminationInputTemplatesJSON(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee);
        TemplateMedicalExaminationViewModel GetTemplateMedicalExaminationViewModel(
            int requestMedicalExaminationEmployeeId);
        RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeesByDocumentId(int documentId);

        Result SaveMedicalExamination(TemplateMedicalExaminationViewModel templateMedicalExaminationViewModel, int userId);

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
      
        #region Clinics

        List<Clinic> GetClincs();
        Clinic GetClinicById(int id);
        Result SaveClinic(Clinic clinic);
        Result DeleteClinic(int id);

        #endregion

        #region Doctors

        List<DoctorMedicalExaminationEmployee> GetDoctorsMedicalExaminationEmployees();
        DoctorMedicalExaminationEmployee GetDoctorMedicalExaminationEmployeeById(int id);
        DoctorMedicalExaminationEmployee GetDoctorMedicalExaminationEmployeeByDoctorId(int medicalExaminationEmployeeId, int doctorId);
        Result SaveDoctorMedicalExaminationEmployee(DoctorMedicalExaminationEmployee doctorMedicalExaminationEmployee);
        Result DeleteDoctorMedicalExaminationEmployee(int id);
        List<DateTime?> GetDatesByWorkSheet(int doctorId);
        int GetCountMedicalExaminationByState(int doctorId, DateTime date, EnDocumentState medicalExaminationState);

        #endregion

        #region MedicalExamination Documents

        List<MedicalExaminationDocuments> GetMedicalExaminationDocuments();
        MedicalExaminationDocuments GetMedicalExaminationDocumentById(int id);
        Result SaveMedicalExaminationDocument(int requestMedicalExaminationEmployeeId, Document document, int userId);
        Result DeleteMedicalExaminationDocument(int id, int requestMedicalExaminationEmployeeId);

        List<MedicalExaminationDocuments> GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(int requestMedicalExaminationEmployeeId);
        MedicalExaminationDocuments GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeIdAndAreaId(
            int requestMedicalExaminationEmployeeId, int areaId);
        Result SaveFileMedicalExaminationDocument(HttpPostedFileBase fileOtherDocument, int medicalExaminationDocumentId);

        #endregion

        #region Contractual Document Types

        List<Area> GetContractualDocumentTypes();
        List<Area> GetContractualDocumentTypes(int companyId);
        List<Area> GetContractualDocumentTypesBySimulation(int companyId, int simulationId);

        #endregion

        #region Medical Examination Types

        List<Area> GetMedicalExaminationDocumentTypes();

        #endregion

        #region Employees

        List<HeaderEmployeeDocuments> GetHeaderEmployeeDocuments(int employeeId);

        #endregion

        #region WorkStations

        List<WorkStation> GetWorkStations();
        WorkStation GetWorkStationById(int id);
        Result SaveWorkStation(WorkStation workStation);
        Result DeleteWorkStation(int id);

        #endregion

        #region DeltaCodes

        List<DeltaCode> GetDeltaCodes();
        DeltaCode GetDeltaCodeById(int id);
        Result SaveDeltaCode(DeltaCode deltaCode);
        Result DeleteDeltaCode(int id);

        #endregion

        #region RiskEvaluations

        List<RiskEvaluation> GetRiskEvaluations();
        List<RiskEvaluation> GetRiskEvaluationsByWorkStation(int workStationId);
        List<RiskEvaluation> GetRiskEvaluationsByDeltaCode(int deltaCodeId);
        RiskEvaluation GetRiskEvaluationById(int id);
        Result SaveRiskEvaluation(RiskEvaluation riskEvaluation);
        Result DeleteRiskEvaluation(int id);

        #endregion

        #region PreventivePlans

        List<PreventivePlan> GetPreventivePlans(int userId);
        PreventivePlan GetPreventivePlanById(int id);
        bool ExistPreventivePlan(int companyId, int documentId);
        Result SavePreventivePlan(PreventivePlan preventivePlan);
        Result DeletePreventivePlan(int id);

        #endregion

        #region TemplatePreventivePlan

        List<TemplatePreventivePlan> GetTemplatePreventivePlans();
        TemplatePreventivePlan GetTemplatePreventivePlanById(int id);
        Result SaveTemplatePreventivePlan(TemplatePreventivePlan templatePreventivePlan);
        Result DeleteTemplatePreventivePlan(int id);
        Dictionary<string, string> GetEditorSnippets(int preventivePlanId, int templateId);

        #endregion

        #region PreventivePlanTemplatePreventivePlan

        List<PreventivePlanTemplatePreventivePlan> GetPreventivePlanTemplatePreventivePlans();
        List<PreventivePlanTemplatePreventivePlan> GetPreventivePlanTemplatePreventivePlansByPreventivePlanId(int preventivePlanId);
        PreventivePlanTemplatePreventivePlan ExistPreventivePlanTemplatePreventivePlan(int preventivePlanId, int templatePreventivePlanId);
        PreventivePlanTemplatePreventivePlan GetPreventivePlanTemplatePreventivePlanById(int id);
        Result SavePreventivePlanTemplatePreventivePlan(PreventivePlanTemplatePreventivePlan preventivePlanTemplatePreventivePlan);
        Result DeletePreventivePlanTemplatePreventivePlan(int id);

        #endregion
    }
}
