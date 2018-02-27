namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public List<RequestMedicalExaminationState> GetRequestMedicalExaminationStates()
        {
            return Context.RequestMedicalExaminationStates
                .Include(x => x.RequestMedicalExaminations)
                .ToList();
        }

        public RequestMedicalExaminationState GetRequestMedicalExaminationState(int id)
        {
            return Context.RequestMedicalExaminationStates
                .Include(x => x.RequestMedicalExaminations)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
