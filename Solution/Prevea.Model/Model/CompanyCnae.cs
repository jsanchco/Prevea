namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class CompanyCnae
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public int CnaeId { get; set; }
        public virtual Cnae Cnae { get; set; }
    }
}
