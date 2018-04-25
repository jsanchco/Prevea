namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public Area GetArea(int id)
        {
            return Repository.GetArea(id);
        }

        public List<Area> GetAreasByEntity(int entityId)
        {
            return Repository.GetAreasByEntity(entityId);
        }

        public Area GetAreaByName(string name)
        {
            return Repository.GetAreaByName(name);
        }

        public List<Area> GetAreasByCompanyAndSimulation(int companyId, int simulationId)
        {
            return Repository.GetAreasByCompanyAndSimulation(companyId, simulationId);
        }
    }
}
