namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<RequestMedicalExaminationEmployeeState> GetRequestMedicalExaminationEmployeeStates()
        {
            return Repository.GetRequestMedicalExaminationEmployeeStates();
        }

        public RequestMedicalExaminationEmployeeState GetRequestMedicalExaminationEmployeeState(int id)
        {
            return Repository.GetRequestMedicalExaminationEmployeeState(id);
        }
    }
}
