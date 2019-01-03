namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Prevea.Model.Model;

    #endregion

    public partial class Repository
    {
        public List<PriorityCorrectiveAction> GetPriorityCorrectiveActions()
        {
            return Context.PriorityCorrectiveActions
                .Include(x => x.CorrectiveActions)
                .ToList();
        }

        public PriorityCorrectiveAction GetPriorityCorrectiveAction(int id)
        {
            return Context.PriorityCorrectiveActions
                .Include(x => x.CorrectiveActions)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
