using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace EmailClient.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string Recipient { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

    }
}
