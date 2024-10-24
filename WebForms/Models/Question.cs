namespace WebForms.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Type { get; set; } // "Text", "SingleChoice", "MultipleChoice"

        public bool IsVisible { get; set; } // Показывать в таблице заполненных форм

        public int TemplateId { get; set; }
    }
}