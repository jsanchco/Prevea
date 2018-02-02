namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public List<ContractualDocumentType> GetContractualDocumentTypes()
        {
            return Context.ContractualDocumentTypes
                .Include(x => x.ContractualsDocumentsCompany)
                .ToList();
        }

        public ContractualDocumentType GetContractualDocumentType(int id)
        {
            return Context.ContractualDocumentTypes
                .Include(x => x.ContractualsDocumentsCompany)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}