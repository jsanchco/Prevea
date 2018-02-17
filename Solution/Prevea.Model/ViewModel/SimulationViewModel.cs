namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class SimulationViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserInitials { get; set; }
        public int? UserAssignedId { get; set; }
        public string UserAssignedInitials { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public bool Original { get; set; }
        public int? SimulationParentId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEnrollment { get; set; }
        public string NIF { get; set; }
        public int NumberEmployees { get; set; }
        public int SimulationStateId { get; set; }
        public string SimulationStateName { get; set; }
        public string SimulationStateDescription { get; set; }
        public decimal TotalForeignPreventionService { get; set; }
        public decimal TotalAgencyService { get; set; }
        public decimal TotalTrainingService { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public bool Active { get; set; }
    }
}
