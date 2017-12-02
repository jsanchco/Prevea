namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<ModePaymentMedicalExamination> GetModesPaymentMedicalExamination()
        {
            return Repository.GetModesPaymentMedicalExamination();
        }

        public ModePaymentMedicalExamination GetModePaymentMedicalExamination(int id)
        {
            return Repository.GetModePaymentMedicalExamination(id);
        }
    }
}
