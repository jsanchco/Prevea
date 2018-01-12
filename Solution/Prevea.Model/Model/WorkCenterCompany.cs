namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class WorkCenterCompany
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public int WorkCenterId { get; set; }
        public WorkCenter WorkCenter { get; set; }
    }
}
