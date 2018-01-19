namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class EngagementType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnEngagementType), Id));

        public virtual ICollection<AgencyService> AgencyServices { get; set; }
    }

    public enum EnEngagementType { NotMapped, Society, Autonomous }
}
