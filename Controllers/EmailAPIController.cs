
using EmailClient.Models;
using EmailClient.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmailClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailAPIController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailAPIController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emailService.SendEmailAsync(emailModel.Recipient, emailModel.Subject, emailModel.Body);
                    return Ok("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return BadRequest("Invalid email model.");
        }

        // Test endpoint to return dummy email data
        [HttpGet("test")]
        public IActionResult GetTestEmail()
        {
            var testEmail = new EmailModel
            {
                Recipient = "dillonlgreek@gmail.com",
                Subject = "Test Email",
                Body = "This is a test email body."
            };

            return Ok(testEmail);
        }
    }
}