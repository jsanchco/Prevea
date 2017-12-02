namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public List<ModePayment> GetModesPayment()
        {
            return Context.ModesPayment
                .ToList();
        }

        public ModePayment GetModePayment(int id)
        {
            return Context.ModesPayment
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
