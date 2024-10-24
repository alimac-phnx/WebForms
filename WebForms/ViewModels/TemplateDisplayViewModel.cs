using WebForms.Models;

namespace WebForms.ViewModels
{
    public class TemplateDisplayViewModel
    {
        public Template Template { get; set; }

        public int QuestionsCount { get; set; }

        public string TopicName { get; set; }

        public string AuthorName { get; set; }
    }
}
