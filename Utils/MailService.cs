using System.Net.Mail;
using System.Net;

namespace Resto_Backend.Utils
{
    public class MailService
    {
        public void SendEmailNotification(string toEmail,string Subject, string Message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("amarrestaurant02@gmail.com"); // Your email
                mail.To.Add(toEmail);
                mail.Subject = Subject;
                mail.Body = $"Dear {toEmail}, {Message}.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("amarrestaurant02@gmail.com", "siru vdoc tjdx jwbf"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false // Ensure this is false!
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Error: " + ex.Message);
            }
        }
    }
}
