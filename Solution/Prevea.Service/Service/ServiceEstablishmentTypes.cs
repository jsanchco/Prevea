﻿namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<EstablishmentType> GetEstablishmentTypes()
        {
            return Repository.GetEstablishmentTypes();
        }
    }
}