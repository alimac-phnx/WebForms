using System.ComponentModel.DataAnnotations;

namespace WebForms.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email must math this form: *@*.*")]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsAdmin { get; set; }

        public string Status { get; set; }

        public ICollection<Template> CreatedTemplates { get; set; } = new List<Template>();

        public ICollection<Form> AssignedForms { get; set; } = new List<Form>();
    }
}
