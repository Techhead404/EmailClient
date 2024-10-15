using System.ComponentModel.DataAnnotations;

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

        
        [EmailAddress]
        public string UserName { get; set; }


        public string Password { get; set; }
    }
}
