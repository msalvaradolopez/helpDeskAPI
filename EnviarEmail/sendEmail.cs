using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Mail;

namespace helpDeskAPI.EnviarEmail
{
    public class sendEmail
    {
        public void mandarEmail(string destinatario, string asunto, string mensaje)
        {
            try
            {

                string _smtpServer = WebConfigurationManager.AppSettings["smtpServer"];
                string _mailSend = WebConfigurationManager.AppSettings["mailSend"];
                string _passMail = WebConfigurationManager.AppSettings["passMail"];
                string _userMail = WebConfigurationManager.AppSettings["userMail"];
                string _smtpPort = WebConfigurationManager.AppSettings["smtpPort"];
                string _strSSL = WebConfigurationManager.AppSettings["strSSL"];


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_smtpServer);

                mail.From = new MailAddress(_mailSend);
                mail.To.Add(destinatario);
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = mensaje;

                SmtpServer.Port = Int32.Parse(_smtpPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(_userMail, _passMail);
                SmtpServer.EnableSsl = _strSSL == "1";

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {

            }
        }
    }
}