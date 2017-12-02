namespace Prevea.Model.ViewModel
{
    public class PaymentModeViewModel
    {
        public int Id { get; set; }
        public bool SinglePayment { get; set; }
        public int[] SplitPayment { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEnrollment { get; set; }
        public int ModePaymentId { get; set; }
        public string ModePaymentName { get; set; }
        public int? ModePaymentMedicalExaminationId { get; set; }
        public string ModePaymentMedicalExaminationName { get; set; }
        public string AccountNumber { get; set; }
    }
}
