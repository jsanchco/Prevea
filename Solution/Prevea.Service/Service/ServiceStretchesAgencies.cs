namespace Prevea.Service.Service
{
    #region Using

    using Model.Model;

    #endregion

    public partial class Service
    {
        public StretchAgency GetStretchAgencyByNumberEmployees(int numberEmployees)
        {
            return Repository.GetStretchAgencyByNumberEmployees(numberEmployees);
        }
    }
}
