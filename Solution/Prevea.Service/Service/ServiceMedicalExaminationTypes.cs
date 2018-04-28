namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public List<Area> GetMedicalExaminationDocumentTypes()
        {
            return Repository.GetAreas()
                .Where(x => x.EntityId == 3 && x.Id != 16 && x.Id != 15)
                .ToList();
        }
    }
}
