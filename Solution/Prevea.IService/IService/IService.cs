namespace Prevea.IService.IService
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Web;

    #endregion

    public interface IService
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
        List<User> GetContactPersonsByCompany(int companyId);
        List<User> GetEmployeesByCompany(int companyId);
        Result SaveUser(int? roleId, User user);
        Result SaveContactPersonCompany(int roleId, int companyId, User user);
        Result SaveEmployeeCompany(int roleId, int companyId, User user);
        Result DeleteContactPersonCompany(int companyId, int userId);
        Result DeleteEmployeeCompany(int companyId, int userId);
        Result SubscribeContactPersonCompany(int companyId, int userId, bool subscribe);
        Result SubscribeEmployeeCompany(int companyId, int userId, bool subscribe);
        Result DeleteUser(int userId);
        Result SubscribeUser(int userId, bool subscribe);

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

        #region Economic Data

        EconomicData GetEconomicData(int economicDataId);
        Result SaveEconomicData(EconomicData economicData);
        Result UpdateEconomicData(int economicDataId, EconomicData economicData);
        Result DeleteEconomicData(int economicDataId);

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
        Result SaveContractualDocument(ContractualDocumentCompany contractualDocument);
        bool DeleteContractualDocument(int contractualDocumentId);

        #endregion

        #region Simulator

        #region Generic
        Simulator GetSimulator(int simulatorId);
        List<Simulator> GetSimulators();
        Result SaveSimulator(Simulator simulator);
        Result UpdateSimulator(int simulatorId, Simulator simulator);
        Result DeleteSimulator(int simulatorId);
        #endregion

        List<Simulator> GetSimulatorsByUser(int userId);
        Result SendToCompanies(int simulatorId);

        #endregion

        #region Notification

        List<Notification> GetNotifications();
        Notification GetNotification(int notificationId);
        Result SaveNotification(Notification notification);
        Result UpdateNotification(int notificationId, Notification notification);
        Result DeleteNotification(int notificationId);

        List<Notification> GetNotificationsByUserId(int userId);
        List<Notification> GetNotificationsByRoleId(int roleId);

        #endregion

        #region Notification Type

        List<NotificationType> GetNotificationTypes();
        NotificationType GetNotificationType(int notificationTypeId);

        #endregion
    }
}
