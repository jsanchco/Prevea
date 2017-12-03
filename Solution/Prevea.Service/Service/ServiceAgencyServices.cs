namespace Prevea.Service.Service
{
    #region Using

    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public AgencyService GetAgencyService(int agencyServiceId)
        {
            return Repository.GetAgencyService(agencyServiceId);
        }

        public Result SaveAgencyService(AgencyService agencyService)
        {
            try
            {
                agencyService = Repository.SaveAgencyService(agencyService);

                if (agencyService == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de los Servicios de Gestoría",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de los Servicios de Gestoría se ha producido con éxito",
                    Object = agencyService,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de los Servicios de Gestoría",
                    Object = agencyService,
                    Status = Status.Error
                };
            }
        }
    }
}
