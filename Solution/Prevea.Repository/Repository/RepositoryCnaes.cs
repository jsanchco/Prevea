namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public List<Cnae> GetCnaes()
        {
            return Context.Cnaes
                .ToList();
        }

        public Cnae GetCnae(int id)
        {
            return Context.Cnaes
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
