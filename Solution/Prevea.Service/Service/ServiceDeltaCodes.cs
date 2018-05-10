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
        public List<DeltaCode> GetDeltaCodes()
        {
            return Repository.GetDeltaCodes();
        }

        public DeltaCode GetDeltaCodeById(int id)
        {
            return Repository.GetDeltaCodeById(id);
        }

        public Result SaveDeltaCode(DeltaCode deltaCode)
        {
            try
            {
                deltaCode = Repository.SaveDeltaCode(deltaCode);

                if (deltaCode == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del DeltaCode",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del DeltaCode se ha producido con éxito",
                    Object = deltaCode,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del DeltaCode",
                    Object = deltaCode,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteDeltaCode(int id)
        {
            try
            {
                var result = Repository.DeleteDeltaCode(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el DeltaCode",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del DeltaCode se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el DeltaCode",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
