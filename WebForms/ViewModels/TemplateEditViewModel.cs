using WebForms.Models;

namespace WebForms.ViewModels
{
    public class TemplateEditViewModel
    {
        public int TemplateId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TopicId { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? NewImageFile { get; set; }

        public List<string> CurrentTags = new List<string>();

        public List<string> NewTags { get; set; } = new List<string>();

        public List<Topic> AvailableTopics = new List<Topic>();

        public List<string> AvailableTags = new List<string>();

        public List<Question> Questions { get; set; } = new List<Question>();
    }
}