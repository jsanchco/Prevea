namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public List<ModePaymentMedicalExamination> GetModesPaymentMedicalExamination()
        {
            return Context.ModesPaymentMedicalExamination
                .ToList();
        }

        public ModePaymentMedicalExamination GetModePaymentMedicalExamination(int id)
        {
            return Context.ModesPaymentMedicalExamination
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
