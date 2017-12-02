namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public PaymentMethod GetPaymentMethod(int paymentMethodId)
        {
            return Context.PaymentMethods
                .Include(x => x.Company)
                .FirstOrDefault(x => x.Id == paymentMethodId);
        }

        public PaymentMethod SavePaymentMethod(PaymentMethod paymentMethod)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.PaymentMethods.AddOrUpdate(paymentMethod);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return paymentMethod;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public PaymentMethod UpdatePaymentMethod(int paymentMethodId, PaymentMethod paymentMethod)
        {
            throw new System.NotImplementedException();
        }

        public bool DeletePaymentMethod(int paymentMethodId)
        {
            throw new System.NotImplementedException();
        }
    }
}
