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
        public List<Sector> GetSectors()
        {
            return Repository.GetSectors();
        }

        public Sector GetSectorById(int id)
        {
            return Repository.GetSectorById(id);
        }

        public Result SaveSector(Sector sector)
        {
            try
            {
                sector = Repository.SaveSector(sector);

                if (sector == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Sector",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Sector se ha producido con éxito",
                    Object = sector,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Sector",
                    Object = sector,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteSector(int id)
        {
            try
            {
                var result = Repository.DeleteSector(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Sector",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Sector se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el Sector",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
