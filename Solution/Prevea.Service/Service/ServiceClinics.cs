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
        public List<Clinic> GetClincs()
        {
            return Repository.GetClincs();
        }

        public Clinic GetClinicById(int id)
        {
            return Repository.GetClinicById(id);
        }

        public Result SaveClinic(Clinic clinic)
        {
            try
            {
                clinic = Repository.SaveClinic(clinic);

                if (clinic == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Clínica",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Clínica se ha producido con éxito",
                    Object = clinic,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Clínica",
                    Object = clinic,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteClinic(int id)
        {
            try
            {
                var result = Repository.DeleteClinic(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Clínica",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la Clínica se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la Clínica",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
