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
        public List<RequestMedicalExaminationEmployeeState> GetRequestMedicalExaminationEmployeeStates()
        {
            return Context.RequestMedicalExaminationEmployeeStates
                .Include(x => x.RequestMedicalExaminationEmployees)
                .ToList();
        }

        public RequestMedicalExaminationEmployeeState GetRequestMedicalExaminationEmployeeState(int id)
        {
            return Context.RequestMedicalExaminationEmployeeStates
                .Include(x => x.RequestMedicalExaminationEmployees)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
