namespace Prevea.Model.ViewModel
{
    public class AgencyServiceViewModel
    {
        public int Id { get; set; }

        public int EngagementTypeId { get; set; }
        public decimal AmountByEngagementType { get; set; }
        public decimal AmountByRoster { get; set; }
        public string Observations { get; set; }
        public bool IncludeInContractualDocument { get; set; }
        public decimal Total { get; set; }
    }
}
