namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<Area> GetAreasByEntity(int entityId)
        {
            return Repository.GetAreasByEntity(entityId);
        }
    }
}
