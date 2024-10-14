using Microsoft.AspNetCore.Mvc;
using EmailClient.Models;
using EmailClient.Services;
using System;
using System.Threading.Tasks;

namespace EmailClient.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailService _emailService;


        public IActionResult Index()
        {
            return View(new EmailModel());
        }
        
        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emailService.SendEmail(email.Recipient, email.Subject, email.Body);
                    ViewBag.Message = "Email sent successfully!";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Failed to send email: {ex.Message}");
                    ViewBag.Message = "Failed to send email.";
                }
            }

            return View("Index", email);
        }
    }
}
