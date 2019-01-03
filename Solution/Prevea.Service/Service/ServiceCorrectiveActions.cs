namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Prevea.IService.IService;
    using Prevea.Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public List<CorrectiveAction> GetCorrectiveActions()
        {
            return Repository.GetCorrectiveActions();
        }

        public CorrectiveAction GetCorrectiveActionById(int id)
        {
            return Repository.GetCorrectiveActionById(id);
        }

        public Result SaveCorrectiveAction(CorrectiveAction correctiveAction)
        {
            try
            {
                correctiveAction = Repository.SaveCorrectiveAction(correctiveAction);

                if (correctiveAction == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de CorrectiveAction",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de CorrectiveAction se ha producido con éxito",
                    Object = correctiveAction,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de CorrectiveAction",
                    Object = correctiveAction,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteCorrectiveAction(int id)
        {
            try
            {
                var result = Repository.DeleteCorrectiveAction(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la CorrectiveAction",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la CorrectiveAction se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la CorrectiveAction",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
