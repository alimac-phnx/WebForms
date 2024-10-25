using WebForms.Models;

namespace WebForms.ViewModels
{
    public class TemplateCreateViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TopicId { get; set; }

        public IFormFile? ImageFile { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<Topic> AvailableTopics { get; set; } = new List<Topic>();

        public List<string> AvailableTags { get; set; } = new List<string>();
    }
}
