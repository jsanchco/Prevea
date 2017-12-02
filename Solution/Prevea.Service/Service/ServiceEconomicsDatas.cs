namespace Prevea.Service.Service
{
    #region Using

    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public EconomicData GetEconomicData(int economicDataId)
        {
            return Repository.GetEconomicData(economicDataId);
        }

        public Result SaveEconomicData(EconomicData economicData)
        {
            try
            {
                economicData = Repository.SaveEconomicData(economicData);

                if (economicData == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de los Datos Económicos",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de los Datos Económicos se ha producido con éxito",
                    Object = economicData,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de los Datos Económicos",
                    Object = economicData,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateEconomicData(int economicDataId, EconomicData economicData)
        {
            throw new System.NotImplementedException();
        }

        public Result DeleteEconomicData(int economicDataId)
        {
            throw new System.NotImplementedException();
        }
    }
}
