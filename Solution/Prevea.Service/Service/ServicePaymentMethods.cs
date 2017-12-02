namespace Prevea.Service.Service
{
    #region Using

    using IService.IService;
    using Model.Model;
    using System;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public PaymentMethod GetPaymentMethod(int paymentMethodId)
        {
            return Repository.GetPaymentMethod(paymentMethodId);
        }

        public Result SavePaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                if (paymentMethod.SplitPayment == null || paymentMethod.SplitPayment.Length == 0)
                    paymentMethod.MonthsSplitPayment = string.Empty;
                else
                    paymentMethod.MonthsSplitPayment = string.Join(",", paymentMethod.SplitPayment);

                paymentMethod = Repository.SavePaymentMethod(paymentMethod);

                if (paymentMethod == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de los Métodos de Pago",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de los Métodos de Pago se ha producido con éxito",
                    Object = paymentMethod,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de los Métodos de Pago",
                    Object = paymentMethod,
                    Status = Status.Error
                };
            }
        }

        public Result UpdatePaymentMethod(int paymentMethodId, PaymentMethod paymentMethod)
        {
            throw new System.NotImplementedException();
        }

        public Result DeletePaymentMethod(int paymentMethodId)
        {
            throw new System.NotImplementedException();
        }
    }
}
