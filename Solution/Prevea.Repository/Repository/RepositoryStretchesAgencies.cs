namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public StretchAgency GetStretchAgencyByNumberEmployees(int numberEmployees)
        {
            if (numberEmployees == 0)
                return null;

            var stretchAgency = Context.StretchesAgencies
                                      .FirstOrDefault(x => x.Initial <= numberEmployees && x.End >= numberEmployees) ??
                                  Context.StretchesAgencies.ToList().Last();

            return stretchAgency;
        }
    }
}
