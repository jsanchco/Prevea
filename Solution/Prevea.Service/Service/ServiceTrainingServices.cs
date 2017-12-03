namespace Prevea.Service.Service
{
    #region Using

    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public TrainingService GetTrainingService(int trainingServiceId)
        {
            return Repository.GetTrainingService(trainingServiceId);
        }

        public Result SaveTrainingService(TrainingService trainingService)
        {
            try
            {
                trainingService = Repository.SaveTrainingService(trainingService);

                if (trainingService == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de los Servicios de Formación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de los Servicios de Formación se ha producido con éxito",
                    Object = trainingService,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de los Servicios de Formación",
                    Object = trainingService,
                    Status = Status.Error
                };
            }
        }
    }
}
