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
        public List<PreventivePlanTemplatePreventivePlan> GetPreventivePlanTemplatePreventivePlans()
        {
            return Repository.GetPreventivePlanTemplatePreventivePlans();
        }

        public List<PreventivePlanTemplatePreventivePlan> GetPreventivePlanTemplatePreventivePlansByPreventivePlanId(int preventivePlanId)
        {
            return Repository.GetPreventivePlanTemplatePreventivePlansByPreventivePlanId(preventivePlanId);
        }

        public PreventivePlanTemplatePreventivePlan ExistPreventivePlanTemplatePreventivePlan(int preventivePlanId, int templatePreventivePlanId)
        {
            return Repository.ExistPreventivePlanTemplatePreventivePlan(preventivePlanId, templatePreventivePlanId);
        }

        public PreventivePlanTemplatePreventivePlan GetPreventivePlanTemplatePreventivePlanById(int id)
        {
            return Repository.GetPreventivePlanTemplatePreventivePlanById(id);
        }

        public Result SavePreventivePlanTemplatePreventivePlan(
            PreventivePlanTemplatePreventivePlan preventivePlanTemplatePreventivePlan)
        {
            try
            {
                preventivePlanTemplatePreventivePlan = Repository.SavePreventivePlanTemplatePreventivePlan(preventivePlanTemplatePreventivePlan);

                if (preventivePlanTemplatePreventivePlan == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del PreventivePlanTemplatePreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del PreventivePlanTemplatePreventivePlan se ha producido con éxito",
                    Object = preventivePlanTemplatePreventivePlan,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del PreventivePlanTemplatePreventivePlan",
                    Object = preventivePlanTemplatePreventivePlan,
                    Status = Status.Error
                };
            }
        }

        public Result DeletePreventivePlanTemplatePreventivePlan(int id)
        {
            try
            {
                var result = Repository.DeletePreventivePlanTemplatePreventivePlan(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el PreventivePlanTemplatePreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del PreventivePlanTemplatePreventivePlan se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el PreventivePlanTemplatePreventivePlan",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
