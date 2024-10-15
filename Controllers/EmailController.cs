using Microsoft.AspNetCore.Mvc;
using EmailClient.Models;
using EmailClient.Services;
using System;
using System.Threading.Tasks;
using System.Net.Mail;

namespace EmailClient.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new EmailModel());
        }
        [HttpGet]
        public IActionResult APIdoc()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailModel email)
        {   
            if (ModelState.IsValid)
            {
                try
                {
                    // Assuming that SendEmailAsync requires both UserName and Password
                    await _emailService.SendEmailAsync(
                        email.Recipient,
                        email.Subject,
                        email.Body
                    );

                    TempData["Message"] = "Email sent successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log error message and display it on the page
                    ModelState.AddModelError(string.Empty, $"Failed to send email: {ex.Message}");
                    ViewBag.Message = $"Failed to send email: {ex.Message}";
                }
            }

            // Return the email model to the view so the user's input is preserved
            return View("Index", email);
        }
    }
}
