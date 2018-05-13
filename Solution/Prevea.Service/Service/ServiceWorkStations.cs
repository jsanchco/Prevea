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
        public List<WorkStation> GetWorkStations()
        {
            return Repository.GetWorkStations();
        }

        public WorkStation GetWorkStationById(int id)
        {
            return Repository.GetWorkStationById(id);
        }

        public Result SaveWorkStation(WorkStation workStation)
        {
            try
            {
                workStation = Repository.SaveWorkStation(workStation);

                if (workStation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del WorkStation",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del WorkStation se ha producido con éxito",
                    Object = workStation,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del WorkStation",
                    Object = workStation,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteWorkStation(int id)
        {
            try
            {
                var result = Repository.DeleteWorkStation(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el WorkStation",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del WorkStation se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el WorkStation",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
