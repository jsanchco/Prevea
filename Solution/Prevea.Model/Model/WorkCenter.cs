namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class WorkCenter
    {
        [Key, Required]
        public int Id { get; set; }

        public string Address  { get; set; }

        public string Description { get; set; }

        public int EstablishmentTypeId { get; set; }
        public EstablishmentType EstablishmentType { get; set; }

        public int WorkCenterStateId { get; set; }
        public WorkCenterState WorkCenterState { get; set; }
    }
}
