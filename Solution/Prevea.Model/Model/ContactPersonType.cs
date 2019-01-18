namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ContactPersonType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnContactPersonType), Id));

        public virtual ICollection<ContactPerson> ContactPersons { get; set; }
    }

    public enum EnContactPersonType { LegalRepresentative = 1, ContactPerson, Invited }
}
