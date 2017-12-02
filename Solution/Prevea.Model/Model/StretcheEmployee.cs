namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class StretchEmployee
    {
        [Key, Required]
        public int Id { get; set; }

        public int Initial { get; set; }
        public int? End { get; set; }
        public decimal AmountByEmployeeInTecniques { get; set; }
        public decimal AmountByEmployeeInHealthVigilance { get; set; }
        public decimal AmountByEmployeeInMedicalExamination { get; set; }
        public bool IsComplete { get; set; }
    }

    public struct StretchCalculate
    {
        public decimal AmountByEmployeeInTecniques;
        public decimal AmountByEmployeeInHealthVigilance;
        public decimal AmountByEmployeeInMedicalExamination;
    }
}
