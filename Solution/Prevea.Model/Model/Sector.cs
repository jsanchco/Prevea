namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Sector
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<WorkStation> WorkStations { get; set; }
    }
}
