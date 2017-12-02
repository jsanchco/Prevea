namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class Role
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
       
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
        
    public enum EnRole { NotMapped, Super, Admin, Library, ContactPerson, Employee, Agency, Doctor, Manager, PreveaPersonal, PreveaCommercial, ExternalPersonal }
}
