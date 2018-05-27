using System.Linq;

namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Net;
    using System.Net.Mail;

    #endregion

    public partial class Service
    {
        public List<Mailing> GetMailings()
        {
            return Repository.GetMailings();
        }

        public Mailing GetMailingById(int id)
        {
            return Repository.GetMailingById(id);
        }

        public Result SaveMailing(Mailing mailing)
        {
            try
            {
                mailing = Repository.SaveMailing(mailing);

                if (mailing == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Mailing",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Mailing se ha producido con éxito",
                    Object = mailing,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Mailing",
                    Object = mailing,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteMailing(int id)
        {
            try
            {
                var result = Repository.DeleteMailing(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Mailing",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Mailing se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el Mailing",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public bool SendEmail(int dataMailId, string emailTo, string subject, string body, string data)
        {
            try
            {
                const string smtpAddress = "ssl0.ovh.net";
                const int portNumber = 587;
                const string emailFrom = "info@preveaspa.com";
                const string password = "If1234567";

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;

                    mail.Body = FormatBodyMail(body, data);
                    if (mail.Body == null)
                        return false;

                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.Timeout = 10000;
                        smtp.EnableSsl = false;
                        smtp.Send(mail);
                    }
                }

                var dataMail = Repository.GetDataMailById(dataMailId);
                dataMail.DataMailStateId = (int) EnDataMailState.Sent;
                Repository.SaveDataMail(dataMail);

                return true;
            }
            catch (Exception e)
            {
                var dataMail = Repository.GetDataMailById(dataMailId);
                dataMail.DataMailStateId = (int)EnDataMailState.Error;
                Repository.SaveDataMail(dataMail);

                Console.WriteLine(e);
                return false;
            }
        }

        private string FormatBodyMail(string bodyMail, string data)
        {
            try
            {
                var columns = new Dictionary<string, string>();
                var index = 0;
                var indexEnd = 0;
                var indexFirstLetter = 0;
                var contColumns = 0;
                while (index < data.Length)
                {
                    index = data.IndexOf("[Columna", index, StringComparison.Ordinal);                    
                    if (index != -1)
                    {
                        indexEnd = data.IndexOf("]", index, StringComparison.Ordinal);
                        var column = data.Substring(index, indexEnd - index + 1);
                        columns.Add(column, "");
                    }
                    else
                    {
                        index = data.Length;
                    }

                    if (indexFirstLetter != 0)
                    {
                        contColumns ++;

                        var indexLastLetter = index;
                        columns["[Columna" + contColumns + "]"] = data.Substring(indexFirstLetter, indexLastLetter - indexFirstLetter);
                    }
                    indexFirstLetter = indexEnd + 1;
                    if (index != data.Length)
                        index = indexEnd;
                }

                foreach (var column in columns)
                    bodyMail = bodyMail.Replace(column.Key, column.Value);
                
                return bodyMail;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
