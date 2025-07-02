using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Services.EmailS;
using Services.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/Email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost]
        public IActionResult SendEmail(EmailDTO email)

        {
            _emailService.SendAsync(email);



            return Ok(new { message = "Email sent successfully" });
        }
    }
}

