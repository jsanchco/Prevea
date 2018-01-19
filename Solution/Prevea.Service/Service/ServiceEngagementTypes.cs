namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<EngagementType> GetEngagmentTypes()
        {
            return Repository.GetEngagmentTypes();
        }

        public EngagementType GetEngagementType(int id)
        {
            return Repository.GetEngagementType(id);
        }
    }
}
