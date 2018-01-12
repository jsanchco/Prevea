namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public List<WorkCenter> GetWorkCenters()
        {
            return Repository.GetWorkCenters();
        }

        public WorkCenter GetWorkCenter(int id)
        {
            return Repository.GetWorkCenter(id);
        }

        public Result SaveWorkCenter(WorkCenter workCenter)
        {
            try
            {
                workCenter = Repository.SaveWorkCenter(workCenter);

                if (workCenter == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Centro de Trabajo",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Centro de Trabajo se ha producido con éxito",
                    Object = workCenter,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Centro de Trabajo",
                    Object = workCenter,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteWorkCenter(int id)
        {
            try
            {
                var result = Repository.DeleteWorkCenter(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Centro de Trabajo",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Centro de Trabajo se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el Centro de Trabajo",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SubscribeWorkCenter(int id, bool subscribe)
        {
            try
            {
                var result = Repository.SubscribeWorkCenter(id, subscribe);
                if (result == null)
                {
                    return new Result
                    {
                        Message = subscribe ? "Se ha producido un error al Dar de Alta al Centro de Trabajo" : "Se ha producido un error al Dar de Baja al Centro de Trabajo",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = subscribe ? "Dar de Alta al Centro de Trabajo se ha producido con éxito" : "Dar de Baja al Centro de Trabajo se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = subscribe ? "Se ha producido un error al Dar de Alta al Centro de Trabajo" : "Se ha producido un error al Dar de Baja al Centro de Trabajo",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
