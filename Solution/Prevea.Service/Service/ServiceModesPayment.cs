namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<ModePayment> GetModesPayment()
        {
            return Repository.GetModesPayment();
        }

        public ModePayment GetModePayment(int modePaymentId)
        {
            return Repository.GetModePayment(modePaymentId);
        }
    }
}
