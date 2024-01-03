using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using VocabList.Core.Services;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

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

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken, bool IsUserPortal)
        {
            StringBuilder mail = new();
            mail.AppendLine("<strong>Merhaba,</strong><br>Parolanızı sıfırlama talebiniz için bir link gönderdik.<br><strong><a target=\"_blank\" href=\"");
            //mail.AppendLine(_configuration["BlazorServerClientAdminUrl"]);
            //mail.AppendLine("update-password/");
            //mail.AppendLine(userId);
            //mail.AppendLine("/");
            //mail.AppendLine(resetToken);

            //string resetUrl = $"{_configuration["BlazorServerClientAdminUrl"]}update-password/{userId}/{resetToken}";

            //Oluşturulan urle reset token parametresini query string ile veriyorum..
            //var origin = _configuration["BlazorServerClientUrl"];
            string origin;
            if (IsUserPortal)
            {
                // Kullanıcı portalı ise kullanılacak url..
                origin = _configuration["BlazorServerClientUserPortalUrl"];
            }
            else
            {
                // Admin paneli ise kullanılacak url..
                origin = _configuration["BlazorServerClientAdminUrl"];
            }
            
            var route = $"update-password/{userId}";
            var enpointUri = new Uri(string.Concat($"{origin}", route));
            var resetUrl = QueryHelpers.AddQueryString(enpointUri.ToString(), "Token", resetToken);
            
            mail.AppendLine(resetUrl);
            mail.AppendLine("\">Parolanızı yenilemek için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Parola sıfırlama isteğinde bulunmadıysanız, bu e-postayı dikkate almayınız.</span><br>Sevgiler..<br><br><br>BTD | VocabList");

            await SendMailAsync(to, "Parola Sıfırlama İsteği", mail.ToString());
        }
    }
}
