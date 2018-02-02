namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<ContractualDocumentType> GetContractualDocumentTypes()
        {
            return Repository.GetContractualDocumentTypes();
        }

        public ContractualDocumentType GetContractualDocumentType(int id)
        {
            return Repository.GetContractualDocumentType(id);
        }
    }
}
