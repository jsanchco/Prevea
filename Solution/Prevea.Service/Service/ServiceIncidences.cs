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
        public List<Incidence> GetIncidences()
        {
            return Repository.GetIncidences();
        }

        public List<Incidence> GetIncidencesByUserId(int userId)
        {
            return Repository.GetIncidencesByUserId(userId);
        }

        public Incidence GetIncidenceById(int id)
        {
            return Repository.GetIncidenceById(id);
        }

        public Result SaveIncidence(Incidence incidence)
        {
            try
            {
                incidence = Repository.SaveIncidence(incidence);

                if (incidence == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Incidence",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Incidence se ha producido con éxito",
                    Object = incidence,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Incidence",
                    Object = incidence,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteIncidence(int id)
        {
            try
            {
                var result = Repository.DeleteIncidence(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Incidence",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la Incidence se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la Incidence",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<IncidenceState> GetIncidenceStates()
        {
            return Repository.GetIncidenceStates();
        }

        public List<CriticalNivel> GetCriticalNivels()
        {
            return Repository.GetCriticalNivels();
        }
    }
}
