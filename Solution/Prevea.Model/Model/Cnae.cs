namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion

    public class Cnae
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string CustomKey { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Company> Companies{ get; set; }
    }
}
