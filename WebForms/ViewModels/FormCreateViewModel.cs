using WebForms.Models;

namespace WebForms.ViewModels
{
    public class FormCreateViewModel
    {
        public Template? Template { get; set; }

        public string? TopicName { get; set; }

        public int QuestionsCount { get; set; }

        public string? AuthorName { get; set; }

        public DateTime AssignedAt { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();

        public static Dictionary<string, string> Placeholders { get; private set; } = new Dictionary<string, string> {
                { "SingleLine", "Answer should be in one line" },
                { "MultipleLine", "Answer must contain a text" },
                { "Integer", "Answer must be an integer" }
            };
    }
}
