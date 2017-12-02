namespace Prevea.IRepository.IRepository
{
    #region Using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.Model;

    #endregion

    public interface IRepository
    {
        #region Users

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
        List<User> GetEmployeesByCompany(int companyId);

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
        ContactPerson SaveContactPerson(ContactPerson contactPerson);
        bool DeleteContactPerson(ContactPerson contactPerson);
        #endregion

        #region Employee
        Employee SaveEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        #endregion

        #region Economic Data

        EconomicData GetEconomicData(int economicDataId);
        EconomicData SaveEconomicData(EconomicData economicData);
        EconomicData UpdateEconomicData(int economicDataId, EconomicData economicData);
        bool DeleteEconomicData(int economicDataId);

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
        bool DeleteContractualDocument(int contractualDocumentId);

        #endregion

        #region Simulator

        #region Generic
        Simulator GetSimulator(int id);
        List<Simulator> GetSimulators();
        Simulator SaveSimulator(Simulator simulator);
        Simulator UpdateSimulator(int id, Simulator simulator);
        bool DeleteSimulator(int id);
        #endregion

        List<Simulator> GetSimulatorsByUser(int userId);

        #endregion

        #region SimulatorCompany
        SimulatorCompany GetSimulatorCompany(int simulatorId, int? companyId = null);
        SimulatorCompany SaveSimulatorCompany(int simulatorId, int? companyId = null);
        SimulatorCompany UpdateSimulatorCompany(int simulatorId, int companyId);
        bool DeleteSimulatorCompany(int simulatorId, int companyId);
        #endregion
    }
}
