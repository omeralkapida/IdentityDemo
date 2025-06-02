using IdentityDemoSysteam.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace IdentityDemoSysteam.Infrastructure.GoogleEmail
{
    public class EmailService: IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var htmlBody = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
<meta charset='UTF-8' />
<meta name='viewport' content='width=device-width, initial-scale=1' />
<style>
  @import url('https://fonts.googleapis.com/css2?family=Nunito:wght@400;700&display=swap');

  body {{
    margin: 0; padding: 0; background-color: #e2e8f0; font-family: 'Nunito', sans-serif; color: #1a202c;
  }}

  .email-container {{
    max-width: 600px; margin: 40px auto; background: #ffffff; border-radius: 12px; box-shadow: 0 10px 30px rgba(0,0,0,0.1);
    overflow: hidden;
  }}

  .header {{
    background: linear-gradient(90deg, #4f46e5 0%, #3b82f6 100%);
    padding: 30px 20px; text-align: center; color: #fff;
  }}

  .header h1 {{
    font-weight: 700; font-size: 28px; margin: 0;
  }}

  .content {{
    padding: 30px 40px;
    border: 3px solid #3b82f6;
  }}

  .content p {{
    font-size: 16px; line-height: 1.6; margin-bottom: 24px;
  }}

  .button {{
    display: block;
    width: 100%;
    background: #ffffff;
    color: #3b82f6 !important;
    padding: 18px 0;
    font-weight: 700;
    border: 2px solid #3b82f6; /* primary border */
    border-radius: 8px;
    text-align: center;
    text-decoration: none;
    box-shadow: 0 8px 20px rgba(59,130,246,0.4); /* shadow-lg */
    transition: background 0.3s ease, color 0.3s ease;
    font-size: 18px;
    cursor: pointer;
  }}

  .button:hover {{
    background: #3b82f6;
    color: #ffffff !important;
    box-shadow: 0 12px 30px rgba(59,130,246,0.6);
  }}

  .footer {{
    text-align: center;
    font-size: 14px;
    color: #64748b;
    padding: 20px 40px 30px;
    border-top: 1px solid #e2e8f0;
  }}

  @media screen and (max-width: 480px) {{
    .email-container {{
      margin: 20px 10px;
    }}
    .content {{
      padding: 20px;
    }}
    .button {{
      padding: 16px 0;
      font-size: 16px;
    }}
  }}
</style>
</head>
<body>
  <div class='email-container'>
    <div class='header'>
      <h1>Şifre Sıfırlama</h1>
    </div>
    <div class='content'>
      <p>Merhaba,</p>
      <p>Şifrenizi sıfırlamak için aşağıdaki butona tıklayın. Bağlantı 30 dakika boyunca geçerli olacaktır.</p>
      <a href='{resetLink}' class='button'>Şifreyi Sıfırla</a>
      <p style='margin-top: 30px; font-size: 14px; color: #64748b;'>Eğer bu isteği siz yapmadıysanız, lütfen bu e-postayı dikkate almayınız.</p>
    </div>
    <div class='footer'>
      &copy; {DateTime.Now.Year} Identity Demo Proje. Tüm hakları saklıdır.
    </div>
  </div>
</body>
</html>
";

            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = "Şifre Sıfırlama Talebi";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };
                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // İstersen loglama yapabilir veya özel bir hata fırlatabilirsin
                throw new InvalidOperationException("E-posta gönderilirken bir hata oluştu.", ex);
            }
        }

    }
}
