namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class WorkStation
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string ProfessionalCategory { get; set; }

        public string Description { get; set; }

        public int SectorId { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
