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
        public List<DataMail> GetDataMails()
        {
            return Repository.GetDataMails();
        }

        public List<DataMail> GetDataMailsByMailing(int mailingId)
        {
            return Repository.GetDataMailsByMailing(mailingId);
        }

        public DataMail GetDataMailById(int id)
        {
            return Repository.GetDataMailById(id);
        }

        public Result SaveDataMail(DataMail dataMail)
        {
            try
            {
                dataMail = Repository.SaveDataMail(dataMail);

                if (dataMail == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del DataMail",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del DataMail se ha producido con éxito",
                    Object = dataMail,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del DataMail",
                    Object = dataMail,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteDataMail(int id)
        {
            try
            {
                var result = Repository.DeleteDataMail(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el DataMail",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del DataMail se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el DataMail",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateDataMails(int mailingId, List<DataMail> dataMails)
        {
            try
            {
                var deleteDataMails = Repository.DeleteAllDataMails(mailingId);
                if (!deleteDataMails)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en UpdateDataMails",
                        Object = null,
                        Status = Status.Error
                    };
                }
   
                foreach (var dataMail in dataMails)
                {
                    var addDataMail = SaveDataMail(dataMail);
                    if (addDataMail.Status == Status.Error)
                    {
                        return addDataMail;
                    }
                }

                return new Result
                {
                    Message = "UpdateDataMails se ha producido con éxito",
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en UpdateDataMails",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
