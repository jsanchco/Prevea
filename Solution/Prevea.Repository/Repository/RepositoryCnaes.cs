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
        public List<Cnae> GetCnaes()
        {
            return Context.Cnaes
                .Include(x => x.WorkStations)
                .ToList();
        }

        public Cnae GetCnae(int id)
        {
            return Context.Cnaes
                .Include(x => x.WorkStations)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
