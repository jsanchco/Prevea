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
        public List<WorkCenter> GetWorkCentersByCompany(int companyId)
        {
            return Repository.GetWorkCentersByCompany(companyId);
        }

        public Result SaveWorkCenterCompany(int companyId, WorkCenter workCenter)
        {
            try
            {
                workCenter = Repository.SaveWorkCenter(workCenter);
                if (workCenter == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Centro de Trabajo-Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var workCenterCompany = Repository.SaveWorkCenterCompany(new WorkCenterCompany
                {
                    CompanyId = companyId,
                    WorkCenterId = workCenter.Id
                });

                if (workCenterCompany == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Centro de Trabajo-Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Centro de Trabajo-Compañia se ha producido con éxito",
                    Object = workCenterCompany,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Centro de Trabajo-Compañia",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteWorkCenterCompany(int workCenterId)
        {
            try
            {
                var workCenterCompany = Repository.GetWorkCenterCompanyByWorkCenter(workCenterId);
                if (workCenterCompany == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Centro de Trabajo-Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var result = Repository.DeleteWorkCenterCompany(workCenterCompany.Id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Centro de Trabajo-Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                result = Repository.DeleteWorkCenter(workCenterId);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Centro de Trabajo-Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Centro de Trabajo-Compañia se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el Centro de Trabajo-Compañia",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
