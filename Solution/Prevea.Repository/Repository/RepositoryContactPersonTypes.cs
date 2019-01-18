namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Prevea.Model.Model;
    using System.Data.Entity;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public List<ContactPersonType> GetContactPersonTypes()
        {
            return Context.ContactPersonTypes
                .Include(x => x.ContactPersons)
                .ToList();
        }

        public ContactPersonType GetContactPersonTypeById(int contactPersonId)
        {
            return Context.ContactPersonTypes
                .Include(x => x.ContactPersons)
                .FirstOrDefault(x => x.Id == contactPersonId);
        }

        public List<ContactPerson> GetContactPersonByCompanyAndContactPersonType(int companyId, int contactPersonType)
        {
            return Context.ContactPersons
                .Where(x => x.CompanyId == companyId && x.ContactPersonTypeId == contactPersonType)
                .Include(x => x.ContactPersonType)
                .Include(x => x.Company)
                .Include(x => x.User)
                .ToList();
        }

        public List<ContactPersonType> GetContactPersonRemainingByCompany(int companyId)
        {
            var contactPersons = GetContactPersons().Where(x => x.CompanyId == companyId);
            var hasLegalRepresentative = false;
            var hasContactPerson = false;
            foreach (var contactPerson in contactPersons)
            {
                if (contactPerson.ContactPersonTypeId == (int) EnContactPersonType.LegalRepresentative)
                    hasLegalRepresentative = true;
                if (contactPerson.ContactPersonTypeId == (int)EnContactPersonType.ContactPerson)
                    hasContactPerson = true;
            }
            var contactPersonTypes = new List<ContactPersonType>();
            if (!hasLegalRepresentative)
                contactPersonTypes.Add(GetContactPersonTypeById((int)EnContactPersonType.LegalRepresentative));
            if (!hasContactPerson)
                contactPersonTypes.Add(GetContactPersonTypeById((int)EnContactPersonType.ContactPerson));

            contactPersonTypes.Add(GetContactPersonTypeById((int)EnContactPersonType.Invited));

            return contactPersonTypes;
        }
    }
}
