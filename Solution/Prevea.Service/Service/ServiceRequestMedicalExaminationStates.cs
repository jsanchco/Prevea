namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<RequestMedicalExaminationState> GetRequestMedicalExaminationStates()
        {
            return Repository.GetRequestMedicalExaminationStates();
        }

        public RequestMedicalExaminationState GetRequestMedicalExaminationState(int id)
        {
            return Repository.GetRequestMedicalExaminationState(id);
        }
    }
}
