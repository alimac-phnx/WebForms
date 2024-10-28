using System.ComponentModel.DataAnnotations;

namespace WebForms.ViewModels
{
    public class RegistrationDataViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email must have this form: *@*.*")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}