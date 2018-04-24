namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public List<Area> GetContractualDocumentTypes()
        {
            return Repository.GetAreas()
                .Where(x => x.EntityId == 2)
                .ToList();
        }

        public List<Area> GetContractualDocumentTypes(int companyId)
        {
            var listContractualDocumentTypes = new List<Area>();
            var contractualsDocumentsByCompany = GetDocumentsContractualsByCompany(companyId);
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
                            x => x.AreaId == 6);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetArea(6));
                    }
                    else
                    {
                        hasContractSPA = true;
                    }
                }
                if (company.SimulationCompanyActive.Simulation.TrainingService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.AreaId == 7);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetArea(7));
                    }
                    else
                    {
                        hasContractFOR = true;
                    }
                }
                if (company.SimulationCompanyActive.Simulation.AgencyService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.AreaId == 8);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetArea(8));
                    }
                    else
                    {
                        hasContractGES = true;
                    }
                }
            }
            if (hasContractSPA)
            {
                var hasDocument =
                    contractualsDocumentsByCompany.FirstOrDefault(
                        x => x.AreaId == 9);
                if (hasDocument == null)
                {
                    listContractualDocumentTypes.Add(Repository.GetArea(9));
                }
            }
            if (hasContractFOR)
            {
                var hasDocument =
                    contractualsDocumentsByCompany.FirstOrDefault(
                        x => x.AreaId == 10);
                if (hasDocument == null)
                {
                    listContractualDocumentTypes.Add(Repository.GetArea(10));
                }
            }
            if (hasContractGES)
            {
                var hasDocument =
                    contractualsDocumentsByCompany.FirstOrDefault(
                        x => x.AreaId == 11);
                if (hasDocument == null)
                {
                    listContractualDocumentTypes.Add(Repository.GetArea(11));
                }                
            }    

            listContractualDocumentTypes.Add(Repository.GetArea(13));
            return listContractualDocumentTypes;
        }

        public List<Area> GetContractualDocumentTypesBySimulation(int companyId, int simulationId)
        {
            var listContractualDocumentTypes = new List<Area>();
            var contractualsDocumentsByCompany = GetDocumentsContractualsByCompany(companyId).Where(x => x.SimulationId == simulationId).ToList();
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
                            x => x.AreaId == 6);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetArea(6));
                    }
                    else
                    {
                        hasContractSPA = true;
                    }
                }
                if (company.SimulationCompanyActive.Simulation.TrainingService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.AreaId == 7);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetArea(7));
                    }
                    else
                    {
                        hasContractFOR = true;
                    }
                }
                if (company.SimulationCompanyActive.Simulation.AgencyService != null)
                {
                    var hasDocument =
                        contractualsDocumentsByCompany.FirstOrDefault(
                            x => x.AreaId == 8);
                    if (hasDocument == null)
                    {
                        listContractualDocumentTypes.Add(Repository.GetArea(8));
                    }
                    else
                    {
                        hasContractGES = true;
                    }
                }
            }
            if (hasContractSPA)
            {
                var hasDocument =
                    contractualsDocumentsByCompany.FirstOrDefault(
                        x => x.AreaId == 9);
                if (hasDocument == null)
                {
                    listContractualDocumentTypes.Add(Repository.GetArea(9));
                }
            }
            if (hasContractFOR)
            {
                var hasDocument =
                    contractualsDocumentsByCompany.FirstOrDefault(
                        x => x.AreaId == 10);
                if (hasDocument == null)
                {
                    listContractualDocumentTypes.Add(Repository.GetArea(10));
                }
            }
            if (hasContractGES)
            {
                var hasDocument =
                    contractualsDocumentsByCompany.FirstOrDefault(
                        x => x.AreaId == 11);
                if (hasDocument == null)
                {
                    listContractualDocumentTypes.Add(Repository.GetArea(11));
                }
            }

            listContractualDocumentTypes.Add(Repository.GetArea(13));
            return listContractualDocumentTypes;
        }
    }
}
