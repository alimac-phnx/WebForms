using System.ComponentModel.DataAnnotations;

namespace WebForms.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsAdmin { get; set; }

        public string Status { get; set; }

        public List<Template> CreatedTemplates { get; set; } = new List<Template>();

        public List<Form> AssignedForms { get; set; } = new List<Form>();
    }
}