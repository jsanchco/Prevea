namespace Prevea.Service.Service
{
    #region Using

    using Model.Model;

    #endregion

    public partial class Service
    {
        public StretchCalculate GetStretchCalculateByNumberEmployees(int numberEmployees)
        {
            return Repository.GetStretchCalculateByNumberEmployees(numberEmployees);
        }
    }
}
