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

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new EmailModel());
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailModel email)
        {

            await _emailService.SendEmailAsync(
                        email.Recipient,
                        email.Subject,
                        email.Body,
                        email.UserName
                        
                    );
            ViewBag.Message = "Success";

            return View("Index", email);
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        // Assuming that SendEmailAsync requires both UserName and Password
            //        await _emailService.SendEmailAsync(
            //            email.Recipient,
            //            email.Subject,
            //            email.Body,
            //            email.UserName,
            //            email.Password  // Add password to the method call
            //        );

            //        ViewBag.Message = "Email sent successfully!";
            //    }
            //    catch (Exception ex)
            //    {
            //        // Log error message and display it on the page
            //        ModelState.AddModelError(string.Empty, $"Failed to send email: {ex.Message}");
            //        ViewBag.Message = $"Failed to send email: {ex.Message}";
            //    }
            //}

            //// Return the email model to the view so the user's input is preserved
            //return View("Index", email);
        }
    }
}
