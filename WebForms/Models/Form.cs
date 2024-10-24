namespace WebForms.Models
{
    public class Form
    {
        public int Id { get; set; }

        public int TemplateId { get; set; }

        public int AssignedByUserId { get; set; }

        public DateTime AssignedAt { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}