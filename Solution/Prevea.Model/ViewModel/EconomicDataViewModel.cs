namespace Prevea.Model.ViewModel
{
    #region Using

    using Model;

    #endregion

    public class EconomicDataViewModel
    {
        public int Id { get; set; }        
        public int ActualNumberEmployees { get; set; }
        public int SubscribeNumberEmployees { get; set; }
        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }
        public bool SinglePayment { get; set; }
        public int[] SplitPayment { get; set; }
        public string CompanyName  { get; set; }
        public string CompanyEnrollment { get; set; }
        public int ModePaymentId { get; set; }
        public string ModePaymentName { get; set; }
        public int? ModePaymentMedicalExaminationId { get; set; }
        public string ModePaymentMedicalExaminationName { get; set; }
        public string AccountNumber { get; set; }
        public StretchCalculate StretchCalculate { get; set; }
    }
}
