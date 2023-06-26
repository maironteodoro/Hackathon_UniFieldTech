using System.Net.Mail;
using System.Net;

namespace UnifieldTech.ViewModels
{
    public class Email
    {
        public void EnviarEmail(string email, string codigo)
        {
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "";
            string smtpPassword = "";

            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Token de Verificação";
                    mailMessage.Body = "Esse é seu código de validação: " + codigo;

                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}
