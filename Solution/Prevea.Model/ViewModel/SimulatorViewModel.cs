namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class SimulatorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIF { get; set; }
        public int NumberEmployees { get; set; }
        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime Date { get; set; }
    }
}
