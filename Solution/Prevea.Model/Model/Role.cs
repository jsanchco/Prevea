namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    public class Role
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnRole), Id));

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
        
    public enum EnRole { Super = 1, Admin, Library, ContactPerson, Employee, Agency, Doctor, Manager, PreveaPersonal, PreveaCommercial, ExternalPersonal }
}
