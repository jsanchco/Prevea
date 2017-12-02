namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public Agency GetAgency(int id)
        {
            return Repository.GetAgency(id);
        }

        public List<Agency> GetAgencies()
        {
            return Repository.GetAgencies();
        }

        public Result SaveAgency(Agency agency, int companyId)
        {
            try
            {
                agency = Repository.SaveAgency(agency, companyId);

                if (agency == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Gestoría",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Gestoría se ha producido con éxito",
                    Object = agency,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Gestoría",
                    Object = agency,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateAgency(Agency agency, int id)
        {
            try
            {
                agency = Repository.UpdateAgency(id, agency);

                if (agency == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Gestoría",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Gestoría se ha producido con éxito",
                    Object = agency,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Gestoría",
                    Object = agency,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteAgency(int id)
        {
            try
            {
                var result = Repository.DeleteAgency(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Gestoría",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la Gestoría se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la Gestoría",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
