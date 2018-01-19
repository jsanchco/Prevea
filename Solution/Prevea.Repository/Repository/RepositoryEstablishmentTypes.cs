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
        public List<EstablishmentType> GetEstablishmentTypes()
        {
            return Context.EstablishmentTypes
                .Include(x => x.WorkCenters)
                .ToList();
        }
    }
}
