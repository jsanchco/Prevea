namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public List<EstablishmentType> GetEstablishmentTypes()
        {
            return Context.EstablishmentTypes
                .ToList();
        }
    }
}
