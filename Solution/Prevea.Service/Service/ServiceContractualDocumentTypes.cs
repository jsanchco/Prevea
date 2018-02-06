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

        public List<ContractualDocumentType> GetContractualDocumentTypesByParent(int contractualParentId)
        {
            var contractualParent = GetContractualDocument(contractualParentId);

            var listContractualDocumentTypes = new List<ContractualDocumentType>();
            switch (contractualParent.ContractualDocumentTypeId)
            {
                case (int)EnContractualDocumentType.ContractSPA:
                    listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(7));
                    listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(8));
                    break;
                case (int)EnContractualDocumentType.ContractGES:
                    listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(7));
                    listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(8));
                    break;
            }

            return listContractualDocumentTypes;
        }

        public ContractualDocumentType GetContractualDocumentType(int id)
        {
            return Repository.GetContractualDocumentType(id);
        }
    }
}
