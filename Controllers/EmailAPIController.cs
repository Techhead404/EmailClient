using Microsoft.AspNetCore.Mvc;
using EmailClient.Models;
using EmailClient.Services;
using System.Threading.Tasks;

namespace EmailClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmailAPIController : Controller
    {
        private readonly EmailService _emailService;

        public EmailAPIController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emailService.SendEmailAsync(email.Recipient, email.Subject, email.Body, email.UserName);
                    return Ok("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to send email: {ex.Message}");
                }
            }

            return BadRequest("Invalid email model.");
        }
    }
}
