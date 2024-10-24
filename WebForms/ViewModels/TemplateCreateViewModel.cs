using WebForms.Models;

namespace WebForms.ViewModels
{
    public class TemplateCreateViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TopicId { get; set; } // Например: "Education", "Quiz", "Other"

        public IFormFile? ImageFile { get; set; }

        public List<string> Tags { get; set; } = new List<string>(); // Список тегов

        public List<Question> Questions { get; set; } = new List<Question>(); // no migration yet -- formtemplate pair?

        public List<Topic> AvailableTopics { get; set; } = new List<Topic>(); // Список тем для выбора

        public List<string> AvailableTags { get; set; } = new List<string>(); // Список тегов для автозаполнения
    }
}
