using System.ComponentModel.DataAnnotations;

namespace WebForms.Data
{
    public class RegistrationData
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email must math this form: *@*.*")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}