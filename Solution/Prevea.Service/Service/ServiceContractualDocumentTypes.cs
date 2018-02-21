using System.Web.UI.WebControls;

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

        public List<ContractualDocumentType> GetContractualDocumentTypes(int companyId)
        {
            var listContractualDocumentTypes = new List<ContractualDocumentType>();
            var contractualsDocumentsByCompany = GetContractualsDocuments(companyId);
            var company = GetCompany(companyId);
            var hasContractSPA = false;
            var hasContractGES = false;
            var hasContractFOR = false;
            if (company.SimulationCompanyActive.Simulation.Original)
            {
                if (company.SimulationCompanyActive.Simulation.ForeignPreventionService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.ContractualDocumentTypeId == (int) EnContractualDocumentType.OfferSPA);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(1));
                    }
                    else
                    {
                        hasContractSPA = true;
                    }
                }
                if (company.SimulationCompanyActive.Simulation.AgencyService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.OfferGES);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(2));
                    }
                    else
                    {
                        hasContractGES = true;
                    }
                }
                if (company.SimulationCompanyActive.Simulation.TrainingService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.OfferFOR);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(3));
                    }
                    else
                    {
                        hasContractFOR = true;
                    }
                }
            }
            if (hasContractSPA)
            {
                listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(4));
            }
            if (hasContractGES)
            {
                listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(5));
            }
            if (hasContractFOR)
            {
                listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(6));
            }

            listContractualDocumentTypes.Add(Repository.GetContractualDocumentType(8));
            return listContractualDocumentTypes;
        }
    }
}
