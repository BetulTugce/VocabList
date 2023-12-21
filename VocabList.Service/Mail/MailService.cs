using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using VocabList.Core.Services;
using System.Text;

namespace VocabList.Service.Mail
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
            {
                mail.To.Add(to); //Mailin gönderileceği adres..
            }
            mail.Subject = subject; //Mailin konusu
            mail.Body = body;

            //Maili gönderen kişinin mail adresi ve kişinin adı..
            mail.From = new(_configuration["Mail:Username"], "VocabList", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("<strong>Merhaba,</strong><br>Parolanızı sıfırlama talebiniz için bir link gönderdik.<br><strong><a target=\"_blank\" href=\"");
            mail.AppendLine(_configuration["BlazorServerClientUrl"]);
            mail.AppendLine("/update-password/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Parolanızı yenilemek için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Parola sıfırlama isteğinde bulunmadıysanız, bu e-postayı dikkate almayınız.</span><br>Sevgiler..<br><br><br>BTD | VocabList");

            await SendMailAsync(to, "Parola Sıfırlama İsteği", mail.ToString());
        }
    }
}
