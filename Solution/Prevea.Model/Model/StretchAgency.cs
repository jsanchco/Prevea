namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class StretchAgency
    {
        [Key, Required]
        public int Id { get; set; }

        public int Initial { get; set; }
        public int? End { get; set; }
        public decimal AmountByRoster { get; set; }
        public decimal Percentege { get; set; }
    }
}
