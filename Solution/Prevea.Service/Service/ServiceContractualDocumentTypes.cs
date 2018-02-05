namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public List<ContractualDocumentType> GetContractualDocumentTypes()
        {
            return Repository.GetContractualDocumentTypes()
                .Where(x => x.Id != (int)EnContractualDocumentType.Firmed)
                .ToList();
        }

        public ContractualDocumentType GetContractualDocumentType(int id)
        {
            return Repository.GetContractualDocumentType(id);
        }
    }
}
