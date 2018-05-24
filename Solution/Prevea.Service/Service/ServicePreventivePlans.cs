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
        public List<PreventivePlan> GetPreventivePlans(int userId)
        {
            return Repository.GetPreventivePlans(userId);
        }

        public PreventivePlan GetPreventivePlanById(int id)
        {
            return Repository.GetPreventivePlanById(id);
        }

        public bool ExistPreventivePlan(int companyId, int documentId)
        {
            return Repository.ExistPreventivePlan(companyId, documentId);
        }

        public Result SavePreventivePlan(PreventivePlan preventivePlan)
        {
            try
            {
                if (Repository.ExistPreventivePlan(preventivePlan.CompanyId, preventivePlan.DocumentId))
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del PreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                preventivePlan = Repository.SavePreventivePlan(preventivePlan);

                if (preventivePlan == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del PreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del PreventivePlan se ha producido con éxito",
                    Object = preventivePlan,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del PreventivePlan",
                    Object = preventivePlan,
                    Status = Status.Error
                };
            }
        }

        public Result DeletePreventivePlan(int id)
        {
            try
            {
                var result = Repository.DeletePreventivePlan(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el PreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del PreventivePlan se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el PreventivePlan",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
