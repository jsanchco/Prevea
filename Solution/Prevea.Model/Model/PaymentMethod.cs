namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.Linq;

    #endregion

    public class PaymentMethod
    {
        [ForeignKey("Company")]
        public int Id { get; set; }

        public bool SinglePayment
        {
            get
            {
                if (SplitPayment == null || SplitPayment.Length == 0)
                    return true;

                return false;
            }
            
        }

        public string MonthsSplitPayment { get; set; }

        [NotMapped]
        //public int[] SplitPayment { get; set; }
        public int[] SplitPayment
        {
            get
            {
                return !string.IsNullOrEmpty(MonthsSplitPayment) ? MonthsSplitPayment.Split(',').Select(n => Convert.ToInt32(n)).ToArray() : null;
            }
        }
        
        public virtual Company Company { get; set; }

        public int ModePaymentId { get; set; }
        public virtual ModePayment ModePayment { get; set; }

        public int? ModePaymentMedicalExaminationId { get; set; }
        public virtual ModePaymentMedicalExamination ModePaymentMedicalExamination { get; set; }

        public string AccountNumber { get; set; }
        public string EntityName { get; set; }
    }
}
