namespace Prevea.Service.Service
{
    #region Using

    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public ForeignPreventionService GetForeignPreventionService(int foreignPreventionServiceId)
        {
            return Repository.GetForeignPreventionService(foreignPreventionServiceId);
        }

        public Result SaveForeignPreventionService(ForeignPreventionService foreignPreventionService)
        {
            try
            {
                foreignPreventionService = Repository.SaveForeignPreventionService(foreignPreventionService);

                if (foreignPreventionService == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de los Servicios de Prevención Agena",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de los Servicios de Prevención Agena se ha producido con éxito",
                    Object = foreignPreventionService,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de los Servicios de Prevención Agena",
                    Object = foreignPreventionService,
                    Status = Status.Error
                };
            }
        }
    }
}
