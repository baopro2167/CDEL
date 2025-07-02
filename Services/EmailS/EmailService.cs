

using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;


using Microsoft.Extensions.Configuration;

using MailKit.Net.Smtp;

namespace Services.EmailS
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendAsync(EmailDTO emailDTO)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(emailDTO.To));
            email.Subject = emailDTO.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailDTO.Body };


            // 2. Gửi email qua SMTP
            using var smtp = new SmtpClient();
            // Thay 587 bằng port thực tế, có thể dùng SecureSocketOptions.Auto hoặc StartTls
            smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            // Nếu SMTP yêu cầu xác thực, bỏ comment và điền đúng username/password:
            smtp.Authenticate(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
