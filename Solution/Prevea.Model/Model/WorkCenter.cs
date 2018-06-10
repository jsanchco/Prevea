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
        public string Province { get; set; }
        public string Location { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }

        public int EstablishmentTypeId { get; set; }
        public virtual EstablishmentType EstablishmentType { get; set; }

        public int WorkCenterStateId { get; set; }
        public virtual WorkCenterState WorkCenterState { get; set; }
    }
}
