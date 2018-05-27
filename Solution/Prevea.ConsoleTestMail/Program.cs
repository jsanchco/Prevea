namespace Prevea.ConsoleTestMail
{
    #region Using

    using System;
    using System.Net;
    using System.Net.Mail;

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("*          Process Test SendMail          *");
            Console.WriteLine("*******************************************");

            Console.WriteLine("Presiona una tecla para comenzar ...");
            Console.ReadLine();

            if (SendMailPrevea())
            {
                Console.WriteLine(String.Empty);
                Console.WriteLine("****************************************");
                Console.WriteLine("*          ¡¡¡¡Process OK!!!!!         *");
                Console.WriteLine("****************************************");
                Console.WriteLine(String.Empty);
            }
            else
            {
                Console.WriteLine(String.Empty);
                Console.WriteLine("****************************************");
                Console.WriteLine("*          ¡¡¡¡Process FAIL!!!!!       *");
                Console.WriteLine("****************************************");
                Console.WriteLine(String.Empty);
            }

            Console.WriteLine("Presiona una tecla para salir ...");

            Console.ReadLine();
        }

        private static bool SendMailPrevea()
        {
            try
            {
                string smtpAddress = "ssl0.ovh.net";
                int portNumber = 587;
                bool enableSSL = false;

                string emailFrom = "info@preveaspa.com";
                string password = "If1234567";
                string emailTo = "jsanchco@gmail.com";
                string subject = "Hello";
                string body = "Hello, I'm just writing this to say Hi!";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.Timeout = 10000;
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
