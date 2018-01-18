namespace Prevea.Model.ViewModel
{
    public class WorkCenterViewModel
    {        
        public int Id { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public int EstablishmentTypeId { get; set; }
        public string EstablishmentTypeName { get; set; }
        public int WorkCenterStateId { get; set; }
        public string WorkCenterStateName { get; set; }
    }
}
