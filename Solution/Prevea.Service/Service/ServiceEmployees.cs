namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public List<HeaderEmployeeDocuments> GetHeaderEmployeeDocuments(int employeeId)
        {
            return Repository.GetRequestMedicalExaminationEmployeeByEmployeeId(employeeId)
                .Select(x => new HeaderEmployeeDocuments
                {
                    RequestMedicalExaminationEmployeeId = x.Id,
                    EmployeeId = x.EmployeeId,
                    Description = "Reconocimiento Médico",
                    Date = x.Date
                })
                .ToList();
        }
    }
}
