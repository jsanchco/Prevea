namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Prevea.Model.Model;

    #endregion

    public partial class Service
    {
        public List<PriorityCorrectiveAction> GetPriorityCorrectiveActions()
        {
            return Repository.GetPriorityCorrectiveActions();
        }

        public PriorityCorrectiveAction GetPriorityCorrectiveAction(int id)
        {
            return Repository.GetPriorityCorrectiveAction(id);
        }
    }
}
