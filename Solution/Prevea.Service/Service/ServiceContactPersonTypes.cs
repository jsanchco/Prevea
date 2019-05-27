namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Prevea.Model.Model;

    #endregion

    public partial class Service
    {
        public List<ContactPersonType> GetContactPersonTypes()
        {
            return Repository.GetContactPersonTypes();
        }

        public ContactPersonType GetContactPersonTypeById(int contactPersonId)
        {
            return Repository.GetContactPersonTypeById(contactPersonId);
        }

        public List<ContactPerson> GetContactPersonByCompanyAndContactPersonType(int companyId, int contactPersonType)
        {
            return Repository.GetContactPersonByCompanyAndContactPersonType(companyId, contactPersonType);
        }

        public List<ContactPersonType> GetContactPersonTypesRemainingByCompany(int companyId)
        {
            return Repository.GetContactPersonRemainingByCompany(companyId);
        }
    }
}
