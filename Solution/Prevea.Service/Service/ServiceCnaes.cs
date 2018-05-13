namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<Cnae> GetCnaes()
        {
            return Repository.GetCnaes();
        }

        public List<WorkStation> GetWorkStationsByCnaeId(int cnaeId)
        {
            return Repository.GetWorkStationsByCnaeId(cnaeId);
        }

        public Cnae GetCnae(int id)
        {
            return Repository.GetCnae(id);
        }
    }
}
