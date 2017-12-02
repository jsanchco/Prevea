namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public StretchEmployee GetStretchEmployeeByNumberEmployees(int numberEmployees)
        {
            if (numberEmployees == 0)
                return null;

            var stretchEmployee = Context.StretchesEmployees
                                      .FirstOrDefault(x => x.Initial <= numberEmployees && x.End >= numberEmployees) ??
                                  Context.StretchesEmployees.ToList().Last();

            return stretchEmployee;
        }

        public StretchCalculate GetStretchCalculateByNumberEmployees(int numberEmployees)
        {
            if (numberEmployees == 0)
                return new StretchCalculate
                {
                    AmountByEmployeeInTecniques = 0,
                    AmountByEmployeeInHealthVigilance = 0,
                    AmountByEmployeeInMedicalExamination = 0
                };

            var stretchEmployee = GetStretchEmployeeByNumberEmployees(numberEmployees);

            if (!stretchEmployee.IsComplete)
                return new StretchCalculate
                {
                    AmountByEmployeeInTecniques = stretchEmployee.AmountByEmployeeInTecniques,
                    AmountByEmployeeInHealthVigilance = stretchEmployee.AmountByEmployeeInHealthVigilance,
                    AmountByEmployeeInMedicalExamination = stretchEmployee.AmountByEmployeeInMedicalExamination
                };

            return
                new StretchCalculate
                {
                    AmountByEmployeeInTecniques = stretchEmployee.AmountByEmployeeInTecniques / numberEmployees,
                    AmountByEmployeeInHealthVigilance = stretchEmployee.AmountByEmployeeInHealthVigilance,
                    AmountByEmployeeInMedicalExamination = stretchEmployee.AmountByEmployeeInMedicalExamination
                };
        }
    }
}
