using System;
using System.Linq;

namespace Prevea.Model.ViewModel
{
    public class PaymentMethodViewModel
    {
        public int Id { get; set; }
        public bool SinglePayment { get; set; }

        //public int[] SplitPayment { get; set; }
        public int[] SplitPayment
        {
            get
            {
                return !string.IsNullOrEmpty(MonthsSplitPayment) ? MonthsSplitPayment.Split(',').Select(n => Convert.ToInt32(n)).ToArray() : null;
            }
        }

        public string MonthsSplitPayment { get; set; }
        public int ModePaymentId { get; set; }
        public int? ModePaymentMedicalExaminationId { get; set; }
        public string AccountNumber { get; set; }
    }
}
