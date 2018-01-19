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
        public List<EngagementType> GetEngagmentTypes()
        {
            return Context.EngagementTypes
                .Include(x => x.AgencyServices)
                .ToList();
        }

        public EngagementType GetEngagementType(int id)
        {
            return Context.EngagementTypes
                .Include(x => x.AgencyServices)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
