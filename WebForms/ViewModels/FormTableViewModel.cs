using WebForms.Models;

namespace WebForms.ViewModels
{
    public class FormTableViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime AssignedAt { get; set; }

        public int AnswerCount { get; set; }
    }
}
