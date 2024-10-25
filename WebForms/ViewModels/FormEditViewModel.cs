using WebForms.Models;

namespace WebForms.ViewModels
{
    public class FormEditViewModel
    {
        public int FormId { get; set; }

        public Template? Template { get; set; }

        public string? TopicName { get; set; }

        public int QuestionsCount { get; set; }

        public string? AuthorName { get; set; }

        public DateTime AssignedAt { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
