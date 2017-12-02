namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class CompanyState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
    }

    public enum EnCompanyState { NotMapped, Alta, BajaPorAdmin, Borrado }
}
